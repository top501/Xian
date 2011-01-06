#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Threading;
using System.Web;
using System.Web.Security;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.ImageServer.Enterprise.Authentication;
using ClearCanvas.ImageServer.Web.Common.Exceptions;

namespace ClearCanvas.ImageServer.Web.Common.Security
{
    class CustomFormAuthenticationModule : IHttpModule
    {
        private bool _contextDisposed = false;

        #region IHttpModule Members

        private HttpApplication _context;
        public void Dispose()
        {
            if (_context != null && !_contextDisposed)
            {
                _context.AuthenticateRequest -= AuthorizeRequest;
                _context.Disposed -= ContextDisposed;
            }
        }

        public void Init(HttpApplication context)
        {
            _context = context;
            _context.AuthorizeRequest += AuthorizeRequest;
            _context.Disposed += ContextDisposed;
        }

        void ContextDisposed(object sender, EventArgs e)
        {
            _contextDisposed = true;
        }

        static void AuthorizeRequest(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.Identity is FormsIdentity)
                {
                    // Note: If user signed out in another window, the ticket would have been 
                    // removed from the browser and this code shoudn't be executed.
                    
                    // resemble the SessionInfo from the ticket.
                    FormsIdentity loginId = (FormsIdentity) HttpContext.Current.User.Identity ;
                    FormsAuthenticationTicket ticket = loginId.Ticket;

                    String[] fields = ticket.UserData.Split('|');
                    String tokenId = fields[0];
                    String userDisplayName = fields[1];
                    SessionToken token = new SessionToken(tokenId, ticket.Expiration);
                    SessionInfo  session = new SessionInfo(loginId.Name, userDisplayName, token);

                    // Initialize the session. This will throw exception if the session is no longer
                    // valid. For eg, time-out.
                    SessionManager.InitializeSession(session);
                }

                if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                {
                    String user = SessionManager.Current != null ? SessionManager.Current.User.Identity.Name : "Unknown";

                    Thread.CurrentThread.Name =
                        String.Format(SR.WebGUILogHeader, 
                            HttpContext.Current.Request.UserHostAddress,
                            HttpContext.Current.Request.Browser.Browser,
                            HttpContext.Current.Request.Browser.Version,
                            user);
                }
                
            }
            catch (SessionValidationException)
            {
                // redirect to login screen
                String error = String.Format("The current session is no longer valid.");
                SessionManager.TerminateSession(error);
            }
            catch(Exception ex)
            {
                // log the exception
                ExceptionHandler.ThrowException(ex);
            }
            
           
        }

        #endregion
    }
}
