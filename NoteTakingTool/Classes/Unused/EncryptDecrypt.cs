﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

/* Tutorial used: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/ */

namespace NoteTakingTool
{
    internal class EncryptDecrypt
    {
        public static string EncryptString(string key, string plainText)
        {
            // Initialisation vector - Is added to the plain text to generate more entropy when the data is encrypted
            byte[] iv = new byte[16];
            byte[] array;

            // Create the AES cryptographic object
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        
                        array = memoryStream.ToArray();
                    }
                }

            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            // Initialisation vector - Is added to the plain text to generate more entropy when the data is encrypted
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            // Create the AES cryptographic object
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static void test()
        {
            //byte[] toEncrypt = UnicodeEncoding.ASCII.GetBytes("This is some data of any length.");

            //byte[] entropy = CreateRandomEntropy();


            //int test = EncryptDataToSt



        }

    }
}
