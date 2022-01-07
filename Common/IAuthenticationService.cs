using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Common
{
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        void Login(string username, string password);

        [OperationContract]
        void Logout(string username);
    }
}
