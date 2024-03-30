using back.Extensions;
using back.ModelExport;
using back.Services.Fichiers;
using Microsoft.AspNetCore.Mvc;
using Services.Protections;

namespace back.Routes;

public static class UploadPourFunRoute
{
    public static RouteGroupBuilder AjouterRouteUploadPourFun(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("Lister les fichiers uploadés")
            .Produces<List<FichierExport>>()
            .ProducesNotFound()
            .RequireAuthorization("admin");

        builder.MapPost("upload", UploaderAsync)
            .WithDescription("Upload un fichier pour le fun ! ATTENTION: passer par postman pour tester (.png, .jpg / 1Mo max)")
            .DisableAntiforgery()
            .RequireAuthorization("admin");

        return builder;
    }

    async static Task<IResult> ListerAsync()
    {
            string[] listeFichier = Directory.GetFiles("FichierPourFun");
            List<FichierExport> listerRetour = [];

            for (int i = 0; i < listeFichier.Length; i++)
            {
                string fichier = listeFichier[i];
                string base64 = Convert.ToBase64String(await File.ReadAllBytesAsync(fichier));

                listerRetour.Add(new FichierExport 
                { 
                    Nom = Path.GetFileName(fichier),
                    Base64 = base64 
                });
            }

            return listerRetour.Count > 0 ? Results.Extensions.OK(listerRetour, FichierExportContext.Default) : Results.NotFound();
    }

    async static Task<IResult> UploaderAsync([FromServices] IProtectionService _protectionServ, 
                                             [FromServices] IFichierService _fichierServ, 
                                             HttpContext _httpContext)
    {
        try
        {
            var retour = _protectionServ.Fichier(_httpContext.Request.Form.Files[0], 1_000_000, ["image/png", "image/jpg"], [".png", ".jpg"]);

            if (!retour.EstOK)
                return Results.BadRequest(retour.Message);

            await _fichierServ.SauvegarderAsync("FichierPourFun", _httpContext.Request.Form.Files[0]);

            return Results.NoContent();
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

}
