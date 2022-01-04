using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace AuthenticationService
{
    class CredentialsStoreProxy : ChannelFactory<IAccountManagement>, IAccountManagement, IDisposable
    {
        IAccountManagement factory;

        public CredentialsStoreProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }
        public CredentialsStoreProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
           

           
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            factory = this.CreateChannel();
        }


        public void CreateAccount(string username, string password)
        {
            factory.CreateAccount(username, password);
        }

        public void DeleteAccount(string username)
        {
            factory.DeleteAccount(username);
        }

        public void DisableAccount(string username)
        {
            factory.DisableAccount(username);
        }

        public void EnableAccount(string username)
        {
            factory.EnableAccount(username);
        }

        public void LockAccount(string username)
        {
            factory.LockAccount(username);
        }
    }
}
