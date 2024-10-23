using System;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;

/// <summary>
/// Represents a DLS Coordinate as a SQL CLR User-Defined Type without Direction.
/// </summary>
[Serializable]
[SqlUserDefinedType(Format.UserDefined, IsByteOrdered = true, MaxByteSize = 40)]
public struct DLS : INullable, IBinarySerialize
{
    private bool isNull;
    public bool IsNull
    {
        get { return isNull; }
    }

    public int Lsd { get; set; }
    public int Section { get; set; }
    public int Township { get; set; }
    public int Range { get; set; }
    public int Meridian { get; set; }

    // Null instance
    public static DLS Null
    {
        get
        {
            DLS dc = new DLS();
            dc.isNull = true;
            return dc;
        }
    }

    // Constructor
    public DLS(bool isNull, int lsd, int section, int township, int range, int meridian)
    {
        this.isNull = isNull;
        this.Lsd = lsd;
        this.Section = section;
        this.Township = township;
        this.Range = range;
        this.Meridian = meridian;
    }

    // ToString method
    public override string ToString()
    {
        if (IsNull)
            return "NULL";

        // Format: LSD-Section-Township-RangeMeridian (e.g., 07-02-047-07W5)
        return $"{Lsd:D2}-{Section:D2}-{Township:D3}-{Range:D2}W{Meridian}";
    }

    // Parse method
    public static DLS Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;

        string str = s.Value;
        if (string.IsNullOrEmpty(str))
            return Null;

        // Expected format: LSD-Section-Township-RangeMeridian (e.g., 07-02-047-07E5)
        string[] parts = str.Split('-');
        if (parts.Length != 4)
            throw new ArgumentException("Invalid DLS coordinate format. Expected format: LSD-Section-Township-RangeMeridian (e.g., 07-02-047-07E5).");

        // Extract Range and Meridian from the fourth part (e.g., "07E5")
        string rangeMeridianPart = parts[3];
        // Assuming Meridian is prefixed with 'E'
        if (!rangeMeridianPart.StartsWith("E"))
            throw new ArgumentException("Invalid Meridian format. Expected 'E' followed by an integer (e.g., E5).");

        string rangePart = rangeMeridianPart.Substring(0, 2); // "07"
        string meridianPart = rangeMeridianPart.Substring(2);   // "5"

        if (!int.TryParse(rangePart, out int rangeValue))
            throw new ArgumentException("Invalid Range format. Expected an integer (e.g., 07).");

        if (!int.TryParse(meridianPart, out int meridianValue))
            throw new ArgumentException("Invalid Meridian format. Expected an integer after 'E' (e.g., E5).");

        int lsd = int.Parse(parts[0]);
        int section = int.Parse(parts[1]);
        int township = int.Parse(parts[2]);

        return new DLS(false, lsd, section, township, rangeValue, meridianValue);
    }

    // Serialization: Deserialize from binary
    public void Read(BinaryReader r)
    {
        isNull = r.ReadBoolean();
        if (!isNull)
        {
            Lsd = r.ReadInt32();
            Section = r.ReadInt32();
            Township = r.ReadInt32();
            Range = r.ReadInt32();
            Meridian = r.ReadInt32();
        }
    }

    // Serialization: Serialize to binary
    public void Write(BinaryWriter w)
    {
        w.Write(isNull);
        if (!isNull)
        {
            w.Write(Lsd);
            w.Write(Section);
            w.Write(Township);
            w.Write(Range);
            w.Write(Meridian);
        }
    }

    // Override Equals and GetHashCode for proper comparison in tests
    public override bool Equals(object obj)
    {
        if (obj is DLS other)
        {
            return this.Lsd == other.Lsd &&
                   this.Section == other.Section &&
                   this.Township == other.Township &&
                   this.Range == other.Range &&
                   this.Meridian == other.Meridian &&
                   this.IsNull == other.IsNull;
        }
        return false;
    }

    public override int GetHashCode()
    {
        // Custom hash code implementation for compatibility
        unchecked // Overflow is fine
        {
            int hash = 17;
            hash = hash * 23 + Lsd.GetHashCode();
            hash = hash * 23 + Section.GetHashCode();
            hash = hash * 23 + Township.GetHashCode();
            hash = hash * 23 + Range.GetHashCode();
            hash = hash * 23 + Meridian.GetHashCode();
            hash = hash * 23 + isNull.GetHashCode();
            return hash;
        }
    }
}

