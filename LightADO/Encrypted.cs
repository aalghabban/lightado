namespace LightADO
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public sealed class Encrypted : EncryptEngine
    {
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("HR$2pIjHR$2pIj12");
        private const int keysize = 256;

        public override string Encrypt(string textToEncrypt)
        {
            byte[] bytes1 = Encoding.UTF8.GetBytes(textToEncrypt);
            string base64String;
            using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(new LightADOSetting().EncryptKey, (byte[])null))
            {
                byte[] bytes2 = passwordDeriveBytes.GetBytes(16);
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes2, Encrypted.initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(bytes1, 0, bytes1.Length);
                                cryptoStream.FlushFinalBlock();
                                base64String = Convert.ToBase64String(memoryStream.ToArray());
                            }
                        }
                    }
                }
            }
            return base64String;
        }

        public override string Decrypt(string textToDecrypt)
        {
            byte[] buffer = Convert.FromBase64String(textToDecrypt);
            string str;
            using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(new LightADOSetting().EncryptKey, (byte[])null))
            {
                byte[] bytes = passwordDeriveBytes.GetBytes(32);
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.Mode = CipherMode.CBC;
                    using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes, Encrypted.initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] numArray = new byte[buffer.Length];
                                int count = cryptoStream.Read(numArray, 0, numArray.Length);
                                str = Encoding.UTF8.GetString(numArray, 0, count);
                            }
                        }
                    }
                }
            }
            return str;
        }
    }
}
