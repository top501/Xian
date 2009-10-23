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
using System.Collections.Generic;
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Model.Parameters;

namespace ClearCanvas.ImageServer.Web.Common.Data.DataSource
{
	/// <summary>
	/// Summary view of a <see cref="WorkQueue"/> item in the WorkQueue configuration UI.
	/// </summary>
	/// <remarks>
	/// A <see cref="WorkQueueSummary"/> contains the summary of a <see cref="WorkQueue"/> and related information and is displayed
	/// in the WorkQueue configuration UI.
	/// </remarks>
	public class WorkQueueSummary
	{
		#region Private members

		private WorkQueue _theWorkQueueItem;

		#endregion Private members

		#region Public Properties

		public DateTime ScheduledDateTime
		{
			get { return _theWorkQueueItem.ScheduledTime; }
		}

		public string TypeString
		{
			get { return _theWorkQueueItem.WorkQueueTypeEnum.Description; }
		}

		public string StatusString
		{
			get { return _theWorkQueueItem.WorkQueueStatusEnum.Description; }
		}

		public string PriorityString
		{
			get { return _theWorkQueueItem.WorkQueuePriorityEnum.Description; }
		}

		public string PatientId { get; set; }

		public string PatientsName { get; set; }

		public ServerEntityKey Key
		{
			get { return _theWorkQueueItem.Key; }
		}

		public string Notes { get; set; }

		public string ProcessorID
		{
			get { return _theWorkQueueItem.ProcessorID; }
		}
		public WorkQueue TheWorkQueueItem
		{
			get { return _theWorkQueueItem; }
			set { _theWorkQueueItem = value; }
		}

		public ServerPartition ThePartition { get; set; }

		public bool RequiresAttention { get; set; }

		public string FullDescription { get; set; }

		#endregion Public Properties
	}

	/// <summary>
	/// Datasource for use with the ObjectDataSource to select a subset of results
	/// </summary>
	public class WorkQueueDataSource
	{
		#region Public Delegates
		public delegate void WorkQueueFoundSetDelegate(IList<WorkQueueSummary> list);

		public WorkQueueFoundSetDelegate WorkQueueFoundSet;
		#endregion

		#region Private Members
		private readonly WorkQueueController _searchController = new WorkQueueController();
		private int _resultCount;
		private IList<WorkQueueSummary> _list = new List<WorkQueueSummary>();

		#endregion

		#region Public Properties

		public string PatientId { get; set; }

		public string ScheduledDate { get; set; }

		public string PatientsName { get; set; }

		public string ProcessingServer { get; set; }

		public ServerPartition Partition { get; set; }

		public WorkQueueTypeEnum[] TypeEnums { get; set; }

		public WorkQueueStatusEnum[] StatusEnums { get; set; }

		public WorkQueuePriorityEnum PriorityEnum { get; set; }

		public string DateFormats { get; set; }

		public IList<WorkQueueSummary> List
		{
			get { return _list; }
		}

		public int ResultCount
		{
			get { return _resultCount; }
			set { _resultCount = value; }
		}

		public IList<ServerEntityKey> SearchKeys { get; set; }

		#endregion

		#region Private Methods
		private IList<WorkQueue> InternalSelect(int startRowIndex, int maximumRows, out int resultCount)
		{
			resultCount = 0;

			if (maximumRows == 0) return new List<WorkQueue>();

			if (SearchKeys != null)
			{
				IList<WorkQueue> workQueueList = new List<WorkQueue>();
				foreach (ServerEntityKey key in SearchKeys)
					workQueueList.Add(WorkQueue.Load(key));

				resultCount = workQueueList.Count;

				return workQueueList;
			}

			WebWorkQueueQueryParameters parameters = new WebWorkQueueQueryParameters
			                                         	{
			                                         		StartIndex = startRowIndex,
			                                         		MaxRowCount = maximumRows
			                                         	};

			if (Partition != null)
				parameters.ServerPartitionKey = Partition.Key;

			if (!string.IsNullOrEmpty(PatientsName))
			{
				string key = PatientsName.Replace("*", "%");
				key = key.Replace("?", "_");
				key = "%" + key + "%";
				parameters.PatientsName = key;
			}
			if (!string.IsNullOrEmpty(PatientId))
			{
				string key = PatientId.Replace("*", "%");
				key = key.Replace("?", "_");
				key = "%" + key + "%";
				parameters.PatientID = key;
			}
            if (!string.IsNullOrEmpty(ProcessingServer))
            {
                string key = ProcessingServer.Replace("*", "%");
                key = key.Replace("?", "_");
                key = "%" + key + "%";
                parameters.ProcessorID = key;
            }

			if (String.IsNullOrEmpty(ScheduledDate))
				parameters.ScheduledTime = null;
			else
				parameters.ScheduledTime = DateTime.ParseExact(ScheduledDate, DateFormats, null);

			if (TypeEnums != null && TypeEnums.Length > 0)
			{
				string types = "(";
				if (TypeEnums.Length == 1)
					types += TypeEnums[0].Enum;
				else {
					string separator = "";
					foreach (WorkQueueTypeEnum typeEnum in TypeEnums)
					{
						types += separator + typeEnum.Enum;
						separator = ",";
					}
				}
                
				parameters.Type = types + ")";
			}

			if (StatusEnums != null && StatusEnums.Length > 0)
			{
				string statuses = "(";
				if (StatusEnums.Length == 1)
					statuses += StatusEnums[0].Enum;
				else
				{
					string separator = "";
					foreach (WorkQueueStatusEnum statusEnum in StatusEnums)
					{
						statuses += separator + statusEnum.Enum;
						separator = ",";
					}
				}

				parameters.Status = statuses + ")";
			}

			if (PriorityEnum != null)
				parameters.Priority = PriorityEnum;

			IList<WorkQueue> list = _searchController.FindWorkQueue(parameters);

			resultCount = parameters.ResultCount;

			return list;
		}
		#endregion

		#region Public Methods
		public IEnumerable<WorkQueueSummary> Select(int startRowIndex, int maximumRows)
		{
			IList<WorkQueue> list = InternalSelect(startRowIndex, maximumRows, out _resultCount);

			_list = new List<WorkQueueSummary>();
            foreach (WorkQueue item in list)
            {
                if (item == null) break;
                _list.Add(CreateWorkQueueSummary(item));
            }

		    if (WorkQueueFoundSet != null)
				WorkQueueFoundSet(_list);

			return _list;
		}
		
		public int SelectCount()
		{
			if (ResultCount != 0) return ResultCount;

			// Ignore the search results
			InternalSelect(0, 1, out _resultCount);

			return ResultCount;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Constructs an instance of <see cref="WorkQueueSummary"/> based on a <see cref="WorkQueue"/> object.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		/// <remark>
		/// 
		/// </remark>
		private WorkQueueSummary CreateWorkQueueSummary(WorkQueue item)
		{
			WorkQueueSummary summary = new WorkQueueSummary
			                           	{
			                           		TheWorkQueueItem = item,
			                           		ThePartition = Partition
			                           	};

			// Fetch the patient info:
			StudyStorageAdaptor ssAdaptor = new StudyStorageAdaptor();
			StudyStorage storages = ssAdaptor.Get(item.StudyStorageKey);
			if (storages == null)
			{
				summary.PatientId = "N/A";
				summary.PatientsName = "N/A";
				return summary;
			}
			StudyAdaptor studyAdaptor = new StudyAdaptor();
			StudySelectCriteria studycriteria = new StudySelectCriteria();
			studycriteria.StudyInstanceUid.EqualTo(storages.StudyInstanceUid);
			studycriteria.ServerPartitionKey.EqualTo(item.ServerPartitionKey);
			IList<Study> studyList = studyAdaptor.Get(studycriteria);

			if (studyList == null || studyList.Count == 0)
			{
				summary.PatientId = "N/A";
				summary.PatientsName = "N/A";
			}
			else
			{
				summary.PatientId = studyList[0].PatientId;
				summary.PatientsName = studyList[0].PatientsName;
			}

			if (item.WorkQueueTypeEnum == WorkQueueTypeEnum.WebMoveStudy
			    || item.WorkQueueTypeEnum == WorkQueueTypeEnum.AutoRoute)
			{
				DeviceDataAdapter deviceAdaptor = new DeviceDataAdapter();
				Device dest = deviceAdaptor.Get(item.DeviceKey);

				summary.Notes = String.Format("Destination AE : {0}", dest.AeTitle);

				if (item.FailureDescription != null)
				{
					summary.FullDescription = String.Format("{0}, {1}", summary.Notes, item.FailureDescription);   //Set the FullDescription for the Tooltip in the GUI
					summary.Notes = summary.FullDescription.Length > 60 
						? summary.FullDescription.Substring(0, 60) 
						: summary.FullDescription;
				}
			}
			else if (item.FailureDescription != null)
			{
				// This used to only be shown when the status was "Failed" for a 
				// queue entry.  We now show it any time there's 
				if (item.FailureDescription.Length > 60)
				{
					summary.Notes = item.FailureDescription.Substring(0, 60);
					summary.Notes += " ...";
				    summary.FullDescription = item.FailureDescription;  //Set the FullDescription for the Tooltip in the GUI
				}
				else
					summary.Notes = item.FailureDescription;
			}

            summary.RequiresAttention = item.WorkQueueStatusEnum.Equals(WorkQueueStatusEnum.Failed) || !ServerPlatform.IsActiveWorkQueue(item);
			return summary;
		}
		#endregion
	}
}