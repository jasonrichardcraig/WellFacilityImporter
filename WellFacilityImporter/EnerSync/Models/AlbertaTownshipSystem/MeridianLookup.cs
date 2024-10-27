using System;
using System.Collections.Generic;

namespace EnerSync.Models.AlbertaTownshipSystem;

public partial class MeridianLookup
{
    public int MeridianId { get; set; }

    public int MeridianValue { get; set; }

    public virtual ICollection<Coordinate> Coordinates { get; set; } = new List<Coordinate>();
}
