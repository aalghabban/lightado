namespace LightADO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class NonQuery : QueryBase
    {
        public event BeforeExecute BeforeNonQueryExecute;

        public event AfterExecute AfterNonQueryExecute;

        public event BeforeOpenConnection BeforeConnectionOpened;

        public event BeforeCloseConnection BeforeConnectionClosed;

        public event AfterOpenConnection AfterConnectionOpened;

        public event AfterCloseConnection AfterConnectionClosed;

        public event LightADO.OnError OnError;

        public NonQuery()
        {
        }

        public NonQuery(string connectionString, bool loadFromConfigration)
          : base(connectionString, loadFromConfigration)
        {
        }

        public bool Execute(string command, CommandType commandType = CommandType.Text, params Parameter[] parameters)
        {
            try
            {
                return this.ExcecuteNonQueryCommand(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters));
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(this.OnError, ex, "");
            }
            return false;
        }

        public bool Execute<T>(string command, T objectToMap, params Parameter[] parameters)
        {
            try
            {
                AutoValidation.ValidateObject<T>(objectToMap);
                EncryptEngine.EncryptOrDecryptObject<T>(objectToMap, true);
                return this.ExcecuteNonQueryCommand<T>(SqlCommandFactory.Create(command, CommandType.StoredProcedure, this.LightAdoSetting, DataMapper.MapObjectToStoredProcedure<T>(command, objectToMap, this.LightAdoSetting, this.OnError, parameters).ToArray()), objectToMap, parameters);
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(this.OnError, ex, "");
            }
            return false;
        }

        public bool Execute(List<Transaction> transactions, bool rollbackOnError = true)
        {
            SqlConnection connection = new SqlConnection(this.LightAdoSetting.ConnectionString);
            connection.Open();

            SqlTransaction sqlTransaction = connection.BeginTransaction();
            try
            {
                foreach (Transaction transaction in transactions)
                {
                    this.ExcecuteNonQueryCommand(
                        SqlCommandFactory.Create(transaction.Command, transaction.CommandType, this.LightAdoSetting, sqlTransaction, DataMapper.MapObjectToStoredProcedure(transaction.Command, transaction.Data, this.LightAdoSetting, this.OnError, transaction.Parameters).ToArray()),
                        transaction.Data, true, transaction.Parameters);
                }

                sqlTransaction.Commit();
                connection.Close();
            }
            catch (Exception ex)
            {
                if (rollbackOnError)
                {
                    sqlTransaction.Rollback();
                }

                QueryBase.ThrowExacptionOrEvent(this.OnError, ex, "");
            }

            return false;
        }

        private SqlCommand ExcecuteNonQueryAndGetSqlCommand(SqlCommand sqlCommand, bool keepConnectionOpend = false)
        {
            try
            {
                if (this.BeforeConnectionOpened != null)
                    this.BeforeConnectionOpened();
                if (sqlCommand.Connection.State == ConnectionState.Closed)
                    sqlCommand.Connection.Open();
                if (this.AfterConnectionOpened != null)
                    this.AfterConnectionOpened();
                if (this.BeforeNonQueryExecute != null)
                    this.BeforeNonQueryExecute();
                sqlCommand.ExecuteNonQuery();
                if (this.AfterNonQueryExecute != null)
                    this.AfterNonQueryExecute();
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(this.OnError, ex, "");
            }
            finally
            {
                if(keepConnectionOpend == false)
                {
                    if (this.BeforeConnectionClosed != null)
                        this.BeforeConnectionClosed();
                    if (sqlCommand.Connection.State == ConnectionState.Open)
                        sqlCommand.Connection.Close();
                    if (this.AfterConnectionClosed != null)
                        this.AfterConnectionClosed();
                }
            }
            return sqlCommand;
        }

        private bool ExcecuteNonQueryCommand(SqlCommand sqlCommand)
        {
            this.ExcecuteNonQueryAndGetSqlCommand(sqlCommand);
            return true;
        }

        private bool ExcecuteNonQueryCommand<T>(
          SqlCommand sqlCommand,
          T objectToMap,
          params Parameter[] parameters)
        {
            OutputParmeterHandler.SetOutputParameter<T>(this.ExcecuteNonQueryAndGetSqlCommand(sqlCommand), objectToMap, parameters);
            return true;
        }

        private bool ExcecuteNonQueryCommand<T>(SqlCommand sqlCommand, T objectToMap, bool keepConnectionOpend = false, params Parameter[] parameters)
        {
            OutputParmeterHandler.SetOutputParameter<T>(this.ExcecuteNonQueryAndGetSqlCommand(sqlCommand, keepConnectionOpend), objectToMap, parameters);
            return true;
        }
    }
}
