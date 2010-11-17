#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using ClearCanvas.Desktop;
using ClearCanvas.Common;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.ImageViewer;
using ClearCanvas.ImageViewer.Explorer.Local;
using ClearCanvas.Dicom;
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.BaseTools;
using ClearCanvas.ImageViewer.StudyManagement;

namespace ClearCanvas.Utilities.DicomEditor
{
	[MenuAction("activate", "explorerlocal-contextmenu/MenuDumpFiles", "Dump")]
	[MenuAction("activate", "global-menus/MenuTools/MenuUtilities/MenuDicomEditor", "Dump")]
    [Tooltip("activate", "OpenDicomFilesVerbose")]
	[IconSet("activate", IconScheme.Colour, "Icons.DicomEditorToolSmall.png", "Icons.DicomEditorToolMedium.png", "Icons.DicomEditorToolLarge.png")]
    [EnabledStateObserver("activate", "Enabled", "EnabledChanged")]
	[GroupHint("activate", "Tools.Dicom.Editor")]
	[ViewerActionPermission("activate", AuthorityTokens.DicomEditor)]
	
	[ExtensionOf(typeof(ImageViewerToolExtensionPoint))]
    [ExtensionOf(typeof(LocalImageExplorerToolExtensionPoint))]
    public class ShowDicomEditorTool : ToolBase
    {		
        private static IShelf _shelf;
        private static DicomEditorComponent _component = null;
        private IDesktopWindow _desktopWindow;	
        private bool _enabled;
        private event EventHandler _enabledChanged;

        public ShowDicomEditorTool()
        {
            _enabled = true;
            _desktopWindow = null;
        }

        /// <summary>
        /// Called by the framework to initialize this tool.
        /// </summary>
        public override void Initialize()
        {
        	base.Initialize();
        	if (ContextBase is ILocalImageExplorerToolContext)
        		((ILocalImageExplorerToolContext) ContextBase).SelectedPathsChanged += OnContextSelectedPathsChanged;
        }

		protected override void Dispose(bool disposing)
		{
			if (ContextBase is ILocalImageExplorerToolContext)
				((ILocalImageExplorerToolContext) ContextBase).SelectedPathsChanged -= OnContextSelectedPathsChanged;
			base.Dispose(disposing);
		}

        /// <summary>
        /// Called to determine whether this tool is enabled/disabled in the UI.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            protected set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    EventsHelper.Fire(_enabledChanged, this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Notifies that the Enabled state of this tool has changed.
        /// </summary>
        public event EventHandler EnabledChanged
        {
            add { _enabledChanged += value; }
            remove { _enabledChanged -= value; }
        }

        private void Dump()
        {
            if (this.ContextBase is IImageViewerToolContext)
            {
                IImageViewerToolContext context = this.ContextBase as IImageViewerToolContext;
                _desktopWindow = context.DesktopWindow;
                IImageSopProvider image = context.Viewer.SelectedPresentationImage as IImageSopProvider;
                if (image == null)
                {                    
                    _desktopWindow.ShowMessageBox(SR.MessagePleaseSelectAnImage, MessageBoxActions.Ok);
                    return;
                }

            	IDicomMessageSopDataSource dataSource = image.ImageSop.DataSource as IDicomMessageSopDataSource;
				if (dataSource == null || dataSource.SourceMessage ==  null)
				{
					 _desktopWindow.ShowMessageBox(SR.MessageUnknownDataSource, MessageBoxActions.Ok);
					return;
				}

                //Fix for Ticket #623 - HH - It turns out that for memory usage optimization the pixel data tag is stripped from the in memory dataset.  
                //So while there are probably many better ways to address the missing pixel data tag a small hack was introduced because this entire utility will 
                //be completely refactored in the very near future to make use of the methods the pacs uses to parse the tags.
                //Addendum to Comment above - HH 07/27/07 - Turns out that our implementation continues to remove the pixel data for optimization at this time so 
                //the workaround is still needed.
				//Addendum to Comment above - JY 09/16/08 - Somewhere along the line, things were changed that made this line redundant - the only reference to
				//it after this point is at +11 lines or so, and all it does is get file.Filename. Therefore, I am commenting this line out.
                //file = new DicomFile(file.Filename);

                if (_component == null)
                {
                    _component = new DicomEditorComponent();
                }
                else
                {
                    _component.Clear();
                }

                _component.Load(dataSource.SourceMessage);
            }
            else if (this.ContextBase is ILocalImageExplorerToolContext)
            {
                ILocalImageExplorerToolContext context = this.ContextBase as ILocalImageExplorerToolContext;
                _desktopWindow = context.DesktopWindow;
                List<string> files = new List<string>();

                if (context.SelectedPaths.Count == 0)
                    return;

                foreach (string rawPath in context.SelectedPaths)
                {
                	if (string.IsNullOrEmpty(rawPath))
                		continue;

                    FileProcessor.Process(rawPath, "*.*", files.Add, true);
                }

                if (files.Count == 0)
                {
                    context.DesktopWindow.ShowMessageBox(SR.MessageNoFilesSelected, MessageBoxActions.Ok);
                    return;
                }

                if (_component == null)
                {
                    _component = new DicomEditorComponent();
                }
                else
                {
                    _component.Clear();
                }

                bool userCancelled = false;

                BackgroundTask task = new BackgroundTask(delegate(IBackgroundTaskContext backgroundcontext)
                {
                    int i = 0;

                    foreach (string file in files)
                    {
                        if (backgroundcontext.CancelRequested)
                        {
                            backgroundcontext.Cancel();
                            userCancelled = true;
                            return;
                        }
                        try
                        {
                            _component.Load(file);
                        }
                        catch (DicomException e)
                        {
                            backgroundcontext.Error(e);
                            return;
                        }
                        backgroundcontext.ReportProgress(new BackgroundTaskProgress((int)(((double)(i + 1) / (double)files.Count) * 100.0), SR.MessageDumpProgressBar));
                        i++;
                    }

                    backgroundcontext.Complete(null);
                }, true);

                try
                {
                    ProgressDialog.Show(task, _desktopWindow, true);
                }
                catch (Exception e)
                {
                    ExceptionHandler.Report(e, SR.MessageFailedDump, _desktopWindow);
                    return;
                }

                if (userCancelled == true)
                    return;
            }

            //common to both contexts
            if (_shelf != null)
            {
                _shelf.Activate();
            }
            else
            {
                _shelf = ApplicationComponent.LaunchAsShelf(
                    _desktopWindow,
                    _component,
                    SR.TitleDicomEditor,
                    "Dicom Editor",
                    ShelfDisplayHint.DockRight | ShelfDisplayHint.DockAutoHide);
                _shelf.Closed += OnShelfClosed;
            }
  
            _component.UpdateComponent();

        }

        private void OnShelfClosed(object sender, ClosedEventArgs e)
        {
            // We need to cache the owner DesktopWindow (_desktopWindow) because this tool is an 
            // ImageViewer tool, disposed when the viewer component is disposed.  Shelves, however,
            // exist at the DesktopWindow level and there can only be one of each type of shelf
            // open at the same time per DesktopWindow (otherwise things look funny).  Because of 
            // this, we need to allow this event handling method to be called after this tool has
            // already been disposed (e.g. viewer workspace closed), which is why we store the 
            // _desktopWindow variable.

            _shelf.Closed -= OnShelfClosed;
            _shelf = null;
        }

		private void OnContextSelectedPathsChanged(object sender, EventArgs e)
		{
			if (ContextBase is ILocalImageExplorerToolContext)
				Enabled = ((ILocalImageExplorerToolContext) ContextBase).SelectedPaths.Count > 0;
		}
    }
}
