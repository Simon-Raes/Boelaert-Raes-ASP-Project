using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text;
using System.Security.Cryptography;

namespace LayeredBusinessModel.BLL
{
    public class PasswordCrypto
    {
        const string passphrase = "Password@123";  //todo: betere passphrase

        public static string encryptPassword(string message)
        {
            byte[] results;
            UTF8Encoding utf8 = new UTF8Encoding();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] encryptionKey = md5.ComputeHash(utf8.GetBytes(passphrase));

            TripleDESCryptoServiceProvider encryptionProvider = new TripleDESCryptoServiceProvider();
            encryptionProvider.Key = encryptionKey;
            encryptionProvider.Mode = CipherMode.ECB;
            encryptionProvider.Padding = PaddingMode.PKCS7;

            byte[] encrypt_data = utf8.GetBytes(message);

            try
            {
                ICryptoTransform encryptor = encryptionProvider.CreateEncryptor();
                results = encryptor.TransformFinalBlock(encrypt_data, 0, encrypt_data.Length);
            }
            finally
            {
                encryptionProvider.Clear();
                md5.Clear();
            }

            //convert array back to a string
            return Convert.ToBase64String(results);
        }


        public static string decryptPassword(string message)
        {
            byte[] results;
            UTF8Encoding utf8 = new UTF8Encoding();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] encryptionKey = md5.ComputeHash(utf8.GetBytes(passphrase));

            TripleDESCryptoServiceProvider encryptionProvider = new TripleDESCryptoServiceProvider();
            encryptionProvider.Key = encryptionKey;
            encryptionProvider.Mode = CipherMode.ECB;
            encryptionProvider.Padding = PaddingMode.PKCS7;

            byte[] decrypt_data = Convert.FromBase64String(message);

            try
            {
                ICryptoTransform decryptor = encryptionProvider.CreateDecryptor();
                results = decryptor.TransformFinalBlock(decrypt_data, 0, decrypt_data.Length);
            }
            finally
            {
                encryptionProvider.Clear();
                md5.Clear();
            }

            return utf8.GetString(results);
        }
    }
}
