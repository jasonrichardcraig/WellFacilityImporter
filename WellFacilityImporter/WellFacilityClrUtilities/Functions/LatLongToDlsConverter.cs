// LatLongToDlsConverter.cs
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace WellFacilityClrUtilities.Functions
{
    public class LatLongToDlsConverter
    {
        // Constants representing the sizes of sections in miles
        private const double SectionSizeMiles = 1.0;          // Each Section is approximately 1 mile square
        private const double QuarterSectionWidthMiles = 0.5;  // Each Quarter Section is 1/2 mile square
        private const double LsdWidthMiles = 0.25;            // Each LSD is 1/4 mile square
        private const double DegreesPerMileLatitude = 1.0 / 69.172; // Degrees per mile for latitude

        // Database connection string
        private const string ConnectionString = "Data Source=(local);Initial Catalog=WellFacilityRepository;Integrated Security=True;Encrypt=False";

        // Define LSD offsets for 4x4 grid (Row0 to Row3, Column0 to Column3)
        private static readonly List<LsdPosition> LsdPositions = new List<LsdPosition>
        {
            // Row0 (North)
            new LsdPosition { LsdNumber = 16, LatOffset = -0.375, LongOffset = -0.375 },
            new LsdPosition { LsdNumber = 15, LatOffset = -0.375, LongOffset = -0.125 },
            new LsdPosition { LsdNumber = 14, LatOffset = -0.375, LongOffset = +0.125 },
            new LsdPosition { LsdNumber = 13, LatOffset = -0.375, LongOffset = +0.375 },

            // Row1
            new LsdPosition { LsdNumber = 9, LatOffset = -0.125, LongOffset = -0.375 },
            new LsdPosition { LsdNumber = 10, LatOffset = -0.125, LongOffset = -0.125 },
            new LsdPosition { LsdNumber = 11, LatOffset = -0.125, LongOffset = +0.125 },
            new LsdPosition { LsdNumber = 12, LatOffset = -0.125, LongOffset = +0.375 },

            // Row2
            new LsdPosition { LsdNumber = 4, LatOffset = +0.125, LongOffset = -0.375 },
            new LsdPosition { LsdNumber = 3, LatOffset = +0.125, LongOffset = -0.125 },
            new LsdPosition { LsdNumber = 2, LatOffset = +0.125, LongOffset = +0.125 },
            new LsdPosition { LsdNumber = 1, LatOffset = +0.125, LongOffset = +0.375 },

            // Row3 (South)
            new LsdPosition { LsdNumber = 5, LatOffset = +0.375, LongOffset = -0.375 },
            new LsdPosition { LsdNumber = 6, LatOffset = +0.375, LongOffset = -0.125 },
            new LsdPosition { LsdNumber = 7, LatOffset = +0.375, LongOffset = +0.125 },
            new LsdPosition { LsdNumber = 8, LatOffset = +0.375, LongOffset = +0.375 }
        };

        /// <summary>
        /// Converts geographic coordinates (latitude and longitude) to DLS coordinates.
        /// </summary>
        /// 
        public static DlsCoordinate ConvertLatLongToDls(double latitude, double longitude)
        {
            // Validate input parameters
            if (latitude < -90.0 || latitude > 90.0)
            {
                Console.WriteLine("Invalid latitude value. Must be between -90 and 90 degrees.");
                return DlsCoordinate.Null;
            }
            if (longitude < -180.0 || longitude > 180.0)
            {
                Console.WriteLine("Invalid longitude value. Must be between -180 and 180 degrees.");
                return DlsCoordinate.Null;
            }

            // Variables to store the identified Section's components
            int identifiedMeridian = 0;
            int identifiedRange = 0;
            int identifiedTownship = 0;
            int identifiedSection = 0;
            string identifiedQuarterSection = "";

            // Variables to track the closest Section
            bool sectionFound = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    // Step 1: Identify the closest Section
                    string queryClosestSection = @"
                        SELECT TOP 1 Meridian, Range, Township, Section, QuarterSection, Latitude, Longitude
                        FROM [AlbertaTownshipSystem].[Coordinates]
                        ORDER BY Location.STDistance(geography::Point(@Latitude, @Longitude, 4326)) ASC";

                    using (SqlCommand cmd = new SqlCommand(queryClosestSection, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Latitude", System.Data.SqlDbType.Float) { Value = latitude });
                        cmd.Parameters.Add(new SqlParameter("@Longitude", System.Data.SqlDbType.Float) { Value = longitude });

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                identifiedMeridian = reader.GetInt32(0);
                                identifiedRange = reader.GetInt32(1);
                                identifiedTownship = reader.GetInt32(2);
                                identifiedSection = reader.GetInt32(3);
                                identifiedQuarterSection = reader.GetString(4).ToUpper();
                                double baseLatitude = reader.GetDouble(5);
                                double baseLongitude = reader.GetDouble(6);

                                // Calculate distance using Haversine formula for logging
                                double distance = HaversineDistance(latitude, longitude, baseLatitude, baseLongitude);

                                Console.WriteLine($"Closest Section: {identifiedMeridian}-{identifiedRange}-{identifiedTownship}-{identifiedSection} | Distance: {distance:F4} miles");

                                sectionFound = true;

                                // Assign base coordinates for offset calculations
                                // Assuming base coordinates are the same as the Section's
                            }
                        }
                    }

                    if (!sectionFound)
                    {
                        Console.WriteLine("No matching Section found.");
                        return DlsCoordinate.Null;
                    }

                    // Step 2: Retrieve the base coordinates of the identified Section
                    double baseLatitudeSection = 0.0;
                    double baseLongitudeSection = 0.0;

                    string queryBaseCoordinates = @"
                        SELECT Latitude, Longitude
                        FROM [AlbertaTownshipSystem].[Coordinates]
                        WHERE Meridian = @Meridian
                          AND [Range] = @Range
                          AND Township = @Township
                          AND Section = @Section
                          AND QuarterSection = @QuarterSection";

                    using (SqlCommand cmdBase = new SqlCommand(queryBaseCoordinates, conn))
                    {
                        cmdBase.Parameters.Add(new SqlParameter("@Meridian", System.Data.SqlDbType.Int) { Value = identifiedMeridian });
                        cmdBase.Parameters.Add(new SqlParameter("@Range", System.Data.SqlDbType.Int) { Value = identifiedRange });
                        cmdBase.Parameters.Add(new SqlParameter("@Township", System.Data.SqlDbType.Int) { Value = identifiedTownship });
                        cmdBase.Parameters.Add(new SqlParameter("@Section", System.Data.SqlDbType.Int) { Value = identifiedSection });
                        cmdBase.Parameters.Add(new SqlParameter("@QuarterSection", System.Data.SqlDbType.VarChar) { Value = identifiedQuarterSection });

                        using (SqlDataReader readerBase = cmdBase.ExecuteReader())
                        {
                            if (readerBase.Read())
                            {
                                baseLatitudeSection = readerBase.GetDouble(0);
                                baseLongitudeSection = readerBase.GetDouble(1);
                                Console.WriteLine($"Base Coordinates for Section {identifiedMeridian}-{identifiedRange}-{identifiedTownship}-{identifiedSection}: Latitude={baseLatitudeSection}, Longitude={baseLongitudeSection}");
                            }
                            else
                            {
                                Console.WriteLine("Base coordinates not found for the identified Section.");
                                return DlsCoordinate.Null;
                            }
                        }
                    }

                    // Step 3: Calculate the offset within the Section
                    double degreesPerMileLongitude = DegreesPerMileLongitude(baseLatitudeSection);

                    // Calculate the difference in degrees
                    double deltaLatDegrees = latitude - baseLatitudeSection;
                    double deltaLongDegrees = longitude - baseLongitudeSection;

                    // Convert degree differences to miles
                    double deltaLatMiles = deltaLatDegrees / DegreesPerMileLatitude;
                    double deltaLongMiles = deltaLongDegrees / degreesPerMileLongitude;

                    Console.WriteLine($"Offset Miles from Section Base: Latitude Offset={deltaLatMiles:F4} miles, Longitude Offset={deltaLongMiles:F4} miles");

                    // Step 4: Determine Quarter Section and residuals
                    string determinedQuarterSection = DetermineQuarterSection(deltaLatMiles, deltaLongMiles);
                    Console.WriteLine($"Determined Quarter Section: {determinedQuarterSection}");

                    double residualLatMiles = deltaLatMiles - GetQuarterSectionLatShift(determinedQuarterSection);
                    double residualLongMiles = deltaLongMiles - GetQuarterSectionLongShift(determinedQuarterSection);

                    Console.WriteLine($"Residual Miles within Quarter Section: Latitude Residual={residualLatMiles:F4} miles, Longitude Residual={residualLongMiles:F4} miles");

                    // Step 5: Calculate Quarter Section Center Coordinates
                    double quarterSectionCenterLat = baseLatitudeSection + GetQuarterSectionLatShift(determinedQuarterSection) * DegreesPerMileLatitude;
                    double quarterSectionCenterLong = baseLongitudeSection + GetQuarterSectionLongShift(determinedQuarterSection) * DegreesPerMileLongitude(baseLatitudeSection);

                    // Step 6: Fan-Out Search to Find Closest LSD Across Entire Township and Range
                    int determinedLsd = FindClosestLsdAcrossTownshipAndRange(conn, identifiedTownship, identifiedRange, latitude, longitude);

                    Console.WriteLine($"Determined LSD: {determinedLsd}");

                    // Step 7: Determine Meridian Direction based on original longitude
                    MeridianDirection direction = longitude >= 0 ? MeridianDirection.East : MeridianDirection.West;
                    int meridianValue = Math.Abs(identifiedMeridian); // Assuming meridian is stored as positive integer

                    // Step 8: Populate the DlsCoordinate
                    DlsCoordinate dlsCoordinate = new DlsCoordinate
                    {
                        Lsd = determinedLsd, // Integer between 1-16
                        Section = identifiedSection,
                        Township = identifiedTownship,
                        Range = identifiedRange,
                        Meridian = meridianValue,
                        Direction = direction
                    };

                    return dlsCoordinate;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Exception: {ex.Message}");
                return DlsCoordinate.Null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Exception: {ex.Message}");
                return DlsCoordinate.Null;
            }
        }

        /// <summary>
        /// Finds the closest LSD across the entire township and range.
        /// </summary>
        private static int FindClosestLsdAcrossTownshipAndRange(SqlConnection conn, int township, int range, double inputLatitude, double inputLongitude)
        {
            // Retrieve all sections within the same township and range
            string queryAllSections = @"
                SELECT Meridian, Range, Township, Section, QuarterSection, Latitude, Longitude
                FROM [AlbertaTownshipSystem].[Coordinates]
                WHERE Township = @Township
                  AND Range = @Range";

            List<(int Meridian, int Range, int Township, int Section, string QuarterSection, double Latitude, double Longitude)> sections = new List<(int, int, int, int, string, double, double)>();

            using (SqlCommand cmdSections = new SqlCommand(queryAllSections, conn))
            {
                cmdSections.Parameters.Add(new SqlParameter("@Township", System.Data.SqlDbType.Int) { Value = township });
                cmdSections.Parameters.Add(new SqlParameter("@Range", System.Data.SqlDbType.Int) { Value = range });

                using (SqlDataReader readerSections = cmdSections.ExecuteReader())
                {
                    while (readerSections.Read())
                    {
                        sections.Add((
                            readerSections.GetInt32(0),
                            readerSections.GetInt32(1),
                            readerSections.GetInt32(2),
                            readerSections.GetInt32(3),
                            readerSections.GetString(4).ToUpper(),
                            readerSections.GetDouble(5),
                            readerSections.GetDouble(6)
                        ));
                    }
                }
            }

            if (sections.Count == 0)
            {
                Console.WriteLine("No sections found within the specified township and range.");
                return 0; // or handle appropriately
            }

            double minDistance = double.MaxValue;
            int closestLsd = 0;

            // Iterate through each section and each LSD within it
            foreach (var section in sections)
            {
                // Retrieve base coordinates for the section
                double baseLat = section.Latitude;
                double baseLong = section.Longitude;

                // Determine Quarter Sections within the section
                List<string> quarterSections = GetAllQuarterSections();

                foreach (var quarterSection in quarterSections)
                {
                    // Calculate Quarter Section Center Coordinates
                    double degreesPerMileLongitude = DegreesPerMileLongitude(baseLat);
                    double quarterSectionCenterLat = baseLat + GetQuarterSectionLatShift(quarterSection) * DegreesPerMileLatitude;
                    double quarterSectionCenterLong = baseLong + GetQuarterSectionLongShift(quarterSection) * degreesPerMileLongitude;

                    // Iterate through all LSDs in the quarter section
                    foreach (var lsd in LsdPositions)
                    {
                        // Calculate LSD Center Coordinates
                        double lsdCenterLat = quarterSectionCenterLat + (lsd.LatOffset * DegreesPerMileLatitude);
                        double lsdCenterLong = quarterSectionCenterLong + (lsd.LongOffset * DegreesPerMileLongitude(baseLong));

                        // Calculate distance from input coordinates to LSD center
                        double distance = HaversineDistance(inputLatitude, inputLongitude, lsdCenterLat, lsdCenterLong);

                        // Update closest LSD if necessary
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestLsd = lsd.LsdNumber;
                        }
                    }
                }
            }

            return closestLsd;
        }

        /// <summary>
        /// Retrieves all possible quarter sections.
        /// </summary>
        private static List<string> GetAllQuarterSections()
        {
            return new List<string> { "NE", "NW", "SE", "SW" };
        }

        /// <summary>
        /// Calculates degrees per mile of longitude at a given latitude.
        /// </summary>
        private static double DegreesPerMileLongitude(double latitude)
        {
            // Adjust for latitude by using the cosine of latitude in radians
            return 1.0 / (69.172 * Math.Cos(ToRadians(latitude)));
        }

        /// <summary>
        /// Calculates the Haversine distance between two geographic points.
        /// </summary>
        private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 3958.8; // Radius of Earth in miles
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Pow(Math.Sin(dLon / 2), 2);

            double c = 2 * Math.Asin(Math.Sqrt(a));

            return R * c;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        private static double ToRadians(double deg)
        {
            return deg * (Math.PI / 180.0);
        }

        /// <summary>
        /// Determines the Quarter Section based on latitude and longitude offsets.
        /// </summary>
        private static string DetermineQuarterSection(double deltaLatMiles, double deltaLongMiles)
        {
            if (deltaLatMiles > QuarterSectionWidthMiles / 2.0)
            {
                if (deltaLongMiles > QuarterSectionWidthMiles / 2.0)
                    return "NE";
                else if (deltaLongMiles < -QuarterSectionWidthMiles / 2.0)
                    return "NW";
                else
                    return "N4"; // North midpoint
            }
            else if (deltaLatMiles < -QuarterSectionWidthMiles / 2.0)
            {
                if (deltaLongMiles > QuarterSectionWidthMiles / 2.0)
                    return "SE";
                else if (deltaLongMiles < -QuarterSectionWidthMiles / 2.0)
                    return "SW";
                else
                    return "S4"; // South midpoint
            }
            else
            {
                if (deltaLongMiles > QuarterSectionWidthMiles / 2.0)
                    return "E4"; // East midpoint
                else if (deltaLongMiles < -QuarterSectionWidthMiles / 2.0)
                    return "W4"; // West midpoint
                else
                    return "CS"; // Center Section
            }
        }

        /// <summary>
        /// Helper method to get Quarter Section latitude shift.
        /// </summary>
        private static double GetQuarterSectionLatShift(string quarterSection)
        {
            switch (quarterSection)
            {
                case "NE":
                case "NW":
                    return QuarterSectionWidthMiles / 2.0;
                case "SE":
                case "SW":
                    return -QuarterSectionWidthMiles / 2.0;
                case "N4":
                    return QuarterSectionWidthMiles;
                case "S4":
                    return -QuarterSectionWidthMiles;
                case "E4":
                case "W4":
                case "CS":
                default:
                    return 0.0;
            }
        }

        /// <summary>
        /// Helper method to get Quarter Section longitude shift.
        /// </summary>
        private static double GetQuarterSectionLongShift(string quarterSection)
        {
            switch (quarterSection)
            {
                case "NE":
                case "SE":
                    return QuarterSectionWidthMiles / 2.0;
                case "NW":
                case "SW":
                    return -QuarterSectionWidthMiles / 2.0;
                case "E4":
                    return QuarterSectionWidthMiles;
                case "W4":
                    return -QuarterSectionWidthMiles;
                case "N4":
                case "S4":
                case "CS":
                default:
                    return 0.0;
            }
        }
    }
    public class LsdPosition
    {
        public int LsdNumber { get; set; }
        public double LatOffset { get; set; }  // Offset in miles from quarter section center
        public double LongOffset { get; set; } // Offset in miles from quarter section center
    }
    public enum MeridianDirection
    {
        East,
        West
    }
}
