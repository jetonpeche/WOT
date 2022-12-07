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
            await Context.Channel.SendMessageAsync($"Erreur réseau");
            return;
        }

        if (liste.Count == 0)
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

    [SlashCommand("ajouter_joueur", "Ajouter un nouveau joueur")]
    public async Task Ajouter(ulong _idDiscord, string _pseudo, bool _estStrateur, bool _estAdmin)
    {
        string jsonString = JsonSerializer.Serialize(new
        {
            IdDiscord = _idDiscord.ToString(),
            Pseudo = _pseudo,
            EstStrateur = _estStrateur,
            EstAdmin = _estAdmin
        });

        int id = await ApiService.PostAsync<int>(EApiType.joueur, "ajouter", jsonString);

        if (id != default)
            await Context.Channel.SendMessageAsync("Le joueur a été ajouté");
        else
            await Context.Channel.SendMessageAsync("Erreur d'ajout");
    }
}
