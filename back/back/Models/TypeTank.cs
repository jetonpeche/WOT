using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class TypeTank
    {
        public TypeTank()
        {
            Tanks = new HashSet<Tank>();
        }

        public int Id { get; set; }
        public string Nom { get; set; } = null!;
        public string NomImage { get; set; } = null!;

        public virtual ICollection<Tank> Tanks { get; set; }
    }
}
