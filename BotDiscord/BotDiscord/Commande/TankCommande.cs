using BotDiscord.Models;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using System.ComponentModel;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BotDiscord.Commande;

public class TankCommande : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("lister_tank", "liste les tanks en MP. L'Id est pour ajouter via discord avec la comande 'ajouter_tank_joueur'")]
    public async Task ListerTank(ETier _tier, ETypeTank? _typeTank = null)
    {
        List<Tank> liste = await ApiService.GetAsync<List<Tank>>(EApiType.tank, $"listerViaDiscord/{(int)_tier}/{(int?)_typeTank}");

        EmbedBuilder embedBuilder = new()
        {
            Title = "Liste des tanks",
            Color = Color.DarkPurple
        };

        string nomTypeTank = "";
        foreach (var element in liste)
        {
            if (_typeTank is null && nomTypeTank != element.NomType)
            {
                nomTypeTank = element.NomType;
                embedBuilder.AddField(element.NomType, "------------------");
            }

            embedBuilder.AddField($"Id: {element.Id} - {element.Nom}", $"{element.NomStatut} | {element.NomType}");
        }

        embedBuilder.AddField("Si le tank n'existe pas demander à un admin*", "Merci " + new Emoji("💋"));

        await ReplyAsync(null, false, embedBuilder.Build());
    }

    [SlashCommand("lister_tank_joueur", "Liste les tanks du joueur au tier choisi")]
    public async Task ListerTankJoueur(SocketUser _joueur, ETier _tier)
    {
        List<Tank> liste = await ApiService.GetAsync<List<Tank>>(EApiType.tank, $"listerTankJoueurViaDiscord/{_joueur.Id}/{(int)_tier}");

        if(liste is null)
        {
            await Context.Channel.SendMessageAsync($"Erreur réseau");
            return;
        }

        string nomTier = _tier.ToString().Replace("id", "");

        EmbedBuilder embedBuilder = new()
        {
            Title = $"Liste des tanks {nomTier} de {_joueur.Username}",
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

    [SlashCommand("ajouter_tank_joueur", "Ajouter un tank pour soi")]
    public async Task AjouterTankJoueur([Description("l'idTank est visible via la commande 'lister_tank'"), MinValue(1)] int _idTank)
    {
        string jsonString = JsonSerializer.Serialize(new { IdDiscord = Context.User.Id.ToString(), IdTank = _idTank });

        string retour = await ApiService.PostAsync<string>(EApiType.joueur, "ajouterTankJoueurViaDiscord", jsonString);

        await Context.Channel.SendMessageAsync(retour == default ? "Erreur: \"idTank\" n'existe pas ou erreur serveur" : retour);
    }

    [SlashCommand("ajouter_tank", "Ajouter un tank")]
    public async Task Ajouter(string _nom, ETier _tier, ETypeTank _typeTank, EStatutTank _statutTank)
    {
        string jsonString = JsonSerializer.Serialize(new
        {
            Nom = _nom,
            IdType = (int)_typeTank,
            IdStatut = (int)_statutTank,
            IdTier = (int)_tier
        });

        int id = await ApiService.PostAsync<int>(EApiType.tank, "ajouter", jsonString);

        if(id != 0)
            await Context.Channel.SendMessageAsync("Le tank a été ajouté");
        else
            await Context.Channel.SendMessageAsync("Erreur d'ajout");
    }
}
