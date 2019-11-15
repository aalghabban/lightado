namespace LightADO
{
    public class ValidationException : System.Exception
    {
        public ValidationException(string message)
        {
            this.Message = message;
        }

        public new string Message { get; private set; }
    }
}