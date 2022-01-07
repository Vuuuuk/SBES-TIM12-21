using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security.Principal;
using System.ServiceModel;

namespace AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        public void Login(string username, string password)
        {

            if (Thread.CurrentPrincipal.IsInRole(Groups.GeneralUser))
            {
                using (CredentialsStoreProxy credentialsStoreProxy = CredentialsStoreProxy.SingletonInstance())
                {
                    try
                    {
                        //Encrypting data
                        byte[] outUsername = AES.EncryptData(username, SecretKey.LoadKey(AES.KeyLocation));
                        byte[] outPassword = AES.EncryptData(password, SecretKey.LoadKey(AES.KeyLocation));

                        if (credentialsStoreProxy.ValidateCredentials(outUsername, outPassword))
                        {
                            //TO DO
                            Console.WriteLine("Successfully logged in.\n");
                        }
                        else
                        {
                            //TO DO
                            Console.WriteLine("Not successfully logged in.\n");
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine("Client certificate check failed. Please contact your system administrator.\n");
                    }
                }
            }
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }

        public void Logout()
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.GeneralUser))
                Console.WriteLine($"{Thread.CurrentPrincipal.Identity.Name} successfully logged out.\n");
                //TO IMPLEMENT
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }
    }
}
