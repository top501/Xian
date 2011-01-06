#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Collections.Generic;
using System.Security.Permissions;

using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Authentication;
using ClearCanvas.Enterprise.Authentication.Brokers;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Enterprise.Common.Admin.UserAdmin;
using ClearCanvas.Enterprise.Core;
using System.Threading;

namespace ClearCanvas.Enterprise.Authentication.Admin.UserAdmin
{
	[ExtensionOf(typeof(CoreServiceExtensionPoint))]
	[ServiceImplementsContract(typeof(IUserAdminService))]
    public class UserAdminService : CoreServiceLayer, IUserAdminService
    {
        #region IUserAdminService Members

        [ReadOperation]
        [PrincipalPermission(SecurityAction.Demand, Role = AuthorityTokens.Admin.Security.User)]
        public ListUsersResponse ListUsers(ListUsersRequest request)
        {
            UserSearchCriteria criteria = new UserSearchCriteria();
			criteria.UserName.SortAsc(0);

			// create the criteria, depending on whether matches should be "exact" or "like"
			if(request.ExactMatchOnly)
			{
				if (!string.IsNullOrEmpty(request.UserName))
					criteria.UserName.EqualTo(request.UserName);
				if (!string.IsNullOrEmpty(request.DisplayName))
					criteria.DisplayName.EqualTo(request.DisplayName);
			}
			else
			{
				if (!string.IsNullOrEmpty(request.UserName))
					criteria.UserName.StartsWith(request.UserName);
				if (!string.IsNullOrEmpty(request.DisplayName))
					criteria.DisplayName.Like(string.Format("%{0}%", request.DisplayName));
			}

            UserAssembler assembler = new UserAssembler();
            List<UserSummary> userSummaries = CollectionUtils.Map<User, UserSummary>(
                PersistenceContext.GetBroker<IUserBroker>().Find(criteria, request.Page),
                delegate(User user)
                {
                    return assembler.GetUserSummary(user);
                });
            return new ListUsersResponse(userSummaries);
        }

        [ReadOperation]
		[PrincipalPermission(SecurityAction.Demand, Role = AuthorityTokens.Admin.Security.User)]
		public LoadUserForEditResponse LoadUserForEdit(LoadUserForEditRequest request)
        {
            User user = FindUserByName(request.UserName);

            UserAssembler assembler = new UserAssembler();
            return new LoadUserForEditResponse(assembler.GetUserDetail(user));
        }

    	[UpdateOperation]
		[PrincipalPermission(SecurityAction.Demand, Role = AuthorityTokens.Admin.Security.User)]
		public AddUserResponse AddUser(AddUserRequest request)
        {
            Platform.CheckForNullReference(request, "request");
            Platform.CheckMemberIsSet(request.UserDetail, "UserDetail");

            UserDetail userDetail = request.UserDetail;
			AuthenticationSettings settings = new AuthenticationSettings();

			// create new user
    		UserInfo userInfo =
    			new UserInfo(userDetail.UserName, userDetail.DisplayName, userDetail.ValidFrom, userDetail.ValidUntil);

			User user = User.CreateNewUser(userInfo, settings.DefaultTemporaryPassword);

            // copy other info such as authority groups from request
            UserAssembler assembler = new UserAssembler();
            assembler.UpdateUser(user, request.UserDetail, PersistenceContext);

			// save
            PersistenceContext.Lock(user, DirtyState.New);
    		PersistenceContext.SynchState();

            return new AddUserResponse(user.GetRef(), assembler.GetUserSummary(user));
        }

        [UpdateOperation]
		[PrincipalPermission(SecurityAction.Demand, Role = AuthorityTokens.Admin.Security.User)]
		public UpdateUserResponse UpdateUser(UpdateUserRequest request)
        {
            User user = FindUserByName(request.UserDetail.UserName);

            // update user account info
            UserAssembler assembler = new UserAssembler();
            assembler.UpdateUser(user, request.UserDetail, PersistenceContext);

            // reset password if requested
            if (request.UserDetail.ResetPassword)
            {
				AuthenticationSettings settings = new AuthenticationSettings();
				user.ResetPassword(settings.DefaultTemporaryPassword);

            }

			PersistenceContext.SynchState();

			return new UpdateUserResponse(assembler.GetUserSummary(user));
        }

		[UpdateOperation]
		[PrincipalPermission(SecurityAction.Demand, Role = AuthorityTokens.Admin.Security.User)]
		public DeleteUserResponse DeleteUser(DeleteUserRequest request)
		{
			Platform.CheckForNullReference(request, "request");
			Platform.CheckForEmptyString(request.UserName, "UserName");

			// prevent current user from deleting own account
			if(request.UserName == Thread.CurrentPrincipal.Identity.Name)
				throw new RequestValidationException(SR.MessageCannotDeleteOwnUserAccount);

			IUserBroker broker = PersistenceContext.GetBroker<IUserBroker>();
			User user = FindUserByName(request.UserName);

			// remove user from groups we don't get errors from db references
			user.AuthorityGroups.Clear();

			// delete user
			broker.Delete(user);

			return new DeleteUserResponse();
		}

    	[UpdateOperation]
		[PrincipalPermission(SecurityAction.Demand, Role = AuthorityTokens.Admin.Security.User)]
		public ResetUserPasswordResponse ResetUserPassword(ResetUserPasswordRequest request)
        {
            Platform.CheckForNullReference(request, "request");
            Platform.CheckMemberIsSet(request.UserName, "UserName");

			User user = FindUserByName(request.UserName);

			AuthenticationSettings settings = new AuthenticationSettings();
			user.ResetPassword(settings.DefaultTemporaryPassword);

            UserAssembler assembler = new UserAssembler();
            return new ResetUserPasswordResponse(assembler.GetUserSummary(user));
        }

    	#endregion

        private User FindUserByName(string name)
        {
            try
            {
                UserSearchCriteria where = new UserSearchCriteria();
                where.UserName.EqualTo(name);

                return PersistenceContext.GetBroker<IUserBroker>().FindOne(where);
            }
            catch (EntityNotFoundException)
            {
                throw new RequestValidationException(string.Format("{0} is not a valid user name.", name));
            }
        }
    }
}
