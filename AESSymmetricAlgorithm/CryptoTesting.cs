using CryptoManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AESSymmetricAlgorithm
{
    public class CryptoTesting
    {
       public static byte[] Test_AES_Encrypt(string data, string secretKey)
        {
            byte[] outData = null;
            try
            {
                outData = AESSymmAlgorithm.EncryptData(data, secretKey);
                Console.WriteLine($"The encrypted data -> { ASCIIEncoding.ASCII.GetString(outData)}.");
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Encrypton failed. Reason: {0}", e.Message);
            }

            return outData;

        }

        public static void Test_AES_Decrypt(byte[] cryptedData, string secretKey)
        {
            string keyFile = "../../../SecretKey.txt";
            string outDec = null;
            try
            {
                outDec = AESSymmAlgorithm.DecryptData(cryptedData, SecretKey.LoadKey(keyFile));

                Console.WriteLine($"The dencrypted data -> {outDec}.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Encrypton failed. Reason: {0}", e.Message);
            }
        }
    }
}
