using back.Models;

namespace back.Services
{
    public class JoueurService
    {
        private WOTContext Context { get; init; }
        private IConfiguration Config { get; init; }

        public JoueurService(WOTContext _context, IConfiguration _config)
        {
            Context = _context;
            Config = _config;
        }

        public async Task<IQueryable> ListerAsync()
        {
            IQueryable? retour = null;

            await Task.Run(() =>
            {
                retour = from x in Context.Joueurs
                         orderby x.Pseudo
                         select new
                         {
                             x.Id,
                             x.IdDiscord,
                             x.Pseudo,
                             EstStrateur = x.EstStrateur == 1,
                             EstAdmin = x.EstAdmin == 1,
                             EstActiver = x.EstActiver == 1,
                             ListeIdTank = x.IdTanks.Select(y => y.Id).ToList()
                         };
            });

            return retour;
        }

        public async Task<IQueryable> ListerStrateurAsync()
        {
            IQueryable? retour = null;

            await Task.Run(() =>
            {
                retour = from x in Context.Joueurs
                         orderby x.Pseudo
                         where x.EstStrateur == 1
                         select new
                         {
                             x.Id,
                             x.IdDiscord,
                             x.Pseudo,
                             EstStrateur = true,
                             EstAdmin = x.EstAdmin == 1,
                             EstActiver = x.EstActiver == 1,
                             ListeIdTank = x.IdTanks.Select(y => y.Id).ToList()
                         };
            });

            return retour;
        }

        public async Task<IQueryable> ListerAdminAsync()
        {
            IQueryable? retour = null;

            await Task.Run(() =>
            {
                retour = from x in Context.Joueurs
                         orderby x.Pseudo
                         where x.EstAdmin == 1
                         select new
                         {
                             x.Id,
                             x.IdDiscord,
                             x.Pseudo,
                             EstStrateur = x.EstStrateur == 1,
                             EstAdmin = true,
                             EstActiver = x.EstActiver == 1,
                             ListeIdTank = x.IdTanks.Select(y => y.Id).ToList()
                         };
            });

            return retour;
        }

        public async Task<int> AjouterAsync(Joueur _joueur)
        {
            try
            {
                await Context.Joueurs.AddAsync(_joueur);
                await Context.SaveChangesAsync();

                return _joueur.Id;
            }
            catch
            {
                return 0;
            }
        }
    }
}
