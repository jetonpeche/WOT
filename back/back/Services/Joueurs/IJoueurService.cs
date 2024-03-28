using back.Enums;
using back.ModelExport;
using back.Models;

namespace back.Services.Joueurs;

public interface IJoueurService
{
    /// <summary>
    /// Permet de lister les joueurs
    /// </summary>
    /// <param name="_roleJoueur">filtre par role</param>
    /// <returns>liste des joueurs</returns>
    Task<JoueurExport[]> ListerAsync(ERoleJoueur _roleJoueur);

    /// <summary>
    /// Lister les joueurs qui possède le tank
    /// </summary>
    /// <param name="_idTank">id tank conserné</param>
    /// <returns>Liste des noms des joueurs</returns>
    Task<string[]> ListerPossedeTankAsync(int _idTank);

    /// <summary>
    /// Recuperer un joueur
    /// </summary>
    /// <param name="_pseudo">pseudo du joueur conserné</param>
    /// <returns>le joueur ou null</returns>
    Task<JoueurExport?> GetInfoAsync(string _pseudo);

    /// <summary>
    /// Donne le mdp 
    /// </summary>
    /// <param name="_pseudo">pseudo conserné</param>
    /// <returns>null => existe pas / mdp</returns>
    Task<string?> GetMdpAsync(string _pseudo);

    /// <summary>
    /// Recuperer l'id du joueur
    /// </summary>
    /// <param name="_idDiscord">id discord du joueur conserné</param>
    /// <returns>id joueur / 0 => existe pas</returns>
    Task<int> GetIdAsync(string _idDiscord);

    /// <summary>
    /// Ajouter un nouveau joueur
    /// </summary>
    /// <param name="_joueur">nouveau joueur</param>
    /// <returns>true => OK / false => pas OK</returns>
    Task<bool> AjouterAsync(Joueur _joueur);

    /// <summary>
    /// Ajouter un tank à un joueur
    /// </summary>
    /// <param name="_idJoueur">id joueur conserné</param>
    /// <param name="_idTank">id tank conserné</param>
    /// <returns>true => OK / false => pas OK</returns>
    Task<bool> AjouterTankJoueurAsync(int _idJoueur, int _idTank);

    /// <summary>
    /// Modifier un joueur
    /// </summary>
    /// <param name="_joueurImport">joueur conserné avec les infos</param>
    /// <returns>true => OK / false => pas OK</returns>
    Task<bool> ModifierAsync(JoueurImport _joueurImport);

    /// <summary>
    /// Inserve l'etat l'activation du compte  
    /// SI 1 => 0 et inversement
    /// </summary>
    /// <param name="_idJoueur">id joueur conserné</param>
    /// <returns>true => OK / false => pas OK</returns>
    Task<bool> InverserEtatActiverAsync(int _idJoueur);

    /// <summary>
    /// Supprimer un tank d'un joueur
    /// </summary>
    /// <param name="_idJoueur">id joueur conserné</param>
    /// <param name="_idTank">id tank du joueur conserné</param>
    /// <returns>true => OK / false => pas OK</returns>
    Task<bool> SupprimerTankJoueurAsync(int _idJoueur, int _idTank);

    /// <summary>
    /// Supprimer un joueur
    /// </summary>
    /// <param name="_idDiscord">id discord conserné</param>
    /// <returns>true => OK / false => pas OK</returns>
    Task<bool> SupprimerAsync(string _idDiscord);

    /// <summary>
    /// Check si un compte joueur est activer
    /// </summary>
    /// <param name="_pseudo"></param>
    /// <returns></returns>
    Task<bool> EstActiverAsync(string _pseudo);

    /// <summary>
    /// Check si le joueur possede le tank
    /// </summary>
    /// <param name="_idJoueur">id joueur conserné</param>
    /// <param name="_idTank">id tank conserné</param>
    /// <returns>true => OK / false => pas OK</returns>
    Task<bool> PossedeTankAsync(int _idJoueur, int _idTank);

    /// <summary>
    /// Check si le joueur possede le tank
    /// </summary>
    /// <param name="_idDiscord">id discord conserné</param>
    /// <param name="_idTank">id tank conserné</param>
    /// <returns>true => OK / false => pas OK</returns>
    Task<bool> PossedeTankAsync(string _idDiscord, int _idTank);

    /// <summary>
    /// Check si le pseudo existe
    /// </summary>
    /// <param name="_pseudo">pseudo conserné</param>
    /// <returns>true => existe / false => existe pas</returns>
    Task<bool> PseudoExisteAsync(string _pseudo);

    /// <summary>
    /// Check si le compte existe
    /// </summary>
    /// <param name="_idDiscord">id discord conserné</param>
    /// <returns>true => existe / false => existe pas</returns>
    Task<bool> ExisteAsync(string _idDiscord);

    /// <summary>
    /// Check si le compte existe
    /// </summary>
    /// <param name="_idJoueur">id joueur conserné</param>
    /// <returns>true => existe / false => existe pas</returns>
    Task<bool> ExisteAsync(int _idJoueur);
}
