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
        [FaultContract(typeof(InvalidGroupException))]
        int Login(string username, string password);

        [OperationContract]
        [FaultContract(typeof(InvalidGroupException))]
        int Logout(string username);
    }
}
