﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
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
using System.Security.Permissions;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Healthcare;
using ClearCanvas.Healthcare.Brokers;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.Admin.$fileinputname$Admin;
using AuthorityTokens=ClearCanvas.Ris.Application.Common.AuthorityTokens;

namespace $rootnamespace$
{
    [ExtensionOf(typeof(ApplicationServiceExtensionPoint))]
    [ServiceImplementsContract(typeof(I$fileinputname$AdminService))]
    public class $fileinputname$AdminService : ApplicationServiceBase, I$fileinputname$AdminService
    {
        #region I$fileinputname$AdminService Members

        [ReadOperation]
        public List$fileinputname$sResponse List$fileinputname$s(List$fileinputname$sRequest request)
        {
            Platform.CheckForNullReference(request, "request");
			
			$fileinputname$SearchCriteria where = new $fileinputname$SearchCriteria();
			//TODO: add sorting
		
            I$fileinputname$Broker broker = PersistenceContext.GetBroker<I$fileinputname$Broker>();
            IList<$fileinputname$> items = broker.Find(where, request.Page);

            $fileinputname$Assembler assembler = new $fileinputname$Assembler();
            return new List$fileinputname$sResponse(
                CollectionUtils.Map<$fileinputname$, $fileinputname$Summary>(items,
                    delegate($fileinputname$ item)
                    {
                        return assembler.CreateSummary(item, PersistenceContext);
                    })
                );
        }

        [ReadOperation]
        public Load$fileinputname$ForEditResponse Load$fileinputname$ForEdit(Load$fileinputname$ForEditRequest request)
        {
            Platform.CheckForNullReference(request, "request");
            Platform.CheckMemberIsSet(request.$fileinputname$Ref, "request.$fileinputname$Ref");

            $fileinputname$ item = PersistenceContext.Load<$fileinputname$>(request.$fileinputname$Ref);

            $fileinputname$Assembler assembler = new $fileinputname$Assembler();
            return new Load$fileinputname$ForEditResponse(assembler.CreateDetail(item, PersistenceContext));
        }

        [ReadOperation]
        public Load$fileinputname$EditorFormDataResponse Load$fileinputname$EditorFormData(Load$fileinputname$EditorFormDataRequest request)
        {
             return new Load$fileinputname$EditorFormDataResponse();
        }

        [UpdateOperation]
        [PrincipalPermission(SecurityAction.Demand, Role = AuthorityTokens.Admin.Data.$fileinputname$)]
        public Add$fileinputname$Response Add$fileinputname$(Add$fileinputname$Request request)
        {
            Platform.CheckForNullReference(request, "request");
            Platform.CheckMemberIsSet(request.$fileinputname$, "request.$fileinputname$");

            $fileinputname$ item = new $fileinputname$();

            $fileinputname$Assembler assembler = new $fileinputname$Assembler();
            assembler.Update$fileinputname$(item, request.$fileinputname$, PersistenceContext);

            PersistenceContext.Lock(item, DirtyState.New);
            PersistenceContext.SynchState();

            return new Add$fileinputname$Response(assembler.CreateSummary(item, PersistenceContext));
        }

        [UpdateOperation]
        [PrincipalPermission(SecurityAction.Demand, Role = AuthorityTokens.Admin.Data.$fileinputname$)]
        public Update$fileinputname$Response Update$fileinputname$(Update$fileinputname$Request request)
        {
            Platform.CheckForNullReference(request, "request");
            Platform.CheckMemberIsSet(request.$fileinputname$, "request.$fileinputname$");
            Platform.CheckMemberIsSet(request.$fileinputname$.$fileinputname$Ref, "request.$fileinputname$.$fileinputname$Ref");

            $fileinputname$ item = PersistenceContext.Load<$fileinputname$>(request.$fileinputname$.$fileinputname$Ref);

            $fileinputname$Assembler assembler = new $fileinputname$Assembler();
            assembler.Update$fileinputname$(item, request.$fileinputname$, PersistenceContext);

            PersistenceContext.SynchState();

            return new Update$fileinputname$Response(assembler.CreateSummary(item, PersistenceContext));
        }

        #endregion
    }
}
