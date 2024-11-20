using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnerSync.Models;

public partial class Well
{
    public string WellId { get; set; } = null!;

    public string? WellProvinceState { get; set; }

    public string? WellType { get; set; }

    public string? WellIdentifier { get; set; }

    public string? FormattedWellIdentifier { get; set; }

    public string? PreviousWellId { get; set; }

    public string? WellLocationException { get; set; }

    public string? WellLegalSubdivision { get; set; }

    public int? WellSection { get; set; }

    public int? WellTownship { get; set; }

    public int? WellRange { get; set; }

    public int? WellMeridian { get; set; }

    public int? WellEventSequence { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? WellName { get; set; }

    public string? FormattedWellName { get; set; }

    public string? ConfidentialType { get; set; }

    public string? ExperimentalConfidentialIndicator { get; set; }

    public DateOnly? ExperimentalConfidentialEffDate { get; set; }

    public DateOnly? ExperimentalConfidentialTermDate { get; set; }

    public string? LicenceType { get; set; }

    public string? LicenceNumber { get; set; }

    public DateOnly? LicenceIssueDate { get; set; }

    public DateOnly? LicenceStatusDate { get; set; }

    public string? LicenceStatus { get; set; }

    public string? Field { get; set; }

    public string? FieldName { get; set; }

    public string? FormattedFieldName { get; set; }

    public string? Area { get; set; }

    public string? AreaName { get; set; }

    public string? PoolDeposit { get; set; }

    public string? PoolDepositName { get; set; }

    public string? FormattedPoolDepositName { get; set; }

    public decimal? PoolDepositDensity { get; set; }

    public string? WellStatusFluid { get; set; }

    public string? WellStatusMode { get; set; }

    public string? WellStatusType { get; set; }

    public string? WellStatusStructure { get; set; }

    public string? WellStatusFluidCode { get; set; }

    public string? WellStatusModeCode { get; set; }

    public string? WellStatusTypeCode { get; set; }

    public string? WellStatusStructureCode { get; set; }

    public DateOnly? WellStatusDate { get; set; }

    public DateOnly? SpudDate { get; set; }

    public string? HorizontalDrill { get; set; }

    public DateOnly? FinishedDrillDate { get; set; }

    public decimal? FinalTotalDepth { get; set; }

    public decimal? MaxTrueVerticalDepth { get; set; }

    public string? VolumetricGasWellLiquidType { get; set; }

    public DateOnly? VolumetricGasWellLiquidEffDate { get; set; }

    public string? LicenseeId { get; set; }

    public string? LicenseeName { get; set; }

    public string? FormattedLicenseeName { get; set; }

    public string? AllowableType { get; set; }

    public int? BlockNumber { get; set; }

    public string? RecoveryMechanismType { get; set; }

    public string? OrphanWellFlg { get; set; }

    public bool WellWikiRowExists { get; set; }
}
