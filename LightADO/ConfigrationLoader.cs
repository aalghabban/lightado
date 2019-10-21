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
    using System.Configuration;
    using System.IO;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Provides access to configuration files for client applications.
    /// </summary>
    internal static class ConfigurationLoader
    {
        /// <summary>
        /// The Configuration Sections
        /// </summary>
        internal enum ConfigurationSections
        {
            /// <summary>
            /// Connection String Section
            /// </summary>
            ConnectionString,

            /// <summary>
            /// The App Settings section
            /// </summary>
            AppSettings
        }

        /// <summary>
        /// returns the value of given key from app settings,
        /// app.config or web.config file
        /// </summary>
        /// <param name="keyName">the key name to read.</param>
        /// <param name="section">from where to read the key</param>
        /// <returns>a value of the given key</returns>
        internal static string GetValueOfKey(string keyName, ConfigurationSections section = ConfigurationSections.ConnectionString)
        {
            string value = null;
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationRoot configRoot = builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")).Build();

            switch (section)
            {
                case ConfigurationSections.ConnectionString:
                    if (ConfigurationManager.ConnectionStrings[keyName] != null)
                    {
                        value = ConfigurationManager.ConnectionStrings[keyName].ConnectionString;
                    }
                    else
                    {
                        value = configRoot.GetConnectionString(keyName);
                    }

                    break;
                case ConfigurationSections.AppSettings:
                    if (ConfigurationManager.AppSettings[keyName] != null)
                    {
                        return ConfigurationManager.AppSettings[keyName];
                    }
                    else
                    {
                        value = configRoot.GetSection("AppSettings" + ":" + keyName).Value;
                    }

                    break;
            }

            return value;
        }
    }
}
