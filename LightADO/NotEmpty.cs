namespace LightADO
{
    [System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NotNullOrEmpty : AutoValidation
    {
        public NotNullOrEmpty(bool allowWhiteSpace = false, string onNotValidMessage = "value must not be null or empty")
        {
            this.onNotValidMessage = onNotValidMessage;
        }

        private string onNotValidMessage;

        public void Validate(string value)
        {
            if (string.IsNullOrEmpty(value) == true)
            {
                throw new LightAdoExcption(new ValidationException(this.onNotValidMessage));
            }

            if (string.IsNullOrWhiteSpace(value) == true)
            {
                throw new LightAdoExcption(new ValidationException(this.onNotValidMessage));
            }
        }
    }
}