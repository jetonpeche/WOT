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
                             EstActiver = x.EstActiver == 1
                         };
            });

            return retour;
        }
    }
}
