using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Client
{
    public class CredentialsStoreProxy : ChannelFactory<IAccountManagement>, IAccountManagement, IDisposable
    {
        IAccountManagement factory;

        public CredentialsStoreProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void CreateAccount(string username, string password)
        {
            factory.CreateAccount(username, password);
        }

        public void DeleteAccount(string username)
        {
            factory.DeleteAccount(username);
        }

        public void DisableAccount(string username)
        {
            factory.DisableAccount(username);
        }

        public void EnableAccount(string username)
        {
            factory.EnableAccount(username);
        }

        public void LockAccount(string username)
        {
            factory.LockAccount(username);
        }

        public int ValidateCredentials(string username, string password)
        {
            return factory.ValidateCredentials(username,password);
        }
    }
}
