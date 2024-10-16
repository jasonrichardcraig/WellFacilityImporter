using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace WellFacilityImporter
{
	public class FacilityInfrastructureDataImporter
	{
		public static void ImportData(string connectionString, string filePath)
		{
			XNamespace ns = "FacilityInfrastructure";

			// Prepare list to hold data  
			List<DataRow> facilityInfrastructureRows = [];

			// Define DataTable schema  
			DataTable facilityInfrastructureTable = CreateFacilityInfrastructureDataTable();

			int counter = 0;

			using (XmlReader reader = XmlReader.Create(filePath))
			{
				while (reader.Read())
				{
					if (reader.IsStartElement("Facility", ns.NamespaceName))
					{
						if (XElement.ReadFrom(reader) is XElement facilityElement)
						{
							// Extract Facility data  
							DataRow facilityRow = ParseFacilityElement(facilityElement, facilityInfrastructureTable);
							facilityInfrastructureRows.Add(facilityRow);

							counter++;

							Console.Write($"\rProcessing count: {counter}");

							// Bulk insert every 1000 rows and clear lists
							if (counter % 1000 == 0)
							{
								// Bulk insert to SQL Server  
								BulkInsertToDatabase(facilityInfrastructureTable, facilityInfrastructureRows, connectionString, "FacilityInfrastructure.Facility");

								// Clear the list to free up memory
								facilityInfrastructureRows.Clear();
							}
						}
					}
				}
			}

			// Final insert for any remaining records that didn't make up a full batch of 1000
			if (facilityInfrastructureRows.Count > 0)
			{
				Console.WriteLine();
				Console.WriteLine("Bulk inserting remaining records to the database...");

				BulkInsertToDatabase(facilityInfrastructureTable, facilityInfrastructureRows, connectionString, "FacilityInfrastructure.Facility");
			}
			Console.WriteLine();
			Console.WriteLine("Data import completed.");
		}

		private static DataTable CreateFacilityInfrastructureDataTable()
		{
			DataTable table = new();
			table.Columns.Add("FacilityID", typeof(string));
			table.Columns.Add("FacilityProvinceState", typeof(string));
			table.Columns.Add("FacilityType", typeof(string));
			table.Columns.Add("FacilityIdentifier", typeof(string));
			table.Columns.Add("FacilityName", typeof(string));
			table.Columns.Add("FacilitySubType", typeof(string));
			table.Columns.Add("FacilitySubTypeDesc", typeof(string));
			table.Columns.Add("ExperimentalConfidential", typeof(string));
			table.Columns.Add("FacilityStartDate", typeof(DateTime));
			table.Columns.Add("FacilityLocation", typeof(string));
			table.Columns.Add("FacilityLegalSubdivision", typeof(string));
			table.Columns.Add("FacilitySection", typeof(int));
			table.Columns.Add("FacilityTownship", typeof(int));
			table.Columns.Add("FacilityRange", typeof(int));
			table.Columns.Add("FacilityMeridian", typeof(int));
			table.Columns.Add("FacilityLicenceStatus", typeof(string));
			table.Columns.Add("FacilityOperationalStatus", typeof(string));
			table.Columns.Add("FacilityOperationalStatusDate", typeof(DateTime));
			table.Columns.Add("LicenceType", typeof(string));
			table.Columns.Add("LicenceNumber", typeof(string));
			table.Columns.Add("EnergyDevelopmentCategoryType", typeof(string));
			table.Columns.Add("LicenceIssueDate", typeof(DateTime));
			table.Columns.Add("LicenseeBAID", typeof(string));
			table.Columns.Add("LicenseeName", typeof(string));
			table.Columns.Add("OperatorBAID", typeof(string));
			table.Columns.Add("OperatorName", typeof(string));
			table.Columns.Add("OperatorStartDate", typeof(DateTime));
			table.Columns.Add("TerminalPipelineLink", typeof(string));
			table.Columns.Add("TPFacilityProvinceState", typeof(string));
			table.Columns.Add("TPFacilityType", typeof(string));
			table.Columns.Add("TPFacilityIdentifier", typeof(string));
			table.Columns.Add("MeterStationPipelineLink", typeof(string));
			table.Columns.Add("MPFacilityProvinceState", typeof(string));
			table.Columns.Add("MPFacilityType", typeof(string));
			table.Columns.Add("MPFacilityIdentifier", typeof(string));
			table.Columns.Add("EnergyDevelopmentCategoryID", typeof(string));
			table.Columns.Add("OrphanWellFlg", typeof(string));
			table.Columns.Add("TierAggregateID", typeof(string));
			table.Columns.Add("TierAggregatePR", typeof(string));
			return table;
		}

		private static DataRow ParseFacilityElement(XElement element, DataTable facilityInfrastructureTable)
		{
			XNamespace ns = "FacilityInfrastructure";

			DataRow row = facilityInfrastructureTable.NewRow();

			row["FacilityID"] = (string?)element.Element(ns + "FacilityID") ?? (object)DBNull.Value;
			row["FacilityProvinceState"] = (string?)element.Element(ns + "FacilityProvinceState") ?? (object)DBNull.Value;
			row["FacilityType"] = (string?)element.Element(ns + "FacilityType") ?? (object)DBNull.Value;
			row["FacilityIdentifier"] = (string?)element.Element(ns + "FacilityIdentifier") ?? (object)DBNull.Value;
			row["FacilityName"] = (string?)element.Element(ns + "FacilityName") ?? (object)DBNull.Value;
			row["FacilitySubType"] = (string?)element.Element(ns + "FacilitySubType") ?? (object)DBNull.Value;
			row["FacilitySubTypeDesc"] = (string?)element.Element(ns + "FacilitySubTypeDesc") ?? (object)DBNull.Value;
			row["ExperimentalConfidential"] = (string?)element.Element(ns + "ExperimentalConfidential") ?? (object)DBNull.Value;
			row["FacilityStartDate"] = DateTime.TryParse((string?)element.Element(ns + "FacilityStartDate"), out var startDate) ? (object)startDate : DBNull.Value;
			row["FacilityLocation"] = (string?)element.Element(ns + "FacilityLocation") ?? (object)DBNull.Value;
			row["FacilityLegalSubdivision"] = (string?)element.Element(ns + "FacilityLegalSubdivision") ?? (object)DBNull.Value;
			row["FacilitySection"] = int.TryParse((string?)element.Element(ns + "FacilitySection"), out var section) ? (object)section : DBNull.Value;
			row["FacilityTownship"] = int.TryParse((string?)element.Element(ns + "FacilityTownship"), out var township) ? (object)township : DBNull.Value;
			row["FacilityRange"] = int.TryParse((string?)element.Element(ns + "FacilityRange"), out var range) ? (object)range : DBNull.Value;
			row["FacilityMeridian"] = int.TryParse((string?)element.Element(ns + "FacilityMeridian"), out var meridian) ? (object)meridian : DBNull.Value;
			row["FacilityLicenceStatus"] = (string?)element.Element(ns + "FacilityLicenceStatus") ?? (object)DBNull.Value;
			row["FacilityOperationalStatus"] = (string?)element.Element(ns + "FacilityOperationalStatus") ?? (object)DBNull.Value;
			row["FacilityOperationalStatusDate"] = DateTime.TryParse((string?)element.Element(ns + "FacilityOperationalStatusDate"), out var opStatusDate) ? (object)opStatusDate : DBNull.Value;
			row["LicenceType"] = (string?)element.Element(ns + "LicenceType") ?? (object)DBNull.Value;
			row["LicenceNumber"] = (string?)element.Element(ns + "LicenceNumber") ?? (object)DBNull.Value;
			row["EnergyDevelopmentCategoryType"] = (string?)element.Element(ns + "EnergyDevelopmentCategoryType") ?? (object)DBNull.Value;
			row["LicenceIssueDate"] = DateTime.TryParse((string?)element.Element(ns + "LicenceIssueDate"), out var licenceIssueDate) ? (object)licenceIssueDate : DBNull.Value;
			row["LicenseeBAID"] = (string?)element.Element(ns + "LicenseeBAID") ?? (object)DBNull.Value;
			row["LicenseeName"] = (string?)element.Element(ns + "LicenseeName") ?? (object)DBNull.Value;
			row["OperatorBAID"] = (string?)element.Element(ns + "OperatorBAID") ?? (object)DBNull.Value;
			row["OperatorName"] = (string?)element.Element(ns + "OperatorName") ?? (object)DBNull.Value;
			row["OperatorStartDate"] = DateTime.TryParse((string?)element.Element(ns + "OperatorStartDate"), out var operatorStartDate) ? (object)operatorStartDate : DBNull.Value;
			row["TerminalPipelineLink"] = (string?)element.Element(ns + "TerminalPipelineLink") ?? (object)DBNull.Value;
			row["TPFacilityProvinceState"] = (string?)element.Element(ns + "TPFacilityProvinceState") ?? (object)DBNull.Value;
			row["TPFacilityType"] = (string?)element.Element(ns + "TPFacilityType") ?? (object)DBNull.Value;
			row["TPFacilityIdentifier"] = (string?)element.Element(ns + "TPFacilityIdentifier") ?? (object)DBNull.Value;
			row["MeterStationPipelineLink"] = (string?)element.Element(ns + "MeterStationPipelineLink") ?? (object)DBNull.Value;
			row["MPFacilityProvinceState"] = (string?)element.Element(ns + "MPFacilityProvinceState") ?? (object)DBNull.Value;
			row["MPFacilityType"] = (string?)element.Element(ns + "MPFacilityType") ?? (object)DBNull.Value;
			row["MPFacilityIdentifier"] = (string?)element.Element(ns + "MPFacilityIdentifier") ?? (object)DBNull.Value;
			row["EnergyDevelopmentCategoryID"] = (string?)element.Element(ns + "EnergyDevelopmentCategoryID") ?? (object)DBNull.Value;
			row["OrphanWellFlg"] = (string?)element.Element(ns + "OrphanWellFlg") ?? (object)DBNull.Value;
			row["TierAggregateID"] = (string?)element.Element(ns + "TierAggregateID") ?? (object)DBNull.Value;
			row["TierAggregatePR"] = (string?)element.Element(ns + "TierAggregatePR") ?? (object)DBNull.Value;

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
