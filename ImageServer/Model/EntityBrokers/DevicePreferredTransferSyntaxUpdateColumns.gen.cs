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
    using ClearCanvas.ImageServer.Enterprise;

   public class DevicePreferredTransferSyntaxUpdateColumns : EntityUpdateColumns
   {
       public DevicePreferredTransferSyntaxUpdateColumns()
       : base("DevicePreferredTransferSyntax")
       {}
        [EntityFieldDatabaseMappingAttribute(TableName="DevicePreferredTransferSyntax", ColumnName="DeviceGUID")]
        public ServerEntityKey DeviceKey
        {
            set { SubParameters["DeviceKey"] = new EntityUpdateColumn<ServerEntityKey>("DeviceKey", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="DevicePreferredTransferSyntax", ColumnName="ServerSopClassGUID")]
        public ServerEntityKey ServerSopClassKey
        {
            set { SubParameters["ServerSopClassKey"] = new EntityUpdateColumn<ServerEntityKey>("ServerSopClassKey", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="DevicePreferredTransferSyntax", ColumnName="ServerTransferSyntaxGUID")]
        public ServerEntityKey ServerTransferSyntaxKey
        {
            set { SubParameters["ServerTransferSyntaxKey"] = new EntityUpdateColumn<ServerEntityKey>("ServerTransferSyntaxKey", value); }
        }
    }
}
