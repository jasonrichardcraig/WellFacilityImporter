using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace WellFacilityImporter
{
    internal class WellInfrastructureDataImporter
    {
        public static void ImportData(string connectionString, string filePath)
        {
            XNamespace ns = "WellInfratructure"; // This is a Typo on Petrinex's part. It should be "WellInfrastructure"

            // Prepare lists to hold data  
            List<DataRow> wellRows = [];
            List<DataRow> linkedFacilityRows = [];
            List<DataRow> commingledWellRows = [];

            // Define DataTable schemas  
            DataTable wellTable = CreateWellDataTable();
            DataTable linkedFacilityTable = CreateLinkedFacilityDataTable();
            DataTable commingledWellTable = CreateCommingledWellDataTable();

            int counter = 0;

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement("Well", ns.NamespaceName))
                    {
                        if (XElement.ReadFrom(reader) is XElement wellElement)
                        {
                            // Extract Well data  
                            DataRow wellRow = ParseWellElement(wellElement, wellTable);
                            wellRows.Add(wellRow);

                            // Extract LinkedFacility data  
                            foreach (XElement linkedFacility in wellElement.Descendants(ns + "LinkedFacility"))
                            {
                                if(linkedFacility.Element(ns + "LinkedFacilityID") != null && linkedFacility?.Element(ns + "LinkedFacilityID")?.Value != "")
                                {
                                    DataRow linkedFacilityRow = ParseLinkedFacilityElement(linkedFacility!, wellRow["WellID"].ToString()!, linkedFacilityTable);
                                    linkedFacilityRows.Add(linkedFacilityRow);
                                }
                            }

                            // Extract CommingledWell data  
                            foreach (XElement commingledWell in wellElement.Descendants(ns + "CommingledWell"))
                            {
                                if ((commingledWell.Element(ns + "ComminglingEffDate") != null && commingledWell?.Element(ns + "ComminglingEffDate")?.Value != "") || (commingledWell.Element(ns + "CommingledReportingWellID") != null && commingledWell?.Element(ns + "CommingledReportingWellID")?.Value != ""))
                                {
                                    DataRow commingledWellRow = ParseCommingledWellElement(commingledWell!, wellRow["WellID"].ToString()!, commingledWellTable);
                                    commingledWellRows.Add(commingledWellRow);
                                }
                            }

                            counter++;

                            Console.Write($"\rProcessing count: {counter}");

                            // Bulk insert every 1000 rows and clear lists
                            if (counter % 1000 == 0)
                            {
                                // Bulk insert to SQL Server  
                                BulkInsertToDatabase(wellTable, wellRows, connectionString, "WellInfrastructure.Well");
                                BulkInsertToDatabase(linkedFacilityTable, linkedFacilityRows, connectionString, "WellInfrastructure.LinkedFacility");
                                BulkInsertToDatabase(commingledWellTable, commingledWellRows, connectionString, "WellInfrastructure.CommingledWell");

                                // Clear the lists to free up memory
                                wellRows.Clear();
                                linkedFacilityRows.Clear();
                                commingledWellRows.Clear();
                            }
                        }
                    }
                }
            }

            // Final insert for any remaining records that didn't make up a full batch of 1000
            if (wellRows.Count > 0 || linkedFacilityRows.Count > 0 || commingledWellRows.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Bulk inserting remaining records to the database...");

                BulkInsertToDatabase(wellTable, wellRows, connectionString, "WellInfrastructure.Well");
                BulkInsertToDatabase(linkedFacilityTable, linkedFacilityRows, connectionString, "WellInfrastructure.LinkedFacility");
                BulkInsertToDatabase(commingledWellTable, commingledWellRows, connectionString, "WellInfrastructure.CommingledWell");
            }
            Console.WriteLine();
            Console.WriteLine("Data import completed.");
        }

        private static DataTable CreateWellDataTable()
        {
            DataTable table = new();
            table.Columns.Add("WellID", typeof(string));
            table.Columns.Add("WellProvinceState", typeof(string));
            table.Columns.Add("WellType", typeof(string));
            table.Columns.Add("WellIdentifier", typeof(string));
            table.Columns.Add("PreviousWellID", typeof(string));
            table.Columns.Add("WellLocationException", typeof(string));
            table.Columns.Add("WellLegalSubdivision", typeof(string));
            table.Columns.Add("WellSection", typeof(int));
            table.Columns.Add("WellTownship", typeof(int));
            table.Columns.Add("WellRange", typeof(int));
            table.Columns.Add("WellMeridian", typeof(int));
            table.Columns.Add("WellEventSequence", typeof(int));
            table.Columns.Add("WellName", typeof(string));
            table.Columns.Add("ConfidentialType", typeof(string));
            table.Columns.Add("ExperimentalConfidentialIndicator", typeof(string));
            table.Columns.Add("ExperimentalConfidentialEffDate", typeof(DateTime));
            table.Columns.Add("ExperimentalConfidentialTermDate", typeof(DateTime));
            table.Columns.Add("LicenceType", typeof(string));
            table.Columns.Add("LicenceNumber", typeof(string));
            table.Columns.Add("LicenceIssueDate", typeof(DateTime));
            table.Columns.Add("LicenceStatusDate", typeof(DateTime));
            table.Columns.Add("LicenceStatus", typeof(string));
            table.Columns.Add("Field", typeof(string));
            table.Columns.Add("FieldName", typeof(string));
            table.Columns.Add("Area", typeof(string));
            table.Columns.Add("AreaName", typeof(string));
            table.Columns.Add("PoolDeposit", typeof(string));
            table.Columns.Add("PoolDepositName", typeof(string));
            table.Columns.Add("PoolDepositDensity", typeof(decimal));
            table.Columns.Add("WellStatusFluid", typeof(string));
            table.Columns.Add("WellStatusMode", typeof(string));
            table.Columns.Add("WellStatusType", typeof(string));
            table.Columns.Add("WellStatusStructure", typeof(string));
            table.Columns.Add("WellStatusFluidCode", typeof(string));
            table.Columns.Add("WellStatusModeCode", typeof(string));
            table.Columns.Add("WellStatusTypeCode", typeof(string));
            table.Columns.Add("WellStatusStructureCode", typeof(string));
            table.Columns.Add("WellStatusDate", typeof(DateTime));
            table.Columns.Add("SpudDate", typeof(DateTime));
            table.Columns.Add("HorizontalDrill", typeof(string));
            table.Columns.Add("FinishedDrillDate", typeof(DateTime));
            table.Columns.Add("FinalTotalDepth", typeof(decimal));
            table.Columns.Add("MaxTrueVerticalDepth", typeof(decimal));
            table.Columns.Add("VolumetricGasWellLiquidType", typeof(string));
            table.Columns.Add("VolumetricGasWellLiquidEffDate", typeof(DateTime));
            table.Columns.Add("LicenceeID", typeof(string));
            table.Columns.Add("LicenceeName", typeof(string));
            table.Columns.Add("AllowableType", typeof(string));
            table.Columns.Add("BlockNumber", typeof(int));
            table.Columns.Add("RecoveryMechanismType", typeof(string));
            table.Columns.Add("OrphanWellFlg", typeof(string));
            return table;
        }

        private static DataTable CreateLinkedFacilityDataTable()
        {
            DataTable table = new();
            table.Columns.Add("LinkedFacilityID", typeof(string));
            table.Columns.Add("WellID", typeof(string));
            table.Columns.Add("LinkedFacilityProvinceState", typeof(string));
            table.Columns.Add("LinkedFacilityType", typeof(string));
            table.Columns.Add("LinkedFacilityIdentifier", typeof(string));
            table.Columns.Add("LinkedFacilityName", typeof(string));
            table.Columns.Add("LinkedFacilitySubType", typeof(string));
            table.Columns.Add("LinkedFacilitySubTypeDesc", typeof(string));
            table.Columns.Add("LinkedStartDate", typeof(DateTime));
            table.Columns.Add("LinkedFacilityOperatorBAID", typeof(string));
            table.Columns.Add("LinkedFacilityOperatorLegalName", typeof(string));
            return table;
        }

        private static DataTable CreateCommingledWellDataTable()
        {
            DataTable table = new();
            table.Columns.Add("ComminglingProcessApprovalNumber", typeof(string));
            table.Columns.Add("WellID", typeof(string));
            table.Columns.Add("ComminglingProcess", typeof(string));
            table.Columns.Add("ComminglingEffDate", typeof(DateTime));
            table.Columns.Add("CommingledReportingWellID", typeof(string));
            table.Columns.Add("CommingledReportingWellProvinceState", typeof(string));
            table.Columns.Add("CommingledReportingWellType", typeof(string));
            table.Columns.Add("CommingledReportingWellIdentifier", typeof(string));
            return table;
        }

        private static DataRow ParseWellElement(XElement element, DataTable wellTable)
        {
            // Define the namespace explicitly based on the XML structure
            XNamespace ns = "WellInfratructure"; // Correct the typo if needed

            DataRow row = wellTable.NewRow();

            row["WellID"] = (string?)element.Element(ns + "WellID") ?? (object)DBNull.Value;
            row["WellProvinceState"] = (string?)element.Element(ns + "WellProvinceState") ?? (object)DBNull.Value;
            row["WellType"] = (string?)element.Element(ns + "WellType") ?? (object)DBNull.Value;
            row["WellIdentifier"] = (string?)element.Element(ns + "WellIdentifier") ?? (object)DBNull.Value;
            row["PreviousWellID"] = (string?)element.Element(ns + "PreviousWellID") ?? (object)DBNull.Value;
            row["WellLocationException"] = (string?)element.Element(ns + "WellLocationException") ?? (object)DBNull.Value;
            row["WellLegalSubdivision"] = (string?)element.Element(ns + "WellLegalSubdivision") ?? (object)DBNull.Value;
            row["WellSection"] = int.TryParse((string?)element.Element(ns + "WellSection"), out var section) ? (object)section : DBNull.Value;
            row["WellTownship"] = int.TryParse((string?)element.Element(ns + "WellTownship"), out var township) ? (object)township : DBNull.Value;
            row["WellRange"] = int.TryParse((string?)element.Element(ns + "WellRange"), out var range) ? (object)range : DBNull.Value;
            row["WellMeridian"] = int.TryParse((string?)element.Element(ns + "WellMeridian"), out var meridian) ? (object)meridian : DBNull.Value;
            row["WellEventSequence"] = int.TryParse((string?)element.Element(ns + "WellEventSequence"), out var sequence) ? (object)sequence : DBNull.Value;
            row["WellName"] = (string?)element.Element(ns + "WellName") ?? (object)DBNull.Value;
            row["ConfidentialType"] = (string?)element.Element(ns + "ConfidentialType") ?? (object)DBNull.Value;
            row["ExperimentalConfidentialIndicator"] = (string?)element.Element(ns + "ExperimentalConfidentialIndicator") ?? (object)DBNull.Value;
            row["ExperimentalConfidentialEffDate"] = DateTime.TryParse((string?)element.Element(ns + "ExperimentalConfidentialEffectiveDate"), out var confEffDate) ? (object)confEffDate : DBNull.Value;
            row["ExperimentalConfidentialTermDate"] = DateTime.TryParse((string?)element.Element(ns + "ExperimentalConfidentialTerminationDate"), out var confTermDate) ? (object)confTermDate : DBNull.Value;
            row["LicenceType"] = (string?)element.Element(ns + "LicenceType") ?? (object)DBNull.Value;
            row["LicenceNumber"] = (string?)element.Element(ns + "LicenceNumber") ?? (object)DBNull.Value;
            row["LicenceIssueDate"] = DateTime.TryParse((string?)element.Element(ns + "LicenceIssueDate"), out var issueDate) ? (object)issueDate : DBNull.Value;
            row["LicenceStatusDate"] = DateTime.TryParse((string?)element.Element(ns + "LicenceStatusDate"), out var statusDate) ? (object)statusDate : DBNull.Value;
            row["LicenceStatus"] = (string?)element.Element(ns + "LicenceStatus") ?? (object)DBNull.Value;
            row["Field"] = (string?)element.Element(ns + "Field") ?? (object)DBNull.Value;
            row["FieldName"] = (string?)element.Element(ns + "FieldName") ?? (object)DBNull.Value;
            row["Area"] = (string?)element.Element(ns + "Area") ?? (object)DBNull.Value;
            row["AreaName"] = (string?)element.Element(ns + "AreaName") ?? (object)DBNull.Value;
            row["PoolDeposit"] = (string?)element.Element(ns + "PoolDeposit") ?? (object)DBNull.Value;
            row["PoolDepositName"] = (string?)element.Element(ns + "PoolDepositName") ?? (object)DBNull.Value;
            row["PoolDepositDensity"] = decimal.TryParse((string?)element.Element(ns + "PoolDepositDensity"), out var density) ? (object)density : DBNull.Value;
            row["WellStatusFluid"] = (string?)element.Element(ns + "WellStatusFluid") ?? (object)DBNull.Value;
            row["WellStatusMode"] = (string?)element.Element(ns + "WellStatusMode") ?? (object)DBNull.Value;
            row["WellStatusType"] = (string?)element.Element(ns + "WellStatusType") ?? (object)DBNull.Value;
            row["WellStatusStructure"] = (string?)element.Element(ns + "WellStatusStructure") ?? (object)DBNull.Value;
            row["WellStatusFluidCode"] = (string?)element.Element(ns + "WellStatusFluidCode") ?? (object)DBNull.Value;
            row["WellStatusModeCode"] = (string?)element.Element(ns + "WellStatusModeCode") ?? (object)DBNull.Value;
            row["WellStatusTypeCode"] = (string?)element.Element(ns + "WellStatusTypeCode") ?? (object)DBNull.Value;
            row["WellStatusStructureCode"] = (string?)element.Element(ns + "WellStatusStructureCode") ?? (object)DBNull.Value;
            row["WellStatusDate"] = DateTime.TryParse((string?)element.Element(ns + "WellStatusDate"), out var statusEffDate) ? (object)statusEffDate : DBNull.Value;
            row["SpudDate"] = DateTime.TryParse((string?)element.Element(ns + "SpudDate"), out var spudDate) ? (object)spudDate : DBNull.Value;
            row["HorizontalDrill"] = (string?)element.Element(ns + "HorizontalDrill") ?? (object)DBNull.Value;
            row["FinishedDrillDate"] = DateTime.TryParse((string?)element.Element(ns + "FinishedDrillDate"), out var finishedDrillDate) ? (object)finishedDrillDate : DBNull.Value;
            row["FinalTotalDepth"] = decimal.TryParse((string?)element.Element(ns + "FinalTotalDepth"), out var totalDepth) ? (object)totalDepth : DBNull.Value;
            row["MaxTrueVerticalDepth"] = decimal.TryParse((string?)element.Element(ns + "MaxTrueVerticalDepth"), out var maxDepth) ? (object)maxDepth : DBNull.Value;
            row["VolumetricGasWellLiquidType"] = (string?)element.Element(ns + "VolumetricGasWellLiquidType") ?? (object)DBNull.Value;
            row["VolumetricGasWellLiquidEffDate"] = DateTime.TryParse((string?)element.Element(ns + "VolumetricGasWellLiquidEffectiveDate"), out var liquidEffDate) ? (object)liquidEffDate : DBNull.Value;
            row["LicenceeID"] = (string?)element.Element(ns + "LicenceeID") ?? (object)DBNull.Value;
            row["LicenceeName"] = (string?)element.Element(ns + "LicenceeName") ?? (object)DBNull.Value;
            row["AllowableType"] = (string?)element.Element(ns + "AllowableType") ?? (object)DBNull.Value;
            row["BlockNumber"] = int.TryParse((string?)element.Element(ns + "BlockNumber"), out var blockNumber) ? (object)blockNumber : DBNull.Value;
            row["RecoveryMechanismType"] = (string?)element.Element(ns + "RecoveryMechanismType") ?? (object)DBNull.Value;
            row["OrphanWellFlg"] = (string?)element.Element(ns + "OrphanWellFlg") ?? (object)DBNull.Value;

            return row;
        }

        private static DataRow ParseLinkedFacilityElement(XElement element, string wellId, DataTable linkedFacilityTable)
        {
            // Define the namespace explicitly based on the XML structure
            XNamespace ns = "WellInfratructure"; // Correct the typo if needed

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
            row["LinkedFacilityOperatorLegalName"] = (string?)element.Element(ns + "LinkedFacilityOperatorLegalName") ?? (object)DBNull.Value;
            return row;
        }

        private static DataRow ParseCommingledWellElement(XElement element, string wellId, DataTable commingledWellTable)
        {
            // Define the namespace explicitly based on the XML structure
            XNamespace ns = "WellInfratructure"; // Correct the typo if needed

            DataRow row = commingledWellTable.NewRow();
            row["ComminglingProcessApprovalNumber"] = (string?)element.Element(ns + "ComminglingProcessApprovalNumber") ?? (object)DBNull.Value;
            row["WellID"] = wellId ?? (object)DBNull.Value;
            row["ComminglingProcess"] = (string?)element.Element(ns + "ComminglingProcess") ?? (object)DBNull.Value;
            row["ComminglingEffDate"] = DateTime.TryParse((string?)element.Element(ns + "ComminglingEffDate"), out var commEffDate) ? (object)commEffDate : DBNull.Value;
            row["CommingledReportingWellID"] = (string?)element.Element(ns + "CommingledReportingWellID") ?? (object)DBNull.Value;
            row["CommingledReportingWellProvinceState"] = (string?)element.Element(ns + "CommingledReportingWellProvinceState") ?? (object)DBNull.Value;
            row["CommingledReportingWellType"] = (string?)element.Element(ns + "CommingledReportingWellType") ?? (object)DBNull.Value;
            row["CommingledReportingWellIdentifier"] = (string?)element.Element(ns + "CommingledReportingWellIdentifier") ?? (object)DBNull.Value;
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
