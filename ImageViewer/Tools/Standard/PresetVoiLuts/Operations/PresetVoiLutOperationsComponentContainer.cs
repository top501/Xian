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
using ClearCanvas.Common;
using ClearCanvas.Desktop;

namespace ClearCanvas.ImageViewer.Tools.Standard.PresetVoiLuts.Operations
{
	[ExtensionPoint]
	public sealed class PresetVoiLutOperationComponentContainerViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	[AssociateView(typeof(PresetVoiLutOperationComponentContainerViewExtensionPoint))]
	public sealed class PresetVoiLutOperationsComponentContainer : ApplicationComponentContainer
	{
        private sealed class PresetVoiLutOperationComponentHost : ApplicationComponentContainer.ContainedComponentHost
		{
			internal PresetVoiLutOperationComponentHost(PresetVoiLutOperationsComponentContainer owner, 
                IPresetVoiLutOperationComponent hostedComponent)
				:base(owner, hostedComponent)
			{
			}

			public new IPresetVoiLutOperationComponent Component
			{
				get { return (IPresetVoiLutOperationComponent)base.Component; }
			}
		}

		private readonly PresetVoiLutOperationComponentHost _componentHost;
		private readonly List<KeyStrokeDescriptor> _availableKeyStrokes;
		private KeyStrokeDescriptor _selectedKeyStroke;

		public PresetVoiLutOperationsComponentContainer(IEnumerable<XKeys> availableKeyStrokes, IPresetVoiLutOperationComponent component)
		{
			Platform.CheckForNullReference(availableKeyStrokes, "availableKeyStrokes");
			Platform.CheckForNullReference(component, "component");

			_availableKeyStrokes = new List<KeyStrokeDescriptor>();
			foreach (XKeys keyStroke in availableKeyStrokes)
				_availableKeyStrokes.Add(keyStroke);

			Platform.CheckPositive(_availableKeyStrokes.Count, "_availableKeyStrokes.Count");
			_selectedKeyStroke = _availableKeyStrokes[0];

			_componentHost = new PresetVoiLutOperationComponentHost(this, component);
		}

		private IPresetVoiLutOperationComponent ContainedComponent
		{
			get { return ((PresetVoiLutOperationComponentHost) ComponentHost).Component; }
		}

		public ApplicationComponentHost ComponentHost
		{
			get { return _componentHost; }
		}

		public IEnumerable<KeyStrokeDescriptor> AvailableKeyStrokes
		{
			get { return _availableKeyStrokes; }
		}

		public KeyStrokeDescriptor SelectedKeyStroke
		{
			get { return _selectedKeyStroke;  }
			set 
			{ 
				if (!_availableKeyStrokes.Contains(value))
					throw new ArgumentException(SR.ExceptionSelectedKeystrokeMustBeOneOfAvailable);

				if (_selectedKeyStroke.Equals(value))
					return;
				
				_selectedKeyStroke = value;
				Modified = true;
				NotifyPropertyChanged("SelectedKeyStroke");
			}
		}

		public bool AcceptEnabled
		{
			get
			{
				if (!ContainedComponent.Valid)
					return false;

				switch (ContainedComponent.EditContext)
				{
					case EditContext.Edit:
						return this.Modified;
					default:
						return true;
				}
			}
		}

		public override bool Modified
		{
			get
			{
				return base.Modified || ComponentHost.Component.Modified;
			}
			protected set
			{
				base.Modified = value;
				NotifyPropertyChanged("AcceptEnabled");
			}
		}

		internal PresetVoiLut GetPresetVoiLut()
		{
			PresetVoiLut preset = new PresetVoiLut(ContainedComponent.GetOperation());
			preset.KeyStroke = _selectedKeyStroke.KeyStroke;
			return preset;
		}
		
		public void OK()
		{
			this.Exit(ApplicationComponentExitCode.Accepted);
		}

		public void Cancel()
		{
			this.Exit(ApplicationComponentExitCode.None);
		}

		public override void Start()
		{
			base.Modified = false;
			base.Start();
			ContainedComponent.PropertyChanged += ComponentPropertyChanged;
			ComponentHost.StartComponent();

			this.ExitCode = ApplicationComponentExitCode.None;
		}

		public override void Stop()
		{
			ComponentHost.StopComponent();
			ContainedComponent.PropertyChanged -= ComponentPropertyChanged;
			base.Stop();
		}

		#region ApplicationComponentContainer overrides

		public override IEnumerable<IApplicationComponent> ContainedComponents
		{
			get { yield return ComponentHost.Component; }
		}

		public override IEnumerable<IApplicationComponent> VisibleComponents
		{
			get { return this.ContainedComponents; }
		}

		public override void EnsureVisible(IApplicationComponent component)
		{
			if (!this.IsStarted)
				throw new InvalidOperationException("The container was never started.");

			// nothing to do, since the hosted components are started by default
		}

		public override void EnsureStarted(IApplicationComponent component)
		{
			if (!this.IsStarted)
				throw new InvalidOperationException("The container was never started.");

			// nothing to do, since the hosted components are visible by default
		}

		#endregion

		private void ComponentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this.NotifyPropertyChanged("AcceptEnabled");
		}
	}
}
