using back.Services;
using Microsoft.AspNetCore.Mvc;
using back.Enums;
using back.Models;

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
        IQueryable? liste = null;

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

        return JsonConvert.SerializeObject(liste is null ? Array.Empty<string>() : liste);
    }

    [HttpPost("Ajouter")]
    public async Task<string> Ajouter(JoueurImport _joueurImport)
    {
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
}
