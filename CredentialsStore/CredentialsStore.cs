using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace CredentialsStore
{
    public class CredentialsStore : IAccountManagement
    {
        public void CreateAccount(string username, string password)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.AdminUser)) 
                Console.WriteLine("Account created.");
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }

        public void DeleteAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.AdminUser))
                Console.WriteLine("Account deleted.");
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }

        public void DisableAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.AdminUser))
                Console.WriteLine("Account disabled.");
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }

        public void EnableAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.AdminUser))
                Console.WriteLine("Account enabled.");
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }

        public void LockAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.AdminUser))
                Console.WriteLine("Account locked.");
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }
    }
}
