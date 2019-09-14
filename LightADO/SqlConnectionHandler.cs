using System;
using System.Configuration;
using System.Data.SqlClient;

namespace LightADO
{
  internal class SqlConnectionHandler
  {
    internal static string ValdiateGivenConnectionString(string value)
    {
      try
      {
        SqlConnection sqlConnection = new SqlConnection(value);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return value;
    }

    internal static string LoadConnectionFromConfigrationFile(string connectionSettingName)
    {
      if (ConfigurationManager.ConnectionStrings[connectionSettingName] == null)
        throw new Exception(string.Format("No connection setting with name {0} was found in the connection Strings section", (object) connectionSettingName));
      string connectionString = ConfigurationManager.ConnectionStrings[connectionSettingName].ConnectionString;
      if (string.IsNullOrWhiteSpace(connectionString))
        throw new Exception("Can't set null or empty as connection string");
      SqlConnectionHandler.ValdiateGivenConnectionString(connectionString);
      return connectionString;
    }
  }
}
