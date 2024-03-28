using System;
using System.Collections.Generic;

namespace back.Models;

public partial class ClanWar
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public virtual ICollection<ClanWarJoueur> ClanWarJoueurs { get; set; } = new List<ClanWarJoueur>();
}
