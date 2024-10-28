using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Linq;

public partial class UserDefinedFunctions
{
    [SqlFunction(IsDeterministic = true)]
    public static SqlString CamelCaseString(SqlString input)
    {
        if (input.IsNull || input.Value.Length == 0)
            return SqlString.Null;

        string strInput = input.Value;
        bool capitalizeNext = true;

        var result = new string(
            strInput
            .Select((c, i) =>
            {
                if (c == '-' || c == '\'')
                {
                    capitalizeNext = true;
                    return c;
                }
                if (c == ' ')
                {
                    capitalizeNext = true;
                    return c;
                }

                if (capitalizeNext)
                {
                    capitalizeNext = false;
                    return char.ToUpper(c);
                }
                else
                {
                    return char.ToLower(c);
                }
            }).ToArray());

        return new SqlString(result);
    }
}