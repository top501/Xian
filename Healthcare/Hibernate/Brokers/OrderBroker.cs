﻿#region License

// Copyright (c) 2010, ClearCanvas Inc.
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

using System.Collections.Generic;
using ClearCanvas.Enterprise.Hibernate.Hql;
using ClearCanvas.Healthcare.Brokers;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Healthcare.Hibernate.Brokers
{
	/// <summary>
	/// Implementation of <see cref="IOrderBroker"/>. See OrderBroker.hbm.xml for queries.
	/// </summary>
	public partial class OrderBroker : IOrderBroker
	{
		#region IOrderBroker Members

		public Order FindDocumentOwner(AttachedDocument document)
		{
			var q = this.GetNamedHqlQuery("documentOrderOwner");
			q.SetParameter(0, document);
			return (Order)q.UniqueResult();
		}

		public IList<Order> FindByOrderingPractitioner(ExternalPractitioner practitioner)
		{
			var q = this.GetNamedHqlQuery("ordersForOrderingPractitioner");
			q.SetParameter(0, practitioner);
			return CollectionUtils.Unique(q.List<Order>());
		}

		public IList<Order> FindByResultRecipient(ResultRecipientSearchCriteria recipientSearchCriteria, OrderSearchCriteria orderSearchCriteria)
		{
			var hqlFrom = new HqlFrom(typeof(Order).Name, "o");
			hqlFrom.Joins.Add(new HqlJoin("o.ResultRecipients", "rr"));

			var query = new HqlProjectionQuery(hqlFrom);
			query.Conditions.AddRange(HqlCondition.FromSearchCriteria("rr", recipientSearchCriteria));
			query.Conditions.AddRange(HqlCondition.FromSearchCriteria("o", orderSearchCriteria));
			return ExecuteHql<Order>(query);
		}

		public int CountByResultRecipient(ResultRecipientSearchCriteria recipientSearchCriteria, OrderSearchCriteria orderSearchCriteria)
		{
			var hqlSelect = new HqlSelect("count(*)");

			var hqlFrom = new HqlFrom(typeof(Order).Name, "o");
			hqlFrom.Joins.Add(new HqlJoin("o.ResultRecipients", "rr"));

			var query = new HqlProjectionQuery(hqlFrom, new[] { hqlSelect });
			query.Conditions.AddRange(HqlCondition.FromSearchCriteria("rr", recipientSearchCriteria));
			query.Conditions.AddRange(HqlCondition.FromSearchCriteria("o", orderSearchCriteria));
			return (int)ExecuteHqlUnique<long>(query);
		}

		#endregion
	}
}
