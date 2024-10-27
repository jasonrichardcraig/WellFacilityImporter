using System;
using System.Collections.Generic;

namespace EnerSync.Models.WellWiki;

public partial class WellProductionDatum
{
    public int Id { get; set; }

    public string WellId { get; set; } = null!;

    public int? Period { get; set; }

    public int? TotalProductionHours { get; set; }

    public double? GasQuantity { get; set; }

    public double? OilQuantity { get; set; }

    public double? WaterQuantity { get; set; }

    public virtual Well Well { get; set; } = null!;
}
