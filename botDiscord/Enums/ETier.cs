using NetCord.Services.ApplicationCommands;

namespace botDiscord.Enums;

public enum ETier
{
    [SlashCommandChoice(Name = "Tier 6")]
    idTier6 = 1,

    [SlashCommandChoice(Name = "Tier 8")]
    idTier8 = 2,

    [SlashCommandChoice(Name = "Tier 10")]
    idTier10 = 3
}
