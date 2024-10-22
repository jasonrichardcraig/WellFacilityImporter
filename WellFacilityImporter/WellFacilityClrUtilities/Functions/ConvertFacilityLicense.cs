using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;

public partial class UserDefinedFunctions
{
    [SqlFunction]
    public static SqlString ConvertFacilityLicense(SqlString input)
    {
        if (input.IsNull || input.Value.Length < 2)
        {
            return SqlString.Null;
        }

        // Remove the leading character and extract the numeric part
        string numericPart = input.Value.Substring(1);

        // Pad the numeric part with leading zeros to ensure it's 7 characters long
        string result = numericPart.PadLeft(7, '0');

        return new SqlString(result);
    }
}
