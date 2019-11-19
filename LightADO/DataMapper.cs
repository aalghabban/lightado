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
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System;
    using static LightADO.Types;

    /// <summary>
    /// Provide access to methods that manage vice versa Mapping between SQL and CLI types.
    /// </summary>
    public class DataMapper
    {
        /// <summary>
        /// Convert Data table to List of T Object.
        /// </summary>
        /// <typeparam name="T">The type of object to convert data table to it.</typeparam>
        /// <param name="table">the data table to convert.</param>
        /// <param name="onError">in case of error will throw this event</param>
        /// <returns>a List of T objects</returns>
        public static List<T> ConvertDataTableToListOfObject<T>(DataTable table, OnError onError = null)
        {
            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                if (properties != null && properties.Length > 0)
                {
                    List<T> objList = new List<T>();
                    foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
                    {
                        objList.Add(EncryptEngine.EncryptOrDecryptObject<T>(MapDataRowToObject<T>(row, onError), OprationType.Descrypt));
                    }

                    return objList;
                }
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, string.Empty);
            }

            return null;
        }

        /// <summary>
        /// Convert data table to single object.
        /// </summary>
        /// <typeparam name="T">The type of object to convert data table to it.</typeparam>
        /// <param name="table">the data table to convert.</param>
        /// <param name="onError">in case of error will throw this event</param>
        /// <returns>a single T object</returns>
        public static T ConvertDataTableToObject<T>(DataTable table, OnError onError = null)
        {
            try
            {
                if (table != null && table.Rows.Count > 0)
                {
                    return EncryptEngine.EncryptOrDecryptObject<T>(DataMapper.MapDataRowToObject<T>(table.Rows[0], onError), OprationType.Descrypt);
                }
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, string.Empty);
            }

            return default;
        }

        /// <summary>
        /// Convert Data Table to DataSet.
        /// </summary>
        /// <param name="dataTable">data table to convert.</param>
        /// <returns>a data set object</returns>
        public static DataSet ConvertDataTableToDataSet(DataTable dataTable)
        {
            DataSet dataSet = null;
            if (dataTable != null)
            {
                dataSet = new DataSet();
                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }

        /// <summary>
        /// Map a data row to a single object.
        /// </summary>
        /// <typeparam name="T">the type of object.</typeparam>
        /// <param name="row">the row to convert to object.</param>
        /// <param name="onError">in case of error will throw this event</param>
        /// <returns>the object after it get mapped.</returns>
        public static T MapDataRowToObject<T>(DataRow row, OnError onError)
        {
            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                T instance = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetCustomAttributes(typeof(Ignore), false).Length == 0)
                    {
                        MapPropertyOfObject(instance, row, property, onError);
                    }
                }

                DefaultValue.SetDefaultValus(instance, Directions.WithQuery);
                return instance;
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, string.Empty);
            }

            return default;
        }

        /// <summary>
        /// Map Object properties to stored Procedure parameters.
        /// </summary>
        /// <typeparam name="T">the type of object to map.</typeparam>
        /// <param name="command">The SQL query to map the object to it.</param>
        /// <param name="objectToMap">object to map to stored Procedure.</param>
        /// <param name="setting">the database settings</param>
        /// <param name="onError">in case of error will throw this event</param>
        /// <param name="parameters">an array of parameters to map to stored Procedure</param>
        /// <returns>a list of parameters.</returns>
        internal static List<Parameter> MapObjectToStoredProcedure<T>(string command, T objectToMap, LightADOSetting setting, OnError onError, params Parameter[] parameters)
        {
            List<Parameter> parameterList = new List<Parameter>();
            try
            {
                DefaultValue.SetDefaultValus(objectToMap, Directions.WithNonQuery);
                foreach (StoredProcedureParameter parameter in new StoredProcedureParameter(command, setting).Parameters)
                {
                    string storedProcedureParameterName = parameter.Name.Remove(0, 1);
                    PropertyInfo property = objectToMap.GetType().GetProperty(storedProcedureParameterName);
                    if (property != null)
                    {
                        if (property.GetCustomAttributes(typeof(ForeignKey), false).Length == 0)
                        {
                            object propertyValue = property.GetValue(objectToMap);
                            if (property.PropertyType.IsEnum)
                            {
                                if (GetCSharpType((SqlDbType)Enum.Parse(typeof(SqlDbType), parameter.TypeName, true)) == typeof(string))
                                {
                                    parameterList.Add(new Parameter(storedProcedureParameterName, propertyValue.ToString(), parameter.GetParameterDirection));
                                }
                                else
                                {
                                    parameterList.Add(new Parameter(storedProcedureParameterName, (int)propertyValue, parameter.GetParameterDirection));
                                }
                            }
                            else
                            {
                                parameterList.Add(new Parameter(storedProcedureParameterName, propertyValue, parameter.GetParameterDirection));
                            }
                        }
                        else
                        {
                            if (property.GetCustomAttributes(typeof(CreateOnNotExists), false).Length > 0)
                            {
                                string crerateOnNotExisitMethodName = ((CreateOnNotExists)property.GetCustomAttribute(typeof(CreateOnNotExists), false)).UseThisMethod;
                                if (property.GetType().GetMethod(crerateOnNotExisitMethodName) == null)
                                {
                                    throw new LightAdoExcption(string.Format("The object {0}, dont't have a method named {1}", property.GetType().ToString(), crerateOnNotExisitMethodName));
                                }
                                property.SetValue(objectToMap, property.GetType().GetMethod(crerateOnNotExisitMethodName).Invoke(null, new object[1] { property.GetValue(objectToMap) }));
                                GetPrimaryKeyValue(objectToMap, parameterList, parameter, storedProcedureParameterName, onError);
                            }
                            else
                            {
                                GetPrimaryKeyValue(objectToMap, parameterList, parameter, storedProcedureParameterName, onError);
                            }

                        }
                    }
                    else if (Array.Find(parameters, x => parameter.Name == x.Name) != null)
                    {
                        parameterList.Add(new Parameter(storedProcedureParameterName, Array.Find(parameters, x => parameter.Name == x.Name).Value, parameter.GetParameterDirection));
                    }
                    else
                    {
                        SearchForCustomColumnNames(objectToMap, parameterList, parameter, storedProcedureParameterName, onError);
                    }
                }
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, string.Empty);
            }

            return parameterList;
        }

        /// <summary>
        /// Search for property with custom column name 
        /// attributes attached to it.
        /// </summary>
        /// <typeparam name="T">the Type of object to search in it's properties.</typeparam>
        /// <param name="objectToMap">object to search in.</param>
        /// <param name="mappedParameters">the mapped parameters</param>
        /// <param name="parameter">the parameter to search for.</param>
        /// <param name="currentParameteNameInStoredProcedure">current parameter in the stored procedure.</param>
        /// <param name="onError">in case of error will throw this event</param>
        private static void SearchForCustomColumnNames<T>(T objectToMap, List<Parameter> mappedParameters, StoredProcedureParameter parameter, string currentParameteNameInStoredProcedure, OnError onError)
        {
            Func<PropertyInfo, bool> predicate = null;

            try
            {
                PropertyInfo[] properties = objectToMap.GetType().GetProperties();
                predicate = p => p.GetCustomAttributes(typeof(ColumnName), true).Length > 0;
                if (predicate != null)
                {
                    foreach (PropertyInfo propertyInfo in properties.Where(predicate))
                    {
                        string columnName = GetColumnName(propertyInfo, onError);
                        if (columnName == currentParameteNameInStoredProcedure)
                        {
                            if (objectToMap.GetType().GetProperty(propertyInfo.Name).GetCustomAttributes(typeof(ForeignKey), false).Length > 0)
                            {
                                GetPrimaryKeyValue(objectToMap, mappedParameters, parameter, columnName, propertyInfo.Name, onError);
                                break;
                            }

                            mappedParameters.Add(new Parameter(currentParameteNameInStoredProcedure, objectToMap.GetType().GetProperty(propertyInfo.Name).GetValue(objectToMap), parameter.GetParameterDirection));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, string.Empty);
            }
        }

        /// <summary>
        /// Map Property Of Object 
        /// </summary>
        /// <typeparam name="T">the T type of the object.</typeparam>
        /// <param name="item">the object to map.</param>
        /// <param name="row">the data row to map.</param>
        /// <param name="propertyInfo">the property info to map</param>
        /// <param name="onError">in case any error</param>
        private static void MapPropertyOfObject<T>(T item, DataRow row, PropertyInfo propertyInfo, OnError onError)
        {
            bool isForignKey = propertyInfo.GetCustomAttributes(typeof(ForeignKey), false).Length > 0;
            string columnName = DataMapper.GetColumnName(propertyInfo, onError);

            try
            {
                if (row.Table.Columns[columnName] == null)
                {
                    return;
                }

                if (isForignKey == false)
                {
                    if (DBNull.Value.Equals(row[columnName]))
                    {
                        propertyInfo.SetValue(item, null);
                    }
                    else if (propertyInfo.PropertyType.IsEnum)
                    {
                        propertyInfo.SetValue(item, Enum.Parse(propertyInfo.PropertyType, row[columnName].ToString(), true));
                    }
                    else if (row[columnName] is string)
                    {
                        propertyInfo.SetValue(item, Convert.ChangeType(new string(row[columnName].ToString().Trim().Where(c => !char.IsControl(c)).ToArray()), propertyInfo.PropertyType));
                    }
                    else
                    {
                        Type type = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                        if (type is null)
                        {
                            type = propertyInfo.PropertyType;
                        }

                        Type conversionType = type;
                        object obj = row[columnName] == null ? null : Convert.ChangeType(row[columnName], conversionType);
                        propertyInfo.SetValue(item, obj);
                    }
                }
                else
                {
                    MapForeignObject(item, row, propertyInfo, onError);
                }
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, columnName);
            }
        }

        /// <summary>
        /// Get The custom column name.
        /// </summary>
        /// <param name="propertyInfo">property to read the column name.</param>
        /// <param name="onError">in case any error</param>
        /// <returns>the name of column.</returns>
        private static string GetColumnName(PropertyInfo propertyInfo, OnError onError)
        {
            try
            {
                string name = propertyInfo.Name;
                ColumnName columnName = propertyInfo.GetCustomAttribute<ColumnName>(true);

                if (columnName != null)
                {
                    name = columnName.Name;
                }

                return name;
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, string.Empty);
            }

            return null;
        }

        /// <summary>
        /// Fire the foreign key object Constructor.
        /// </summary>
        /// <typeparam name="T">the T type of the object to map.</typeparam>
        /// <param name="item">the object to map</param>
        /// <param name="row">the row data to convert.</param>
        /// <param name="propertyInfo">the property info to red.</param>
        /// <param name="onError">in case any error</param>
        private static void MapForeignObject<T>(T item, DataRow row, PropertyInfo propertyInfo, OnError onError)
        {
            try
            {
                Type propertyType = propertyInfo.PropertyType;

                if (propertyType.GetConstructor(new Type[1] { row[propertyInfo.Name].GetType() }) == null)
                {
                    return;
                }

                object instance = Activator.CreateInstance(propertyType, row[propertyInfo.Name]);
                typeof(T).InvokeMember(propertyInfo.Name, BindingFlags.SetProperty, null, item, new object[1] { instance });
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, string.Empty);
            }
        }

        /// <summary>
        /// Get Primary key value
        /// </summary>
        /// <typeparam name="T">the T type of object</typeparam>
        /// <param name="objectToMap">object to map</param>
        /// <param name="parameters">the list of parameters to map.</param>
        /// <param name="parameter">the parameter to check.</param>
        /// <param name="currentParameteName">current Parameter Name</param>
        /// <param name="onError">in case any error</param>
        private static void GetPrimaryKeyValue<T>(T objectToMap, List<Parameter> parameters, StoredProcedureParameter parameter, string currentParameteName, OnError onError)
        {
            try
            {
                object obj = objectToMap.GetType().GetProperty(currentParameteName).GetValue(objectToMap);
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    if (obj.GetType().GetProperty(property.Name).GetCustomAttributes(typeof(PrimaryKey), false).Length > 0)
                    {
                        parameters.Add(new Parameter(currentParameteName, obj.GetType().GetProperty(property.Name).GetValue(obj), parameter.GetParameterDirection));
                        return;
                    }
                }

                throw new LightAdoExcption(string.Format("primary key is Not defined in {0}", obj.GetType().ToString()));
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, string.Empty);
            }
        }

        /// <summary>
        /// Get Primary key value
        /// </summary>
        /// <typeparam name="T">the T type of object</typeparam>
        /// <param name="objectToMap">object to map</param>
        /// <param name="parameters">the list of parameters to map.</param>
        /// <param name="parameter">the parameter to check.</param>
        /// <param name="currentParameteName">current Parameter Name</param>
        /// <param name="propertyName">property Name to check.</param>
        /// <param name="onError">in case any error</param>
        private static void GetPrimaryKeyValue<T>(T objectToMap, List<Parameter> parameters, StoredProcedureParameter parameter, string currentParameteName, string propertyName, OnError onError)
        {
            try
            {
                object obj = objectToMap.GetType().GetProperty(propertyName).GetValue(objectToMap);
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    if (obj.GetType().GetProperty(property.Name).GetCustomAttributes(typeof(PrimaryKey), false).Length > 0)
                    {
                        parameters.Add(new Parameter(currentParameteName, obj.GetType().GetProperty(property.Name).GetValue(obj), parameter.GetParameterDirection));
                        return;
                    }
                }

                throw new LightAdoExcption(string.Format("primary key is Not defined in {0}", obj.GetType().ToString()));
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, string.Empty);
            }
        }

        /// <summary>
        /// Convert SQL type to CLI
        /// </summary>
        /// <param name="sqltype">the SQL type to convert.</param>
        /// <returns>the CLI Type</returns>
        private static Type GetCSharpType(SqlDbType sqltype)
        {
            new Dictionary<SqlDbType, Type>() {
                {
                    SqlDbType.BigInt,
                        typeof (long)
                }, {
                    SqlDbType.Binary,
                    typeof (byte[])
                }, {
                    SqlDbType.Bit,
                    typeof (bool)
                }, {
                    SqlDbType.Char,
                    typeof (string)
                }, {
                    SqlDbType.Date,
                    typeof (DateTime)
                }, {
                    SqlDbType.DateTime,
                    typeof (DateTime)
                }, {
                    SqlDbType.DateTime2,
                    typeof (DateTime)
                }, {
                    SqlDbType.DateTimeOffset,
                    typeof (DateTimeOffset)
                }, {
                    SqlDbType.Decimal,
                    typeof (decimal)
                }, {
                    SqlDbType.Float,
                    typeof (double)
                }, {
                    SqlDbType.Image,
                    typeof (byte[])
                }, {
                    SqlDbType.Int,
                    typeof (int)
                }, {
                    SqlDbType.Money,
                    typeof (decimal)
                }, {
                    SqlDbType.NChar,
                    typeof (string)
                }, {
                    SqlDbType.NText,
                    typeof (string)
                }, {
                    SqlDbType.NVarChar,
                    typeof (string)
                }, {
                    SqlDbType.Real,
                    typeof (float)
                }, {
                    SqlDbType.SmallDateTime,
                    typeof (DateTime)
                }, {
                    SqlDbType.SmallInt,
                    typeof (short)
                }, {
                    SqlDbType.SmallMoney,
                    typeof (decimal)
                }, {
                    SqlDbType.Text,
                    typeof (string)
                }, {
                    SqlDbType.Time,
                    typeof (TimeSpan)
                }, {
                    SqlDbType.Timestamp,
                    typeof (byte[])
                }, {
                    SqlDbType.TinyInt,
                    typeof (byte)
                }, {
                    SqlDbType.UniqueIdentifier,
                    typeof (Guid)
                }, {
                    SqlDbType.VarBinary,
                    typeof (byte[])
                }, {
                    SqlDbType.VarChar,
                    typeof (string)
                }
            }.TryGetValue(sqltype, out Type type);

            return type;
        }
    }
}