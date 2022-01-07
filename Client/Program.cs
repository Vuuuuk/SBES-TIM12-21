using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
            
            using (AuthenticationProxy authenticationProxy = new AuthenticationProxy(binding, authenticationServiceAddress)) 
            {
                int i = 0;
                string username = "";
                string password = "";
                while (i == 0)
                {
                    Console.WriteLine("Upisite username:");
                    username = Console.ReadLine();
                    Console.WriteLine("Upisite password:");
                    password = Console.ReadLine();
                    i = authenticationProxy.Login(username, password);
                    if(i == 0)
                    {
                        Console.WriteLine("Korisnik ne postoji.Upisite ponovo...");
                    }
                }
                if(i == 1)
                {
                    Console.WriteLine("Uspesno ste ulogovani kao user!\nPritisnite ENTER da biste se izlogovali.");
                    Console.ReadLine();
                }
                else
                {
                    using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreAddress))
                    {
                        int n;
                        do
                        {
                            Console.WriteLine("Uspesno ste ulogovani kao admin!\n");
                            Console.WriteLine("Izaberite jednu od opcija:");
                            Console.WriteLine("1. Create Account");
                            Console.WriteLine("2. Delete Account");
                            Console.WriteLine("3. Disable Account");
                            Console.WriteLine("4. Enable Account");
                            Console.WriteLine("5. Lock Account");
                            Console.WriteLine("6. Log Out");
                            n = Int32.Parse(Console.ReadLine());
                        
                            switch (n)
                            {
                                case 1:
                                    Console.WriteLine("Upisite username:");
                                    string userCreate = Console.ReadLine();
                                    Console.WriteLine("Upisite password:");
                                    string passCreate = Console.ReadLine();
                                    credentialsStoreProxy.CreateAccount(userCreate, passCreate);
                                    break;
                                case 2:
                                    Console.WriteLine("Upisite username:");
                                    string userDelete = Console.ReadLine();
                                    credentialsStoreProxy.DeleteAccount(userDelete);
                                    break;
                                case 3:
                                    Console.WriteLine("Upisite username:");
                                    string userDisable = Console.ReadLine();
                                    credentialsStoreProxy.DisableAccount(userDisable);
                                    break;
                                case 4:
                                    Console.WriteLine("Upisite username:");
                                    string userEnable = Console.ReadLine();
                                    credentialsStoreProxy.EnableAccount(userEnable);
                                    break;
                                case 5:
                                    Console.WriteLine("Upisite username:");
                                    string userLock = Console.ReadLine();
                                    credentialsStoreProxy.LockAccount(userLock);
                                    break;
                                case 6:
                                    authenticationProxy.Logout(username);
                                    break;

                            }
                        } while (n != 6);
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
