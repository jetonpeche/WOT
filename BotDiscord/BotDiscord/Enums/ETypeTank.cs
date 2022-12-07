using Discord.Interactions;

namespace BotDiscord.Enums;

public enum ETypeTank
{
    [ChoiceDisplay("Léger")] idLeger = 1,
    [ChoiceDisplay("Médium")] idMedium,
    [ChoiceDisplay("Lourd")] idLourd,
    [ChoiceDisplay("Chasseur de char")] idTd,
    [ChoiceDisplay("Artillerie")] idArty
}
