using BotDiscord.Models;
using BotDiscord.ModelsExport;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Text.Json;

namespace BotDiscord.Commande;

public class TankCommande : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("lister_tank", "liste les tanks. L'Id est pour ajouter via discord avec la comande 'ajouter_tank_joueur'")]
    public async Task ListerTank([Summary("Tier")] ETier _tier, 
                                 [Summary("Type_de_tank")] ETypeTank? _typeTank = null)
    {
        Tank[]? tabTank = await ApiService.GetAsync(EApiType.tank, $"listerViaDiscord?IdTier={(int)_tier}{(_typeTank is not null ? $"&IdType={(int)_typeTank}" : "")}", TankContext.Default.TankArray);

        if (tabTank is null)
        {
            await RespondAsync("Erreur réseau");
            return;
        }

        EmbedBuilder embedBuilder = new()
        {
            Title = "Liste des tanks",
            Color = Color.DarkPurple
        };

        string nomTypeTank = "";
        foreach (var element in tabTank)
        {
            if (_typeTank is null && nomTypeTank != element.NomType)
            {
                nomTypeTank = element.NomType;
                embedBuilder.AddField(element.NomType, "------------------");
            }

            embedBuilder.AddField($"Id: {element.Id} - {element.Nom}", $"{element.NomStatut} | {element.NomType}");
        }

        embedBuilder.AddField("Si le tank n'existe pas demander à un admin*", "Merci " + new Emoji("💋"));

        await RespondAsync(embed: embedBuilder.Build());
    }


    [SlashCommand("ajouter_tank", "Ajouter un tank")]
    public async Task Ajouter([Summary("Nom")] string _nom,
                              [Summary("Tier")] ETier _tier, 
                              [Summary("Type_de_tank")] ETypeTank _typeTank, 
                              [Summary("Statut_du_tank")] EStatutTank _statutTank)
    {
        string jsonString = JsonSerializer.Serialize(new TankExport
        {
            Nom = _nom,
            IdType = (int)_typeTank,
            IdStatut = (int)_statutTank,
            IdTier = (int)_tier
        }, TankExportContext.Default.TankExport);

        int id = await ApiService.PostAsync<int>(EApiType.tank, "ajouter", jsonString);

        if(id is not 0)
            await RespondAsync("Le tank a été ajouté");
        else
            await RespondAsync("Erreur d'ajout");
    }
}
