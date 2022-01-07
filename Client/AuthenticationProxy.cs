using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Client
{
    public class AuthenticationProxy : ChannelFactory<IAuthenticationService>, IAuthenticationService, IDisposable
    {
        IAuthenticationService factory;

        public AuthenticationProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public int Login(string username, string password)
        {
            return factory.Login(username, password);
        }

        public void Logout(string username)
        {
            factory.Logout(username);
        }
    }
}
