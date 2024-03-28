using System;
using System.Collections.Generic;

namespace back.Models;

public partial class ClanWarJoueur
{
    public int IdClanWar { get; set; }

    public int IdJoueur { get; set; }

    public int? IdTank { get; set; }

    public virtual ClanWar IdClanWarNavigation { get; set; } = null!;

    public virtual Joueur IdJoueurNavigation { get; set; } = null!;

    public virtual Tank? IdTankNavigation { get; set; }
}
