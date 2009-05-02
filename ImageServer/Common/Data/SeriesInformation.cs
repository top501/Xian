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

using System.Xml.Serialization;
using ClearCanvas.Dicom;

namespace ClearCanvas.ImageServer.Common.Data
{
    /// <summary>
    /// Represents serializable series information.
    /// </summary>
    [XmlRoot("Series")]
    public class SeriesInformation
    {
        #region Private Members
        private string _seriesInstanceUid;
        private string _seriesDescription;
        private string _modality;
        private int _numberOfInstances = 0;
        #endregion

        #region Constructors

        public SeriesInformation()
        {
        }

        public SeriesInformation(IDicomAttributeProvider attributeProvider)
        {
            if (attributeProvider[DicomTags.SeriesInstanceUid] != null)
                SeriesInstanceUid = attributeProvider[DicomTags.SeriesInstanceUid].ToString();
            if (attributeProvider[DicomTags.SeriesDescription] != null)
                SeriesDescription = attributeProvider[DicomTags.SeriesDescription].ToString();
            if (attributeProvider[DicomTags.Modality] != null)
                Modality = attributeProvider[DicomTags.Modality].ToString();

        }

        #endregion

        #region Public Properties
        [XmlAttribute]
        public string SeriesInstanceUid
        {
            get { return _seriesInstanceUid; }
            set { _seriesInstanceUid = value; }
        }
        [XmlAttribute]
        public string Modality
        {
            get { return _modality; }
            set { _modality = value; }
        }
        public string SeriesDescription
        {
            get { return _seriesDescription; }
            set { _seriesDescription = value; }
        }

        public int NumberOfInstances
        {
            get { return _numberOfInstances; }
            set { _numberOfInstances = value; }
        }

        #endregion

    }
}