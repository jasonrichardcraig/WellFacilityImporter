using System;
using System.Collections.Generic;

namespace EnerSync.Models.FacilityApprovalsDaily;

public partial class FacilityLicence
{
    public string LicenceNumber { get; set; } = null!;

    public string? CompanyName { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? SurfaceLocation { get; set; }

    public string? EnergyDevelopmentCategoryType { get; set; }

    public string? RatingLevel { get; set; }

    public string? LicenceStatus { get; set; }

    public string? LicenceStatusDate { get; set; }

    public string? MaxH2sconcentration { get; set; }

    public string? NonRoutineLicence { get; set; }

    public string? NonRoutineStatus { get; set; }
}
