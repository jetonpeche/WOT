namespace BotDiscord.Models;

public sealed class Tank
{
    public int Id { get; set; }
    public string Nom { get; set; } = null!;
    public string NomTier { get; set; } = null!;
    public string NomStatut { get; set; } = null!;
    public string NomType { get; set; } = null!;
}