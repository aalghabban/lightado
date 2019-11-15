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
    using System.Linq;
    using System.Reflection;
    using System;
    using static LightADO.Types;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
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
        /// Encrypt Or Decrypt object
        /// </summary>
        /// <typeparam name="T">type T of the object</typeparam>
        /// <param name="objectToEncrypt">object To Encrypt</param>
        /// <param name="oprationType">encrypt or decrypt</param>
        /// <returns>the object after decrypt or encrypt</returns>
        internal static T EncryptOrDecryptObject<T>(T objectToEncrypt, OprationType oprationType)
        {
            if (IsDefined(typeof(T), typeof(EncryptEngine), true) == true)
            {
                EncryptOrDecryptProperties(objectToEncrypt, oprationType);
            }
            else
            {
                EncrypOrDecrypProperty(objectToEncrypt, oprationType);
            }

            return objectToEncrypt;
        }

        /// <summary>
        /// Encrypt Or Decrypt All Properties for object.
        /// </summary>
        /// <typeparam name="T">the type T of object</typeparam>
        /// <param name="objectToEncrypt">object To Encrypt</param>
        /// <param name="oprationType">oprationType wither to encrypt or decrypt</param>
        private static void EncryptOrDecryptProperties<T>(T objectToEncrypt, OprationType oprationType)
        {
            object encryptAttribute = objectToEncrypt.GetType().GetCustomAttribute(typeof(EncryptEngine), true);
            foreach (PropertyInfo property in objectToEncrypt.GetType().GetProperties())
            {
                if (property.GetValue(objectToEncrypt) != null && property.GetValue(objectToEncrypt) is string)
                {
                    if (oprationType == OprationType.Encrypt)
                    {
                        property.SetValue(objectToEncrypt, CallEncryptMethod(encryptAttribute, property.GetValue(objectToEncrypt).ToString()));
                    }
                    else
                    {
                        property.SetValue(objectToEncrypt, CallDecryptMethod(encryptAttribute, property.GetValue(objectToEncrypt).ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// Encrypt Or Decrypt All Properties for object.
        /// </summary>
        /// <typeparam name="T">the type T of obejct</typeparam>
        /// <param name="objectToEncrypt">object To Encrypt</param>
        /// <param name="oprationType">oprationType wither to encrypt or decrypt</param>
        private static void EncrypOrDecrypProperty<T>(T objectToEncrypt, OprationType oprationType)
        {
            var properties = objectToEncrypt.GetType().GetProperties()
                .Where(prop => prop.IsDefined(typeof(EncryptEngine), false));

            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(objectToEncrypt) != null && property.GetValue(objectToEncrypt) is string)
                {
                    if (oprationType == OprationType.Encrypt)
                    {
                        property.SetValue(objectToEncrypt, CallEncryptMethod(((MemberInfo)property).GetCustomAttribute(typeof(EncryptEngine), true), property.GetValue(objectToEncrypt).ToString()));
                    }
                    else
                    {
                        property.SetValue(objectToEncrypt, CallDecryptMethod(((MemberInfo)property).GetCustomAttribute(typeof(EncryptEngine), true), property.GetValue(objectToEncrypt).ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// Call an encryption method.
        /// </summary>
        /// <param name="value">value to to send to the function.</param>
        /// <returns>a value after it get encrypted</returns>
        private static string CallEncryptMethod(object customEncryptObject, string value)
        {
            return customEncryptObject.GetType().GetMethod("Encrypt").Invoke(customEncryptObject, new object[1] {
                (object) value }).ToString();
        }

        /// <summary>
        /// Call an decrypt method.
        /// </summary>
        /// <param name="customEncryptObject">custom decrypt Object</param>
        /// <param name="value">value to to send to the function.</param>
        /// <returns>a value after it get decrypted</returns>
        private static string CallDecryptMethod(object customEncryptObject, string value)
        {
            return customEncryptObject.GetType().GetMethod("Decrypt").Invoke(customEncryptObject, new object[1] {
                (object) value }).ToString();
        }
    }
}