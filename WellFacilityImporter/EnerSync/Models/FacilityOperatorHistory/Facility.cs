using System;
using System.Collections.Generic;

namespace EnerSync.Models.FacilityOperatorHistory;

public partial class Facility
{
    public string FacilityId { get; set; } = null!;

    public string? FacilityProvinceState { get; set; }

    public string? FacilityType { get; set; }

    public string? FacilityIdentifier { get; set; }

    public string? FacilityName { get; set; }

    public string? FacilitySubType { get; set; }

    public string? FacilitySubTypeDesc { get; set; }

    public virtual ICollection<OperatorHistory> OperatorHistories { get; set; } = new List<OperatorHistory>();
}
