#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tables;
using System;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Common.Utilities;
using System.Threading;

namespace ClearCanvas.ImageViewer.TestTools
{
	[ExtensionPoint]
	public sealed class PerformanceAnalysisComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	[AssociateView(typeof(PerformanceAnalysisComponentViewExtensionPoint))]
	public class PerformanceAnalysisComponent : ApplicationComponent
	{
		public class ReportItem
		{
			public ReportItem(string category, string identifier, int numberOfCalls, TimeSpan cumulativeTime)
			{
				Category = category;
				Identifier = identifier;
				NumberOfCalls = numberOfCalls;
				CumulativeTime = cumulativeTime;
			}

			public readonly string Category;
			public readonly string Identifier;
			public readonly int NumberOfCalls;
			public readonly TimeSpan CumulativeTime;

			public ReportItem Increment(TimeSpan seconds)
			{
				return new ReportItem(Category, Identifier, NumberOfCalls + 1, CumulativeTime + seconds);
			}
		}
		
		private Table<ReportItem> _reportTable;
		private SimpleActionModel _menuModel;
		private volatile SynchronizationContext _uiThreadContext;

		public PerformanceAnalysisComponent()
		{
		}

		public ITable Table
		{
			get { return _reportTable; }	
		}

		public ActionModelNode MenuModel
		{
			get { return _menuModel; }	
		}

		public override void Start()
		{
			base.Start();

			_uiThreadContext = SynchronizationContext.Current;

			_reportTable = new Table<ReportItem>();
			_reportTable.Columns.Add(new TableColumn<ReportItem, string>("Category", delegate(ReportItem item) { return item.Category; }));
			_reportTable.Columns.Add(new TableColumn<ReportItem, string>("Identifier", delegate(ReportItem item) { return item.Identifier; }));
			_reportTable.Columns.Add(new TableColumn<ReportItem, int>("#Calls", delegate(ReportItem item) { return item.NumberOfCalls; }));
			_reportTable.Columns.Add(new TableColumn<ReportItem, string>("Time", delegate(ReportItem item) { return item.CumulativeTime.ToString(); }));

			_menuModel = new SimpleActionModel(new ResourceResolver(this.GetType().Assembly));
			_menuModel.AddAction("reset", "Reset", null, null, Reset);

			PerformanceReportBroker.Report += OnReceivedReport;
		}

		public override void Stop()
		{
			PerformanceReportBroker.Report -= OnReceivedReport;
			_uiThreadContext = null;
			
			base.Stop();
		}

		public void Reset()
		{
			_reportTable.Items.Clear();
		}

		private void OnReceivedReport(object sender, ItemEventArgs<PerformanceReport> report)
		{
			if (_uiThreadContext == null)
				return;

			if (_uiThreadContext != SynchronizationContext.Current)
			{
				_uiThreadContext.Post(delegate { OnReceivedReport(sender, report); }, null);
				return;
			}

			ReportItem item = CollectionUtils.SelectFirst(_reportTable.Items, 
				delegate(ReportItem test)
					{
						return report.Item.Category == test.Category && test.Identifier == report.Item.Identifier;
					});

			if (item == null)
			{
				item = new ReportItem(report.Item.Category, report.Item.Identifier, 1, report.Item.TotalTime);
				_reportTable.Items.Add(item);
			}
			else 
			{
				int index = _reportTable.Items.IndexOf(item);
				ReportItem newItem = item.Increment(report.Item.TotalTime);
				_reportTable.Items[index] = newItem;
				_reportTable.Items.NotifyItemUpdated(index);
			}
		}
	}
}
