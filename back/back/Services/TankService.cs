using back.Models;

namespace back.Services;
public class TankService
{
    private WOTContext Context { get; init; }
    private string UrlImg { get; set; } = "http://localhost:5019/imgUnite";

    public TankService(WOTContext _context)
    {
        Context = _context;
    }
    public async Task<IQueryable> ListerAsync()
    {
        IQueryable? retour = null;

        await Task.Run(() =>
        {
            retour = from x in Context.Tanks
                     orderby x.IdTierNavigation.Nom, x.Nom
                     select new
                    {
                        x.Id,
                        x.Nom,
                        x.IdTypeTank,
                        UrlImage = string.Format($"{UrlImg}/{x.IdTypeTankNavigation.NomImage}"),
                        Tier = new { Id = x.IdTier, Nom = x.IdTierNavigation.Nom }
                    };
        });

        return retour;
    }

    public async Task<IQueryable> ListerAsync(int _idCompte)
    {
        IQueryable? retour = null;

        await Task.Run(() =>
        {
            retour = from u in Context.Joueurs
                    where u.Id == _idCompte
                    select u.IdTanks.OrderBy(x => x.IdTierNavigation.Nom).ThenBy(x => x.Nom).Select(x => new
                    {
                        x.Id,
                        x.Nom,
                        x.IdTypeTank,
                        UrlImage = string.Format($"{UrlImg}/{x.IdTypeTankNavigation.NomImage}"),
                        Tier = new { Id = x.IdTier, Nom = x.IdTierNavigation.Nom }
                    });
        });

        return retour;
    }

    public async Task<IQueryable> ListerAsync(string _idDiscord)
    {
        IQueryable? retour = null;

        await Task.Run(() =>
        {
            retour = from u in Context.Joueurs
                     where u.IdDiscord == _idDiscord
                     select u.IdTanks.OrderBy(x => x.IdTierNavigation.Nom).ThenBy(x => x.Nom).Select(x => new
                     {
                         x.Id,
                         x.Nom,
                         x.IdTypeTank,
                         UrlImage = string.Format($"{UrlImg}/{x.IdTypeTankNavigation.NomImage}"),
                         Tier = new { Id = x.IdTier, Nom = x.IdTierNavigation.Nom }
                     });
        });

        return retour;
    }

    public async Task<int> Ajouter(Tank _tank)
    {
        await Context.Tanks.AddAsync(_tank);

        await Context.SaveChangesAsync();

        // obj _tank est completé apres l'ajout
        // on peut recuperer son ID via _tank

        return _tank.Id;
    }
}
