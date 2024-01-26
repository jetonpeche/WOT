using back.ModelExport;
using back.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace back.Services.Tanks;

internal sealed class TankService: ITankService
{
    private WOTContext Context { get; init; }

    public TankService(WOTContext _context)
    {
        Context = _context;
    }

    public async Task<TankExport[]> ListerAsync(bool _seulementVisible)
    {
        try
        {
            IQueryable<Tank> requete = Context.Tanks
                .OrderBy(x => x.IdTierNavigation.Nom)
                .OrderBy(x => x.IdTankStatut)
                .OrderBy(x => x.Nom);

            if (_seulementVisible)
                requete = requete.Where(x => x.EstVisible == 1);

            var retour = await requete.Select(x => new TankExport
            {
                Id = x.Id,
                Nom = x.Nom,
                IdStatut = x.IdTankStatut,
                IdTypeTank = x.IdTypeTank,
                IdTier = x.IdTier,
                EstVisible = x.EstVisible == 1,
                NbPossesseur = (ushort)x.IdJoueurs.Count()
            }).ToArrayAsync();

            return retour;
        }
        catch
        {
            throw;
        }
    }

    public async Task<string[]> ListerNomAsync(int _idJoueur)
    {
        try
        {
            var retour = await Context.Joueurs
                .Where(j => j.Id == _idJoueur)
                .SelectMany(tanks => tanks.IdTanks.Select(j => j.Nom))
                .ToArrayAsync();

            return retour;
        }
        catch
        {
            throw;
        }
    }

    public async Task<Tank2Export[]> ListerAsync(string _idDiscord, int _idTier)
    {
        var retour = await Context.Tanks
            .Where(x => x.IdTier == _idTier && x.IdJoueurs.Any(y => y.IdDiscord == _idDiscord))
            .OrderBy(x => x.IdTypeTank)
            .OrderBy(x => x.IdTankStatut)
            .OrderBy(x => x.Nom)
            .Select(x => new Tank2Export
            {
                Id = x.Id,
                Nom = x.Nom,
                NomTier = x.IdTierNavigation.Nom,
                NomStatut = x.IdTankStatutNavigation.Nom,
                NomType = x.IdTypeTankNavigation.Nom
            }).ToArrayAsync();

        return retour;
    }

    public async Task<Tank2Export[]> ListerAsync(int _idTier, int _idType = 0)
    {
        IQueryable<Tank> requete = Context.Tanks
            .OrderBy(x => x.IdTypeTank)
            .OrderBy(x => x.IdTankStatut)
            .OrderBy(x => x.Nom)
            .Where(x => x.IdTier == _idTier);

        if(_idType > 0)
            requete = requete.Where(x => x.IdTypeTank == _idType);

        var retour = await requete
            .Select(x => new Tank2Export
            {
                Id = x.Id,
                Nom = x.Nom,
                NomTier = x.IdTierNavigation.Nom,
                NomStatut = x.IdTankStatutNavigation.Nom,
                NomType = x.IdTypeTankNavigation.Nom
            }).ToArrayAsync();

        return retour;
    }

    public async Task<bool> AjouterAsync(Tank _tank)
    {
        try
        {
            Context.Tanks.Add(_tank);
            int nb = await Context.SaveChangesAsync();

            return nb is 1;
        }
        catch
        {
            throw;
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

    public async Task<bool> SupprimerAsync(int _idTank)
    {
        try
        {
            string sql = $"DELETE FROM TankJoueur WHERE idTank = {_idTank}" +
                         $"DELETE FROM ClanWarJoueur WHERE idTank = {_idTank};" +
                         $"DELETE FROM Tank WHERE id = {_idTank};";

            int nb = await Context.Database.ExecuteSqlRawAsync(sql);

            return nb > 0;
        }
        catch
        {
            throw;
        }
    }
}
