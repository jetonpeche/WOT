using back.ModelExport;
using back.Models;

namespace back.Services.Tanks;

public interface ITankService
{
    /// <summary>
    /// Lister les tanks
    /// </summary>
    /// <param name="_seulementVisible">si false prends tout</param>
    /// <returns>liste des tanks</returns>
    Task<TankExport[]> ListerAsync(bool _seulementVisible);

    Task<TankExport[]> ListerAsync(int _idJoueur);

    /// <summary>
    /// Lister les noms des tanks du joueur
    /// </summary>
    /// <param name="_idJoueur">id joueur conserné</param>
    /// <returns></returns>
    Task<string[]> ListerNomAsync(int _idJoueur);

    /// <summary>
    /// Lister les tanks du joueur
    /// </summary>
    /// <param name="_idDiscord">id discord conserné</param>
    /// <param name="_idTier">id tier conserné</param>
    /// <returns>Liste des tanks</returns>
    Task<Tank2Export[]> ListerAsync(string _idDiscord, int _idTier);

    /// <summary>
    /// Lister les tanks 
    /// </summary>
    /// <param name="_idTier">filtre par id tier</param>
    /// <param name="_idType">filtre par id type (option)</param>
    /// <returns>liste des tanks</returns>
    Task<Tank2Export[]> ListerAsync(int _idTier, int _idType = 0);

    /// <summary>
    /// Ajouter un nouveau tank
    /// </summary>
    /// <param name="_tank">nouveau tank</param>
    /// <returns>true => OK / false => pas OK</returns>
    Task<bool> AjouterAsync(Tank _tank);

    /// <summary>
    /// Modifier un tank
    /// </summary>
    /// <param name="_tank">tank à modifier</param>
    /// <returns></returns>
    Task<bool> ModifierAsync(Tank _tank);

    /// <summary>
    /// Supprimer un tank
    /// </summary>
    /// <param name="_idTank">id tank conserné</param>
    /// <returns></returns>
    Task<bool> SupprimerAsync(int _idTank);
}
