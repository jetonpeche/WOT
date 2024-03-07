using NetCord.Services.ApplicationCommands;

namespace botDiscord.Enums;

public enum EEtatClanWar
{
    [SlashCommandChoice(Name = "Participe pas")]
    ParticipePas,
    Participe,
    Toutes
}
