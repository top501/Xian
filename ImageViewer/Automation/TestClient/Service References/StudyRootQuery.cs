﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.832
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery
{
    using System.Runtime.Serialization;
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/contracts/query")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.SeriesIdentifier))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.ImageIdentifier))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.StudyIdentifier))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.StudyRootStudyIdentifier))]
    public partial class Identifier : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string InstanceAvailabilityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RetrieveAeTitleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SpecificCharacterSetField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string InstanceAvailability
        {
            get
            {
                return this.InstanceAvailabilityField;
            }
            set
            {
                this.InstanceAvailabilityField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RetrieveAeTitle
        {
            get
            {
                return this.RetrieveAeTitleField;
            }
            set
            {
                this.RetrieveAeTitleField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SpecificCharacterSet
        {
            get
            {
                return this.SpecificCharacterSetField;
            }
            set
            {
                this.SpecificCharacterSetField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/contracts/query")]
    [System.SerializableAttribute()]
    public partial class SeriesIdentifier : ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.Identifier
    {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ModalityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> NumberOfSeriesRelatedInstancesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SeriesDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SeriesDescriptionField;
        
        private string SeriesInstanceUidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SeriesNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SeriesTimeField;
        
        private string StudyInstanceUidField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Modality
        {
            get
            {
                return this.ModalityField;
            }
            set
            {
                this.ModalityField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> NumberOfSeriesRelatedInstances
        {
            get
            {
                return this.NumberOfSeriesRelatedInstancesField;
            }
            set
            {
                this.NumberOfSeriesRelatedInstancesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SeriesDate
        {
            get
            {
                return this.SeriesDateField;
            }
            set
            {
                this.SeriesDateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SeriesDescription
        {
            get
            {
                return this.SeriesDescriptionField;
            }
            set
            {
                this.SeriesDescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string SeriesInstanceUid
        {
            get
            {
                return this.SeriesInstanceUidField;
            }
            set
            {
                this.SeriesInstanceUidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SeriesNumber
        {
            get
            {
                return this.SeriesNumberField;
            }
            set
            {
                this.SeriesNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SeriesTime
        {
            get
            {
                return this.SeriesTimeField;
            }
            set
            {
                this.SeriesTimeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string StudyInstanceUid
        {
            get
            {
                return this.StudyInstanceUidField;
            }
            set
            {
                this.StudyInstanceUidField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/contracts/query")]
    [System.SerializableAttribute()]
    public partial class ImageIdentifier : ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.Identifier
    {
        
        private System.Nullable<int> InstanceNumberField;
        
        private string SeriesInstanceUidField;
        
        private string SopInstanceUidField;
        
        private string StudyInstanceUidField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public System.Nullable<int> InstanceNumber
        {
            get
            {
                return this.InstanceNumberField;
            }
            set
            {
                this.InstanceNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string SeriesInstanceUid
        {
            get
            {
                return this.SeriesInstanceUidField;
            }
            set
            {
                this.SeriesInstanceUidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string SopInstanceUid
        {
            get
            {
                return this.SopInstanceUidField;
            }
            set
            {
                this.SopInstanceUidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string StudyInstanceUid
        {
            get
            {
                return this.StudyInstanceUidField;
            }
            set
            {
                this.StudyInstanceUidField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/contracts/query")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.StudyRootStudyIdentifier))]
    public partial class StudyIdentifier : ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.Identifier
    {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccessionNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.ComponentModel.BindingList<string> ModalitiesInStudyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> NumberOfStudyRelatedInstancesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> NumberOfStudyRelatedSeriesField;
        
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
        public string AccessionNumber
        {
            get
            {
                return this.AccessionNumberField;
            }
            set
            {
                this.AccessionNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.ComponentModel.BindingList<string> ModalitiesInStudy
        {
            get
            {
                return this.ModalitiesInStudyField;
            }
            set
            {
                this.ModalitiesInStudyField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> NumberOfStudyRelatedInstances
        {
            get
            {
                return this.NumberOfStudyRelatedInstancesField;
            }
            set
            {
                this.NumberOfStudyRelatedInstancesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> NumberOfStudyRelatedSeries
        {
            get
            {
                return this.NumberOfStudyRelatedSeriesField;
            }
            set
            {
                this.NumberOfStudyRelatedSeriesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientId
        {
            get
            {
                return this.PatientIdField;
            }
            set
            {
                this.PatientIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsBirthDate
        {
            get
            {
                return this.PatientsBirthDateField;
            }
            set
            {
                this.PatientsBirthDateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsBirthTime
        {
            get
            {
                return this.PatientsBirthTimeField;
            }
            set
            {
                this.PatientsBirthTimeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsName
        {
            get
            {
                return this.PatientsNameField;
            }
            set
            {
                this.PatientsNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsSex
        {
            get
            {
                return this.PatientsSexField;
            }
            set
            {
                this.PatientsSexField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyDate
        {
            get
            {
                return this.StudyDateField;
            }
            set
            {
                this.StudyDateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyDescription
        {
            get
            {
                return this.StudyDescriptionField;
            }
            set
            {
                this.StudyDescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyId
        {
            get
            {
                return this.StudyIdField;
            }
            set
            {
                this.StudyIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string StudyInstanceUid
        {
            get
            {
                return this.StudyInstanceUidField;
            }
            set
            {
                this.StudyInstanceUidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyTime
        {
            get
            {
                return this.StudyTimeField;
            }
            set
            {
                this.StudyTimeField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/contracts/query")]
    [System.SerializableAttribute()]
    public partial class StudyRootStudyIdentifier : ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.StudyIdentifier
    {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/contracts/query")]
    [System.SerializableAttribute()]
    public partial class DataValidationFault : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/contracts/query")]
    [System.SerializableAttribute()]
    public partial class QueryFailedFault : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string DescriptionField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/contracts/query", ConfigurationName="ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.IStudyRootQuery", SessionMode=System.ServiceModel.SessionMode.NotAllowed)]
    public interface IStudyRootQuery
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/StudyQuery", ReplyAction="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/StudyQueryRespons" +
            "e")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.DataValidationFault), Action="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/StudyQueryDataVal" +
            "idationFaultFault", Name="DataValidationFault")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.QueryFailedFault), Action="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/StudyQueryQueryFa" +
            "iledFaultFault", Name="QueryFailedFault")]
        System.ComponentModel.BindingList<ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.StudyRootStudyIdentifier> StudyQuery(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.StudyRootStudyIdentifier queryQriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/SeriesQuery", ReplyAction="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/SeriesQueryRespon" +
            "se")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.QueryFailedFault), Action="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/SeriesQueryQueryF" +
            "ailedFaultFault", Name="QueryFailedFault")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.DataValidationFault), Action="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/SeriesQueryDataVa" +
            "lidationFaultFault", Name="DataValidationFault")]
        System.ComponentModel.BindingList<ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.SeriesIdentifier> SeriesQuery(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.SeriesIdentifier queryQriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/ImageQuery", ReplyAction="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/ImageQueryRespons" +
            "e")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.DataValidationFault), Action="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/ImageQueryDataVal" +
            "idationFaultFault", Name="DataValidationFault")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.QueryFailedFault), Action="http://www.clearcanvas.ca/dicom/contracts/query/IStudyRootQuery/ImageQueryQueryFa" +
            "iledFaultFault", Name="QueryFailedFault")]
        System.ComponentModel.BindingList<ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.ImageIdentifier> ImageQuery(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.ImageIdentifier queryQriteria);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IStudyRootQueryChannel : ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.IStudyRootQuery, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class StudyRootQueryClient : System.ServiceModel.ClientBase<ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.IStudyRootQuery>, ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.IStudyRootQuery
    {
        
        public StudyRootQueryClient()
        {
        }
        
        public StudyRootQueryClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
        
        public StudyRootQueryClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public StudyRootQueryClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public StudyRootQueryClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.ComponentModel.BindingList<ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.StudyRootStudyIdentifier> StudyQuery(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.StudyRootStudyIdentifier queryQriteria)
        {
            return base.Channel.StudyQuery(queryQriteria);
        }
        
        public System.ComponentModel.BindingList<ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.SeriesIdentifier> SeriesQuery(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.SeriesIdentifier queryQriteria)
        {
            return base.Channel.SeriesQuery(queryQriteria);
        }
        
        public System.ComponentModel.BindingList<ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.ImageIdentifier> ImageQuery(ClearCanvas.ImageViewer.Automation.TestClient.StudyRootQuery.ImageIdentifier queryQriteria)
        {
            return base.Channel.ImageQuery(queryQriteria);
        }
    }
}
