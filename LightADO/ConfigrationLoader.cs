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
    using System.IO;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Provides access to configuration files for client applications.
    /// </summary>
    internal static class ConfigurationLoader
    {
        /// <summary>
        /// returns the value of given key from app settings,
        /// app.config or web.config file
        /// </summary>
        /// <param name="keyName">the key name to read.</param>
        /// <param name="section">from where to read the key</param>
        /// <returns>a value of the given key</returns>
        internal static string GetValueOfKey(string keyName, Types.ConfigurationSections section = Types.ConfigurationSections.ConnectionString)
        {
            string value = null;
            IConfigurationBuilder builder = new ConfigurationBuilder();
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")))
            {
                IConfigurationRoot configRoot = builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")).Build();
                switch (section)
                {
                    case Types.ConfigurationSections.ConnectionString:
                        value = configRoot.GetConnectionString(keyName);
                        break;
                    case Types.ConfigurationSections.AppSettings:
                        value = configRoot.GetSection("AppSettings" + ":" + keyName).Value;

                        break;
                }
            }
            else
            {
                value = System.Environment.GetEnvironmentVariable(keyName);
            }

            return value;
        }
    }
}
