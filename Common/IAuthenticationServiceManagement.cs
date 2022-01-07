using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Common
{
    [ServiceContract]
    public interface IAuthenticationServiceManagement
    {
        [OperationContract]
        bool ValidateCredentials(byte[] username, byte[] password);

        [OperationContract]
        void LockAccount(byte[] username);

        [OperationContract]
        void EnableAccount(byte[] username);

        [OperationContract]
        void DisableAccount(byte[] username);
    }
}
