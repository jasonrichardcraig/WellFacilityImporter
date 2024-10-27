using System;
using System.Collections.Generic;

namespace EnerSync.Models.BusinessAssociate;

public partial class BusinessAssociate
{
    public string Baidentifier { get; set; } = null!;

    public string? BalegalName { get; set; }

    public string? Baaddress { get; set; }

    public string? BaphoneNumber { get; set; }

    public string? BacorporateStatus { get; set; }

    public DateTime? BacorporateStatusEffectiveDate { get; set; }

    public string? AmalgamatedIntoBaid { get; set; }

    public string? AmalgamatedIntoBalegalName { get; set; }

    public DateTime? BaamalgamationEstablishedDate { get; set; }

    public string? BalicenceEligibilityType { get; set; }

    public string? BalicenceEligibiltyDesc { get; set; }

    public string? BaabbreviatedName { get; set; }
}
