using back.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace back.Services;
public class TankService
{
    private WOTContext Context { get; init; }
    private IConfiguration Config { get; init; }

    public TankService(WOTContext _context, IConfiguration _config)
    {
        Context = _context;
        Config = _config;
    }

    public async Task<IQueryable> ListerAsync(bool _seulementVisible)
    {
        IQueryable? retour = null;

        await Task.Run(() =>
        {
            if(_seulementVisible)
            {
                retour = from x in Context.Tanks
                         orderby x.IdTierNavigation.Nom, x.IdTankStatut, x.Nom
                         where x.EstVisible == 1
                         select new
                         {
                             x.Id,
                             x.Nom,
                             IdStatut = x.IdTankStatut,
                             x.IdTypeTank,
                             x.IdTier,
                             EstVisible = true
                         };
            }
            else
            {
                retour = from x in Context.Tanks
                         orderby x.IdTierNavigation.Nom, x.IdTankStatut, x.Nom
                         select new
                         {
                             x.Id,
                             x.Nom,
                             IdStatut = x.IdTankStatut,
                             x.IdTypeTank,
                             x.IdTier,
                             EstVisible = x.EstVisible == 1
                         };
            }
            
        });

        return retour;
    }

    public async Task<List<string>> ListerAsync(int _idCompte)
    {
        List<string> retour = new();

        await Task.Run(() =>
        {
            retour = Context.Joueurs
                    .Where(j => j.Id == _idCompte)
                    .SelectMany(tanks => tanks.IdTanks.Select(j => j.Nom))
                    .ToList();
        });

        return retour;
    }

    public async Task<List<dynamic>> ListerAsync(string _idDiscord, int _idTier)
    {
        using (SqlConnection sqlCon = new(Config.GetConnectionString("Defaut")))
        {
            await sqlCon.OpenAsync();

            var cmd = sqlCon.CreateCommand();

            cmd.CommandText = "SELECT t.id, t.nom, Tier.nom as nomTier, ts.nom as nomStatut, tt.nom as nomType " +
                              "FROM TankJoueur tj " +
                              "JOIN tank t ON tj.idTank = t.id " +
                              "JOIN joueur j ON j.id = tj.idJoueur " +
                              "JOIN TankStatut ts ON ts.id = t.idTankStatut " +
                              "JOIN TypeTank tt ON tt.id = t.idTypeTank " +
                              "JOIN Tier ON Tier.id = t.idTier " +
                              "WHERE idDiscord = @id AND t.idTier = @idTier AND t.estVisible = 1 " +
                              "ORDER BY tt.id, idTankStatut, t.nom";

            cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = _idDiscord;
            cmd.Parameters.Add("@idTier", SqlDbType.Int).Value = _idTier;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                List<dynamic> liste = new();

                while (reader.Read())
                {
                    liste.Add(new
                    {
                        Id = reader.GetInt32(0),
                        Nom = reader.GetString(1),
                        NomTier = reader.GetString(2),
                        NomStatut = reader.GetString(3),
                        NomType = reader.GetString(4)
                    });
                }

                sqlCon.Close();
                reader.Close();

                return liste;
            }
        }
    }

    public async Task<List<dynamic>> ListerAsync(int _idTier, int? _idType = null)
    {
        using (SqlConnection sqlCon = new(Config.GetConnectionString("Defaut")))
        {
            await sqlCon.OpenAsync();

            var cmd = sqlCon.CreateCommand();

            cmd.CommandText = "SELECT t.id, t.nom, Tier.nom as nomTier, ts.nom as nomStatut, tt.nom as nomType " +
                              "FROM tank t " +
                              "JOIN TankStatut ts ON ts.id = t.idTankStatut " +
                              "JOIN TypeTank tt ON tt.id = t.idTypeTank " +
                              "JOIN Tier ON Tier.id = t.idTier " +
                              "WHERE t.idTier = @idTier " + $"{(_idType is not null ? "AND t.idTypeTank = @idType" : "")}" + " AND t.estVisible = 1 " +
                              "ORDER BY tt.id, idTankStatut, t.nom";

            if(_idType is not null)
                cmd.Parameters.Add("@idType", SqlDbType.Int).Value = _idType.Value;
            
            cmd.Parameters.Add("@idTier", SqlDbType.Int).Value = _idTier;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                List<dynamic> liste = new();

                while (reader.Read())
                {
                    liste.Add(new
                    {
                        Id = reader.GetInt32(0),
                        Nom = reader.GetString(1),
                        NomTier = reader.GetString(2),
                        NomStatut = reader.GetString(3),
                        NomType = reader.GetString(4)
                    });
                }

                sqlCon.Close();
                reader.Close();

                return liste;
            }
        }
    }

    public async Task<int> AjouterAsync(Tank _tank)
    {
        try
        {
            await Context.Tanks.AddAsync(_tank);

            await Context.SaveChangesAsync();

            // obj _tank est completé apres l'ajout
            // on peut recuperer son ID via _tank

            return _tank.Id;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<bool> ModifierAsync(Tank _tank)
    {
        try
        {
            Context.Tanks.Update(_tank);

            await Context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }
}
