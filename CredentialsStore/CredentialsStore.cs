using Client;
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

        DBHandler handler = new DBHandler();
        static string authenticationServiceAddress = "net.tcp://localhost:4000/AuthenticationService";
        AuthenticationProxy authenticationProxy = new AuthenticationProxy(new NetTcpBinding(), authenticationServiceAddress);


public void CreateAccount(string username, string password)
        {
            if(!Thread.CurrentPrincipal.IsInRole("Admin")) {

                string name = Thread.CurrentPrincipal.Identity.Name;
                Console.WriteLine("ACCESS DENIED! \t User " + name +" Tried to Initiate an Admin Action (Create Account) \t" + DateTime.Now.ToShortDateString());

                throw new Exception("ACCESS DENIED");
            }


            User U = new User(username, password, false, false, DateTime.Now);
            List<User> users = handler.getUsers();
            if (users.FindIndex(o => o.GetUsername() == username) == -1)
            {
                handler.addUser(U);
            }
            else
            {
                throw new Exception("That Username is Already Taken!");
            }
           // throw new NotImplementedException();
        }

        public void DeleteAccount(string username)
        {
            if (!Thread.CurrentPrincipal.IsInRole("Admin"))
            {

                string name = Thread.CurrentPrincipal.Identity.Name;
                Console.WriteLine("ACCESS DENIED! \t User " + name + " Tried to Initiate an Admin Action (Delete Account) \t" + DateTime.Now.ToShortDateString());

                throw new Exception("ACCESS DENIED");
            }


            List<User> users = handler.getUsers();
            if ((users.FindIndex(o => o.GetUsername() == username) != -1)) {
                users.RemoveAt(users.FindIndex(o => o.GetUsername() == username));

                handler.addUsers(users);
            }
            else
            {
                throw new Exception("That User Does Not Exist! Delete Failed");
            }
           // throw new NotImplementedException();
        }

        public void DisableAccount(string username)
        {
            if (!Thread.CurrentPrincipal.IsInRole("Admin"))
            {

                string name = Thread.CurrentPrincipal.Identity.Name;
                Console.WriteLine("ACCESS DENIED! \t User " + name + " Tried to Initiate an Admin Action (Disable Account) \t" + DateTime.Now.ToShortDateString());

                throw new Exception("ACCESS DENIED");
            }

            List<User> users = handler.getUsers();
            if ((users.FindIndex(o => o.GetUsername() == username) != -1)) {

              
                users[users.FindIndex(o => o.GetUsername() == username)].SetDisabled(true);

                handler.addUsers(users);
            }
            else
            {
                throw new Exception("That User Does Not Exist! Disable Failed");
            }
        }

        public void EnableAccount(string username)
        {

            if (!Thread.CurrentPrincipal.IsInRole("Admin"))
            {

                string name = Thread.CurrentPrincipal.Identity.Name;
                Console.WriteLine("ACCESS DENIED! \t User " + name + " Tried to Initiate an Admin Action (Enable Account) \t" + DateTime.Now.ToShortDateString());

                throw new Exception("ACCESS DENIED");
            }

            List<User> users = handler.getUsers();
            if ((users.FindIndex(o => o.GetUsername() == username) != -1))
            {


                users[users.FindIndex(o => o.GetUsername() == username)].SetDisabled(false);

                handler.addUsers(users);
            }
            else
            {
                throw new Exception("That User Does Not Exist! Enabling Failed");
            }
            }

        public void LockAccount(string username)
        {
            if (!Thread.CurrentPrincipal.IsInRole("Admin"))
            {

                string name = Thread.CurrentPrincipal.Identity.Name;
                Console.WriteLine("ACCESS DENIED! \t User " + name + " Tried to Initiate an Admin Action (Lock Account) \t" + DateTime.Now.ToShortDateString());

                throw new Exception("ACCESS DENIED");
            }

            List<User> users = handler.getUsers();
            if ((users.FindIndex(o => o.GetUsername() == username) != -1))
            {


                users[users.FindIndex(o => o.GetUsername() == username)].SetLocked(true);

                handler.addUsers(users);

                Thread.Sleep(new TimeSpan(0, 5, 0));

                users = handler.getUsers();
                users[users.FindIndex(o => o.GetUsername() == username)].SetLocked(false);
                handler.addUsers(users);
            }
            else
            {
                throw new Exception("That User Does Not Exist! Lock Failed");
            }
        }
    }
}
