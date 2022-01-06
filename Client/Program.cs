using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string authenticationServiceAddress = "net.tcp://localhost:4000/AuthenticationService";
            string credentialsStoreAddress = "net.tcp://localhost:5000/CredentialsStore";

            //WINDOWS AUTHENTICATION PROTOCOL INIT

            //Used for Anti-Phishing protection
            EndpointAddress authenticationServiceEndpointAddress = new EndpointAddress(new Uri(authenticationServiceAddress), EndpointIdentity.CreateUpnIdentity("authservice")); 
            EndpointAddress credentialsStoreEndpointAddress = new EndpointAddress(new Uri(credentialsStoreAddress), EndpointIdentity.CreateUpnIdentity("credentialsstore"));

            binding.Security.Mode = SecurityMode.Message; //Safer but slower then SecurityMode.Transport as it encrypts each message separately
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows; //Based on windows user accounts
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign; //Anti-Tampering signature (per message) protection

            Console.WriteLine($"Currently used by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + "\n");

            using (AuthenticationProxy authenticationProxy = new AuthenticationProxy(binding, authenticationServiceEndpointAddress)) 
            {
                try
                {
                    //CALL AS METHODS HERE
                }
                catch (FaultException<InvalidGroupException> ex)
                {
                    Console.WriteLine(ex.Detail.exceptionMessage);
                    authenticationProxy.Abort(); //To avoid AS server faulted state
                }
                catch (MessageSecurityException)
                {
                    Console.WriteLine($"Server identity check failed, expected -> [authservice]. Please contact your system administrator.\n");
                    authenticationProxy.Abort(); //To avoid AS server faulted state
                }
            }

            using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreEndpointAddress)) 
            {
                try
                {
                    //CALL CS METHODS HERE
                }
                catch (FaultException<InvalidGroupException> ex)
                {
                    Console.WriteLine(ex.Detail.exceptionMessage);
                    credentialsStoreProxy.Abort(); //To avoid CS server faulted state
                }
                catch (MessageSecurityException)
                {
                    Console.WriteLine($"Server identity check failed, expected -> [credentialsstore]. Please contact your system administrator.\n");
                    credentialsStoreProxy.Abort(); //To avoid CS server faulted state
                }
            }

            Console.ReadLine();
        }
    }
}
