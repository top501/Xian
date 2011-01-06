#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

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
public partial class DeviceTypeEnum : ServerEnum
{
      #region Private Static Members
      private static readonly DeviceTypeEnum _Workstation = GetEnum("Workstation");
      private static readonly DeviceTypeEnum _Modality = GetEnum("Modality");
      private static readonly DeviceTypeEnum _Server = GetEnum("Server");
      private static readonly DeviceTypeEnum _Broker = GetEnum("Broker");
      private static readonly DeviceTypeEnum _PriorsServer = GetEnum("PriorsServer");
      #endregion

      #region Public Static Properties
      /// <summary>
      /// Workstation
      /// </summary>
      public static DeviceTypeEnum Workstation
      {
          get { return _Workstation; }
      }
      /// <summary>
      /// Modality
      /// </summary>
      public static DeviceTypeEnum Modality
      {
          get { return _Modality; }
      }
      /// <summary>
      /// Server
      /// </summary>
      public static DeviceTypeEnum Server
      {
          get { return _Server; }
      }
      /// <summary>
      /// Broker
      /// </summary>
      public static DeviceTypeEnum Broker
      {
          get { return _Broker; }
      }
      /// <summary>
      /// Server with Prior Studies for the Web Viewer
      /// </summary>
      public static DeviceTypeEnum PriorsServer
      {
          get { return _PriorsServer; }
      }

      #endregion

      #region Constructors
      public DeviceTypeEnum():base("DeviceTypeEnum")
      {}
      #endregion
      #region Public Members
      public override void SetEnum(short val)
      {
          ServerEnumHelper<DeviceTypeEnum, IDeviceTypeEnumBroker>.SetEnum(this, val);
      }
      static public List<DeviceTypeEnum> GetAll()
      {
          return ServerEnumHelper<DeviceTypeEnum, IDeviceTypeEnumBroker>.GetAll();
      }
      static public DeviceTypeEnum GetEnum(string lookup)
      {
          return ServerEnumHelper<DeviceTypeEnum, IDeviceTypeEnumBroker>.GetEnum(lookup);
      }
      #endregion
}
}
