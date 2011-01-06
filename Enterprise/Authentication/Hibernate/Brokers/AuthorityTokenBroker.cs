#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Enterprise.Hibernate.Hql;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Enterprise.Authentication.Hibernate.Brokers
{
    public partial class AuthorityTokenBroker
    {
        public string[] FindTokensByUserName(string userName)
        {
            UserSearchCriteria where = new UserSearchCriteria();
            where.UserName.EqualTo(userName);

            // want this to be as fast as possible - use joins and only select the AuthorityToken names
            HqlQuery query = new HqlQuery("select distinct t.Name from User u join u.AuthorityGroups g join g.AuthorityTokens t");
            query.Conditions.AddRange(HqlCondition.FromSearchCriteria("u", where));

            // take advantage of query caching if possible
            query.Cacheable = true;

            IList<string> tokens = this.ExecuteHql<string>(query);
            string[] result = new string[tokens.Count];
            tokens.CopyTo(result, 0);
            return result;
        }

        public bool AssertUserHasToken(string userName, string token)
        {
            UserSearchCriteria whereUser = new UserSearchCriteria();
            whereUser.UserName.EqualTo(userName);

            AuthorityTokenSearchCriteria whereToken = new AuthorityTokenSearchCriteria();
            whereToken.Name.EqualTo(token);

            // want this to be as fast as possible - use joins and only select the count
            HqlQuery query = new HqlQuery("select count(*) from User u join u.AuthorityGroups g join g.AuthorityTokens t");
            query.Conditions.AddRange(HqlCondition.FromSearchCriteria("u", whereUser));
            query.Conditions.AddRange(HqlCondition.FromSearchCriteria("t", whereToken));

            // expect exactly one integer result
            return ExecuteHqlUnique<long>(query) > 0;
        }

    }
}
