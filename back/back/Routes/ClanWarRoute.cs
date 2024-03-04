using back.Enums;
using back.Extensions;
using back.ModelExport;
using back.Models;
using back.Services.ClanWars;
using back.Services.Joueurs;
using Microsoft.AspNetCore.Mvc;

namespace back.Routes;

public static class ClanWarRoute
{
    public static RouteGroupBuilder AjouterRouteClanWar(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister/{idDiscord}/{etatClanWar}", ListerAsync)
            .WithDescription("Lister les clan war avec la participant de la personne qui possede l'id discord")
            .Produces<ClanWarExport[]>();

        builder.MapGet("detail/{idClanWar}", RecupererDetailAsync)
            .WithDescription("Permet de recupérer les details d'une clan war")
            .Produces<ClanWarDetailExport>()
            .ProducesBadRequest()
            .ProducesNotFound();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter une nouvelle clan war")
            .ProducesCreated();

        builder.MapPost("participer", ParticiperAsync)
            .WithDescription("Permet de s'inscrire à la prochaine clan war (ne pas mettre de date) ou une clan war spécifique (avec la date)")
            .ProducesBadRequest()
            .ProducesNotFound()
            .ProducesNoContent();

        builder.MapDelete("desinscrire", DesinscrireAsync)
            .WithDescription("Permet de se désinscrire à la prochaine clan war (ne pas mettre de date) ou une clan war spécifique (avec la date)")
            .ProducesBadRequest()
            .ProducesNotFound()
            .ProducesNoContent();

        builder.MapDelete("supprimer", SupprimerAsync)
            .WithDescription("Permet de supprimer une clan war")
            .ProducesNotFound()
            .ProducesNoContent();

        return builder;
    }

    async static Task<IResult> ListerAsync([FromServices] IClanWarService _clanWarServ,
                                           [FromRoute(Name = "idDiscord")] string _idDiscord,
                                           [FromRoute(Name = "etatClanWar")] EEtatClanWar _etatClanWar)
    {
        try
        {
            var retour = await _clanWarServ.ListerAsync(_idDiscord, _etatClanWar);

            return Results.Extensions.OK(retour, ClanWarExportContext.Default);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> RecupererDetailAsync([FromServices] IClanWarService _clanWarServ,
                                                    [FromRoute(Name = "idClanWar")] int _idClanWar)
    {
        try
        {
            if (_idClanWar <= 0)
                return Results.BadRequest("'idClanWar' doit être supérieur à 0");

            var retour = await _clanWarServ.GetDetailAsync(_idClanWar);

            return retour is null ? Results.NotFound("La clan war n'éxiste pas") : Results.Extensions.OK(retour, ClanWarDetailExportContext.Default);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> AjouterAsync(HttpContext _httpContext,
                                            [FromServices] IJoueurService _joueurServ,
                                            [FromServices] LinkGenerator _linkGenerator,
                                            [FromServices] IClanWarService _clanWarServ,
                                            [FromBody] ClanWarImport _clanWarImport)
    {
        try
        {
            if (!await _joueurServ.ExisteAsync(_clanWarImport.IdDiscord))
                return Results.NotFound("Je ne te connais pas");

            if (await _clanWarServ.ExisteAsync(_clanWarImport.Date))
                return Results.BadRequest("Une clan war éxiste déjà à cette date");

            ClanWar clanWar = new()
            {
                Date = _clanWarImport.Date
            };

            await _clanWarServ.AjouterAsync(clanWar);

            return Results.Created("", new { clanWar.Id });
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> ParticiperAsync([FromServices] IJoueurService _joueurServ,
                                               [FromServices] IClanWarService _clanWarServ,
                                               [FromBody] ParticipantClanWarImport _participantClanWarImport)
    {
        try
        {
            if (!await _joueurServ.ExisteAsync(_participantClanWarImport.IdDiscord))
                return Results.NotFound("Je ne te connais pas, participation impossible");

            int idJoueur = await _joueurServ.GetIdAsync(_participantClanWarImport.IdDiscord);

            if (DateTime.TryParse(_participantClanWarImport.Date, out DateTime date))
            {
                if (!await _clanWarServ.ExisteAsync(date))
                    return Results.NotFound($"La clan war du {date.ToString("d")} n'existe pas");

                int idClanWar = await _clanWarServ.GetIdAsync(date);

                if (await _clanWarServ.ParticipeDejaAsync(idClanWar, idJoueur))
                    return Results.BadRequest("Tu participes déjà à cette clan war");

                ClanWarJoueur clanWarJoueur = new()
                {
                    IdJoueur = idJoueur,
                    IdClanWar = idClanWar,
                    IdTank = null
                };

                await _clanWarServ.AjouterParticipantAsync(clanWarJoueur);

                return Results.NoContent();
            }
            // inscription a la prochaine clan war
            else
            {
                int idClanWar = await _clanWarServ.GetIdProchaineClanWarAsync();

                if (idClanWar is 0)
                    return Results.NotFound("Aucune clan war prochainement");

                if (await _clanWarServ.ParticipeDejaAsync(idClanWar, idJoueur))
                    return Results.BadRequest("Tu participes déjà à la prochaine clan war");

                ClanWarJoueur clanWarJoueur = new()
                {
                    IdJoueur = idJoueur,
                    IdClanWar = idClanWar,
                    IdTank = null
                };

                await _clanWarServ.AjouterParticipantAsync(clanWarJoueur);

                return Results.NoContent();
            }
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> DesinscrireAsync([FromServices] IJoueurService _joueurServ,
                                                [FromServices] IClanWarService _clanWarServ,
                                                [FromBody] ParticipantClanWarImport _participantClanWarImport)
    {
        try
        {
            if (!await _joueurServ.ExisteAsync(_participantClanWarImport.IdDiscord))
                return Results.NotFound("Je ne te connais pas, désinscription impossible");

            int idJoueur = await _joueurServ.GetIdAsync(_participantClanWarImport.IdDiscord);
            int idClanWar = 0;
            bool ok = false;

            if (DateTime.TryParse(_participantClanWarImport.Date, out DateTime date))
            {
                if (!await _clanWarServ.ExisteAsync(date))
                    return Results.NotFound($"La clan war du {date.ToString("d")} n'existe pas");

                idClanWar = await _clanWarServ.GetIdAsync(date);
            }
            else
            {
                idClanWar = await _clanWarServ.GetIdProchaineClanWarAsync();

                if (idClanWar is 0)
                    return Results.NotFound("Aucune clan n'existe prochainement");
            }

            ok = await _clanWarServ.DesinscrireAsync(idClanWar, idJoueur);

            return ok ? Results.NoContent() : Results.BadRequest($"Tu ne participes pas à cette clan war");
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> SupprimerAsync([FromServices] IJoueurService _joueurServ,
                                              [FromServices] IClanWarService _clanWarServ,
                                              [FromBody] ClanWarImport _clanWarImport)
    {
        try
        {
            if (!await _joueurServ.ExisteAsync(_clanWarImport.IdDiscord))
                return Results.NotFound("Je ne te connais pas");

            bool ok = await _clanWarServ.SupprimerAsync(_clanWarImport.Date);

            return ok ? Results.NoContent() : Results.NotFound("La clan war n'existe pas");
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }
}
