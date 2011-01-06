#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections;
using System.Collections.Generic;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Validation;
using ClearCanvas.Ris.Application.Common;

namespace ClearCanvas.Ris.Client
{
	[ExtensionPoint]
	public class AddressesEditorComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	[AssociateView(typeof(AddressesEditorComponentViewExtensionPoint))]
	public class AddressEditorComponent : ApplicationComponent
	{
		private AddressDetail _address;
		private readonly List<EnumValueInfo> _addressTypes;
		private readonly bool _addressTypeEnabled;


		public AddressEditorComponent(AddressDetail address, List<EnumValueInfo> addressTypes)
		{
			_address = address;
			_addressTypes = addressTypes;
			_addressTypeEnabled = addressTypes.Count > 1;
		}

		/// <summary>
		/// Sets the subject upon which the editor acts
		/// Not for use by the view
		/// </summary>
		public AddressDetail Address
		{
			get { return _address; }
			set { _address = value; }
		}

		public override void Start()
		{
			this.Validation.Add(new ValidationRule("ValidUntil",
				delegate
				{
					// only need to validate the if both ValidFrom and ValidUntil are specified
					if (!_address.ValidRangeFrom.HasValue || !_address.ValidRangeUntil.HasValue)
						return new ValidationResult(true, "");

					var ok = DateTime.Compare(_address.ValidRangeUntil.Value, _address.ValidRangeFrom.Value) >= 0;
					return new ValidationResult(ok, SR.MessageValidUntilMustBeLaterOrEqualValidFrom);
				}));

			base.Start();
		}

		[ValidateNotNull]
		public string Street
		{
			get { return _address.Street; }
			set
			{
				_address.Street = value;
				this.Modified = true;
			}
		}

		public string Unit
		{
			get { return _address.Unit; }
			set
			{
				_address.Unit = value;
				this.Modified = true;
			}
		}

		[ValidateNotNull]
		public string City
		{
			get { return _address.City; }
			set
			{
				_address.City = value;
				this.Modified = true;
			}
		}

		[ValidateNotNull]
		public string Province
		{
			get { return _address.Province; }
			set
			{
				_address.Province = value;
				this.Modified = true;
			}
		}

		public ICollection<string> ProvinceChoices
		{
			get { return AddressEditorComponentSettings.Default.ProvinceChoices; }
		}

		public string Country
		{
			get { return _address.Country; }
			set
			{
				_address.Country = value;
				this.Modified = true;
			}
		}

		public ICollection<string> CountryChoices
		{
			get { return AddressEditorComponentSettings.Default.CountryChoices; }
		}

		public string PostalCode
		{
			get { return _address.PostalCode; }
			set
			{
				_address.PostalCode = value;
				this.Modified = true;
			}
		}

		public DateTime? ValidFrom
		{
			get { return _address.ValidRangeFrom; }
			set {
				_address.ValidRangeFrom = value == null ? value : value.Value.Date;
				this.Modified = true;
			}
		}

		public DateTime? ValidUntil
		{
			get { return _address.ValidRangeUntil; }
			set {
				_address.ValidRangeUntil = value == null ? value : value.Value.Date;
				this.Modified = true;
			}
		}

		public bool AddressTypeEnabled
		{
			get { return _addressTypeEnabled; }
		}

		public EnumValueInfo AddressType
		{
			get { return _address.Type; }
			set
			{
				_address.Type = value;
				this.Modified = true;
			}
		}

		public IList AddressTypeChoices
		{
			get { return _addressTypes; }
		}

		public void Accept()
		{
			if (this.HasValidationErrors)
			{
				this.ShowValidation(true);
				return;
			}

			this.ExitCode = ApplicationComponentExitCode.Accepted;
			Host.Exit();
		}

		public void Cancel()
		{
			this.ExitCode = ApplicationComponentExitCode.None;
			Host.Exit();
		}

		public bool AcceptEnabled
		{
			get { return this.Modified; }
		}

		public event EventHandler AcceptEnabledChanged
		{
			add { this.ModifiedChanged += value; }
			remove { this.ModifiedChanged -= value; }
		}

	}
}
