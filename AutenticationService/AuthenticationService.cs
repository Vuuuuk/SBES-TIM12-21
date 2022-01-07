using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        List<User> ime = new List<User>();
        NetTcpBinding binding = new NetTcpBinding();

        public void Login(string username, string password)
        {
            string credentialsStoreAddress = "net.tcp://localhost:6000/CredentialsStore";
            using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreAddress)) 
            {
                if (!credentialsStoreProxy.ValidateCredentials(username, password))
                {
                    throw new Exception("That user does not exist!");
                }
            }

            User u = new User(username, password);
            ime.Add(u);

        }

        public void Logout(string username)
        {
            for(int i = 0;i < ime.Count; i++)
            {
                if(ime[i].username == username)
                {
                    ime.RemoveAt(i);
                }
            }
        }
    }
}
