using back.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    // genere un XML et permet de voir le sumary dans swagger pour chaque fonctions dans le controller
    string xmlNomFichier = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //swagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlNomFichier));
});

ConfigurationManager config = builder.Configuration;

// connexion a la base de donnée
builder.Services.AddDbContext<WOTContext>(o => o.UseSqlServer(config.GetConnectionString("Defaut")));

// injection de dependance
builder.Services
    .AddScoped<TankService>()
    .AddScoped<TokenService>()
    .AddScoped<ProtectionService>();

builder.Services.AddCors(options => options.AddPolicy("CORS", c => c.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()));

// genere une clé pour le JWT a partir d'une clé secrete
string cleSecrete = config.GetValue<string>("Token:cleSecrete");
SymmetricSecurityKey cle = new (Encoding.UTF8.GetBytes(cleSecrete));

// config le JWT pour la validation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            // se qu'on veut valider
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            // la valeur par defaut est 5min de validité
            // On defini la valeur mini à 0
            // ClockSkew = TimeSpan.Zero,

            // valider les données
            ValidIssuer = config.GetValue<string>("Token:issuer"),
            ValidAudience = config.GetValue<string>("Token:audience"),
            IssuerSigningKey = cle
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // creer un JSON pour swagger
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "mon API V1");

        // cache les shemas des classes en bas de page
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseCors("CORS");

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

//Scaffold-DbContext "Data Source=DESKTOP-1KR1QP3;Initial Catalog=WOT;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models