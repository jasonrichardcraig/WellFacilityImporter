using System;
using System.Collections.Generic;

namespace EnerSync.Models.WellWiki;

public partial class WellDirectionalDrilling
{
    public int Id { get; set; }

    public string WellId { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public double? Depth { get; set; }

    public string? Reason { get; set; }

    public virtual Well Well { get; set; } = null!;
}
