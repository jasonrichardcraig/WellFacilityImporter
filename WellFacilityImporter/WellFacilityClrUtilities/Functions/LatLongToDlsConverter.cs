using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

    public class LatLongToDlsConverter
    {
        // Constants representing the sizes of sections in miles
        private const double SectionSizeMiles = 1.0;          // Each Section is approximately 1 mile square
        private const double QuarterSectionWidthMiles = 0.5;  // Each Quarter Section is 1/2 mile square
        private const double LsdWidthMiles = 0.125;           // Each LSD is 1/8 mile square
        private const double DegreesPerMileLatitude = 1.0 / 69.172; // Degrees per mile for latitude

        // Database connection string
        private const string ConnectionString = "context connection=true";

        // Define LSD offsets for 4x4 grid (Row0 to Row3, Column1 to Column3)
        private static readonly List<LsdPosition> LsdPositions = new List<LsdPosition>
        {
            // Row0 (North) - Positive LatOffset
            new LsdPosition { LsdNumber = 1, LatOffset = +0.1875, LongOffset = -0.1875 },
            new LsdPosition { LsdNumber = 2, LatOffset = +0.1875, LongOffset = -0.0625 },
            new LsdPosition { LsdNumber = 3, LatOffset = +0.1875, LongOffset = +0.0625 },
            new LsdPosition { LsdNumber = 4, LatOffset = +0.1875, LongOffset = +0.1875 },

            // Row1
            new LsdPosition { LsdNumber = 5, LatOffset = +0.0625, LongOffset = -0.1875 },
            new LsdPosition { LsdNumber = 6, LatOffset = +0.0625, LongOffset = -0.0625 },
            new LsdPosition { LsdNumber = 7, LatOffset = +0.0625, LongOffset = +0.0625 },
            new LsdPosition { LsdNumber = 8, LatOffset = +0.0625, LongOffset = +0.1875 },

            // Row2
            new LsdPosition { LsdNumber = 9, LatOffset = -0.0625, LongOffset = -0.1875 },
            new LsdPosition { LsdNumber = 10, LatOffset = -0.0625, LongOffset = -0.0625 },
            new LsdPosition { LsdNumber = 11, LatOffset = -0.0625, LongOffset = +0.0625 },
            new LsdPosition { LsdNumber = 12, LatOffset = -0.0625, LongOffset = +0.1875 },

            // Row3 (South)
            new LsdPosition { LsdNumber = 13, LatOffset = -0.1875, LongOffset = -0.1875 },
            new LsdPosition { LsdNumber = 14, LatOffset = -0.1875, LongOffset = -0.0625 },
            new LsdPosition { LsdNumber = 15, LatOffset = -0.1875, LongOffset = +0.0625 },
            new LsdPosition { LsdNumber = 16, LatOffset = -0.1875, LongOffset = +0.1875 }
        };

    /// <summary>
    /// Converts geographic coordinates (latitude and longitude) to DLS coordinates.
    /// </summary>
    /// <param name="latitude">Latitude in degrees.</param>
    /// <param name="longitude">Longitude in degrees.</param>
    /// <returns>DlsCoordinate object representing the DLS coordinate.</returns>
    [SqlFunction(IsDeterministic = false, IsPrecise = false, DataAccess = DataAccessKind.Read)]
    public static DlsCoordinate ConvertLatLongToDls(double latitude, double longitude)
        {
            // Validate input parameters
            if (latitude < -90.0 || latitude > 90.0)
            {
                return DlsCoordinate.Null;
            }
            if (longitude < -180.0 || longitude > 180.0)
            {
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

                    // Step 1: Determine the Township based on Latitude
                    identifiedTownship = DetermineTownshipFromLatitude(latitude);

                    // Step 2: Identify the closest Section within the determined Township
                    string queryClosestSection = @"
                        SELECT TOP 1 Meridian, Range, Township, Section, QuarterSection, Latitude, Longitude
                        FROM [AlbertaTownshipSystem].[Coordinates]
                        WHERE Township = @Township
                        ORDER BY Location.STDistance(geography::Point(@Latitude, @Longitude, 4326)) ASC";

                    using (SqlCommand cmd = new SqlCommand(queryClosestSection, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Township", System.Data.SqlDbType.Int) { Value = identifiedTownship });
                        cmd.Parameters.Add(new SqlParameter("@Latitude", System.Data.SqlDbType.Float) { Value = latitude });
                        cmd.Parameters.Add(new SqlParameter("@Longitude", System.Data.SqlDbType.Float) { Value = longitude });

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                identifiedMeridian = reader.GetInt32(0);
                                identifiedRange = reader.GetInt32(1);
                                identifiedTownship = reader.GetInt32(2); // Reconfirming township
                                identifiedSection = reader.GetInt32(3);
                                identifiedQuarterSection = reader.GetString(4).ToUpper();
                                double baseLatitude = reader.GetDouble(5);
                                double baseLongitude = reader.GetDouble(6);

                                // Calculate distance using Haversine formula for logging
                                double distance = HaversineDistance(latitude, longitude, baseLatitude, baseLongitude);

                                sectionFound = true;
                            }
                        }
                    }

                    if (!sectionFound)
                    {
                        return DlsCoordinate.Null;
                    }

                    // Step 3: Calculate the offset within the Section
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
                            }
                            else
                            {
                                return DlsCoordinate.Null;
                            }
                        }
                    }

                    // Calculate degrees per mile longitude at the base latitude
                    double degreesPerMileLongitude = DegreesPerMileLongitude(baseLatitudeSection);

                    // Calculate the difference in degrees
                    double deltaLatDegrees = latitude - baseLatitudeSection;
                    double deltaLongDegrees = longitude - baseLongitudeSection;

                    // Convert degree differences to miles
                    double deltaLatMiles = deltaLatDegrees / DegreesPerMileLatitude;
                    double deltaLongMiles = deltaLongDegrees / degreesPerMileLongitude;

                    // Step 4: Determine Quarter Section and residuals
                    string determinedQuarterSection = DetermineQuarterSection(deltaLatMiles, deltaLongMiles);

                    double residualLatMiles = deltaLatMiles - GetQuarterSectionLatShift(determinedQuarterSection);
                    double residualLongMiles = deltaLongMiles - GetQuarterSectionLongShift(determinedQuarterSection);

                    // Step 5: Calculate Quarter Section Center Coordinates
                    double quarterSectionCenterLat = baseLatitudeSection + GetQuarterSectionLatShift(determinedQuarterSection) * DegreesPerMileLatitude;
                    double quarterSectionCenterLong = baseLongitudeSection + GetQuarterSectionLongShift(determinedQuarterSection) * degreesPerMileLongitude;

                    // Step 6: Find Closest LSD Within the Determined Quarter Section (Grid-Based)
                    int determinedLsd = FindClosestLsdWithinQuarterSection(
                        conn,
                        identifiedTownship,
                        identifiedRange,
                        determinedQuarterSection,
                        quarterSectionCenterLat,
                        quarterSectionCenterLong,
                        residualLatMiles,
                        residualLongMiles);

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
                    };

                    // Corrected DLS Coordinate Formatting: Use 'E' or 'W' instead of full words
                    string formattedDls = dlsCoordinate.ToString();

                    return dlsCoordinate;
                }
            }
            catch (SqlException ex)
            {
                return DlsCoordinate.Null;
            }
            catch (Exception ex)
            {
                return DlsCoordinate.Null;
            }
        }

        /// <summary>
        /// Finds the closest LSD within the determined quarter section using grid-based assignment.
        /// </summary>
        private static int FindClosestLsdWithinQuarterSection(
            SqlConnection conn,
            int township,
            int range,
            string quarterSection,
            double quarterSectionCenterLat,
            double quarterSectionCenterLong,
            double residualLatMiles,
            double residualLongMiles)
        {
            // Define grid size
            double gridSize = QuarterSectionWidthMiles / 4.0; // 0.125 miles per grid cell

            // Determine grid row based on latitude residual
            int row = 0;
            if (residualLatMiles > gridSize * 1.0)
                row = 0; // Top row (North)
            else if (residualLatMiles > 0)
                row = 1; // Upper-middle row
            else if (residualLatMiles > -gridSize * 1.0)
                row = 2; // Lower-middle row
            else
                row = 3; // Bottom row (South)

            // Determine grid column based on longitude residual with corrected mapping
            int column = 0;
            if (residualLongMiles <= -0.0625)
                column = 1; // LSD6
            else if (residualLongMiles <= 0.0625)
                column = 2; // LSD7
            else
                column = 3; // LSD8

            // Map grid indices to LSD number
            // LSD numbering starts from top-left (Row 0, Column 0) to bottom-right (Row 3, Column 3)
            // LSD Numbers: 1-4 (Row 0), 5-8 (Row 1), 9-12 (Row 2), 13-16 (Row 3)
            // Adjust LSD numbering if necessary
            int lsdNumber = (row * 4) + (column + 1);

            return lsdNumber;
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
                    return QuarterSectionWidthMiles / 2.0; // +0.25
                case "SE":
                case "SW":
                    return -QuarterSectionWidthMiles / 2.0; // -0.25
                case "N4":
                    return QuarterSectionWidthMiles / 2.0; // +0.25
                case "S4":
                    return -QuarterSectionWidthMiles / 2.0; // -0.25
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
                    return QuarterSectionWidthMiles / 2.0; // +0.25
                case "NW":
                case "SW":
                    return -QuarterSectionWidthMiles / 2.0; // -0.25
                case "E4":
                    return QuarterSectionWidthMiles / 2.0; // +0.25
                case "W4":
                    return -QuarterSectionWidthMiles / 2.0; // -0.25
                case "N4":
                case "S4":
                case "CS":
                default:
                    return 0.0;
            }
        }

        /// <summary>
        /// Calculates degrees per mile of longitude at a given latitude.
        /// </summary>
        private static double DegreesPerMileLongitude(double latitude)
        {
            // Adjust for latitude by using the cosine of latitude in radians
            double radians = ToRadians(latitude);
            double cosLat = Math.Cos(radians);
            if (cosLat == 0)
            {
                throw new ArgumentException("Latitude results in undefined DegreesPerMileLongitude (cos(latitude) = 0).");
            }
            return 1.0 / (69.172 * cosLat);
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
        /// Determines the Township based on the input latitude.
        /// </summary>
        private static int DetermineTownshipFromLatitude(double latitude)
        {
            // Define the starting latitude for Township 1
            const double startingLatitude = 49.0;
            // Define the latitude span per township (approximately 6 miles)
            const double latitudeSpanPerTownship = 6.0 * DegreesPerMileLatitude; // 6 miles * DegreesPerMileLatitude (~0.0867 degrees)

            if (latitude < startingLatitude)
            {
                throw new ArgumentException("Latitude is below the starting point of the Township grid.");
            }

            // Calculate the township number
            int townshipNumber = (int)Math.Floor((latitude - startingLatitude) / latitudeSpanPerTownship) + 1;

            return townshipNumber;
        }

    /// <summary>
    /// Represents a Legal Subdivision (LSD) position with latitude and longitude offsets.
    /// </summary>
    public class LsdPosition
    {
        public int LsdNumber { get; set; }
        public double LatOffset { get; set; }  // Offset in miles from quarter section center
        public double LongOffset { get; set; } // Offset in miles from quarter section center
    }

    /// <summary>
    /// Enum representing the direction of the meridian.
    /// </summary>
    public enum MeridianDirection
    {
        East,
        West
    }
}
