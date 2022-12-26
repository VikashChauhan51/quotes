using Quotes.API.Configurations;
using System.Runtime.CompilerServices;

namespace Quotes.API.Helpers;

public static class ConnectionStringBuilder
{

    public static string BuildConnectionString(SqlConfig sqlConfig)
    {
        DefaultInterpolatedStringHandler stringHandler= new DefaultInterpolatedStringHandler();
        stringHandler.AppendLiteral("Server=");
        stringHandler.AppendLiteral(sqlConfig.ServerName);
        stringHandler.AppendLiteral(";");
        stringHandler.AppendLiteral("Database=");
        stringHandler.AppendLiteral(sqlConfig.DbName);
        stringHandler.AppendLiteral(";");
        stringHandler.AppendFormatted(sqlConfig.Credentials); //User Id=sa;Password=Welcome@123;
        stringHandler.AppendLiteral("Trust Server Certificate=true;");
        return stringHandler.ToStringAndClear();
    }
}
