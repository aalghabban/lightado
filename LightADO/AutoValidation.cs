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
    /// Set Property to be auto validate before  
    /// sending the object data to the database.
    /// </summary>
    public class AutoValidation : Attribute
    {
        /// <summary>
        /// create a new instance of Auto Validation, you will need to set 
        /// the validation method name otherwise auto validation will call 
        /// Validate
        /// </summary>
        AutoValidation()
        {
        }

        /// <summary>
        /// create a new instance of Auto Validation, with method name to be called 
        /// at runtime as validation method.
        /// </summary>
        /// <param name="methodName">the validation method name in your class.</param>
        AutoValidation(string methodName)
        {
            this.MethodName = methodName;
        }

        /// <summary>
        /// Get or set custom validation method name 
        /// to be called at run time.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Loop throw each property in object 
        /// and check if it has an auto validation 
        /// attribute if yes call the validation 
        /// method of the class.
        /// </summary>
        /// <typeparam name="T">the type of the object to validate</typeparam>
        /// <param name="objectToValidate">object to validate</param>
        internal static void ValidateObject<T>(T objectToValidate)
        {
            foreach (PropertyInfo property in objectToValidate.GetType().GetProperties())
            {
                AutoValidation autoValidation = property.GetCustomAttribute<AutoValidation>(true);
                if (autoValidation != null)
                {
                    object propertyValue = objectToValidate.GetType().GetProperty(property.Name).GetValue((object)objectToValidate);
                    ValidateProperty(autoValidation, propertyValue);
                }
            }
        }

        /// <summary>
        /// Validate a property value against 
        /// Custom validation method.
        /// </summary>
        /// <param name="autoValidation">auto validation settings</param>
        /// <param name="propertyValue">property value to validate</param>
        private static void ValidateProperty(AutoValidation autoValidation, object propertyValue)
        {
            string methodName = string.IsNullOrEmpty(autoValidation.MethodName) == true ? "Validate" : autoValidation.MethodName;
            autoValidation.GetType().GetMethod(methodName).Invoke(autoValidation, new object[1] { propertyValue });
        }
    }
}