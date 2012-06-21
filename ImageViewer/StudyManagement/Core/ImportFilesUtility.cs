﻿#region License

// Copyright (c) 2012, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Data.Linq;
using System.IO;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Network;
using ClearCanvas.Dicom.Utilities.Command;
using ClearCanvas.ImageViewer.Common;
using ClearCanvas.ImageViewer.Common.ServerDirectory;
using ClearCanvas.ImageViewer.Common.StudyManagement;
using ClearCanvas.ImageViewer.Common.WorkItem;
using ClearCanvas.ImageViewer.StudyManagement.Core.Command;
using ClearCanvas.ImageViewer.StudyManagement.Core.Storage;
using ClearCanvas.ImageViewer.StudyManagement.Core.WorkItemProcessor;

namespace ClearCanvas.ImageViewer.StudyManagement.Core
{

    /// <summary>
    /// Class used for returning result information when processing.  Used for importing.
    /// </summary>
    public class DicomProcessingResult
    {
        public String AccessionNumber;
        public String StudyInstanceUid;
        public String SeriesInstanceUid;
        public String SopInstanceUid;
        public bool Successful;
        public String ErrorMessage;
        public DicomStatus DicomStatus;
        public bool RestoreRequested;
        public bool RetrySuggested;

        public void SetError(DicomStatus status, String message)
        {
            Successful = false;
            DicomStatus = status;
            ErrorMessage = message;
        }

        public void Initialize()
        {
            Successful = true;
            ErrorMessage = string.Empty;
            DicomStatus = DicomStatuses.Success;
        }
    }

    /// <summary>
    /// A context object used when importing a batch of DICOM SOP Instances from a DICOM association.
    /// </summary>
    public class DicomReceiveImportContext : ImportFilesContext
    {
        #region Private Members

        private readonly IWorkItemActivityMonitor _monitor;
        private readonly IDicomServiceNode _dicomServerNode;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sourceAE">The AE title of the remote application sending the SOP Instances.</param>
        /// <param name="configuration">Storage configuration. </param>
        public DicomReceiveImportContext(string sourceAE, StorageConfiguration configuration) : base(sourceAE, configuration)
        {
            // TODO (CR Jun 2012): This object is disposable and should be cleaned up.

            _monitor = WorkItemActivityMonitor.Create(false);
            _monitor.WorkItemsChanged += WorkItemsChanged;

            var serverList = ServerDirectory.GetRemoteServersByAETitle(sourceAE);
            if (serverList.Count > 0)
                _dicomServerNode = CollectionUtils.FirstElement(serverList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a StudyProcessRequest object for a specific SOP Instance.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override ProcessStudyRequest CreateRequest(DicomMessageBase message)
        {
            var request = new DicomReceiveRequest
                              {
                                  FromAETitle = _dicomServerNode == null ? SourceAE : _dicomServerNode.Name,
                                  Priority = WorkItemPriorityEnum.High,
                                  Patient = new WorkItemPatient(message.DataSet),
                                  Study = new WorkItemStudy(message.DataSet)
                              };

            return request;
        }

        /// <summary>
        /// Create a <see cref="ProcessStudyProgress"/> object for the type of import.
        /// </summary>
        /// <returns></returns>
        public override ProcessStudyProgress CreateProgress()
        {
            return new ProcessStudyProgress { IsCancelable = false, TotalFilesToProcess = 1 };
        }

        #endregion

        #region Private Methods

        private void WorkItemsChanged(object sender, WorkItemsChangedEventArgs e)
        {
            // TODO (CR Jun 2012): ObservableDictionary is not thread safe, and even though this
            // is a "read" operation, all usages of StudyWorkItems should be inside a lock.
            foreach (var item in e.ChangedItems)
            {
                if (item.Request is DicomReceiveRequest)
                {
                    WorkItem workItem;
                    if (StudyWorkItems.TryGetValue(item.StudyInstanceUid, out workItem))
                    {
                        if (workItem.Oid == item.Identifier)
                        {
                            workItem.Status = item.Status;
                            workItem.Progress = item.Progress;
                        }
                    }
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// A context object used when importing a batch of DICOM SOP Instances from disk.
    /// </summary>
    public class ImportStudyContext : ImportFilesContext
    {
        #region Contructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sourceAE">The local AE title of the application importing the studies.</param>
        /// <param name="configuration">The storage configuration. </param>
        public ImportStudyContext(string sourceAE, StorageConfiguration configuration)
            : base(sourceAE, configuration)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a <see cref="ProcessStudyRequest"/> object for a specific SOP Instnace.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override ProcessStudyRequest CreateRequest(DicomMessageBase message)
        {
            var request = new ImportStudyRequest
                              {
                                  Priority = WorkItemPriorityEnum.High,
                                  Patient = new WorkItemPatient(message.DataSet),
                                  Study = new WorkItemStudy(message.DataSet)
                              };
            return request;
        }

        /// <summary>
        /// Create a <see cref="ProcessStudyProgress"/> object for the type of import.
        /// </summary>
        /// <returns></returns>
        public override ProcessStudyProgress CreateProgress()
        {
            return new ProcessStudyProgress { IsCancelable = true, TotalFilesToProcess = 1 };
        }

        #endregion
    }

    /// <summary>
    /// Encapsulates the context of the application when <see cref="ImportFilesUtility"/> is called.
    /// </summary>
    public abstract class ImportFilesContext
    {     
        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="ImportFilesContext"/> to be used
        /// by <see cref="ImportFilesUtility"/> 
        /// </summary>
        protected ImportFilesContext(string sourceAE, StorageConfiguration configuration)
        {
            StudyWorkItems = new ObservableDictionary<string, WorkItem>();
            SourceAE = sourceAE;
            StorageConfiguration = configuration;

            ExpirationDelaySeconds = WorkItemServiceSettings.Default.ExpireDelaySeconds;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the source AE title where the image(s) are imported from
        /// </summary>
        public string SourceAE { get; private set; }

        /// <summary>
        /// Map of the studies and corresponding WorkItems items for the current context
        /// </summary>
        public ObservableDictionary<string,WorkItem> StudyWorkItems { get; private set; }

        /// <summary>
        /// Storage configuration.
        /// </summary>
        public StorageConfiguration StorageConfiguration { get; private set; }

        /// <summary>
        /// Delay to expire inserted WorkItems
        /// </summary>
        public int ExpirationDelaySeconds { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Abstract method for creating a <see cref="ProcessStudyRequest"/> object for the given DICOM message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract ProcessStudyRequest CreateRequest(DicomMessageBase message);

        /// <summary>
        /// Abstract method for creating a <see cref="ProcessStudyProgress"/> object for the request.
        /// </summary>
        /// <returns></returns>
        public abstract ProcessStudyProgress CreateProgress();

        #endregion
    }

    /// <summary>
    /// Import utility for importing specific SOP Instances, either in memory from the network or on disk.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that the files being imported do not have to belong to the same study.  ImportFilesUtility will 
    /// automatically detect the study the files belong to, and import them to the proper location.
    /// </para>
    /// </remarks>
    public class ImportFilesUtility
    {
        #region Private Members
        private readonly ImportFilesContext _context; 
        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="ImportFilesUtility"/> to import DICOM object(s)
        /// into the system.
        /// </summary>
        /// <param name="context">The context of the operation.</param>
        public ImportFilesUtility(ImportFilesContext context)
        {
            Platform.CheckForNullReference(context, "context");
            _context = context;
        } 
        #endregion

        #region Public Methods

        /// <summary>
        /// Imports the specified <see cref="DicomMessageBase"/> object into the system.
        /// The object will be inserted into the <see cref="WorkItem"/> for processing
        /// </summary>
        /// <param name="message">The DICOM object to be imported.</param>
        /// <param name="badFileBehavior"> </param>
        /// <param name="fileImportBehaviour"> </param>
        /// <returns>An instance of <see cref="DicomProcessingResult"/> that describes the result of the processing.</returns>
        /// <exception cref="DicomDataException">Thrown when the DICOM object contains invalid data</exception>
        public DicomProcessingResult Import(DicomMessageBase message, BadFileBehaviourEnum badFileBehavior, FileImportBehaviourEnum fileImportBehaviour)
        {
            // TODO (CR Jun 2012): This is a pretty long method.
            
            Platform.CheckForNullReference(message, "message");
            String studyInstanceUid = message.DataSet[DicomTags.StudyInstanceUid].GetString(0, string.Empty);
            String seriesInstanceUid = message.DataSet[DicomTags.SeriesInstanceUid].GetString(0, string.Empty);
            String sopInstanceUid = message.DataSet[DicomTags.SopInstanceUid].GetString(0, string.Empty);
            String accessionNumber = message.DataSet[DicomTags.AccessionNumber].GetString(0, string.Empty);
            String patientsName = message.DataSet[DicomTags.PatientsName].GetString(0, string.Empty);

            // TODO (CR Jun 2012): This appears to do nothing.
            string newName = patientsName;
            if (!newName.Equals(patientsName))
                message.DataSet[DicomTags.PatientsName].SetStringValue(newName);

			var result = new DicomProcessingResult
			                               	{
			                               		Successful = true,
			                               		StudyInstanceUid = studyInstanceUid,
			                               		SeriesInstanceUid = seriesInstanceUid,
			                               		SopInstanceUid = sopInstanceUid,
			                               		AccessionNumber = accessionNumber
			                               	};

            WorkItem workItem;
            _context.StudyWorkItems.TryGetValue(studyInstanceUid, out workItem);
            
        	try
			{
				Validate(message);
			}
			catch (DicomDataException e)
			{
				result.SetError(DicomStatuses.ProcessingFailure, e.Message);
                if (workItem != null)
                    SignalWorkItemFailure(workItem, "Some images failed to import/receive. See log for details.");
                return result;
			}

            if (workItem!=null)
            {
                if (workItem.Status == WorkItemStatusEnum.Deleted || workItem.Status == WorkItemStatusEnum.Canceled || workItem.Status == WorkItemStatusEnum.Canceling)
                {
                    // TODO Marmot (CR June 2012): not DicomStatuses.Cancel?
                    result.SetError(DicomStatuses.StorageStorageOutOfResources, "Receive canceled by user");
                    return result;                       
                }
            }

            if (LocalStorageMonitor.IsMaxUsedSpaceExceeded)
            {
                result.SetError(DicomStatuses.StorageStorageOutOfResources, string.Format("Out Of Storage" /*keep it short. Max Length for LO is 16 */));
                if (workItem!=null)
                    SignalWorkItemFailure(workItem, "Some images failed to import/receive due to lack of storage space.");
                return result;
            }

            Process(message, fileImportBehaviour, workItem, result);

            if (result.DicomStatus != DicomStatuses.Success)
            {
                if (result.RetrySuggested)
                {
                    Platform.Log(LogLevel.Warn,
                                 "Failure importing file with retry suggested, retrying Import of file: {0}",
                                 sopInstanceUid);

                    Process(message, fileImportBehaviour, workItem, result);
                }
                if (result.DicomStatus != DicomStatuses.Success)
                {
                    _context.StudyWorkItems.TryGetValue(studyInstanceUid, out workItem);

                    if (workItem != null)
                        SignalWorkItemFailure(workItem, "Some images failed to import/receive. See log for details.");
                }
            }
            return result;
        }

        #endregion

        #region Private Methods
        
        private void Process(DicomMessageBase message, FileImportBehaviourEnum fileImportBehaviour, WorkItem workItem, DicomProcessingResult result)
        {
            result.Initialize();

            // Use the command processor for rollback capabilities.
            using (
                var commandProcessor =
                    new ViewerCommandProcessor(String.Format("Processing Sop Instance {0}", result.SopInstanceUid)))
            {
                try
                {
                    var studyLocation = new StudyLocation(message.DataSet[DicomTags.StudyInstanceUid].ToString());

                    String destinationFile = studyLocation.GetSopInstancePath(result.SeriesInstanceUid,
                                                                              result.SopInstanceUid);

                    DicomFile file = ConvertToDicomFile(message, destinationFile, _context.SourceAE);

                    // Create the Study Folder, if need be
                    commandProcessor.AddCommand(new CreateDirectoryCommand(studyLocation.StudyFolder));

                    bool duplicateFile = false;
                    string dupName = Guid.NewGuid().ToString() + ".dcm";

                    if (File.Exists(destinationFile))
                    {
                        // TODO (CR Jun 2012): Shouldn't the commands themselves make this decision at the time
                        // the file is being saved? Otherwise, what happens if the same SOP were being saved 2x simultaneously.
                        // I know the odds are low, but just pointing it out.
                        duplicateFile = true;
                        destinationFile = Path.Combine(Path.GetDirectoryName(destinationFile), dupName);
                    }

                    if (fileImportBehaviour == FileImportBehaviourEnum.Move)
                    {
                        commandProcessor.AddCommand(new RenameFileCommand(file.Filename, destinationFile, true));
                    }
                    else if (fileImportBehaviour == FileImportBehaviourEnum.Copy)
                    {
                        commandProcessor.AddCommand(new CopyFileCommand(file.Filename, destinationFile, true));
                    }
                    else if (fileImportBehaviour == FileImportBehaviourEnum.Save)
                    {
                        commandProcessor.AddCommand(new SaveDicomFileCommand(destinationFile, file, true));
                    }

                    InsertWorkItemCommand command;
                    if (duplicateFile)
                    {
                        command = workItem != null
                                      ? new InsertWorkItemCommand(workItem, result.StudyInstanceUid,
                                                                  result.SeriesInstanceUid, result.SopInstanceUid,
                                                                  dupName)
                                      : new InsertWorkItemCommand(_context.CreateRequest(file),
                                                                  _context.CreateProgress(), result.StudyInstanceUid,
                                                                  result.SeriesInstanceUid, result.SopInstanceUid,
                                                                  dupName);
                    }
                    else
                    {
                        command = workItem != null
                                      ? new InsertWorkItemCommand(workItem, result.StudyInstanceUid,
                                                                  result.SeriesInstanceUid, result.SopInstanceUid)
                                      : new InsertWorkItemCommand(_context.CreateRequest(file),
                                                                  _context.CreateProgress(), result.StudyInstanceUid,
                                                                  result.SeriesInstanceUid, result.SopInstanceUid);
                    }

                    command.ExpirationDelaySeconds = _context.ExpirationDelaySeconds;
                    commandProcessor.AddCommand(command);

                    if (commandProcessor.Execute())
                    {
                        result.DicomStatus = DicomStatuses.Success;

                        // TODO (CR Jun 2012): What about the first image imported? It won't be in this dictionary.
                        if (_context.StudyWorkItems.ContainsKey(result.StudyInstanceUid))
                        {
                            var progress = command.WorkItem.Progress as ProcessStudyProgress;
                            if (progress != null)
                            {
                                using (var context = new DataAccessContext(DataAccessContext.WorkItemMutex))
                                {
                                    var broker = context.GetWorkItemBroker();

                                    command.WorkItem = broker.GetWorkItem(command.WorkItem.Oid);
                                    progress = command.WorkItem.Progress as ProcessStudyProgress;
                                    if (progress != null)
                                    {
                                        progress.TotalFilesToProcess++;
                                        command.WorkItem.Progress = progress;
                                    }

                                    context.Commit();
                                }
                            }

                            // TODO (CR Jun 2012): ImportItemProcess publishes as items are added to the dictionary, which happens right after this.
                            Platform.GetService(
                                 (IWorkItemActivityMonitorService service) =>
                                 service.Publish(new WorkItemPublishRequest { Item = WorkItemDataHelper.FromWorkItem(command.WorkItem) }));

                            // Save the updated WorkItem
                            _context.StudyWorkItems[result.StudyInstanceUid] = command.WorkItem;
                        }
                    }
                    else
                    {
                        if (commandProcessor.FailureException is ChangeConflictException)
                            result.RetrySuggested = true; // Change conflict may work if we just retry

                        Platform.Log(LogLevel.Warn, "Failure Importing file: {0}", file.Filename);
                        string failureMessage = String.Format(
                            "Failure processing message: {0}. Sending failure status.",
                            commandProcessor.FailureReason);
                        result.SetError(DicomStatuses.ProcessingFailure, failureMessage);
           
                        // processor already rolled back                     
                    }
                }
                catch (Exception e)
                {
                    Platform.Log(LogLevel.Error, e, "Unexpected exception when {0}.  Rolling back operation.",
                                 commandProcessor.Description);
                    commandProcessor.Rollback();
                    result.SetError(result.DicomStatus ?? DicomStatuses.ProcessingFailure, e.Message);
                }
            }
        }

        /// <summary>
        /// Update the WQI to indicate there're errors which will result in incomplete study. 
        /// </summary>
        /// <param name="workItem"></param>
        /// <param name="error"></param>
        private void SignalWorkItemFailure(WorkItem workItem, string error)
        {
            /// Note: This method does not fail the WQI. It's the WQI processor's responsibility to decide how to handle this.
            
            WorkItem actualItem;

            using (var context = new DataAccessContext(DataAccessContext.WorkItemMutex))
            {
                var broker = context.GetWorkItemBroker();

                actualItem = broker.GetWorkItem(workItem.Oid);
                // Note, a workItem that was to-be committed could end up here that never got in the database (likely due to a lock timeout), just ignore in that case.
                if (actualItem == null)
                    return;

                if (actualItem.Progress == null)
                    actualItem.Progress = _context.CreateProgress();

                var progress = actualItem.Progress as ProcessStudyProgress;
                if (progress != null)
                {
                    // Note: this is kind of a hack. Tried to keep track of number of failures but it appears 
                    // the current design always updates all properties whenever the context is committed. This causes the value to 
                    // be overwritten with old values when mutiple threads post updates on the same WQI.
                    progress.OtherFatalFailures = error;

                    actualItem.Progress = progress;
                    context.Commit();
                }                
            }

            Platform.GetService(
                (IWorkItemActivityMonitorService service) =>
                    service.Publish(new WorkItemPublishRequest { Item = WorkItemDataHelper.FromWorkItem(actualItem) }));
        }
       
        private static void Validate(DicomMessageBase message)
        {
            String studyInstanceUid = message.DataSet[DicomTags.StudyInstanceUid].GetString(0, string.Empty);

            if (string.IsNullOrEmpty(studyInstanceUid))
                throw new DicomDataException("Study Instance UID does not have a value.");

            // TODO (CR Jun 2012): What about SeriesInstanceUid?

            String sopInstanceUid = message.DataSet[DicomTags.SopInstanceUid].GetString(0, string.Empty);
            if (string.IsNullOrEmpty(sopInstanceUid))
                throw new DicomDataException("SOP Instance UID does not have a value.");

        }

        static private DicomFile ConvertToDicomFile(DicomMessageBase message, string filename, string sourceAE)
        {
            // This routine sets some of the group 0x0002 elements.
            DicomFile file;
            if (message is DicomFile)
            {
                file = message as DicomFile;
            }
            else if (message is DicomMessage)
            {
                file = new DicomFile(message as DicomMessage, filename);
            }
            else
            {
                throw new NotSupportedException(String.Format("Cannot convert {0} to DicomFile", message.GetType()));
            }

            file.SourceApplicationEntityTitle = sourceAE;
            file.TransferSyntax = message.TransferSyntax.Encapsulated ? 
				message.TransferSyntax : TransferSyntax.ExplicitVrLittleEndian;

            return file;
        }
        
        #endregion
	}
}
