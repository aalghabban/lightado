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
    public abstract class AutoValidation : Attribute
    {
        AutoValidation()
        {
        }

        AutoValidation(string methodName, string errorMessage = null)
        {
            this.MethodName = methodName;
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; set; }

        public string MethodName { get; set; }

        public abstract bool Validate(object propertyValue);

        internal static bool ValidateObject<T>(T objectToValidate)
        {
            foreach (PropertyInfo property in objectToValidate.GetType().GetProperties())
            {
                AutoValidation validation = property.GetCustomAttribute<AutoValidation>(true);
                if (validation != null)
                {
                    object propertyValue = objectToValidate.GetType().GetProperty(property.Name).GetValue((object)objectToValidate);
                    AutoValidation.ValidateProperty(validation, propertyValue);
                }
            }
            return true;
        }

        private static void ValidateProperty(AutoValidation validationAttribute, object propertyValue)
        {
            string methodName = string.IsNullOrEmpty(validationAttribute.MethodName) == true ? "Validate" : validationAttribute.MethodName;
            if (bool.Parse(validationAttribute.GetType().GetMethod(methodName).Invoke(validationAttribute, new object[1] { propertyValue }).ToString()))
            {
                return;
            }

            if (string.IsNullOrEmpty(validationAttribute.ErrorMessage) == false)
            {
                throw new LightADOValidationException(validationAttribute.ErrorMessage);
            }

            throw new LightADOValidationException(string.Format("Violation of {0}, set the error message as [{1}( ErrorMessage=\"This A demo \")]; to display it", validationAttribute.GetType().Name, validationAttribute.GetType().Name));
        }
    }
}