using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;

public partial class UserDefinedFunctions
{
    [SqlFunction(IsDeterministic = true)]

    public static SqlString FormatWellIdentifier(SqlString wellIdentifier)
    {
        if (wellIdentifier.IsNull || wellIdentifier.Value.Length < 16) // Adjust length if needed
        {
            return SqlString.Null;
        }

        // Format the string based on the required segments
        string formattedIdentifier = $"{wellIdentifier.Value.Substring(0, 3)}/" +
                                     $"{wellIdentifier.Value.Substring(3, 2)}-" +
                                     $"{wellIdentifier.Value.Substring(5, 2)}-" +
                                     $"{wellIdentifier.Value.Substring(7, 3)}-" +
                                     $"{wellIdentifier.Value.Substring(10, 2)}W" +
                                     $"{wellIdentifier.Value.Substring(13, 1)}/" +
                                     $"{wellIdentifier.Value.Substring(14, 2)}";

        return new SqlString(formattedIdentifier);
    }
}
