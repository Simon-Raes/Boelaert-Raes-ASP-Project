using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.Configuration;

namespace LayeredBusinessModel.BLL
{
    public class CryptographyModel
    {
        //get the passphrase from web.config
        private static String passphrase = ConfigurationManager.AppSettings["PassPhrase"];

        public static string encryptPassword(string message)
        {
            byte[] results = null;
            UTF8Encoding utf8 = new UTF8Encoding();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            TripleDESCryptoServiceProvider encryptionProvider = new TripleDESCryptoServiceProvider();
            encryptionProvider.Key = md5.ComputeHash(utf8.GetBytes(passphrase));
            encryptionProvider.Mode = CipherMode.ECB;
            encryptionProvider.Padding = PaddingMode.PKCS7;

            byte[] encrypt_data = utf8.GetBytes(message);

            try
            {
                ICryptoTransform encryptor = encryptionProvider.CreateEncryptor();
                results = encryptor.TransformFinalBlock(encrypt_data, 0, encrypt_data.Length);
            }
            catch (CryptographicException)
            {

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
            byte[] results = null;
            UTF8Encoding utf8 = new UTF8Encoding();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            TripleDESCryptoServiceProvider encryptionProvider = new TripleDESCryptoServiceProvider();
            encryptionProvider.Key = md5.ComputeHash(utf8.GetBytes(passphrase));
            encryptionProvider.Mode = CipherMode.ECB;
            encryptionProvider.Padding = PaddingMode.PKCS7;

            byte[] decrypt_data = Convert.FromBase64String(message);

            try
            {
                ICryptoTransform decryptor = encryptionProvider.CreateDecryptor();
                results = decryptor.TransformFinalBlock(decrypt_data, 0, decrypt_data.Length);
            }
            catch (CryptographicException)
            {

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
