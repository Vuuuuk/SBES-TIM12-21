using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace AuthenticationService
{
    class CredentialsStoreProxy : ChannelFactory<IAuthenticationServiceManagement>, IAuthenticationServiceManagement, IDisposable
    {
        IAuthenticationServiceManagement factory;

        public CredentialsStoreProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            //Credentials.Windows.AllowNtlm = false; not usable as we dont have domain controllers.
            factory = this.CreateChannel();
        }
        public CredentialsStoreProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            //Client certificate configuration init

            string clientName = CertificateFormatter.ParseName(WindowsIdentity.GetCurrent().Name); //Parsed WindowsIdentity.Name
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust; //Authority validation mode
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck; //Do not check if Authority marked the certificate as unusable 
            this.Credentials.ClientCertificate.Certificate = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, clientName); //Client public/private-key.PFX 

            //Credentials.Windows.AllowNtlm = false; not usable as we dont have domain controllers.
            factory = this.CreateChannel();
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
