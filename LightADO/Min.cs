namespace LightADO
{
    [System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class Min : AutoValidation
    {
        public Min(double minimum, string onNotValidMessage = "value not valid")
        {
            this.minimum = minimum;
            this.onNotValidMessage = onNotValidMessage;
        }

        public string onNotValidMessage;

        public double minimum;

        public void Validate(double value)
        {
            if (value < this.minimum)
            {
                throw new LightAdoExcption(new ValidationException(this.onNotValidMessage));
            }
        }
    }
}