using System;
using System.Collections.Generic;

namespace SINGLE_STAGE.Entities;

public partial class Seatrow
{
    public int Id { get; set; }

    public string Row { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
