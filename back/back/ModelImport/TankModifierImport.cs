namespace back.ModelImport;

public class TankModifierImport
{
    public int Id { get; set; }
    public string Nom { get; set; } = null!;
    public int IdType { get; set; }
    public int IdStatut { get; set; }
    public int IdTier { get; set; }
    public bool EstVisible { get; set; }
}
