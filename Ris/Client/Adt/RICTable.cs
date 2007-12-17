#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.RegistrationWorkflow;
using ClearCanvas.Common;
using ClearCanvas.Ris.Client.Formatting;

namespace ClearCanvas.Ris.Client.Adt
{
    public class RICTable : Table<RICSummary>
    {
        public RICTable()
        {
            this.Columns.Add(new TableColumn<RICSummary, string>(SR.ColumnRequestedProcedures,
                delegate(RICSummary item) { return item.RequestedProcedureName; }, 0.4f));
            this.Columns.Add(new TableColumn<RICSummary, string>(SR.ColumnScheduledFor,
                delegate(RICSummary item) { return (item.ScheduledTime == null ? SR.TextNotScheduled : FormatTime(item.ScheduledTime.Value)); }, 0.2f));
            this.Columns.Add(new TableColumn<RICSummary, string>(SR.ColumnStatus,
                delegate(RICSummary item) { return item.Status; }, 0.2f));
            this.Columns.Add(new TableColumn<RICSummary, string>(SR.ColumnInsurance,
                delegate(RICSummary item) { return item.Insurance; }, 0.1f));
            this.Columns.Add(new TableColumn<RICSummary, string>(SR.ColumnOrderingFacility,
                delegate(RICSummary item) { return item.OrderingFacility; }, 0.1f));
            this.Columns.Add(new TableColumn<RICSummary, string>(SR.ColumnOrderingPhysician,
                delegate(RICSummary item) { return PersonNameFormat.Format(item.OrderingPractitioner, "%F, %G"); }, 0.2f));
        }

        protected string FormatTime(DateTime time)
        {
            DateTime today = Platform.Time.Date;
            DateTime datePart = time.Date;

            if (datePart == today)
            {
                return String.Format(SR.TextTodayTime, Format.Time(time));
            }
            else if (datePart == today.AddDays(-1))
            {
                return SR.TextYesterday;
            }
            else if (datePart == today.AddDays(1))
            {
                return String.Format(SR.TextTomorrowTime, Format.Time(time));
            }
            else if (datePart.CompareTo(today) < 0)
            {
                TimeSpan ts = today.Subtract(datePart);
                return String.Format(SR.TextXDaysAgo, ts.Days);
            }

            return Format.DateTime(time);
        }
    }
}
