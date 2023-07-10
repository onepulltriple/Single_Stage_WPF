using System;
using System.Collections.Generic;

namespace SINGLE_STAGE.Entities;

public partial class Appearance
{
    public int Id { get; set; }

    public decimal? RoyaltyAdvance { get; set; }

    public decimal? RoyaltyAtEnd { get; set; }

    public int ArtistId { get; set; }

    public int PerformanceId { get; set; }

    public virtual Artist Artist { get; set; }

    public virtual Performance Performance { get; set; }
}
