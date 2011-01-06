#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Healthcare {


	/// <summary>
	/// AttachedDocument entity
	/// </summary>
	public partial class AttachedDocument : ClearCanvas.Enterprise.Core.Entity
	{
	
		/// <summary>
		/// This method is called from the constructor.  Use this method to implement any custom
		/// object initialization.
		/// </summary>
		private void CustomInitialize()
		{
		}

		public virtual DateTime? DocumentReceivedTime
		{
			get { return null; }
		}

		/// <summary>
		/// Marks this document as having been attached.
		/// </summary>
		public virtual void Attach()
		{
			return;
		}

		/// <summary>
		/// Marks this document as being detached.
		/// </summary>
		public virtual void Detach()
		{
			
		}

		/// <summary>
		/// Summary of derived-class specific details of the attached document
		/// </summary>
		public virtual IDictionary<string, string> DocumentHeaders
		{
			get { return null; }
		}

		public virtual string DocumentTypeName
		{
			get { return "Attached Document"; }
		}

		/// <summary>
		/// Shifts the object in time by the specified number of minutes, which may be negative or positive.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The method is not intended for production use, but is provided for the purpose
		/// of generating back-dated data for demos and load-testing.
		/// </para>
		/// </remarks>
		/// <param name="minutes"></param>
		public virtual void TimeShift(int minutes)
		{
			_creationTime = _creationTime.AddMinutes(minutes);
		}

		public static string BuildContentUrl(AttachedDocument document, string pathDelimiter)
		{
			var builder = new StringBuilder();
			builder.Append(document.CreationTime.Year.ToString());
			builder.Append(pathDelimiter);
			builder.Append(document.CreationTime.Month.ToString());
			builder.Append(pathDelimiter);
			builder.Append(document.CreationTime.Day.ToString());
			builder.Append(pathDelimiter);
			builder.AppendFormat("{0}.{1}", document.GetRef().ToString(false, false), document.FileExtension);
			return builder.ToString();
		}
	}
}