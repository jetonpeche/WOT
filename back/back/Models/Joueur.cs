using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class Joueur
    {
        public Joueur()
        {
            IdTanks = new HashSet<Tank>();
        }

        public int Id { get; set; }
        public string IdDiscord { get; set; } = null!;
        public string Pseudo { get; set; } = null!;
        public int EstAdmin { get; set; }
        public int EstStrateur { get; set; }
        public int EstActiver { get; set; }

        public virtual ICollection<Tank> IdTanks { get; set; }
    }
}
