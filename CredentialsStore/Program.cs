using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;

namespace CredentialsStore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.ReadKey();
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string addressClient = "net.tcp://localhost:5000/CredentialsStore";
            string addressAuthentificationService = "net.tcp://localhost:6000/CredentialsStore";
            ServiceHost host = new ServiceHost(typeof(CredentialsStore));

            host.AddServiceEndpoint(typeof(IAccountManagement), binding, addressClient);
            host.AddServiceEndpoint(typeof(IAccountManagement), binding, addressAuthentificationService);

            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            
            host.Open();

            Console.WriteLine("Credentials store servis successfully started.");
            Console.ReadLine();
        }
    }
}
