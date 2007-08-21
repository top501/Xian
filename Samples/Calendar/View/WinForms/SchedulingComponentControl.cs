using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using ClearCanvas.Desktop.View.WinForms;
using ClearCanvas.Controls.WinForms;

namespace ClearCanvas.Samples.Calendar.View.WinForms
{
    /// <summary>
    /// Provides a Windows Forms user-interface for <see cref="SchedulingComponent"/>
    /// </summary>
    public partial class SchedulingComponentControl : ApplicationComponentUserControl
    {
        private SchedulingComponent _component;

        /// <summary>
        /// Constructor
        /// </summary>
        public SchedulingComponentControl(SchedulingComponent component)
            :base(component)
        {
            InitializeComponent();

            _component = component;

            // TODO add .NET databindings to _component
            _comment.DataBindings.Add("Value", _component, "Comment", true, DataSourceUpdateMode.OnPropertyChanged);
            _followUpDate.DataBindings.Add("Value", _component, "AppointmentDate", true, DataSourceUpdateMode.OnPropertyChanged);
            _patientInfo.DataBindings.Add("Value", _component, "PatientInfo", true, DataSourceUpdateMode.OnPropertyChanged);

            _appointmentsTableView.Table = _component.Appointments;
        }

        private void _addButton_Click(object sender, EventArgs e)
        {
            using (CursorManager cm = new CursorManager(Cursors.WaitCursor))
            {
                _component.AddAppointment();
            }
        }
    }
}
