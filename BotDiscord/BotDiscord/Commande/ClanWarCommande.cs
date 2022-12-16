using BotDiscord.Classes;
using BotDiscord.Models;
using Discord;
using Discord.Interactions;
using Newtonsoft.Json;

namespace BotDiscord.Commande;

public class ClanWarCommande: InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("lister_clan_war", "Liste les clan war")]
    public async Task Lister()
    {
        List<ClanWar> liste = await ApiService.GetAsync<List<ClanWar>>(EApiType.clanWar, $"lister/{Context.User.Id}");

        if(liste is null)
        {
            await RespondAsync("Erreur");
            return;
        }

        if(liste.Count is 0)
        {
            await RespondAsync("Aucun clan war programmée prochainement");
            return;
        }

        EmbedBuilder embedBuilder = new()
        {
            Title = "Liste des prochaines clan war",
            Color = Color.DarkRed
        };

        foreach (var element in liste)
            embedBuilder.AddField(element.Date, $"Participe: {(element.Participe ? "Oui" : "Non")} | Nb participant: {element.NbParticipant}");

        await RespondAsync(embed: embedBuilder.Build());
    }

    [SlashCommand("ajouter_clan_war", "Ajouter une clan war")]
    public async Task Ajouter([Summary(description: "Date au format JJ/MM")] string _date)
    {
        if(!Outil.FormatDateOK(_date))
        {
            await RespondAsync("La date doit être au format JJ/MM");
            return;
        }

        DateTime date = DateTime.Parse($"{_date}/{DateTime.Now.Year}");

        if(date <= DateTime.Now)
        {
            await RespondAsync("La date doit être supérieur à la date actuelle");
            return;
        }

        string jsonString = JsonConvert.SerializeObject(new { Date = date, IdDiscord = Context.User.Id.ToString() });

        int id = await ApiService.PostAsync<int>(EApiType.clanWar, "ajouter", jsonString);

        if (id is -1)
            await RespondAsync("La date de la clan war existe déjà");
        else if (id is 0)
            await RespondAsync("Erreur d'ajout");
        else
            await RespondAsync("La clan war a été ajouté");
        
    }

    [SlashCommand("participer_clan_war", "S'inscrire à la clan war (si pas de date => prochaine clan war)")]
    public async Task Participer([Summary(description: "Date au format JJ/MM ou JJ/MM/AAAA")] string? _date = null)
    {
        string jsonString = JsonConvert.SerializeObject(new { Date = $"{_date}", IdDiscord = Context.User.Id.ToString() });

        string retour = await ApiService.PostAsync<string>(EApiType.clanWar, "participerViaDiscord", jsonString);

        if (retour == default)
            await RespondAsync("Erreur d'ajout à la clan war");
        else
            await RespondAsync(retour);
    }

    [SlashCommand("desinscrire_clan_war", "Se désinscrire de la clan war (si pas de date => prochaine clan war)")]
    public async Task Desinscription([Summary(description: "Date au format JJ/MM ou JJ/MM/AAAA")] string? _date = null)
    {
        string jsonString = JsonConvert.SerializeObject(new { Date = _date, IdDiscord = Context.User.Id.ToString() });

        string retour = await ApiService.PostAsync<string>(EApiType.clanWar, "desinscrireViaDiscord", jsonString);

        if (retour == default)
            await RespondAsync("Erreur lors de la désinscription");
        else
            await RespondAsync(retour);
    }

    [SlashCommand("supprimer_clan_war", "Supprime la clan war")]
    public async Task Supprimer([Summary(description: "Date au format JJ/MM")] string _date)
    {
        if (!Outil.FormatDateOK(_date))
        {
            await RespondAsync("La date doit être au format JJ/MM");
            return;
        }

        DateTime date = DateTime.Parse($"{_date}/{DateTime.Now.Year}");

        string jsonString = JsonConvert.SerializeObject(new { Date = date, IdDiscord = Context.User.Id.ToString() });

        int id = await ApiService.PostAsync<int>(EApiType.clanWar, "supprimer", jsonString);

        if (id is -1)
            await RespondAsync("La date de la clan war n'existe pas");
        else if (id is 0)
            await RespondAsync("Erreur de suppression");
        else
            await RespondAsync("La clan war a été supprimé");
    }
}
