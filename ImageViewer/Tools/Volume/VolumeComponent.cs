using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.ImageViewer.Tools.Volume
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
	public class VolumeComponent : ApplicationComponent
	{
		private TissueSettingsCollection _tissueSettingsCollection;

		/// <summary>
		/// Constructor
		/// </summary>
		public VolumeComponent()
		{
			this.TissueSettingsCollection.ItemAdded += new EventHandler<TissueSettingsEventArgs>(OnTissueSettingsAdded);
		}

		public TissueSettingsCollection TissueSettingsCollection
		{
			get 
			{
				if (_tissueSettingsCollection == null)
					_tissueSettingsCollection = new TissueSettingsCollection();

				return _tissueSettingsCollection; 
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


		public void SelectTissue(int tissueSettingsIndex)
		{
		}

		public void CreateVolume()
		{
		}

		private void OnTissueSettingsAdded(object sender, TissueSettingsEventArgs e)
		{
		}
	}
}
