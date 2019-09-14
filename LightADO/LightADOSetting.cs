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
                    this.encryptKey = System.Configuration.ConfigurationManager.AppSettings["EncryptKey"];
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
            this.ConnectionString = SqlConnectionHandler.LoadConnectionFromConfigrationFile("DefaultConnection");
        }

        public LightADOSetting(string connectionString, bool loadFromConfigrationFile)
        {
            if (loadFromConfigrationFile)
                this.ConnectionString = SqlConnectionHandler.LoadConnectionFromConfigrationFile(connectionString);
            else
                this.ConnectionString = connectionString;
        }
    }
}
