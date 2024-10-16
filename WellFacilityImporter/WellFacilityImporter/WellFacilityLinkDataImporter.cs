using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace WellFacilityImporter
{
    public class WellFacilityLinkDataImporter
    {
        public static void ImportData(string connectionString, string filePath)
        {
            XNamespace ns = "Well_x0020_to_x0020_Facility_x0020_Link";

            DataTable wellFacilityLinkTable = CreateWellFacilityLinkDataTable();
            DataTable linkedFacilityTable = CreateLinkedFacilityDataTable();

            List<DataRow> wellFacilityLinkRows = [];
            List<DataRow> linkedFacilityRows = [];

            int counter = 0;

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement("WellFacilityLink", ns.NamespaceName))
                    {
                        XElement wellFacilityLinkElement = (XElement)XElement.ReadFrom(reader);
                        DataRow wellFacilityLinkRow = ParseWellFacilityLinkElement(wellFacilityLinkElement, wellFacilityLinkTable);

                        if (!string.IsNullOrEmpty(wellFacilityLinkRow["WellID"].ToString()))
                        {
                            wellFacilityLinkRows.Add(wellFacilityLinkRow);

                            foreach (XElement linkedFacility in wellFacilityLinkElement.Descendants(ns + "LinkedFacility"))
                            {
                                DataRow linkedRow = ParseLinkedFacilityElement(linkedFacility, wellFacilityLinkRow["WellID"].ToString()!, linkedFacilityTable);
                                linkedFacilityRows.Add(linkedRow);
                            }

                            counter++;
                            Console.Write($"\rProcessing count: {counter}");

                            // Bulk insert every 1000 records and clear lists
                            if (counter % 1000 == 0)
                            {

                                BulkInsertToDatabase(wellFacilityLinkTable, wellFacilityLinkRows, connectionString, "WellFacilityLink.WellFacilityLink");
                                BulkInsertToDatabase(linkedFacilityTable, linkedFacilityRows, connectionString, "WellFacilityLink.LinkedFacility");

                                // Clear the lists to free up memory
                                wellFacilityLinkRows.Clear();
                                linkedFacilityRows.Clear();
                            }
                        }
                    }
                }
            }

            // Final bulk insert for any remaining records that didn't make up a full batch of 1000
            if (wellFacilityLinkRows.Count > 0 || linkedFacilityRows.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Bulk inserting remaining records to the database...");

                BulkInsertToDatabase(wellFacilityLinkTable, wellFacilityLinkRows, connectionString, "WellFacilityLink.WellFacilityLink");
                BulkInsertToDatabase(linkedFacilityTable, linkedFacilityRows, connectionString, "WellFacilityLink.LinkedFacility");
            }
            Console.WriteLine();
            Console.WriteLine("Data import completed.");
        }

        private static DataTable CreateWellFacilityLinkDataTable()
        {
            DataTable wellFacilityLinkTable = new();

            wellFacilityLinkTable.Columns.Add("WellID", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellProvinceState", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellType", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellIdentifier", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellLocationException", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellLegalSubdivision", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellSection", typeof(int));
            wellFacilityLinkTable.Columns.Add("WellTownship", typeof(int));
            wellFacilityLinkTable.Columns.Add("WellRange", typeof(int));
            wellFacilityLinkTable.Columns.Add("WellMeridian", typeof(int));
            wellFacilityLinkTable.Columns.Add("WellEventSequence", typeof(int));
            wellFacilityLinkTable.Columns.Add("WellName", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellStatusFluid", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellStatusMode", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellStatusType", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellStatusStructure", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellStatusFluidCode", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellStatusModeCode", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellStatusTypeCode", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellStatusStructureCode", typeof(string));
            wellFacilityLinkTable.Columns.Add("WellStatusStartDate", typeof(DateTime));

            return wellFacilityLinkTable;
        }

        private static DataTable CreateLinkedFacilityDataTable()
        {
            DataTable linkedFacilityTable = new();

            linkedFacilityTable.Columns.Add("LinkedFacilityID", typeof(string));
            linkedFacilityTable.Columns.Add("WellID", typeof(string)); // Foreign key or reference
            linkedFacilityTable.Columns.Add("LinkedFacilityProvinceState", typeof(string));
            linkedFacilityTable.Columns.Add("LinkedFacilityType", typeof(string));
            linkedFacilityTable.Columns.Add("LinkedFacilityIdentifier", typeof(string));
            linkedFacilityTable.Columns.Add("LinkedFacilityName", typeof(string));
            linkedFacilityTable.Columns.Add("LinkedFacilitySubType", typeof(string));
            linkedFacilityTable.Columns.Add("LinkedFacilitySubTypeDesc", typeof(string));
            linkedFacilityTable.Columns.Add("LinkedStartDate", typeof(DateTime));
            linkedFacilityTable.Columns.Add("LinkedFacilityOperatorBAID", typeof(string));
            linkedFacilityTable.Columns.Add("LinkedFacilityOperatorName", typeof(string));

            return linkedFacilityTable;
        }

        private static DataRow ParseWellFacilityLinkElement(XElement element, DataTable wellTable)
        {
            // Define the namespace explicitly based on the XML structure
            XNamespace ns = "Well_x0020_to_x0020_Facility_x0020_Link";

            DataRow row = wellTable.NewRow();

            row["WellID"] = (string?)element.Element(ns + "WellID") ?? (object)DBNull.Value;
            row["WellProvinceState"] = (string?)element.Element(ns + "WellProvinceState") ?? (object)DBNull.Value;
            row["WellType"] = (string?)element.Element(ns + "WellType") ?? (object)DBNull.Value;
            row["WellIdentifier"] = (string?)element.Element(ns + "WellIdentifier") ?? (object)DBNull.Value;
            row["WellLocationException"] = (string?)element.Element(ns + "WellLocationException") ?? (object)DBNull.Value;
            row["WellLegalSubdivision"] = (string?)element.Element(ns + "WellLegalSubdivision") ?? (object)DBNull.Value;
            row["WellSection"] = int.TryParse((string?)element.Element(ns + "WellSection"), out var section) ? (object)section : DBNull.Value;
            row["WellTownship"] = int.TryParse((string?)element.Element(ns + "WellTownship"), out var township) ? (object)township : DBNull.Value;
            row["WellRange"] = int.TryParse((string?)element.Element(ns + "WellRange"), out var range) ? (object)range : DBNull.Value;
            row["WellMeridian"] = int.TryParse((string?)element.Element(ns + "WellMeridian"), out var meridian) ? (object)meridian : DBNull.Value;
            row["WellEventSequence"] = int.TryParse((string?)element.Element(ns + "WellEventSequence"), out var sequence) ? (object)sequence : DBNull.Value;
            row["WellName"] = (string?)element.Element(ns + "WellName") ?? (object)DBNull.Value;
            row["WellStatusFluid"] = (string?)element.Element(ns + "WellStatusFluid") ?? (object)DBNull.Value;
            row["WellStatusMode"] = (string?)element.Element(ns + "WellStatusMode") ?? (object)DBNull.Value;
            row["WellStatusType"] = (string?)element.Element(ns + "WellStatusType") ?? (object)DBNull.Value;
            row["WellStatusStructure"] = (string?)element.Element(ns + "WellStatusStructure") ?? (object)DBNull.Value;
            row["WellStatusFluidCode"] = (string?)element.Element(ns + "WellStatusFluidCode") ?? (object)DBNull.Value;
            row["WellStatusModeCode"] = (string?)element.Element(ns + "WellStatusModeCode") ?? (object)DBNull.Value;
            row["WellStatusTypeCode"] = (string?)element.Element(ns + "WellStatusTypeCode") ?? (object)DBNull.Value;
            row["WellStatusStructureCode"] = (string?)element.Element(ns + "WellStatusStructureCode") ?? (object)DBNull.Value;
            row["WellStatusStartDate"] = DateTime.TryParse((string?)element.Element(ns + "WellStatusStartDate"), out var startDate) ? (object)startDate : DBNull.Value;

            return row;
        }

        private static DataRow ParseLinkedFacilityElement(XElement element, string wellId, DataTable linkedFacilityTable)
        {
            // Define the namespace explicitly based on the XML structure
            XNamespace ns = "Well_x0020_to_x0020_Facility_x0020_Link";

            DataRow row = linkedFacilityTable.NewRow();
            row["LinkedFacilityID"] = (string?)element.Element(ns + "LinkedFacilityID") ?? (object)DBNull.Value;
            row["WellID"] = wellId ?? (object)DBNull.Value;
            row["LinkedFacilityProvinceState"] = (string?)element.Element(ns + "LinkedFacilityProvinceState") ?? (object)DBNull.Value;
            row["LinkedFacilityType"] = (string?)element.Element(ns + "LinkedFacilityType") ?? (object)DBNull.Value;
            row["LinkedFacilityIdentifier"] = (string?)element.Element(ns + "LinkedFacilityIdentifier") ?? (object)DBNull.Value;
            row["LinkedFacilityName"] = (string?)element.Element(ns + "LinkedFacilityName") ?? (object)DBNull.Value;
            row["LinkedFacilitySubType"] = (string?)element.Element(ns + "LinkedFacilitySubType") ?? (object)DBNull.Value;
            row["LinkedFacilitySubTypeDesc"] = (string?)element.Element(ns + "LinkedFacilitySubTypeDesc") ?? (object)DBNull.Value;
            row["LinkedStartDate"] = DateTime.TryParse((string?)element.Element(ns + "LinkedStartDate"), out var linkedStartDate) ? (object)linkedStartDate : DBNull.Value;
            row["LinkedFacilityOperatorBAID"] = (string?)element.Element(ns + "LinkedFacilityOperatorBAID") ?? (object)DBNull.Value;
            row["LinkedFacilityOperatorName"] = (string?)element.Element(ns + "LinkedFacilityOperatorName") ?? (object)DBNull.Value;
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
