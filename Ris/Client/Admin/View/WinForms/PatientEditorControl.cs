using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ClearCanvas.Ris.Client.Admin.View.WinForms
{
    public partial class PatientEditorControl : UserControl
    {
        private PatientEditorComponent _component;

        public PatientEditorControl(PatientEditorComponent component)
        {
            InitializeComponent();
            _component = component;

            // create bindings
            _familyName.DataBindings.Add("Value", _component, "FamilyName", true, DataSourceUpdateMode.OnPropertyChanged);
            _givenName.DataBindings.Add("Value", _component, "GivenName", true, DataSourceUpdateMode.OnPropertyChanged);
            _middleName.DataBindings.Add("Value", _component, "MiddleName", true, DataSourceUpdateMode.OnPropertyChanged);

            _sex.DataSource = _component.SexChoices;
            _sex.DataBindings.Add("Value", _component, "Sex", true, DataSourceUpdateMode.OnPropertyChanged);

            _dateOfBirth.DataBindings.Add("Value", _component, "DateOfBirth", true, DataSourceUpdateMode.OnPropertyChanged);
            _dateOfDeath.DataBindings.Add("Value", _component, "TimeOfDeath", true, DataSourceUpdateMode.OnPropertyChanged);

            _mrn.DataBindings.Add("Value", _component, "MrnID", true, DataSourceUpdateMode.OnPropertyChanged);

            _mrnSite.DataSource = _component.MrnSiteChoices;
            _mrnSite.DataBindings.Add("Value", _component, "MrnSite", true, DataSourceUpdateMode.OnPropertyChanged);

            _healthcard.DataBindings.Add("Value", _component, "HealthcardID", true, DataSourceUpdateMode.OnPropertyChanged);

            _healthcardProvince.DataSource = _component.HealthcardProvinceChoices;
            _healthcardProvince.DataBindings.Add("Value", _component, "HealthcardProvince", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void PatientEditorControl_Load(object sender, EventArgs e)
        {
        }
    }
}
