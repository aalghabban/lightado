﻿/*
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

    public sealed class LightADOSetting
    {
        private string connectionStirng = string.Empty;
        private string encryptKey = string.Empty;

        public string ConnectionString
        {
            get
            {
                return this.connectionStirng;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Can't set null or empty as connection string");
                this.connectionStirng = SqlConnectionHandler.ValdiateGivenConnectionString(value);
            }
        }

        public string EncryptKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.encryptKey))
                {
                    this.encryptKey = ConfigurationLoader.GetValueOfKey("EncryptKey", ConfigurationLoader.ConfigurationSections.AppSettings);
                    if (string.IsNullOrWhiteSpace(this.encryptKey))
                        throw new Exception("The Encryption key is not set in the appSetting section of the configration file, Create new key with name EncryptKey and set the value of it to random string");
                }

                return this.encryptKey;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Encryption key is not set, in the configration file");
                this.encryptKey = value;
            }
        }

        public LightADOSetting()
        {
            this.ConnectionString = this.LoadConnectionString();
        }

        public LightADOSetting(string connectionString, bool loadFromConfigrationFile)
        {
            if (loadFromConfigrationFile)
                this.ConnectionString = this.LoadConnectionString(connectionString);
            else
                this.ConnectionString = connectionString;
        }

        private string LoadConnectionString(string connectionStringName = "DefaultConnection")
        {
            string connectionString = ConfigurationLoader.GetValueOfKey(connectionStringName);
            if (connectionString == null)
                throw new Exception("Lightado did not find a connection string with name DefaultConnection, in both appsettings.json or the app.confg");

            return connectionString;
        }

    }
}
