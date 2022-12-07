using back.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace back.Controllers;

[Route("[controller]")]
[ApiController]
public class TankController : ControllerBase
{
    private TankService TankServ { get; init; }
    private ProtectionService ProtectionServ { get; init; }

	public TankController(TankService _tankService, ProtectionService _protectionService)
	{
		TankServ = _tankService;
		ProtectionServ = _protectionService;
	}

	[HttpGet("lister")]
	public async Task<string> Lister()
	{
		var liste = await TankServ.ListerAsync();

		return JsonConvert.SerializeObject(liste);
	}

	[HttpGet("lister/{idCompte}")]
	public async Task<string> Lister(int idCompte)
	{
		var liste = await TankServ.ListerAsync(idCompte);

        return JsonConvert.SerializeObject(liste);
    }

    [HttpGet("listerViaDiscord/{idDiscord}/{idTier}")]
    public async Task<string> Lister(string idDiscord, int idTier)
	{
        var liste = await TankServ.ListerAsync(idDiscord, idTier);

        return JsonConvert.SerializeObject(liste);
    }

	[HttpPost("ajouter")]
	public async Task<string>Ajouter(TankImport _tankImport)
	{
		Tank tank = new()
		{
			Nom = ProtectionServ.XSS(_tankImport.Nom),
			IdTier = _tankImport.IdTier,
			IdTankStatut = _tankImport.IdStatut,
			IdTypeTank = _tankImport.IdType,
            EstVisible = 1
        };

		int id = await TankServ.AjouterAsync(tank);

		return JsonConvert.SerializeObject(id);
	}
}
