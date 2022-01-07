using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutenticationService
{
    public class UserHandler
    {
        public List<User> getUsers()
        {
            List<User> ret = new List<User>();

            StreamReader sr = new StreamReader("users.txt");

            string linija = sr.ReadLine();
            string[] args = linija.Split('|');

            foreach(string s in args)
            {
                User u = new User(s,"");

                ret.Add(u);
            }

            sr.Close();

            return ret;
        }


        public void addUsers(List<User> users)
        {
            string text = "";

            for(int i = 0;i < users.Count; i++)
            {
                text = text + users[i].username + "|";
            }
            StreamWriter sw = new StreamWriter("users.txt", false, Encoding.ASCII);
            sw.WriteLine(text);
            sw.Close();
        }
    }
}
