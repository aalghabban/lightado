/*
 * Copyright (C) 2019 ALGHABBAn
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace LightADO
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Provide an option to set a default value for the property before it 
    /// get mapped from or to the database.
    /// </summary>
    public sealed class DefaultValue : Attribute
    {
        /// <summary>
        /// The direction of setting 
        /// up the default value.
        /// </summary>
        public enum Directions
        {
            /// <summary>
            /// Only with Query.
            /// </summary>
            WithQuery,

            /// <summary>
            /// Only with non Query.
            /// </summary>
            WithNonQuery,

            /// <summary>
            /// With both of them.
            /// </summary>
            WithBoth
        }

        /// <summary>
        /// The type of the passed value.
        /// </summary>
        public enum ValueTypes
        {
            /// <summary>
            /// Get the value from the Object Property.
            /// </summary>
            Properties,

            /// <summary>
            /// Get The type from the object methods.
            /// </summary>
            Methods,

            /// <summary>
            /// straightforward value
            /// </summary>
            Value
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValue"/> class.
        /// </summary>
        public DefaultValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValue"/> class.
        /// </summary>
        /// <param name="value">the default value to set.</param>
        /// <param name="valueType">where to look for the default value</param>
        /// <param name="direction">the direction of the setting default value</param>
        /// <param name="parameters">any parameters to sent to a method</param>
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
