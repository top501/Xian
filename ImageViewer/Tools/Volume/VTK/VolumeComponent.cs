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

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.BaseTools;

namespace ClearCanvas.ImageViewer.Tools.Volume.VTK
{
	/// <summary>
	/// Extension point for views onto <see cref="VolumeComponent"/>
	/// </summary>
	[ExtensionPoint]
	public class VolumeComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	/// <summary>
	/// VolumeComponent class
	/// </summary>
	[AssociateView(typeof(VolumeComponentViewExtensionPoint))]
	public class VolumeComponent : ImageViewerToolComponent
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public VolumeComponent(IDesktopWindow desktopWindow)
			: base(desktopWindow)
		{
		}

		public bool CreateVolumeEnabled
		{
			get 
			{
				if (this.ImageViewer == null)
				{
					return false;
				}
				else
				{
					if (this.ImageViewer.SelectedTile == null)
						return false;
					else
						return !(this.ImageViewer.SelectedPresentationImage is VolumePresentationImage);
				}
			}
		}

		public bool VolumeSettingsEnabled
		{
			get
			{
				if (this.ImageViewer == null)
				{
					return false;
				}
				else
				{
					if (this.ImageViewer.SelectedTile == null)
						return false;
					else
						return this.ImageViewer.SelectedPresentationImage is VolumePresentationImage;
				}
			}
		}

		public GraphicCollection VolumeGraphics
		{
			get
			{
				if (this.ImageViewer == null)
					return null;

				if (this.ImageViewer.SelectedPresentationImage == null)
					return null;

				IAssociatedTissues volume = this.ImageViewer.SelectedPresentationImage as IAssociatedTissues;

				if (volume == null)
					return null;

				return volume.TissueLayers;
			}
		}

		#region IApplicationComponent methods

		public override void Start()
		{
			base.Start();
		}

		public override void Stop()
		{
			// TODO prepare the component to exit the live phase
			// This is a good place to do any clean up
			base.Stop();
		}

		#endregion

		public void CreateVolume()
		{
			if (this.ImageViewer == null)
				return;

			if (this.ImageViewer.SelectedImageBox == null)
				return;

			IDisplaySet selectedDisplaySet = this.ImageViewer.SelectedImageBox.DisplaySet;
			VolumePresentationImage image = new VolumePresentationImage(selectedDisplaySet);

			AddTissueLayers(image);

			IDisplaySet displaySet = new DisplaySet(String.Format("{0} (3D)", selectedDisplaySet.Name), String.Format("VTK.{0}", Guid.NewGuid().ToString()));
			displaySet.PresentationImages.Add(image);
			this.ImageViewer.LogicalWorkspace.ImageSets[0].DisplaySets.Add(displaySet);

			IImageBox imageBox = this.ImageViewer.SelectedImageBox;
			imageBox.DisplaySet = displaySet;
			imageBox.Draw();
			imageBox[0, 0].Select();

			NotifyAllPropertiesChanged();
		}

		protected override void OnActiveImageViewerChanged(ActiveImageViewerChangedEventArgs e)
		{
			if (e.DeactivatedImageViewer != null)
				e.DeactivatedImageViewer.EventBroker.DisplaySetSelected -= OnDisplaySetSelected;
			
			if (e.ActivatedImageViewer != null)
				e.ActivatedImageViewer.EventBroker.DisplaySetSelected += OnDisplaySetSelected;

			NotifyAllPropertiesChanged();
		}

		private void OnDisplaySetSelected(object sender, DisplaySetSelectedEventArgs e)
		{
			NotifyPropertyChanged("CreateVolumeEnabled");
			NotifyPropertyChanged("VolumeSettingsEnabled");

			NotifyAllPropertiesChanged();
		}

		private void AddTissueLayers(VolumePresentationImage image)
		{
			GraphicCollection layers = image.TissueLayers;

			TissueSettings tissue = new TissueSettings();
			tissue.SelectPreset("Bone");
			layers.Add(new VolumeGraphic(tissue));

			tissue = new TissueSettings();
			tissue.SelectPreset("Blood");
			layers.Add(new VolumeGraphic(tissue));
		}

	}
}
