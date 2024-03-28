using System;
using System.Collections.Generic;

namespace back.Models;

public partial class Tier
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public virtual ICollection<Tank> Tanks { get; set; } = new List<Tank>();
}
