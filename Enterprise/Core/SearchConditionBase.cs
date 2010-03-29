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
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Enterprise.Core
{
	/// <summary>
	/// Defines the set of possible test conditions.
	/// </summary>
	public enum SearchConditionTest
	{
		None,
		Equal,
		Like,
		NotLike,
		Between,
		In,
		LessThan,
		LessThanOrEqual,
		MoreThan,
		MoreThanOrEqual,
		NotEqual,
		Null,
		NotNull,
		Exists,
		NotExists,
		NotIn,
	}

	/// <summary>
	/// Type-independent base class for the <see cref="SearchCondition{T}"/> and <see cref="RelatedEntityCondition{T}"/> classes.
	/// </summary>
	public abstract class SearchConditionBase : SearchCriteria
	{
		private object[] _values;
		private SearchConditionTest _test;
		private int _sortPosition;
		private bool _sortDirection;

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		protected SearchConditionBase()
			: this((string)null)
		{
		}

		/// <summary>
		/// Constructs a search condition with the specified key.
		/// </summary>
		/// <param name="key"></param>
		protected SearchConditionBase(string key)
			: base(key)
		{
			_test = SearchConditionTest.None;
			_sortPosition = -1;     // do not sort on this field
			_sortDirection = true;    // default sort direction to ascending
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="other"></param>
		protected SearchConditionBase(SearchConditionBase other)
			: base(other)
		{
			_values = other._values;
			_test = other._test;
			_sortPosition = other._sortPosition;
			_sortDirection = other._sortDirection;
		}

		#endregion

		#region Public API

		/// <summary>
		/// Specifies an ascending sort on the property associated with this condition.
		/// </summary>
		/// <param name="position"></param>
		public void SortAsc(int position)
		{
			_sortPosition = position;
			_sortDirection = true;
		}

		/// <summary>
		/// Specifies a descending sort on the property associated with this condition.
		/// </summary>
		/// <param name="position"></param>
		public void SortDesc(int position)
		{
			_sortPosition = position;
			_sortDirection = false;
		}

		/// <summary>
		/// Gets a value indicating if this criteria instance is empty, that is,
		/// it does not specify a condition.
		/// </summary>
		public override bool IsEmpty
		{
			get
			{
				return _test == SearchConditionTest.None && _sortPosition == -1;
			}
		}

		#endregion

		#region API for brokers that consume this criteria

		/// <summary>
		/// Gets the <see cref="SearchConditionTest"/> that this condition uses.
		/// </summary>
		public SearchConditionTest Test
		{
			get { return _test; }
		}

		/// <summary>
		/// Gets the set of values that this condition tests against.
		/// </summary>
		/// <remarks>
		/// The number of values in the set depends on the type test being performed.
		/// Most tests test agains a single value, but Between requires 2 values and
		/// In can work with any number of values.  Null and NotNull do not test against any values.
		/// </remarks>
		public object[] Values
		{
			get { return _values; }
		}

		/// <summary>
		/// Gets the relative priority of this sort directive, or -1 if this instance does not specify a sort directive.
		/// </summary>
		public int SortPosition
		{
			get { return _sortPosition; }
		}

		/// <summary>
		/// Gets the direction of this sort constraint (true for ascending, false for descending).
		/// </summary>
		public bool SortDirection
		{
			get { return _sortDirection; }
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Tests whether the specified value matches this condition.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected override bool IsSatisfiedBy(object value)
		{
			switch (_test)
			{
				case SearchConditionTest.Equal:
					return EqualTo(value, _values[0]);
				case SearchConditionTest.NotEqual:
					return NotEqualTo(value, _values[0]);
				case SearchConditionTest.Like:
					return Like((string)value, (string)_values[0]);
				case SearchConditionTest.NotLike:
					return NotLike((string)value, (string)_values[0]);
				case SearchConditionTest.Between:
					return Between(value, _values[0], _values[1]);
				case SearchConditionTest.In:
					return In(value, _values);
				case SearchConditionTest.NotIn:
					return NotIn(value, _values);
				case SearchConditionTest.LessThan:
					return LessThan(value, _values[0]);
				case SearchConditionTest.LessThanOrEqual:
					return LessThanOrEqual(value, _values[0]);
				case SearchConditionTest.MoreThan:
					return MoreThan(value, _values[0]);
				case SearchConditionTest.MoreThanOrEqual:
					return MoreThanOrEqual(value, _values[0]);
				case SearchConditionTest.NotNull:
					return IsNotNull(value);
				case SearchConditionTest.Null:
					return IsNull(value);
				default:
					throw new Exception();  // invalid
			}
		}

		/// <summary>
		/// Gets a text representation of the condition and sort information represented by this instance.
		/// </summary>
		/// <param name="prefix"></param>
		/// <returns></returns>
		protected override string[] Dump(string prefix)
		{
			var lines = new List<string>();
			if (_test != SearchConditionTest.None)
			{
				var condition = string.Format("{0}.{1}({2})", prefix, _test,
					StringUtilities.Combine(_values, ",", val => val.ToString()));
				lines.Add(condition);
			}
			if (_sortPosition > -1)
			{
				var sort = string.Format("{0}.{1}({2})", prefix, _sortDirection ? "SortAsc" : "SortDesc", _sortPosition);
				lines.Add(sort);
			}
			return lines.ToArray();
		}

		#endregion

		#region Protected API

		protected void SetCondition(SearchConditionTest test, params object[] values)
		{
			// do not set a condition if any value is null
			foreach (var val in values)
			{
				if (IsNullValue(val))
					throw new ArgumentNullException();
			}

			_test = test;
			_values = values;
		}

		protected static bool IsNullValue(object val)
		{
			return val == null || val.ToString().Length == 0;
		}

		#endregion


		#region Static Comparison functions

		internal static bool EqualTo(object variable, object value)
		{
			return Equals(variable, value);
		}

		internal static bool NotEqualTo(object variable, object value)
		{
			return !Equals(variable, value);
		}

		internal static bool Like(string variable, string value)
		{
			// TODO implement this
			throw new NotImplementedException();
		}

		internal static bool NotLike(string variable, string value)
		{
			// TODO implement this
			throw new NotImplementedException();
		}

		internal static bool Between(object variable, object lower, object upper)
		{
			var comparer = Comparer<object>.Default;
			return comparer.Compare(lower, variable) > -1 && comparer.Compare(variable, upper) == -1;
		}

		internal static bool In(object variable, object[] values)
		{
			return CollectionUtils.Contains(values, v => Equals(v, variable));
		}

		internal static bool NotIn(object variable, object[] values)
		{
			return !CollectionUtils.Contains(values, v => Equals(v, variable));
		}

		internal static bool LessThan(object variable, object value)
		{
			var comparer = Comparer<object>.Default;
			return comparer.Compare(value, variable) == -1;
		}

		internal static bool LessThanOrEqual(object variable, object value)
		{
			var comparer = Comparer<object>.Default;
			return comparer.Compare(value, variable) <= 0;
		}

		internal static bool MoreThan(object variable, object value)
		{
			var comparer = Comparer<object>.Default;
			return comparer.Compare(value, variable) == 1;
		}

		internal static bool MoreThanOrEqual(object variable, object value)
		{
			var comparer = Comparer<object>.Default;
			return comparer.Compare(value, variable) >= 0;
		}

		internal static bool IsNull(object variable)
		{
			return ReferenceEquals(variable, null);
		}

		internal static bool IsNotNull(object variable)
		{
			return !ReferenceEquals(variable, null);
		}

		#endregion
	}
}
