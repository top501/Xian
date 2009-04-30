﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
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

// This file is auto-generated by the ClearCanvas.Model.SqlServer2005.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model
{
    using System;
    using System.Collections.Generic;
    using ClearCanvas.ImageServer.Model.EntityBrokers;
    using ClearCanvas.ImageServer.Enterprise;
    using System.Reflection;

[Serializable]
public partial class ServiceLockTypeEnum : ServerEnum
{
      #region Private Static Members
      private static readonly ServiceLockTypeEnum _FilesystemDelete = GetEnum("FilesystemDelete");
      private static readonly ServiceLockTypeEnum _FilesystemReinventory = GetEnum("FilesystemReinventory");
      private static readonly ServiceLockTypeEnum _FilesystemStudyProcess = GetEnum("FilesystemStudyProcess");
      private static readonly ServiceLockTypeEnum _FilesystemLosslessCompress = GetEnum("FilesystemLosslessCompress");
      private static readonly ServiceLockTypeEnum _FilesystemLossyCompress = GetEnum("FilesystemLossyCompress");
      private static readonly ServiceLockTypeEnum _FilesystemRebuildXml = GetEnum("FilesystemRebuildXml");
      private static readonly ServiceLockTypeEnum _ArchiveApplicationLog = GetEnum("ArchiveApplicationLog");
      private static readonly ServiceLockTypeEnum _PurgeAlerts = GetEnum("PurgeAlerts");
      private static readonly ServiceLockTypeEnum _ImportFiles = GetEnum("ImportFiles");
      #endregion

      #region Public Static Properties
      /// <summary>
      /// This services checks if a filesystem is above its high watermark.  If the filesystem is above the high watermark it migrates studies, deletes studies, and purges studies until the low watermark is reached.
      /// </summary>
      public static ServiceLockTypeEnum FilesystemDelete
      {
          get { return _FilesystemDelete; }
      }
      /// <summary>
      /// This service re-inventories the studies stored on a filesystem.  It scans the contents of the filesystem, and if a study is not already stored in the database, it will insert records to process the study into the WorkQueue.
      /// </summary>
      public static ServiceLockTypeEnum FilesystemReinventory
      {
          get { return _FilesystemReinventory; }
      }
      /// <summary>
      /// This service scans the contents of a filesystem and reapplies Study Processing rules to all studies on the filesystem.
      /// </summary>
      public static ServiceLockTypeEnum FilesystemStudyProcess
      {
          get { return _FilesystemStudyProcess; }
      }
      /// <summary>
      /// This service checks for studies that are eligible to be lossless compressed on a filesystem.  It works independently from the watermarks configured for the filesystem and will insert records into the WorkQueue to compress the studies as soon as they are eligible.
      /// </summary>
      public static ServiceLockTypeEnum FilesystemLosslessCompress
      {
          get { return _FilesystemLosslessCompress; }
      }
      /// <summary>
      /// This service checks for studies that are eligible to be lossy compressed on a filesystem.  It works independently from the watermarks configured for the filesystem and will insert records into the WorkQueue to compress the studies as soon as they are eligible.
      /// </summary>
      public static ServiceLockTypeEnum FilesystemLossyCompress
      {
          get { return _FilesystemLossyCompress; }
      }
      /// <summary>
      /// Rebuild the Study XML file for each study stored on the Filesystem
      /// </summary>
      public static ServiceLockTypeEnum FilesystemRebuildXml
      {
          get { return _FilesystemRebuildXml; }
      }
      /// <summary>
      /// This service removes application log entries from the database and archives them in zip files to a filesystem.  When initially run, it selects a filesystem from the lowest filesystem tier configured on the system.
      /// </summary>
      public static ServiceLockTypeEnum ArchiveApplicationLog
      {
          get { return _ArchiveApplicationLog; }
      }
      /// <summary>
      /// This service by default removes Alert records from the database after a configurable time.  If configured it can save the alerts in zip files on a filesystem.  When initially run, it selects a filesystem from the lowest filesystem tier configured on the system to archive to.
      /// </summary>
      public static ServiceLockTypeEnum PurgeAlerts
      {
          get { return _PurgeAlerts; }
      }
      /// <summary>
      /// This service periodically scans the filesystem for dicom files and imports them into the system.
      /// </summary>
      public static ServiceLockTypeEnum ImportFiles
      {
          get { return _ImportFiles; }
      }

      #endregion

      #region Constructors
      public ServiceLockTypeEnum():base("ServiceLockTypeEnum")
      {}
      #endregion
      #region Public Members
      public override void SetEnum(short val)
      {
          ServerEnumHelper<ServiceLockTypeEnum, IServiceLockTypeEnumBroker>.SetEnum(this, val);
      }
      static public List<ServiceLockTypeEnum> GetAll()
      {
          return ServerEnumHelper<ServiceLockTypeEnum, IServiceLockTypeEnumBroker>.GetAll();
      }
      static public ServiceLockTypeEnum GetEnum(string lookup)
      {
          return ServerEnumHelper<ServiceLockTypeEnum, IServiceLockTypeEnumBroker>.GetEnum(lookup);
      }
      #endregion
}
}
