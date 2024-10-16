using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace WellFacilityImporter
{
    public class WellLicenceDataImporter
    {
        public static void ImportData(string connectionString, string filePath)
        {
            XNamespace ns = "WellLicence";

            // Prepare list to hold data  
            List<DataRow> wellLicenceRows = [];

            // Define DataTable schema  
            DataTable wellLicenceTable = CreateWellLicenceDataTable();

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
                            DataRow wellLicenceRow = ParseLicenceElement(licenceElement, wellLicenceTable);
                            wellLicenceRows.Add(wellLicenceRow);

                            counter++;

                            Console.Write($"\rProcessing count: {counter}");

                            // Bulk insert every 1000 rows and clear lists
                            if (counter % 1000 == 0)
                            {
                                // Bulk insert to SQL Server  
                                BulkInsertToDatabase(wellLicenceTable, wellLicenceRows, connectionString, "WellLicence.Well");

                                // Clear the list to free up memory
                                wellLicenceRows.Clear();
                            }
                        }
                    }
                }
            }

            // Final insert for any remaining records that didn't make up a full batch of 1000
            if (wellLicenceRows.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Bulk inserting remaining records to the database...");

                BulkInsertToDatabase(wellLicenceTable, wellLicenceRows, connectionString, "WellLicence.Well");
            }
            Console.WriteLine();
            Console.WriteLine("Data import completed.");
        }

        private static DataTable CreateWellLicenceDataTable()
        {
            DataTable table = new();
            table.Columns.Add("LicenceType", typeof(string));
            table.Columns.Add("LicenceNumber", typeof(string));
            table.Columns.Add("LicenceIssueDate", typeof(DateTime));
            table.Columns.Add("LicenceStatus", typeof(string));
            table.Columns.Add("LicenceStatusDate", typeof(DateTime));
            table.Columns.Add("LicenceeID", typeof(string));
            table.Columns.Add("LicenceeName", typeof(string));
            table.Columns.Add("LicenceLocation", typeof(string));
            table.Columns.Add("LicenceLegalSubdivision", typeof(string));
            table.Columns.Add("LicenceSection", typeof(int));
            table.Columns.Add("LicenceTownship", typeof(int));
            table.Columns.Add("LicenceRange", typeof(int));
            table.Columns.Add("LicenceMeridian", typeof(int));
            table.Columns.Add("DrillingOperationType", typeof(string));
            table.Columns.Add("WellPurpose", typeof(string));
            table.Columns.Add("WellLicenceType", typeof(string));
            table.Columns.Add("WellSubstance", typeof(string));
            table.Columns.Add("ProjectedFormation", typeof(string));
            table.Columns.Add("TerminatingFormation", typeof(string));
            table.Columns.Add("ProjectedTotalDepth", typeof(decimal));
            table.Columns.Add("AERClass", typeof(string));
            table.Columns.Add("HeadLessor", typeof(string));
            table.Columns.Add("WellCompletionType", typeof(string));
            table.Columns.Add("TargetPool", typeof(string));
            table.Columns.Add("OrphanWellFlg", typeof(string));
            return table;
        }

        private static DataRow ParseLicenceElement(XElement element, DataTable wellLicenceTable)
        {
            XNamespace ns = "WellLicence";

            DataRow row = wellLicenceTable.NewRow();

            row["LicenceType"] = (string?)element.Element(ns + "LicenceType") ?? (object)DBNull.Value;
            row["LicenceNumber"] = (string?)element.Element(ns + "LicenceNumber") ?? (object)DBNull.Value;
            row["LicenceIssueDate"] = DateTime.TryParse((string?)element.Element(ns + "LicenceIssueDate"), out var issueDate) ? (object)issueDate : DBNull.Value;
            row["LicenceStatus"] = (string?)element.Element(ns + "LicenceStatus") ?? (object)DBNull.Value;
            row["LicenceStatusDate"] = DateTime.TryParse((string?)element.Element(ns + "LicenceStatusDate"), out var statusDate) ? (object)statusDate : DBNull.Value;
            row["LicenceeID"] = (string?)element.Element(ns + "Licencee") ?? (object)DBNull.Value;
            row["LicenceeName"] = (string?)element.Element(ns + "LicenceeName") ?? (object)DBNull.Value;
            row["LicenceLocation"] = (string?)element.Element(ns + "LicenceLocation") ?? (object)DBNull.Value;
            row["LicenceLegalSubdivision"] = (string?)element.Element(ns + "LicenceLegalSubdivision") ?? (object)DBNull.Value;
            row["LicenceSection"] = int.TryParse((string?)element.Element(ns + "LicenceSection"), out var section) ? (object)section : DBNull.Value;
            row["LicenceTownship"] = int.TryParse((string?)element.Element(ns + "LicenceTownship"), out var township) ? (object)township : DBNull.Value;
            row["LicenceRange"] = int.TryParse((string?)element.Element(ns + "LicenceRange"), out var range) ? (object)range : DBNull.Value;
            row["LicenceMeridian"] = int.TryParse((string?)element.Element(ns + "LicenceMeridian"), out var meridian) ? (object)meridian : DBNull.Value;
            row["DrillingOperationType"] = (string?)element.Element(ns + "DrillingOperationType") ?? (object)DBNull.Value;
            row["WellPurpose"] = (string?)element.Element(ns + "WellPurpose") ?? (object)DBNull.Value;
            row["WellLicenceType"] = (string?)element.Element(ns + "WellLicenceType") ?? (object)DBNull.Value;
            row["WellSubstance"] = (string?)element.Element(ns + "WellSubstance") ?? (object)DBNull.Value;
            row["ProjectedFormation"] = (string?)element.Element(ns + "ProjectedFormation") ?? (object)DBNull.Value;
            row["TerminatingFormation"] = (string?)element.Element(ns + "TerminatingFormation") ?? (object)DBNull.Value;
            row["ProjectedTotalDepth"] = decimal.TryParse((string?)element.Element(ns + "ProjectedTotalDepth"), out var totalDepth) ? (object)totalDepth : DBNull.Value;
            row["AERClass"] = (string?)element.Element(ns + "AERClass") ?? (object)DBNull.Value;
            row["HeadLessor"] = (string?)element.Element(ns + "HeadLessor") ?? (object)DBNull.Value;
            row["WellCompletionType"] = (string?)element.Element(ns + "WellCompletionType") ?? (object)DBNull.Value;
            row["TargetPool"] = (string?)element.Element(ns + "TargetPool") ?? (object)DBNull.Value;
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
                    DestinationTableName = destinationTableName,
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
