using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{

    public class User
    {
        private string username;

        public string GetUsername()
        {
            return username;
        }

        public void SetUsername(string value)
        {
            username = value;
        }

        private string password;

        public string GetPassword()
        {
            return password;
        }

        public void SetPassword(string value)
        {
            password = value;
        }

        private bool locked;

        public bool GetLocked()
        {
            return locked;
        }

        public void SetLocked(bool value)
        {
            locked = value;
        }

        private bool disabled;

        public bool GetDisabled()
        {
            return disabled;
        }

        public void SetDisabled(bool value)
        {
            disabled = value;
        }

        private DateTime lockedTime;

        public DateTime GetLockedTime()
        {
            return lockedTime;
        }

        public void SetLockedTime(DateTime value)
        {
            lockedTime = value;
        }

        public User(string username, string password, bool locked, bool disabled, DateTime lockedTime, bool admin)
        {
            this.SetUsername(username);
            this.SetPassword(password);
            this.SetLocked(locked);
            this.SetDisabled(disabled);
            this.SetLockedTime(lockedTime);
            isAdmin = admin;
        }

        private bool isAdmin;

        public bool getAdmin()
        {
            return isAdmin;
        }

        public DateTime getLockedDuration()
        {
            DateTime ret = GetLockedTime().AddMinutes(5);
            return ret;
        }
    }
}
