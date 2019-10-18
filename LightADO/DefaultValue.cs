namespace LightADO
{
    using System;
    using System.Reflection;

    public sealed class DefaultValue : Attribute
    {
        public enum Directions
        {
            WithQuery,

            WithNonQuery,

            WithBoth
        }

        public enum ValueTypes
        {
            Properties,

            Methods,

            Value
        }

        public DefaultValue()
        {
        }

        public DefaultValue(string value, ValueTypes valueType = ValueTypes.Value, Directions direction = Directions.WithNonQuery, params object[] parameters)
        {
            this.Value = value;
            this.Direction = direction;
            this.Parameters = parameters;
            this.ValueType = valueType;
        }

        internal static T SetDefaultValus<T>(T objectToMapDefaultValues, Directions directions = Directions.WithBoth)
        {
            foreach (PropertyInfo property in objectToMapDefaultValues.GetType().GetProperties())
            {
                DefaultValue customAttribute = property.GetCustomAttribute<DefaultValue>(true);
                if (customAttribute != null)
                {
                    if (customAttribute.Direction == Directions.WithBoth || customAttribute.Direction == directions)
                    {
                        DefaultValue defaultValue = customAttribute;
                        object value = objectToMapDefaultValues.GetType().GetProperty(property.Name).GetValue(objectToMapDefaultValues);
                        if (value == null)
                        {
                            SetDefaultValue(objectToMapDefaultValues, customAttribute, objectToMapDefaultValues.GetType().GetProperty(property.Name));
                        }
                    }
                }
            }

            return objectToMapDefaultValues;
        }

        private static T SetDefaultValue<T>(T objectToMapDefaultValues, DefaultValue defaultValueSettings, PropertyInfo property)
        {
            object valueAfterTypedChanged = null;
            if (defaultValueSettings.ValueType == ValueTypes.Value)
            {
                valueAfterTypedChanged = Convert.ChangeType(defaultValueSettings.Value, property.PropertyType);
            }
            else
            {
                if (defaultValueSettings.ValueType == ValueTypes.Properties)
                {
                    valueAfterTypedChanged = Convert.ChangeType(property.PropertyType.GetProperty(defaultValueSettings.Value.ToString()).GetValue(null), property.PropertyType);
                }
                else
                {
                    valueAfterTypedChanged = Convert.ChangeType(property.PropertyType.GetMethod(defaultValueSettings.Value.ToString()).Invoke(null, defaultValueSettings.Parameters), property.PropertyType);
                }
            }

            objectToMapDefaultValues.GetType().GetProperty(property.Name).SetValue(objectToMapDefaultValues, valueAfterTypedChanged);

            return objectToMapDefaultValues;
        }

        internal object Value { get; private set; }

        internal Directions Direction { get; private set; }

        internal object[] Parameters { get; private set; }

        internal ValueTypes ValueType { get; private set; }
    }
}
