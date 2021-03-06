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

        //[ThreadStatic] - Thread safety is important, we are going to use this snippet when we are making multiple connection from different places, every thread is using its own instance
        private static CredentialsStoreProxy instance = null; //Singleton pattern instance

        IAuthenticationServiceManagement factory = null;

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

        //SINGLETON PATTERN
        public static CredentialsStoreProxy SingletonInstance()
        {
            //If the instance is NULL, create one with the parameter bellow using the default Constructor for this class
            if (instance == null)
            {
                //CS server certificate configuration init
                NetTcpBinding bindingCredentialsStore = new NetTcpBinding();
                bindingCredentialsStore.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate; //Certificate-based authentication
                X509Certificate2 serverCertificate = CertificateManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, "credentialsstore"); //Server public-key.CER 
                EndpointAddress credentialsStoreAddress = new EndpointAddress(new Uri("net.tcp://localhost:6000/CredentialsStore"),
                                                                              new X509CertificateEndpointIdentity(serverCertificate));

                instance = new CredentialsStoreProxy(bindingCredentialsStore, credentialsStoreAddress);
            }

            return instance;
        }

        public int ValidateCredentials(byte[] username, byte[] password, byte[] signature)
        {
            return factory.ValidateCredentials(username, password, signature);
        }

        public int ResetUserOnLogOut(byte[] username, byte[] signature)
        {
            return factory.ResetUserOnLogOut(username, signature);
        }

        public int CheckIn(byte[] username, byte[] signature)
        {
            return factory.CheckIn(username, signature);
        }
    }
}
