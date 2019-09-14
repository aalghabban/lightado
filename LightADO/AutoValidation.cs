namespace LightADO
{
    using System;
    using System.Reflection;

    public abstract class AutoValidation : Attribute
    {
        public string ErrorMessage { get; set; }

        public string CallThisMethod { get; set; }

        internal static bool ValidateObject<T>(T objectToValidate)
        {
            foreach (PropertyInfo property in objectToValidate.GetType().GetProperties())
            {
                object[] customAttributes = property.GetCustomAttributes(true);
                if (customAttributes != null && (uint)customAttributes.Length > 0U)
                {
                    foreach (object customAttribute in customAttributes)
                    {
                        if (customAttribute.GetType().BaseType.Name == nameof(AutoValidation))
                        {
                            object propertyValue = objectToValidate.GetType().GetProperty(property.Name).GetValue((object)objectToValidate);
                            AutoValidation.ValidateProperty(customAttribute, propertyValue);
                        }
                    }
                }
            }
            return true;
        }

        private static void ValidateProperty(object customAttribute, object propertyValue)
        {
            string name = customAttribute.GetType().GetProperty("CallThisMethod").GetValue(customAttribute) != null ? customAttribute.GetType().GetProperty("CallThisMethod").GetValue(customAttribute).ToString() : "Validate";
            if (bool.Parse(customAttribute.GetType().GetMethod(name).Invoke(customAttribute, new object[1]
            {
        propertyValue
            }).ToString()))
                return;
            if (customAttribute.GetType().GetProperty("ErrorMessage").GetValue(customAttribute) != null)
                throw new LightADOValidationException(customAttribute.GetType().GetProperty("ErrorMessage").GetValue(customAttribute).ToString());
            throw new LightADOValidationException(string.Format("Violation of {0}, set the error message as [{1}( ErrorMessage=\"This A demo \")]; to display it", (object)customAttribute.GetType().Name, (object)customAttribute.GetType().Name));
        }

        public abstract bool Validate(object propertyValue);
    }
}
