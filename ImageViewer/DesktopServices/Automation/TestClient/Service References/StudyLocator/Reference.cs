﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Identifier", Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.SeriesIdentifier))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.ImageIdentifier))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyIdentifier))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier))]
    public partial class Identifier : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string InstanceAvailabilityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RetrieveAeTitleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SpecificCharacterSetField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string InstanceAvailability {
            get {
                return this.InstanceAvailabilityField;
            }
            set {
                if ((object.ReferenceEquals(this.InstanceAvailabilityField, value) != true)) {
                    this.InstanceAvailabilityField = value;
                    this.RaisePropertyChanged("InstanceAvailability");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RetrieveAeTitle {
            get {
                return this.RetrieveAeTitleField;
            }
            set {
                if ((object.ReferenceEquals(this.RetrieveAeTitleField, value) != true)) {
                    this.RetrieveAeTitleField = value;
                    this.RaisePropertyChanged("RetrieveAeTitle");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SpecificCharacterSet {
            get {
                return this.SpecificCharacterSetField;
            }
            set {
                if ((object.ReferenceEquals(this.SpecificCharacterSetField, value) != true)) {
                    this.SpecificCharacterSetField = value;
                    this.RaisePropertyChanged("SpecificCharacterSet");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SeriesIdentifier", Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    public partial class SeriesIdentifier : ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.Identifier {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ModalityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> NumberOfSeriesRelatedInstancesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SeriesDescriptionField;
        
        private string SeriesInstanceUidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> SeriesNumberField;
        
        private string StudyInstanceUidField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Modality {
            get {
                return this.ModalityField;
            }
            set {
                if ((object.ReferenceEquals(this.ModalityField, value) != true)) {
                    this.ModalityField = value;
                    this.RaisePropertyChanged("Modality");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> NumberOfSeriesRelatedInstances {
            get {
                return this.NumberOfSeriesRelatedInstancesField;
            }
            set {
                if ((this.NumberOfSeriesRelatedInstancesField.Equals(value) != true)) {
                    this.NumberOfSeriesRelatedInstancesField = value;
                    this.RaisePropertyChanged("NumberOfSeriesRelatedInstances");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SeriesDescription {
            get {
                return this.SeriesDescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.SeriesDescriptionField, value) != true)) {
                    this.SeriesDescriptionField = value;
                    this.RaisePropertyChanged("SeriesDescription");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string SeriesInstanceUid {
            get {
                return this.SeriesInstanceUidField;
            }
            set {
                if ((object.ReferenceEquals(this.SeriesInstanceUidField, value) != true)) {
                    this.SeriesInstanceUidField = value;
                    this.RaisePropertyChanged("SeriesInstanceUid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> SeriesNumber {
            get {
                return this.SeriesNumberField;
            }
            set {
                if ((this.SeriesNumberField.Equals(value) != true)) {
                    this.SeriesNumberField = value;
                    this.RaisePropertyChanged("SeriesNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string StudyInstanceUid {
            get {
                return this.StudyInstanceUidField;
            }
            set {
                if ((object.ReferenceEquals(this.StudyInstanceUidField, value) != true)) {
                    this.StudyInstanceUidField = value;
                    this.RaisePropertyChanged("StudyInstanceUid");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ImageIdentifier", Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    public partial class ImageIdentifier : ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.Identifier {
        
        private System.Nullable<int> InstanceNumberField;
        
        private string SeriesInstanceUidField;
        
        private string SopClassUidField;
        
        private string SopInstanceUidField;
        
        private string StudyInstanceUidField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public System.Nullable<int> InstanceNumber {
            get {
                return this.InstanceNumberField;
            }
            set {
                if ((this.InstanceNumberField.Equals(value) != true)) {
                    this.InstanceNumberField = value;
                    this.RaisePropertyChanged("InstanceNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string SeriesInstanceUid {
            get {
                return this.SeriesInstanceUidField;
            }
            set {
                if ((object.ReferenceEquals(this.SeriesInstanceUidField, value) != true)) {
                    this.SeriesInstanceUidField = value;
                    this.RaisePropertyChanged("SeriesInstanceUid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string SopClassUid {
            get {
                return this.SopClassUidField;
            }
            set {
                if ((object.ReferenceEquals(this.SopClassUidField, value) != true)) {
                    this.SopClassUidField = value;
                    this.RaisePropertyChanged("SopClassUid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string SopInstanceUid {
            get {
                return this.SopInstanceUidField;
            }
            set {
                if ((object.ReferenceEquals(this.SopInstanceUidField, value) != true)) {
                    this.SopInstanceUidField = value;
                    this.RaisePropertyChanged("SopInstanceUid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string StudyInstanceUid {
            get {
                return this.StudyInstanceUidField;
            }
            set {
                if ((object.ReferenceEquals(this.StudyInstanceUidField, value) != true)) {
                    this.StudyInstanceUidField = value;
                    this.RaisePropertyChanged("StudyInstanceUid");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StudyIdentifier", Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier))]
    public partial class StudyIdentifier : ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.Identifier {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccessionNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] ModalitiesInStudyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> NumberOfStudyRelatedInstancesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> NumberOfStudyRelatedSeriesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReferringPhysiciansNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StudyDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StudyDescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StudyIdField;
        
        private string StudyInstanceUidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StudyTimeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccessionNumber {
            get {
                return this.AccessionNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.AccessionNumberField, value) != true)) {
                    this.AccessionNumberField = value;
                    this.RaisePropertyChanged("AccessionNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] ModalitiesInStudy {
            get {
                return this.ModalitiesInStudyField;
            }
            set {
                if ((object.ReferenceEquals(this.ModalitiesInStudyField, value) != true)) {
                    this.ModalitiesInStudyField = value;
                    this.RaisePropertyChanged("ModalitiesInStudy");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> NumberOfStudyRelatedInstances {
            get {
                return this.NumberOfStudyRelatedInstancesField;
            }
            set {
                if ((this.NumberOfStudyRelatedInstancesField.Equals(value) != true)) {
                    this.NumberOfStudyRelatedInstancesField = value;
                    this.RaisePropertyChanged("NumberOfStudyRelatedInstances");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> NumberOfStudyRelatedSeries {
            get {
                return this.NumberOfStudyRelatedSeriesField;
            }
            set {
                if ((this.NumberOfStudyRelatedSeriesField.Equals(value) != true)) {
                    this.NumberOfStudyRelatedSeriesField = value;
                    this.RaisePropertyChanged("NumberOfStudyRelatedSeries");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReferringPhysiciansName {
            get {
                return this.ReferringPhysiciansNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ReferringPhysiciansNameField, value) != true)) {
                    this.ReferringPhysiciansNameField = value;
                    this.RaisePropertyChanged("ReferringPhysiciansName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyDate {
            get {
                return this.StudyDateField;
            }
            set {
                if ((object.ReferenceEquals(this.StudyDateField, value) != true)) {
                    this.StudyDateField = value;
                    this.RaisePropertyChanged("StudyDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyDescription {
            get {
                return this.StudyDescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.StudyDescriptionField, value) != true)) {
                    this.StudyDescriptionField = value;
                    this.RaisePropertyChanged("StudyDescription");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyId {
            get {
                return this.StudyIdField;
            }
            set {
                if ((object.ReferenceEquals(this.StudyIdField, value) != true)) {
                    this.StudyIdField = value;
                    this.RaisePropertyChanged("StudyId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string StudyInstanceUid {
            get {
                return this.StudyInstanceUidField;
            }
            set {
                if ((object.ReferenceEquals(this.StudyInstanceUidField, value) != true)) {
                    this.StudyInstanceUidField = value;
                    this.RaisePropertyChanged("StudyInstanceUid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyTime {
            get {
                return this.StudyTimeField;
            }
            set {
                if ((object.ReferenceEquals(this.StudyTimeField, value) != true)) {
                    this.StudyTimeField = value;
                    this.RaisePropertyChanged("StudyTime");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StudyRootStudyIdentifier", Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    public partial class StudyRootStudyIdentifier : ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyIdentifier {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PatientIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PatientsBirthDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PatientsBirthTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PatientsNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PatientsSexField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientId {
            get {
                return this.PatientIdField;
            }
            set {
                if ((object.ReferenceEquals(this.PatientIdField, value) != true)) {
                    this.PatientIdField = value;
                    this.RaisePropertyChanged("PatientId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsBirthDate {
            get {
                return this.PatientsBirthDateField;
            }
            set {
                if ((object.ReferenceEquals(this.PatientsBirthDateField, value) != true)) {
                    this.PatientsBirthDateField = value;
                    this.RaisePropertyChanged("PatientsBirthDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsBirthTime {
            get {
                return this.PatientsBirthTimeField;
            }
            set {
                if ((object.ReferenceEquals(this.PatientsBirthTimeField, value) != true)) {
                    this.PatientsBirthTimeField = value;
                    this.RaisePropertyChanged("PatientsBirthTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsName {
            get {
                return this.PatientsNameField;
            }
            set {
                if ((object.ReferenceEquals(this.PatientsNameField, value) != true)) {
                    this.PatientsNameField = value;
                    this.RaisePropertyChanged("PatientsName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsSex {
            get {
                return this.PatientsSexField;
            }
            set {
                if ((object.ReferenceEquals(this.PatientsSexField, value) != true)) {
                    this.PatientsSexField = value;
                    this.RaisePropertyChanged("PatientsSex");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DataValidationFault", Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    public partial class DataValidationFault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="QueryFailedFault", Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    public partial class QueryFailedFault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string DescriptionField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/query", ConfigurationName="StudyLocator.IStudyRootQuery")]
    public interface IStudyRootQuery {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/StudyQuery", ReplyAction="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/StudyQueryResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.DataValidationFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/StudyQueryDataValidationFau" +
            "ltFault", Name="DataValidationFault")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.QueryFailedFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/StudyQueryQueryFailedFaultF" +
            "ault", Name="QueryFailedFault")]
        ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier[] StudyQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier queryCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/SeriesQuery", ReplyAction="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/SeriesQueryResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.QueryFailedFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/SeriesQueryQueryFailedFault" +
            "Fault", Name="QueryFailedFault")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.DataValidationFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/SeriesQueryDataValidationFa" +
            "ultFault", Name="DataValidationFault")]
        ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.SeriesIdentifier[] SeriesQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.SeriesIdentifier queryCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/ImageQuery", ReplyAction="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/ImageQueryResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.QueryFailedFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/ImageQueryQueryFailedFaultF" +
            "ault", Name="QueryFailedFault")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.DataValidationFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/ImageQueryDataValidationFau" +
            "ltFault", Name="DataValidationFault")]
        ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.ImageIdentifier[] ImageQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.ImageIdentifier queryCriteria);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IStudyRootQueryChannel : ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.IStudyRootQuery, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class StudyRootQueryClient : System.ServiceModel.ClientBase<ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.IStudyRootQuery>, ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.IStudyRootQuery {
        
        public StudyRootQueryClient() {
        }
        
        public StudyRootQueryClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public StudyRootQueryClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StudyRootQueryClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StudyRootQueryClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier[] StudyQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier queryCriteria) {
            return base.Channel.StudyQuery(queryCriteria);
        }
        
        public ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.SeriesIdentifier[] SeriesQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.SeriesIdentifier queryCriteria) {
            return base.Channel.SeriesQuery(queryCriteria);
        }
        
        public ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.ImageIdentifier[] ImageQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.ImageIdentifier queryCriteria) {
            return base.Channel.ImageQuery(queryCriteria);
        }
    }
}
