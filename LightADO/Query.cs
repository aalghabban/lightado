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
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using static LightADO.Types;

    /// <summary>
    /// Provides a way to execute, map object from - to SQL Command.
    /// </summary>
    public class Query : QueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Query"/> class.
        /// </summary>
        public Query() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Query"/> class.
        /// </summary>
        /// <param name="connectionString">connection string or key name</param>
        public Query(string connectionString)
          : base(connectionString)
        {
        }

        /// <summary>
        /// will be fired Before Open Connection.
        /// </summary>
        public event BeforeOpenConnection BeforeConnectionOpened;

        /// <summary>
        /// will be fired Before Connection Closed.
        /// </summary>
        public event BeforeCloseConnection BeforeConnectionClosed;

        /// <summary>
        /// will be fired After Connection Opened.
        /// </summary>
        public event AfterOpenConnection AfterConnectionOpened;

        /// <summary>
        /// will be fired After Connection Closed.
        /// </summary>
        public event AfterCloseConnection AfterConnectionClosed;

        /// <summary>
        /// will be fired Before Query Execute
        /// </summary>
        public event BeforeExecute BeforeQueryExecute;

        /// <summary>
        /// will be fired After Query Execute
        /// </summary>
        public event BeforeExecute AfterQueryExecute;

        /// <summary>
        /// will be fired On Error.
        /// </summary>
        public event LightADO.OnError OnError;

        /// <summary>
        /// Asynchronous Execute To Data Table
        /// </summary>
        /// <param name="command">SQL Command To Execute.</param>
        /// <param name="commandType">Command Type text or SP</param>
        /// <param name="parameters">parameters of the command</param>
        /// <returns>a data table object</returns>
        public Task<DataTable> ExecuteToDataTableAsync(string command, CommandType commandType = CommandType.StoredProcedure, params Parameter[] parameters)
        {
            return Task.FromResult<DataTable>(this.ExecuteToDataTable(command, commandType, parameters));
        }

        /// <summary>
        /// Execute To Data Table
        /// </summary>
        /// <param name="command">SQL Command To Execute.</param>
        /// <param name="commandType">Command Type text or SP</param>
        /// <param name="parameters">parameters of the command</param>
        /// <returns>a data table object</returns>
        public DataTable ExecuteToDataTable(string command, CommandType commandType = CommandType.StoredProcedure, params Parameter[] parameters)
        {
            return this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters));
        }

        /// <summary>
        /// Execute To Data Set
        /// </summary>
        /// <param name="command">SQL Command To Execute.</param>
        /// <param name="commandType">Command Type text or SP</param>
        /// <param name="parameters">parameters of the command</param>
        /// <returns>a data Set object</returns>
        public Task<DataSet> ExecuteToDataSetAsync(string command, CommandType commandType = CommandType.StoredProcedure, params Parameter[] parameters)
        {
            return Task.FromResult<DataSet>(this.ExecuteToDataSet(command, commandType, parameters));
        }

        /// <summary>
        /// Execute To Data Set
        /// </summary>
        /// <param name="command">SQL Command To Execute.</param>
        /// <param name="commandType">Command Type text or SP</param>
        /// <param name="parameters">parameters of the command</param>
        /// <returns>a data Set object</returns>
        public DataSet ExecuteToDataSet(string command, CommandType commandType = CommandType.StoredProcedure, params Parameter[] parameters)
        {
            return DataMapper.ConvertDataTableToDataSet(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)));
        }

        /// <summary>
        /// Execute Command and map result to Object
        /// </summary>
        /// <typeparam name="T">The T Type of the object</typeparam>
        /// <param name="command">Command To Execute.</param>
        /// <param name="commandType">the command type text or SP</param>
        /// <param name="parameters">the parameters of the Command</param>
        /// <returns>a single T object</returns>
        public Task<T> ExecuteToObjectAsync<T>(string command, CommandType commandType = CommandType.StoredProcedure, params Parameter[] parameters)
        {
            return Task.FromResult<T>(this.ExecuteToObject<T>(command, commandType, parameters));
        }

        /// <summary>
        /// Execute Command and map result to Object
        /// </summary>
        /// <typeparam name="T">The T Type of the object</typeparam>
        /// <param name="command">Command To Execute.</param>
        /// <param name="commandType">the command type text or SP</param>
        /// <param name="parameters">the parameters of the Command</param>
        /// <returns>a single T object</returns>
        public T ExecuteToObject<T>(string command, CommandType commandType = CommandType.StoredProcedure, params Parameter[] parameters)
        {
            return DataMapper.ConvertDataTableToObject<T>(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)), this.OnError);
        }

        /// <summary>
        /// Execute Command and map result to Object
        /// </summary>
        /// <typeparam name="T">The T Type of the object</typeparam>
        /// <param name="command">Command To Execute.</param>
        /// <param name="mapResultToThisObject">Map result to this object</param>
        /// <param name="commandType">the command type text or SP</param>
        /// <param name="parameters">the parameters of the Command</param>
        public void ExecuteToObject<T>(string command, T mapResultToThisObject, CommandType commandType = CommandType.StoredProcedure, params Parameter[] parameters)
        {
            T obj = DataMapper.ConvertDataTableToObject<T>(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)), this.OnError);
            if ((object)obj == null)
            {
                return;
            }

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                mapResultToThisObject.GetType().GetProperty(property.Name).SetValue((object)mapResultToThisObject, property.GetValue((object)obj));
            }
        }

        /// <summary>
        /// Execute To List Of Object
        /// </summary>
        /// <typeparam name="T">The type T of object</typeparam>
        /// <param name="command">command to execute.</param>
        /// <param name="commandType">command type text or SP.</param>
        /// <param name="parameters">parameters of the commands</param>
        /// <returns>a list of T.</returns>
        public Task<List<T>> ExecuteToListOfObjectAsync<T>(string command, CommandType commandType = CommandType.StoredProcedure, params Parameter[] parameters)
        {
            return Task.FromResult<List<T>>(DataMapper.ConvertDataTableToListOfObject<T>(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)), this.OnError));
        }
                
        /// <summary>
        /// Execute To List Of Object
        /// </summary>
        /// <typeparam name="T">The type T of object</typeparam>
        /// <param name="command">command to execute.</param>
        /// <param name="commandType">command type text or SP.</param>
        /// <param name="parameters">parameters of the commands</param>
        /// <returns>a list of T.</returns>
        public List<T> ExecuteToListOfObject<T>(string command, CommandType commandType = CommandType.StoredProcedure, params Parameter[] parameters)
        {
            return DataMapper.ConvertDataTableToListOfObject<T>(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)), this.OnError);
        }

        /// <summary>
        /// Execute command and get JSON or XML file.
        /// </summary>
        /// <typeparam name="T">the T type of the object</typeparam>
        /// <param name="command">command or SP name.</param>
        /// <param name="commandType">command as text or SP name</param>
        /// <param name="formatType">what format the result should be.</param>
        /// <param name="parameters">the command parameters.</param>
        /// <returns>a JSON or XML of the an object</returns>
        public Task<string> ExecuteToObjectAsync<T>(string command, CommandType commandType = CommandType.StoredProcedure, FormatType formatType = FormatType.XML, params Parameter[] parameters)
        {
            return Task.FromResult<string>(this.ExecuteToObject<T>(command, commandType, formatType, parameters));
        }

        /// <summary>
        /// Execute command and get JSON or XML file.
        /// </summary>
        /// <typeparam name="T">the T type of the object</typeparam>
        /// <param name="command">command or SP name.</param>
        /// <param name="commandType">command as text or SP name</param>
        /// <param name="formatType">what format the result should be.</param>
        /// <param name="parameters">the command parameters.</param>
        /// <returns>a JSON or XML of the an object</returns>
        public string ExecuteToObject<T>(string command, CommandType commandType = CommandType.StoredProcedure, FormatType formatType = FormatType.XML, params Parameter[] parameters)
        {
            T convertedObject = this.ExecuteToObject<T>(command, commandType, parameters);
            if (convertedObject != null)
            {
                switch (formatType)
                {
                    case FormatType.XML:
                        return this.SerializeToXml(convertedObject);
                    case FormatType.Json:
                        return JsonConvert.SerializeObject(convertedObject);
                }
            }

            return null;
        }

        /// <summary>
        /// Execute command and get JSON or XML file.
        /// </summary>
        /// <typeparam name="T">the T type of the object</typeparam>
        /// <param name="command">command or SP name.</param>
        /// <param name="commandType">command as text or SP name</param>
        /// <param name="formatType">what format the result should be.</param>
        /// <param name="parameters">the command parameters.</param>
        /// <returns>a List of JSON or XML of the an object</returns>
        public Task<List<string>> ExecuteToListOfObjectAsync<T>(string command, CommandType commandType = CommandType.StoredProcedure, FormatType formatType = FormatType.XML, params Parameter[] parameters)
        {
            return Task.FromResult<List<string>>(this.ExecuteToListOfObject<T>(command, commandType, formatType, parameters));
        }

        /// <summary>
        /// Execute command and get JSON or XML file.
        /// </summary>
        /// <typeparam name="T">the T type of the object</typeparam>
        /// <param name="command">command or SP name.</param>
        /// <param name="commandType">command as text or SP name</param>
        /// <param name="formatType">what format the result should be.</param>
        /// <param name="parameters">the command parameters.</param>
        /// <returns>a List of JSON or XML of the an object</returns>
        public List<string> ExecuteToListOfObject<T>(string command, CommandType commandType = CommandType.StoredProcedure, FormatType formatType = FormatType.XML, params Parameter[] parameters)
        {
            List<T> listOfObject = this.ExecuteToListOfObject<T>(command, commandType, parameters);
            List<string> convertedObjectList = null;
            if (listOfObject != null && listOfObject.Count > 0)
            {
                convertedObjectList = new List<string>();
                foreach (T obj in listOfObject)
                {
                    switch (formatType)
                    {
                        case FormatType.XML:
                            convertedObjectList.Add(this.SerializeToXml<T>(obj));
                            break;
                        case FormatType.Json:
                            convertedObjectList.Add(JsonConvert.SerializeObject(obj));
                            break;
                    }
                }
            }

            return convertedObjectList;
        }

        /// <summary>
        /// Execute Command And Get Data table.
        /// </summary>
        /// <param name="command">command to execute</param>
        /// <returns>a data table object</returns>
        private DataTable ExecuteToDataTable(SqlCommand command)
        {
            DataTable dataTable = (DataTable)null;
            try
            {
                if (command != null)
                {
                    this.BeforeConnectionOpened?.Invoke();
                    if (command.Connection.State == ConnectionState.Closed)
                    {
                        command.Connection.Open();
                    }

                    this.AfterConnectionOpened?.Invoke();
                    this.BeforeQueryExecute?.Invoke();
                    SqlDataReader sqlDataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    this.AfterQueryExecute?.Invoke();
                }
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(this.OnError, ex, string.Empty);
            }
            finally
            {
                this.BeforeConnectionClosed?.Invoke();
                if (command.Connection.State == ConnectionState.Open)
                {
                    command.Connection.Close();
                }

                this.AfterConnectionClosed?.Invoke();
            }

            return dataTable;
        }

        /// <summary>
        /// Convert Object to XML document.
        /// </summary>
        /// <typeparam name="T">The type of the object to convert.</typeparam>
        /// <param name="value">Object to convert.</param>
        /// <returns>an xml string</returns>
        private string SerializeToXml<T>(T value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new LightAdoExcption(ex);
            }
        }
    }
}