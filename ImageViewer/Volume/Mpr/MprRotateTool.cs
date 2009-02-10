#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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
using System.Drawing;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.ImageViewer.InputManagement;
using ClearCanvas.ImageViewer.BaseTools;
using ClearCanvas.ImageViewer.StudyManagement;
using NullFX.Win32;

namespace ClearCanvas.ImageViewer.Volume.Mpr
{
	[MouseToolButton(XMouseButtons.Right, false)]
	// Note: This combined with GetKeyState hack allowed me to have shift key alter this tool (rotates about Z)
	[DefaultMouseToolButton(XMouseButtons.Right, ModifierFlags.Shift)]
	[MenuAction("activate", "imageviewer-contextmenu/Rotate MPR", "Select", Flags = ClickActionFlags.CheckAction)]
	[ButtonAction("activate", "global-toolbars/ToolbarsMpr/RotateMpr", "Select", Flags = ClickActionFlags.CheckAction)]
	[CheckedStateObserver("activate", "Active", "ActivationChanged")]
	[VisibleStateObserver("activate", "ToolVisible", "ToolVisibleChanged")]
	[Tooltip("activate", "Rotate MPR")]
	[IconSet("activate", IconScheme.Colour, "Icons.RotateMprToolSmall.png", "Icons.RotateMprToolLarge.png",
		"Icons.RotateMprToolLarge.png")]
	[GroupHint("activate", "Tools.Mpr.Manipulation.Rotate")]
	// Note: I don't expect this to be a real tool in the long run, this was just an easy way to enable
	//	and test oblique slicing
	[ExtensionOf(typeof (ImageViewerToolExtensionPoint))]
	public class RotateMprTool : MouseImageViewerTool
	{
		public RotateMprTool()
		{
			this.CursorToken = new CursorToken("Icons.RotateMprToolSmall.png", this.GetType().Assembly);
		}

		#region Visibility Control

		public event EventHandler ToolVisibleChanged;

		private bool _toolVisible;

		public bool ToolVisible
		{
			get { return _toolVisible; }
			private set
			{
				if (_toolVisible != value)
				{
					_toolVisible = value;
					EventsHelper.Fire(ToolVisibleChanged, this, new EventArgs());
				}

				_toolVisible = value;
			}
		}

		// Tool is only visible when PresImage has a VolumeSliceSopDataSource (i.e. is an MPR DisplaySet)
		private void UpdateToolVisible()
		{
			bool visible = false;

			IImageSopProvider sopProvider = Context.Viewer.SelectedPresentationImage as IImageSopProvider;
			if (sopProvider != null)
				visible = sopProvider.ImageSop.DataSource is VolumeSliceSopDataSource;

			ToolVisible = visible;
		}

		protected override void OnPresentationImageSelected(object sender, PresentationImageSelectedEventArgs e)
		{
			UpdateToolVisible();

			base.OnPresentationImageSelected(sender, e);
		}

		#endregion

		#region IMouseButtonHandler Members

		private Point _mouseDownLocation;
		private bool _lockedIn;
		private bool _lockedX; // otherwise Y
		private static int _startRotateX;
		private static int _startRotateY;
		private static int _startRotateZ;

		public override bool Start(IMouseInformation mouseInformation)
		{
			base.Start(mouseInformation);

			_mouseDownLocation = mouseInformation.Location;
			_lockedIn = false;

			MprLayoutManager layout = GetMprLayoutManager();
			if (layout == null)
				return true;
			_startRotateX = layout.GetObliqueImageRotationX(Context.Viewer.SelectedPresentationImage);
			_startRotateY = layout.GetObliqueImageRotationY(Context.Viewer.SelectedPresentationImage);
			_startRotateZ = layout.GetObliqueImageRotationZ(Context.Viewer.SelectedPresentationImage);

			return true;
		}

		public override bool Track(IMouseInformation mouseInformation)
		{
			base.Track(mouseInformation);

			MprLayoutManager layout = GetMprLayoutManager();
			if (layout == null)
				return true;

			Point Delta = new Point(mouseInformation.Location.X - _mouseDownLocation.X,
			                        mouseInformation.Location.Y - _mouseDownLocation.Y);

			// This little hack let me do more with my tool, when shift is down I'll rotate around Z
			KeyStateInfo shiftKeystate = KeyboardInfo.GetKeyState(System.Windows.Forms.Keys.ShiftKey);
			//Debug.WriteLine("KeyState: " + shiftKeystate.IsPressed);
			if (shiftKeystate.IsPressed)
			{
				int currentRotateZ = Math.Max(Math.Min(_startRotateZ - Delta.Y / 4, 90), -90);
				layout.RotateObliqueImage(Context.Viewer.SelectedPresentationImage, _startRotateX, _startRotateY, currentRotateZ);
			}
			else
			{
#if true // "locked in" behavior
				if (!_lockedIn)
				{
					if (Math.Abs(Delta.X) > 8 || Math.Abs(Delta.Y) > 8)
					{
						_lockedIn = true;
						_lockedX = Math.Abs(Delta.X) > Math.Abs(DeltaY);
					}
				}
				else
				{
					if (_lockedX)
					{
						int currentRotateX = Math.Max(Math.Min(_startRotateX + Delta.X / 4, 90), -90);
						layout.RotateObliqueImage(Context.Viewer.SelectedPresentationImage, currentRotateX, _startRotateY, _startRotateZ);
					}
					else
					{
						int currentRotateY = Math.Max(Math.Min(_startRotateY + Delta.Y / 4, 90), -90);
						layout.RotateObliqueImage(Context.Viewer.SelectedPresentationImage, _startRotateX, currentRotateY, _startRotateZ);
					}
				}
#else
			int currentRotateX = Math.Max(Math.Min(_startRotateX + Delta.X / 4, 90), -90);
			int currentRotateY = Math.Max(Math.Min(_startRotateY + Delta.Y / 4, 90), -90);
			layout.RotatePresentationImage(Context.Viewer.SelectedPresentationImage, currentRotateX, currentRotateY, _startRotateZ);
#endif
			}
			return true;
		}

		private MprLayoutManager GetMprLayoutManager()
		{
			IPresentationImage selectedImage = this.Context.Viewer.SelectedPresentationImage;

			DicomGrayscalePresentationImage presImage = selectedImage as DicomGrayscalePresentationImage;
			if (presImage == null) return null;
			MprImageViewerComponent ivc = presImage.ImageViewer as MprImageViewerComponent;
			if (ivc == null) return null;
			MprLayoutManager layout = ivc.LayoutManager as MprLayoutManager;
			return layout;
		}

		public override bool Stop(IMouseInformation mouseInformation)
		{
			base.Stop(mouseInformation);

			return false;
		}

		public override void Cancel()
		{
		}

		#endregion
	}
}

/******************************************************/
/*          NULLFX FREE SOFTWARE LICENSE              */
/******************************************************/
/*  GetKeyState Utility                               */
/*  by: Steve Whitley                                 */
/*  � 2005 NullFX Software                            */
/*                                                    */
/* NULLFX SOFTWARE DISCLAIMS ALL WARRANTIES,          */
/* RESPONSIBILITIES, AND LIABILITIES ASSOCIATED WITH  */
/* USE OF THIS CODE IN ANY WAY, SHAPE, OR FORM        */
/* REGARDLESS HOW IMPLICIT, EXPLICIT, OR OBSCURE IT   */
/* IS. IF THERE IS ANYTHING QUESTIONABLE WITH REGARDS */
/* TO THIS SOFTWARE BREAKING AND YOU GAIN A LOSS OF   */
/* ANY NATURE, WE ARE NOT THE RESPONSIBLE PARTY. USE  */
/* OF THIS SOFTWARE CREATES ACCEPTANCE OF THESE TERMS */
/*                                                    */
/* USE OF THIS CODE MUST RETAIN ALL COPYRIGHT NOTICES */
/* AND LICENSES (MEANING THIS TEXT).                  */
/*                                                    */
/******************************************************/

namespace NullFX.Win32
{
	using System.Windows.Forms;
	using System.Runtime.InteropServices;

	public class KeyboardInfo
	{
		private KeyboardInfo()
		{
		}

		[DllImport("user32")]
		private static extern short GetKeyState(int vKey);

		public static KeyStateInfo GetKeyState(Keys key)
		{
			short keyState = GetKeyState((int) key);
			int low = Low(keyState),
			    high = High(keyState);
			bool toggled = low == 1 ? true : false,
			     pressed = high == 1;
			return new KeyStateInfo(key, pressed, toggled);
		}

		private static int High(int keyState)
		{
			return keyState > 0
			       	? keyState >> 0x10
			       	: (keyState >> 0x10) & 0x1;
		}

		private static int Low(int keyState)
		{
			return keyState & 0xffff;
		}
	}

	public struct KeyStateInfo
	{
		private Keys _key;

		private bool _isPressed,
		             _isToggled;

		public KeyStateInfo(Keys key,
		                    bool ispressed,
		                    bool istoggled)
		{
			_key = key;
			_isPressed = ispressed;
			_isToggled = istoggled;
		}

		public static KeyStateInfo Default
		{
			get
			{
				return new KeyStateInfo(Keys.None,
				                        false,
				                        false);
			}
		}

		public Keys Key
		{
			get { return _key; }
		}

		public bool IsPressed
		{
			get { return _isPressed; }
		}

		public bool IsToggled
		{
			get { return _isToggled; }
		}
	}
}