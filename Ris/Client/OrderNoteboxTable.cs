﻿#region License

// Copyright (c) 2010, ClearCanvas Inc.
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
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.OrderNotes;
using ClearCanvas.Ris.Client.Formatting;

namespace ClearCanvas.Ris.Client
{
	public class OrderNoteboxTable : Table<OrderNoteboxItemSummary>
	{
		private const int NumRows = 4;
		private readonly DateTimeTableColumn<OrderNoteboxItemSummary> _postTimeColumn;

		public OrderNoteboxTable()
			: this(NumRows)
		{
		}

		private OrderNoteboxTable(int cellRowCount)
			: base(cellRowCount)
		{
			var resolver = new ResourceResolver(this.GetType().Assembly);

			var urgentColumn = new TableColumn<OrderNoteboxItemSummary, IconSet>(SR.ColumnUrgent,
				item => item.Urgent ? new IconSet("SingleExclamation.png") : null, 0.3f)
				{
					Comparison = (item1, item2) => item1.Urgent.CompareTo(item2.Urgent),
					ResourceResolver = resolver
				};
			this.Columns.Add(urgentColumn);

			/* JR: this isn't needed right now, because acknowledged notes are never shown.
			TableColumn<OrderNoteboxItemSummary, IconSet> acknowledgedColumn =
				new TableColumn<OrderNoteboxItemSummary, IconSet>(SR.ColumnStatus,
					delegate(OrderNoteboxItemSummary item) { return GetIsAcknowledgedIcon(item.IsAcknowledged); },
					0.3f);
			acknowledgedColumn.Comparison = delegate(OrderNoteboxItemSummary item1, OrderNoteboxItemSummary item2)
				{ return item1.IsAcknowledged.CompareTo(item2.IsAcknowledged); };
			acknowledgedColumn.ResourceResolver = resolver;
			this.Columns.Add(acknowledgedColumn);
			 */

			this.Columns.Add(new TableColumn<OrderNoteboxItemSummary, string>(SR.ColumnMRN, item => MrnFormat.Format(item.Mrn), 1.0f));
			this.Columns.Add(new TableColumn<OrderNoteboxItemSummary, string>(SR.ColumnPatientName, item => PersonNameFormat.Format(item.PatientName), 1.0f));
			this.Columns.Add(new TableColumn<OrderNoteboxItemSummary, string>(SR.ColumnDescription, 
				item => string.Format("{0} {1}", AccessionFormat.Format(item.AccessionNumber), item.DiagnosticServiceName),
				1.0f, 1));

			this.Columns.Add(new TableColumn<OrderNoteboxItemSummary, string>(SR.ColumnFrom,
				item => item.OnBehalfOfGroup != null
					? String.Format(SR.FormatFromOnBehalf, StaffNameAndRoleFormat.Format(item.Author), item.OnBehalfOfGroup.Name, item.PostTime)
					: String.Format(SR.FormatFrom, StaffNameAndRoleFormat.Format(item.Author), item.PostTime),
				1.0f, 2));

			this.Columns.Add(new TableColumn<OrderNoteboxItemSummary, string>(SR.ColumnTo,
				item => String.Format(SR.FormatTo, RecipientsList(item.StaffRecipients, item.GroupRecipients)),
				1.0f, 3));

			this.Columns.Add(_postTimeColumn = new DateTimeTableColumn<OrderNoteboxItemSummary>(SR.ColumnPostTime,
				item => item.PostTime));
			_postTimeColumn.Visible = false;

			this.Sort(new TableSortParams(_postTimeColumn, false));
		}

		/* this isn't needed right now, because acknowledged notes are never shown.
		private static IconSet GetIsAcknowledgedIcon(bool isAcknowledged)
		{
			return isAcknowledged ? new IconSet("NoteRead.png") : new IconSet("NoteUnread.png");
		}
		*/

		// Creates a semi-colon delimited list of the recipients
		private static string RecipientsList(IEnumerable<StaffSummary> staffRecipients, IEnumerable<StaffGroupSummary> groupRecipients)
		{
			var sb = new StringBuilder();
			const string itemSeparator = ";  ";

			foreach (var staffSummary in staffRecipients)
			{
				if (String.Equals(PersonNameFormat.Format(staffSummary.Name), PersonNameFormat.Format(LoginSession.Current.FullName)))
				{
					sb.Insert(0, "me; ");
				}
				else
				{
					sb.Append(StaffNameAndRoleFormat.Format(staffSummary));
					sb.Append(itemSeparator);
				}
			}

			foreach (var groupSummary in groupRecipients)
			{
				sb.Append(groupSummary.Name);
				sb.Append(itemSeparator);
			}

			return sb.ToString().TrimEnd(itemSeparator.ToCharArray());
		}
	}
}