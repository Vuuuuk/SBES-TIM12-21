using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CredentialsStore
{
    public class UsersDB
    {

        //Configuration manager init
        ConfigurationManager conf = new ConfigurationManager();

        public void addUser(User U)
        {
            string text = "";
            string locked;
            string disabled;

            if (U.GetLocked()) 
                locked = "Y"; 
            else locked = "N";

            if (U.GetDisabled()) 
                disabled = "Y";
            else disabled = "N";

            text = text + U.GetUsername() + "|" + U.GetPassword().GetHashCode() + "|" + locked + "|" + disabled + "|" +
                          U.GetLockedTime() + "|" + U.GetLoggedInTime();

            StreamWriter sw = new StreamWriter("usersDB.txt", true, Encoding.ASCII);
            sw.WriteLine(text);
            sw.Close();
        }

        public void addUsers(List<User> users)
        {
            string text = "";
            string locked;
            string disabled;

            foreach (User U in users)
            {

                if (U.GetLocked()) locked = "Y"; else locked = "N";
                if (U.GetDisabled()) disabled = "Y"; else disabled = "N";
                text = text + U.GetUsername() + "|" + U.GetPassword() + "|" + locked + "|" + disabled + "|" +
                    U.GetLockedTime() + "|" + U.GetLoggedInTime() + "\n";
            }
            StreamWriter sw = new StreamWriter("usersDB.txt", false, Encoding.ASCII);
            sw.Write(text);
            sw.Close();
        }

        public List<User> getUsers()
        {
            List<User> ret = new List<User>();

            StreamReader sr = new StreamReader("usersDB.txt");

            string line = sr.ReadLine();
            while (line != null)
            {
                if (line != "" && line != "\n")
                {
                    string[] args = line.Split('|');

                    bool locked = false;
                    bool disabled = false;

                    if (args[2] == "Y")
                        locked = true;
                    if (args[3] == "Y")
                        disabled = true;

                    //Automatic account unlocking based on getUsers() method call
                    if (locked)
                    {
                        int vreme =  ((int.Parse(args[4].Split(':')[0]) * 60) + (int.Parse(args[4].Split(':')[1]))) + conf.GetLockDuration(); 
                        if (vreme <= ((DateTime.Now.Hour * 60) + DateTime.Now.Minute))
                        {
                            locked = false;
                            args[4] = "";
                        }
                    }

                    //Automatic account 
                    if (args[5] != string.Empty)
                    {
                        int lVreme = ((int.Parse(args[5].Split(':')[0]) * 60) + (int.Parse(args[5].Split(':')[1]))) + conf.GetTimeOutInterval(); 
                        if (lVreme <= ((DateTime.Now.Hour * 60) + DateTime.Now.Minute))
                        {
                            disabled = true;
                            args[5] = "";
                        }
                    }

                    User U = new User(args[0], args[1], locked, disabled, args[4], args[5]);
                    ret.Add(U);
                    line = sr.ReadLine();
                }
                else
                    line = sr.ReadLine();
            }

            sr.Close();

            return ret;
        }
    }
}
