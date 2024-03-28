using System;
using System.Collections.Generic;

namespace back.Models;

public partial class Joueur
{
    public int Id { get; set; }

    public string IdDiscord { get; set; } = null!;

    public string Pseudo { get; set; } = null!;

    public string Mdp { get; set; } = null!;

    public int EstAdmin { get; set; }

    public int EstStrateur { get; set; }

    public int EstActiver { get; set; }

    public virtual ICollection<ClanWarJoueur> ClanWarJoueurs { get; set; } = new List<ClanWarJoueur>();

    public virtual ICollection<Tank> IdTanks { get; set; } = new List<Tank>();
}
