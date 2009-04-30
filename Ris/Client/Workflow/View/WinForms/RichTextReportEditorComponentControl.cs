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

using System.ComponentModel;
using System.Windows.Forms;
using ClearCanvas.Desktop.View.WinForms;
using ClearCanvas.Ris.Client.View.WinForms;

namespace ClearCanvas.Ris.Client.Workflow.View.WinForms
{
	/// <summary>
	/// Provides a Windows Forms user-interface for <see cref="TranscriptionEditorComponent"/>.
	/// </summary>
	public partial class RichTextReportEditorComponentControl : ApplicationComponentUserControl
	{
		private readonly IReportEditorComponent _component;
		private readonly CannedTextSupport _cannedTextSupport;

		/// <summary>
		/// Constructor.
		/// </summary>
		public RichTextReportEditorComponentControl(IReportEditorComponent component)
			: base(component)
		{
			_component = component;
			InitializeComponent();

			_richText.DataBindings.Add("Text", _component, "EditorText", true, DataSourceUpdateMode.OnPropertyChanged);
			_cannedTextSupport = new CannedTextSupport(_richText, _component.CannedTextLookupHandler);

			if (_component.PreviewVisible)
			{
				Control reportPreview = (Control)_component.ReportPreviewHost.ComponentView.GuiElement;
				reportPreview.Dock = DockStyle.Fill;
				_splitContainer.Panel1.Controls.Add(reportPreview);
				_splitContainer.Panel1Collapsed = false;
			}
			else
			{
				_splitContainer.Panel1Collapsed = true;
			}

			((INotifyPropertyChanged)_component).PropertyChanged += _component_PropertyChanged;
		}

		void _component_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "EditorText")
			{
				_richText.Text = _component.EditorText;
			}
		}
	}
}
