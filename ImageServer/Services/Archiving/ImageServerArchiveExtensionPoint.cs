#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.Common;

namespace ClearCanvas.ImageServer.Services.Archiving
{
	/// <summary>
	/// Extension point for plugins to the <see cref="IImageServerArchive"/> interface.
	/// </summary>
	[ExtensionPoint()]
	public class ImageServerArchiveExtensionPoint: ExtensionPoint<IImageServerArchivePlugin>
	{
	}
}
