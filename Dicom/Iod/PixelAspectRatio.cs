#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod
{
	/// <summary>
	/// Represents the pixel aspect ratio of an image.
	/// </summary>
	public class PixelAspectRatio : IEquatable<PixelAspectRatio>
	{
		#region Private Members
		
		int _row;
		int _column;
		
		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public PixelAspectRatio(int row, int column)
		{
			_row = row;
			_column = column;
		}

		/// <summary>
		/// Protected constructor.
		/// </summary>
		protected PixelAspectRatio()
		{
		}

		#region Public Properties

		/// <summary>
		/// Gets whether or not this object represents a null value.
		/// </summary>
		public bool IsNull
		{
			get { return _row == 0 || _column == 0; }
		}

		//TODO: change this to Vertical/Horizontal, as specified in DICOM PS3.3 C.7.6.3.1.7?

		/// <summary>
		/// Gets the row (vertical) component of the ratio.
		/// </summary>
		public virtual int Row
        {
            get { return _row; }
            protected set { _row = value; }
        }

		/// <summary>
		/// Gets the column (horizontal) component of the ratio.
		/// </summary>
        public virtual int Column
        {
            get { return _column; }
			protected set { _column = value; }
		}

		/// <summary>
		/// Gets the pixel aspect ratio as a floating point value, or zero if <see cref="IsNull"/> is true.
		/// </summary>
		/// <remarks>
		/// The aspect ratio of a pixel is defined as the ratio of it's vertical and horizontal
		/// size(s), or <see cref="Row"/> divided by <see cref="Column"/>.
		/// </remarks>
		public float Value
		{
			get
			{
				if (IsNull)
					return 0;

				return Row / (float)Column;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets a string suitable for direct insertion into a <see cref="DicomAttributeMultiValueText"/> attribute.
		/// </summary>
		public override string ToString()
		{
			return String.Format(@"{0}\{1}", _row, _column);
		}

		/// <summary>
		/// Creates a <see cref="PixelAspectRatio"/> object from a dicom multi-valued string.
		/// </summary>
		/// <returns>
		/// Null if there are not exactly 2 parsed values in the input string.
		/// </returns>
		public static PixelAspectRatio FromString(string multiValuedString)
		{
			int[] values;
			if (DicomStringHelper.TryGetIntArray(multiValuedString, out values) && values.Length == 2)
					return new PixelAspectRatio(values[0], values[1]);

			return null;
		}

		#region IEquatable<PixelAspectRatio> Members

		public bool Equals(PixelAspectRatio other)
		{
			if (other == null)
				return false;

			return _row == other._row && _column == other._column;
		}

		#endregion

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			return this.Equals(obj as PixelAspectRatio);
		}

		/// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
		}

		#endregion
	}
}
