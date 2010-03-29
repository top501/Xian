﻿#region License

// Copyright (c) 2010, ClearCanvas Inc.
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

using ClearCanvas.Desktop;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common.ReportingWorkflow;
using ClearCanvas.Ris.Client.Formatting;

namespace ClearCanvas.Ris.Client.Workflow
{
	public class TranscriptionDocument : Document
	{
		private readonly ReportingWorklistItemSummary _worklistItem;
		private readonly string _folderName;
		private readonly EntityRef _worklistRef;
		private readonly string _worklistClassName;
		private TranscriptionComponent _component;

		public TranscriptionDocument(ReportingWorklistItemSummary worklistItem, IReportingWorkflowItemToolContext context)
			: base(worklistItem.ProcedureStepRef, context.DesktopWindow)
		{
			_worklistItem = worklistItem;
			_folderName = context.SelectedFolder.Name;

			if(context.SelectedFolder is TranscriptionWorkflowFolder)
			{
				_worklistRef = ((TranscriptionWorkflowFolder)context.SelectedFolder).WorklistRef;
				_worklistClassName = ((TranscriptionWorkflowFolder)context.SelectedFolder).WorklistClassName;
			}
			else
			{
				_worklistRef = null;
				_worklistClassName = null;
			}
		}

		public override string GetTitle()
		{
			return TranscriptionDocument.GetTitle(_worklistItem);
		}

		public override bool SaveAndClose()
		{
			_component.SaveReport(true);
			return base.Close();
		}

		public override IApplicationComponent GetComponent()
		{
			_component = new TranscriptionComponent(_worklistItem, _folderName, _worklistRef, _worklistClassName);
			return _component;
		}

		public static string GetTitle(ReportingWorklistItemSummary item)
		{
			return string.Format("Transcription - {0} - {1}", PersonNameFormat.Format(item.PatientName), MrnFormat.Format(item.Mrn));
		}

		public static string StripTitle(string title)
		{
			return title.Replace("Transcription - ", "");
		}
	}
}