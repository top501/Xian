#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.IdentityModel.Policy;
using System.Security.Cryptography.X509Certificates;

using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Ris.Application.Services;
using ClearCanvas.Ris.Application.Common;

namespace ClearCanvas.Ris.Server
{
    [ExtensionOf(typeof(ApplicationRootExtensionPoint))]
    public class Application : IApplicationRoot
    {
        private List<ServiceHost> _serviceHosts = new List<ServiceHost>();
        private const int OneMegaByte = 1048576;

        #region IApplicationRoot Members

        public void RunApplication(string[] args)
        {
            Platform.Log(LogLevel.Info, "Starting application root {0}", this.GetType().FullName);

            string baseAddress = WebServicesSettings.Default.BaseUrl;

            _serviceHosts = new List<ServiceHost>();

            MountServices(new CoreServiceExtensionPoint(), baseAddress);
            MountServices(new ApplicationServiceExtensionPoint(), baseAddress);

            Platform.Log(LogLevel.Info, "Starting WCF services on {0}...", baseAddress);
            foreach (ServiceHost host in _serviceHosts)
            {
                host.Open();
            }
            Platform.Log(LogLevel.Info, "WCF Services started on {0}", baseAddress);

            // kick NHibernate, rather than waiting for it to load on demand
            PersistentStoreRegistry.GetDefaultStore();

            Console.WriteLine("PRESS ANY KEY TO EXIT");

            Console.Read();

            Platform.Log(LogLevel.Info, "Stopping WCF services...");
            foreach (ServiceHost host in _serviceHosts)
            {
                host.Close();
            }
            Platform.Log(LogLevel.Info, "WCF services stopped.");
        }

        #endregion

        private void MountServices(IExtensionPoint serviceLayer, string baseAddress)
        {
            IServiceFactory serviceFactory = new ServiceFactory(serviceLayer);
            serviceFactory.ServiceCreation += ServiceCreationEventHandler;

            ICollection<Type> serviceClasses = serviceFactory.ListServiceClasses();
            foreach (Type serviceClass in serviceClasses)
            {
                MountService(serviceClass, serviceFactory, baseAddress);
            }
        }

        private void MountService(Type serviceClass, IServiceFactory serviceFactory, string baseAddress)
        {
            ServiceImplementsContractAttribute contractAttribute = AttributeUtils.GetAttribute<ServiceImplementsContractAttribute>(serviceClass, false);
            if(contractAttribute == null)
            {
                Platform.Log(LogLevel.Error, "Unknown contract for service {0}", serviceClass.Name);
                return;
            }

            Platform.Log(LogLevel.Info, "Mounting service {0}", serviceClass.Name);

            AuthenticationAttribute authenticationAttribute = AttributeUtils.GetAttribute<AuthenticationAttribute>(contractAttribute.ServiceContract);
            bool authenticated = authenticationAttribute == null ? true : authenticationAttribute.AuthenticationRequired;

            WSHttpBinding binding = new WSHttpBinding();
            binding.MaxReceivedMessageSize = OneMegaByte;
            binding.ReaderQuotas.MaxStringContentLength = OneMegaByte;
            binding.ReaderQuotas.MaxArrayLength = OneMegaByte;
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Message.ClientCredentialType = authenticated ?
                MessageCredentialType.UserName : MessageCredentialType.None;

            // create service host
            Uri uri = new Uri(new Uri(baseAddress), contractAttribute.ServiceContract.FullName);
            

            ServiceHost host = new ServiceHost(serviceClass, uri);

            // add behaviour to grab AOP proxied service instance
            host.Description.Behaviors.Add(new InstanceManagementServiceBehavior(contractAttribute.ServiceContract, serviceFactory));

            // establish endpoint
            host.AddServiceEndpoint(contractAttribute.ServiceContract, binding, "");

            // expose meta-data via HTTP GET
            ServiceMetadataBehavior metadataBehavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metadataBehavior == null)
            {
                metadataBehavior = new ServiceMetadataBehavior();
                metadataBehavior.HttpGetEnabled = true;
                host.Description.Behaviors.Add(metadataBehavior);
            }
#if DEBUG
            ServiceBehaviorAttribute debuggingBehavior = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            debuggingBehavior.IncludeExceptionDetailInFaults = true;
#endif
            // set up the certificate - required for WSHttpBinding
            host.Credentials.ServiceCertificate.SetCertificate(
                StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, uri.Host);

            if (authenticated)
            {
                // set up authentication model
                host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
                host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CustomUserValidator();


                // set up authorization
                List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
                policies.Add(new CustomAuthorizationPolicy());
                host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
                host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            }


            _serviceHosts.Add(host);
        }

        private void ServiceCreationEventHandler(object sender, ServiceCreationEventArgs e)
        {
            // insert the error handler advice at the beginning of the interception chain
            e.ServiceOperationInterceptors.Insert(0, new ErrorHandlerAdvice());
        }
    }
}
