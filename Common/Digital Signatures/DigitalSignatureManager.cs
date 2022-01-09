using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Common
{
    public class DigitalSignatureManager
    {
        public static byte[] Create(byte[] data, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PrivateKey;// Looks for the certificate's private key to sign a message

            if (csp == null)
                throw new Exception("Valid certificate not found!");

            SHA1Managed sha1 = new SHA1Managed();// We cant't use SHA256 because we are on .NET 4.0 (>= .NET 4.6 supports SHA256)
            byte[] dataByteHash = sha1.ComputeHash(data);

            byte[] signature = csp.SignHash(dataByteHash, CryptoConfig.MapNameToOID("SHA1"));
            return signature;
        }

        public static bool Verify(byte[] data, byte[] signature, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PublicKey.Key;

            SHA1Managed sha1 = new SHA1Managed();
            byte [] hashdataBytesArray = sha1.ComputeHash(data);

            return csp.VerifyHash(hashdataBytesArray, CryptoConfig.MapNameToOID("SHA1"), signature);// Compares hash value from signature and newly created hash value
        }
    }
}
