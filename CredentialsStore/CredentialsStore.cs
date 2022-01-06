using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CredentialsStore
{
    public class CredentialsStore : IAccountManagement
    {
        public void CreateAccount(string username, string password)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.adminUser)) { }
            else
                throw new NotImplementedException();
        }

        public void DeleteAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.adminUser)) { }
            else
                throw new NotImplementedException();
        }

        public void DisableAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.adminUser)) { }
            else
                throw new NotImplementedException();
        }

        public void EnableAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.adminUser)) { }
            else
                throw new NotImplementedException();
        }

        public void LockAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.adminUser)) { }
            else
                throw new NotImplementedException();
        }
    }
}
