using botDiscord.Enums;
using NetCord.Rest;
using NetCord;
using NetCord.Services.ApplicationCommands;
using BotDiscord.Models;
using botDiscord.ModelsExport;

namespace botDiscord.Commandes;

[SlashCommand("clan_war", "commande clan war")]
public class ClanWarCmd : ApplicationCommandModule<SlashCommandContext>
{
    [SubSlashCommand("lister", "lister les clan war")]
    public async Task Lister([SlashCommandParameter(Name = "etat_clan_war")] EEtatClanWar _eEtatClanWar)
    {
        ClanWar[]? clanWar = await ApiService.GetAsync(EApiType.clanWar,
                                                       $"lister/{Context.User.Id}/{(int)_eEtatClanWar}",
                                                       ClanWarContext.Default.ClanWarArray);

        if (clanWar is null)
        {
            await RespondAsync(InteractionCallback.Message("Erreur réseau"));
            return;
        }
        else if (clanWar.Length is 0)
        {
            await RespondAsync(InteractionCallback.Message("Aucune clan war programmée"));
            return;
        }

        List<EmbedFieldProperties> liste = new();

        foreach (var element in clanWar)
        {
            liste.Add(new()
            {
                Name = element.Date,
                Value = $"Participe: {(element.Participe ? "Oui" : "Non")} | Nb participant: {element.NbParticipant}"
            });
        }

        EmbedProperties embed = new()
        {
            Title = "Liste des clan war",
            Color = new Color(128, 0, 128),
            Fields = liste
        };

        await RespondAsync(InteractionCallback.Message(new() { Embeds = [embed] }));
    }

    [SubSlashCommand("ajouter", "programmer une nouvelle clan war")]
    public async Task Ajouter([SlashCommandParameter(Name = "date", Description = "Exemple: JJ/MM/AAAA, JJ/MM, AAAA-MM-JJ, MM-JJ")] string _date)
    {
        if (DateTime.TryParse(_date, out var date))
        {
            var reponse = await ApiService.PostAsync(EApiType.clanWar, "ajouter", new DateIdDiscordExport<DateTime>
            {
                Date = date,
                IdDiscord = Context.User.Id.ToString()
            }, DateIdDiscordExportContext.Default.DateIdDiscordExportDateTime);

            if (reponse is null)
                await RespondAsync(InteractionCallback.Message("Erreur réseau"));

            else if (reponse.IsSuccessStatusCode)
                await RespondAsync(InteractionCallback.Message($"La clan a été programmée pour le: {date.ToString("d")}"));

            else
            {
                string msg = await reponse.Content.ReadAsStringAsync();
                await RespondAsync(InteractionCallback.Message(msg.Replace('"', ' ')));
            }
        }
        else
            await RespondAsync(InteractionCallback.Message("Doit être une date (exemple: JJ/MM/AAAA, JJ/MM, AAAA-MM-JJ, MM-JJ"));
    }

    [SubSlashCommand("participer", "Participer à une clan war (Si pas de date => prochaine clan war)")]
    public async Task Participer([SlashCommandParameter(Name = "date", Description = "Exemple: JJ/MM/AAAA, JJ/MM, AAAA-MM-JJ, MM-JJ")] string? _date = null)
    {
        if(_date is not null)
        {
            if(!DateTime.TryParse(_date, out _))
                await RespondAsync(InteractionCallback.Message("Exemple: JJ/MM/AAAA, JJ/MM, AAAA-MM-JJ, MM-JJ"));
        }

        var reponse = await ApiService.PostAsync(EApiType.clanWar, "participer", new DateIdDiscordExport<string>
        {
            Date = DateTime.TryParse(_date, out var date) ? date.ToString("d") : string.Empty,
            IdDiscord = Context.User.Id.ToString()
        }, DateIdDiscordExportContext.Default.DateIdDiscordExportString);

        if (reponse is null)
            await RespondAsync(InteractionCallback.Message("Erreur réseau"));

        else if (reponse.IsSuccessStatusCode)
            await RespondAsync(InteractionCallback.Message($"Tu as été inscript à la clan war"));

        else
        {
            string msg = await reponse.Content.ReadAsStringAsync();
            await RespondAsync(InteractionCallback.Message(msg.Replace('"', ' ')));
        }
    }

    [SubSlashCommand("desinscrire", "Se désinscrire de la clan war (si pas de date => prochaine clan war)")]
    public async Task Desinscrire([SlashCommandParameter(Name = "date", Description = "Exemple: JJ/MM/AAAA, JJ/MM, AAAA-MM-JJ, MM-JJ")] string? _date = null)
    {
        if (_date is not null)
        {
            if (!DateTime.TryParse(_date, out _))
                await RespondAsync(InteractionCallback.Message("Exemple: JJ/MM/AAAA, JJ/MM, AAAA-MM-JJ, MM-JJ"));
        }

        var reponse = await ApiService.DeleteAsync(EApiType.clanWar, "desinscrire", new DateIdDiscordExport<string>
        {
            Date = DateTime.TryParse(_date, out var date) ? date.ToString("d") : string.Empty,
            IdDiscord = Context.User.Id.ToString()
        }, DateIdDiscordExportContext.Default.DateIdDiscordExportString);

        if (reponse is null)
            await RespondAsync(InteractionCallback.Message("Erreur réseau"));

        else if (reponse.IsSuccessStatusCode)
            await RespondAsync(InteractionCallback.Message($"Tu as été désinscrit à la clan war"));

        else
        {
            string msg = await reponse.Content.ReadAsStringAsync();
            await RespondAsync(InteractionCallback.Message(msg.Replace('"', ' ')));
        }
    }

    [SubSlashCommand("supprimer", "Supprimer une clan war")]
    public async Task Supprimer([SlashCommandParameter(Name = "date", Description = "Exemple: JJ/MM/AAAA, JJ/MM, AAAA-MM-JJ, MM-JJ")] string _date)
    {
        if (DateTime.TryParse(_date, out var date))
        {
            var reponse = await ApiService.DeleteAsync(EApiType.clanWar, "supprimer", new DateIdDiscordExport<DateTime>
            {
                Date = date,
                IdDiscord = Context.User.Id.ToString()
            }, DateIdDiscordExportContext.Default.DateIdDiscordExportDateTime);

            if (reponse is null)
                await RespondAsync(InteractionCallback.Message("Erreur réseau"));

            else if (reponse.IsSuccessStatusCode)
                await RespondAsync(InteractionCallback.Message($"La clan a été supprimée pour le: {date.ToString("d")}"));

            else
            {
                string msg = await reponse.Content.ReadAsStringAsync();
                await RespondAsync(InteractionCallback.Message(msg.Replace('"', ' ')));
            }
        }
        else
            await RespondAsync(InteractionCallback.Message("Doit être une date (exemple: JJ/MM/AAAA, JJ/MM, AAAA-MM-JJ, MM-JJ"));
    }
}
