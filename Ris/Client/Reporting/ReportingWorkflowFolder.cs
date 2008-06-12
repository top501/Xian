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
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.ReportingWorkflow;

namespace ClearCanvas.Ris.Client.Reporting
{
	public interface IReportingWorkflowFolderDropContext : IDropContext
	{
		/// <summary>
		/// Gets the enablement of the specified operation from the folder system
		/// </summary>
		/// <param name="operationName"></param>
		/// <returns></returns>
		bool GetOperationEnablement(string operationName);

		/// <summary>
		/// Gets the folder that is the drop target of the current operation
		/// </summary>
		ReportingWorkflowFolder DropTargetFolder { get; }

		/// <summary>
		/// Gets the folder system that owns the drop target folder
		/// </summary>
		WorkflowFolderSystem FolderSystem { get; }
	}

	public abstract class ReportingWorkflowFolder : WorkflowFolder<ReportingWorklistItem>
	{
		class DropContext : IReportingWorkflowFolderDropContext
		{
			private ReportingWorkflowFolder _folder;

			public DropContext(ReportingWorkflowFolder folder)
			{
				_folder = folder;
			}

			#region IReportingWorkflowFolderDropContext Members

			public bool GetOperationEnablement(string operationName)
			{
				return _folder._folderSystem.GetOperationEnablement(operationName);
			}

			public ReportingWorkflowFolder DropTargetFolder
			{
				get { return _folder; }
			}

			public WorkflowFolderSystem FolderSystem
			{
				get
				{
					return _folder.WorkflowFolderSystem;
				}
			}

			#endregion

			#region IDropContext Members

			public IDesktopWindow DesktopWindow
			{
				get { return _folder._folderSystem.DesktopWindow; }
			}

			#endregion
		}

		private readonly WorkflowFolderSystem _folderSystem;
		private readonly EntityRef _worklistRef;

		public ReportingWorkflowFolder(WorkflowFolderSystem folderSystem, string folderName, string folderDescription, EntityRef worklistRef, ExtensionPoint<IDropHandler<ReportingWorklistItem>> dropHandlerExtensionPoint)
			: base(folderSystem, folderName, folderDescription, new ReportingWorklistTable())
		{
			_folderSystem = folderSystem;

			if (dropHandlerExtensionPoint != null)
			{
				this.InitDragDropHandling(dropHandlerExtensionPoint, new DropContext(this));
			}

			_worklistRef = worklistRef;
		}

		public ReportingWorkflowFolder(WorkflowFolderSystem folderSystem, string folderName, ExtensionPoint<IDropHandler<ReportingWorklistItem>> dropHandlerExtensionPoint)
			: this(folderSystem, folderName, null, null, dropHandlerExtensionPoint)
		{
		}

		public ReportingWorkflowFolder(WorkflowFolderSystem folderSystem, string folderName, string folderDescription, EntityRef worklistRef)
			: this(folderSystem, folderName, folderDescription, worklistRef, null)
		{
		}

		public ReportingWorkflowFolder(WorkflowFolderSystem folderSystem, string folderName)
			: this(folderSystem, folderName, null, null, null)
		{
		}

		public EntityRef WorklistRef
		{
			get { return _worklistRef; }
		}

		protected override bool CanQuery()
		{
			return true;
		}

		protected override QueryItemsResult QueryItems()
		{
			QueryItemsResult result = null;
			Platform.GetService<IReportingWorkflowService>(
				delegate(IReportingWorkflowService service)
				{
					QueryWorklistRequest request = _worklistRef == null
						? new QueryWorklistRequest(this.WorklistClassName, true, true)
						: new QueryWorklistRequest(_worklistRef, true, true);

					QueryWorklistResponse<ReportingWorklistItem> response = service.QueryWorklist(request);
					result = new QueryItemsResult(response.WorklistItems, response.ItemCount);
				});

			return result;
		}

		protected override int QueryCount()
		{
			int count = -1;
			Platform.GetService<IReportingWorkflowService>(
				delegate(IReportingWorkflowService service)
				{
					QueryWorklistRequest request = _worklistRef == null
						? new QueryWorklistRequest(this.WorklistClassName, false, true)
						: new QueryWorklistRequest(_worklistRef, false, true);

					QueryWorklistResponse<ReportingWorklistItem> response = service.QueryWorklist(request);
					count = response.ItemCount;
				});

			return count;
		}

		public bool GetOperationEnablement(string operationName)
		{
			return _folderSystem.GetOperationEnablement(operationName);
		}
	}
}
