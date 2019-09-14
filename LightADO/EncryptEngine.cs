namespace LightADO
{
    using System;
    using System.Reflection;


    public abstract class EncryptEngine : Attribute
    {
        internal static T EncryptOrDecryptObject<T>(T objectToEncrypt, bool encryptOrDecrypt)
        {
            if (!Attribute.IsDefined((MemberInfo)typeof(T), typeof(EncryptEngine), true))
                EncryptEngine.EncryptOrDecryptAllProperties<T>(objectToEncrypt, encryptOrDecrypt, true);
            else
                EncryptEngine.EncryptOrDecryptAllProperties<T>(objectToEncrypt, encryptOrDecrypt, false);
            return objectToEncrypt;
        }

        private static string CallEncryptMethod(object customEncryptObject, string value)
        {
            return customEncryptObject.GetType().GetMethod("Encrypt").Invoke(customEncryptObject, new object[1]
            {
        (object) value
            }).ToString();
        }

        private static string CallDecryptMethod(object customEncryptObject, string value)
        {
            return customEncryptObject.GetType().GetMethod("Decrypt").Invoke(customEncryptObject, new object[1]
            {
        (object) value
            }).ToString();
        }

        private static void EncryptOrDecryptAllProperties<T>(
          T objectToEncrypt,
          bool callEncryptMethod = true,
          bool callForProperty = false)
        {
            foreach (PropertyInfo property in objectToEncrypt.GetType().GetProperties())
            {
                if (property.GetValue((object)objectToEncrypt) is string)
                {
                    object propertyValue = objectToEncrypt.GetType().GetProperty(property.Name).GetValue((object)objectToEncrypt);
                    if (propertyValue is string)
                    {
                        if (callForProperty)
                        {
                            if (Attribute.IsDefined((MemberInfo)property, typeof(EncryptEngine), true))
                                EncryptEngine.EncrypOrDecrypProperty<T>(objectToEncrypt, callEncryptMethod, callForProperty, property, propertyValue);
                        }
                        else
                            EncryptEngine.EncrypOrDecrypProperty<T>(objectToEncrypt, callEncryptMethod, callForProperty, property, propertyValue);
                    }
                }
            }
        }

        private static void EncrypOrDecrypProperty<T>(
          T objectToEncrypt,
          bool callEncryptMethod,
          bool callForProperty,
          PropertyInfo propertyInfo,
          object propertyValue)
        {
            if (propertyValue == null)
                return;
            if (callEncryptMethod)
            {
                if (callForProperty)
                    propertyInfo.SetValue((object)objectToEncrypt, (object)EncryptEngine.CallEncryptMethod((object)CustomAttributeExtensions.GetCustomAttribute((MemberInfo)propertyInfo, typeof(EncryptEngine), true), propertyValue.ToString()));
                else
                    propertyInfo.SetValue((object)objectToEncrypt, (object)EncryptEngine.CallEncryptMethod((object)CustomAttributeExtensions.GetCustomAttribute((MemberInfo)objectToEncrypt.GetType(), typeof(EncryptEngine), true), propertyValue.ToString()));
            }
            else if (callForProperty)
                propertyInfo.SetValue((object)objectToEncrypt, (object)EncryptEngine.CallDecryptMethod((object)CustomAttributeExtensions.GetCustomAttribute((MemberInfo)propertyInfo, typeof(EncryptEngine), true), propertyValue.ToString()));
            else
                propertyInfo.SetValue((object)objectToEncrypt, (object)EncryptEngine.CallDecryptMethod((object)CustomAttributeExtensions.GetCustomAttribute((MemberInfo)objectToEncrypt.GetType(), typeof(EncryptEngine), true), propertyValue.ToString()));
        }

        public abstract string Encrypt(string valueToEncrypt);

        public abstract string Decrypt(string valueToDecrypt);
    }
}
