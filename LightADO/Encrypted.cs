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
