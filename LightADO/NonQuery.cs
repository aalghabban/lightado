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
    using System.Data.SqlClient;
    using System.Data;
    using System.Threading.Tasks;
    using System;
    using static LightADO.Types;

    /// <summary>
    /// Providers a methods to execute non queries into a database.
    /// </summary>
    public class NonQuery : QueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonQuery"/> class.
        /// </summary>
        public NonQuery() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonQuery"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string or a key name in the configuration file</param>
        public NonQuery(string connectionString) : base(connectionString) { }

        /// <summary>
        /// will be fired before the Non Query get execute.
        /// </summary>
        public event BeforeExecute BeforeNonQueryExecute;

        /// <summary>
        /// will be fired after the Non Query get execute.
        /// </summary>
        public event AfterExecute AfterNonQueryExecute;

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
        /// will be fired On Error.
        /// </summary>
        public event LightADO.OnError OnError;

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="command">the SP or the SQL command as text.</param>
        /// <param name="commandType">the command type</param>
        /// <param name="parameters">any parameters needed by the query.</param>
        /// <returns>true if the query get Executes</returns>
        public bool Execute(string command, CommandType commandType = CommandType.Text, params Parameter[] parameters)
        {
            return this.ExcecuteNonQueryCommand(SqlCommandFactory.Create(command, commandType, this.LightAdoSetting, parameters));
        }

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="command">the SP or the SQL command as text.</param>
        /// <param name="commandType">the command type</param>
        /// <param name="parameters">any parameters needed by the query.</param>
        /// <returns>true if the query get Executes</returns>
        public Task<bool> ExecuteAsync(string command, CommandType commandType = CommandType.Text, params Parameter[] parameters)
        {
            return Task.FromResult<bool>(this.Execute(command, commandType, parameters));
        }

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <typeparam name="T">the Type of object to map</typeparam>
        /// <param name="command">the SP or the SQL command as text.</param>
        /// <param name="objectToMap">object to map</param>
        /// <param name="parameters">any parameters needed by the query.</param>
        /// <returns>true if the query get Executed</returns>
        public Task<bool> ExecuteAsync<T>(string command, T objectToMap, params Parameter[] parameters)
        {
            return Task.FromResult<bool>(this.Execute(command, objectToMap, parameters));
        }

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <typeparam name="T">the Type of object to map</typeparam>
        /// <param name="command">the SP or the SQL command as text.</param>
        /// <param name="objectToMap">object to map</param>
        /// <param name="parameters">any parameters needed by the query.</param>
        /// <returns>true if the query get Executed</returns>
        public bool Execute<T>(string command, T objectToMap, params Parameter[] parameters)
        {
            AutoValidation.ValidateObject<T>(objectToMap);
            EncryptEngine.EncryptOrDecryptObject<T>(objectToMap, OprationType.Encrypt);
            return this.ExcecuteNonQueryCommand<T>(SqlCommandFactory.Create(command, CommandType.StoredProcedure, this.LightAdoSetting, DataMapper.MapObjectToStoredProcedure<T>(command, objectToMap, this.LightAdoSetting, this.OnError, parameters).ToArray()), objectToMap, parameters);
        }

        /// <summary>
        /// Execute Stored Procedure with list of objects.
        /// </summary>
        /// <typeparam name="T">the Type of object to map</typeparam>
        /// <param name="command">the SP or the SQL command as text.</param>
        /// <param name="objectToMap">object to map</param>
        /// <param name="parameters">any parameters needed by the query.</param>
        /// <returns>true if the query get Executed</returns>
        public Task<bool> ExecuteAsync<T>(string command, List<T> objectToMap, params Parameter[] parameters)
        {
            return Task.FromResult<bool>(this.Execute(command, objectToMap, parameters));
        }

        /// <summary>
        /// Execute Stored Procedure with list of objects.
        /// </summary>
        /// <typeparam name="T">the Type of object to map</typeparam>
        /// <param name="command">the SP or the SQL command as text.</param>
        /// <param name="objectToMap">object to map</param>
        /// <param name="parameters">any parameters needed by the query.</param>
        /// <returns>true if the query get Executed</returns>
        public bool Execute<T>(string command, List<T> objectToMap, params Parameter[] parameters)
        {
            foreach (T obj in objectToMap)
            {
                AutoValidation.ValidateObject<T>(obj);
                EncryptEngine.EncryptOrDecryptObject<T>(obj, OprationType.Encrypt);
                this.ExcecuteNonQueryCommand<T>(SqlCommandFactory.Create(command, CommandType.StoredProcedure, this.LightAdoSetting, DataMapper.MapObjectToStoredProcedure<T>(command, obj, this.LightAdoSetting, this.OnError, parameters).ToArray()), obj, parameters);
            }

            return true;
        }

        /// <summary>
        /// Execute a Non Query Transaction Command.
        /// </summary>
        /// <param name="transactions">Transaction list to Execute</param>
        /// <param name="rollbackOnError">wither to Rollback or not</param>
        /// <returns>true if all Transaction Execute</returns>
        public Task<bool> ExecuteAsync(List<Transaction> transactions, bool rollbackOnError = true)
        {
            return Task.FromResult<bool>(this.Execute(transactions, rollbackOnError));
        }

        /// <summary>
        /// Execute a Non Query Transaction Command.
        /// </summary>
        /// <param name="transactions">Transaction list to Execute</param>
        /// <param name="rollbackOnError">wither to Rollback or not</param>
        /// <returns>true if all Transaction Execute</returns>
        public bool Execute(List<Transaction> transactions, bool rollbackOnError = true)
        {
            SqlConnection connection = new SqlConnection(this.LightAdoSetting.ConnectionString);
            connection.Open();

            SqlTransaction sqlTransaction = connection.BeginTransaction();
            try
            {
                foreach (Transaction transaction in transactions)
                {
                    this.ExcecuteNonQueryCommand(SqlCommandFactory.Create(transaction.Command, transaction.CommandType, this.LightAdoSetting, sqlTransaction, DataMapper.MapObjectToStoredProcedure(transaction.Command, transaction.Data, this.LightAdoSetting, this.OnError, transaction.Parameters).ToArray()), transaction.Data, true, transaction.Parameters);
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

                QueryBase.ThrowExacptionOrEvent(this.OnError, ex, string.Empty);
            }

            return false;
        }

        /// <summary>
        /// Execute NonQuery Command and get SQL command
        /// </summary>
        /// <param name="sqlCommand">the SQL commend to Execute or SP name</param>
        /// <param name="keepConnectionOpend">wither to keep the connection open or not.</param>
        /// <returns>SQL Command Object</returns>
        private SqlCommand ExcecuteNonQueryAndGetSqlCommand(SqlCommand sqlCommand, bool keepConnectionOpend = false)
        {
            try
            {
                this.BeforeConnectionOpened?.Invoke();
                if (sqlCommand.Connection.State == ConnectionState.Closed)
                {
                    sqlCommand.Connection.Open();
                }

                this.AfterConnectionOpened?.Invoke();
                this.BeforeNonQueryExecute?.Invoke();
                sqlCommand.ExecuteNonQuery();
                this.AfterNonQueryExecute?.Invoke();
            }
            catch (Exception ex)
            {
                QueryBase.ThrowExacptionOrEvent(this.OnError, ex, string.Empty);
            }
            finally
            {
                if (keepConnectionOpend == false)
                {
                    this.BeforeConnectionClosed?.Invoke();
                    if (sqlCommand.Connection.State == ConnectionState.Open)
                    {
                        sqlCommand.Connection.Close();
                    }

                    this.AfterConnectionClosed?.Invoke();
                }
            }

            return sqlCommand;
        }

        /// <summary>
        /// Execute NonQuery Command
        /// </summary>
        /// <param name="sqlCommand">the SQL commend to Execute or SP name</param>
        /// <returns>true if the query get Execute</returns>
        private bool ExcecuteNonQueryCommand(SqlCommand sqlCommand)
        {
            this.ExcecuteNonQueryAndGetSqlCommand(sqlCommand);
            return true;
        }

        /// <summary>
        /// Execute NonQuery Command
        /// </summary>
        /// <typeparam name="T">The T Type of the object to map.</typeparam>
        /// <param name="sqlCommand">the SQL commend to execute or SP name</param>
        /// <param name="objectToMap">the object to map</param>
        /// <param name="parameters">the list of parameters to map.</param>
        /// <returns>true if the query get Execute</returns>
        private bool ExcecuteNonQueryCommand<T>(SqlCommand sqlCommand, T objectToMap, params Parameter[] parameters)
        {
            OutputParmeterHandler.SetOutputParameter<T>(this.ExcecuteNonQueryAndGetSqlCommand(sqlCommand), objectToMap, parameters);
            return true;
        }

        /// <summary>
        /// Execute NonQuery Command
        /// </summary>
        /// <typeparam name="T">The T Type of the object to map.</typeparam>
        /// <param name="sqlCommand">the SQL commend to execute or SP name</param>
        /// <param name="objectToMap">the object to map</param>
        /// <param name="keepConnectionOpend">wither to keep the connection open or not.</param>
        /// <param name="parameters">the list of parameters to map.</param>
        /// <returns>true if the query get Execute</returns>
        private bool ExcecuteNonQueryCommand<T>(SqlCommand sqlCommand, T objectToMap, bool keepConnectionOpend = false, params Parameter[] parameters)
        {
            OutputParmeterHandler.SetOutputParameter<T>(this.ExcecuteNonQueryAndGetSqlCommand(sqlCommand, keepConnectionOpend), objectToMap, parameters);
            return true;
        }
    }
}