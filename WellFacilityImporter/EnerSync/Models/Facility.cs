using System;
using System.Collections.Generic;

namespace EnerSync.Models;

public partial class Facility
{
    public string FacilityId { get; set; } = null!;

    public string? FacilityProvinceState { get; set; }

    public string? FacilityType { get; set; }

    public string? FacilityIdentifier { get; set; }

    public string? FacilityName { get; set; }

    public string? FormattedFacilityName { get; set; }

    public string? FacilitySubType { get; set; }

    public string? FacilitySubTypeDesc { get; set; }

    public string? ExperimentalConfidential { get; set; }

    public DateTime? FacilityStartDate { get; set; }

    public string? FacilityLocation { get; set; }

    public string? FacilityLegalSubdivision { get; set; }

    public int? FacilitySection { get; set; }

    public int? FacilityTownship { get; set; }

    public int? FacilityRange { get; set; }

    public int? FacilityMeridian { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? FacilityLicenceStatus { get; set; }

    public string? FacilityOperationalStatus { get; set; }

    public DateTime? FacilityOperationalStatusDate { get; set; }

    public string? LicenceType { get; set; }

    public string? LicenceNumber { get; set; }

    public string? EnergyDevelopmentCategoryType { get; set; }

    public DateTime? LicenceIssueDate { get; set; }

    public string? LicenseeBaid { get; set; }

    public string? LicenseeName { get; set; }

    public string? OperatorBaid { get; set; }

    public string? OperatorName { get; set; }

    public DateTime? OperatorStartDate { get; set; }

    public string? TerminalPipelineLink { get; set; }

    public string? TpfacilityProvinceState { get; set; }

    public string? TpfacilityType { get; set; }

    public string? TpfacilityIdentifier { get; set; }

    public string? MeterStationPipelineLink { get; set; }

    public string? MpfacilityProvinceState { get; set; }

    public string? MpfacilityType { get; set; }

    public string? MpfacilityIdentifier { get; set; }

    public string? EnergyDevelopmentCategoryId { get; set; }

    public string? OrphanWellFlg { get; set; }

    public string? TierAggregateId { get; set; }

    public string? TierAggregatePr { get; set; }
}
