#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Configuration;

namespace ClearCanvas.Enterprise.Core.Printing
{

	[SettingsGroupDescription("Configures the behaviour of the printing functionality.")]
	[SettingsProvider(typeof(ClearCanvas.Common.Configuration.StandardSettingsProvider))]
	internal sealed partial class PrintSettings
	{
		public PrintSettings()
		{
		}
	}
}
