namespace LightADO
{
    using Microsoft.CSharp.RuntimeBinder;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class DataMapper
    {
        public static List<T> ConvertDataTableToListOfObject<T>(DataTable table, OnError onError = null)
        {
            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                if (properties != null && (uint)properties.Length > 0U)
                {
                    List<T> objList = new List<T>();
                    foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
                        objList.Add(EncryptEngine.EncryptOrDecryptObject<T>(DataMapper.MapDataRowToObject<T>(row, onError), false));
                    return objList;
                }
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, "");
            }
            return (List<T>)null;
        }

        public static T ConvertDataTableToObject<T>(DataTable table, OnError onError = null)
        {
            try
            {
                if (table != null && table.Rows.Count > 0)
                    return EncryptEngine.EncryptOrDecryptObject<T>(DataMapper.MapDataRowToObject<T>(table.Rows[0], onError), false);
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, "");
            }
            return default(T);
        }

        public static DataSet ConvertDataTableToDataSet(DataTable dataTable)
        {
            DataSet dataSet = (DataSet)null;
            if (dataTable != null)
            {
                dataSet = new DataSet();
                dataSet.Tables.Add(dataTable);
            }
            return dataSet;
        }

        internal static List<Parameter> MapObjectToStoredProcedure<T>(
          string command,
          T objectToMap,
          LightADOSetting setting,
          OnError onError,
          params Parameter[] parameters)
        {
            Func<PropertyInfo, bool> predicate = (Func<PropertyInfo, bool>)null;
            List<Parameter> parameterList = new List<Parameter>();
            try
            {
                foreach (StoredProcedureParameter parameter1 in new StoredProcedureParameter(command, setting).Parameters)
                {
                    StoredProcedureParameter parameter = parameter1;
                    string str = parameter.Name.Remove(0, 1);
                    if (objectToMap.GetType().GetProperty(str) != (PropertyInfo)null)
                    {
                        if (objectToMap.GetType().GetProperty(str).GetCustomAttributes(typeof(ForeignKey), false).Length == 0)
                        {
                            if (objectToMap.GetType().GetProperty(str).PropertyType.IsEnum)
                            {
                                if (DataMapper.GetCSharpType((SqlDbType)Enum.Parse(typeof(SqlDbType), parameter.TypeName, true)) == typeof(string))
                                {
                                    parameterList.Add(new Parameter(str, (object)objectToMap.GetType().GetProperty(str).GetValue((object)objectToMap).ToString(), parameter.GetParameterDirection));
                                }
                                else
                                {
                                    parameterList.Add(new Parameter(str, (object)(int)objectToMap.GetType().GetProperty(str).GetValue((object)objectToMap), parameter.GetParameterDirection));
                                }
                            }
                            else
                            {
                                parameterList.Add(new Parameter(str, objectToMap.GetType().GetProperty(str).GetValue((object)objectToMap), parameter.GetParameterDirection));
                            }
                        }
                        else
                        {
                            DataMapper.GetPrimaryKeyValueFromSubObject<T>(objectToMap, parameterList, parameter, str, onError);
                        }

                    }
                    else if (Array.Find<Parameter>(parameters, (Predicate<Parameter>)(x => parameter.Name == x.Name)) != null)
                    {
                        parameterList.Add(new Parameter(str, Array.Find<Parameter>(parameters, (Predicate<Parameter>)(x => parameter.Name == x.Name)).Value, parameter.GetParameterDirection));
                    }
                    else
                    {
                        IEnumerable<PropertyInfo> properties = (IEnumerable<PropertyInfo>)objectToMap.GetType().GetProperties();
                        if (predicate == null)
                            predicate = (Func<PropertyInfo, bool>)(p => (uint)p.GetCustomAttributes(typeof(ColumnName), true).Length > 0U);
                        if (!(properties.Where<PropertyInfo>(predicate).FirstOrDefault<PropertyInfo>() != (PropertyInfo)null))
                            throw new Exception(string.Format("Stored Procedure expect paramete named: {0},  which was not supplied", (object)parameter.Name.Remove(0, 1)));
                        DataMapper.SearchForCustomColumnNames<T>(objectToMap, parameterList, parameter, str, onError);
                    }
                }
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, "");
            }
            return parameterList;
        }

        private static Type GetCSharpType(SqlDbType sqltype)
        {
            Type type = (Type)null;
            new Dictionary<SqlDbType, Type>()
      {
        {
          SqlDbType.BigInt,
          typeof (long)
        },
        {
          SqlDbType.Binary,
          typeof (byte[])
        },
        {
          SqlDbType.Bit,
          typeof (bool)
        },
        {
          SqlDbType.Char,
          typeof (string)
        },
        {
          SqlDbType.Date,
          typeof (DateTime)
        },
        {
          SqlDbType.DateTime,
          typeof (DateTime)
        },
        {
          SqlDbType.DateTime2,
          typeof (DateTime)
        },
        {
          SqlDbType.DateTimeOffset,
          typeof (DateTimeOffset)
        },
        {
          SqlDbType.Decimal,
          typeof (Decimal)
        },
        {
          SqlDbType.Float,
          typeof (double)
        },
        {
          SqlDbType.Image,
          typeof (byte[])
        },
        {
          SqlDbType.Int,
          typeof (int)
        },
        {
          SqlDbType.Money,
          typeof (Decimal)
        },
        {
          SqlDbType.NChar,
          typeof (string)
        },
        {
          SqlDbType.NText,
          typeof (string)
        },
        {
          SqlDbType.NVarChar,
          typeof (string)
        },
        {
          SqlDbType.Real,
          typeof (float)
        },
        {
          SqlDbType.SmallDateTime,
          typeof (DateTime)
        },
        {
          SqlDbType.SmallInt,
          typeof (short)
        },
        {
          SqlDbType.SmallMoney,
          typeof (Decimal)
        },
        {
          SqlDbType.Text,
          typeof (string)
        },
        {
          SqlDbType.Time,
          typeof (TimeSpan)
        },
        {
          SqlDbType.Timestamp,
          typeof (byte[])
        },
        {
          SqlDbType.TinyInt,
          typeof (byte)
        },
        {
          SqlDbType.UniqueIdentifier,
          typeof (Guid)
        },
        {
          SqlDbType.VarBinary,
          typeof (byte[])
        },
        {
          SqlDbType.VarChar,
          typeof (string)
        }
      }.TryGetValue(sqltype, out type);
            return type;
        }

        private static void SearchForCustomColumnNames<T>(
          T objectToMap,
          List<Parameter> mappedParameters,
          StoredProcedureParameter parameter,
          string currentParameteNameInStoredProcedure,
          OnError onError)
        {
            Func<PropertyInfo, bool> predicate = (Func<PropertyInfo, bool>)null;
            try
            {
                IEnumerable<PropertyInfo> properties = (IEnumerable<PropertyInfo>)objectToMap.GetType().GetProperties();
                if (predicate == null)
                    predicate = (Func<PropertyInfo, bool>)(p => (uint)p.GetCustomAttributes(typeof(ColumnName), true).Length > 0U);
                foreach (PropertyInfo propertyInfo in properties.Where<PropertyInfo>(predicate))
                {
                    string columnName = DataMapper.GetColumnName(propertyInfo, onError);
                    if (columnName == currentParameteNameInStoredProcedure)
                    {
                        if ((uint)objectToMap.GetType().GetProperty(propertyInfo.Name).GetCustomAttributes(typeof(ForeignKey), false).Length > 0U)
                        {
                            DataMapper.GetPrimaryKeyValueFromSubObject<T>(objectToMap, mappedParameters, parameter, columnName, propertyInfo.Name, onError);
                            break;
                        }
                        mappedParameters.Add(new Parameter(currentParameteNameInStoredProcedure, objectToMap.GetType().GetProperty(propertyInfo.Name).GetValue((object)objectToMap), parameter.GetParameterDirection));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, "");
            }
        }

        private static bool GetPrimaryKeyValueFromSubObject<T>(
          T objectToMap,
          List<Parameter> parameters,
          StoredProcedureParameter parameter,
          string currentParameteName,
          string propertyName,
          OnError onError)
        {
            try
            {
                object obj = objectToMap.GetType().GetProperty(propertyName).GetValue((object)objectToMap);
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    if ((uint)obj.GetType().GetProperty(property.Name).GetCustomAttributes(typeof(PrimaryKey), false).Length > 0U)
                    {
                        parameters.Add(new Parameter(currentParameteName, obj.GetType().GetProperty(property.Name).GetValue(obj), parameter.GetParameterDirection));
                        return true;
                    }
                }
                throw new Exception(string.Format("primary key is Not defined in {0}", (object)obj.GetType().ToString()));
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, "");
            }
            return false;
        }

        public static T MapDataRowToObject<T>(DataRow row, OnError onError)
        {
            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                T instance = Activator.CreateInstance<T>();
                foreach (PropertyInfo propertyInfo in properties)
                    DataMapper.MapPropertyOfObject<T>(instance, row, propertyInfo, onError);
                return instance;
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, "");
            }
            return default(T);
        }

        private static void MapPropertyOfObject<T>(
          T item,
          DataRow row,
          PropertyInfo propertyInfo,
          OnError onError)
        {
            bool flag = (uint)propertyInfo.GetCustomAttributes(typeof(ForeignKey), false).Length > 0U;
            string columnName = DataMapper.GetColumnName(propertyInfo, onError);
            try
            {
                if (row.Table.Columns[columnName] == null)
                    return;
                if (!flag)
                {
                    if (DBNull.Value.Equals(row[columnName]))
                        propertyInfo.SetValue((object)item, (object)null);
                    else if (propertyInfo.PropertyType.IsEnum)
                        propertyInfo.SetValue((object)item, Enum.Parse(propertyInfo.PropertyType, row[columnName].ToString(), true));
                    else if (row[columnName] is string)
                    {
                        propertyInfo.SetValue((object)item, Convert.ChangeType((object)new string(row[columnName].ToString().Trim().Where<char>((Func<char, bool>)(c => !char.IsControl(c))).ToArray<char>()), propertyInfo.PropertyType));
                    }
                    else
                    {
                        Type type = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                        if ((object)type == null)
                            type = propertyInfo.PropertyType;
                        Type conversionType = type;
                        object obj = row[columnName] == null ? (object)null : Convert.ChangeType(row[columnName], conversionType);
                        propertyInfo.SetValue((object)item, obj);
                    }
                }
                else
                    DataMapper.MapForeignObject<T>(item, row, propertyInfo, onError);
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, columnName);
            }
        }

        private static string GetColumnName(PropertyInfo propertyInfo, OnError onError)
        {
            try
            {
                string name = propertyInfo.Name;
                object[] customAttributes = propertyInfo.GetCustomAttributes(true);
                if (customAttributes != null && (uint)customAttributes.Length > 0U)
                {
                    foreach (object obj in customAttributes)
                    {
                        if (obj.GetType().Name == "ColumnName")
                            name = obj.GetType().GetProperty("Name").GetValue(obj).ToString();
                    }
                }
                return name;
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, "");
            }
            return (string)null;
        }

        private static void MapForeignObject<T>(
          T item,
          DataRow row,
          PropertyInfo propertyInfo,
          OnError onError)
        {
            try
            {
                Type propertyType = propertyInfo.PropertyType;
                if (!(propertyType.GetConstructor(new Type[1]
                {
          row[propertyInfo.Name].GetType()
                }) != (ConstructorInfo)null))
                    return;
                object instance = Activator.CreateInstance(propertyType, row[propertyInfo.Name]);
                typeof(T).InvokeMember(propertyInfo.Name, BindingFlags.SetProperty, (System.Reflection.Binder)null, (object)item, new object[1]
                {
          instance
                });
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, "");
            }
        }

        private static bool GetPrimaryKeyValueFromSubObject<T>(
          T objectToMap,
          List<Parameter> parameters,
          StoredProcedureParameter parameter,
          string currentParameteName,
          OnError onError)
        {
            try
            {
                object obj = objectToMap.GetType().GetProperty(currentParameteName).GetValue((object)objectToMap);
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    if ((uint)obj.GetType().GetProperty(property.Name).GetCustomAttributes(typeof(PrimaryKey), false).Length > 0U)
                    {
                        parameters.Add(new Parameter(currentParameteName, obj.GetType().GetProperty(property.Name).GetValue(obj), parameter.GetParameterDirection));
                        return true;
                    }
                }
                throw new Exception(string.Format("primary key is Not defined in {0}", (object)obj.GetType().ToString()));
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(onError, ex, "");
            }
            return false;
        }
    }
}
