using back.Models;
using back.Routes;
using back.Services.ClanWars;
using back.Services.Fichiers;
using back.Services.Joueurs;
using back.Services.Tanks;
using certyAPI.Services.Mdp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Outil.Services;
using Services.Jwts;
using Services.Protections;
using System.Reflection;
using System.Security.Cryptography;

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

// connexion a la base de donn�e
builder.Services.AddDbContext<WotContext>(o => o.UseSqlServer(config.GetConnectionString("Defaut")));

// injection de dependance
builder.Services
    .AddScoped<ITankService, TankService>()
    .AddScoped<IJoueurService, JoueurService>()
    .AddScoped<IClanWarService, ClanWarService>()
    .AddSingleton<IJwtService>(x => new JwtService(rsa, "wotApi"))
    .AddSingleton<IProtectionService>(x => new ProtectionService(null))
    .AddTransient<IMdpService, MdpService>()
    .AddTransient<IFichierService, FichierService>()
    .AddTransient<IJwtService>(x => new JwtService(rsa, ""));

builder.Services.AddCors(options => options.AddDefaultPolicy(c => c.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()));

// genere une cl� pour le JWT a partir d'une cl� secrete
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("admin", x => x.RequireRole("admin"))
    .AddPolicy("strateur", x => x.RequireRole("admin", "strateur"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        // se qu'on veut valider ou non
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    // permet de valider le chiffrement du JWT en definissant la cl� utilis�e
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

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/joueur").AjouterRouteJoueur();
app.MapGroup("/clanWar").AjouterRouteClanWar();
app.MapGroup("/tank").AjouterRouteTank();
app.MapGroup("/upload").AjouterRouteUploadPourFun();

app.Run();

//Scaffold-DbContext "Data Source=DESKTOP-U41J905\SQLEXPRESS;Initial Catalog=WOT;Integrated Security=True;Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
// supp warning swagger => 1591 dans build g�n�ral