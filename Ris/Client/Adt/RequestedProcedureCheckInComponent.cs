using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.Admin;
using ClearCanvas.Ris.Application.Common.Admin.StaffAdmin;
using ClearCanvas.Ris.Application.Common.RegistrationWorkflow;

namespace ClearCanvas.Ris.Client.Adt
{
    /// <summary>
    /// Extension point for views onto <see cref="RequestedProcedureCheckInComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class RequestedProcedureCheckInComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    /// <summary>
    /// RequestedProcedureCheckInComponent class
    /// </summary>
    [AssociateView(typeof(RequestedProcedureCheckInComponentViewExtensionPoint))]
    public class RequestedProcedureCheckInComponent : ApplicationComponent
    {
        private RegistrationWorklistItem _worklistItem;
        private RequestedProcedureCheckInTable _requestedProcedureCheckInTable;

        /// <summary>
        /// Constructor
        /// </summary>
        public RequestedProcedureCheckInComponent(RegistrationWorklistItem item)
        {
            _worklistItem = item;
        }

        public override void Start()
        {
            _requestedProcedureCheckInTable = new RequestedProcedureCheckInTable();

            try
            {
                Platform.GetService<IRegistrationWorkflowService>(
                    delegate(IRegistrationWorkflowService service)
                    {
                        GetDataForCheckInTableResponse response = service.GetDataForCheckInTable(new GetDataForCheckInTableRequest(_worklistItem.WorklistClassName, _worklistItem.PatientProfileRef));
                        _requestedProcedureCheckInTable.Items.AddRange(
                            CollectionUtils.Map<CheckInTableItem, RequestedProcedureCheckInTableEntry>(response.CheckInTableItems,
                                    delegate(CheckInTableItem item)
                                    {
                                        RequestedProcedureCheckInTableEntry entry = new RequestedProcedureCheckInTableEntry(item);
                                        entry.CheckedChanged += new EventHandler(RequestedProcedureCheckedStateChangedEventHandler);
                                        return entry;
                                    }));
                    });
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }

            // Special case for 0 or 1 Requested Procedure.  No need to show the dialog box
            if (_requestedProcedureCheckInTable.Items.Count == 0)
            {
                this.ExitCode = ApplicationComponentExitCode.Normal;
                Host.Exit();
            }
            else if (_requestedProcedureCheckInTable.Items.Count == 1)
            {
                _requestedProcedureCheckInTable.Items[0].Checked = true;
                SaveChanges();
                this.ExitCode = ApplicationComponentExitCode.Normal;
                Host.Exit();
            }

            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }

        #region Presentation Model

        public ITable RequestedProcedureTable
        {
            get { return _requestedProcedureCheckInTable; }
        }

        #endregion

        public void Accept()
        {
            if (this.HasValidationErrors)
            {
                this.ShowValidation(true);
            }
            else
            {
                try
                {
                    SaveChanges();
                    this.ExitCode = ApplicationComponentExitCode.Normal;
                    Host.Exit();
                }
                catch (Exception e)
                {
                    ExceptionHandler.Report(e, this.Host.DesktopWindow);
                }
            }
        }

        private void SaveChanges()
        {
            // TODO: Need to get the real current staff that is using the system
            EntityRef staffRef = null;
            Platform.GetService<IStaffAdminService>(
                delegate(IStaffAdminService service)
                {
                    FindStaffsResponse findResponse = service.FindStaffs(new FindStaffsRequest("Clerk", "Registration"));
                    if (findResponse.Staffs.Count == 0)
                    {
                        StaffDetail newStaff = new StaffDetail();
                        newStaff.PersonNameDetail.FamilyName = "Clerk";
                        newStaff.PersonNameDetail.GivenName = "Registration";
                        AddStaffResponse addResponse = service.AddStaff(new AddStaffRequest(newStaff));
                        staffRef = addResponse.Staff.StaffRef;
                    }
                    else
                    {
                        StaffSummary staff = CollectionUtils.FirstElement(findResponse.Staffs) as StaffSummary;
                        staffRef = staff.StaffRef;
                    }
                });

            // Get the list of RequestedProcedure EntityRef from the table
            List<EntityRef> selectedRequestedProcedures = new List<EntityRef>();
            foreach (RequestedProcedureCheckInTableEntry entry in _requestedProcedureCheckInTable.Items)
            {
                if (entry.Checked)
                    selectedRequestedProcedures.Add(entry.CheckInTableItem.RequestedProcedureRef);
            }      

            Platform.GetService<IRegistrationWorkflowService>(
                delegate(IRegistrationWorkflowService service)
                {
                    service.CheckInProcedure(new CheckInProcedureRequest(selectedRequestedProcedures, staffRef));
                });        
        }

        public void Cancel()
        {
            this.ExitCode = ApplicationComponentExitCode.Cancelled;
            Host.Exit();
        }

        public bool AcceptEnabled
        {
            get { return this.Modified; }
        }

        public event EventHandler AcceptEnabledChanged
        {
            add { this.ModifiedChanged += value; }
            remove { this.ModifiedChanged -= value; }
        }

        private void RequestedProcedureCheckedStateChangedEventHandler(object sender, EventArgs e)
        {
            foreach (RequestedProcedureCheckInTableEntry entry in _requestedProcedureCheckInTable.Items)
            {
                if (entry.Checked)
                {
                    this.Modified = true;
                    return;
                }
            }

            this.Modified = false;
        }

    }
}
