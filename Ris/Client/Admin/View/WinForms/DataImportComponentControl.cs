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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using ClearCanvas.Desktop.View.WinForms;

namespace ClearCanvas.Ris.Client.Admin.View.WinForms
{
    /// <summary>
    /// Provides a Windows Forms user-interface for <see cref="ImportDiagnosticServicesComponent"/>
    /// </summary>
    public partial class DataImportComponentControl : ApplicationComponentUserControl
    {
        private DataImportComponent _component;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataImportComponentControl(DataImportComponent component)
            : base(component)
        {
            InitializeComponent();
            _component = component;

            _importer.DataSource = _component.ImportTypeChoices;
            _importer.DataBindings.Add("Value", _component, "ImportType", true, DataSourceUpdateMode.OnPropertyChanged);
            _dataFile.DataBindings.Add("Text", _component, "FileName", true, DataSourceUpdateMode.OnPropertyChanged);
            //_batchSize.DataBindings.Add("Value", _component, "BatchSize", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void _browseButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = ".";
            openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _dataFile.Text = openFileDialog1.FileName;
            }
        }

        private void _startButton_Click(object sender, EventArgs e)
        {
            _component.StartImport();
        }
    }
}
