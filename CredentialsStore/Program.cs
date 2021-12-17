using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace CredentialsStore
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string addressClient = "net.tcp://localhost:5000/CredentialsStore";
            string addressAuthentificationService = "net.tcp://localhost:6000/CredentialsStore";
            ServiceHost host = new ServiceHost(typeof(CredentialsStore));

            host.AddServiceEndpoint(typeof(IAccountManagement), binding, addressClient);
            host.AddServiceEndpoint(typeof(IAccountManagement), binding, addressAuthentificationService);
            host.Open();

            Console.WriteLine("Credentials store servis successfully started.");
            Console.ReadLine();
        }
    }
}
