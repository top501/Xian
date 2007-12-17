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
using System.Text;

namespace ClearCanvas.Ris.Client
{
    /// <summary>
    /// Defines an interface for handling drag and drop operations
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public interface IDropHandler<TItem>
    {
        /// <summary>
        /// Return true if the specified items can be accepted by this handler
        /// </summary>
        /// <param name="dropContext">Provides information about the context of the drop operation.  The argument passed will
        /// typically extend the <see cref="IDropContext"/> interface in order to provide additional data, and the handler 
        /// will need to cast to a known subtype.</param>
        /// <param name="items">The items being dropped</param>
        /// <returns></returns>
        bool CanAcceptDrop(IDropContext dropContext, ICollection<TItem> items);

        /// <summary>
        /// Return true if the specified items were successfully processed by this handler
        /// </summary>
        /// <param name="dropContext">Provides information about the context of the drop operation.  The argument passed will
        /// typically extend the <see cref="IDropContext"/> interface in order to provide additional data, and the handler 
        /// will need to cast to a known subtype.</param>
        /// <param name="items">The items being dropped</param>
        /// <returns></returns>
        bool ProcessDrop(IDropContext dropContext, ICollection<TItem> items);
    }
}
