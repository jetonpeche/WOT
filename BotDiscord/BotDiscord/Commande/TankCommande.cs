using BotDiscord.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Text.Json;

namespace BotDiscord.Commande;

public class TankCommande : InteractionModuleBase<SocketInteractionContext>
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
