#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Runtime.Serialization;
using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.Ris.Application.Common.Login
{
    [DataContract]
    public class LogoutRequest : LoginServiceRequestBase
    {
        public LogoutRequest(string user, SessionToken sessionToken, string clientIP, string clientMachineID)
			: base(user, clientIP, clientMachineID)
        {
            this.SessionToken = sessionToken;
        }

        /// <summary>
        /// SessionToken. Required.
        /// </summary>
        [DataMember]
        public SessionToken SessionToken;
    }
}
