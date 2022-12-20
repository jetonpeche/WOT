namespace back.ModelExport
{
    public class JoueurExport
    {
        public int Id { get; set; }
        public string Pseudo { get; set; } = null!;
        public bool EstAdmin { get; set; }
        public bool EstStrateur { get; set; }
        public List<int> ListeIdTank { get; set; } = null!;
    }
}
