using BotDiscord.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Text.Json;

namespace BotDiscord.Commande;

public class JoueurCommande: InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("lister_joueur", "Liste les joueurs")]
    public async Task Lister(ERoleJoueur _roleJoueur)
    {
        List<Joueur> liste = await ApiService.GetAsync<List<Joueur>>(EApiType.joueur, $"lister2/{(int)_roleJoueur}");

        if (liste is null)
        {
            await RespondAsync($"Erreur réseau");
            return;
        }

        if (liste.Count == 0)
        {
            await RespondAsync($"Aucun joueur");
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

    [SlashCommand("ajouter_joueur", "Ajouter un nouveau joueur")]
    public async Task Ajouter(SocketUser _joueur, string _pseudo, bool _estStrateur, bool _estAdmin)
    {
        string jsonString = JsonSerializer.Serialize(new
        {
            IdDiscord = _joueur.Id.ToString(),
            Pseudo = _pseudo,
            EstStrateur = _estStrateur,
            EstAdmin = _estAdmin
        });

        int id = await ApiService.PostAsync<int>(EApiType.joueur, "ajouter", jsonString);

        if (id is -1)
            await RespondAsync($"Le joueur {_joueur.Username} existe déjà");
        if (id is 0)
            await RespondAsync("Erreur d'ajout");
        else
            await RespondAsync("Le joueur a été ajouté");
    }

    [SlashCommand("supprimer_joueur", "Supprime le joueur choisi")]
    public async Task Supprimer(SocketUser _joueur)
    {
        if (_joueur.IsBot)
        {
            await RespondAsync("Tu ne peux pas me supprimer");
            return;
        }

        int retour = await ApiService.GetAsync<int>(EApiType.joueur, $"supprimer/{_joueur.Id}");

        if (retour is 1)
            await RespondAsync("Le joueur a été supprimé" + new Emoji("👋"));
        else if (retour is -1)
            await RespondAsync("Le joueur n'existe pas");
        else
            await RespondAsync("Erreur de suppression");
    }

    [SlashCommand("supprime_moi", "Supprime le joueur qui éxecute la commande de la base de donnée")]
    public async Task Supprimer()
    {
        int retour = await ApiService.GetAsync<int>(EApiType.joueur, $"supprimer/{Context.User.Id}");

        if (retour is 1)
            await RespondAsync("Tu as été supprimé" + new Emoji("👋"));
        else if(retour is -1)
            await RespondAsync("Tu n'existes pas");
        else
            await RespondAsync("Erreur de suppression");
    }
}
