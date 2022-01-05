using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Common
{
    [ServiceContract]
    public interface IAccountManagement
    {
        [OperationContract]
        void CreateAccount(string username, string password);

        [OperationContract]
        void DeleteAccount(string username);

        [OperationContract]
        void LockAccount(string username);

        [OperationContract]
        void EnableAccount(string username);

        [OperationContract]
        void DisableAccount(string username);
        [OperationContract]
        void Test(byte[] username);
    }
}
