using botDiscord.Enums;
using botDiscord.ModelsExport;
using BotDiscord.Models;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace botDiscord.Commandes;

[SlashCommand("tank_joueur", "tank joueur commande")]
public class TankJoueurCmd : ApplicationCommandModule<SlashCommandContext>
{
    [SubSlashCommand("lister", "Liste les tanks du joueur au tier choisi")]
    public async Task Lister(User _joueur, [SlashCommandParameter(Name = "tier")] ETier _tier)
    {
        Tank[]? tabTank = await ApiService.GetAsync(EApiType.tank,
                                                    $"listerTankJoueurViaDiscord/{_joueur.Id}/{(int)_tier}",
                                                    TankContext.Default.TankArray);

        if (tabTank is null)
        {
            await RespondAsync(InteractionCallback.Message("Erreur réseau"));
            return;
        }

        string nomTier = _tier.ToString().Replace("id", "");

        if (tabTank.Length is 0)
        {
            await RespondAsync(InteractionCallback.Message($"Il n'y a qu'aucun tank de {nomTier}"));
            return;
        }

        List<EmbedFieldProperties> liste = new();

        string nomTypeTank = "";
        foreach (var element in tabTank)
        {
            if (nomTypeTank != element.NomType)
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
                Name = element.Nom,
                Value = $"{element.NomStatut} | {element.NomType}"
            });
        }

        EmbedProperties embed = new()
        {
            Title = $"Liste des tanks {nomTier} de {_joueur.Username}",
            Color = new Color(128, 0, 128),
            Fields = liste
        };

        await RespondAsync(InteractionCallback.Message(new() { Embeds = [embed] }));
    }

    [SubSlashCommand("ajouter", "Ajouter un tank pour soi")]
    public async Task Ajouter([SlashCommandParameter(Name = "id_tank", Description = "l'id Tank est visible via la commande '/tank lister'", MinValue = 1)] int _idTank)
    {
        string? retour = await ApiService.PostAsync<string, IdTankDiscordExport>(EApiType.joueur, "ajouterTankJoueur", new IdTankDiscordExport
        { 
            IdDiscord = Context.User.Id.ToString(), 
            IdTank = _idTank 
        }, IdTankDiscordExportContext.Default.IdTankDiscordExport);

        await RespondAsync(InteractionCallback.Message(retour is null ? "Erreur: \"idTank\" n'existe pas ou erreur serveur" : retour));
    }
}
