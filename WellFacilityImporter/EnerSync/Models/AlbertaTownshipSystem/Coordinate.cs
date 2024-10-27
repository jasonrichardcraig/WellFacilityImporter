using System;
using System.Collections.Generic;

namespace EnerSync.Models.AlbertaTownshipSystem;

public partial class Coordinate
{
    public int Id { get; set; }

    public int Meridian { get; set; }

    public int Range { get; set; }

    public int Township { get; set; }

    public int Section { get; set; }

    public string QuarterSection { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public int? YearComputed { get; set; }

    public int? MonthComputed { get; set; }

    public int? DayComputed { get; set; }

    public string? StationCode { get; set; }

    public string? StatusCode { get; set; }

    public string? HorizontalClassification { get; set; }

    public string? CommentField { get; set; }

    public string? HorizontalOrigin { get; set; }

    public string? HorizontalMethod { get; set; }

    public string? HorizontalDatum { get; set; }

    public string? RoadAllowanceCode { get; set; }

    public double? Elevation { get; set; }

    public DateOnly? ElevationDate { get; set; }

    public string? ElevationOrigin { get; set; }

    public string? ElevationMethod { get; set; }

    public string? ElevationAccuracy { get; set; }

    public string? VerticalDatum { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ElevationAccuracyLookup? ElevationAccuracyNavigation { get; set; }

    public virtual ElevationMethodLookup? ElevationMethodNavigation { get; set; }

    public virtual HorizontalClassificationLookup? HorizontalClassificationNavigation { get; set; }

    public virtual HorizontalDatumLookup? HorizontalDatumNavigation { get; set; }

    public virtual MeridianLookup MeridianNavigation { get; set; } = null!;

    public virtual RoadAllowanceCodeLookup? RoadAllowanceCodeNavigation { get; set; }

    public virtual StationCodeLookup? StationCodeNavigation { get; set; }

    public virtual StatusCodeLookup? StatusCodeNavigation { get; set; }

    public virtual VerticalDatumLookup? VerticalDatumNavigation { get; set; }
}
