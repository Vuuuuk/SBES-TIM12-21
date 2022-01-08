using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace CredentialsStore
{
    public class DBHandler
    {
       
        public DBHandler()
        {
           
        }

        public void addUser(User U)
        {
            string text = "";
            string locked;
            string disabled;
            string admin;

            if (U.GetLocked()) locked = "Y"; else locked = "N";
            if (U.GetDisabled()) disabled = "Y"; else disabled = "N";
            if (U.getAdmin()) admin = "Y"; else admin = "N";
            text = text + U.GetUsername() + "|" + U.GetPassword() + "|" + locked + "|" + disabled + "|" +
                U.GetLockedTime().Year + "|" + U.GetLockedTime().Month + "|" + U.GetLockedTime().Day +  "|" + admin + "|"  + "\n";

            StreamWriter sw = new StreamWriter("users.txt",true, Encoding.ASCII);
            sw.WriteLine(text);
            sw.Close();

        }

        public List<User> getUsers()
        {
            List<User> ret = new List<User>();

            StreamReader sr = new StreamReader("users.txt");

            string line = sr.ReadLine();
            while (line != null)
            {
                if (line != "" && line != "\n")
                {
                    string[] args = line.Split('|');

                    bool locked = false;
                    bool disabled = false;
                    bool admin = false;




                    if (args[2] == "Y")
                        locked = true;
                    if (args[3] == "Y")
                        disabled = true;
                    if (args[7] == "Y")
                        admin = true;


                    User U = new User(args[0], args[1], locked, disabled, new DateTime(int.Parse(args[4]), int.Parse(args[5]), int.Parse(args[6])),admin);
                    ret.Add(U);
                    line = sr.ReadLine();
                }
                else
                {
                   line =  sr.ReadLine();
                }
            }

            sr.Close();

            return ret;
        }


        public void addUsers(List<User> users)
        {
            string text = "";
            string locked;
            string disabled;
            string admin;

            foreach (User U in users)
            {

                if (U.GetLocked()) locked = "Y"; else locked = "N";
                if (U.GetDisabled()) disabled = "Y"; else disabled = "N";
                if (U.getAdmin()) admin = "Y"; else admin = "N";
                text = text + U.GetUsername() + "|" + U.GetPassword() + "|" + locked + "|" + disabled + "|" +
                    U.GetLockedTime().Year + "|" + U.GetLockedTime().Month + "|" + U.GetLockedTime().Day + "|" + admin + "|" + "\n";
            }
            StreamWriter sw = new StreamWriter("users.txt",false, Encoding.ASCII);
            sw.WriteLine(text);
            sw.Close();
        }

    }
}
