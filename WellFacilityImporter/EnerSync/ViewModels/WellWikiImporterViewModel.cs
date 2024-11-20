using CommunityToolkit.Mvvm.ComponentModel;
using EnerSync.Data;
using EnerSync.Models;
using HtmlAgilityPack;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Net.Http;

namespace EnerSync.ViewModels
{
    public class WellWikiImporterViewModel : ObservableValidator
    {
        private EnerSyncContext _enerSyncContext;
        private static readonly HttpClient client = new();
        
        public ObservableCollection<ImportProgressItem> ImportProgressLog { get; } = new(); 
        public List<string> Wells { get; set; } = new();

        public WellWikiImporterViewModel(EnerSyncContext enerSyncContext)
        {
            _enerSyncContext = enerSyncContext;
        }

        public async Task ImportData()
        {

            ImportProgressLog.Clear();

            foreach (var well in Wells)
            {
                await ImportWellData(well);
            }

            LogMessage("Data import process completed.");

        }

        private async Task ImportWellData(string well)
        {
            string baseUrl = "https://wellwiki.org";  // Base URL
            string url = $"https://wellwiki.org/index.php?search={ConvertToWellWellWikiLocation(well)}";
            string connectionString = Properties.Settings.Default.ConnectionString;

            try
            {
                // Download the page content
                string pageContent = await client.GetStringAsync(url);

                // Load the HTML into HtmlDocument
                HtmlDocument doc = new();
                doc.LoadHtml(pageContent);

                // Scrape the first column (links) from the table rows
                var wellLinks = doc.DocumentNode.SelectNodes("//ul[@class='mw-search-results']//li//div[@class='mw-search-result-heading']//a");

                if (wellLinks != null)
                {
                    LogMessage("Following first column links:");

                    foreach (var link in wellLinks)
                    {
                        // Extract href attribute from the first column
                        var hrefValue = link.GetAttributeValue("href", string.Empty);

                        // Construct full URL by combining base URL and href
                        var fullUrl = baseUrl + hrefValue;
                        LogMessage($"Link: {fullUrl}");

                        // Visit the link and fetch its content
                        await ScrapeWellPage(fullUrl, connectionString);
                        break;  // Only process the first link
                    }
                    LogMessage("Data import completed.");
                }
                else
                {
                    LogMessage("No well links found on the page.");
                }
            }
            catch (HttpRequestException e)
            {
                LogMessage($"Error fetching page: {e.Message}");
            }
        }

        private async Task ScrapeWellPage(string url, string connectionString)
        {
            try
            {
                string pageContent = await client.GetStringAsync(url);
                HtmlDocument doc = new();
                doc.LoadHtml(pageContent);

                LogMessage($"Processing Well Page: {url}");
                await GetWellDetails(doc, connectionString);
            }
            catch (HttpRequestException e)
            {
                LogMessage($"Error fetching well page {url}: {e.Message}");
            }
        }

        private async Task GetWellDetails(HtmlDocument doc, string connectionString)
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

        private async Task<string> ExtractBasicWellDetails(HtmlDocument doc, string connectionString)
        {
            string? wellID = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Well ID:')]")?.InnerText.Replace("Well ID:", "").Trim();
            string? location = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Location:')]")?.InnerText.Replace("Location:", "").Trim();
            string? locationAlias = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Location Alias:')]")?.InnerText.Replace("Location Alias:", "").Trim();
            string? locationAlternateAlias = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Location Alias 2:')]")?.InnerText.Replace("Location Alias 2:", "").Trim();
            string? country = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Country:')]")?.InnerText.Replace("Country:", "").Trim();
            string? province = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Province:')]")?.InnerText.Replace("Province:", "").Trim();

            if (!int.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Township:')]")?.InnerText.Replace("Township:", "").Trim(), out int township))
            {
                LogMessage("Warning: Failed to convert Township to an integer.");
            }

            if (!int.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Meridian:')]")?.InnerText.Replace("Meridian:", "").Trim(), out int meridian))
            {
                LogMessage("Warning: Failed to convert Meridian to an integer.");
            }

            if (!int.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Range:')]")?.InnerText.Replace("Range:", "").Trim(), out int range))
            {
                LogMessage("Warning: Failed to convert Range to an integer.");
            }

            if (!int.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Section:')]")?.InnerText.Replace("Section:", "").Trim(), out int section))
            {
                LogMessage("Warning: Failed to convert Section to an integer.");
            }

            string? county = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'County/Municipality:')]")?.InnerText.Replace("County/Municipality:", "").Trim();
            string? wellName = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Well Name:')]")?.InnerText.Replace("Well Name:", "").Trim();
            string? operatorName = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Operator Name:')]")?.InnerText.Replace("Operator Name:", "").Trim();
            string? licenseNumber = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'License Number:')]")?.InnerText.Replace("License Number:", "").Trim();

            if (!DateTime.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'License Date:')]")?.InnerText.Replace("License Date:", "").Trim(), out DateTime licenseDate))
            {
                LogMessage("Warning: Failed to convert License Date to a DateTime.");
            }

            string? licenseStatus = doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'License Status:')]")?.InnerText.Replace("License Status:", "").Trim();

            if (!DateTime.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Spud Date:')]")?.InnerText.Replace("Spud Date:", "").Trim(), out DateTime spudDate))
            {
                LogMessage("Warning: Failed to convert Spud Date to a DateTime.");
            }

            if (!DateTime.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Final Drill Date:')]")?.InnerText.Replace("Final Drill Date:", "").Trim(), out DateTime finalDrillDate))
            {
                LogMessage("Warning: Failed to convert Final Drill Date to a DateTime.");
            }

            if (!double.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Well Total Depth:')]")?.InnerText.Replace("Well Total Depth:", "").Replace("m", "").Trim(), out double wellTotalDepth))
            {
                LogMessage("Warning: Failed to convert Well Total Depth to a double.");
            }

            if (!double.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Surface Hole Latitude:')]")?.InnerText.Replace("Surface Hole Latitude:", "").Trim(), out double surfaceHoleLatitude))
            {
                LogMessage("Warning: Failed to convert Surface Hole Latitude to a double.");
            }

            if (!float.TryParse(doc.DocumentNode.SelectSingleNode("//td[contains(text(), 'Surface Hole Longitude:')]")?.InnerText.Replace("Surface Hole Longitude:", "").Trim(), out float surfaceHoleLongitude))
            {
                LogMessage("Warning: Failed to convert Surface Hole Longitude to a float.");
            }

            // Prepare SQL insert command
            using SqlConnection conn = new(connectionString);
            conn.Open();
            string query = @"
            INSERT INTO WellWiki.Well (WellID, AlternateWellID, WellName, FormattedWellName, Location, LocationAlias, LocationAlternateAlias, Country, Province, Township, Meridian, Range, Section, County, 
                SurfaceHoleLatitude, SurfaceHoleLongitude, OperatorName, LicenseNumber, LicenseDate, LicenseStatus, SpudDate, FinalDrillDate, WellTotalDepth, DateTimeCreated)
            VALUES (@WellID, @AlternateWellID, @WellName, @FormattedWellName, @Location, @LocationAlias, @LocationAlternateAlias, @Country, @Province, @Township, @Meridian, @Range, @Section, @County, 
                @SurfaceHoleLatitude, @SurfaceHoleLongitude, @OperatorName, @LicenseNumber, @LicenseDate, @LicenseStatus, @SpudDate, @FinalDrillDate, @WellTotalDepth, GETDATE())";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@WellID", string.IsNullOrWhiteSpace(wellID) ? (object)DBNull.Value : wellID);
            cmd.Parameters.AddWithValue("@AlternateWellID", string.IsNullOrWhiteSpace(location) ? (object)DBNull.Value : ConvertDlsToWellID(location));
            cmd.Parameters.AddWithValue("@WellName", string.IsNullOrWhiteSpace(wellName) ? (object)DBNull.Value : wellName);
            cmd.Parameters.AddWithValue("@FormattedWellName", string.IsNullOrWhiteSpace(wellName) ? (object)DBNull.Value : CamelCaseString(wellName));
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
                LogMessage($"Inserted Well: {wellID}");
                return wellID!;
            }
            catch (Exception ex)
            {
                LogMessage($"Error inserting Well: {ex.Message}");
                return null!;
            }
        }


        private async Task ExtractWellHistory(HtmlDocument doc, string wellID, string connectionString)
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
                            LogMessage($"Error inserting Well History: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                LogMessage("Well History section not found.");
            }
        }

        private async Task ExtractDirectionalDrilling(HtmlDocument doc, string wellID, string connectionString)
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
                            LogMessage($"Error inserting Directional Drilling data: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                LogMessage("Directional Drilling section not found.");
            }
        }

        private async Task ExtractPerforationTreatments(HtmlDocument doc, string wellID, string connectionString)
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
                            LogMessage($"Error inserting Perforation Treatments data: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                LogMessage("Perforation Treatments section not found.");
            }
        }

        private async Task ExtractProductionData(HtmlDocument doc, string wellID, string connectionString)
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
                            LogMessage($"Error inserting Production Data: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                LogMessage("Production Data section not found.");
            }
        }

        private void LogMessage(string message)
        {
            ImportProgressLog.Add(new ImportProgressItem { TimeStamp = DateTime.Now, Message = message });
        }

        public static string ConvertToWellWellWikiLocation(string input)
        {
            // Remove the first character and the second last character
            string output = input.Substring(1, input.Length - 3) + input[^1];

            return output;
        }

        public static string ConvertDlsToWellID(string dlsString)
        {
            if (dlsString == null || dlsString.Length < 19)
            {
                return null!;
            }

            string prefix = dlsString.Substring(0, 2);
            string lsd = dlsString.Substring(3, 2);
            string section = dlsString.Substring(6, 2);
            string township = dlsString.Substring(9, 3);
            string range = dlsString.Substring(13, 2);
            string meridian = dlsString.Substring(15, 2);
            string suffix = dlsString.Substring(18, 1);

            // Ensure the suffix is two digits and prefix it with '0' if needed
            suffix = "0" + suffix;

            // Concatenate the result with '1' as the leading character
            string result = "1" + prefix + lsd + section + township + range + meridian + suffix;

            return result;
        }

        public static string CamelCaseString(string input)
        {
            if (input == null || input.Length == 0)
                return null!;

            string strInput = input;
            bool capitalizeNext = true;

            var result = new string(
                strInput
                .Select((c, i) =>
                {
                    if (c == '-' || c == '\'')
                    {
                        capitalizeNext = true;
                        return c;
                    }
                    if (c == ' ')
                    {
                        capitalizeNext = true;
                        return c;
                    }

                    if (capitalizeNext)
                    {
                        capitalizeNext = false;
                        return char.ToUpper(c);
                    }
                    else
                    {
                        return char.ToLower(c);
                    }
                }).ToArray());

            return result;
        }

    }
}
