using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;

public partial class UserDefinedFunctions
{
    [SqlFunction]
    public static SqlString ConvertDlsToWellID(SqlString dlsString)
    {
        if (dlsString.IsNull || dlsString.Value.Length < 19)
        {
            return SqlString.Null;
        }

        string prefix = dlsString.Value.Substring(0, 2);
        string lsd = dlsString.Value.Substring(3, 2);
        string section = dlsString.Value.Substring(6, 2);
        string township = dlsString.Value.Substring(9, 3);
        string range = dlsString.Value.Substring(13, 2);
        string meridian = dlsString.Value.Substring(15, 2);
        string suffix = dlsString.Value.Substring(18, 1);

        // Ensure the suffix is two digits and prefix it with '0' if needed
        suffix = "0" + suffix;

        // Concatenate the result with '1' as the leading character
        string result = "1" + prefix + lsd + section + township + range + meridian + suffix;

        return new SqlString(result);
    }
}
