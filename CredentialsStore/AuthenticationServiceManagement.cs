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
        UsersDB db = new UsersDB();

        public int ValidateCredentials(byte[] username, byte[] password, byte[] signature)
        {
            //RETURNS -4 IF SIGNATURE IS NOT VALID
            //RETURNS -3 IF USER IS DISABLED
            //RETURNS -2 IF USER IS LOCKED
            //RETURNS -1 IF USER DATA IS NOT VALID
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
                    if (users[i].GetUsername() == outUsername && (users[i].GetPassword() == outPassword))
                    {
                        if (users[i].GetDisabled())
                            return -3; //USER IS DISABLED
                        if (users[i].GetLocked())
                            return -2; //USER IS LOCKED

                        Console.WriteLine($"Account - {outUsername} with password {outPassword} verified successfully.\n");
                        db.addUsers(users);
                        return 1;
                    }

                return -1; //USER DATA IS NOT VALID
            }
            else
                return -4; //SIGNATURE IS NOT VALID
        }
    }
}
