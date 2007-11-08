#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
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
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common.ReportingWorkflow;

namespace ClearCanvas.Ris.Client.Reporting
{
    [MenuAction("view", "global-menus/Patient/View Details...")]
    //[ButtonAction("view", "global-toolbars/Patient/ViewPatient")]
    [ButtonAction("view", "folderexplorer-items-toolbar/Patient Details")]
    [MenuAction("view", "folderexplorer-items-contextmenu/Patient Details")]
    [ClickHandler("view", "View")]
    [EnabledStateObserver("view", "Enabled", "EnabledChanged")]
    [Tooltip("view", "Open patient details")]
	[IconSet("view", IconScheme.Colour, "PatientDetailsToolSmall.png", "PatientDetailsToolMedium.png", "PatientDetailsToolLarge.png")]
    [ExtensionOf(typeof(ReportingWorkflowItemToolExtensionPoint))]
    public class PatientBiographyTool : Tool<IToolContext>
    {
        private bool _enabled;
        private event EventHandler _enabledChanged;
        
        public override void Initialize()
        {
            base.Initialize();

            if (this.ContextBase is IReportingWorkflowItemToolContext)
            {
                ((IReportingWorkflowItemToolContext)this.ContextBase).SelectedItemsChanged += delegate
                {
                    this.Enabled = DetermineEnablement();
                };
            }
        }

        private bool DetermineEnablement()
        {
            if (this.ContextBase is IReportingWorkflowItemToolContext)
            {
                return (((IReportingWorkflowItemToolContext)this.ContextBase).SelectedItems != null
                    && ((IReportingWorkflowItemToolContext)this.ContextBase).SelectedItems.Count == 1);
            }

            return false;
        }

        public bool Enabled
        {
            get 
            { 
                this.Enabled = DetermineEnablement();
                return _enabled;
            }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    EventsHelper.Fire(_enabledChanged, this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler EnabledChanged
        {
            add { _enabledChanged += value; }
            remove { _enabledChanged -= value; }
        }

        public void View()
        {
            if (this.ContextBase is IReportingWorkflowItemToolContext)
            {
                IReportingWorkflowItemToolContext context = (IReportingWorkflowItemToolContext)this.ContextBase;
                ReportingWorklistItem item = CollectionUtils.FirstElement<ReportingWorklistItem>(context.SelectedItems);
                OpenPatient(item.PatientProfileRef, context.DesktopWindow);
            }
        }

        protected static void OpenPatient(EntityRef profile, IDesktopWindow window)
        {
            try
            {
                Document doc = DocumentManager.Get(profile.ToString());
                if (doc == null)
                {
                    doc = new PatientBiographyDocument(profile, window);
                    doc.Open();
                }
                else
                {
                    doc.Activate();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, window);
            }
        }
    }
}
