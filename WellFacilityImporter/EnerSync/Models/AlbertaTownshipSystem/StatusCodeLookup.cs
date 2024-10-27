using System;
using System.Collections.Generic;

namespace EnerSync.Models.AlbertaTownshipSystem;

public partial class StatusCodeLookup
{
    public string StatusCode { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Coordinate> Coordinates { get; set; } = new List<Coordinate>();
}
