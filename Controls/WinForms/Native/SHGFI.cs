#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

// ReSharper disable InconsistentNaming

using System;

namespace ClearCanvas.Controls.WinForms.Native
{
	[Flags]
	internal enum SHGFI
	{
		SHGFI_ICON = 0x000000100,
		SHGFI_DISPLAYNAME = 0x000000200,
		SHGFI_TYPENAME = 0x000000400,
		SHGFI_ATTRIBUTES = 0x000000800,
		SHGFI_ICONLOCATION = 0x000001000,
		SHGFI_EXETYPE = 0x000002000,
		SHGFI_SYSICONINDEX = 0x000004000,
		SHGFI_LINKOVERLAY = 0x000008000,
		SHGFI_SELECTED = 0x000010000,
		SHGFI_ATTR_SPECIFIED = 0x000020000,
		SHGFI_LARGEICON = 0x000000000,
		SHGFI_SMALLICON = 0x000000001,
		SHGFI_OPENICON = 0x000000002,
		SHGFI_SHELLICONSIZE = 0x000000004,
		SHGFI_PIDL = 0x000000008,
		SHGFI_USEFILEATTRIBUTES = 0x000000010,
		SHGFI_ADDOVERLAYS = 0x000000020,
		SHGFI_OVERLAYINDEX = 0x000000040
	}
}

// ReSharper restore InconsistentNaming