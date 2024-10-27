using System;
using System.Collections.Generic;

namespace EnerSync.Models.FacilityOperatorHistory;

public partial class OperatorHistory
{
    public string FacilityId { get; set; } = null!;

    public string OperatorBaid { get; set; } = null!;

    public string? OperatorName { get; set; }

    public string StartDate { get; set; } = null!;

    public string? EndDate { get; set; }

    public virtual Facility Facility { get; set; } = null!;
}
