namespace back.ModelImport;

public class JoueurImport
{
    public string IdDiscord { get; set; } = null!;
    public string Pseudo { get; set; } = null!;
    public bool EstStrateur { get; set; }
    public bool EstAdmin { get; set; }
}
