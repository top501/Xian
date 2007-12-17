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

using System.Threading;
using System.Collections.Generic;
using ClearCanvas.Enterprise.Authentication;
using ClearCanvas.Enterprise.Authentication.Brokers;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare;
using ClearCanvas.Healthcare.Alert;
using ClearCanvas.Healthcare.Brokers;
using ClearCanvas.Ris.Application.Common;

namespace ClearCanvas.Ris.Application.Services
{
    /// <summary>
    /// Base class for all RIS application services.
    /// </summary>
    /// <remarks>
    /// This class makes several important assumptions:
    /// 1. Instances are never shared across threads.
    /// 2. Instances are used on a per-call basis.  That is, an instance services a single request and is then discarded.
    /// </remarks>
    public abstract class ApplicationServiceBase : IApplicationServiceLayer
    {
        /// <summary>
        /// Cached current-user Staff object.  Caching is acceptable only if this service instance is never re-used.
        /// </summary>
        private Staff _currentUserStaff;

        /// <summary>
        /// Cached User object.  Caching is acceptable only if this service instance is never re-used.
        /// </summary>
        private User _currentUser;

        /// <summary>
        /// Obtains the staff associated with the current user.  If no <see cref="Staff"/> is associated with the current user,
        /// a <see cref="RequestValidationException"/> is thrown.
        /// </summary>
        protected Staff CurrentUserStaff
        {
            get
            {
                if (_currentUserStaff == null)
                {
                    try
                    {
                        _currentUserStaff = PersistenceContext.GetBroker<IStaffBroker>().FindStaffForUser(Thread.CurrentPrincipal.Identity.Name);
                    }
                    catch (EntityNotFoundException)
                    {
                        throw new RequestValidationException(SR.ExceptionNoStaffForUser);
                    }
                }

                return _currentUserStaff;
            }
        }

        /// <summary>
        /// Obtains the <see cref="User"/> entity for the current user.
        /// </summary>
        protected User CurrentUser
        {
            get
            {
                if(_currentUser == null)
                {
                    UserSearchCriteria criteria = new UserSearchCriteria();
                    criteria.UserName.EqualTo(Thread.CurrentPrincipal.Identity.Name);
                    _currentUser = this.PersistenceContext.GetBroker<IUserBroker>().FindOne(criteria);
                }

                return _currentUser;
            }
        }

        /// <summary>
        /// Gets the current <see cref="IPersistenceContext"/>.
        /// </summary>
        protected IPersistenceContext PersistenceContext
        {
            get { return PersistenceScope.Current; }
        }
    }
}
