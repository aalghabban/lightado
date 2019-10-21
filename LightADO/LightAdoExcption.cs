namespace LightADO
{
    using System;

    public class LightAdoExcption : Exception
    {
        public new string Message { get; set; }

        public LightAdoExcption(string message)
        {
            this.Message = message;
        }
    }
}