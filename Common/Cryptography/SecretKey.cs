using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class SecretKey
    {
		// Generates a random 32byte secret key
		public static string GenerateKey()
		{
			SymmetricAlgorithm symmAlgorithm = AesCryptoServiceProvider.Create();

			return ASCIIEncoding.ASCII.GetString(symmAlgorithm.Key);
		}

		// Stores the secret key in a .txt file
		public static void StoreKey(string secretKey, string outFile)
		{
			FileStream fOutput = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);
			byte[] buffer = Encoding.ASCII.GetBytes(secretKey);

			try
			{
				fOutput.Write(buffer, 0, buffer.Length);
			}
			catch (Exception e)
			{
				Console.WriteLine("Storing key failed with errror -> {0}", e.Message);
			}
			finally
			{
				fOutput.Close();
			}
		}

		// Loads the secret key from a .txt file
		public static string LoadKey(string inFile)
		{
			FileStream fInput = new FileStream(inFile, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[(int)fInput.Length];

			try
			{
				fInput.Read(buffer, 0, (int)fInput.Length);
			}
			catch (Exception e)
			{
				Console.WriteLine("Loading key failed with error -> {0}", e.Message);
			}
			finally
			{
				fInput.Close();
			}

			return ASCIIEncoding.ASCII.GetString(buffer);
		}
	}
}
