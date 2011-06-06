﻿#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca

// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.Common;

namespace ClearCanvas.ImageViewer.Layout
{
	/// <summary>
	/// Defines an extension point for match-criteria contributors.
	/// </summary>
	[ExtensionPoint]
	class HpMatchCriteriaContributorExtensionPoint : ExtensionPoint<IHpMatchCriteriaContributor>
	{
	}

	/// <summary>
	/// Defines an extension point for layout definition contributors.
	/// </summary>
	[ExtensionPoint]
	class HpLayoutDefinitionContributorExtensionPoint : ExtensionPoint<IHpLayoutDefinitionContributor>
	{
	}

	/// <summary>
	/// Defines an extension point for imagebox definition contributors.
	/// </summary>
	[ExtensionPoint]
	class HpImageBoxDefinitionContributorExtensionPoint : ExtensionPoint<IHpImageBoxDefinitionContributor>
	{
	}
}
