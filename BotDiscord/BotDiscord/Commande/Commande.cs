using BotDiscord.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace BotDiscord.Commande;

public class Commande : InteractionModuleBase<SocketInteractionContext>
{        
    [SlashCommand("lister_tank_joueur", "Liste les tanks du joueur au tier choisi")]
    public async Task ListerTankJoueur(SocketUser _utilisateur, ETier _tier)
    {
        List<Tank> liste = await ApiService.GetAsync<List<Tank>>(EApiType.tank, $"listerViaDiscord/{_utilisateur.Id}/{(int)_tier}");

        if(liste is null)
        {
            await Context.Channel.SendMessageAsync($"Erreur réseau");
            return;
        }

        string nomTier = _tier.ToString().Replace("id", "");

        EmbedBuilder embedBuilder = new()
        {
            Title = $"Liste des tanks {nomTier} de {_utilisateur.Username}",
            Color = Color.DarkPurple
        };

        if(liste.Count is 0)
        {
            await Context.Channel.SendMessageAsync($"Il n'y a qu'un tank de {nomTier}");
            return;
        }

        string nomTypeTank = "";
        foreach (var element in liste)
        {
            if(nomTypeTank != element.NomType)
            {
                nomTypeTank = element.NomType;
                embedBuilder.AddField(element.NomType, "------------------");
            }

            embedBuilder.AddField(element.Nom, $"{element.NomStatut} | {element.NomType}");
        }

        await Context.Channel.SendMessageAsync(null, false, embedBuilder.Build());
    }

    [SlashCommand("lister_joueur", "Liste les joueurs")]
    public async Task ListerJoueur()
    {
        List<Joueur> liste = await ApiService.GetAsync<List<Joueur>>(EApiType.joueur, $"lister");

        if (liste is null)
        {
            await Context.Channel.SendMessageAsync($"Erreur réseau");
            return;
        }

        if(liste.Count == 0)
        {
            await Context.Channel.SendMessageAsync($"Aucun joueur");
            return;
        }

        EmbedBuilder embedBuilder = new()
        {
            Title = $"Liste des joueurs",
            Color = Color.DarkOrange
        };

        foreach (var element in liste)
        {
            embedBuilder.AddField(element.Pseudo, 
                $"Strateur: {(element.EstStrateur ? "Oui" : "Non")} | " +
                $"Admin: {(element.EstAdmin ? "Oui" : "Non")}");
        }

        await Context.Channel.SendMessageAsync(null, false, embedBuilder.Build());
    }
}
