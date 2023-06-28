using System;
using System.Collections.Generic;

namespace SINGLE_STAGE.Entities;

public partial class Seatnumber
{
    public int Id { get; set; }

    public int Number { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
