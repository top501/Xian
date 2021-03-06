#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.IO;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom.Utilities.Command;
using ClearCanvas.ImageServer.Core.Reconcile.CreateStudy;
using ClearCanvas.ImageServer.Model;

namespace ClearCanvas.ImageServer.Core.Reconcile
{
    /// <summary>
    /// Command to save the <see cref="UidMapper"/> used in the reconciliation.
    /// </summary>
    public class SaveUidMapXmlCommand : CommandBase, IDisposable
    {
        #region Private Members
        private readonly UidMapper _map;
        private readonly StudyStorageLocation _studyLocation;
        private string _path;
        private string _backupPath; 
        #endregion

        #region Constructors

        public SaveUidMapXmlCommand(StudyStorageLocation studyLocation, UidMapper mapper) :
            base("SaveUidMap", true)
        {
            _studyLocation = studyLocation;
            _map = mapper;
        } 
        #endregion

        #region Overridden Protected Methods
        protected override void OnExecute(CommandProcessor theProcessor)
        {
            if (_map == null)
                return;// nothing to save

            _path = Path.Combine(_studyLocation.GetStudyPath(), "UidMap.xml");
            if (RequiresRollback)
                Backup();

            _map.Save(_path);
        }

        protected override void OnUndo()
        {
            if (File.Exists(_backupPath))
            {
                File.Copy(_backupPath, _path, true);
            }
        } 
        #endregion

        #region Private Methods

        private void Backup()
        {
            _backupPath = FileUtils.Backup(_path, ProcessorContext.BackupDirectory);
        }
        
        #endregion

        #region Public Methods
        public void Dispose()
        {
            if (File.Exists(_backupPath))
            {
                File.Delete(_backupPath);
            }
        } 
        #endregion
    }
}