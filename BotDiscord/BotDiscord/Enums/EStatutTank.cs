using Discord.Interactions;

namespace BotDiscord.Enums;

public enum EStatutTank
{
    [ChoiceDisplay("Méta")] idMeta = 1,
    [ChoiceDisplay("Accepté")] idAccepter,
    [ChoiceDisplay("Toléré")] idTolerer,
    [ChoiceDisplay("Autre")] idAutre
}
