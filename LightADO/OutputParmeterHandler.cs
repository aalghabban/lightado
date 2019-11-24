/*
 * a.alghabban@icloud.com
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
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Data;
    using System.Reflection;
    using System;
    using System.Linq;

    /// <summary>
    /// Providers a way to get the output parameters from a SQL command execution.
    /// </summary>
    internal class OutputParmeterHandler
    {
        /// <summary>
        /// Get Output Parameters
        /// </summary>
        /// <param name="sqlCommand">SQL Command</param>
        /// <returns>the list of parameters</returns>
        internal static List<Parameter> GetOutputParamters(SqlCommand sqlCommand)
        {
            List<Parameter> parameterList = new List<Parameter>();
            foreach (SqlParameter parameter in sqlCommand.Parameters)
            {
                if (parameter.Direction == ParameterDirection.Output)
                {
                    parameterList.Add(new Parameter(parameter.ParameterName, parameter.Value, ParameterDirection.Input));
                }
            }

            return parameterList;
        }

        /// <summary>
        /// Set Output Parameter
        /// </summary>
        /// <typeparam name="T">type T of object</typeparam>
        /// <param name="sqlCommand">the SQL command to get the output parameter from it.</param>
        /// <param name="objectToMap">object to map</param>
        /// <param name="parameters">parameters to map</param>
        internal static void SetOutputParameter<T>(SqlCommand sqlCommand, T objectToMap, params Parameter[] parameters)
        {
            List<Parameter> outputParamters = OutputParmeterHandler.GetOutputParamters(sqlCommand);
            if (outputParamters == null || outputParamters.Count <= 0)
            {
                return;
            }

            foreach (Parameter parameter in outputParamters)
            {
                if (objectToMap.GetType().GetProperty(parameter.Name.Remove(0, 2)) != null)
                {
                    objectToMap.GetType().GetProperty(parameter.Name.Remove(0, 2)).SetValue(objectToMap, parameter.Value);
                }
                else if (Array.Find(parameters, x => parameter.Name.Remove(0, 1) == x.Name) != null)
                {
                    Array.Find(parameters, x => parameter.Name.Remove(0, 1) == x.Name).Value = parameter.Value;
                }
                else
                {
                    PropertyInfo[] properties = objectToMap.GetType().GetProperties();
                    Func<PropertyInfo, bool> predicate = null;
                    predicate = p => p.GetCustomAttributes(typeof(ColumnName), true).Length > 0;
                    if (predicate != null)
                    {
                        foreach (PropertyInfo propertyInfo in properties.Where(predicate))
                        {
                            ColumnName columnName = propertyInfo.GetCustomAttribute<ColumnName>(true);
                            if (columnName.Name == parameter.Name.Remove(0, 2))
                            {
                                objectToMap.GetType().GetProperty(propertyInfo.Name).SetValue(objectToMap, parameter.Value);
                            }
                        }
                    }
                }
            }
        }
    }
}