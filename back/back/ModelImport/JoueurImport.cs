namespace back.ModelImport;

public class JoueurImport
{
    /// <summary>
    /// Uniquement pour la modification du joueur
    /// </summary>
    public int Id { get; set; }
    public string IdDiscord { get; set; } = null!;
    public string Pseudo { get; set; } = null!;
    public bool EstStrateur { get; set; }
    public bool EstAdmin { get; set; }
}
