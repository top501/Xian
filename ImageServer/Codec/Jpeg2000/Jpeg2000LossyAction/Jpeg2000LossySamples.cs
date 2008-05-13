#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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

using System.Collections.Generic;
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Rules;

namespace ClearCanvas.ImageServer.Codec.Jpeg2000.Jpeg2000LossyAction
{
	[ExtensionOf(typeof(SampleRuleExtensionPoint))]
	public class Jpeg2000LossySamples : ISampleRule
	{
		private readonly IList<ServerRuleApplyTimeEnum> _applyTime = new List<ServerRuleApplyTimeEnum>();

		public Jpeg2000LossySamples()
		{
			_applyTime.Add(ServerRuleApplyTimeEnum.GetEnum("CompressingStudy"));
		}
		public string Name
		{
			get { return "Jpeg2000LossyParameters"; }
		}
		public string Description
		{
			get { return "JPEG 2000 Lossy Parameters"; }
		}

		public ServerRuleTypeEnum Type
		{
			get { return ServerRuleTypeEnum.GetEnum("LossyCompressParameters"); }
		}

		public IList<ServerRuleApplyTimeEnum> ApplyTimeList
		{
			get { return _applyTime; }
		}

		public XmlDocument Rule
		{
			get
			{
				XmlDocument doc = new XmlDocument();
				XmlNode node = doc.CreateElement("rule");
				doc.AppendChild(node);
				XmlElement conditionNode = doc.CreateElement("condition");
				node.AppendChild(conditionNode);
				conditionNode.SetAttribute("expressionLanguage", "dicom");
				XmlNode actionNode = doc.CreateElement("action");
				node.AppendChild(actionNode);

				XmlElement orNode = doc.CreateElement("or");
				conditionNode.AppendChild(conditionNode);

				XmlElement equalNode = doc.CreateElement("equal");
				equalNode.SetAttribute("test", "$Modality");
				equalNode.SetAttribute("refValue", "DX");
				orNode.AppendChild(equalNode);

				equalNode = doc.CreateElement("equal");
				equalNode.SetAttribute("test", "$Modality");
				equalNode.SetAttribute("refValue", "CR");
				orNode.AppendChild(equalNode);

				XmlElement lossyCompress = doc.CreateElement("jpeg-2000-lossy");
				actionNode.AppendChild(lossyCompress);
				return doc;
			}
		}
	}
}
