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
using System.Collections.Generic;
using System.Diagnostics;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Dicom;
using ClearCanvas.ImageViewer.BaseTools;
using ClearCanvas.ImageViewer.Mathematics;
using ClearCanvas.ImageViewer.StudyManagement;

namespace ClearCanvas.ImageViewer.Tools.Synchronization
{
	[MenuAction("activate", "global-menus/MenuTools/MenuStandard/MenuAutoStackSynchronization", "Toggle", Flags = ClickActionFlags.CheckAction)]
	[ButtonAction("activate", "global-toolbars/ToolbarStandard/ToolbarAutoStackSynchronization", "Toggle", Flags = ClickActionFlags.CheckAction)]
	[CheckedStateObserver("activate", "Active", "ActiveChanged")]
	[Tooltip("activate", "TooltipAutoStackSynchronization")]
	[IconSet("activate", IconScheme.Colour, "Icons.AutoStackSynchronizationToolSmall.png", "Icons.AutoStackSynchronizationToolMedium.png", "Icons.AutoStackSynchronizationToolLarge.png")]
	[GroupHint("activate", "Tools.Image.Manipulation.Stacking.SynchronizeAuto")]

	[ExtensionOf(typeof(ImageViewerToolExtensionPoint))]
	public class StackingSynchronizationTool : ImageViewerTool
	{
		#region ImageInfo struct 

		private class ImageInfo
		{
			public Vector3D Normal;
			public Vector3D Position;
		}

		#endregion

		private bool _active;
		private event EventHandler _activeChanged;

		private SynchronizationToolCoordinator _coordinator;

		private readonly Dictionary<string, ImageInfo> _sopInfoDictionary;

		private static readonly float _fiveDegreesInRadians = (float)(5 * Math.PI / 180);

		public StackingSynchronizationTool()
		{
			_active = false;
			_sopInfoDictionary = new Dictionary<string, ImageInfo>();
		}

		public bool Active
		{
			get { return _active; }
			set
			{
				if (_active == value)
					return;

				_active = value;
				EventsHelper.Fire(_activeChanged, this, EventArgs.Empty);
			}
		}

		public event EventHandler ActiveChanged
		{
			add { _activeChanged += value; }
			remove { _activeChanged -= value; }
		}

		public override void Initialize()
		{
			base.Initialize();

			_coordinator = SynchronizationToolCoordinator.Get(base.ImageViewer);
			_coordinator.StackingSynchronizationTool = this;
		}

		protected override void Dispose(bool disposing)
		{
			_coordinator.Release();

			base.Dispose(disposing);
		}

		private void Toggle()
		{
			Active = !Active;
			_coordinator.OnSynchronizedImages(Synchronize());
		}

		private IEnumerable<IImageBox> GetImageBoxesToSynchronize(IImageBox referenceImageBox)
		{
			IImageSopProvider provider = referenceImageBox.TopLeftPresentationImage as IImageSopProvider;
			if (provider == null || String.IsNullOrEmpty(provider.ImageSop.FrameOfReferenceUid) || String.IsNullOrEmpty(provider.ImageSop.StudyInstanceUID))
				yield break;

			foreach (IImageBox imageBox in this.Context.Viewer.PhysicalWorkspace.ImageBoxes)
			{
				if (referenceImageBox != imageBox && imageBox != null && imageBox.DisplaySet != null && imageBox.DisplaySet.PresentationImages.Count > 1)
					yield return imageBox;
			}
		}

		private ImageInfo GetImageInformation(ImageSop sop)
		{
			ImageInfo info;

			//Caching as much of the floating point arithmetic as we can for each image
			//improves the efficiency of finding the closest slice by about 4x.
			if (!_sopInfoDictionary.ContainsKey(sop.SopInstanceUID))
			{
				// Calculation of position of the center of the image in patient coordinates 
				// using the matrix method described in Dicom PS 3.3 C.7.6.2.1.1.
				info = new ImageInfo();
				info.Position = ImagePositionHelper.SourceToPatientCenterOfImage(sop);
				info.Normal = ImagePositionHelper.CalculateNormalVector(sop);

				if (info.Position == null || info.Normal == null)
					return null;

				_sopInfoDictionary[sop.SopInstanceUID] = info;
			}
			else
			{
				info = _sopInfoDictionary[sop.SopInstanceUID];
			}

			return info;
		}

		private int CalculateClosestSlice(IImageBox referenceImageBox, IImageBox imageBox)
		{
			ImageSop referenceSop = ((IImageSopProvider) referenceImageBox.TopLeftPresentationImage).ImageSop;

			ImageInfo referenceImageInfo = GetImageInformation(referenceSop);
			if (referenceImageInfo == null)
				return -1;

			float closestDistance = float.MaxValue;
			int closestIndex = -1;

			//find the closest one, closest to the top of the stack (beginning of display set).
			for (int index = imageBox.DisplaySet.PresentationImages.Count - 1; index >= 0 ; --index)
			{
				IImageSopProvider provider = imageBox.DisplaySet.PresentationImages[index] as IImageSopProvider;
				if (provider != null)
				{
					ImageSop sop = provider.ImageSop;

					if (sop.FrameOfReferenceUid == referenceSop.FrameOfReferenceUid && sop.StudyInstanceUID == referenceSop.StudyInstanceUID)
					{
						ImageInfo info = GetImageInformation(sop);
						if (info != null)
						{
							float angle = Math.Abs((float)Math.Acos(info.Normal.Dot(referenceImageInfo.Normal)));
							if (angle <= _fiveDegreesInRadians || (Math.PI - angle) <= _fiveDegreesInRadians)
							{
								Vector3D position = info.Position - referenceImageInfo.Position;
								float distance = position.Magnitude;

								if (Math.Abs(distance) <= closestDistance)
								{
									closestDistance = distance;
									closestIndex = index;
								}
							}
						}
					}
				}
			}

			return closestIndex;
		}

		private IEnumerable<IImageBox> SynchronizeWithImage(IImageBox referenceImageBox)
		{
			foreach (IImageBox imageBox in GetImageBoxesToSynchronize(referenceImageBox))
			{
				int index = CalculateClosestSlice(referenceImageBox, imageBox);
				if (index >= 0 && index != imageBox.TopLeftPresentationImageIndex)
				{
					imageBox.TopLeftPresentationImageIndex = index;
					yield return imageBox;
				}
			}
		}

		public IEnumerable<IImageBox> Synchronize()
		{
			IImageBox referenceImageBox = this.Context.Viewer.SelectedImageBox;
			if (Active && referenceImageBox != null)
			{
				foreach (IImageBox imageBox in SynchronizeWithImage(referenceImageBox))
					yield return imageBox;
			}
		}
	}
}
