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
                        x.IdTypeTankNavigation.NomImage,
                        Tier = new { Id = x.IdTier, Nom = x.IdTierNavigation.Nom }
                    };
        });

        return retour;
    }

    public async Task<List<dynamic>> ListerAsync(int _idCompte)
    {
        using (SqlConnection sqlCon = new(Config.GetConnectionString("Defaut")))
        {
            await sqlCon.OpenAsync();

            var cmd = sqlCon.CreateCommand();

            cmd.CommandText = "SELECT t.id, t.nom, ts.nom as nomStatut, t.idTankStatut, idTypeTank, idTier, nomImage " +
                              "FROM TankJoueur tj " +
                              "JOIN tank t ON tj.idTank = t.id " +
                              "JOIN TankStatut ts ON ts.id = t.idTankStatut " +
                              "JOIN TypeTank tt ON tt.id = t.idTypeTank " +
                              "WHERE idJoueur = @id";

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = _idCompte;

            await cmd.PrepareAsync();

            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                List<dynamic> liste = new(); 

                while (reader.Read())
                {
                    liste.Add(new 
                    { 
                        Id = reader.GetValue(0),
                        Nom = reader.GetString(1),
                        IdType = reader.GetValue(4),
                        NomImage = reader.GetString(6),
                        IdTier = reader.GetInt32(5),
                        IdStatut = reader.GetInt32(3),
                        NomStatut = reader.GetString(2)
                    });
                }

                sqlCon.Close();
                reader.Close();

                return liste;
            }
        }
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
                              "ORDER BY idTankStatut, tt.id, t.nom";

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
}
