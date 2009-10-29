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
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.ImageViewer.Utilities.StudyFilters.BaseTools
{
	[ButtonAction("launch", DefaultToolbarActionSite + "/ToolbarLaunchInViewer", "Launch")]
	[IconSet("launch", IconScheme.Colour, "OpenToolSmall.png", "OpenToolSmall.png", "OpenToolSmall.png")]
	[ExtensionOf(typeof (StudyFilterToolExtensionPoint))]
	public class LaunchViewerTool : StudyFilterTool
	{
		public void Launch()
		{
			if (base.Selection == null || base.Selection.Count == 0)
				return;

			int n = 0;
			string[] selection = new string[base.Selection.Count];
			foreach (StudyItem item in base.Selection)
			{
				selection[n++] = item.File.FullName;
			}

			bool cancelled = true;
			ImageViewerComponent viewer = new ImageViewerComponent();
			try
			{
				viewer.LoadImages(selection, base.Context.DesktopWindow, out cancelled);
			}
			catch (Exception ex)
			{
				base.DesktopWindow.ShowMessageBox(ex.Message, MessageBoxActions.Ok);
			}

			if (cancelled)
			{
				viewer.Dispose();
				return;
			}

			try
			{
				LaunchImageViewerArgs launchArgs = new LaunchImageViewerArgs(WindowBehaviour.Auto);
				ImageViewerComponent.Launch(viewer, launchArgs);
			}
			catch (Exception ex)
			{
				base.DesktopWindow.ShowMessageBox(ex.Message, MessageBoxActions.Ok);
				Platform.Log(LogLevel.Error, ex, "ImageViewerComponent launch failure.");
			}
		}
	}
}