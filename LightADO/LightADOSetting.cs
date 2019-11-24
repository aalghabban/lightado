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
    using System;

    /// <summary>
    /// Providers an options to load light ADO settings.
    /// </summary>
    public sealed class LightADOSetting
    {
        /// <summary>
        /// data base connection string.
        /// </summary>
        private string connectionStirng = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightADOSetting"/> class.
        /// </summary>
        public LightADOSetting()
        {
            this.ConnectionString = this.LoadConnectionString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightADOSetting"/> class.
        /// with connection string.
        /// </summary>
        /// <param name="connectionString">connection string</param>
        public LightADOSetting(string connectionString)
        {
            if (SqlConnectionHandler.IsConnectionStringValid(connectionString) == true)
            {
                this.ConnectionString = connectionString;
            }
            else
            {
                this.ConnectionString = this.LoadConnectionString(connectionString);
            }
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.connectionStirng;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new LightAdoExcption("Can't set null or empty as connection string");
                }

                this.connectionStirng = SqlConnectionHandler.ValdiateGivenConnectionString(value);
            }
        }

        /// <summary>
        /// Load connection string from Configuration files
        /// </summary>
        /// <param name="connectionStringName">the connection string key name by default will DefaultConnection</param>
        /// <returns>connection string</returns>
        private string LoadConnectionString(string connectionStringName = "DefaultConnection")
        {
            string connectionString = ConfigurationLoader.GetValueOfKey(connectionStringName);
            if (connectionString == null)
            {
                throw new LightAdoExcption("Lightado did not find a connection string with name DefaultConnection, in both appsettings.json or the app.confg");
            }

            return connectionString;
        }
    }
}