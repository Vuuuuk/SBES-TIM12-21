using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CredentialsStore
{
    public class AuthenticationServiceManagement : IAuthenticationServiceManagement
    {
        public bool ValidateCredentials(byte[] username, byte[] password)
        {
            string outUsername = AES.DecryptData(username, SecretKey.LoadKey(AES.KeyLocation));
            string outPassword = AES.DecryptData(password, SecretKey.LoadKey(AES.KeyLocation));
            Console.WriteLine($"Account - {outUsername} with password {outPassword} verified successfully.\n");
            return true;
            //TO IMPLEMENT
        }

        public void DisableAccount(byte[] username)
        {
            Console.WriteLine("Account disabled.\n");
            //TO IMPLEMENT
        }

        public void EnableAccount(byte[] username)
        {
            Console.WriteLine("Account enabled.\n");
            //TO IMPLEMENT
        }

        public void LockAccount(byte[] username)
        {
            Console.WriteLine("Account locked.\n");
            //TO IMPLEMENT
        }
    }
}
