namespace LightADO
{
    [System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class Max : AutoValidation
    {
        public Max(double maximum, string onNotValidMessage = "valid not valid")
        {
            this.maximum = maximum;
            this.onNotValidMessage = onNotValidMessage;
        }

        private string onNotValidMessage;

        private double maximum;

        public void Validate(double value)
        {
            if (value > this.maximum)
            {
                throw new LightAdoExcption(new ValidationException(this.onNotValidMessage));
            }
        }
    }
}