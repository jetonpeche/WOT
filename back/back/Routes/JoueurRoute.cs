using back.Enums;
using back.Extensions;
using back.ModelExport;
using back.Models;
using back.Services.Joueurs;
using certyAPI.Services.Mdp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Services.Jwts;
using Services.Protections;
using System.Security.Claims;

namespace back.Routes;

public static class JoueurRoute
{
    public static RouteGroupBuilder AjouterRouteJoueur(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister/{roleJoueur}", ListerAsync)
            .WithDescription("Lister les joueurs, valeur possible pour 'roleJoueur': admin, strateur, tous")
            .Produces<JoueurExport[]>()
            .CacheOutput("parJoueur")
            .RequireAuthorization("admin");

        builder.MapGet("listerPossedeTank/{idTank}", ListerTankPossederAsync)
            .WithDescription("Lister les joueurs qui possèdent le tank")
            .Produces<string[]>()
            .ProducesBadRequest()
            .CacheOutput("parTankJoueur")
            .RequireAuthorization();

        builder.MapGet("info/{pseudo}", InfosAsync)
            .WithDescription("Récupèrer les infos d'un utilisateur")
            .WithName("infoJoueur")
            .Produces<JoueurExport?>()
            .ProducesBadRequest()
            .ProducesNotFound()
            .RequireAuthorization();

        builder.MapPost("connexion", ConnexionAsync)
            .WithDescription("Récupèrer les infos d'un utilisateur")
            .Produces<JoueurExport>()
            .ProducesBadRequest()
            .ProducesNotFound();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un nouveau joueur")
            .ProducesCreated()
            .ProducesBadRequest()
            .RequireAuthorization("admin");

        builder.MapPost("ajouterTankJoueur", AjouterTankJoueurAsync)
            .WithDescription("Ajouter un tank à un joueur")
            .ProducesNoContent()
            .ProducesBadRequest()
            .ProducesNotFound()
            .RequireAuthorization();

        builder.MapPut("modifier", ModifierAsync)
            .WithDescription("Modifier un joueur")
            .ProducesNoContent()
            .ProducesBadRequest()
            .ProducesNotFound()
            .RequireAuthorization();            

        builder.MapPut("inserverEtatActiver/{idJoueur:int}", InserverEtatActiverAsync)
            .WithDescription("Inserse l'état du compte du joueur (exemple: activer => désactiver)")
            .ProducesNoContent()
            .ProducesBadRequest()
            .ProducesNotFound()
            .RequireAuthorization("admin");

        builder.MapPut("supprimerTankJoueur", SupprimerTankJoueurAsync)
            .WithDescription("Supprimer le tank du joueur")
            .ProducesNoContent()
            .ProducesNotFound()
            .RequireAuthorization();

        builder.MapDelete("supprimer/{idDiscord}", SupprimerAsync)
            .WithDescription("Supprimer un joueur")
            .ProducesBadRequest()
            .ProducesNoContent()
            .RequireAuthorization("admin");

        return builder;
    }

    static async Task<IResult> ListerAsync([FromServices] IJoueurService _joueurServ,
                                           [FromRoute(Name = "roleJoueur")] ERoleJoueur _roleJoueur)
    {
        try
        {
            var liste = await _joueurServ.ListerAsync(_roleJoueur);

            return Results.Extensions.OK(liste, JoueurExportContext.Default);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    static async Task<IResult> ListerTankPossederAsync([FromServices] IJoueurService _joueurServ,
                                                       [FromRoute(Name = "idTank")] int _idTank)
    {
        try
        {
            if (_idTank <= 0)
                return Results.BadRequest();

            var liste = await _joueurServ.ListerPossedeTankAsync(_idTank);

            return Results.Ok(liste);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    static async Task<IResult> InfosAsync([FromServices] IJoueurService _joueurServ,
                                          [FromRoute(Name = "pseudo")] string _pseudo)
    {
        try
        {
            if (string.IsNullOrEmpty(_pseudo))
                return Results.BadRequest();

            var retour = await _joueurServ.GetInfoAsync(_pseudo);

            return retour is null ? Results.NotFound("Tu n'existes pas") : Results.Extensions.OK(retour, JoueurExportContext.Default);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    static async Task<IResult> ConnexionAsync([FromServices] IJoueurService _joueurServ,
                                              [FromServices] IMdpService _mdpServ,
                                              [FromServices] IJwtService _jwtServ,
                                              [FromBody] ConnexionImport _connexionImport)
    {
        try
        {
            string? mdpHash = await _joueurServ.GetMdpAsync(_connexionImport.Pseudo);

            if(mdpHash is null || !_mdpServ.VerifierHash(_connexionImport.Mdp, mdpHash))
                return Results.BadRequest("Le login ou le mot de passe est faux");

            var infos = (await _joueurServ.GetInfoAsync(_connexionImport.Pseudo))!;

            if (!await _joueurServ.EstActiverAsync(_connexionImport.Pseudo))
                return Results.BadRequest("Le compte est désactivé");

            string role = "utilisateur";

            if (infos.EstAdmin)
                role = "admin";

            else if (infos.EstStrateur)
                role = "strateur";

            infos.Jwt = _jwtServ.Generer([
                new Claim(ClaimTypes.Role, role)
            ]);

            return Results.Extensions.OK(infos, JoueurExportContext.Default);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    static async Task<IResult> AjouterAsync(HttpContext _httpContext,
                                            [FromServices] IJoueurService _joueurServ,
                                            [FromServices] IProtectionService _protectionServ,
                                            [FromServices] LinkGenerator _linkGenerator,
                                            [FromServices] IMdpService _mdpServ,
                                            [FromServices] IOutputCacheStore _cache,
                                            [FromBody] JoueurImport _joueurImport)
    {
        try
        {
            if (await _joueurServ.ExisteAsync(_joueurImport.IdDiscord))
                return Results.BadRequest("id discord existe déjà");

            if (await _joueurServ.PseudoExisteAsync(_joueurImport.Pseudo))
                return Results.BadRequest($"{_joueurImport.Pseudo} existe déjà");

            Joueur joueur = new()
            {
                IdDiscord = _protectionServ.XSS(_joueurImport.IdDiscord),
                Pseudo = _protectionServ.XSS(_joueurImport.Pseudo),
                EstAdmin = _joueurImport.EstAdmin ? 1 : 0,
                EstStrateur = _joueurImport.EstStrateur ? 1 : 0,
                Mdp = _mdpServ.Hasher(_joueurImport.Mdp),
                EstActiver = 1
            };

            await _joueurServ.AjouterAsync(joueur);
            await _cache.EvictByTagAsync("joueur", default);

            string uri = _linkGenerator.GetPathByName(_httpContext, "infoJoueur", new { pseudo = joueur.Pseudo })!;

            return Results.Created(uri, new { joueur.Id });
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    static async Task<IResult> AjouterTankJoueurAsync([FromServices] IJoueurService _joueurServ,
                                                      [FromServices] IOutputCacheStore _cache,
                                                      [FromBody] JoueurTankImport _joueurTankImport)
    {
        try
        {
            int idJoueur = 0;

            if (_joueurTankImport.IdJoueur.HasValue)
            {
                idJoueur = _joueurTankImport.IdJoueur.Value;

                if (await _joueurServ.PossedeTankAsync(idJoueur, _joueurTankImport.IdTank))
                    return Results.BadRequest("Tu possèdes déjà le tank");
            }
            else
            {
                if (await _joueurServ.PossedeTankAsync(_joueurTankImport.IdDiscord!, _joueurTankImport.IdTank))
                    return Results.BadRequest("Tu possèdes déjà le tank");

                idJoueur = await _joueurServ.GetIdAsync(_joueurTankImport.IdDiscord!);
            }

            if (idJoueur == 0)
                return Results.NotFound("Je ne te connais pas");

            bool ok = await _joueurServ.AjouterTankJoueurAsync(idJoueur, _joueurTankImport.IdTank);

            if(ok)
            {
                await _cache.EvictByTagAsync("tankJoueur", default);
                return Results.NoContent();
            }

            return Results.NotFound();
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    static async Task<IResult> ModifierAsync([FromServices] IJoueurService _joueurServ,
                                             [FromServices] IOutputCacheStore _cache,
                                             [FromBody] JoueurImport _joueurImport)
    {
        try
        {
            if (_joueurImport.Id <= 0)
                return Results.BadRequest("id doit être supérieur à 0");

            bool ok = await _joueurServ.ModifierAsync(_joueurImport);

            if (ok)
            {
                await _cache.EvictByTagAsync("joueur", default);
                return Results.NoContent();
            }

            return Results.NotFound();
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    static async Task<IResult> InserverEtatActiverAsync([FromServices] IJoueurService _joueurServ,
                                                        [FromServices] IOutputCacheStore _cache,
                                                        [FromRoute(Name = "idJoueur")] int _idJoueur)
    {
        try
        {
            if (_idJoueur <= 0)
                return Results.BadRequest("id joueur doit être pus grand que 0");

            bool ok = await _joueurServ.InverserEtatActiverAsync(_idJoueur);

            if(ok)
            {
                await _cache.EvictByTagAsync("joueur", default);
                return Results.NoContent();
            }

            return Results.NotFound();
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    static async Task<IResult> SupprimerTankJoueurAsync([FromServices] IJoueurService _joueurServ,
                                                        [FromServices] IOutputCacheStore _cache,
                                                        [FromBody] JoueurTankImport _joueurTankImport)
    {
        int idJoueur = 0;

        if (_joueurTankImport.IdJoueur.HasValue)
            idJoueur = _joueurTankImport.IdJoueur.Value;
        else
            idJoueur = await _joueurServ.GetIdAsync(_joueurTankImport.IdDiscord!);

        if (idJoueur == 0)
            return Results.NotFound("Je ne te connais pas");

        bool ok = await _joueurServ.SupprimerTankJoueurAsync(idJoueur, _joueurTankImport.IdTank);

        if(ok)
        {
            await _cache.EvictByTagAsync("tankJoueur", default);
            return Results.NoContent();
        }

        return Results.NotFound();
    }

    static async Task<IResult> SupprimerAsync([FromServices] IJoueurService _joueurServ,
                                              [FromServices] IOutputCacheStore _cache,
                                              [FromRoute(Name = "idDiscord")] string _idDiscord)
    {
        try
        {
            if (!await _joueurServ.ExisteAsync(_idDiscord))
                return Results.BadRequest("id discord n'existe pas");

            await _joueurServ.SupprimerAsync(_idDiscord);

            await _cache.EvictByTagAsync("joueur", default);
            await _cache.EvictByTagAsync("tankJoueur", default);

            return Results.NoContent();
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }
}
