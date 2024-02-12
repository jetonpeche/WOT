using BotDiscord.Classes;
using BotDiscord.Models;
using Discord;
using Discord.Interactions;
using System.Text.Json;

namespace BotDiscord.Commande;

public class ClanWarCommande: InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("lister_clan_war", "Liste les clan war")]
    public async Task Lister([Summary("EtatClanWar", "Filtre les clan war")] EEtatClanWar _etatClanWar)
    {
        ClanWar[]? tabClanWar = await ApiService.GetAsync(EApiType.clanWar, 
                                                          $"lister/{Context.User.Id}/{(int)_etatClanWar}",
                                                          ClanWarContext.Default.ClanWarArray);

        if(tabClanWar is null)
        {
            await RespondAsync("Erreur");
            return;
        }

        if(tabClanWar.Length is 0)
        {
            await RespondAsync("Aucun clan war");
            return;
        }

        EmbedBuilder embedBuilder = new()
        {
            Title = "Liste des prochaines clan war",
            Color = Color.DarkRed
        };

        foreach (var element in tabClanWar)
            embedBuilder.AddField(element.Date, $"Participe: {(element.Participe ? "Oui" : "Non")} | Nb participant: {element.NbParticipant}");

        await RespondAsync(embed: embedBuilder.Build());
    }

    [SlashCommand("ajouter_clan_war", "Ajouter une clan war")]
    public async Task Ajouter([Summary("Date", "Date au format JJ/MM ou JJ/MM/AAAA")] string _date)
    {
        if(!Outil.FormatDateOK(_date))
        {
            await RespondAsync("La date doit être au format JJ/MM ou JJ/MM/AAAA");
            return;
        }

        DateTime date = DateTime.Parse(_date.Split('/').Length is 2 ? $"{_date}/{DateTime.Now.Year}" : _date);

        if(date < DateTime.Now.Date)
        {
            await RespondAsync("La date doit être supérieur à la date actuelle");
            return;
        }

        string jsonString = JsonSerializer.Serialize(new { Date = date, IdDiscord = Context.User.Id.ToString() });

        var reponse = await ApiService.PostAsync(EApiType.clanWar, "ajouter", jsonString);

        if (reponse is null)
            await RespondAsync("Erreur d'ajout");

        else if (reponse.IsSuccessStatusCode)
            await RespondAsync("La clan war a été programée");

        else
        {
            string msg = (await JsonSerializer.DeserializeAsync<string>(await reponse.Content.ReadAsStreamAsync()))!;
            await RespondAsync(msg.Replace('"', ' '));
        }
    }

    [SlashCommand("participer_clan_war", "S'inscrire à la clan war (si pas de date => prochaine clan war)")]
    public async Task Participer([Summary("Date", "Date au format JJ/MM ou JJ/MM/AAAA")] string? _date = null)
    {
        string jsonString = JsonSerializer.Serialize(new { Date = $"{_date}", IdDiscord = Context.User.Id.ToString() });

        string? retour = await ApiService.PostAsync<string>(EApiType.clanWar, "participer", jsonString);

        if (retour is null)
            await RespondAsync("Erreur d'ajout à la clan war");
        else
            await RespondAsync(retour);
    }

    [SlashCommand("desinscrire_clan_war", "Se désinscrire de la clan war (si pas de date => prochaine clan war)")]
    public async Task Desinscription([Summary("Date", "Date au format JJ/MM ou JJ/MM/AAAA")] string? _date = null)
    {
        string jsonString = JsonSerializer.Serialize(new { Date = _date, IdDiscord = Context.User.Id.ToString() });

        var reponse = await ApiService.DeleteAsync(EApiType.clanWar, "desinscrire", jsonString);

        if (reponse is null)
            await RespondAsync("Erreur lors de la désinscription");

        else if (reponse.IsSuccessStatusCode)
            await RespondAsync("Tu ne participes plus à cette clan war");

        else
        {
            string msg = (await JsonSerializer.DeserializeAsync<string>(await reponse.Content.ReadAsStreamAsync()))!;
            await RespondAsync(msg.Replace('"', ' '));
        }
    }
     
    [SlashCommand("supprimer_clan_war", "Supprime la clan war")]
    public async Task Supprimer([Summary("Date", "Date au format JJ/MM ou JJ/MM/AAAA")] string _date)
    {
        if (!Outil.FormatDateOK(_date))
        {
            await RespondAsync("La date doit être au format JJ/MM ou JJ/MM/AAAA");
            return;
        }

        DateTime date = DateTime.Parse(_date.Split('/').Length is 2 ? $"{_date}/{DateTime.Now.Year}" : _date);

        string jsonString = JsonSerializer.Serialize(new { Date = date, IdDiscord = Context.User.Id.ToString() });

        var reponse = await ApiService.DeleteAsync(EApiType.clanWar, "supprimer", jsonString);

        if (reponse is null)
            await RespondAsync("Erreur de suppression");

        else if (reponse.IsSuccessStatusCode)
            await RespondAsync("La clan war a été supprimée");

        else
        {
            string msg = (await JsonSerializer.DeserializeAsync<string>(await reponse.Content.ReadAsStreamAsync()))!;
            await RespondAsync(msg.Replace('"', ' '));
        }
    }
}
