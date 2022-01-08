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

            binding.Security.Mode = SecurityMode.Message; //Safer but slower then SecurityMode.Transport as it encrypts each message separately
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows; //Based on windows user accounts
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign; //Anti-Tampering signature (per message) protection

            Console.WriteLine($"Currently used by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + "\n");

            string username = "";
            string password = "";
            string option = "";

            using (AuthenticationProxy authenticationProxy = new AuthenticationProxy(binding, authenticationServiceAddress)) 
            {
                using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreAddress))
                {
                    do
                    {
                        Console.WriteLine("-----SERVER OPTIONS-----\n");
                        Console.WriteLine("1. Sign In");
                        Console.WriteLine("2. Create Account");
                        Console.WriteLine("3. Delete Account");
                        Console.WriteLine("4. Disable Account");
                        Console.WriteLine("5. Enable Account");
                        Console.WriteLine("6. Lock Account");
                        Console.WriteLine("7. Sign Out");

                        Console.Write("\n-> ");
                        option = Console.ReadLine();

                        switch (option)
                        {
                            case "1":
                                if(username == "")
                                {
                                    Console.Write("Your username: ");
                                    username = Console.ReadLine();
                                    Console.Write("Your password: ");
                                    password = Console.ReadLine();

                                    try
                                    {
                                        int ret = authenticationProxy.Login(username, password);

                                        switch (ret)
                                        {
                                            case -3:
                                                Console.WriteLine($"\n{username} is DISABLED. Please contact your system administrator.\n");
                                                username = "";
                                                break;
                                            case -2:
                                                Console.WriteLine($"\n{username} is LOCKED. Please contact your system administrator or wait some time and try again.\n");
                                                username = "";
                                                break;
                                            case -1:
                                                Console.WriteLine($"\n{username} or your password does not exist please try again.\n");
                                                username = "";
                                                break;
                                            case 0:
                                                Console.WriteLine($"\n{username} does not exist in our Database. Please contact your system administrator.\n");
                                                username = "";
                                                break;
                                            case 1:
                                                Console.WriteLine($"\n{username} successfully logged in.\n");
                                                break;
                                        }
                                    }

                                    catch (FaultException<InvalidGroupException> ex)
                                    {
                                        Console.WriteLine(ex.Detail.exceptionMessage);
                                        username = "";
                                        break;
                                    }
                                }
                                else
                                    Console.WriteLine("You are already logged in, please log out first.\n");

                                break;
                            case "2":
                                try
                                {
                                    Console.Write("Account username: ");
                                    username = Console.ReadLine();
                                    Console.Write("Account password: ");
                                    password = Console.ReadLine();
                                    credentialsStoreProxy.CreateAccount(username, password);
                                    Console.WriteLine("Account successfully created.\n");
                                    break;

                                }
                                catch (FaultException<InvalidGroupException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                                catch (FaultException<InvalidUserException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                            case "3":
                                try
                                {
                                    Console.Write("Account username: ");
                                    username = Console.ReadLine();
                                    credentialsStoreProxy.DeleteAccount(username);
                                    Console.WriteLine("Account successfully deleted.\n");
                                    break;
                                }
                                catch (FaultException<InvalidGroupException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                                catch (FaultException<InvalidUserException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                            case "4":
                                try
                                {
                                    Console.Write("Account username: ");
                                    username = Console.ReadLine();
                                    credentialsStoreProxy.DisableAccount(username);
                                    Console.WriteLine("Account successfully disabled.\n");
                                    break;
                                }
                                catch (FaultException<InvalidGroupException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                                catch (FaultException<InvalidUserException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;

                                }
                            case "5":
                                try
                                {
                                    Console.Write("Account username: ");
                                    username = Console.ReadLine();
                                    credentialsStoreProxy.EnableAccount(username);
                                    Console.WriteLine("Account successfully enabled.\n");
                                    break;
                                }
                                catch (FaultException<InvalidGroupException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                                catch (FaultException<InvalidUserException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                            case "6":
                                try
                                {
                                    Console.Write("Account username: ");
                                    username = Console.ReadLine();
                                    credentialsStoreProxy.LockAccount(username);
                                    Console.WriteLine("Account successfully locked.\n");
                                    break;
                                }
                                catch (FaultException<InvalidGroupException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                                catch (FaultException<InvalidUserException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                            case "7":
                                if (username != "")
                                {
                                    authenticationProxy.Logout(username);
                                    username = "";
                                }
                                else
                                    Console.WriteLine("You have to be logged in first.\n");
                                break;
                        }

                    } while (option != "exit");
                }
            }
        }
    }
}
