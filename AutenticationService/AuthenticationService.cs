using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {

        public List<User> users = new List<User>();

        public void AccountDisabled(string username)
        {
            //credential check

            if(users.FindIndex(o => o.GetUsername() == username) != -1)
            {
                users[users.FindIndex(o => o.GetUsername() == username)].SetDisabled(true);

            }
        }

        public bool CheckIn(string username)
        {
            
               if ( users[users.FindIndex(o => o.GetUsername() == username)].GetDisabled() == true)
            {
                users.RemoveAt(users.FindIndex(o => o.GetUsername() == username));
                return false;
            }
            return true;

            
        }

        public void Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
