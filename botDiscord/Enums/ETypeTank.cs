using NetCord.Services.ApplicationCommands;

namespace botDiscord.Enums;

public enum ETypeTank
{
    [SlashCommandChoice(Name = "Léger")] idLeger = 1,
    [SlashCommandChoice(Name = "Médium")] idMedium,
    [SlashCommandChoice(Name = "Lourd")] idLourd,
    [SlashCommandChoice(Name = "Chasseur de char")] idTd,
    [SlashCommandChoice(Name = "Artillerie")] idArty
}
