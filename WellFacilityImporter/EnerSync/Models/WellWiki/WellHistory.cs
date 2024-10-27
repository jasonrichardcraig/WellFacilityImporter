using System;
using System.Collections.Generic;

namespace EnerSync.Models.WellWiki;

public partial class WellHistory
{
    public int Id { get; set; }

    public string WellId { get; set; } = null!;

    public DateOnly? Date { get; set; }

    public string? Event { get; set; }

    public virtual Well Well { get; set; } = null!;
}
