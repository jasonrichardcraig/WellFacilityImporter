using System;
using System.Collections.Generic;

namespace EnerSync.Models.WellFacilityLink;

public partial class WellFacilityLink
{
    public string WellId { get; set; } = null!;

    public string? WellProvinceState { get; set; }

    public string? WellType { get; set; }

    public string? WellIdentifier { get; set; }

    public string? WellLocationException { get; set; }

    public string? WellLegalSubdivision { get; set; }

    public int? WellSection { get; set; }

    public int? WellTownship { get; set; }

    public int? WellRange { get; set; }

    public int? WellMeridian { get; set; }

    public int? WellEventSequence { get; set; }

    public string? WellName { get; set; }

    public string? WellStatusFluid { get; set; }

    public string? WellStatusMode { get; set; }

    public string? WellStatusType { get; set; }

    public string? WellStatusStructure { get; set; }

    public string? WellStatusFluidCode { get; set; }

    public string? WellStatusModeCode { get; set; }

    public string? WellStatusTypeCode { get; set; }

    public string? WellStatusStructureCode { get; set; }

    public DateOnly? WellStatusStartDate { get; set; }

    public virtual ICollection<LinkedFacility> LinkedFacilities { get; set; } = new List<LinkedFacility>();
}
