using back.Models;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClanWarController : ControllerBase
    {
        private ClanWarService ClanWarServ { get; init; }
        private JoueurService JoueurServ { get; init; }

        public ClanWarController(ClanWarService _clanWarService, JoueurService _joueurService)
        {
            ClanWarServ = _clanWarService;
            JoueurServ = _joueurService;
        }

        /// <summary>
        /// Liste les clan wars avec la participation (oui / non) du joueur
        /// </summary>
        /// <param name="idDiscord">id discord du compte qui le demande</param>
        /// <returns></returns>
        [HttpGet("lister/{idDiscord}")]
        public async Task<string> Lister(string idDiscord)
        {
            if(JoueurServ.Existe(idDiscord))
            {
                var retour = await ClanWarServ.ListerAsync(idDiscord);

                return JsonConvert.SerializeObject(retour);
            }

            return JsonConvert.SerializeObject(null);
        }

        /// <summary>
        /// Ajout d'une clan war
        /// "IdJoueur" => utiliser dans l'app web uniquement
        /// </summary>
        /// <param name="_clanWarImport"></param>
        /// <returns></returns>
        [HttpPost("ajouter")]
        public async Task<string> Ajouter(ClanWarImport _clanWarImport)
        {
            // proviens du bot
            if(!_clanWarImport.IdJoueur.HasValue)
            {
                if (!JoueurServ.Existe(_clanWarImport.IdDiscord!))
                    return JsonConvert.SerializeObject(0);
            }
            // proviens de l'app
            else if(!JoueurServ.Existe(_clanWarImport.IdJoueur.Value))
                return JsonConvert.SerializeObject(0);

            if (ClanWarServ.Existe(_clanWarImport.Date))
                return JsonConvert.SerializeObject(-1);

            ClanWar clanWar = new()
            {
                Date = _clanWarImport.Date
            };

            int id = await ClanWarServ.AjouterAsync(clanWar);

            return JsonConvert.SerializeObject(id);
        }

        /// <summary>
        /// Supprime une clan war
        /// "IdJoueur" => utiliser dans l'app web uniquement
        /// </summary>
        /// <param name="_clanWarImport"></param>
        /// <returns></returns>
        [HttpPost("supprimer")]
        public async Task<string> Supprimer(ClanWarImport _clanWarImport)
        {
            // proviens du bot
            if (!_clanWarImport.IdJoueur.HasValue)
            {
                if (!JoueurServ.Existe(_clanWarImport.IdDiscord!))
                    return JsonConvert.SerializeObject(0);
            }
            // proviens de l'app
            else if (!JoueurServ.Existe(_clanWarImport.IdJoueur.Value))
                return JsonConvert.SerializeObject(0);

            if (!ClanWarServ.Existe(_clanWarImport.Date))
                return JsonConvert.SerializeObject(-1);

            int retour = await ClanWarServ.SupprimerAsync(_clanWarImport.Date);

            return JsonConvert.SerializeObject(retour);
        }
    }
}
