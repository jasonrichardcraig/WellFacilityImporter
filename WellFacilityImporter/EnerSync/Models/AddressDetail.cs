using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnerSync.Models
{
    public class AddressDetail : INullable, IBinarySerialize
    {
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? County { get; set; }
        public string? City { get; set; }
        public string? TownBorough { get; set; }
        public string? VillageSuburb { get; set; }
        public string? Neighbourhood { get; set; }
        public string? AnySettlement { get; set; }
        public string? MajorStreets { get; set; }
        public string? MajorMinorStreets { get; set; }
        public string? Building { get; set; }

        private bool isNull;

        public bool IsNull
        {
            get { return isNull; }
        }

        public static AddressDetail Null
        {
            get
            {
                AddressDetail addr = new AddressDetail();
                addr.isNull = true;
                return addr;
            }
        }

        public static AddressDetail Parse(SqlString s)
        {
            if (s.IsNull)
                return Null;

            // Example string parsing (modify according to your needs)
            string[] parts = s.Value.Split(',');

            if (parts.Length != 11)
                throw new ArgumentException("Invalid input. Expecting format: 'Country,State,County,City,TownBorough,VillageSuburb,Neighbourhood,AnySettlement,MajorStreets,MajorMinorStreets,Building'");

            return new AddressDetail
            {
                Country = parts[0].Trim(),
                State = parts[1].Trim(),
                County = parts[2].Trim(),
                City = parts[3].Trim(),
                TownBorough = parts[4].Trim(),
                VillageSuburb = parts[5].Trim(),
                Neighbourhood = parts[6].Trim(),
                AnySettlement = parts[7].Trim(),
                MajorStreets = parts[8].Trim(),
                MajorMinorStreets = parts[9].Trim(),
                Building = parts[10].Trim()
            };
        }

        public override string ToString()
        {
            if (isNull)
                return "NULL";
            return $"{Country}, {State}, {County}, {City}, {TownBorough}, {VillageSuburb}, {Neighbourhood}, {AnySettlement}, {MajorStreets}, {MajorMinorStreets}, {Building}";
        }

        public void Read(BinaryReader r)
        {
            Country = r.ReadString();
            State = r.ReadString();
            County = r.ReadString();
            City = r.ReadString();
            TownBorough = r.ReadString();
            VillageSuburb = r.ReadString();
            Neighbourhood = r.ReadString();
            AnySettlement = r.ReadString();
            MajorStreets = r.ReadString();
            MajorMinorStreets = r.ReadString();
            Building = r.ReadString();
        }

        public void Write(BinaryWriter w)
        {
            w.Write(Country ?? string.Empty);
            w.Write(State ?? string.Empty);
            w.Write(County ?? string.Empty);
            w.Write(City ?? string.Empty);
            w.Write(TownBorough ?? string.Empty);
            w.Write(VillageSuburb ?? string.Empty);
            w.Write(Neighbourhood ?? string.Empty);
            w.Write(AnySettlement ?? string.Empty);
            w.Write(MajorStreets ?? string.Empty);
            w.Write(MajorMinorStreets ?? string.Empty);
            w.Write(Building ?? string.Empty);
        }
    }
}
