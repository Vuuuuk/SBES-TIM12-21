using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using AutenticationService;

namespace AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        UserHandler uh = new UserHandler();
        NetTcpBinding binding = new NetTcpBinding();

        
        public int Login(string username, string password)
        {
            string credentialsStoreAddress = "net.tcp://localhost:6000/CredentialsStore";
            using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreAddress)) 
            {
                int k = credentialsStoreProxy.ValidateCredentials(username, password);
                if (k == 0)
                {
                    throw new Exception("That user does not exist!");
                }
                User u = new User(username, password);
                List<User> ime = uh.getUsers();
                ime.Add(u);
                uh.addUsers(ime);
                return k;
            }
        }

        public void Logout(string username)
        {
            List<User> ime = uh.getUsers();
            for(int i = 0;i < ime.Count; i++)
            {
                if(ime[i].username == username)
                {
                    ime.RemoveAt(i);
                    break;
                }
            }
            uh.addUsers(ime);
        }
    }
}
