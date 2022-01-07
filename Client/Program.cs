using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string authenticationServiceAddress = "net.tcp://localhost:4000/AuthenticationService";
            string credentialsStoreAddress = "net.tcp://localhost:5000/CredentialsStore";

            using (AuthenticationProxy authenticationProxy = new AuthenticationProxy(binding, authenticationServiceAddress))
            {
                Task t = Task.Factory.StartNew(() => Repeat(authenticationProxy, ""));
                if(authenticationProxy.State == CommunicationState.Closed)

                using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreAddress))
                {
                    /*
                                    try
                                    {

                                        credentialsStoreProxy.CreateAccount("ADMIN", "Admin");
                                        credentialsStoreProxy.DeleteAccount("ADMIN");
                                        credentialsStoreProxy.CreateAccount("BANDER", "JHONSON");
                                        credentialsStoreProxy.DisableAccount("BANDER");
                                        credentialsStoreProxy.CreateAccount("SIMON", "VIBIN");
                                        credentialsStoreProxy.LockAccount("SIMON");
                                    }
                                    catch(Exception e)
                                    {
                                        Console.WriteLine(e);
                                    }
                                    */
                }
            }


            


            Console.ReadLine();
        }

        static void Repeat(AuthenticationProxy proxy, string user)
        {
            while (true)
            {
                if(!proxy.CheckIn(user))
                {
                    Console.WriteLine("Im sorry {0}, This account has been temporarily disabled",user);
                    break;
                }
                Thread.Sleep(30000);

            }
            proxy.Close();
        }


    }



}
