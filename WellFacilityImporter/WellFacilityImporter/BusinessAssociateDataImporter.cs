using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace WellFacilityImporter
{
    public class BusinessAssociateDataImporter
    {
        public static void ImportData(string connectionString, string filePath)
        {
            XNamespace ns = "BusinessAssociate";

            // Prepare list to hold data  
            List<DataRow> businessAssociateRows = [];

            // Define DataTable schema  
            DataTable businessAssociateTable = CreateBusinessAssociateDataTable();

            int counter = 0;

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement("BA", ns.NamespaceName))
                    {
                        if (XElement.ReadFrom(reader) is XElement baElement)
                        {
                            // Extract BA data  
                            DataRow businessAssociateRow = ParseBAElement(baElement, businessAssociateTable);
                            businessAssociateRows.Add(businessAssociateRow);

                            counter++;

                            Console.Write($"\rProcessing count: {counter}");

                            // Bulk insert every 1000 rows and clear lists
                            if (counter % 1000 == 0)
                            {
                                // Bulk insert to SQL Server  
                                BulkInsertToDatabase(businessAssociateTable, businessAssociateRows, connectionString, "BusinessAssociate.BusinessAssociate");

                                // Clear the list to free up memory
                                businessAssociateRows.Clear();
                            }
                        }
                    }
                }
            }

            // Final insert for any remaining records that didn't make up a full batch of 1000
            if (businessAssociateRows.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Bulk inserting remaining records to the database...");

                BulkInsertToDatabase(businessAssociateTable, businessAssociateRows, connectionString, "BusinessAssociate.BusinessAssociate");
            }
            Console.WriteLine();
            Console.WriteLine("Data import completed.");
        }

        private static DataTable CreateBusinessAssociateDataTable()
        {
            DataTable table = new();
            table.Columns.Add("BAIdentifier", typeof(string));
            table.Columns.Add("BALegalName", typeof(string));
            table.Columns.Add("BAAddress", typeof(string));
            table.Columns.Add("BAPhoneNumber", typeof(string));
            table.Columns.Add("BACorporateStatus", typeof(string));
            table.Columns.Add("BACorporateStatusEffectiveDate", typeof(DateTime));
            table.Columns.Add("AmalgamatedIntoBAID", typeof(string));
            table.Columns.Add("AmalgamatedIntoBALegalName", typeof(string));
            table.Columns.Add("BAAmalgamationEstablishedDate", typeof(DateTime));
            table.Columns.Add("BALicenceEligibilityType", typeof(string));
            table.Columns.Add("BALicenceEligibiltyDesc", typeof(string));
            table.Columns.Add("BAAbbreviatedName", typeof(string));
            return table;
        }

        private static DataRow ParseBAElement(XElement element, DataTable businessAssociateTable)
        {
            XNamespace ns = "BusinessAssociate";

            DataRow row = businessAssociateTable.NewRow();
            row["BAIdentifier"] = (string?)element.Element(ns + "BAIdentifier") ?? (object)DBNull.Value;
            row["BALegalName"] = (string?)element.Element(ns + "BALegalName") ?? (object)DBNull.Value;
            row["BAAddress"] = (string?)element.Element(ns + "BAAddress") ?? (object)DBNull.Value;
            row["BAPhoneNumber"] = (string?)element.Element(ns + "BAPhoneNumber") ?? (object)DBNull.Value;
            row["BACorporateStatus"] = (string?)element.Element(ns + "BACorporateStatus") ?? (object)DBNull.Value;
            row["BACorporateStatusEffectiveDate"] = DateTime.TryParse((string?)element.Element(ns + "BACorporateStatusEffectiveDate"), out var statusDate) ? (object)statusDate : DBNull.Value;
            row["AmalgamatedIntoBAID"] = (string?)element.Element(ns + "AmalgamatedIntoBAID") ?? (object)DBNull.Value;
            row["AmalgamatedIntoBALegalName"] = (string?)element.Element(ns + "AmalgamatedIntoBALegalName") ?? (object)DBNull.Value;
            row["BAAmalgamationEstablishedDate"] = DateTime.TryParse((string?)element.Element(ns + "BAAmalgamationEstablishedDate"), out var amalgamationDate) ? (object)amalgamationDate : DBNull.Value;
            row["BALicenceEligibilityType"] = (string?)element.Element(ns + "BALicenceEligibilityType") ?? (object)DBNull.Value;
            row["BALicenceEligibiltyDesc"] = (string?)element.Element(ns + "BALicenceEligibiltyDesc") ?? (object)DBNull.Value;
            row["BAAbbreviatedName"] = (string?)element.Element(ns + "BAAbbreviatedName") ?? (object)DBNull.Value;

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
