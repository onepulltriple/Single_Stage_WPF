using System;
using System.Collections.Generic;

namespace SINGLE_STAGE.Entities;

public partial class Ticketholder
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public bool Discount { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
