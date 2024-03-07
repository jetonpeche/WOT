using botDiscord.Enums;
using BotDiscord.Models;
using BotDiscord.ModelsExport;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using System.Text.Json;

namespace botDiscord.Commandes;

[SlashCommand("tank", "commande tank")]
public class TankCmd: ApplicationCommandModule<SlashCommandContext>
{
    [SubSlashCommand("lister", "liste les tanks. L'Id est pour ajouter via discord avec la comande 'ajouter_tank_joueur'")]
    public async Task ListerTank([SlashCommandParameter(Name = "tier")] ETier _tier,
                                 [SlashCommandParameter(Name = "type_de_tank")] ETypeTank? _typeTank = null)
    {
        Tank[]? tabTank = await ApiService.GetAsync(
            EApiType.tank,
            $"listerViaDiscord?IdTier={(int)_tier}{(_typeTank is not null ? $"&IdType={(int)_typeTank}" : "")}",
            TankContext.Default.TankArray
        );

        if (tabTank is null)
        {
            await RespondAsync(InteractionCallback.Message("Erreur réseau"));
            return;
        }

        List<EmbedFieldProperties> liste = new();

        EmbedProperties embed = new()
        {
            Title = "Liste des tanks",
            Color = new Color(128, 0, 128),
        };

        string nomTypeTank = "";
        foreach (var element in tabTank)
        {
            if (_typeTank is null && nomTypeTank != element.NomType)
            {
                nomTypeTank = element.NomType;
                liste.Add(new()
                {
                    Name = element.NomType,
                    Value = "------------------"
                });
            }

            liste.Add(new()
            {
                Name = $"Id: {element.Id} - {element.Nom}",
                Value = $"{element.NomStatut} | {element.NomType}"
            });
        }

        liste.Add(new()
        {
            Name = "Si le tank n'existe pas, demande à un admin", 
            Value = $"Merci mon bébou {new EmojiProperties("💋").Unicode}"
        });

        embed.AddFields(liste);

        await RespondAsync(InteractionCallback.Message(new() { Embeds = [embed] }));
    }

    [SlashCommand("ajouter", "Ajouter un tank")]
    public async Task Ajouter([SlashCommandParameter(Name = "nom")] string _nom,
                              [SlashCommandParameter(Name = "tier")] ETier _tier,
                              [SlashCommandParameter(Name = "type")] ETypeTank _typeTank,
                              [SlashCommandParameter(Name = "statut_du_tank")] EStatutTank _statutTank)
    {
        int id = await ApiService.PostAsync<int, TankExport>(EApiType.tank, "ajouter", new TankExport
        {
            Nom = _nom,
            IdType = (int)_typeTank,
            IdStatut = (int)_statutTank,
            IdTier = (int)_tier
        }, TankExportContext.Default.TankExport);

        await RespondAsync(InteractionCallback.Message(id is not 0 ? "Le tank a été ajouté" : "Erreur d'ajout"));
    }
}
