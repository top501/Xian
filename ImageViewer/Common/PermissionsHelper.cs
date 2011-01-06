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
using System.Threading;
using ClearCanvas.Common;
using ClearCanvas.ImageViewer;

namespace ClearCanvas.ImageViewer.Common
{
	/// <summary>
	/// Helper class for determining whether or not the current user has certain viewer-specific permissions.
	/// </summary>
	/// <remarks>
	/// Regardless of what is input, each of the methods in this class will unconditionally check for
	/// the <see cref="AuthorityTokens.ViewerVisible"/> token, which is a global token intended to limit
	/// access to all viewer components.
	/// </remarks>
	public static class PermissionsHelper
	{
		/// <summary>
		/// Checks whether the current user has the correct permissions based on the provided authority token.
		/// </summary>
		public static bool IsInRole(string authorityToken)
		{
			return IsInRoles(authorityToken);
		}

		/// <summary>
		/// Checks whether the current user has the correct permissions based on the provided authority tokens.
		/// </summary>
		public static bool IsInRoles(params string[] authorityTokens)
		{
			return IsInRoles((IEnumerable<string>) (authorityTokens ?? new string[0]));
		}

		/// <summary>
		/// Checks whether the current user has the correct permissions based on the provided authority tokens.
		/// </summary>
		public static bool IsInRoles(IEnumerable<string> authorityTokens)
		{
			if (Thread.CurrentPrincipal == null || !Thread.CurrentPrincipal.Identity.IsAuthenticated)
				return true;

			if (!Thread.CurrentPrincipal.IsInRole(AuthorityTokens.ViewerVisible))
				return false;

			authorityTokens = authorityTokens ?? new string[0];

			foreach (string authorityToken in authorityTokens)
			{
				if (!Thread.CurrentPrincipal.IsInRole(authorityToken))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Checks whether the current user is in at least one of the specified roles/authority tokens.
		/// </summary>
		public static bool IsInAnyRole(params string[] authorityTokens)
		{
			return IsInAnyRole((IEnumerable<string>)(authorityTokens ?? new string[0]));
		}

		/// <summary>
		/// Checks whether the current user is in at least one of the specified roles/authority tokens.
		/// </summary>
		public static bool IsInAnyRole(IEnumerable<string> authorityTokens)
		{
			if (Thread.CurrentPrincipal == null || !Thread.CurrentPrincipal.Identity.IsAuthenticated)
				return true;

			if (!Thread.CurrentPrincipal.IsInRole(AuthorityTokens.ViewerVisible))
				return false; //doesn't matter which roles they're in.

			authorityTokens = authorityTokens ?? new string[0];

			foreach (string authorityToken in authorityTokens)
			{
				if (Thread.CurrentPrincipal.IsInRole(authorityToken))
					return true;
			}

			return false;
		}
	}
}
