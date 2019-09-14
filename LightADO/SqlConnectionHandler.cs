// Decompiled with JetBrains decompiler
// Type: LightADO.SqlConnectionHandler
// Assembly: LightADO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 51CA6897-B553-4842-97D6-3F8C9C17C880
// Assembly location: C:\Users\ALGHABBAN\source\repos\ClassLibrary1\packages\LightAdo.net.4.6.0\lib\LightADO.dll

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
