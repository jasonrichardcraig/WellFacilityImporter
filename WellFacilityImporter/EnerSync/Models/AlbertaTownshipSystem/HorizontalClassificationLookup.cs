using System;
using System.Collections.Generic;

namespace EnerSync.Models.AlbertaTownshipSystem;

public partial class HorizontalClassificationLookup
{
    public string HorizontalClassification { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Coordinate> Coordinates { get; set; } = new List<Coordinate>();
}
