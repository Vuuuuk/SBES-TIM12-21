using Common;
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
            NetTcpBinding bindingClient = new NetTcpBinding();
            NetTcpBinding bindingAuthentificationService = new NetTcpBinding();
            string addressClient = "net.tcp://localhost:5000/CredentialsStore";
            string addressAuthentificationService = "net.tcp://localhost:6000/CredentialsStore";

            //WINDOWS AUTHENTICATION PROTOCOL INIT FOR CLIENT

            bindingClient.Security.Mode = SecurityMode.Message; //Safer but slower then SecurityMode.Transport as it encrypts each message separately
            bindingClient.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows; //Based on windows user accounts
            bindingClient.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign; //Anti-Tampering signature (per message) protection

            ServiceHost hostCredentialsStore = new ServiceHost(typeof(CredentialsStore));
            ServiceHost hostAuthenticationServiceManagement = new ServiceHost(typeof(AuthenticationServiceManagement));

            //CERTIFICATE SERVER CONFIGURATION INIT

            string serverName = CertificateFormatter.ParseName(WindowsIdentity.GetCurrent().Name); //Parsed WindowsIdentity.Name
            bindingAuthentificationService.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate; //Certificate-based authentication
            hostAuthenticationServiceManagement.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust; //Authority validation mode
            hostAuthenticationServiceManagement.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck; //Do not check if Authority marked the certificate as unusable
            hostAuthenticationServiceManagement.Credentials.ServiceCertificate.Certificate = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, serverName); //Server public/private-key.PFX

            hostCredentialsStore.AddServiceEndpoint(typeof(IAccountManagement), bindingClient, addressClient);
            hostAuthenticationServiceManagement.AddServiceEndpoint(typeof(IAuthenticationServiceManagement), bindingAuthentificationService, addressAuthentificationService);

            hostCredentialsStore.Open();

            try
            {
                hostAuthenticationServiceManagement.Open();
            }
            catch(InvalidOperationException)
            {
                Console.WriteLine("Server certificate check failed. Please contact your system administrator.\n");
                hostCredentialsStore.Abort(); //To avoid CS server faulted state
            }

            Console.WriteLine($"Credentials store servis successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");

            Console.ReadLine();
        }
    }
}
