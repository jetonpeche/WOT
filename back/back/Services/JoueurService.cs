using back.ModelExport;
using back.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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

        public async Task<List<JoueurExport>> ListerAsync()
        {
            List<JoueurExport> retour = Array.Empty<JoueurExport>().ToList();

            await Task.Run(() =>
            {
                retour = (from x in Context.Joueurs
                         orderby x.Pseudo
                         select new JoueurExport
                         {
                             Id = x.Id,
                             IdDiscord = x.IdDiscord,
                             Pseudo = x.Pseudo,
                             EstStrateur = x.EstStrateur == 1,
                             EstAdmin = x.EstAdmin == 1,
                             EstActiver = x.EstActiver == 1,
                             ListeIdTank = x.IdTanks.Select(y => y.Id).ToList()
                         }).ToList();
            });

            return retour;
        }

        public async Task<List<JoueurExport>> ListerStrateurAsync()
        {
            List<JoueurExport> retour = Array.Empty<JoueurExport>().ToList();

            await Task.Run(() =>
            {
                retour = (from x in Context.Joueurs
                         orderby x.Pseudo
                         where x.EstStrateur == 1
                         select new JoueurExport
                         {
                             Id = x.Id,
                             IdDiscord = x.IdDiscord,
                             Pseudo = x.Pseudo,
                             EstStrateur = true,
                             EstAdmin = x.EstAdmin == 1,
                             EstActiver = x.EstActiver == 1,
                             ListeIdTank = x.IdTanks.Select(y => y.Id).ToList()
                         }).ToList();
            });

            return retour;
        }

        public async Task<List<JoueurExport>> ListerAdminAsync()
        {
            List<JoueurExport> retour = Array.Empty<JoueurExport>().ToList();

            await Task.Run(() =>
            {
                retour = (from x in Context.Joueurs
                         orderby x.Pseudo
                         where x.EstAdmin == 1
                         select new JoueurExport
                         {
                             Id = x.Id,
                             IdDiscord = x.IdDiscord,
                             Pseudo = x.Pseudo,
                             EstStrateur = x.EstStrateur == 1,
                             EstAdmin = true,
                             EstActiver = x.EstActiver == 1,
                             ListeIdTank = x.IdTanks.Select(y => y.Id).ToList()
                         }).ToList();
            });

            return retour;
        }

        public async Task<List<string>> ListerPossedeTankAsync(int _idTank)
        {
            List<string> retour = new();

            await Task.Run(() =>
            {
                retour = Context.Tanks
                        .Where(t => t.Id == _idTank)
                        .SelectMany(tanks => tanks.IdJoueurs.Select(j => j.Pseudo))
                        .ToList();
            });

            return retour;
        }

        public async Task<JoueurExport?> GetInfoAsync(string _pseudo)
        {
            JoueurExport? retour = null;

            await Task.Run(() =>
            {
                if(PseudoExiste(_pseudo))
                {
                    retour = Context.Joueurs
                    .Where(x => x.Pseudo.ToLower() == _pseudo.ToLower())
                    .Select(j => new JoueurExport()
                    {
                        Id = j.Id,
                        IdDiscord = j.IdDiscord,
                        Pseudo = j.Pseudo,
                        EstAdmin = j.EstAdmin == 1,
                        EstStrateur = j.EstStrateur == 1,
                        ListeIdTank = j.IdTanks.Select(x => x.Id).ToList()
                    }).First();
                }
            });

            return retour;
        }

        public async Task<int> GetIdAsync(string _idDiscord)
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

        public async Task<bool> AjouterTankJoueurAsync(int _idJoueur, int _idTank)
        {
            try
            {
                using (SqlConnection sqlCon = new(Config.GetConnectionString("defaut")))
                {
                    await sqlCon.OpenAsync();

                    SqlCommand cmd = sqlCon.CreateCommand();

                    cmd.CommandText = "INSERT INTO TankJoueur (idJoueur, idTank) VALUES (@idJoueur, @idTank)";

                    cmd.Parameters.AddRange(new SqlParameter[]
                        {
                        new SqlParameter("@idJoueur", SqlDbType.Int) { Value = _idJoueur },
                        new SqlParameter("@idTank", SqlDbType.Int) { Value = _idTank }
                        });

                    await cmd.PrepareAsync();
                    await cmd.ExecuteNonQueryAsync();

                    await sqlCon.CloseAsync();

                    return true;
                }
            }
            catch
            {
                return false;
            }
            
        }

        public async Task<bool> ModifierAsync(JoueurImport _joueurImport)
        {
            try
            {
                using (SqlConnection sqlCon = new(Config.GetConnectionString("defaut")))
                {
                    await sqlCon.OpenAsync();

                    SqlCommand cmd = sqlCon.CreateCommand();

                    cmd.CommandText = "UPDATE joueur " +
                        "SET " +
                            "pseudo = @pseudo, " +
                            "idDiscord = @idDiscord," +
                            "estStrateur = @strateur," +
                            "estAdmin = @estAdmin " +
                        "WHERE id = @idJoueur";

                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter("@idJoueur", SqlDbType.Int) { Value = _joueurImport.Id },
                        new SqlParameter("@pseudo", SqlDbType.VarChar, 200) { Value = _joueurImport.Pseudo },
                        new SqlParameter("@idDiscord", SqlDbType.VarChar, 100) { Value = _joueurImport.IdDiscord },
                        new SqlParameter("@strateur", SqlDbType.Int) { Value = _joueurImport.EstStrateur },
                        new SqlParameter("@estAdmin", SqlDbType.Int) { Value = _joueurImport.EstAdmin }
                    });

                    await cmd.PrepareAsync();
                    await cmd.ExecuteNonQueryAsync();

                    await sqlCon.CloseAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Activer(int _idJoueur)
        {
            try
            {
                using (SqlConnection sqlCon = new(Config.GetConnectionString("Defaut")))
                {
                    await sqlCon.OpenAsync();

                    SqlCommand cmd = sqlCon.CreateCommand();

                    cmd.CommandText = "UPDATE Joueur SET estActiver = 1 WHERE id = @idJoueur";

                    cmd.Parameters.Add("@idJoueur", SqlDbType.Int).Value = _idJoueur;

                    await cmd.PrepareAsync();
                    await cmd.ExecuteNonQueryAsync();

                    await sqlCon.CloseAsync();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Desactiver(int _idJoueur)
        {
            try
            {
                using (SqlConnection sqlCon = new(Config.GetConnectionString("Defaut")))
                {
                    await sqlCon.OpenAsync();

                    SqlCommand cmd = sqlCon.CreateCommand();

                    cmd.CommandText = "UPDATE Joueur SET estActiver = 0 WHERE id = @idJoueur";

                    cmd.Parameters.Add("@idJoueur", SqlDbType.Int).Value = _idJoueur;

                    await cmd.PrepareAsync();
                    await cmd.ExecuteNonQueryAsync();

                    await sqlCon.CloseAsync();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SupprimerTankJoueurAsync(int _idJoueur, int _idTank)
        {
            try
            {
                using (SqlConnection sqlCon = new(Config.GetConnectionString("defaut")))
                {
                    await sqlCon.OpenAsync();

                    SqlCommand cmd = sqlCon.CreateCommand();

                    cmd.CommandText = "DELETE FROM TankJoueur WHERE idJoueur = @idJoueur AND idTank = @idTank";

                    cmd.Parameters.AddRange(new SqlParameter[]
                        {
                        new SqlParameter("@idJoueur", SqlDbType.Int) { Value = _idJoueur },
                        new SqlParameter("@idTank", SqlDbType.Int) { Value = _idTank }
                        });

                    await cmd.PrepareAsync();
                    await cmd.ExecuteNonQueryAsync();

                    await sqlCon.CloseAsync();

                    return true;
                }
            }
            catch
            {
                return false;
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

        public bool EstActiver(string _pseudo)
        {
            var retour = Context.Joueurs
                .Where(j => j.Pseudo == _pseudo)
                .Select(j => j.EstActiver).First();

            return retour == 1;
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

        public bool PseudoExiste(string _pseudo)
        {
            int nb = Context.Joueurs.Count(x => x.Pseudo.ToLower() == _pseudo.ToLower());

            return nb is 1;
        }

        public bool Existe(string _idDiscord)
        {
            int nb = Context.Joueurs.Count(x => x.IdDiscord == _idDiscord);

            return nb is 1;
        }

        public bool Existe(int _idJoueur)
        {
            int nb = Context.Joueurs.Count(x => x.Id == _idJoueur);

            return nb is 1;
        }
    }
}
