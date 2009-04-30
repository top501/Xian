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

using System;
using System.Runtime.Serialization;
using ClearCanvas.Enterprise.Common;
using System.Collections.Generic;

namespace ClearCanvas.Ris.Application.Common.ModalityWorkflow
{
    [DataContract]
    public class ModalityPerformedProcedureStepDetail : DataContractBase
    {
        public ModalityPerformedProcedureStepDetail(EntityRef modalityPerformendProcedureStepRef, string description, EnumValueInfo state, DateTime startTime, DateTime? endTime, StaffSummary performer, List<ProcedureStepSummary> modalityProcedureSteps, List<DicomSeriesDetail> dicomSeries, Dictionary<string, string> extendedProperties)
        {
            this.ModalityPerformendProcedureStepRef = modalityPerformendProcedureStepRef;
            this.Description = description;
            this.State = state;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Performer = performer;
            this.ModalityProcedureSteps = modalityProcedureSteps;
        	this.DicomSeries = dicomSeries;
            this.ExtendedProperties = extendedProperties;
        }

        [DataMember]
        public EntityRef ModalityPerformendProcedureStepRef;

        [DataMember]
        public string Description;

        [DataMember]
        public EnumValueInfo State;

        [DataMember]
        public DateTime StartTime;

        [DataMember]
        public DateTime? EndTime;

        [DataMember]
        public StaffSummary Performer;

        /// <summary>
        /// Modality procedure steps that were performed with this performed procedure step.
        /// </summary>
        [DataMember]
        public List<ProcedureStepSummary> ModalityProcedureSteps;

		[DataMember]
		public List<DicomSeriesDetail> DicomSeries;

        [DataMember]
        public Dictionary<string, string> ExtendedProperties;
    }
}