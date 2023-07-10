using System;
using System.Collections.Generic;

namespace SINGLE_STAGE.Entities;

public partial class Cavent
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal? TicketPrice { get; set; }

    public bool SoldOut { get; set; }

    public virtual ICollection<Performance> Performances { get; set; } = new List<Performance>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
