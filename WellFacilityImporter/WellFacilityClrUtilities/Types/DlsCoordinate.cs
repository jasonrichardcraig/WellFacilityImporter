using System;
using System.IO;
using WellFacilityClrUtilities.Functions;

[Serializable]
public struct DlsCoordinate
{
    public bool IsNull { get; set; }
    public int Lsd { get; set; }
    public int Section { get; set; }
    public int Township { get; set; }
    public int Range { get; set; }
    public int Meridian { get; set; }
    public MeridianDirection Direction { get; set; }

    // Constructor
    public DlsCoordinate(bool isNull, int lsd, int section, int township, int range, int meridian, MeridianDirection direction)
    {
        IsNull = isNull;
        Lsd = lsd;
        Section = section;
        Township = township;
        Range = range;
        Meridian = meridian;
        Direction = direction;
    }

    // Null instance
    public static DlsCoordinate Null => new DlsCoordinate(true, 0, 0, 0, 0, 0, MeridianDirection.East);

    // ToString method
    public override string ToString()
    {
        if (IsNull)
            return "NULL";

        string meridianPrefix = Direction == MeridianDirection.West ? "W" : "E";
        // Format: LSD-Section-Township-RangeMeridian (e.g., 07-02-047-07W5)
        return $"{Lsd:D2}-{Section:D2}-{Township:D3}-{Range:D2}{meridianPrefix}{Meridian}";
    }

    // Parse method
    public static DlsCoordinate Parse(string s)
    {
        if (string.IsNullOrEmpty(s))
            return Null;

        // Expected format: LSD-Section-Township-RangeMeridian (e.g., 07-02-047-07W5)
        string[] parts = s.Split('-');
        if (parts.Length != 4)
            throw new ArgumentException("Invalid DLS coordinate format. Expected format: LSD-Section-Township-RangeMeridian (e.g., 07-02-047-07W5).");

        // Extract Range and Meridian from the fourth part (e.g., "07W5")
        string rangeMeridianPart = parts[3];
        // Assuming Meridian is prefixed with 'W' or 'E'
        int meridianIndex = rangeMeridianPart.IndexOfAny(new char[] { 'W', 'E' });
        if (meridianIndex == -1)
            throw new ArgumentException("Invalid Meridian format. Expected 'W' or 'E' followed by an integer (e.g., W5).");

        string rangePart = rangeMeridianPart.Substring(0, meridianIndex);
        string meridianPrefix = rangeMeridianPart.Substring(meridianIndex, 1);
        string meridianPart = rangeMeridianPart.Substring(meridianIndex + 1);

        if (!int.TryParse(rangePart, out int rangeValue))
            throw new ArgumentException("Invalid Range format. Expected an integer (e.g., 07).");

        if (!int.TryParse(meridianPart, out int meridianValue))
            throw new ArgumentException("Invalid Meridian format. Expected an integer after 'W' or 'E' (e.g., W5).");

        MeridianDirection direction = meridianPrefix == "W" ? MeridianDirection.West : MeridianDirection.East;

        return new DlsCoordinate(false, int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), rangeValue, meridianValue, direction);
    }

    // Serialization methods (optional)
    public void Write(BinaryWriter writer)
    {
        writer.Write(IsNull);
        writer.Write(Lsd);
        writer.Write(Section);
        writer.Write(Township);
        writer.Write(Range);
        writer.Write(Meridian);
        writer.Write((int)Direction);
    }

    public void Read(BinaryReader reader)
    {
        // Implement if needed
    }
}
