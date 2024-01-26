namespace back.ModelImport;

public sealed class TankModifierImport
{
    public int Id { get; set; }
    public required string Nom { get; set; }
    public int IdType { get; set; }
    public int IdStatut { get; set; }
    public int IdTier { get; set; }
    public bool EstVisible { get; set; }
}
