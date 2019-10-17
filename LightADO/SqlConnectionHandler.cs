namespace LightADO
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.FileExtensions;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.IO;

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
    }
}
