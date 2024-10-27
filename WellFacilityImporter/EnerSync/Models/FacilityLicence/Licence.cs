using System;
using System.Collections.Generic;

namespace EnerSync.Models.FacilityLicence;

public partial class Licence
{
    public string LicenceType { get; set; } = null!;

    public string LicenceNumber { get; set; } = null!;

    public string? LicenceStatus { get; set; }

    public DateTime? LicenceStatusDate { get; set; }

    public string? Licensee { get; set; }

    public string? LicenseeName { get; set; }

    public string? EnergyDevelopmentCategoryType { get; set; }

    public string? LicenceLocation { get; set; }

    public string? LicenceLegalSubdivision { get; set; }

    public int? LicenceSection { get; set; }

    public int? LicenceTownship { get; set; }

    public int? LicenceRange { get; set; }

    public int? LicenceMeridian { get; set; }

    public string? OrphanWellFlg { get; set; }
}
