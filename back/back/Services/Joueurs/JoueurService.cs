using back.Enums;
using back.ModelExport;
using back.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace back.Services.Joueurs;

internal class JoueurService: IJoueurService
{
    private WotContext Context { get; init; }
    private IConfiguration Config { get; init; }

    public JoueurService(WotContext _context, IConfiguration _config)
    {
        Context = _context;
        Config = _config;
    }

    public async Task<JoueurExport[]> ListerAsync(ERoleJoueur _roleJoueur)
    {
        try
        {
            IQueryable<Joueur> requete = Context.Joueurs.OrderBy(x => x.Pseudo);

            requete = _roleJoueur switch
            {
                ERoleJoueur.admin => requete.Where(x => x.EstAdmin == 1),
                ERoleJoueur.strateur => requete.Where(x => x.EstStrateur == 1),
                _ => requete
            };

            var retour = await requete
                .Select(x => new JoueurExport
                {
                    Id = x.Id,
                    IdDiscord = x.IdDiscord,
                    Pseudo = x.Pseudo,
                    EstStrateur = x.EstStrateur == 1,
                    EstAdmin = x.EstAdmin == 1,
                    EstActiver = x.EstActiver == 1,
                    ListeIdTank = x.IdTanks.Select(y => y.Id).ToArray()
                }).ToArrayAsync();

            return retour;
        }
        catch
        {
            throw;
        }
    }

    public async Task<string[]> ListerPossedeTankAsync(int _idTank)
    {
        try
        {
            var retour = await Context.Tanks
                .Where(x => x.Id == _idTank)
                .SelectMany(x => x.IdJoueurs.Select(y => y.Pseudo))
                .ToArrayAsync();

            return retour;
        }
        catch
        {
            throw;
        }
    }

    public async Task<JoueurExport?> GetInfoAsync(string _pseudo)
    {
        try
        {
            var retour = await Context.Joueurs
                .Where(x => x.Pseudo.ToLower() == _pseudo.ToLower())
                .Select(x => new JoueurExport
                {
                    Id = x.Id,
                    IdDiscord = x.IdDiscord,
                    Pseudo = x.Pseudo,
                    EstAdmin = x.EstAdmin == 1,
                    EstStrateur = x.EstStrateur == 1,
                    EstActiver = x.EstActiver == 1,
                    ListeIdTank = x.IdTanks.Select(x => x.Id).ToArray()
                }).FirstOrDefaultAsync();

            return retour;
        }
        catch
        {
            throw;
        }
    }

    public async Task<int> GetIdAsync(string _idDiscord)
    {
        try
        {
            int id = await Context.Joueurs
                .Where(x => x.IdDiscord == _idDiscord)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            return id;
        }
        catch
        {
            throw;
        }
    }

    public async Task<string?> GetMdpAsync(string _pseudo)
    {
        try
        {
            return await Context.Joueurs.Where(x => x.Pseudo == _pseudo)
                .Select(x => x.Mdp)
                .FirstOrDefaultAsync();
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> AjouterAsync(Joueur _joueur)
    {
        try
        {
            Context.Joueurs.Add(_joueur);
            int nb = await Context.SaveChangesAsync();

            return nb is 1;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> AjouterTankJoueurAsync(int _idJoueur, int _idTank)
    {
        try
        {
            string sql = $"INSERT INTO TankJoueur (idJoueur, idTank) VALUES ({_idJoueur}, {_idTank})";
            int nb = await Context.Database.ExecuteSqlRawAsync(sql);

            return nb > 0;
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
            int nb = await Context.Joueurs
                .Where(x => x.Id == _joueurImport.Id)
                .ExecuteUpdateAsync(x => 
                    x.SetProperty(y => y.Pseudo, _joueurImport.Pseudo)
                    .SetProperty(y => y.IdDiscord, _joueurImport.IdDiscord)
                    .SetProperty(y => y.EstStrateur, _joueurImport.EstStrateur ? 1 : 0)
                    .SetProperty(y => y.EstAdmin, _joueurImport.EstAdmin ? 1 : 0)
                );

            return nb is 1;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> InverserEtatActiverAsync(int _idJoueur)
    {
        try
        {
            int nb = await Context.Joueurs
                .Where(x => x.Id == _idJoueur)
                .ExecuteUpdateAsync(x => 
                    x.SetProperty(y => y.EstActiver, z => z.EstActiver == 1 ? 0 : 1)
                );

            return nb is 1;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> SupprimerTankJoueurAsync(int _idJoueur, int _idTank)
    {
        try
        {
            string sql = $"DELETE FROM TankJoueur WHERE idJoueur = {_idJoueur} AND idTank = {_idTank}";
            int nb = await Context.Database.ExecuteSqlRawAsync(sql);

            return nb > 0;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> SupprimerAsync(string _idDiscord)
    {
        try
        {
            int nb = await Context.Joueurs
                .Where(x => x.IdDiscord == _idDiscord)
                .ExecuteDeleteAsync();
            
            return nb is 1;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> EstActiverAsync(string _pseudo)
    {
        var retour = await Context.Joueurs
            .Where(j => j.Pseudo == _pseudo)
            .Select(j => j.EstActiver)
            .FirstAsync();

        return retour == 1;
    }

    public async Task<bool> PossedeTankAsync(int _idJoueur, int _idTank)
    {
        try
        {
            return await Context.Joueurs
                .AnyAsync(x => 
                    x.Id == _idJoueur && 
                    x.IdTanks.Where(y => y.Id == _idTank).Count() == 1
                );
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> PossedeTankAsync(string _idDiscord, int _idTank)
    {
        try
        {
            return await Context.Joueurs
                .AnyAsync(x => 
                    x.IdDiscord == _idDiscord && 
                    x.IdTanks.Where(y => y.Id == _idTank).Count() == 1
                );
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> PseudoExisteAsync(string _pseudo)
    {
        return await Context.Joueurs.AnyAsync(x => x.Pseudo.ToLower() == _pseudo.ToLower());
    }

    public async Task<bool> ExisteAsync(string _idDiscord)
    {
        return await Context.Joueurs.AnyAsync(x => x.IdDiscord == _idDiscord);
    }

    public async Task<bool> ExisteAsync(int _idJoueur)
    {
        return await Context.Joueurs.AnyAsync(x => x.Id == _idJoueur);
    }
}
