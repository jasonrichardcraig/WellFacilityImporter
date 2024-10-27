using System;
using System.Collections.Generic;

namespace EnerSync.Models.WellInfrastructure;

public partial class LinkedFacility
{
    public string LinkedFacilityId { get; set; } = null!;

    public string WellId { get; set; } = null!;

    public string? LinkedFacilityProvinceState { get; set; }

    public string? LinkedFacilityType { get; set; }

    public string? LinkedFacilityIdentifier { get; set; }

    public string? LinkedFacilityName { get; set; }

    public string? LinkedFacilitySubType { get; set; }

    public string? LinkedFacilitySubTypeDesc { get; set; }

    public DateOnly? LinkedStartDate { get; set; }

    public string? LinkedFacilityOperatorBaid { get; set; }

    public string? LinkedFacilityOperatorLegalName { get; set; }

    public virtual Well Well { get; set; } = null!;
}
