using back.Services;
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
}
