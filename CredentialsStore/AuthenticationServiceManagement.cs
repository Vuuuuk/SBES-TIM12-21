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
        public void DisableAccount(string username)
        {
            Console.WriteLine("Account disabled.\n");
            //TO IMPLEMENT
        }

        public void EnableAccount(string username)
        {
            Console.WriteLine("Account enabled.\n");
            //TO IMPLEMENT
        }

        public void LockAccount(string username)
        {
            Console.WriteLine("Account locked.\n");
            //TO IMPLEMENT
        }
    }
}
