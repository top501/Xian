#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.Common;
using ClearCanvas.Desktop.Configuration;

namespace ClearCanvas.Desktop.View.WinForms.Configuration
{
	/// <summary>
	/// Provides a Windows Forms view onto <see cref="SettingEditorComponent"/>
	/// </summary>
	[ExtensionOf(typeof(SettingEditorComponentViewExtensionPoint))]
	public class SettingEditorComponentView : WinFormsView, IApplicationComponentView
	{
		private SettingEditorComponent _component;
		private SettingEditorComponentControl _control;


		#region IApplicationComponentView Members

		public void SetComponent(IApplicationComponent component)
		{
			_component = (SettingEditorComponent)component;
		}

		#endregion

		public override object GuiElement
		{
			get
			{
				if (_control == null)
				{
					_control = new SettingEditorComponentControl(_component);
				}
				return _control;
			}
		}
	}
}