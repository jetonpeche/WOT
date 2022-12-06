namespace BotDiscord.Models;

public sealed class Joueur
{
    public int Id { get; set; }
    public string IdDiscord { get; set; } = null!;
    public string Pseudo { get; set; } = null!;
    public bool EstStrateur { get; set; }
    public bool EstAdmin { get; set; }
    public bool EstActiver { get; set; }
}
