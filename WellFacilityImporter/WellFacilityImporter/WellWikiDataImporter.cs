using HtmlAgilityPack;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;

namespace WellFacilityImporter
{
    public class WellWikiDataImporter
    {
        private static readonly HttpClient client = new();

        public static async Task ImportData(string companyName, string connectionString)
        {
            string baseUrl = "https://wellwiki.org";  // Base URL
            string url = $"https://wellwiki.org/index.php?title={companyName}";

            try
            {
                // Download the page content
                string pageContent = await client.GetStringAsync(url);

                // Load the HTML into HtmlDocument
                HtmlDocument doc = new();
                doc.LoadHtml(pageContent);

                // Scrape the first column (links) from the table rows
                var wellLinks = doc.DocumentNode.SelectNodes("//table[contains(@class,'display')]//tr//td[1]//a");

                if (wellLinks != null)
                {
                    Console.WriteLine("Following first column links:");

                    foreach (var link in wellLinks)
                    {
                        // Extract href attribute from the first column
                        var hrefValue = link.GetAttributeValue("href", string.Empty);

                        // Construct full URL by combining base URL and href
                        var fullUrl = baseUrl + hrefValue;
                        Console.WriteLine($"Link: {fullUrl}");

                        // Visit the link and fetch its content
                        await ScrapeWellPage(fullUrl, connectionString);
                    }
                    Console.WriteLine();
                    Console.WriteLine("Data import completed.");
                }
                else
                {
                    Console.WriteLine("No well links found on the page.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error fetching page: {e.Message}");
            }
        }

        private static async Task ScrapeWellPage(string url, string connectionString)
        {
            try
            {
                string pageContent = await client.GetStringAsync(url);
                HtmlDocument doc = new();
                doc.LoadHtml(pageContent);

                Console.WriteLine($"Processing Well Page: {url}");
                await GetWellDetails(doc, connectionString);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error fetching well page {url}: {e.Message}");
            }
        }

        private static async Task GetWellDetails(HtmlDocument doc, string connectionString)
        {
            var wellID = await ExtractBasicWellDetails(doc, connectionString);

            if (wellID != null)
            {
                await Task.WhenAll(
                    ExtractWellHistory(doc, wellID, connectionString),
                    ExtractDirectionalDrilling(doc, wellID, connectionString),
                    ExtractPerforationTreatments(doc, wellID, connectionString),
                    ExtractProductionData(doc, wellID, connectionString)
                );
            }
        }

        private static async Task<string> ExtractBasicWellDetails(HtmlDocument doc, string connectionString)
        {
            string? wellID = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Well ID:')]")?.InnerText.Replace("Well ID:", "").Trim();
            string? location = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Location:')]")?.InnerText.Replace("Location:", "").Trim();
            string? locationAlias = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Location Alias:')]")?.InnerText.Replace("Location Alias:", "").Trim();
            string? locationAlternateAlias = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Location Alias 2:')]")?.InnerText.Replace("Location Alias 2:", "").Trim();
            string? country = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Country:')]")?.InnerText.Replace("Country:", "").Trim();
            string? province = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Province:')]")?.InnerText.Replace("Province:", "").Trim();

            if (!int.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Township:')]")?.InnerText.Replace("Township:", "").Trim(), out int township))
            {
                Console.WriteLine("Warning: Failed to convert Township to an integer.");
            }

            if (!int.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Meridian:')]")?.InnerText.Replace("Meridian:", "").Trim(), out int meridian))
            {
                Console.WriteLine("Warning: Failed to convert Meridian to an integer.");
            }

            if (!int.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Range:')]")?.InnerText.Replace("Range:", "").Trim(), out int range))
            {
                Console.WriteLine("Warning: Failed to convert Range to an integer.");
            }

            if (!int.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Section:')]")?.InnerText.Replace("Section:", "").Trim(), out int section))
            {
                Console.WriteLine("Warning: Failed to convert Section to an integer.");
            }

            string? county = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'County/Municipality:')]")?.InnerText.Replace("County/Municipality:", "").Trim();
            string? wellName = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Well Name:')]")?.InnerText.Replace("Well Name:", "").Trim();
            string? operatorName = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Operator Name:')]")?.InnerText.Replace("Operator Name:", "").Trim();
            string? licenseNumber = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'License Number:')]")?.InnerText.Replace("License Number:", "").Trim();

            if (!DateTime.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'License Date:')]")?.InnerText.Replace("License Date:", "").Trim(), out DateTime licenseDate))
            {
                Console.WriteLine("Warning: Failed to convert License Date to a DateTime.");
            }

            string? licenseStatus = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'License Status:')]")?.InnerText.Replace("License Status:", "").Trim();

            if (!DateTime.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Spud Date:')]")?.InnerText.Replace("Spud Date:", "").Trim(), out DateTime spudDate))
            {
                Console.WriteLine("Warning: Failed to convert Spud Date to a DateTime.");
            }

            if (!DateTime.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Final Drill Date:')]")?.InnerText.Replace("Final Drill Date:", "").Trim(), out DateTime finalDrillDate))
            {
                Console.WriteLine("Warning: Failed to convert Final Drill Date to a DateTime.");
            }

            if (!double.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Well Total Depth:')]")?.InnerText.Replace("Well Total Depth:", "").Replace("m", "").Trim(), out double wellTotalDepth))
            {
                Console.WriteLine("Warning: Failed to convert Well Total Depth to a double.");
            }

            if (!double.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Surface Hole Latitude:')]")?.InnerText.Replace("Surface Hole Latitude:", "").Trim(), out double surfaceHoleLatitude))
            {
                Console.WriteLine("Warning: Failed to convert Surface Hole Latitude to a double.");
            }

            if (!float.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Surface Hole Longitude:')]")?.InnerText.Replace("Surface Hole Longitude:", "").Trim(), out float surfaceHoleLongitude))
            {
                Console.WriteLine("Warning: Failed to convert Surface Hole Longitude to a float.");
            }

            // Prepare SQL insert command
            using SqlConnection conn = new(connectionString);
            conn.Open();
            string query = @"
            INSERT INTO WellWiki.Well (WellID, WellName, Location, LocationAlias, LocationAlternateAlias, Country, Province, Township, Meridian, Range, Section, County, 
                SurfaceHoleLatitude, SurfaceHoleLongitude, OperatorName, LicenseNumber, LicenseDate, LicenseStatus, SpudDate, FinalDrillDate, WellTotalDepth, DateTimeCreated)
            VALUES (@WellID, @WellName, @Location, @LocationAlias, @LocationAlternateAlias, @Country, @Province, @Township, @Meridian, @Range, @Section, @County, 
                @SurfaceHoleLatitude, @SurfaceHoleLongitude, @OperatorName, @LicenseNumber, @LicenseDate, @LicenseStatus, @SpudDate, @FinalDrillDate, @WellTotalDepth, GETDATE())";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@WellID", string.IsNullOrWhiteSpace(wellID) ? (object)DBNull.Value : wellID);
            cmd.Parameters.AddWithValue("@WellName", string.IsNullOrWhiteSpace(wellName) ? (object)DBNull.Value : wellName);
            cmd.Parameters.AddWithValue("@Location", string.IsNullOrWhiteSpace(location) ? (object)DBNull.Value : location);
            cmd.Parameters.AddWithValue("@LocationAlias", string.IsNullOrWhiteSpace(locationAlias) ? (object)DBNull.Value : locationAlias);
            cmd.Parameters.AddWithValue("@LocationAlternateAlias", string.IsNullOrWhiteSpace(locationAlternateAlias) ? (object)DBNull.Value : locationAlternateAlias);
            cmd.Parameters.AddWithValue("@Country", string.IsNullOrWhiteSpace(country) ? (object)DBNull.Value : country);
            cmd.Parameters.AddWithValue("@Province", string.IsNullOrWhiteSpace(province) ? (object)DBNull.Value : province);
            cmd.Parameters.AddWithValue("@Township", township != 0 ? township : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Meridian", meridian != 0 ? meridian : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Range", range != 0 ? range : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Section", section != 0 ? section : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@County", string.IsNullOrWhiteSpace(county) ? (object)DBNull.Value : county);
            cmd.Parameters.AddWithValue("@SurfaceHoleLatitude", surfaceHoleLatitude != 0 ? surfaceHoleLatitude : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@SurfaceHoleLongitude", surfaceHoleLongitude != 0 ? surfaceHoleLongitude : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@OperatorName", string.IsNullOrWhiteSpace(operatorName) ? (object)DBNull.Value : operatorName);
            cmd.Parameters.AddWithValue("@LicenseNumber", string.IsNullOrWhiteSpace(licenseNumber) ? (object)DBNull.Value : licenseNumber);
            cmd.Parameters.AddWithValue("@LicenseDate", licenseDate != DateTime.MinValue ? licenseDate : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LicenseStatus", string.IsNullOrWhiteSpace(licenseStatus) ? (object)DBNull.Value : licenseStatus);
            cmd.Parameters.AddWithValue("@SpudDate", spudDate != DateTime.MinValue ? spudDate : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@FinalDrillDate", finalDrillDate != DateTime.MinValue ? finalDrillDate : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@WellTotalDepth", wellTotalDepth != 0 ? wellTotalDepth : (object)DBNull.Value);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine($"Inserted Well: {wellID}");
                return wellID!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting Well: {ex.Message}");
                return null!;
            }
        }


        private static async Task ExtractWellHistory(HtmlDocument doc, string wellID, string connectionString)
        {
            var wellHistoryTable = doc.DocumentNode.SelectSingleNode("//h2[span[@id='Well_History']]/following-sibling::table[1]");

            if (wellHistoryTable != null)
            {
                using SqlConnection conn = new(connectionString);
                conn.Open();
                foreach (var row in wellHistoryTable.SelectNodes(".//tr"))
                {
                    var cells = row.SelectNodes(".//td");
                    if (cells != null && cells.Count > 1)
                    {
                        string date = cells[0].InnerText.Trim();
                        string eventDetail = cells[1].InnerText.Trim();

                        string query = @"
                        INSERT INTO WellWiki.WellHistory (WellID, Date, Event)
                        VALUES (@WellID, @Date, @Event)";

                        using SqlCommand cmd = new(query, conn);
                        cmd.Parameters.AddWithValue("@WellID", wellID);
                        cmd.Parameters.AddWithValue("@Date", DateTime.TryParse(date, out DateTime parsedDate) ? (object)parsedDate : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Event", eventDetail);

                        try
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error inserting Well History: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Well History section not found.");
            }
        }

        private static async Task ExtractDirectionalDrilling(HtmlDocument doc, string wellID, string connectionString)
        {
            var directionalDrillingTable = doc.DocumentNode.SelectSingleNode("//h2[span[@id='Directional_Drilling']]/following-sibling::table[1]");

            if (directionalDrillingTable != null)
            {
                using SqlConnection conn = new(connectionString);
                conn.Open();
                foreach (var row in directionalDrillingTable.SelectNodes(".//tr"))
                {
                    var cells = row.SelectNodes(".//td");
                    if (cells != null && cells.Count > 2)
                    {
                        string startDate = cells[0].InnerText.Trim();
                        string depth = cells[1].InnerText.Trim();
                        string reason = cells[2].InnerText.Trim();

                        string query = @"
                        INSERT INTO WellWiki.WellDirectionalDrilling (WellID, StartDate, Depth, Reason)
                        VALUES (@WellID, @StartDate, @Depth, @Reason)";

                        using SqlCommand cmd = new(query, conn);
                        cmd.Parameters.AddWithValue("@WellID", wellID);
                        cmd.Parameters.AddWithValue("@StartDate", DateTime.TryParse(startDate, out DateTime parsedStartDate) ? (object)parsedStartDate : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Depth", double.TryParse(depth, out double parsedDepth) ? (object)parsedDepth : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Reason", reason);

                        try
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error inserting Directional Drilling data: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Directional Drilling section not found.");
            }
        }

        private static async Task ExtractPerforationTreatments(HtmlDocument doc, string wellID, string connectionString)
        {
            var perforationTable = doc.DocumentNode.SelectSingleNode("//h2[span[@id='Perforation_Treatments']]/following-sibling::table[1]");

            if (perforationTable != null)
            {
                using SqlConnection conn = new(connectionString);
                conn.Open();
                foreach (var row in perforationTable.SelectNodes(".//tr"))
                {
                    var cells = row.SelectNodes(".//td");
                    if (cells != null && cells.Count > 4)
                    {
                        string perforationDate = cells[0].InnerText.Trim();
                        string perforationType = cells[1].InnerText.Trim();
                        string intervalTop = cells[2].InnerText.Trim();
                        string intervalBase = cells[3].InnerText.Trim();
                        string numberOfShots = cells[4].InnerText.Trim();

                        string query = @"
                        INSERT INTO WellWiki.WellPerforationTreatments (WellID, PerforationDate, PerforationType, IntervalTop, IntervalBase, NumberOfShots)
                        VALUES (@WellID, @PerforationDate, @PerforationType, @IntervalTop, @IntervalBase, @NumberOfShots)";

                        using SqlCommand cmd = new(query, conn);
                        cmd.Parameters.AddWithValue("@WellID", wellID);
                        cmd.Parameters.AddWithValue("@PerforationDate", DateTime.TryParse(perforationDate, out DateTime parsedDate) ? (object)parsedDate : DBNull.Value);
                        cmd.Parameters.AddWithValue("@PerforationType", perforationType);
                        cmd.Parameters.AddWithValue("@IntervalTop", double.TryParse(intervalTop, out double parsedTop) ? (object)parsedTop : DBNull.Value);
                        cmd.Parameters.AddWithValue("@IntervalBase", double.TryParse(intervalBase, out double parsedBase) ? (object)parsedBase : DBNull.Value);
                        cmd.Parameters.AddWithValue("@NumberOfShots", int.TryParse(numberOfShots, out int parsedShots) ? (object)parsedShots : DBNull.Value);

                        try
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error inserting Perforation Treatments data: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Perforation Treatments section not found.");
            }
        }

        private static async Task ExtractProductionData(HtmlDocument doc, string wellID, string connectionString)
        {
            var productionTable = doc.DocumentNode.SelectSingleNode("//h2[span[@id='Production_Data']]/following-sibling::table[1]");

            if (productionTable != null)
            {
                using SqlConnection conn = new(connectionString);
                conn.Open();
                foreach (var row in productionTable.SelectNodes(".//tr"))
                {
                    var cells = row.SelectNodes(".//td");
                    if (cells != null && cells.Count > 4)
                    {
                        string period = cells[0].InnerText.Trim();
                        string productionHours = cells[1].InnerText.Trim();
                        string gasQuantity = cells[2].InnerText.Trim();
                        string oilQuantity = cells[3].InnerText.Trim();
                        string waterQuantity = cells[4].InnerText.Trim();

                        string query = @"
                        INSERT INTO WellWiki.WellProductionData (WellID, Period, TotalProductionHours, GasQuantity, OilQuantity, WaterQuantity)
                        VALUES (@WellID, @Period, @TotalProductionHours, @GasQuantity, @OilQuantity, @WaterQuantity)";

                        using SqlCommand cmd = new(query, conn);
                        cmd.Parameters.AddWithValue("@WellID", wellID);
                        cmd.Parameters.AddWithValue("@Period", int.TryParse(period, out int parsedPeriod) ? (object)parsedPeriod : DBNull.Value);
                        cmd.Parameters.AddWithValue("@TotalProductionHours", int.TryParse(productionHours, out int parsedHours) ? (object)parsedHours : DBNull.Value);
                        cmd.Parameters.AddWithValue("@GasQuantity", double.TryParse(gasQuantity, out double parsedGas) ? (object)parsedGas : DBNull.Value);
                        cmd.Parameters.AddWithValue("@OilQuantity", double.TryParse(oilQuantity, out double parsedOil) ? (object)parsedOil : DBNull.Value);
                        cmd.Parameters.AddWithValue("@WaterQuantity", double.TryParse(waterQuantity, out double parsedWater) ? (object)parsedWater : DBNull.Value);

                        try
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error inserting Production Data: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Production Data section not found.");
            }
        }

    }
}
