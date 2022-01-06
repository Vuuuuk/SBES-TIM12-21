using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security.Principal;
using System.ServiceModel;

namespace AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        public void Login(string username, string password)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.generalUser))
                Console.WriteLine($"{Thread.CurrentPrincipal.Identity.Name} successfully logged in.\n");
                //TO IMPLEMENT
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }

        public void Logout()
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.generalUser))
                Console.WriteLine($"{Thread.CurrentPrincipal.Identity.Name} successfully logged out.\n");
                //TO IMPLEMENT
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }
    }
}
