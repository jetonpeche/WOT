using back.Models;
using Microsoft.Data.SqlClient;

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

        public async Task<int> GetId(string _idDiscord)
        {
            int id = default;

            await Task.Run(() =>
            {
                id = (from j in Context.Joueurs
                     where j.IdDiscord == _idDiscord
                     select j.Id).FirstOrDefault();
            });

            return id;
        }

        public async Task<int> AjouterAsync(Joueur _joueur)
        {
            try
            {
                await Context.Joueurs.AddAsync(_joueur);
                await Context.SaveChangesAsync();

                // obj _joueur est completé apres l'ajout
                // on peut recuperer son ID via _joueur

                return _joueur.Id;
            }
            catch
            {
                return 0;
            }
        }

        public async Task AjouterTankJoueurAsync(int _idJoueur, int _idTank)
        {
            using (SqlConnection sqlCon = new(Config.GetConnectionString("defaut")))
            {
                await sqlCon.OpenAsync();

                SqlCommand cmd = sqlCon.CreateCommand();

                cmd.CommandText = "INSERT INTO TankJoueur (idJoueur, idTank) VALUES (@idJoueur, @idTank)";

                cmd.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter("@idJoueur", System.Data.SqlDbType.Int) { Value = _idJoueur },
                        new SqlParameter("@idTank", System.Data.SqlDbType.Int) { Value = _idTank }
                    });

                await cmd.PrepareAsync();
                await cmd.ExecuteNonQueryAsync();

                await sqlCon.CloseAsync();
            }
        }

        public async Task<bool> SupprimerAsync(string _idDiscord)
        {
            try
            {
                Joueur joueur = (from j in Context.Joueurs
                                 where j.IdDiscord == _idDiscord
                                 select j).First();

                Context.Joueurs.Remove(joueur);

                await Context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool PossedeTank(int _idJoueur, int _idTank)
        {
            int? nb = Context.Joueurs
                .Where(x => x.Id == _idJoueur)
                .Select(x => x.IdTanks.Where(x => x.Id == _idTank))
                .FirstOrDefault()?
                .Count();

            return nb.HasValue && nb.Value is 1;
        }

        public bool PossedeTank(string _idDiscord, int _idTank)
        {
            int? nb = Context.Joueurs
                .Where(x => x.IdDiscord == _idDiscord)
                .Select(x => x.IdTanks.Where(x => x.Id == _idTank))
                .FirstOrDefault()?
                .Count();

            return nb.HasValue && nb.Value is 1;
        }

        public bool Existe(string _idDiscord)
        {
            int nb = Context.Joueurs.Count(x => x.IdDiscord == _idDiscord);

            return nb is 1;
        }
    }
}
