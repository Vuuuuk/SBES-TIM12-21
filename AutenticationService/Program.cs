using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;

namespace AuthenticationService
{
    class Program
    {
        static void Main(string[] args)
        {
           // Console.ReadKey();
            //SERVER INIT
            string srvCertCN = "credentialsstore";
            NetTcpBinding binding = new NetTcpBinding();
            NetTcpBinding binding1 = new NetTcpBinding();

            binding1.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:4000/AuthenticationService";
            ServiceHost host = new ServiceHost(typeof(AuthenticationService));

            host.AddServiceEndpoint(typeof(IAuthenticationService), binding, address);
            host.Open();

            Console.WriteLine("Authentication servis successfully started.\n");

            //CLIENT INIT

            //string credentialsStoreAddress = "net.tcp://localhost:6000/CredentialsStore";
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress credentialsStoreAddress = new EndpointAddress(new Uri("net.tcp://localhost:6000/CredentialsStore"),
                                      new X509CertificateEndpointIdentity(srvCert));
            using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding1, credentialsStoreAddress)) { }

            Console.WriteLine("Authentication servis [as a client] successfully started.\n");


            Console.ReadLine();
        }
    }
}
