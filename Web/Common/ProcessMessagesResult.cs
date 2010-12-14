#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Runtime.Serialization;

namespace ClearCanvas.Web.Common
{
    [DataContract]
    public class ProcessMessagesResult
    {
        /// <summary>
        /// Indicates whether the message sent in ProcessMessages has been queued or processed.
        /// </summary>
        [DataMember]
        public bool Pending { get; set; }

        /// <summary>
        /// Set of events returned by the server. It may be the result of 
        /// the processing of either the current or previous messages.
        /// </summary>
        [DataMember]
        public EventSet EventSet { get; set; }
    }
}