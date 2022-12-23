using back.Enums;
using back.ModelExport;
using back.Models;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

[Route("[controller]")]
[ApiController]
public class JoueurController : Controller
{
    private JoueurService JoueurServ { get; init; }
    private ProtectionService ProtectionServ { get; init; }

    public JoueurController(JoueurService _joueurService, ProtectionService _protectionService)
	{
        JoueurServ = _joueurService;
        ProtectionServ = _protectionService;
    }

    [HttpGet("lister")]
    public async Task<string> Lister()
    {
        var liste = await JoueurServ.ListerAsync();

        return JsonConvert.SerializeObject(liste);
    }

    /// <summary>
    /// 0 => admin / 1 => strateur / 2 => tous
    /// </summary>
    [HttpGet("lister2/{roleJoueur}")]
    public async Task<string> Lister(ERoleJoueur roleJoueur)
    {
        List<JoueurExport> liste = new();

        switch (roleJoueur)
        {
            case ERoleJoueur.admin:
                liste = await JoueurServ.ListerAdminAsync();
                break;

            case ERoleJoueur.strateur:
                liste = await JoueurServ.ListerStrateurAsync();
                break;

            case ERoleJoueur.tous:
                liste = await JoueurServ.ListerAsync();
                break;
        }

        return JsonConvert.SerializeObject(liste);
    }

    /// <summary>
    /// Donne les infos du joueur ou null (existe pas)
    /// </summary>
    /// <param name="pseudo"></param>
    [HttpGet("info/{pseudo}")]
    public async Task<string> GetInfo(string pseudo)
    {
        var infoJoueur = await JoueurServ.GetInfoAsync(pseudo);

        return JsonConvert.SerializeObject(infoJoueur == null ? null : infoJoueur);
    }

    /// <summary>
    /// Ajout d'un nouveau joueur
    /// </summary>
    /// <returns> -1 => existe deja / 0 => erreur / autre (id du joueur) => OK</returns>
    [HttpPost("ajouter")]
    public async Task<string> Ajouter(JoueurImport _joueurImport)
    {
        if(JoueurServ.Existe(_joueurImport.IdDiscord))
            return JsonConvert.SerializeObject(-1);

        Joueur joueur = new()
        {
            IdDiscord = ProtectionServ.XSS(_joueurImport.IdDiscord),
            Pseudo = ProtectionServ.XSS(_joueurImport.Pseudo),
            EstAdmin = _joueurImport.EstAdmin ? 1 : 0,
            EstStrateur = _joueurImport.EstStrateur ? 1 : 0,
            EstActiver = 1
        };

        int id = await JoueurServ.AjouterAsync(joueur);

        return JsonConvert.SerializeObject(id);
    }

    /// <summary>
    /// Active un joueur
    /// </summary>
    /// <param name="idJoueur"></param>
    /// <returns></returns>
    [HttpPost("activer/{idJoueur}")]
    public async Task<string> Activer(int idJoueur)
    {
        bool retour = await JoueurServ.Activer(idJoueur);

        return JsonConvert.SerializeObject(retour);
    }

    /// <summary>
    /// Desactive un joueur
    /// </summary>
    /// <param name="idJoueur"></param>
    /// <returns></returns>
    [HttpPost("desactiver/{idJoueur}")]
    public async Task<string> Desactiver(int idJoueur)
    {
        Console.WriteLine(idJoueur);
        bool retour = await JoueurServ.Desactiver(idJoueur);

        return JsonConvert.SerializeObject(retour);
    }

    /// <summary>
    /// Ajouter un char a un joueur
    /// "IdJoueur" n'est pas utilisé SI via discord
    /// </summary>
    /// <returns>App web => Bool / discord => texte</returns>
    [HttpPost("ajouterTankJoueur")]
    public async Task<string> Ajouter(JoueurTankImport _joueurTankImport)
    {
        int idJoueur = default;
        bool appelerViaDiscord = false;

        if (!_joueurTankImport.IdJoueur.HasValue)
        {
            appelerViaDiscord = true;

            if (JoueurServ.PossedeTank(_joueurTankImport.IdDiscord!, _joueurTankImport.IdTank))
                return JsonConvert.SerializeObject("Tu possèdes déjà le tank");

            idJoueur = await JoueurServ.GetIdAsync(_joueurTankImport.IdDiscord!);
        }
        else
            idJoueur = _joueurTankImport.IdJoueur.Value;

        if(idJoueur == default)
            return JsonConvert.SerializeObject("Je ne te connais pas");

        bool estAjout = await JoueurServ.AjouterTankJoueurAsync(idJoueur, _joueurTankImport.IdTank);

        if(appelerViaDiscord)
            return JsonConvert.SerializeObject(estAjout ? "Le tank a été ajouté" : "erreur d'ajout");

        return JsonConvert.SerializeObject(estAjout);
    }

    [HttpPost("modifier")]
    public async Task<string> Modifier(JoueurImport _joueurImport)
    {
        bool retour = await JoueurServ.ModifierAsync(_joueurImport);

        return JsonConvert.SerializeObject(retour);
    }

    /// <summary>
    /// Supprime la possession d'un tank
    /// "IdJoueur" n'est pas utilisé SI via discord
    /// </summary>
    /// <returns>App web => Bool / discord => texte</returns>
    [HttpPost("supprimerTankJoueur")]
    public async Task<string> Supprimer(JoueurTankImport _joueurTankImport)
    {
        int idJoueur = default;
        bool appelerViaDiscord = false;

        if (!_joueurTankImport.IdJoueur.HasValue)
        {
            appelerViaDiscord = true;
            idJoueur = await JoueurServ.GetIdAsync(_joueurTankImport.IdDiscord!);
        }
        else
            idJoueur = _joueurTankImport.IdJoueur.Value;

        if (idJoueur == default)
            return JsonConvert.SerializeObject("Je ne te connais pas");

        bool estSupp = await JoueurServ.SupprimerTankJoueurAsync(idJoueur, _joueurTankImport.IdTank);

        if (appelerViaDiscord)
            return JsonConvert.SerializeObject(estSupp ? "Le tank a été dépossédé" : "Erreur de dépossédage");

        return JsonConvert.SerializeObject(estSupp);
    }

    /// <summary>
    /// Supprime le joueur et toutes ses données
    /// </summary>
    /// <returns>1 => OK / 0 => erreur / -1 => existe pas</returns>
    [HttpGet("supprimer/{idDiscord}")]
    public async Task<string> Supprimer(string idDiscord)
    {
        if(JoueurServ.Existe(idDiscord))
        {
            bool retour = await JoueurServ.SupprimerAsync(idDiscord);
            return JsonConvert.SerializeObject(retour ? 1 : 0);
        }
        else
        {
            return JsonConvert.SerializeObject(-1);
        }
    }
}
