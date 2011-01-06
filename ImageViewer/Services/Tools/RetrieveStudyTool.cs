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
using System.Diagnostics;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Dicom.Utilities;
using ClearCanvas.ImageViewer.Common;
using ClearCanvas.ImageViewer.Services.Auditing;
using ClearCanvas.ImageViewer.Services.ServerTree;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.ImageViewer.Services.DicomServer;
using ClearCanvas.ImageViewer.Explorer.Dicom;
using System.ServiceModel;
using ClearCanvas.ImageViewer.Services.LocalDataStore;

namespace ClearCanvas.ImageViewer.Services.Tools
{
	[ButtonAction("activate", "dicomstudybrowser-toolbar/ToolbarRetrieveStudy", "RetrieveStudy")]
	[MenuAction("activate", "dicomstudybrowser-contextmenu/MenuRetrieveStudy", "RetrieveStudy")]
	[EnabledStateObserver("activate", "Enabled", "EnabledChanged")]
	[Tooltip("activate", "TooltipRetrieveStudy")]
	[IconSet("activate", IconScheme.Colour, "Icons.RetrieveStudyToolSmall.png", "Icons.RetrieveStudyToolSmall.png", "Icons.RetrieveStudyToolSmall.png")]

	[ViewerActionPermission("activate", ImageViewer.Services.AuthorityTokens.Study.Retrieve)]
	
	[ExtensionOf(typeof(StudyBrowserToolExtensionPoint))]
	public class RetrieveStudyTool : StudyBrowserTool
	{
		public RetrieveStudyTool()
		{

		}

		public override void Initialize()
		{
			base.Initialize();

			SetDoubleClickHandler();
		}

		private void RetrieveStudy()
		{
			if (!Enabled || Context.SelectedServerGroup.IsLocalDatastore || Context.SelectedStudy == null)
                return;

			EventResult result = EventResult.Success;

			Dictionary<ApplicationEntity, List<StudyInformation>> retrieveInformation = new Dictionary<ApplicationEntity, List<StudyInformation>>();
			foreach (StudyItem item in Context.SelectedStudies)
			{
				ApplicationEntity applicationEntity = item.Server as ApplicationEntity;
				if (applicationEntity != null && !retrieveInformation.ContainsKey(applicationEntity))
					retrieveInformation[applicationEntity] = new List<StudyInformation>();

				StudyInformation studyInformation = new StudyInformation();
				studyInformation.PatientId = item.PatientId;
				studyInformation.PatientsName = item.PatientsName;
				DateTime studyDate;
				DateParser.Parse(item.StudyDate, out studyDate);
				studyInformation.StudyDate = studyDate;
				studyInformation.StudyDescription = item.StudyDescription;
				studyInformation.StudyInstanceUid = item.StudyInstanceUid;

				retrieveInformation[applicationEntity].Add(studyInformation);
			}

			DicomServerServiceClient client = new DicomServerServiceClient();

			try
			{
				client.Open();

				foreach (KeyValuePair<ApplicationEntity, List<StudyInformation>> kvp in retrieveInformation)
				{
					AEInformation aeInformation = new AEInformation();
					aeInformation.AETitle = kvp.Key.AETitle;
					aeInformation.HostName = kvp.Key.Host;
					aeInformation.Port = kvp.Key.Port;

					client.RetrieveStudies(aeInformation, kvp.Value);
				}

				client.Close();

				LocalDataStoreActivityMonitorComponentManager.ShowSendReceiveActivityComponent(Context.DesktopWindow);
			}
			catch (EndpointNotFoundException)
			{
				client.Abort();
				result = EventResult.MajorFailure;
				Context.DesktopWindow.ShowMessageBox(SR.MessageRetrieveDicomServerServiceNotRunning, MessageBoxActions.Ok);
			}
			catch (Exception e)
			{
				result = EventResult.MajorFailure;
				ExceptionHandler.Report(e, SR.MessageFailedToRetrieveStudy, Context.DesktopWindow);
			}
			finally
			{
				foreach (KeyValuePair<ApplicationEntity, List<StudyInformation>> kvp in retrieveInformation)
				{
					AuditedInstances requestedInstances = new AuditedInstances();
					foreach (StudyInformation study in kvp.Value)
						requestedInstances.AddInstance(study.PatientId, study.PatientsName, study.StudyInstanceUid);
					AuditHelper.LogBeginReceiveInstances(kvp.Key.AETitle, kvp.Key.Host, requestedInstances, EventSource.CurrentUser, result);
				}
			}
		}

		private bool GetAtLeastOneServerSupportsLoading()
		{
			if (Context.SelectedServerGroup.IsLocalDatastore && base.IsLocalStudyLoaderSupported)
				return true;

			foreach (Server server in base.Context.SelectedServerGroup.Servers)
			{
				if (server.IsStreaming && base.IsStreamingStudyLoaderSupported)
					return true;
				else if (!server.IsStreaming && base.IsRemoteStudyLoaderSupported)
					return true;
			}

			return false;
		}

		private void SetDoubleClickHandler()
		{
			if (!GetAtLeastOneServerSupportsLoading() && base.Context.SelectedServerGroup.Servers.Count > 0)
				Context.DefaultActionHandler = RetrieveStudy;
		}

		protected override void OnSelectedStudyChanged(object sender, EventArgs e)
		{
			UpdateEnabled();
		}
		
		protected override void OnSelectedServerChanged(object sender, EventArgs e)
		{
			UpdateEnabled();
			SetDoubleClickHandler();
		}

		private void UpdateEnabled()
		{
			Enabled = (Context.SelectedStudy != null &&
			                !Context.SelectedServerGroup.IsLocalDatastore &&
			                LocalDataStoreActivityMonitor.IsConnected);
		}
	}
}
