using System;
using System.Collections.Generic;

namespace back.Models;

public partial class TypeTank
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public string NomImage { get; set; } = null!;

    public virtual ICollection<Tank> Tanks { get; set; } = new List<Tank>();
}
