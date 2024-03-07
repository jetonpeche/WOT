using NetCord.Services.ApplicationCommands;

namespace botDiscord.Enums;

public enum EStatutTank
{
    [SlashCommandChoice(Name = "Méta")] idMeta = 1,
    [SlashCommandChoice(Name = "Accepté")] idAccepter,
    [SlashCommandChoice(Name = "Toléré")] idTolerer,
    [SlashCommandChoice(Name = "Autre")] idAutre
}
