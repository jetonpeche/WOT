using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class ClanWar
    {
        public ClanWar()
        {
            ClanWarJoueurs = new HashSet<ClanWarJoueur>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<ClanWarJoueur> ClanWarJoueurs { get; set; }
    }
}
