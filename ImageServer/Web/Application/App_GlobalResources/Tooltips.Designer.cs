//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "10.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Tooltips {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Tooltips() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Tooltips", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Current Usage   : {0}%
        ///High Watermark : {1}%
        ///Low Watermark  : {2}%.
        /// </summary>
        internal static string AdminFilesystem_DiskUsage {
            get {
                return ResourceManager.GetString("AdminFilesystem_DiskUsage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Accept DICOM Associations from any device to this partition..
        /// </summary>
        internal static string AdminPartition_AddEditDialog_AcceptAnyDevice {
            get {
                return ResourceManager.GetString("AdminPartition_AddEditDialog_AcceptAnyDevice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enable/Disable always replacing duplicate reports with the latest received.  When enabled, the duplicate SOP policy is overridden. .
        /// </summary>
        internal static string AdminPartition_AddEditDialog_AcceptLatestReport {
            get {
                return ResourceManager.GetString("AdminPartition_AddEditDialog_AcceptLatestReport", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The DICOM Application Entity Title for the partition..
        /// </summary>
        internal static string AdminPartition_AddEditDialog_AETitle {
            get {
                return ResourceManager.GetString("AdminPartition_AddEditDialog_AETitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Automatically add devices when they connect to this partition..
        /// </summary>
        internal static string AdminPartition_AddEditDialog_AutoInsertDevices {
            get {
                return ResourceManager.GetString("AdminPartition_AddEditDialog_AutoInsertDevices", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A textual description of the partition..
        /// </summary>
        internal static string AdminPartition_AddEditDialog_Description {
            get {
                return ResourceManager.GetString("AdminPartition_AddEditDialog_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A policy for dealing with duplication DICOM objects received by the partition..
        /// </summary>
        internal static string AdminPartition_AddEditDialog_DuplicateObjectPolicy {
            get {
                return ResourceManager.GetString("AdminPartition_AddEditDialog_DuplicateObjectPolicy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A unique folder name to store images within for the partition. A folder with this name will be created in each filesystem..
        /// </summary>
        internal static string AdminPartition_AddEditDialog_PartitionFolderName {
            get {
                return ResourceManager.GetString("AdminPartition_AddEditDialog_PartitionFolderName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Default rule applied if no other rules of the type apply to a DICOM message/study..
        /// </summary>
        internal static string AdminRules_AddEditDialog_Default {
            get {
                return ResourceManager.GetString("AdminRules_AddEditDialog_Default", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enable/Disable the rule.
        /// </summary>
        internal static string AdminRules_AddEditDialog_Enabled {
            get {
                return ResourceManager.GetString("AdminRules_AddEditDialog_Enabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule that specifies DICOM messages or studies that are exempt from the rule..
        /// </summary>
        internal static string AdminRules_AddEditDialog_Exempt {
            get {
                return ResourceManager.GetString("AdminRules_AddEditDialog_Exempt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This device can send Accept Key/Object Presentation States objects to ImageServer.
        /// </summary>
        internal static string DeviceFeatures_AcceptKOPRFeature {
            get {
                return ResourceManager.GetString("DeviceFeatures_AcceptKOPRFeature", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ImageServer can auto-route images to this device.
        /// </summary>
        internal static string DeviceFeatures_AutoRoute {
            get {
                return ResourceManager.GetString("DeviceFeatures_AutoRoute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This device can query ImageServer.
        /// </summary>
        internal static string DeviceFeatures_Query {
            get {
                return ResourceManager.GetString("DeviceFeatures_Query", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This device can retrieve images from ImageServer.
        /// </summary>
        internal static string DeviceFeatures_Retrieve {
            get {
                return ResourceManager.GetString("DeviceFeatures_Retrieve", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This device can send DICOM images to ImageServer.
        /// </summary>
        internal static string DeviceFeatures_Store {
            get {
                return ResourceManager.GetString("DeviceFeatures_Store", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by Accession Number.
        /// </summary>
        internal static string SearchByAccessionNumber {
            get {
                return ResourceManager.GetString("SearchByAccessionNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by Description.
        /// </summary>
        internal static string SearchByAeDescription {
            get {
                return ResourceManager.GetString("SearchByAeDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search by Application Entity (AE) Title.
        /// </summary>
        internal static string SearchByAETitle {
            get {
                return ResourceManager.GetString("SearchByAETitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search by apply time.
        /// </summary>
        internal static string SearchByApplyTime {
            get {
                return ResourceManager.GetString("SearchByApplyTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by Study Description.
        /// </summary>
        internal static string SearchByDescription {
            get {
                return ResourceManager.GetString("SearchByDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search by hostname.
        /// </summary>
        internal static string SearchByHostname {
            get {
                return ResourceManager.GetString("SearchByHostname", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by IP Address.
        /// </summary>
        internal static string SearchByIpAddress {
            get {
                return ResourceManager.GetString("SearchByIpAddress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search by log content.
        /// </summary>
        internal static string SearchByLogContent {
            get {
                return ResourceManager.GetString("SearchByLogContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search by log date.
        /// </summary>
        internal static string SearchByLogDate {
            get {
                return ResourceManager.GetString("SearchByLogDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by Patient Id.
        /// </summary>
        internal static string SearchByPatientID {
            get {
                return ResourceManager.GetString("SearchByPatientID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by Patient Name.
        /// </summary>
        internal static string SearchByPatientName {
            get {
                return ResourceManager.GetString("SearchByPatientName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search studies deleted by particular users.
        /// </summary>
        internal static string SearchByPersonWhoDeletedStudies {
            get {
                return ResourceManager.GetString("SearchByPersonWhoDeletedStudies", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by processing server.
        /// </summary>
        internal static string SearchByProcessingServer {
            get {
                return ResourceManager.GetString("SearchByProcessingServer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by Referring Physician.
        /// </summary>
        internal static string SearchByRefPhysician {
            get {
                return ResourceManager.GetString("SearchByRefPhysician", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by organization (Responsible Organization).
        /// </summary>
        internal static string SearchByResponsibleOrganization {
            get {
                return ResourceManager.GetString("SearchByResponsibleOrganization", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by owner (Responsible Person).
        /// </summary>
        internal static string SearchByResponsiblePerson {
            get {
                return ResourceManager.GetString("SearchByResponsiblePerson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by scheduled date.
        /// </summary>
        internal static string SearchByScheduledDate {
            get {
                return ResourceManager.GetString("SearchByScheduledDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by status.
        /// </summary>
        internal static string SearchByStatus {
            get {
                return ResourceManager.GetString("SearchByStatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search the list by Study Date.
        /// </summary>
        internal static string SearchByStudyDate {
            get {
                return ResourceManager.GetString("SearchByStudyDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search by thread.
        /// </summary>
        internal static string SearchByThread {
            get {
                return ResourceManager.GetString("SearchByThread", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search by type.
        /// </summary>
        internal static string SearchByType {
            get {
                return ResourceManager.GetString("SearchByType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleted studies can be viewed and permanently deleted on the &quot;Deleted Studies&quot; page..
        /// </summary>
        internal static string ServerPartitionAddEditDialog_AuditDeleteStudy {
            get {
                return ResourceManager.GetString("ServerPartitionAddEditDialog_AuditDeleteStudy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Show name in other formats.
        /// </summary>
        internal static string ShowOtherNameFormats {
            get {
                return ResourceManager.GetString("ShowOtherNameFormats", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There does not seem to be any activity for this item. The server may not be running or there is a problem with this entry..
        /// </summary>
        internal static string WorkQueueIsStuck {
            get {
                return ResourceManager.GetString("WorkQueueIsStuck", resourceCulture);
            }
        }
    }
}
