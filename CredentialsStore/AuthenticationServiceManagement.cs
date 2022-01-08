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

        public int ValidateCredentials(byte[] username, byte[] password)
        {
            //RETURNS -3 IF USER IS DISABLED
            //RETURNS -2 IF USER IS LOCKED
            //RETURNS -1 IF USER DATA IS NOT VALID
            //RETURNS 0  IF USER DOES NOT EXISTS
            //RETURNS 1  IF USER DATA IS VALID

            //Current UsesDB init
            List<User> users = db.getUsers();

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
                    return 1;
                }

            return -1; //USER DATA IS NOT VALID
        }
    }
}
