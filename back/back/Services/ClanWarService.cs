using back.Models;

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

    public async Task<int> AjouterAsync(ClanWar _clanWar)
    {
        await Context.ClanWars.AddAsync(_clanWar);
        await Context.SaveChangesAsync();

        return _clanWar.Id;
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

    public bool Existe(DateTime _date)
    {
        var retour = (from cw in Context.ClanWars
                     where cw.Date.Equals(_date)
                     select cw).Count();

        return retour == 1;
    }
}
