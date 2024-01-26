using back.Enums;
using back.ModelExport;
using back.Models;

namespace back.Services.ClanWars;

public interface IClanWarService
{
    /// <summary>
    /// Permet de lister les clans war via discord
    /// </summary>
    /// <param name="_idDiscord">id discord de la personne appellante</param>
    /// <param name="_eEtatClanWar">filtre les clan war</param>
    /// <returns>liste des clan war</returns>
    Task<ClanWarExport[]> ListerAsync(string _idDiscord, EEtatClanWar _eEtatClanWar = EEtatClanWar.toute);

    /// <summary>
    /// Permet de recuperer l'id d'une clan war
    /// </summary>
    /// <param name="_date">date de la clan war conserné</param>
    /// <returns>id clan war / 0 => pas de clan war</returns>
    Task<int> GetIdAsync(DateTime _date);

    /// <summary>
    /// Recuperer l'id de la prochaine clan war par rapport à la date actuelle
    /// </summary>
    /// <returns>id de la clan wars / 0 => pas de clan war</returns>
    Task<int> GetProchaineClanWarAsync();

    /// <summary>
    /// Ajouter une nouvelle clan war
    /// </summary>
    /// <param name="_clanWar">nouvelle clan war</param>
    /// <returns>true => OK / false => pas OK</returns>
    Task<bool> AjouterAsync(ClanWar _clanWar);

    /// <summary>
    /// Permet d'ajouter un joueur a une clan war
    /// </summary>
    /// <param name="_clanWarJoueur"></param>
    /// <returns></returns>
    Task<bool> AjouterParticipantAsync(ClanWarJoueur _clanWarJoueur);

    /// <summary>
    /// Permet de désinscrire un joueur d'une clan war
    /// </summary>
    /// <param name="_idClanWar">id clan war conserné</param>
    /// <param name="_idJoueur">id joueur conserné</param>
    /// <returns></returns>
    Task<bool> DesinscrireAsync(int _idClanWar, int _idJoueur);

    /// <summary>
    /// Supprimer une clan war
    /// </summary>
    /// <param name="_date">date de la clan war</param>
    /// <returns>true => OK / false => pas OK</returns>
    Task<bool> SupprimerAsync(DateTime _date);

    /// <summary>
    /// Check si un joueur est deja inscrit a cette clan war 
    /// </summary>
    /// <param name="_idClanWar">id clan war conserné</param>
    /// <param name="_idJoueur">id joueur conserné</param>
    /// <returns>true => existe / false => existe pas</returns>
    Task<bool> ParticipeDejaAsync(int _idClanWar, int _idJoueur);

    /// <summary>
    /// Check si une clan war existe à cette date
    /// </summary>
    /// <param name="_date">date de la clan war</param>
    /// <returns>true => existe / false => existe pas</returns>
    Task<bool> ExisteAsync(DateTime _date);
}
