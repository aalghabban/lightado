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

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlCommandFactory"/> class.
    /// </summary>
    internal class SqlCommandFactory
    {
        /// <summary>
        /// Create a new SQL Command.
        /// </summary>
        /// <param name="commnadName">command name</param>
        /// <param name="commandType">wither command is text or stored procedure</param>
        /// <param name="lightAdoSetting">light ADO settings</param>
        /// <param name="parameters">parameters to send to the command</param>
        /// <returns>A SQL Command ready to execute.</returns>
        internal static SqlCommand Create(string commnadName, CommandType commandType, LightADOSetting lightAdoSetting, params Parameter[] parameters)
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

        /// <summary>
        /// Create a new SQL Command.
        /// </summary>
        /// <param name="commnadName">command name</param>
        /// <param name="commandType">command type or text.</param>
        /// <param name="lightAdoSetting">Light ADO settings.</param>
        /// <param name="transaction">transaction to execute.</param>
        /// <param name="parameters">parameters of the command.</param>
        /// <returns>SQL Command to execute.</returns>
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
