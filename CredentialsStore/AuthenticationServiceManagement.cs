using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CredentialsStore
{
    public class AuthenticationServiceManagement : IAuthenticationServiceManagement
    {
        //Database init
        UsersDB db = new UsersDB();

        //Account policy init
        ConfigurationManager config = new ConfigurationManager();

        //Failed attempts storage init
        Dictionary<string, int> failedAttempts = new Dictionary<string, int>();

        public int ValidateCredentials(byte[] username, byte[] password, byte[] signature)
        {
            //RETURNS -4 IF SIGNATURE IS NOT VALID
            //RETURNS -3 IF USER IS DISABLED
            //RETURNS -2 IF USER IS LOCKED
            //RETURNS -1 IF USER DATA DOES NOT EXIST
            //RETURNS 0  IF USER DOES NOT EXISTS
            //RETURNS 1  IF USER DATA IS VALID

            //Current UsesDB init
            List<User> users = db.getUsers();

            //Digital signature validation 
            byte[] data = new byte[username.Length + password.Length];
            Buffer.BlockCopy(username, 0, data, 0, username.Length);
            Buffer.BlockCopy(password, 0, data, username.Length, password.Length);

            //INVALID DIGITAL SIGNATURE FOR DEMONSTRATION PURPOSES
            //Buffer.BlockCopy(password, 0, data, 0, password.Length);
            //Buffer.BlockCopy(username, 0, data, password.Length, username.Length);

            if (DigitalSignatureHelperFunctions.VerifyDigitalSignature(data, signature))
            {
                //Decrypting data
                string outUsername = AES.DecryptData(username, SecretKey.LoadKey(AES.KeyLocation));
                string outPassword = AES.DecryptData(password, SecretKey.LoadKey(AES.KeyLocation));

                for (int i = 0; i < users.Count(); i++)
                    if (users[i].GetUsername() == outUsername && (users[i].GetPassword() == outPassword.GetHashCode().ToString()))
                    {
                        if (users[i].GetDisabled())
                        {
                            db.addUsers(users);
                            return -3; //USER IS DISABLED
                        }
                        if (users[i].GetLocked())
                        {
                            db.addUsers(users);
                            return -2; //USER IS LOCKED
                        }

                        Console.WriteLine($"Account - {outUsername} with password {outPassword} verified successfully.\n");
                        users[i].SetLoggedTime(); //Confirming when the user logged in
                        db.addUsers(users);
                        return 1;
                    }

                    //FAILED ATTEMPTS COUNTER AND LOGIC
                    else if (users[i].GetUsername() == outUsername && (users[i].GetPassword() != outPassword.GetHashCode().ToString()))
                    {
                        if (failedAttempts.ContainsKey(outUsername))
                        {
                            failedAttempts[outUsername]++;
                            if (failedAttempts[outUsername] == config.GetFailedAttempts())
                            {
                                failedAttempts.Remove(outUsername);
                                users[i].SetLocked(true);
                                users[i].SetLockedTime();
                                db.addUsers(users);
                                return -2; //USER IS LOCKED
                            }

                        }
                        else
                            failedAttempts.Add(outUsername, 1);

                        return 0; //USER PASSWORD IS NOT VALID
                    }

                return -1; //USER DATA IS NOT VALID
            }
            else
                return -4; //SIGNATURE IS NOT VALID
        }

        //RETURNS  0  IF DATA IS RESET
        //RETURNS -1  IF SIGNATURE IS NOT VALID

        public int ResetUserOnLogOut(byte[] username, byte[] signature)
        {
            //Current UsesDB init
            List<User> users = db.getUsers();

            //Digital signature validation 
            if (DigitalSignatureHelperFunctions.VerifyDigitalSignature(username, signature))
            {
                //Decrypting data
                string outUsername = AES.DecryptData(username, SecretKey.LoadKey(AES.KeyLocation));

                for(int i = 0; i < users.Count(); i++)
                    if(users[i].GetUsername() == outUsername)
                    {
                        users[i].SetLoggedTime(string.Empty);
                        db.addUsers(users);
                        break;
                    }
                return 0; //DATA IS RESET
            }
            else
                return -1; //SIGNATURE IS NOT VALID
        }

        //RETURNS -5 IF THE SIGNATURE IS NOT VALID
        //RETURNS -4 TIMEOUT
        //RETURNS -3 DISABLED
        //RETURNS -2 LOCKED
        //RETURNS -1 IF USER DOES NOT EXISTS
        //RETURNS 0  IF USER DATA IS VALID

        public int CheckIn(byte[] username, byte[] signature)
        {
            List<User> users = db.getUsers();

            //Digital signature validation 
            if (DigitalSignatureHelperFunctions.VerifyDigitalSignature(username, signature))
            {
                    string outUsername = AES.DecryptData(username, SecretKey.LoadKey(AES.KeyLocation));

                for (int i = 0; i < users.Count(); i++)
                    if (users[i].GetUsername() == outUsername)
                    {
                        if (users[i].GetDisabled())
                        {
                            db.addUsers(users);
                            return -3; //USER IS DISABLED
                        }
                        if (users[i].GetLocked())
                        {
                            db.addUsers(users);
                            return -2; //USER IS LOCKED
                        }
                        if (users[i].GetLoggedInTime() == "")
                        {
                            db.addUsers(users);
                            return -4; // TIME OUT
                        }
                        Console.WriteLine($"Account - {outUsername} checked in successfully.\n");
                        db.addUsers(users);
                        return 1;
                    }

                return -1;
            }
            else
                return -5; //SIGNATURE IS NOT VALID

        }
    }
}
