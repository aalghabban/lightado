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

namespace LightADO {
    using System.Reflection;
    using System;

    [AttributeUsage (AttributeTargets.Property)]
    /// <summary>
    /// Set Property to be auto validate before  
    /// sending the object data to the database.
    /// </summary>
    public class AutoValidation : Attribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoValidation"/> class , 
        /// assuming validation method name will be called Validate.
        /// </summary>
        public AutoValidation () {
            this.ValidationMethodName = "Validate";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoValidation"/> class , 
        /// and set the validation method name to be called at run time. 
        /// </summary>
        /// <param name="validationMethodName">The validation method name in your class.</param>
        public AutoValidation (string validationMethodName) {
            if (string.IsNullOrEmpty (validationMethodName)) {
                throw new LightAdoExcption ("validation method name can't be null or empty");
            }

            this.ValidationMethodName = validationMethodName;
        }

        /// <summary>
        /// Gets or sets custom validation method name 
        /// to be called at run time.
        /// </summary>
        public string ValidationMethodName { get; set; }

        /// <summary>
        /// Loop throw each property in object 
        /// and check if it has an auto validation 
        /// attribute if yes call the validation 
        /// method of the class.
        /// </summary>
        /// <typeparam name="T">the type of the object to validate</typeparam>
        /// <param name="objectToValidate">object to validate</param>
        internal static void ValidateObject<T> (T objectToValidate) {
            if (objectToValidate == null) {
                throw new LightAdoExcption ("object to validate is null.");
            }

            foreach (PropertyInfo property in objectToValidate.GetType ().GetProperties ()) {
                object[] autoValidations = property.GetCustomAttributes (typeof (AutoValidation), true);
                if (autoValidations != null && autoValidations.Length > 0) {
                    foreach (object autoValidation in autoValidations) {
                        object propertyValue = objectToValidate.GetType ().GetProperty (property.Name).GetValue (objectToValidate);
                        ValidateProperty ((AutoValidation) autoValidation, propertyValue);
                    }
                }
            }
        }

        /// <summary>
        /// Validate a property value against 
        /// Custom validation method.
        /// </summary>
        /// <param name="autoValidation">auto validation settings</param>
        /// <param name="propertyValue">property value to validate</param>
        private static void ValidateProperty (AutoValidation autoValidation, object propertyValue) {
            if (autoValidation == null) {
                throw new LightAdoExcption ("null auto validation object.");
            }

            if (autoValidation.GetType ().GetMethod (autoValidation.ValidationMethodName) == null) {
                throw new LightAdoExcption (string.Format ("no validation method with name {0} found.", autoValidation.ValidationMethodName));
            }

            autoValidation.GetType ().GetMethod (autoValidation.ValidationMethodName).Invoke (null, new object[1] { propertyValue });
        }
    }
}