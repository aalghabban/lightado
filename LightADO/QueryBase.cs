namespace LightADO
{
    using System;

    public class QueryBase
    {
        public LightADOSetting LightAdoSetting { get; set; }

        public QueryBase()
        {
            this.LightAdoSetting = new LightADOSetting();
        }

        public QueryBase(string connectionString, bool loadFromConfigrationFile)
        {
            this.LightAdoSetting = new LightADOSetting(connectionString, loadFromConfigrationFile);
        }

        internal static void ThrowExacptionOrEvent(
          OnError onError,
          Exception exception,
          string extraInfo = "")
        {
            if (onError == null)
                throw exception;
            exception.Source = extraInfo;
            onError(exception);
        }
    }
}
