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
        [FaultContract(typeof(InvalidGroupException))]
        void CreateAccount(string username, string password);

        [OperationContract]
        [FaultContract(typeof(InvalidGroupException))]
        void DeleteAccount(string username);

        [OperationContract]
        [FaultContract(typeof(InvalidGroupException))]
        void LockAccount(string username);

        [OperationContract]
        [FaultContract(typeof(InvalidGroupException))]
        void EnableAccount(string username);

        [OperationContract]
        [FaultContract(typeof(InvalidGroupException))]
        void DisableAccount(string username);
    }
}
