using back.Extensions;
using back.ModelExport;
using back.Models;
using back.ModelsFiltre;
using back.Services.Tanks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Services.Protections;

namespace back.Routes;

public static class TankRoute
{
    public static RouteGroupBuilder AjouterRouteTank(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister/{seulementVisible:bool}", ListerAsync)
            .WithDescription("Lister les tanks")
            .Produces<TankExport[]>()
            .CacheOutput("parTank")
            .RequireAuthorization();

        builder.MapGet("lister/{idJoueur:int}", ListerTankJoueurAsync)
            .WithDescription("Lister les tanks du joueur")
            .Produces<TankExport[]>()
            .ProducesBadRequest()
            .RequireAuthorization();

        builder.MapGet("listerViaDiscord", ListerViaDiscordAsync)
            .Produces<Tank2Export[]>()
            .ProducesBadRequest();

        builder.MapGet("listerTankJoueurViaDiscord/{idDiscord}/{idTier:int}", ListerTankJoueurViaDiscordAsync)
            .Produces<Tank2Export[]>()
            .ProducesBadRequest(); 

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un nouveau tank")
            .ProducesCreated()
            .RequireAuthorization("admin");

        builder.MapPut("modifier", ModifierAsync)
            .WithDescription("Modifier un tank")
            .ProducesNoContent()
            .ProducesBadRequest()
            .ProducesNotFound()
            .RequireAuthorization("admin");

        builder.MapDelete("supprimer/{idTank:int}", SupprimerAsync)
            .WithDescription("Permet de supprimer un tank")
            .ProducesNoContent()
            .ProducesBadRequest()
            .ProducesNotFound()
            .RequireAuthorization("admin");

        return builder;
    }

    async static Task<IResult> ListerAsync([FromServices] ITankService _tankServ,
                                           [FromRoute(Name = "seulementVisible")] bool _seulementVisible)
    {
        try
        {
            var liste = await _tankServ.ListerAsync(_seulementVisible);

            return Results.Extensions.OK(liste, TankExportContext.Default);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> ListerTankJoueurAsync([FromServices] ITankService _tankServ,
                                                     [FromRoute(Name = "idJoueur")] int _idJoueur)
    {
        try
        {
            if (_idJoueur <= 0)
                return Results.BadRequest("id joueur doit être supérieur à 0");

            var liste = await _tankServ.ListerAsync(_idJoueur);

            return Results.Extensions.OK(liste, TankExportContext.Default);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> ListerViaDiscordAsync([FromServices] ITankService _tankServ,
                                                     [AsParameters] FiltreTierTypeTank _filtre)
    {
        try
        {
            if (_filtre.IdTier <= 0)
                return Results.BadRequest("id tier doit être superieur à 0");

            if(_filtre.IdType.HasValue && _filtre.IdType.Value <= 0)
                return Results.BadRequest("id type doit être superieur à 0");

            var liste = await _tankServ.ListerAsync(_filtre.IdTier, _filtre.IdType ?? 0);

            return Results.Extensions.OK(liste, Tank2ExportContext.Default);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> ListerTankJoueurViaDiscordAsync([FromServices] ITankService _tankServ,
                                                               [FromRoute(Name = "idDiscord")] string _idDiscord,
                                                               [FromRoute(Name = "idTier")] int _idTier)
    {
        try
        {
            if (_idTier <= 0)
                return Results.BadRequest("id tier doit être supérieur à 0");

            var liste = await _tankServ.ListerAsync(_idDiscord, _idTier);

            return Results.Extensions.OK(liste, Tank2ExportContext.Default);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> AjouterAsync([FromServices] ITankService _tankServ,
                                            [FromServices] IProtectionService _protectionServ,
                                            [FromServices] IOutputCacheStore _cache,
                                            [FromBody] TankImport _tankImport)
    {
        try
        {
            Tank tank = new()
            {
                Nom = _protectionServ.XSS(_tankImport.Nom),
                IdTier = _tankImport.IdTier,
                IdTankStatut = _tankImport.IdStatut,
                IdTypeTank = _tankImport.IdType,
                EstVisible = 1
            };

            await _tankServ.AjouterAsync(tank);
            await _cache.EvictByTagAsync("tank", default);

            return Results.Created("", new { tank.Id });
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> ModifierAsync([FromServices] ITankService _tankServ,
                                             [FromServices] IProtectionService _protectionServ,
                                             [FromServices] IOutputCacheStore _cache,
                                             [FromBody] TankModifierImport _tankImport)
    {
        try
        {
            if (_tankImport.Id <= 0)
                return Results.BadRequest("id tank doit être superieur à 0");

            Tank tank = new()
            {
                Id = _tankImport.Id,
                Nom = _protectionServ.XSS(_tankImport.Nom),
                IdTier = _tankImport.IdTier,
                IdTankStatut = _tankImport.IdStatut,
                IdTypeTank = _tankImport.IdType,
                EstVisible = 1
            };

            bool ok = await _tankServ.ModifierAsync(tank);

            if(ok)
            {
                await _cache.EvictByTagAsync("tank", default);
                return Results.NoContent();
            }

            return Results.NotFound();
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> SupprimerAsync([FromServices] ITankService _tankServ,
                                              [FromServices] IOutputCacheStore _cache,
                                              [FromRoute(Name = "idTank")] int _idTank)
    {
        try
        {
            if (_idTank <= 0)
                return Results.BadRequest("id tank doit être superieur à 0");

            bool ok = await _tankServ.SupprimerAsync(_idTank);

            if(ok)
            {
                await _cache.EvictByTagAsync("tank", default);
                return Results.NoContent();
            }

            return Results.NotFound();
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }
}
