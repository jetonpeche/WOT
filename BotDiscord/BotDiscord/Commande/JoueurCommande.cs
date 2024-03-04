using BotDiscord.Models;
using BotDiscord.ModelsExport;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Text.Json;

namespace BotDiscord.Commande;

public class JoueurCommande: InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("lister_joueur", "Liste les joueurs")]
    public async Task Lister([Summary("Role_du_joueur")] ERoleJoueur _roleJoueur)
    {
        Joueur[]? tabJoueur = await ApiService.GetAsync(EApiType.joueur, $"lister/{(int)_roleJoueur}", JoueurContext.Default.JoueurArray);

        if (tabJoueur is null)
        {
            await RespondAsync($"Erreur réseau");
            return;
        }

        if (tabJoueur.Length == 0)
        {
            await RespondAsync($"Aucun joueur");
            return;
        }

        EmbedBuilder embedBuilder = new()
        {
            Title = $"Liste des joueurs",
            Color = Color.DarkOrange
        };

        foreach (var element in tabJoueur)
        {
            embedBuilder.AddField(element.Pseudo,
                $"Strateur: {(element.EstStrateur ? "Oui" : "Non")} | " +
                $"Admin: {(element.EstAdmin ? "Oui" : "Non")}");
        }

        await Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
    }

    [SlashCommand("inscription", "s'inscrire")]
    public async Task Ajouter([Summary("Pseudo")] string _pseudo)
    {
        string jsonString = JsonSerializer.Serialize(new JoueurExport
        {
            IdDiscord = Context.User.Id.ToString(),
            Pseudo = _pseudo,
            EstStrateur = false,
            EstAdmin = false
        }, JoueurExportContext.Default.JoueurExport);

        var reponse = await ApiService.PostAsync(EApiType.joueur, "ajouter", jsonString);

        if (reponse is null)
            await RespondAsync("Erreur d'ajout");

        else if (reponse.IsSuccessStatusCode)
            await RespondAsync("Le joueur a été ajouté");

        else
            await RespondAsync($"Tu es déjà inscrit");
    }

    [SlashCommand("ajouter_joueur", "Ajouter un nouveau joueur")]
    public async Task Ajouter(SocketUser _joueur, 
                             [Summary("Pseudo")] string _pseudo, 
                             [Summary("Est_un_strateur")] bool _estStrateur, 
                             [Summary("Est_un_admin")] bool _estAdmin)
    {
        string jsonString = JsonSerializer.Serialize(new JoueurExport
        {
            IdDiscord = _joueur.Id.ToString(),
            Pseudo = _pseudo,
            EstStrateur = _estStrateur,
            EstAdmin = _estAdmin
        }, JoueurExportContext.Default.JoueurExport);

        var reponse = await ApiService.PostAsync(EApiType.joueur, "ajouter", jsonString);

        if (reponse is null)
            await RespondAsync("Erreur d'ajout");

        else if(reponse.IsSuccessStatusCode)
            await RespondAsync("Le joueur a été ajouté");
           
        else
            await RespondAsync($"Le joueur {_joueur.Username} existe déjà");
    }

    [SlashCommand("supprimer_joueur", "Supprime le joueur choisi")]
    public async Task Supprimer(SocketUser _joueur)
    {
        if (_joueur.IsBot)
        {
            await RespondAsync("Tu ne peux pas me supprimer");
            return;
        }

        await SupprimerAsync(_joueur.Id);
    }

    [SlashCommand("supprime_moi", "Supprime le joueur qui éxecute la commande de la base de donnée")]
    public async Task Supprimer()
    {
        await SupprimerAsync(Context.User.Id);
    }

    private async Task SupprimerAsync(ulong _idDiscord)
    {
        
    }
}
