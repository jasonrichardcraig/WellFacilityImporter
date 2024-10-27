using System;
using System.Collections.Generic;

namespace EnerSync.Models.WellLicence;

public partial class Well
{
    public string? LicenceType { get; set; }

    public string? LicenceNumber { get; set; }

    public DateTime? LicenceIssueDate { get; set; }

    public string? LicenceStatus { get; set; }

    public DateTime? LicenceStatusDate { get; set; }

    public string? LicenseeId { get; set; }

    public string? LicenseeName { get; set; }

    public string? LicenceLocation { get; set; }

    public string? LicenceLegalSubdivision { get; set; }

    public int? LicenceSection { get; set; }

    public int? LicenceTownship { get; set; }

    public int? LicenceRange { get; set; }

    public int? LicenceMeridian { get; set; }

    public string? DrillingOperationType { get; set; }

    public string? WellPurpose { get; set; }

    public string? WellLicenceType { get; set; }

    public string? WellSubstance { get; set; }

    public string? ProjectedFormation { get; set; }

    public string? TerminatingFormation { get; set; }

    public decimal? ProjectedTotalDepth { get; set; }

    public string? Aerclass { get; set; }

    public string? HeadLessor { get; set; }

    public string? WellCompletionType { get; set; }

    public string? TargetPool { get; set; }

    public string? OrphanWellFlg { get; set; }
}
