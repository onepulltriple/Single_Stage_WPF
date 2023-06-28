using System;
using System.Collections.Generic;

namespace SINGLE_STAGE.Entities;

public partial class Performance
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int CaventId { get; set; }

    public virtual ICollection<Appearance> Appearances { get; set; } = new List<Appearance>();

    public virtual Cavent Cavent { get; set; } = null!;
}
