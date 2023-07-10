using System;
using System.Collections.Generic;

namespace SINGLE_STAGE.Entities;

public partial class Artist
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Appearance> Appearances { get; set; } = new List<Appearance>();
}
