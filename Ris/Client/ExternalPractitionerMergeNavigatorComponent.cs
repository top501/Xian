﻿using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Validation;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.Admin.ExternalPractitionerAdmin;

namespace ClearCanvas.Ris.Client
{
	public class ExternalPractitionerMergeNavigatorComponent : NavigatorComponentContainer
	{
		private ExternalPractitionerMergeSelectedDuplicateComponent _selectedDuplicateComponent;
		private ExternalPractitionerMergePropertiesComponent _mergePropertiesComponent;
		private ExternalPractitionerMergeSelectedContactPointsComponent _selectContactPointsComponent;
		private ExternalPractitionerMergeSelectDefaultContactPointComponent _selectDefaultContactPointComponent;
		private ExternalPractitionerMergeAffectedOrdersComponent _affectedOrdersComponent;
		private ExternalPractitionerOverviewComponent _confirmationComponent;

		private readonly EntityRef _originalPractitionerRef;
		private readonly ExternalPractitionerDetail _mergedPractitioner;
		private ExternalPractitionerDetail _originalPractitioner;
		private ExternalPractitionerDetail _selectedDuplicate;


		public ExternalPractitionerMergeNavigatorComponent(EntityRef practitionerRef)
		{
			_originalPractitionerRef = practitionerRef;
			_mergedPractitioner = new ExternalPractitionerDetail();
		}

		public override void Start()
		{
			this.Pages.Add(new NavigatorPage("Select Duplicate", _selectedDuplicateComponent = new ExternalPractitionerMergeSelectedDuplicateComponent()));
			this.Pages.Add(new NavigatorPage("Resolve Property Conflicts", _mergePropertiesComponent = new ExternalPractitionerMergePropertiesComponent()));
			this.Pages.Add(new NavigatorPage("Choose Contact Points", _selectContactPointsComponent = new ExternalPractitionerMergeSelectedContactPointsComponent()));
			this.Pages.Add(new NavigatorPage("Choose Default Contact Point", _selectDefaultContactPointComponent = new ExternalPractitionerMergeSelectDefaultContactPointComponent()));
			this.Pages.Add(new NavigatorPage("Resolve Order Conflicts", _affectedOrdersComponent = new ExternalPractitionerMergeAffectedOrdersComponent()));
			this.Pages.Add(new NavigatorPage("Confirmation", _confirmationComponent = new ExternalPractitionerOverviewComponent()));
			this.ValidationStrategy = new AllComponentsValidationStrategy();
			base.Start();
		}

		public override bool ShowTree
		{
			// Disable tree pane, so user can only navigate with the Forward and Backward buttons.
			get { return false; }
		}

		public override void Accept()
		{
			if (this.HasValidationErrors)
			{
				this.ShowValidation(true);
				return;
			}

			base.Accept();
		}


		protected override void MoveTo(int index)
		{
			var previousIndex = this.CurrentPageIndex;
			base.MoveTo(index);

			if (previousIndex < 0 || index > previousIndex)
				OnMovedForward();
			else
				OnMovedBackward();
		}

		private void OnMovedForward()
		{
			var currentComponent = this.CurrentPage.Component;
			if (currentComponent == _selectedDuplicateComponent)
			{
				_originalPractitioner = LoadPractitioner(_originalPractitionerRef);
				_selectedDuplicateComponent.PractitionerRef = _originalPractitioner.PractitionerRef;
				_mergePropertiesComponent.OriginalPractitioner = _originalPractitioner;
				_selectContactPointsComponent.OriginalPractitioner = _originalPractitioner;
			}
			else if (currentComponent == _mergePropertiesComponent)
			{
				// If selection change, load detail of the selected duplicate practitioner.
				if (_selectedDuplicateComponent.SelectedPractitioner == null)
					_selectedDuplicate = null;
				else if (_selectedDuplicate == null)
					_selectedDuplicate = LoadPractitioner(_selectedDuplicateComponent.SelectedPractitioner.PractitionerRef);
				else if (!_selectedDuplicate.PractitionerRef.Equals(_selectedDuplicateComponent.SelectedPractitioner.PractitionerRef, true))
					_selectedDuplicate = LoadPractitioner(_selectedDuplicateComponent.SelectedPractitioner.PractitionerRef);

				_mergePropertiesComponent.DuplicatePractitioner = _selectedDuplicate;
			}
			else if (currentComponent == _selectContactPointsComponent)
			{
				_selectContactPointsComponent.DuplicatePractitioner = _selectedDuplicate;
			}
			else if (currentComponent == _selectDefaultContactPointComponent)
			{
				// Update default contact point comopnent with the latest contact point selections
				_selectDefaultContactPointComponent.ContactPoints = _selectContactPointsComponent.SelectedContactPoints;
			}
			else if (currentComponent == _affectedOrdersComponent)
			{
				
			}
			else if (currentComponent == _confirmationComponent)
			{
				_mergedPractitioner.PractitionerRef = _originalPractitionerRef;
				_mergePropertiesComponent.Save(_mergedPractitioner);
				_selectContactPointsComponent.Save(_mergedPractitioner);
				_selectDefaultContactPointComponent.Save(_mergedPractitioner);
				_confirmationComponent.PractitionerDetail = _mergedPractitioner;
			}
		}

		private static void OnMovedBackward()
		{
			// Nothing to do when moving backward.  But fill-in this method if there is.
		}

		private static ExternalPractitionerDetail LoadPractitioner(EntityRef practitionerRef)
		{
			ExternalPractitionerDetail detail = null;

			if (practitionerRef != null)
			{
				Platform.GetService(
					delegate(IExternalPractitionerAdminService service)
					{
						var request = new LoadMergeExternalPractitionerFormDataRequest(practitionerRef) { IncludeDetail = true };
						var response = service.LoadMergeExternalPractitionerFormData(request);
						detail = response.PractitionerDetail;
					});
			}

			return detail;
		}
	}
}
