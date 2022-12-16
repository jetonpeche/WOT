using back.Models;
using Microsoft.Data.SqlClient;

namespace back.Services;

public class ClanWarService
{
    private WOTContext Context { get; init; }

    public ClanWarService(WOTContext _wotContext)
    {
        Context = _wotContext;
    }

    public async Task<IQueryable> ListerAsync(string _idDiscord)
    {
        IQueryable? retour = null;

        await Task.Run(() =>
        {
            retour = from cw in Context.ClanWars
                     where cw.Date >= DateTime.Now
                     orderby cw.Date
                     select new
                     {
                         Date = cw.Date.ToString("d"),
                         Participe = cw.ClanWarJoueurs.Where(cwj => cwj.IdJoueurNavigation.IdDiscord == _idDiscord).Count() == 1,
                         NbParticipant = cw.ClanWarJoueurs.Count()
                     };
        });

        return retour;
    }

    public async Task<int> GetIdAsync(DateTime _date)
    {
        try
        {
            int id = 0;

            await Task.Run(() =>
            {
                id = Context.ClanWars.First(x => x.Date.Equals(_date)).Id;
            });

            return id;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<int?> GetProchaineClanWarAsync()
    {
        int? id = null;

        try
        {
            await Task.Run(() =>
            {
                id = Context.ClanWars.OrderBy(x => x.Date).Where(x => x.Date >= DateTime.Now).FirstOrDefault()?.Id;
            });

            return id.GetValueOrDefault();
        }
        catch
        {
            return null;
        }     
    }

    public async Task<int> AjouterAsync(ClanWar _clanWar)
    {
        await Context.ClanWars.AddAsync(_clanWar);
        await Context.SaveChangesAsync();

        return _clanWar.Id;
    }

    public async Task<bool> AjouterParticipantAsync(ClanWarJoueur _clanWarJoueur)
    {
        try
        {
            await Context.ClanWarJoueurs.AddAsync(_clanWarJoueur);
            await Context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DesinscrireAsync(int _idClanWar, int _idJoueur)
    {
        try
        {
            ClanWarJoueur clanWarJoueur = Context.ClanWarJoueurs.Where(x => x.IdClanWar == _idClanWar && x.IdJoueur == _idJoueur).First();

            Context.ClanWarJoueurs.Remove(clanWarJoueur);
            await Context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<int> SupprimerAsync(DateTime _date)
    {
        try
        {
            ClanWar clanWar = (from cw in Context.ClanWars
                               where cw.Date == _date
                               select cw).First();

            Context.ClanWars.Remove(clanWar);
            await Context.SaveChangesAsync();

            return 1;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<bool> ParticipeDejaAsync(int _idClanWar, int _idJoueur)
    {
        try
        {
            int nb = default;

            await Task.Run(() =>
            {
                nb = Context.ClanWarJoueurs.Where(x => x.IdClanWar == _idClanWar && x.IdJoueur == _idJoueur).Count();
            });

            return nb == 1;
        }
        catch
        {
            return false;
        }
    }

    public bool Existe(DateTime _date)
    {
        var retour = (from cw in Context.ClanWars
                     where cw.Date.Equals(_date)
                     select cw).Count();

        return retour == 1;
    }
}
