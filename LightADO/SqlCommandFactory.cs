/*
 * Copyright (C) 2019 ALGHABBAn
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace LightADO
{
    using System.Data;
    using System.Data.SqlClient;

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
