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
    using System.Reflection;
    using System;
    using static LightADO.Types;

    [AttributeUsage(AttributeTargets.Property)]
    /// <summary>
    /// Provide an option to set a default value for the property before it 
    /// get mapped from or to the database.
    /// </summary>
    public sealed class DefaultValue : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValue"/> class.
        /// </summary>
        public DefaultValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValue"/> class.
        /// </summary>
        /// <param name="value">the default value to set.</param>
        /// <param name="valueType">where to look for the default value</param>
        /// <param name="direction">the direction of the setting default value</param>
        /// <param name="parameters">any parameters to sent to a method</param>
        public DefaultValue(string value, Directions direction = Directions.WithNonQuery, params object[] parameters)
        {
            this.Value = value;
            this.Direction = direction;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets the default values.
        /// </summary>
        internal object Value { get; private set; }

        /// <summary>
        /// Gets the direction of the default values.
        /// </summary>
        internal Directions Direction { get; private set; }

        /// <summary>
        /// Gets the parameters of the default values.
        /// </summary>
        internal object[] Parameters { get; private set; }

        /// <summary>
        /// Loop throw each property in object
        /// and set the default property for it.
        /// </summary>
        /// <typeparam name="T">the T type of the object to set default values.</typeparam>
        /// <param name="objectToMapDefaultValues">object to set it's default values.</param>
        /// <param name="directions">in which direct to set the default values.</param>
        /// <returns>objectToMapDefaultValues after default values was set</returns>
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
                        SetDefaultValue(objectToMapDefaultValues, customAttribute, objectToMapDefaultValues.GetType().GetProperty(property.Name));
                    }
                }
            }

            return objectToMapDefaultValues;
        }

        /// <summary>
        /// Set Default Values for property
        /// </summary>
        /// <typeparam name="T">the T type of object.</typeparam>
        /// <param name="objectToMapDefaultValues">object To Map Default Values</param>
        /// <param name="defaultValueSettings">default Value Settings</param>
        /// <param name="property">property to set it's value</param>
        /// <returns>the property after setting the values</returns>
        private static T SetDefaultValue<T>(T objectToMapDefaultValues, DefaultValue defaultValueSettings, PropertyInfo property)
        {
            object valueAfterTypedChanged;
            if (defaultValueSettings.Value.GetType() == typeof(string))
            {
                if (defaultValueSettings.Value.ToString().Contains(".") == false)
                {
                    valueAfterTypedChanged = Convert.ChangeType(defaultValueSettings.Value, property.PropertyType);
                }
                else
                {
                    valueAfterTypedChanged = Convert.ChangeType(GetDefaultValue(defaultValueSettings.Value.ToString()), property.PropertyType);
                }
            }

            return objectToMapDefaultValues;
        }


        private static object GetDefaultValue(string defaultValue)
        {
            string[] paths = defaultValue.Split('.');
            Type type = GetDefaultValueType(paths[0]);
            object value = null;
            for (int path = 1; path < paths.Length; path++)
            {
                MethodInfo method = GetDefaultValueMethod(paths[path], type);
                if (method != null)
                {
                    value = method.Invoke(null, null);
                }
                else
                {
                    PropertyInfo propertyInfo = GetDefaultValueProperty(paths[path], type);
                    value = propertyInfo.GetValue(null);
                }
            }

            return value;
        }

        private static Type GetDefaultValueType(string typeName)
        {
            Type type = Type.GetType("System." + typeName);
            if (type == null)
            {
                throw new LightAdoExcption("Type not found");
            }

            return type;
        }

        private static MethodInfo GetDefaultValueMethod(string methodName, Type type)
        {
            return type.GetMethod(methodName);
        }

        private static PropertyInfo GetDefaultValueProperty(string propertyName, Type type)
        {
            return type.GetProperty(propertyName);
        }
    }
}