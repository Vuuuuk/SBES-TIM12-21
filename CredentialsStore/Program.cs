using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
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

            hostCredentialsStore.AddServiceEndpoint(typeof(IAccountManagement), bindingClient, addressClient);
            hostAuthenticationServiceManagement.AddServiceEndpoint(typeof(IAuthenticationServiceManagement), bindingAuthentificationService, addressAuthentificationService);

            hostCredentialsStore.Open();
            hostAuthenticationServiceManagement.Open();

            Console.WriteLine($"Credentials store servis successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");

            Console.ReadLine();
        }
    }
}
