#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Collections.Generic;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common;

namespace ClearCanvas.Ris.Client
{
	/// <summary>
	/// A specialize Staff SelectorEditComponent that includes the current user.
	/// </summary>
	public class StaffSelectorEditorComponent : SelectorEditorComponent<StaffSummary, StaffSelectorTable>
	{
		public class DummyItem : StaffSummary
		{
			public DummyItem()
			{
				this.Name = new PersonNameDetail();
				this.Name.FamilyName = SR.DummyItemUser;
				this.StaffId = "";
				this.StaffType = new EnumValueInfo("", "");
				this.StaffRef = new EntityRef(typeof(DummyItem), new object(), 0);
			}
		}

		private static readonly StaffSummary _currentUserItem = new DummyItem();

		private static IEnumerable<StaffSummary> CollectionAndCurrentUser(IEnumerable<StaffSummary> items)
		{
			List<StaffSummary> a = new List<StaffSummary>();
			a.Add(_currentUserItem);
			a.AddRange(items);
			return a;
		}

		public StaffSelectorEditorComponent(IEnumerable<StaffSummary> allItems, IEnumerable<StaffSummary> selectedItems, bool includeCurrentUser)
			: base(
				CollectionAndCurrentUser(allItems),
				includeCurrentUser ? CollectionAndCurrentUser(selectedItems) : selectedItems, 
				delegate(StaffSummary s) { return s.StaffRef; })
		{
		}

		public bool IncludeCurrentUser
		{
			get { return base.SelectedItems.Contains(_currentUserItem); }
		}

		public override IList<StaffSummary> SelectedItems
		{
			get
			{
				return CollectionUtils.Select(base.SelectedItems, delegate(StaffSummary staff) { return staff != _currentUserItem; });
			}
		}
	}
}