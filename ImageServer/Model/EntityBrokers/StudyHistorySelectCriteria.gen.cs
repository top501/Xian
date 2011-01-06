#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

// This file is auto-generated by the ClearCanvas.Model.SqlServer2005.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model.EntityBrokers
{
    using System;
    using System.Xml;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;

    public partial class StudyHistorySelectCriteria : EntitySelectCriteria
    {
        public StudyHistorySelectCriteria()
        : base("StudyHistory")
        {}
        public StudyHistorySelectCriteria(StudyHistorySelectCriteria other)
        : base(other)
        {}
        public override object Clone()
        {
            return new StudyHistorySelectCriteria(this);
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyHistory", ColumnName="InsertTime")]
        public ISearchCondition<DateTime> InsertTime
        {
            get
            {
              if (!SubCriteria.ContainsKey("InsertTime"))
              {
                 SubCriteria["InsertTime"] = new SearchCondition<DateTime>("InsertTime");
              }
              return (ISearchCondition<DateTime>)SubCriteria["InsertTime"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyHistory", ColumnName="StudyStorageGUID")]
        public ISearchCondition<ServerEntityKey> StudyStorageKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyStorageKey"))
              {
                 SubCriteria["StudyStorageKey"] = new SearchCondition<ServerEntityKey>("StudyStorageKey");
              }
              return (ISearchCondition<ServerEntityKey>)SubCriteria["StudyStorageKey"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyHistory", ColumnName="StudyHistoryTypeEnum")]
        public ISearchCondition<StudyHistoryTypeEnum> StudyHistoryTypeEnum
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyHistoryTypeEnum"))
              {
                 SubCriteria["StudyHistoryTypeEnum"] = new SearchCondition<StudyHistoryTypeEnum>("StudyHistoryTypeEnum");
              }
              return (ISearchCondition<StudyHistoryTypeEnum>)SubCriteria["StudyHistoryTypeEnum"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyHistory", ColumnName="StudyData")]
        public ISearchCondition<XmlDocument> StudyData
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyData"))
              {
                 SubCriteria["StudyData"] = new SearchCondition<XmlDocument>("StudyData");
              }
              return (ISearchCondition<XmlDocument>)SubCriteria["StudyData"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyHistory", ColumnName="ChangeDescription")]
        public ISearchCondition<XmlDocument> ChangeDescription
        {
            get
            {
              if (!SubCriteria.ContainsKey("ChangeDescription"))
              {
                 SubCriteria["ChangeDescription"] = new SearchCondition<XmlDocument>("ChangeDescription");
              }
              return (ISearchCondition<XmlDocument>)SubCriteria["ChangeDescription"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyHistory", ColumnName="DestStudyStorageGUID")]
        public ISearchCondition<ServerEntityKey> DestStudyStorageKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("DestStudyStorageKey"))
              {
                 SubCriteria["DestStudyStorageKey"] = new SearchCondition<ServerEntityKey>("DestStudyStorageKey");
              }
              return (ISearchCondition<ServerEntityKey>)SubCriteria["DestStudyStorageKey"];
            } 
        }
    }
}
