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
