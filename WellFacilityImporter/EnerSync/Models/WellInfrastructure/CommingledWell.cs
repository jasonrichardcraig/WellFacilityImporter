using System;
using System.Collections.Generic;

namespace EnerSync.Models.WellInfrastructure;

public partial class CommingledWell
{
    public string ComminglingProcessApprovalNumber { get; set; } = null!;

    public string WellId { get; set; } = null!;

    public string? ComminglingProcess { get; set; }

    public DateOnly? ComminglingEffDate { get; set; }

    public string? CommingledReportingWellId { get; set; }

    public string? CommingledReportingWellProvinceState { get; set; }

    public string? CommingledReportingWellType { get; set; }

    public string? CommingledReportingWellIdentifier { get; set; }

    public virtual Well Well { get; set; } = null!;
}
