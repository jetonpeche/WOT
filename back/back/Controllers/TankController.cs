//using back.Models;
//using back.Services.Tanks;
//using Microsoft.AspNetCore.Mvc;

//namespace back.Controllers;

//[Route("[controller]")]
//[ApiController]
//public class TankController : ControllerBase
//{
//    private TankService TankServ { get; init; }
//    private ProtectionService ProtectionServ { get; init; }

//	public TankController(TankService _tankService, ProtectionService _protectionService)
//	{
//		TankServ = _tankService;
//		ProtectionServ = _protectionService;
//	}

//	/// <summary>
//	/// Liste les tanks 
//	/// </summary>
//	/// <param name="seulementVisible">false => liste visible et invisible</param>
//	/// <returns>Liste des tanks</returns>
//	[HttpGet("lister/{seulementVisible}")]
//	public async Task<string> Lister(bool seulementVisible)
//	{
//		var liste = await TankServ.ListerAsync(seulementVisible);

//		return JsonConvert.SerializeObject(liste);
//	}

//	/// <summary>
//	/// Liste les tanks du joueur
//	/// </summary>
//	/// <param name="idCompte"></param>
//	/// <returns>Liste nom des tanks</returns>
//	[HttpGet("lister2/{idCompte}")]
//	public async Task<string> Lister(int idCompte)
//	{
//		var liste = await TankServ.ListerAsync(idCompte);

//        return JsonConvert.SerializeObject(liste);
//    }

//	[HttpGet("listerViaDiscord/{idTier}/{idType:int?}")]
//	public async Task<string> Lister(int idTier, int? idType = null)
//	{
//		var liste = await TankServ.ListerAsync(idTier, idType);

//		return JsonConvert.SerializeObject(liste);
//	}

//    [HttpGet("listerTankJoueurViaDiscord/{idDiscord}/{idTier}")]
//    public async Task<string> Lister(string idDiscord, int idTier)
//	{
//        var liste = await TankServ.ListerAsync(idDiscord, idTier);

//        return JsonConvert.SerializeObject(liste);
//    }

//	/// <summary>
//	/// Ajouter un tank
//	/// </summary>
//	/// <returns>0 => erreur / autre => OK</returns>
//	[HttpPost("ajouter")]
//	public async Task<string> Ajouter(TankImport _tankImport)
//	{
//		Tank tank = new()
//		{
//			Nom = ProtectionServ.XSS(_tankImport.Nom),
//			IdTier = _tankImport.IdTier,
//			IdTankStatut = _tankImport.IdStatut,
//			IdTypeTank = _tankImport.IdType,
//            EstVisible = 1
//        };

//		int id = await TankServ.AjouterAsync(tank);

//		return JsonConvert.SerializeObject(id);
//	}

//	/// <summary>
//	/// Modifier un tank
//	/// </summary>
//	/// <returns>true / false</returns>
//	[HttpPost("modifier")]
//	public async Task<string> Modifier(TankModifierImport _tankImport)
//	{
//		Tank tank = new()
//		{
//			Id= _tankImport.Id,
//			Nom = ProtectionServ.XSS(_tankImport.Nom),
//			IdTier = _tankImport.IdTier,
//			IdTankStatut = _tankImport.IdStatut,
//			IdTypeTank = _tankImport.IdType,
//			EstVisible = _tankImport.EstVisible ? 1 : 0
//		};

//		bool retour = await TankServ.ModifierAsync(tank);

//		return JsonConvert.SerializeObject(retour);
//	}

//	/// <summary>
//	/// Supprime un tank
//	/// </summary>
//	/// <returns>true / false</returns>
//	[HttpGet("supprimer/{idTank}")]
//	public async Task<string> Supprimer(int idTank)
//	{
//		bool retour = await TankServ.SupprimerAsync(idTank);

//		return JsonConvert.SerializeObject(retour);
//	}
//}
