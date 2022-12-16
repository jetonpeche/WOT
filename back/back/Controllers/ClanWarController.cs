using back.Enums;
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
        /// Liste les clan war avec la participation (oui / non) du joueur et le nombre de participants
        /// </summary>
        /// <param name="idDiscord">id discord du compte qui le demande</param>
        /// <param name="etatClanWar">0 => participe pas / 1 => participe / 2 => toutes</param>
        /// <returns></returns>
        [HttpGet("listerViaDiscord/{idDiscord}/{etatClanWar}")]
        public async Task<string> Lister(string idDiscord, EEtatClanWar etatClanWar)
        {
            if(JoueurServ.Existe(idDiscord))
            {
                var retour = await ClanWarServ.ListerAsync(idDiscord, etatClanWar);

                return JsonConvert.SerializeObject(retour);
            }

            return JsonConvert.SerializeObject(null);
        }

        /// <summary>
        /// Ajout d'une clan war
        /// "IdJoueur" => utiliser dans l'app web uniquement
        /// </summary>
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
        /// Inscrit le joueur aux clan war
        /// Si "Date" vide ou pas date => inscription prochaine clan war
        /// Sinon incription à la date de la clan war
        /// </summary>
        /// <returns>Message du resultat</returns>
        [HttpPost("participerViaDiscord")]
        public async Task<string> Participer(ParticipantClanWarImport _participantClanWarImport)
        {
            if (!JoueurServ.Existe(_participantClanWarImport.IdDiscord!))
                return JsonConvert.SerializeObject("Je ne te connais pas, participation impossible");

            if(DateTime.TryParse(_participantClanWarImport.Date, out DateTime date))
            {
                if(!ClanWarServ.Existe(date))
                    return JsonConvert.SerializeObject($"La clan war du {date.ToString("d")} n'existe pas");

                int idClanWar = await ClanWarServ.GetIdAsync(date);
                int idJoueur = await JoueurServ.GetIdAsync(_participantClanWarImport.IdDiscord!);

                if(await ClanWarServ.ParticipeDejaAsync(idClanWar, idJoueur))
                    return JsonConvert.SerializeObject("Tu participes déjà à cette clan war");

                ClanWarJoueur clanWarJoueur = new()
                {
                    IdJoueur = idJoueur,
                    IdClanWar = idClanWar,
                    IdTank = null
                };

                bool retour = await ClanWarServ.AjouterParticipantAsync(clanWarJoueur);

                return JsonConvert.SerializeObject(retour ? "Tu as été ajouté" : "Erreur d'ajout à la clan war");
            }
            // inscription a la prochaine clan war
            else
            {
                int idJoueur = await JoueurServ.GetIdAsync(_participantClanWarImport.IdDiscord!);
                int? idClanWar = await ClanWarServ.GetProchaineClanWarAsync();

                if (!idClanWar.HasValue)
                    return JsonConvert.SerializeObject("Aucune clan war prochainement");

                if (await ClanWarServ.ParticipeDejaAsync(idClanWar.Value, idJoueur))
                    return JsonConvert.SerializeObject("Tu participes déjà à la prochaine clan war");

                ClanWarJoueur clanWarJoueur = new()
                {
                    IdJoueur = idJoueur,
                    IdClanWar = idClanWar.Value,
                    IdTank = null
                };

                bool retour = await ClanWarServ.AjouterParticipantAsync(clanWarJoueur);

                return JsonConvert.SerializeObject(retour ? "Tu as été ajouté" : "Erreur d'ajout à la clan war");
            }
        }

        /// <summary>
        /// Désinscription du joueur aux clan war
        /// Si "Date" vide ou pas date => Désinscription prochaine clan war
        /// Sinon désincription à la date de la clan war
        /// </summary>
        /// <returns>Message du resultat</returns>
        [HttpPost("desinscrireViaDiscord")]
        public async Task<string> Desinscription(ParticipantClanWarImport _participantClanWarImport)
        {
            if (!JoueurServ.Existe(_participantClanWarImport.IdDiscord!))
                return JsonConvert.SerializeObject("Je ne te connais pas, désinscription impossible");

            if(DateTime.TryParse(_participantClanWarImport.Date, out DateTime date))
            {
                if (!ClanWarServ.Existe(date))
                    return JsonConvert.SerializeObject($"La clan war du {date.ToString("d")} n'existe pas");

                int idClanWar = await ClanWarServ.GetIdAsync(date);
                int idJoueur = await JoueurServ.GetIdAsync(_participantClanWarImport.IdDiscord!);

                if (!await ClanWarServ.ParticipeDejaAsync(idClanWar, idJoueur))
                    return JsonConvert.SerializeObject($"Tu ne participes pas à cette clan war");

                bool retour = await ClanWarServ.DesinscrireAsync(idClanWar, idJoueur);

                return JsonConvert.SerializeObject(retour ? "Tu as été désincrit" : "Erreur désinscription");
            }
            else
            {
               int? idClanWar = await ClanWarServ.GetProchaineClanWarAsync();

                if (!idClanWar.HasValue)
                    return JsonConvert.SerializeObject("Aucune clan n'existe prochainement");

                int idJoueur = await JoueurServ.GetIdAsync(_participantClanWarImport.IdDiscord!);

                if (!await ClanWarServ.ParticipeDejaAsync(idClanWar.Value, idJoueur))
                    return JsonConvert.SerializeObject($"Tu ne participes pas à cette clan war");

                bool retour = await ClanWarServ.DesinscrireAsync(idClanWar.Value, idJoueur);

                return JsonConvert.SerializeObject(retour ? "Tu as été désincrit" : "Erreur désinscription");
            }
        }

        /// <summary>
        /// Supprime une clan war
        /// "IdJoueur" => utiliser dans l'app web uniquement
        /// </summary>
        /// <returns>0 => erreur ou inconnu id discord / 1 => OK / -1 => date existe pas</returns>
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
