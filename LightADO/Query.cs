﻿namespace LightADO
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;

    public class Query : QueryBase
    {
        public event BeforeExecute BeforeQueryExecute;

        public event AfterExecute AfterQueryExecute;

        public event BeforeOpenConnection BeforeConnectionOpened;

        public event BeforeCloseConnection BeforeConnectionClosed;

        public event AfterOpenConnection AfterConnectionOpened;

        public event AfterCloseConnection AfterConnectionClosed;

        public event LightADO.OnError OnError;

        public Query() : base()
        {
        }

        public Query(string connectionString, bool loadFromConfigrationFile)
          : base(connectionString, loadFromConfigrationFile)
        {
        }

        public DataTable ExecuteToDataTable(
          string command,
          CommandType commandType = CommandType.StoredProcedure,
          params Parameter[] parameters)
        {
            return this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters));
        }

        public DataTable ExecuteToDataTable(
          string command,
          Exception nullException,
          CommandType commandType = CommandType.StoredProcedure,
          params Parameter[] parameters)
        {
            return this.CheckNull<DataTable>(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)), nullException);
        }

        public DataSet ExecuteToDataSet(
          string command,
          CommandType commandType = CommandType.StoredProcedure,
          params Parameter[] parameters)
        {
            return DataMapper.ConvertDataTableToDataSet(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)));
        }

        public DataSet ExecuteToDataSet(
          string command,
          Exception nullException,
          CommandType commandType = CommandType.StoredProcedure,
          params Parameter[] parameters)
        {
            return this.CheckNull<DataSet>(DataMapper.ConvertDataTableToDataSet(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters))), nullException);
        }

        public T ExecuteToObject<T>(
          string command,
          CommandType commandType = CommandType.StoredProcedure,
          params Parameter[] parameters)
        {
            return DataMapper.ConvertDataTableToObject<T>(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)), this.OnError);
        }

        public T ExecuteToObject<T>(
          string command,
          Exception nullException,
          CommandType commandType = CommandType.StoredProcedure,
          params Parameter[] parameters)
        {
            return this.CheckNull<T>(DataMapper.ConvertDataTableToObject<T>(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)), this.OnError), nullException);
        }

        public void ExecuteToObject<T>(
          string command,
          T mapResultToThisObject,
          CommandType commandType = CommandType.StoredProcedure,
          params Parameter[] parameters)
        {
            T obj = DataMapper.ConvertDataTableToObject<T>(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)), this.OnError);
            if ((object)obj == null)
                return;
            foreach (PropertyInfo property in obj.GetType().GetProperties())
                mapResultToThisObject.GetType().GetProperty(property.Name).SetValue((object)mapResultToThisObject, property.GetValue((object)obj));
        }

        public void ExecuteToObject<T>(
          string command,
          Exception nullException,
          T mapResultToThisObject,
          CommandType commandType = CommandType.StoredProcedure,
          params Parameter[] parameters)
        {
            T obj = DataMapper.ConvertDataTableToObject<T>(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)), this.OnError);
            if ((object)obj != null)
            {
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                    mapResultToThisObject.GetType().GetProperty(property.Name).SetValue((object)mapResultToThisObject, property.GetValue((object)obj));
            }
            else
                QueryBase.ThrowExacptionOrEvent(this.OnError, nullException, "");
        }

        public List<T> ExecuteToListOfObject<T>(
          string command,
          CommandType commandType = CommandType.StoredProcedure,
          params Parameter[] parameters)
        {
            return DataMapper.ConvertDataTableToListOfObject<T>(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)), this.OnError);
        }

        public List<T> ExecuteToListOfObject<T>(
          string command,
          Exception nullException,
          CommandType commandType = CommandType.StoredProcedure,
          params Parameter[] parameters)
        {
            return this.CheckNullList<T>(DataMapper.ConvertDataTableToListOfObject<T>(this.ExecuteToDataTable(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters)), this.OnError), nullException);
        }

        public string ExecuteToObject<T>(
          string command,
          CommandType commandType = CommandType.StoredProcedure,
          FormatType formatType = FormatType.XML,
          params Parameter[] parameters)
        {
            T obj = this.ExecuteToObject<T>(command, commandType, parameters);
            if ((object)obj != null)
            {
                switch (formatType)
                {
                    case FormatType.XML:
                        return this.SerializeToXml<T>(obj);
                    case FormatType.Json:
                        return JsonConvert.SerializeObject(obj);
                }
            }
            return (string)null;
        }

        public List<string> ExecuteToListOfObject<T>(
          string command,
          CommandType commandType = CommandType.StoredProcedure,
          FormatType formatType = FormatType.XML,
          params Parameter[] parameters)
        {
            List<T> listOfObject = this.ExecuteToListOfObject<T>(command, commandType, parameters);
            List<string> stringList1;
            if (listOfObject != null && listOfObject.Count > 0)
            {
                List<string> stringList2 = new List<string>();
                foreach (T obj in listOfObject)
                {
                    switch (formatType)
                    {
                        case FormatType.XML:
                            stringList2.Add(this.SerializeToXml<T>(obj));
                            break;
                        case FormatType.Json:
                            stringList2.Add(JsonConvert.SerializeObject(obj));
                            break;
                    }
                }
                stringList1 = stringList2;
            }
            else
                stringList1 = (List<string>)null;
            return stringList1;
        }

        public string ExecuteToObject<T>(
          string command,
          Exception nullException,
          CommandType commandType = CommandType.StoredProcedure,
          FormatType formatType = FormatType.XML,
          params Parameter[] parameters)
        {
            string str = this.ExecuteToObject<T>(command, commandType, formatType, parameters);
            if (string.IsNullOrWhiteSpace(str))
                return str;
            throw nullException;
        }

        public List<string> ExecuteToListOfObject<T>(
          string command,
          Exception nullException,
          CommandType commandType = CommandType.StoredProcedure,
          FormatType formatType = FormatType.XML,
          params Parameter[] parameters)
        {
            List<string> listOfObject = this.ExecuteToListOfObject<T>(command, commandType, formatType, parameters);
            if (listOfObject != null && listOfObject.Count > 0)
                return listOfObject;
            throw nullException;
        }

        private T CheckNull<T>(T objectToCheck, Exception exception)
        {
            if ((object)objectToCheck == null)
                QueryBase.ThrowExacptionOrEvent(this.OnError, exception, "");
            return objectToCheck;
        }

        private List<T> CheckNullList<T>(List<T> objectToCheck, Exception exception)
        {
            if (objectToCheck == null || objectToCheck.Count == 0)
                QueryBase.ThrowExacptionOrEvent(this.OnError, exception, "");
            return objectToCheck;
        }

        private List<object> CheckNullListDynamic(List<object> objectToCheck, Exception exception)
        {
            try
            {
                if (objectToCheck == null || objectToCheck.Count == 0)
                    QueryBase.ThrowExacptionOrEvent(this.OnError, exception, "");
                return objectToCheck;
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(this.OnError, ex, "");
            }
            return (List<object>)null;
        }

        private DataTable ExecuteToDataTable(SqlCommand command)
        {
            DataTable dataTable = (DataTable)null;
            try
            {
                if (command != null)
                {
                    if (this.BeforeConnectionOpened != null)
                        this.BeforeConnectionOpened();
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    if (this.AfterConnectionOpened != null)
                        this.AfterConnectionOpened();
                    if (this.BeforeQueryExecute != null)
                        this.BeforeQueryExecute();
                    SqlDataReader sqlDataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    dataTable = new DataTable();
                    dataTable.Load((IDataReader)sqlDataReader);
                    if (this.AfterQueryExecute != null)
                        this.AfterQueryExecute();
                }
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(this.OnError, ex, "");
            }
            finally
            {
                if (this.BeforeConnectionClosed != null)
                    this.BeforeConnectionClosed();
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
                if (this.AfterConnectionClosed != null)
                    this.AfterConnectionClosed();
            }
            return dataTable;
        }

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
                throw new Exception("An error occurred", ex);
            }
        }
    }
}
