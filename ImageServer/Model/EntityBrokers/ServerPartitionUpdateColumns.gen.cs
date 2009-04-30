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

namespace ClearCanvas.ImageServer.Model.EntityBrokers
{
    using System;
    using System.Xml;
    using ClearCanvas.ImageServer.Enterprise;

   public class ServerPartitionUpdateColumns : EntityUpdateColumns
   {
       public ServerPartitionUpdateColumns()
       : base("ServerPartition")
       {}
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="AcceptAnyDevice")]
        public Boolean AcceptAnyDevice
        {
            set { SubParameters["AcceptAnyDevice"] = new EntityUpdateColumn<Boolean>("AcceptAnyDevice", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="AeTitle")]
        public String AeTitle
        {
            set { SubParameters["AeTitle"] = new EntityUpdateColumn<String>("AeTitle", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="AuditDeleteStudy")]
        public Boolean AuditDeleteStudy
        {
            set { SubParameters["AuditDeleteStudy"] = new EntityUpdateColumn<Boolean>("AuditDeleteStudy", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="AutoInsertDevice")]
        public Boolean AutoInsertDevice
        {
            set { SubParameters["AutoInsertDevice"] = new EntityUpdateColumn<Boolean>("AutoInsertDevice", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="DefaultRemotePort")]
        public Int32 DefaultRemotePort
        {
            set { SubParameters["DefaultRemotePort"] = new EntityUpdateColumn<Int32>("DefaultRemotePort", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="Description")]
        public String Description
        {
            set { SubParameters["Description"] = new EntityUpdateColumn<String>("Description", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="DuplicateSopPolicyEnum")]
        public DuplicateSopPolicyEnum DuplicateSopPolicyEnum
        {
            set { SubParameters["DuplicateSopPolicyEnum"] = new EntityUpdateColumn<DuplicateSopPolicyEnum>("DuplicateSopPolicyEnum", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="Enabled")]
        public Boolean Enabled
        {
            set { SubParameters["Enabled"] = new EntityUpdateColumn<Boolean>("Enabled", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchAccessionNumber")]
        public Boolean MatchAccessionNumber
        {
            set { SubParameters["MatchAccessionNumber"] = new EntityUpdateColumn<Boolean>("MatchAccessionNumber", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchIssuerOfPatientId")]
        public Boolean MatchIssuerOfPatientId
        {
            set { SubParameters["MatchIssuerOfPatientId"] = new EntityUpdateColumn<Boolean>("MatchIssuerOfPatientId", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchPatientId")]
        public Boolean MatchPatientId
        {
            set { SubParameters["MatchPatientId"] = new EntityUpdateColumn<Boolean>("MatchPatientId", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchPatientsBirthDate")]
        public Boolean MatchPatientsBirthDate
        {
            set { SubParameters["MatchPatientsBirthDate"] = new EntityUpdateColumn<Boolean>("MatchPatientsBirthDate", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchPatientsName")]
        public Boolean MatchPatientsName
        {
            set { SubParameters["MatchPatientsName"] = new EntityUpdateColumn<Boolean>("MatchPatientsName", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchPatientsSex")]
        public Boolean MatchPatientsSex
        {
            set { SubParameters["MatchPatientsSex"] = new EntityUpdateColumn<Boolean>("MatchPatientsSex", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="PartitionFolder")]
        public String PartitionFolder
        {
            set { SubParameters["PartitionFolder"] = new EntityUpdateColumn<String>("PartitionFolder", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="Port")]
        public Int32 Port
        {
            set { SubParameters["Port"] = new EntityUpdateColumn<Int32>("Port", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="StudyCount")]
        public Int32 StudyCount
        {
            set { SubParameters["StudyCount"] = new EntityUpdateColumn<Int32>("StudyCount", value); }
        }
    }
}
