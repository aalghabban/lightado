namespace LightADO
{
    using Microsoft.Extensions.Configuration;
    using System.Configuration;
    using System.IO;

    internal static class ConfigurationLoader
    {
        internal enum ConfigurationSections
        {
            ConnectionString, 
            AppSettings
        }

        internal static string GetValueOfKey(string keyName, ConfigurationSections section = ConfigurationSections.ConnectionString)
        {
            string value = null;
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationRoot configRoot = builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")).Build();

            if (section == ConfigurationSections.ConnectionString)
            {
                if (ConfigurationManager.ConnectionStrings[keyName] != null)
                {
                    value = ConfigurationManager.ConnectionStrings[keyName].ConnectionString;
                }
                else
                {

                    value = configRoot.GetConnectionString(keyName);
                }
            }
            else
            {
                if (ConfigurationManager.AppSettings[keyName] != null)
                {
                    return ConfigurationManager.AppSettings[keyName];
                }
                else
                {
                    value = configRoot.GetSection("AppSettings" + ":" + keyName).Value;
                }
            }

            return value;
        }
    }
}
