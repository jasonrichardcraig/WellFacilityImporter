using System;
using System.Collections.Generic;

namespace EnerSync.Models.WellWiki;

public partial class WellPerforationTreatment
{
    public int Id { get; set; }

    public string WellId { get; set; } = null!;

    public DateOnly? PerforationDate { get; set; }

    public string? PerforationType { get; set; }

    public double? IntervalTop { get; set; }

    public double? IntervalBase { get; set; }

    public int? NumberOfShots { get; set; }

    public virtual Well Well { get; set; } = null!;
}
