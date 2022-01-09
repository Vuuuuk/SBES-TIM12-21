using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace Common
{
    public class DigitalSignatureHelperFunctions
    {
        public static byte[] GenerateDigitalSignature(byte[] data)
        {
            string signCertificate = DigitalSignatureFormatter.ParseName(WindowsIdentity.GetCurrent().Name) + "_sign";
            X509Certificate2 certificateSign = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertificate);

            return DigitalSignatureManager.Create(data, certificateSign);   
        }
        public static bool VerifyDigitalSignature(byte[] data, byte[] signature)
        {
            string clienName = DigitalSignatureFormatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);// Using when we use certificates
            string clientNameSign = clienName + "_sign";
            X509Certificate2 certificate = CertificateManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clientNameSign);

            if (DigitalSignatureManager.Verify(data, signature, certificate))
            {
                Console.WriteLine("Data signature is valid.\n");
                return true;
            }
            else
            {
                Console.WriteLine("Data signature is invalid.\n");
                return false;
            }
        }
    }
}
