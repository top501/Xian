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
using ClearCanvas.Common;
using ClearCanvas.ImageServer.Common.Command;
using ClearCanvas.ImageServer.Model;

namespace ClearCanvas.ImageServer.Services.Archiving.Hsm
{
    /// <summary>
    /// Represents the execution context for an <see cref="ArchiveQueue"/>
    /// </summary>
    public class ArchiveProcessorContext : ServerExecutionContext
    {
        #region Private Fields
        private readonly Model.ArchiveQueue _item;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of <see cref="ArchiveProcessorContext"/>
        /// </summary>
        /// <param name="item"></param>
        public ArchiveProcessorContext(ArchiveQueue item):base(item.GetKey().Key.ToString())
        {
            Platform.CheckForNullReference(item, "item");
            _item = item;
        }
        #endregion

        #region Private Methods
        protected override string GetTemporaryPath()
        {
            StudyStorageLocation storage = StudyStorageLocation.FindStorageLocations(StudyStorage.Load(_item.StudyStorageKey))[0];
            if (storage == null)
                return base.GetTemporaryPath();
            else
            {
                String basePath =  GetTempPathRoot();

                if (String.IsNullOrEmpty(basePath))
                {
                    basePath = Path.Combine(storage.FilesystemPath, "temp");
                }
                String tempDirectory = Path.Combine(basePath, String.Format("ArchiveQueue-{0}", _item.GetKey()));

                for (int i = 2; i < 1000; i++)
                {
                    if (!Directory.Exists(tempDirectory))
                    {
                        break;
                    }

                    tempDirectory = Path.Combine(basePath, String.Format("ArchiveQueue-{0}({1})", _item.GetKey(), i));
                }

                if (!Directory.Exists(tempDirectory))
                    Directory.CreateDirectory(tempDirectory);
                return tempDirectory;
            }
        }
        #endregion
    }
}