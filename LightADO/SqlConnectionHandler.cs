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
    using System.Data.SqlClient;
    using System;

    /// <summary>
    /// provides options to handle any connections matter
    /// </summary>
    internal class SqlConnectionHandler
    {
        /// <summary>
        /// Check if connection string is valid connection string.
        /// </summary>
        /// <param name="value">the value to check.</param>
        /// <returns>the connection string after it get validated</returns>
        internal static string ValdiateGivenConnectionString(string value)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(value);
                return value;
            }
            catch (Exception ex)
            {
                throw new LightAdoExcption(ex);
            }
        }

        /// <summary>
        /// is Connection string valid
        /// </summary>
        /// <param name="connectionString">connection string to validate</param>
        /// <returns>true if connection is valid otherwise it throw false</returns>
        internal static bool IsConnectionStringValid(string connectionString)
        {
            try
            {
                ValdiateGivenConnectionString(connectionString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}