using System;
using System.Collections.Generic;

namespace EnerSync.Models.WellWiki;

public partial class Well
{
    public string WellId { get; set; } = null!;

    public string? AlternateWellId { get; set; }

    public string? WellName { get; set; }

    public string? FormattedWellName { get; set; }

    public string? Location { get; set; }

    public string? LocationAlias { get; set; }

    public string? LocationAlternateAlias { get; set; }

    public string? Country { get; set; }

    public string? Province { get; set; }

    public int? Township { get; set; }

    public int? Meridian { get; set; }

    public int? Range { get; set; }

    public int? Section { get; set; }

    public string? County { get; set; }

    public double? SurfaceHoleLatitude { get; set; }

    public double? SurfaceHoleLongitude { get; set; }

    public string? OperatorName { get; set; }

    public string? LicenseNumber { get; set; }

    public DateOnly? LicenseDate { get; set; }

    public string? LicenseStatus { get; set; }

    public DateOnly? SpudDate { get; set; }

    public DateOnly? FinalDrillDate { get; set; }

    public double? WellTotalDepth { get; set; }

    public DateTime DateTimeCreated { get; set; }

    public virtual ICollection<WellDirectionalDrilling> WellDirectionalDrillings { get; set; } = new List<WellDirectionalDrilling>();

    public virtual ICollection<WellHistory> WellHistories { get; set; } = new List<WellHistory>();

    public virtual ICollection<WellPerforationTreatment> WellPerforationTreatments { get; set; } = new List<WellPerforationTreatment>();

    public virtual ICollection<WellProductionDatum> WellProductionData { get; set; } = new List<WellProductionDatum>();
}
