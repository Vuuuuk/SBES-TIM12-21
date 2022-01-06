using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace AuthenticationService
{
    class CredentialsStoreProxy : ChannelFactory<IAuthenticationServiceManagement>, IAuthenticationServiceManagement, IDisposable
    {
        IAuthenticationServiceManagement factory;

        public CredentialsStoreProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            //Credentials.Windows.AllowNtlm = false; not usable as we dont have domain controllers.
            factory = this.CreateChannel();
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
    }
}
