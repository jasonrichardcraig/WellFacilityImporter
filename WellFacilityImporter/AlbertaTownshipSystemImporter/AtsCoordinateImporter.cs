using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace AlbertaTownshipSystemImporter
{
    public class AtsCoordinateImporter
    {
        private readonly string _connectionString;
        private const int BatchSize = 1000;

        public AtsCoordinateImporter(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ImportFromFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                DataTable dataTable = CreateDataTable();
                string line;
                int totalRecords = 0;

                while ((line = reader.ReadLine()!) != null)
                {
                    var record = ParseLineToDataRow(line, dataTable);

                    if (record != null)
                    {
                        dataTable.Rows.Add(record);
                        totalRecords++;
                    }

                    // When the DataTable reaches the batch size, bulk insert and clear it
                    if (dataTable.Rows.Count >= BatchSize)
                    {
                        BulkInsertCoordinates(dataTable);
                        dataTable.Clear(); // Clear after the bulk copy
                        Console.WriteLine($"Inserted {totalRecords} records so far.");
                    }
                }

                // Insert any remaining rows that were less than 1000
                if (dataTable.Rows.Count > 0)
                {
                    BulkInsertCoordinates(dataTable);
                    Console.WriteLine($"Inserted the remaining {dataTable.Rows.Count} records.");
                }

                Console.WriteLine($"Total records inserted: {totalRecords}");
            }
        }

        private DataTable CreateDataTable()
        {
            var dataTable = new DataTable();

            // Define columns in DataTable (matching SQL Server table columns)
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Meridian", typeof(int));
            dataTable.Columns.Add("Range", typeof(int));
            dataTable.Columns.Add("Township", typeof(int));
            dataTable.Columns.Add("Section", typeof(int));
            dataTable.Columns.Add("QuarterSection", typeof(string));
            dataTable.Columns.Add("Latitude", typeof(double));
            dataTable.Columns.Add("Longitude", typeof(double));
            dataTable.Columns.Add("YearComputed", typeof(int));
            dataTable.Columns.Add("MonthComputed", typeof(int));
            dataTable.Columns.Add("DayComputed", typeof(int));
            dataTable.Columns.Add("StationCode", typeof(string));
            dataTable.Columns.Add("StatusCode", typeof(string));
            dataTable.Columns.Add("HorizontalClassification", typeof(string));
            dataTable.Columns.Add("CommentField", typeof(string));
            dataTable.Columns.Add("HorizontalOrigin", typeof(string));
            dataTable.Columns.Add("HorizontalMethod", typeof(string));
            dataTable.Columns.Add("HorizontalDatum", typeof(string));
            dataTable.Columns.Add("RoadAllowanceCode", typeof(string));
            dataTable.Columns.Add("Elevation", typeof(double));
            dataTable.Columns.Add("ElevationDate", typeof(DateTime));
            dataTable.Columns.Add("ElevationOrigin", typeof(string));
            dataTable.Columns.Add("ElevationMethod", typeof(string));
            dataTable.Columns.Add("ElevationAccuracy", typeof(string));
            dataTable.Columns.Add("VerticalDatum", typeof(string));
            dataTable.Columns.Add("UpdateDate", typeof(DateTime));

            return dataTable;
        }

        private DataRow ParseLineToDataRow(string line, DataTable dataTable)
        {
            try
            {
                var row = dataTable.NewRow();

                row["ID"] = DBNull.Value; // ID is an identity column in the database

                // Parse Meridian
                row["Meridian"] = !string.IsNullOrWhiteSpace(line.Substring(0, 1))
                                    ? (object)int.Parse(line.Substring(0, 1))
                                    : DBNull.Value;

                // Parse Range
                row["Range"] = !string.IsNullOrWhiteSpace(line.Substring(1, 2))
                               ? (object)int.Parse(line.Substring(1, 2))
                               : DBNull.Value;

                // Parse Township
                row["Township"] = !string.IsNullOrWhiteSpace(line.Substring(3, 3))
                                  ? (object)int.Parse(line.Substring(3, 3))
                                  : DBNull.Value;

                // Parse Section
                row["Section"] = !string.IsNullOrWhiteSpace(line.Substring(6, 2))
                                 ? (object)int.Parse(line.Substring(6, 2))
                                 : DBNull.Value;

                // Parse QuarterSection
                string quarterSection = line.Substring(8, 2).Trim();
                row["QuarterSection"] = !string.IsNullOrWhiteSpace(quarterSection)
                                        ? (object)quarterSection
                                        : DBNull.Value;

                // Parse Latitude
                row["Latitude"] = !string.IsNullOrWhiteSpace(line.Substring(10, 11))
                                  ? (object)double.Parse(line.Substring(10, 11), CultureInfo.InvariantCulture)
                                  : DBNull.Value;

                // Parse Longitude
                row["Longitude"] = !string.IsNullOrWhiteSpace(line.Substring(21, 12))
                                   ? (object)double.Parse(line.Substring(21, 12), CultureInfo.InvariantCulture)
                                   : DBNull.Value;

                // Parse YearComputed
                row["YearComputed"] = !string.IsNullOrWhiteSpace(line.Substring(33, 4))
                                      ? (object)int.Parse(line.Substring(33, 4))
                                      : DBNull.Value;

                // Parse MonthComputed
                row["MonthComputed"] = !string.IsNullOrWhiteSpace(line.Substring(37, 2))
                                       ? (object)int.Parse(line.Substring(37, 2))
                                       : DBNull.Value;

                // Parse DayComputed
                row["DayComputed"] = !string.IsNullOrWhiteSpace(line.Substring(39, 2))
                                     ? (object)int.Parse(line.Substring(39, 2))
                                     : DBNull.Value;

                // Parse StationCode
                row["StationCode"] = !string.IsNullOrWhiteSpace(line.Substring(41, 1))
                                     ? (object)line.Substring(41, 1)
                                     : DBNull.Value;

                // Parse StatusCode
                row["StatusCode"] = !string.IsNullOrWhiteSpace(line.Substring(42, 1))
                                    ? (object)line.Substring(42, 1)
                                    : DBNull.Value;

                // Parse HorizontalClassification
                row["HorizontalClassification"] = !string.IsNullOrWhiteSpace(line.Substring(43, 1))
                                                  ? (object)line.Substring(43, 1)
                                                  : DBNull.Value;

                // Parse CommentField
                string commentField = line.Substring(44, 12).Trim();
                row["CommentField"] = !string.IsNullOrWhiteSpace(commentField)
                                      ? (object)commentField
                                      : DBNull.Value;

                // Parse HorizontalOrigin
                row["HorizontalOrigin"] = !string.IsNullOrWhiteSpace(line.Substring(56, 1))
                                          ? (object)line.Substring(56, 1)
                                          : DBNull.Value;

                // Parse HorizontalMethod
                row["HorizontalMethod"] = !string.IsNullOrWhiteSpace(line.Substring(57, 1))
                                          ? (object)line.Substring(57, 1)
                                          : DBNull.Value;

                // Parse HorizontalDatum
                row["HorizontalDatum"] = !string.IsNullOrWhiteSpace(line.Substring(58, 1))
                                         ? (object)line.Substring(58, 1)
                                         : DBNull.Value;

                // Parse RoadAllowanceCode
                row["RoadAllowanceCode"] = !string.IsNullOrWhiteSpace(line.Substring(59, 1))
                                           ? (object)line.Substring(59, 1)
                                           : DBNull.Value;

                // Parse Elevation (nullable double)
                var elevation = ParseNullableDouble(line.Substring(60, 6));
                row["Elevation"] = elevation.HasValue ? (object)elevation.Value : DBNull.Value;

                // Parse ElevationDate (nullable DateTime)
                var elevationDate = ParseNullableDateTime(line.Substring(66, 8));
                row["ElevationDate"] = elevationDate.HasValue ? (object)elevationDate.Value : DBNull.Value;

                // Parse ElevationOrigin
                row["ElevationOrigin"] = !string.IsNullOrWhiteSpace(line.Substring(74, 1))
                                         ? (object)line.Substring(74, 1)
                                         : DBNull.Value;

                // Parse ElevationMethod
                row["ElevationMethod"] = !string.IsNullOrWhiteSpace(line.Substring(75, 1))
                                         ? (object)line.Substring(75, 1)
                                         : DBNull.Value;

                // Parse ElevationAccuracy
                row["ElevationAccuracy"] = !string.IsNullOrWhiteSpace(line.Substring(76, 1))
                                           ? (object)line.Substring(76, 1)
                                           : DBNull.Value;

                // Parse VerticalDatum
                row["VerticalDatum"] = !string.IsNullOrWhiteSpace(line.Substring(77, 1))
                                       ? (object)line.Substring(77, 1)
                                       : DBNull.Value;

                // Parse UpdateDate (nullable DateTime)
                var updateDate = ParseNullableDateTime(line.Substring(94, 14), "yyyyMMddHHmmss");
                row["UpdateDate"] = updateDate.HasValue ? (object)updateDate.Value : DBNull.Value;

                return row;
            }
            catch
            {
                Console.WriteLine($"Error parsing line: {line}");
                // Handle or log parsing errors here
                return null!;
            }
        }

        private void BulkInsertCoordinates(DataTable dataTable)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "[AlbertaTownshipSystem].[Coordinates]";
                    // Since DataTable columns match the database columns exactly, no need for column mappings
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }

        private double? ParseNullableDouble(string value)
        {
            if (double.TryParse(value, out var result))
            {
                return result;
            }
            return null;
        }

        private DateTime? ParseNullableDateTime(string value, string format = "yyyyMMdd")
        {
            if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }
            return null;
        }
    }

}
