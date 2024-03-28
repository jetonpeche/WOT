using System;
using System.Collections.Generic;

namespace back.Models;

public partial class Tank
{
    public int Id { get; set; }

    public int IdTier { get; set; }

    public int IdTankStatut { get; set; }

    public int IdTypeTank { get; set; }

    public string Nom { get; set; } = null!;

    public int? EstVisible { get; set; }

    public virtual ICollection<ClanWarJoueur> ClanWarJoueurs { get; set; } = new List<ClanWarJoueur>();

    public virtual TankStatut IdTankStatutNavigation { get; set; } = null!;

    public virtual Tier IdTierNavigation { get; set; } = null!;

    public virtual TypeTank IdTypeTankNavigation { get; set; } = null!;

    public virtual ICollection<Joueur> IdJoueurs { get; set; } = new List<Joueur>();
}
