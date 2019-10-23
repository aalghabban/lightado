namespace LightADO.Tests
{
    public class Encrypto : EncryptEngine
    {
        public override string Decrypt(string valueToDecrypt)
        {
            return "Decrypted";
        }

        public override string Encrypt(string valueToEncrypt)
        {
            return "Encrypted";
        }
    }
}
