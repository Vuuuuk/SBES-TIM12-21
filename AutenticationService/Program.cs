using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;

namespace AuthenticationService
{
    class Program
    {
        static void Main(string[] args)
        {

            //AES SECRET KEY GENERATION

            string secretKey = SecretKey.GenerateKey();
            SecretKey.StoreKey(secretKey, AES.KeyLocation);

            //AuthenticationService SERVER INIT

            NetTcpBinding bindingClient = new NetTcpBinding();
            string address = "net.tcp://localhost:4000/AuthenticationService";

            //WINDOWS AUTHENTICATION PROTOCOL INIT FOR CLIENT

            bindingClient.Security.Mode = SecurityMode.Message; //Safer but slower then SecurityMode.Transport as it encrypts each message separately
            bindingClient.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows; //Based on windows user accounts
            bindingClient.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign; //Anti-Tampering signature (per message) protection

            ServiceHost host = new ServiceHost(typeof(AuthenticationService));

            host.AddServiceEndpoint(typeof(IAuthenticationService), bindingClient, address);

            host.Open();

            Console.WriteLine($"Authentication servis successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");

            using (CredentialsStoreProxy credentialsStoreProxy = CredentialsStoreProxy.SingletonInstance())
            {
                try
                {
                    //CALL CS METHODS FOR AS HERE
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Client certificate check failed. Please contact your system administrator.\n");
                    credentialsStoreProxy.Abort(); //To avoid CS server faulted state
                }
            }
            
            Console.ReadLine();

            //RESET CURRENTLY LOGGED IN USERS ON AS CLOSE

            CurrentUsers users = new CurrentUsers();
            users.resetCurrentUsers();

            host.Close();
        }
    }
}
