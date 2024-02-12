using back.Enums;
using back.ModelExport;
using back.Models;
using Microsoft.EntityFrameworkCore;

namespace back.Services.ClanWars;

internal sealed class ClanWarService: IClanWarService
{
    private WOTContext Context { get; init; }

    public ClanWarService(WOTContext _wotContext)
    {
        Context = _wotContext;
    }

    public async Task<ClanWarExport[]> ListerAsync(string _idDiscord, EEtatClanWar _eEtatClanWar = EEtatClanWar.toute)
    {
        IQueryable<ClanWar> requete = Context.ClanWars
            .OrderBy(x => x.Date)
            .Where(x => x.Date >= DateTime.Now.Date);

        requete = _eEtatClanWar switch
        {
            EEtatClanWar.participePas => requete.Where(x => x.ClanWarJoueurs.Any(y => y.IdJoueurNavigation.IdDiscord == _idDiscord) == false),
            EEtatClanWar.participe => requete.Where(x => x.ClanWarJoueurs.Any(y => y.IdJoueurNavigation.IdDiscord == _idDiscord)),
            _ => requete
        };

        var retour = await requete.Select(x => new ClanWarExport
        {
            Id = x.Id,
            Date = x.Date.ToString("d"),
            NbParticipant = (ushort)x.ClanWarJoueurs.Count(),
            Participe = x.ClanWarJoueurs
                .Where(y => y.IdJoueurNavigation.IdDiscord == _idDiscord)
                .Count() == 1
        }).ToArrayAsync();

        return retour;
    }

    public async Task<int> GetIdAsync(DateTime _date)
    {
        try
        {
            int id = await Context.ClanWars
                .Where(x => x.Date.Equals(_date))
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            return id;
        }
        catch
        {
            throw;
        }
    }

    public async Task<int> GetProchaineClanWarAsync()
    {
        try
        {
            int id = await Context.ClanWars
                .OrderBy(x => x.Date)
                .Where(x => x.Date >= DateTime.Now)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            return id;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> AjouterAsync(ClanWar _clanWar)
    {
        try
        {
            Context.ClanWars.Add(_clanWar);
            int nb = await Context.SaveChangesAsync();

            return nb is 1;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> AjouterParticipantAsync(ClanWarJoueur _clanWarJoueur)
    {
        try
        {
            Context.ClanWarJoueurs.Add(_clanWarJoueur);
            int nb = await Context.SaveChangesAsync();

            return nb is 1;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> DesinscrireAsync(int _idClanWar, int _idJoueur)
    {
        try
        {
            int nb = await Context.ClanWarJoueurs
                .Where(x => x.IdClanWar == _idClanWar && x.IdJoueur == _idJoueur)
                .ExecuteDeleteAsync();

            return nb is 1;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> SupprimerAsync(DateTime _date)
    {
        try
        {
            int nb = await Context.ClanWars
                .Where(x => x.Date == _date)
                .ExecuteDeleteAsync();

            return nb > 0;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> ParticipeDejaAsync(int _idClanWar, int _idJoueur)
    {
        try
        {
            int nb = await Context.ClanWarJoueurs
                .Where(x => x.IdClanWar == _idClanWar && x.IdJoueur == _idJoueur)
                .CountAsync();


            return nb == 1;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> ExisteAsync(DateTime _date)
    {
        int nb = await Context.ClanWars
            .Where(x => x.Date.Equals(_date))
            .CountAsync();

        return nb is 1;
    }
}
