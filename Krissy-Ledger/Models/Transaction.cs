using System;
using System.Collections.Generic;

namespace Krissy_Ledger.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public string? ServiceName { get; set; }

    public decimal ServicePrice { get; set; }

    public decimal TipAmount { get; set; }

    public string? UserName { get; set; }
}
