using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace WellFacilityImporter
{
    public class ComputedFieldsUpdater
    {
        public static void UpdateData(string connectionString)
        {

            Console.WriteLine($"Updating Database");

            using SqlConnection conn = new(connectionString);

            conn.Open();

            try
            {
                var sqlCommand = new SqlCommand(@"UPDATE [BusinessAssociate].[BusinessAssociate]
                                                    SET [FormattedLegalName] = [Converters].[CamelCaseString]([BALegalName])", conn);
                sqlCommand.ExecuteNonQuery();

                sqlCommand = new SqlCommand(@"UPDATE [FacilityInfrastructure].[Facility]
                                                SET [FormattedFacilityName] = [Converters].[CamelCaseString]([FacilityName])", conn);
                sqlCommand.ExecuteNonQuery();

                sqlCommand = new SqlCommand(@"UPDATE [WellInfrastructure].[Well]
                                               SET [FormattedWellIdentifier] = [Converters].[FormatWellIdentifier]([WellIdentifier])
                                                  ,[FormattedWellName] = [Converters].[CamelCaseString]([WellName])
                                                  ,[FormattedFieldName] = [Converters].[CamelCaseString]([FieldName])
                                                  ,[FormattedPoolDepositName] = [Converters].[CamelCaseString]([PoolDepositName])
	                                              ,[FormattedLicenseeName] = [Converters].[CamelCaseString]([LicenseeName])", conn);
                sqlCommand.ExecuteNonQuery();

                sqlCommand = new SqlCommand(@"UPDATE [WellWiki].[Well]
                                               SET [AlternateWellID] = [Converters].[ConvertDlsToWellID]([Location])
                                                  ,[FormattedWellName] = [Converters].[CamelCaseString]([WellName])", conn);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating database: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }

            Console.WriteLine();
            Console.WriteLine("Data update completed.");
        }
    }
}
