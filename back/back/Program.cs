using back.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.OpenApi.Models;
using back.Services.ClanWars;
using back.Routes;
using back.Services.Joueurs;
using back.Services.Tanks;

var builder = WebApplication.CreateBuilder(args);

string cheminCleRsa = builder.Configuration.GetValue<string>("cheminCleRsa")!;

RSA rsa = RSA.Create();

// creer la cle une seule fois
if (!File.Exists(cheminCleRsa))
{
    // cree un fichier bin pour signer le JWT
    var clePriver = rsa.ExportRSAPrivateKey();
    File.WriteAllBytes(cheminCleRsa, clePriver);
}

// recupere la cl?
rsa.ImportRSAPrivateKey(File.ReadAllBytes(cheminCleRsa), out _);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    // genere un XML et permet de voir le sumary dans swagger pour chaque fonctions dans le controller
    string xmlNomFichier = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    swagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlNomFichier));

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // ou le trouver
        In = ParameterLocation.Header,

        // description
        Description = "Token",

        // nom dans le header
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",

        // JWT de type Bearer
        Scheme = "Bearer"
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },

            new string[]{}
        }
    });
});

ConfigurationManager config = builder.Configuration;

// connexion a la base de donnée
builder.Services.AddDbContext<WOTContext>(o => o.UseSqlServer(config.GetConnectionString("Defaut")));

// injection de dependance
builder.Services
    .AddScoped<ITankService, TankService>()
    .AddScoped<IJoueurService, JoueurService>()
    .AddScoped<IClanWarService, ClanWarService>()
    .AddSingleton(x => new TokenService(rsa, "wotApp"))
    .AddSingleton<ProtectionService>();

builder.Services.AddCors(options => options.AddDefaultPolicy(c => c.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()));

// genere une clé pour le JWT a partir d'une clé secrete
builder.Services.AddAuthorizationBuilder();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        // se qu'on veut valider ou non
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    // permet de valider le chiffrement du JWT en definissant la clé utilisée
                    option.Configuration = new OpenIdConnectConfiguration
                    {
                        SigningKeys = { new RsaSecurityKey(rsa) }
                    };

                    // pour avoir les cl? valeur normal comme dans les claims
                    // par defaut ajouter des Uri pour certain truc comme le "sub"
                    option.MapInboundClaims = false;
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.DefaultModelsExpandDepth(-1));
}

app.UseCors();

app.UseAuthorization();
app.UseAuthentication();

app.MapGroup("/joueur").AjouterRouteJoueur();
app.MapGroup("/clanWar").AjouterRouteClanWar();

app.Run();

//Scaffold-DbContext "Data Source=DESKTOP-U41J905\SQLEXPRESS;Initial Catalog=WOT;Integrated Security=True;Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
// supp warning swagger => 1591 dans build général
/*
 * pour le summary dans swagger (fichier du projet)
  <PropertyGroup>
	<GenerateDocumentationFile>true</GenerateDocumentationFile>
	</PropertyGroup>
 */