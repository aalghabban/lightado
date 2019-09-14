// Decompiled with JetBrains decompiler
// Type: LightADO.SqlCommandFactory
// Assembly: LightADO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 51CA6897-B553-4842-97D6-3F8C9C17C880
// Assembly location: C:\Users\ALGHABBAN\source\repos\ClassLibrary1\packages\LightAdo.net.4.6.0\lib\LightADO.dll

using System.Data;
using System.Data.SqlClient;

namespace LightADO
{
    internal class SqlCommandFactory
    {
        internal static SqlCommand Create(string commnadName, CommandType commandType, LightADOSetting lightAdoSetting,params Parameter[] parameters)
        {
            SqlCommand sqlCommand = new SqlCommand(commnadName, new SqlConnection(lightAdoSetting.ConnectionString));
            sqlCommand.CommandType = commandType;
            if (parameters != null && (uint)parameters.Length > 0U)
            {
                for (int index = 0; index < parameters.Length; ++index)
                {
                    Parameter parameter = parameters[index];
                    SqlParameter sqlParameter = new SqlParameter(parameter.Name, parameter.Value);
                    sqlParameter.Size = int.MaxValue;
                    sqlParameter.Direction = parameter.Direction;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
            }
            return sqlCommand;
        }

        internal static SqlCommand Create(string commnadName, CommandType commandType, LightADOSetting lightAdoSetting, SqlTransaction transaction, params Parameter[] parameters)
        {
            SqlCommand sqlCommand = new SqlCommand(commnadName, transaction.Connection);
            sqlCommand.CommandType = commandType;
            sqlCommand.Transaction = transaction;

            if (parameters != null && (uint)parameters.Length > 0U)
            {
                for (int index = 0; index < parameters.Length; ++index)
                {
                    Parameter parameter = parameters[index];
                    SqlParameter sqlParameter = new SqlParameter(parameter.Name, parameter.Value);
                    sqlParameter.Size = int.MaxValue;
                    sqlParameter.Direction = parameter.Direction;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
            }
            return sqlCommand;
        }
    }
}
