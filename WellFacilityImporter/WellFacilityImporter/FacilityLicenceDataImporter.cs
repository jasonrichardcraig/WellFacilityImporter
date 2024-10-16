using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace WellFacilityImporter
{
    public class FacilityLicenceDataImporter
    {
        public static void ImportData(string connectionString, string filePath)
        {
            XNamespace ns = "FacilityLicence";

            // Prepare list to hold data  
            List<DataRow> facilityLicenceRows = [];

            // Define DataTable schema  
            DataTable facilityLicenceTable = CreateFacilityLicenceDataTable();

            int counter = 0;

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement("Licence", ns.NamespaceName))
                    {
                        if (XElement.ReadFrom(reader) is XElement licenceElement)
                        {
                            // Extract Licence data  
                            DataRow facilityLicenceRow = ParseLicenceElement(licenceElement, facilityLicenceTable);
                            facilityLicenceRows.Add(facilityLicenceRow);

                            counter++;

                            Console.Write($"\rProcessing count: {counter}");

                            // Bulk insert every 1000 rows and clear lists
                            if (counter % 1000 == 0)
                            {
                                // Bulk insert to SQL Server  
                                BulkInsertToDatabase(facilityLicenceTable, facilityLicenceRows, connectionString, "FacilityLicence.Licence");

                                // Clear the list to free up memory
                                facilityLicenceRows.Clear();
                            }
                        }
                    }
                }
            }

            // Final insert for any remaining records that didn't make up a full batch of 1000
            if (facilityLicenceRows.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Bulk inserting remaining records to the database...");

                BulkInsertToDatabase(facilityLicenceTable, facilityLicenceRows, connectionString, "FacilityLicence.Licence");
            }
            Console.WriteLine();
            Console.WriteLine("Data import completed.");
        }

        private static DataTable CreateFacilityLicenceDataTable()
        {
            DataTable table = new();
            table.Columns.Add("LicenceType", typeof(string));
            table.Columns.Add("LicenceNumber", typeof(string));
            table.Columns.Add("LicenceStatus", typeof(string));
            table.Columns.Add("LicenceStatusDate", typeof(DateTime));
            table.Columns.Add("Licensee", typeof(string));
            table.Columns.Add("LicenseeName", typeof(string));
            table.Columns.Add("EnergyDevelopmentCategoryType", typeof(string));
            table.Columns.Add("LicenceLocation", typeof(string));
            table.Columns.Add("LicenceLegalSubdivision", typeof(string));
            table.Columns.Add("LicenceSection", typeof(int));
            table.Columns.Add("LicenceTownship", typeof(int));
            table.Columns.Add("LicenceRange", typeof(int));
            table.Columns.Add("LicenceMeridian", typeof(int));
            table.Columns.Add("OrphanWellFlg", typeof(string));
            return table;
        }

        private static DataRow ParseLicenceElement(XElement element, DataTable facilityLicenceTable)
        {
            XNamespace ns = "FacilityLicence";

            DataRow row = facilityLicenceTable.NewRow();
            row["LicenceType"] = (string?)element.Element(ns + "LicenceType") ?? (object)DBNull.Value;
            row["LicenceNumber"] = (string?)element.Element(ns + "LicenceNumber") ?? (object)DBNull.Value;
            row["LicenceStatus"] = (string?)element.Element(ns + "LicenceStatus") ?? (object)DBNull.Value;
            row["LicenceStatusDate"] = DateTime.TryParse((string?)element.Element(ns + "LicenceStatusDate"), out var statusDate) ? (object)statusDate : DBNull.Value;
            row["Licensee"] = (string?)element.Element(ns + "Licensee") ?? (object)DBNull.Value;
            row["LicenseeName"] = (string?)element.Element(ns + "LicenseeName") ?? (object)DBNull.Value;
            row["EnergyDevelopmentCategoryType"] = (string?)element.Element(ns + "EnergyDevelopmentCategoryType") ?? (object)DBNull.Value;
            row["LicenceLocation"] = (string?)element.Element(ns + "LicenceLocation") ?? (object)DBNull.Value;
            row["LicenceLegalSubdivision"] = (string?)element.Element(ns + "LicenceLegalSubdivision") ?? (object)DBNull.Value;
            row["LicenceSection"] = int.TryParse((string?)element.Element(ns + "LicenceSection"), out var section) ? (object)section : DBNull.Value;
            row["LicenceTownship"] = int.TryParse((string?)element.Element(ns + "LicenceTownship"), out var township) ? (object)township : DBNull.Value;
            row["LicenceRange"] = int.TryParse((string?)element.Element(ns + "LicenceRange"), out var range) ? (object)range : DBNull.Value;
            row["LicenceMeridian"] = int.TryParse((string?)element.Element(ns + "LicenceMeridian"), out var meridian) ? (object)meridian : DBNull.Value;
            row["OrphanWellFlg"] = (string?)element.Element(ns + "OrphanWellFlg") ?? (object)DBNull.Value;

            return row;
        }

        private static void BulkInsertToDatabase(DataTable dataTable, List<DataRow> rows, string connectionString, string destinationTableName)
        {
            if (rows.Count == 0) return;

            dataTable.Rows.Clear();

            // Add the rows to the DataTable
            foreach (DataRow row in rows)
            {
                dataTable.Rows.Add(row);
            }

            using SqlConnection conn = new(connectionString);
            conn.Open();

            try
            {
                using SqlBulkCopy bulkCopy = new(conn)
                {
                    DestinationTableName = destinationTableName
                };

                bulkCopy.WriteToServer(dataTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during bulk insert: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
