using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace WellFacilityImporter
{
    public class FacilityOperatorHistoryDataImporter
    {
        public static void ImportData(string connectionString, string filePath)
        {
            XNamespace ns = "FacilityOperatorHistory";

            // Prepare lists to hold data  
            List<DataRow> facilityRows = [];
            List<DataRow> operatorHistoryRows = [];

            // Define DataTable schemas  
            DataTable facilityTable = CreateFacilityDataTable();
            DataTable operatorHistoryTable = CreateOperatorHistoryDataTable();

            int facilityCounter = 0;
            int operatorCounter = 0;

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement("FacilityOperator", ns.NamespaceName))
                    {
                        if (XElement.ReadFrom(reader) is XElement facilityOperatorElement)
                        {
                            // Extract basic Facility data  
                            DataRow facilityRow = ParseFacilityElement(facilityOperatorElement, facilityTable);
                            facilityRows.Add(facilityRow);
                            facilityCounter++;

                            // Extract Operator History data
                            string facilityID = (string?)facilityOperatorElement.Element(ns + "FacilityID") ?? string.Empty;

                            foreach (XElement operatorElement in facilityOperatorElement.Descendants(ns + "Operator"))
                            {
                                DataRow operatorRow = ParseOperatorElement(operatorElement, operatorHistoryTable, facilityID);
                                operatorHistoryRows.Add(operatorRow);
                                operatorCounter++;;
                            }

                            Console.Write($"\rProcessing count: {facilityCounter}");

                            // Bulk insert every 1000 rows and clear lists
                            if (facilityCounter % 1000 == 0)
                            {
                                BulkInsertToDatabase(facilityTable, facilityRows, connectionString, "FacilityOperatorHistory.Facility");
                                facilityRows.Clear();
                            }

                            if (operatorCounter % 1000 == 0)
                            {
                                BulkInsertToDatabase(operatorHistoryTable, operatorHistoryRows, connectionString, "FacilityOperatorHistory.OperatorHistory");
                                operatorHistoryRows.Clear();
                            }
                        }
                    }
                }
            }

            // Final insert for any remaining records that didn't make up a full batch of 1000
            if (facilityRows.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Bulk inserting remaining facilities to the database...");
                BulkInsertToDatabase(facilityTable, facilityRows, connectionString, "FacilityOperatorHistory.Facility");
            }

            if (operatorHistoryRows.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Bulk inserting remaining operators to the database...");
                BulkInsertToDatabase(operatorHistoryTable, operatorHistoryRows, connectionString, "FacilityOperatorHistory.OperatorHistory");
            }

            Console.WriteLine();
            Console.WriteLine("Data import completed.");
        }

        private static DataTable CreateFacilityDataTable()
        {
            DataTable table = new();
            table.Columns.Add("FacilityID", typeof(string));
            table.Columns.Add("FacilityProvinceState", typeof(string));
            table.Columns.Add("FacilityType", typeof(string));
            table.Columns.Add("FacilityIdentifier", typeof(string));
            table.Columns.Add("FacilityName", typeof(string));
            table.Columns.Add("FacilitySubType", typeof(string));
            table.Columns.Add("FacilitySubTypeDesc", typeof(string));
            return table;
        }

        private static DataTable CreateOperatorHistoryDataTable()
        {
            DataTable table = new();
            table.Columns.Add("FacilityID", typeof(string)); // Foreign key to Facility table
            table.Columns.Add("OperatorBAID", typeof(string));
            table.Columns.Add("OperatorName", typeof(string));
            table.Columns.Add("StartDate", typeof(string)); // Store as YYYY-MM
            table.Columns.Add("EndDate", typeof(string)); // Store as YYYY-MM
            return table;
        }

        private static DataRow ParseFacilityElement(XElement element, DataTable facilityTable)
        {
            XNamespace ns = "FacilityOperatorHistory";

            DataRow row = facilityTable.NewRow();
            row["FacilityID"] = (string?)element.Element(ns + "FacilityID") ?? (object)DBNull.Value;
            row["FacilityProvinceState"] = (string?)element.Element(ns + "FacilityProvinceState") ?? (object)DBNull.Value;
            row["FacilityType"] = (string?)element.Element(ns + "FacilityType") ?? (object)DBNull.Value;
            row["FacilityIdentifier"] = (string?)element.Element(ns + "FacilityIdentifier") ?? (object)DBNull.Value;
            row["FacilityName"] = (string?)element.Element(ns + "FacilityName") ?? (object)DBNull.Value;
            row["FacilitySubType"] = (string?)element.Element(ns + "FacilitySubType") ?? (object)DBNull.Value;
            row["FacilitySubTypeDesc"] = (string?)element.Element(ns + "FacilitySubTypeDesc") ?? (object)DBNull.Value;
            return row;
        }

        private static DataRow ParseOperatorElement(XElement element, DataTable operatorHistoryTable, string facilityID)
        {
            XNamespace ns = "FacilityOperatorHistory";

            DataRow row = operatorHistoryTable.NewRow();
            row["FacilityID"] = facilityID;
            row["OperatorBAID"] = (string?)element.Element(ns + "OperatorBAID") ?? (object)DBNull.Value;
            row["OperatorName"] = (string?)element.Element(ns + "OperatorName") ?? (object)DBNull.Value;
            row["StartDate"] = (string?)element.Element(ns + "StartDate") ?? (object)DBNull.Value;
            row["EndDate"] = (string?)element.Element(ns + "EndDate") ?? (object)DBNull.Value;
            return row;
        }

        private static void BulkInsertToDatabase(DataTable dataTable, List<DataRow> rows, string connectionString, string destinationTableName)
        {
            if (rows.Count == 0) return;

            dataTable.Rows.Clear();
            dataTable.BeginLoadData();

            // Add the rows to the DataTable
            foreach (DataRow row in rows)
            {
                dataTable.Rows.Add(row);
            }

            dataTable.EndLoadData();

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
