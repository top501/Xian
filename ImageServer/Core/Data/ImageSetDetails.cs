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

using System.Xml.Serialization;
using ClearCanvas.Dicom;

namespace ClearCanvas.ImageServer.Core.Data
{
	/// <summary>
	/// Represents the serializable detailed information of an image set.
	/// </summary>
	[XmlRoot("Details")]
	public class ImageSetDetails
	{
		#region Private Fields
		private StudyInformation _studyInfo=new StudyInformation();
		private int _sopCount;
		#endregion

		#region Constructors

		public ImageSetDetails()
		{
		}

		public ImageSetDetails(IDicomAttributeProvider attributeProvider)
		{
			StudyInfo = new StudyInformation(attributeProvider);
		}

		#endregion

		#region Public Properties

		public int SopInstanceCount
		{
			get { return _sopCount; }
			set { _sopCount = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="StudyInformation"/> of the image set.
		/// </summary>
		public StudyInformation StudyInfo
		{
			get{ return _studyInfo;}
			set { _studyInfo = value; }
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Inserts a <see cref="DicomMessageBase"/> into the set.
		/// </summary>
		/// <param name="message"></param>
		public void InsertFile(DicomMessageBase message)
		{
			StudyInfo.Add(message);
		}
		#endregion
	}
}