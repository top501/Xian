#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Net;
using System.ServiceModel;
using System.Web;
using ClearCanvas.Common;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Enterprise.Common.Authentication;
using ClearCanvas.ImageServer.Common;

namespace ClearCanvas.ImageServer.Enterprise.Authentication
{
    /// <summary>
    /// Wrapper for <see cref="IAuthenticationService"/> service.
    /// </summary>
    public sealed class LoginService : IDisposable
    {
        public SessionInfo Login(string userName, string password, string appName)
        {
            Platform.CheckForEmptyString(userName, "userName");
            Platform.CheckForEmptyString(password, "password");
            Platform.CheckForEmptyString(appName, "appName");

            SessionInfo session = null;
            
            Platform.GetService<IAuthenticationService>(
                delegate(IAuthenticationService  service)
                    {
                        try
                        {

                            InitiateSessionRequest request = new InitiateSessionRequest(userName, appName, Dns.GetHostName(), password);

                            request.GetAuthorizations = true;
                            InitiateSessionResponse response = service.InitiateSession(request);

                            if (response != null)
                            {
                                LoginCredentials credentials = new LoginCredentials();
                                credentials.UserName = userName;
                                credentials.DisplayName = response.DisplayName;
                                credentials.SessionToken = response.SessionToken;
                                credentials.Authorities = response.AuthorityTokens;
                                CustomPrincipal user = new CustomPrincipal(new CustomIdentity(userName, response.DisplayName),credentials);
                                session = new SessionInfo(user);

                                Platform.Log(LogLevel.Info, "{0} has successfully logged in.", userName);                                
                            }
                        }
                        catch (FaultException<PasswordExpiredException> ex)
                        {
                            throw ex.Detail;
                        }
                        catch(FaultException<UserAccessDeniedException> ex)
                        {

                            throw ex.Detail;
                        }
                    }
                );

            return session;
        }

        public void Logout(SessionInfo session)
        {
            TerminateSessionRequest request =
                new TerminateSessionRequest(session.User.Identity.Name, session.Credentials.SessionToken);

            Platform.GetService<IAuthenticationService>(
                delegate(IAuthenticationService service)
                    {
                        service.TerminateSession(request);
                    });
        }

        public void Validate(SessionInfo session)
        {
            ValidateSessionRequest request = new ValidateSessionRequest(session.User.Identity.Name, session.Credentials.SessionToken);
            request.GetAuthorizations = true;

            try
            {
                Platform.GetService<IAuthenticationService>(
                delegate(IAuthenticationService service)
                    {
                        ValidateSessionResponse response = service.ValidateSession(request);
                        // update session info
                        session.Credentials.Authorities = response.AuthorityTokens;
                        session.Credentials.SessionToken = response.SessionToken;
                    });
            }
			catch(FaultException<InvalidUserSessionException> ex)
			{
				throw new SessionValidationException(ex.Detail);
			}
            catch(Exception ex)
            {
                //TODO: for now we can't distinguish communicate errors and credential validation errors.
                // All exceptions are treated the same: we can't verify the login.
                SessionValidationException e = new SessionValidationException(ex);
                throw e;
            }
            

            
        }

        public void ChangePassword(string userName, string oldPassword, string newPassword)
        {

            ChangePasswordRequest request = new ChangePasswordRequest(userName, oldPassword, newPassword);
            Platform.GetService<IAuthenticationService>(
                delegate(IAuthenticationService service)
                    {
                        service.ChangePassword(request);
                        Platform.Log(LogLevel.Info, "Password for {0} has been changed.", userName);
                    });
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}