using System;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;
using System.Data;

public class DlsToLatLongConverter
{
    // Constants representing the sizes of sections in miles
    private const double QuarterSectionWidthMiles = 0.5; // Each Quarter Section is 1/2 mile square
    private const double LsdWidthMiles = 0.25;           // Each LSD is 1/4 mile square
    private const double DegreesPerMileLatitude = 1.0 / 69.172; // Degrees per mile for latitude

    // Function to calculate degrees per mile of longitude at a given latitude
    private static double DegreesPerMileLongitude(double latitude)
    {
        return 1.0 / (69.172 * Math.Cos(latitude * (Math.PI / 180.0)));
    }

    // CLR function to convert DLS coordinates to latitude and longitude
    [SqlFunction(IsDeterministic = false, IsPrecise = false, DataAccess = DataAccessKind.Read)]
    public static SqlGeography ConvertDlsToLatLong(int lsd, int section, int township, int range, int meridian)
    {
        // Validate input parameters
        if (lsd < 1 || lsd > 16 || section < 1 || section > 36 || township < 1 || range < 1 || meridian < 1)
        {
            throw new ArgumentException("Invalid DLS components provided.");
        }

        // Variables to store base latitude, longitude, and quarter section
        double baseLatitude = 0.0;
        double baseLongitude = 0.0;
        string quarterSection = "";

        // Retrieve base coordinates from the database
        try
        {
            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 1 Latitude, Longitude, QuarterSection
                    FROM [AlbertaTownshipSystem].[Coordinates]
                    WHERE Meridian = @Meridian
                      AND [Range] = @Range
                      AND Township = @Township
                      AND Section = @Section", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Meridian", SqlDbType.Int) { Value = meridian });
                    cmd.Parameters.Add(new SqlParameter("@Range", SqlDbType.Int) { Value = range });
                    cmd.Parameters.Add(new SqlParameter("@Township", SqlDbType.Int) { Value = township });
                    cmd.Parameters.Add(new SqlParameter("@Section", SqlDbType.Int) { Value = section });

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            baseLatitude = reader.GetDouble(0);
                            baseLongitude = reader.GetDouble(1);
                            quarterSection = reader.GetString(2).ToUpper();
                        }
                        else
                        {
                            throw new Exception("No matching record found for the given DLS components.");
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            throw new Exception("Error executing SQL query.", ex);
        }

        // Calculate degrees per mile of longitude at the base latitude
        double degreesPerMileLongitude = DegreesPerMileLongitude(baseLatitude);

        // Initialize total shift variables
        double totalLatitudeShiftMiles = 0.0;
        double totalLongitudeShiftMiles = 0.0;

        // Adjust base coordinates based on the quarter section
        switch (quarterSection)
        {
            case "NE":
                totalLatitudeShiftMiles += QuarterSectionWidthMiles / 2.0;
                totalLongitudeShiftMiles += QuarterSectionWidthMiles / 2.0;
                break;
            case "NW":
                totalLatitudeShiftMiles += QuarterSectionWidthMiles / 2.0;
                totalLongitudeShiftMiles -= QuarterSectionWidthMiles / 2.0;
                break;
            case "SE":
                totalLatitudeShiftMiles -= QuarterSectionWidthMiles / 2.0;
                totalLongitudeShiftMiles += QuarterSectionWidthMiles / 2.0;
                break;
            case "SW":
                totalLatitudeShiftMiles -= QuarterSectionWidthMiles / 2.0;
                totalLongitudeShiftMiles -= QuarterSectionWidthMiles / 2.0;
                break;
            case "CS":
                // Center of the section; no adjustment needed
                break;
            case "S4":
                totalLatitudeShiftMiles -= QuarterSectionWidthMiles;
                break;
            case "N4":
                totalLatitudeShiftMiles += QuarterSectionWidthMiles;
                break;
            case "E4":
                totalLongitudeShiftMiles += QuarterSectionWidthMiles;
                break;
            case "W4":
                totalLongitudeShiftMiles -= QuarterSectionWidthMiles;
                break;
            default:
                if (quarterSection.StartsWith("P") || quarterSection.StartsWith("Q") || quarterSection.StartsWith("X"))
                {
                    // Custom governing points; no standard adjustment applied
                }
                else
                {
                    throw new ArgumentException($"Unknown QuarterSection value: {quarterSection}");
                }
                break;
        }

        // Correct LSD numbering pattern according to the DLS system
        // LSDs are numbered in a serpentine pattern starting from the southeast corner
        int[,] lsdPositions = new int[4, 4]
        {
            { 1,  2,  3,  4 },    // South row (row 0) - LSD1 to LSD4
            { 8,  7,  6,  5 },    // Row1 - LSD8 to LSD5
            { 9, 10, 11, 12 },    // Row2 - LSD9 to LSD12
            { 16,15,14,13 }       // North row3 - LSD16 to LSD13
        };

        // Find the row and column corresponding to the LSD number
        int lsdRow = -1;
        int lsdCol = -1;
        bool found = false;

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (lsdPositions[row, col] == lsd)
                {
                    lsdRow = row;
                    lsdCol = col;
                    found = true;
                    break;
                }
            }
            if (found) break;
        }

        if (!found)
        {
            throw new ArgumentException("Invalid LSD number.");
        }

        // Calculate the offset in miles from the center of the section
        // The center of the section is (0,0). Each LSD is 0.25 mile square.
        // LSDs are arranged in a 4x4 grid. Offset ranges from -0.375 to +0.375 miles.
        double lsdLatitudeOffsetMiles = (lsdRow - 1.5) * LsdWidthMiles;
        double lsdLongitudeOffsetMiles = (lsdCol - 1.5) * LsdWidthMiles;

        // Accumulate the total shifts
        totalLatitudeShiftMiles += lsdLatitudeOffsetMiles;
        totalLongitudeShiftMiles += lsdLongitudeOffsetMiles;

        // **Adjust Longitude Shift Scaling**
        // To account for the discrepancy (~1.45 km needed), apply a scaling factor
        // Calculate the required additional shift based on the discrepancy observed
        // Observed discrepancy in longitude: ~-0.024063 degrees
        // degreesPerMileLongitude at ~53°: ~0.01656 degrees/mile
        // Required miles shift: ~0.024063 / 0.01656 ? 1.45 miles west
        // Compare with current shift: totalLongitudeShiftMiles
        // If totalLongitudeShiftMiles = 0.125, to achieve ~1.45 miles, scaling factor ? 11.6
        // However, this seems too high; likely the base longitude or shift calculations need review
        // Instead, verify if the shift calculations are accurate

        // For this example, since the discrepancy is only ~1.45 km, consider if additional factors are involved

        // Adjust the latitude and longitude based on the total shifts
        double adjustedLatitude = baseLatitude + totalLatitudeShiftMiles * DegreesPerMileLatitude;

        // **Critical Correction: Adjust longitude shift direction**
        // Since west longitudes are negative, a westward shift should decrease the longitude value
        double adjustedLongitude = baseLongitude - totalLongitudeShiftMiles * degreesPerMileLongitude;

        // **Final Adjustment (Optional)**
        // If after all calculations, a residual discrepancy remains, apply a minor correction factor
        // Example:
        // adjustedLongitude -= 0.000027; // Subtract the observed discrepancy

        // **Logging for Debugging (Optional)**
        // Uncomment the following lines if you have access to console output for debugging
        /*
        Console.WriteLine($"Base Latitude: {baseLatitude}, Base Longitude: {baseLongitude}");
        Console.WriteLine($"Quarter Section: {quarterSection}");
        Console.WriteLine($"Total Latitude Shift Miles: {totalLatitudeShiftMiles}, Total Longitude Shift Miles: {totalLongitudeShiftMiles}");
        Console.WriteLine($"Adjusted Latitude: {adjustedLatitude}, Adjusted Longitude: {adjustedLongitude}");
        */

        // Create and return the geographic point using SRID 4326 (WGS 84)
        return SqlGeography.Point(adjustedLatitude, adjustedLongitude, 4326);
    }
}
