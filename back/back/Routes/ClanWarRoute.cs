using back.Enums;
using back.Extensions;
using back.ModelExport;
using back.Services.ClanWars;
using Microsoft.AspNetCore.Mvc;

namespace back.Routes;

public static class ClanWarRoute
{
    public static RouteGroupBuilder AjouterRouteClanWar(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister/{idDiscord}/{etatClanWar}", ListerAsync)
            .Produces<ClanWarExport[]>()
            ;

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
            return Results.NoContent();
        }
    }
}
