using System;
using System.Collections.Generic;

namespace SINGLE_STAGE.Entities;

public partial class Ticket
{
    public int Id { get; set; }

    public int CaventId { get; set; }

    public int TicketholderId { get; set; }

    public int SeatRowId { get; set; }

    public int SeatNumberId { get; set; }

    public virtual Cavent Cavent { get; set; }

    public virtual Seatnumber SeatNumber { get; set; }

    public virtual Seatrow SeatRow { get; set; }

    public virtual Ticketholder Ticketholder { get; set; }
}
