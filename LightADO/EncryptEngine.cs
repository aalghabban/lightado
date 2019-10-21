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
    /// Providers a options to call an Encryption/decryption method at run time.
    /// </summary>
    public abstract class EncryptEngine : Attribute
    {
        /// <summary>
        /// Encrypt as string.
        /// </summary>
        /// <param name="valueToEncrypt">value to Encrypt</param>
        /// <returns>the value after it get Encrypted</returns>
        public abstract string Encrypt(string valueToEncrypt);

        /// <summary>
        /// Decrypt as string.
        /// </summary>
        /// <param name="valueToDecrypt">value to Decrypt</param>
        /// <returns>the value after it get Decrypted</returns>
        public abstract string Decrypt(string valueToDecrypt);

        /// <summary>
        /// Encrypt or Decrypt Object
        /// </summary>
        /// <typeparam name="T">the T type of the object</typeparam>
        /// <param name="objectToEncrypt">object To Encrypt</param>
        /// <param name="encryptOrDecrypt">encrypt Or Decrypt</param>
        /// <returns>the object after setting encryptions</returns>
        internal static T EncryptOrDecryptObject<T>(T objectToEncrypt, bool encryptOrDecrypt)
        {
            if (Attribute.IsDefined(typeof(T), typeof(EncryptEngine), true) == true)
            {
                EncryptOrDecryptAllProperties(objectToEncrypt, encryptOrDecrypt, true);
            }
            else
            {
                EncryptOrDecryptAllProperties(objectToEncrypt, encryptOrDecrypt, false);
            }
                
            return objectToEncrypt;
        }

        /// <summary>
        /// Encrypt Or Decrypt All Properties
        /// </summary>
        /// <typeparam name="T">the T type of object</typeparam>
        /// <param name="objectToEncrypt">object To Encrypt</param>
        /// <param name="callEncryptMethod">call Encrypt Method</param>
        /// <param name="callForProperty">call For Property</param>
        private static void EncryptOrDecryptAllProperties<T>(T objectToEncrypt, bool callEncryptMethod = true, bool callForProperty = false)
        {
            foreach (PropertyInfo property in objectToEncrypt.GetType().GetProperties())
            {
                if (property.GetValue(objectToEncrypt) is string)
                {
                    object propertyValue = objectToEncrypt.GetType().GetProperty(property.Name).GetValue(objectToEncrypt);
                    if (propertyValue is string)
                    {
                        if (callForProperty)
                        {
                            if (Attribute.IsDefined((MemberInfo)property, typeof(EncryptEngine), true))
                            {
                                EncrypOrDecrypProperty(objectToEncrypt, callEncryptMethod, callForProperty, property, propertyValue);
                            }
                        }
                        else
                        {
                            EncrypOrDecrypProperty(objectToEncrypt, callEncryptMethod, callForProperty, property, propertyValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call an encryption method.
        /// </summary>
        /// <param name="customEncryptObject">custom Encrypt Object</param>
        /// <param name="value">value to to send to the function.</param>
        /// <returns>a value after it get encrypted</returns>
        private static string CallEncryptMethod(object customEncryptObject, string value)
        {
            return customEncryptObject.GetType().GetMethod("Encrypt").Invoke(customEncryptObject, new object[1] { (object)value }).ToString();
        }

        /// <summary>
        /// Call an decrypt method.
        /// </summary>
        /// <param name="customEncryptObject">custom decrypt Object</param>
        /// <param name="value">value to to send to the function.</param>
        /// <returns>a value after it get decrypted</returns>
        private static string CallDecryptMethod(object customEncryptObject, string value)
        {
            return customEncryptObject.GetType().GetMethod("Decrypt").Invoke(customEncryptObject, new object[1] { (object)value }).ToString();
        }

        /// <summary>
        /// Encrypt or Decrypt property.
        /// </summary>
        /// <typeparam name="T">the T type of object.</typeparam>
        /// <param name="objectToEncrypt">object To Encrypt</param>
        /// <param name="callEncryptMethod">call Encrypt Method</param>
        /// <param name="callForProperty">call For Property</param>
        /// <param name="propertyInfo">property Info</param>
        /// <param name="propertyValue">property Value</param>
        private static void EncrypOrDecrypProperty<T>(T objectToEncrypt, bool callEncryptMethod, bool callForProperty, PropertyInfo propertyInfo, object propertyValue)
        {
            if (propertyValue == null)
            {
                return;
            }

            if (callEncryptMethod)
            {
                if (callForProperty)
                {
                    propertyInfo.SetValue(objectToEncrypt, CallEncryptMethod(((MemberInfo)propertyInfo).GetCustomAttribute(typeof(EncryptEngine), true), propertyValue.ToString()));
                }
                else
                {
                    propertyInfo.SetValue(objectToEncrypt, CallEncryptMethod(((MemberInfo)objectToEncrypt.GetType()).GetCustomAttribute(typeof(EncryptEngine), true), propertyValue.ToString()));
                }
            }
            else if (callForProperty)
            {
                propertyInfo.SetValue(objectToEncrypt, EncryptEngine.CallDecryptMethod(((MemberInfo)propertyInfo).GetCustomAttribute(typeof(EncryptEngine), true), propertyValue.ToString()));
            }
            else
            {
                propertyInfo.SetValue(objectToEncrypt, EncryptEngine.CallDecryptMethod(((MemberInfo)objectToEncrypt.GetType()).GetCustomAttribute(typeof(EncryptEngine), true), propertyValue.ToString()));
            }  
        }
    }
}
