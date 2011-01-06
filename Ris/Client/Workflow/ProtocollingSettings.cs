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
using System.Configuration;
using System.Xml;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;

namespace ClearCanvas.Ris.Client.Workflow
{
	[SettingsGroupDescription("Configures behaviour of the Protocolling component.")]
	[SettingsProvider(typeof(ClearCanvas.Common.Configuration.StandardSettingsProvider))]
	internal sealed partial class ProtocollingSettings
	{
		private XmlDocument _xmlDoc;
		private XmlNode _root;
		private readonly string _lastDefault = "__LastDefault";

		public ProtocollingSettings()
		{
			ApplicationSettingsRegistry.Instance.RegisterInstance(this);
		}

		public string GetDefaultProtocolGroup(string procedureName)
		{
			XmlElement element = (XmlElement)Root.SelectSingleNode(String.Format("procedure-protocolgroup-default[@procedureName='{0}']", procedureName));
			if(element != null)
			{
				return element.GetAttribute("protocolGroupName");
			}
			return null;
		}

		public void SetDefaultProtocolGroup(string protocolGroupName, string procedureName)
		{
			SetDefaultProtocolGroupHelper(protocolGroupName, procedureName);
			SetDefaultProtocolGroupHelper(protocolGroupName, _lastDefault);
			this.DefaultProtocolGroupsXml = _xmlDoc.OuterXml;
			this.Save();
		}

		public string LastDefaultProtocolGroup
		{
			get { return GetDefaultProtocolGroup(_lastDefault); }
		}

		private void SetDefaultProtocolGroupHelper(string protocolGroupName, string procedureName)
		{
			XmlElement element = (XmlElement)Root.SelectSingleNode(String.Format("procedure-protocolgroup-default[@procedureName='{0}']", procedureName));

			if (element == null)
			{
				element = this.GetXmlDocument().CreateElement("procedure-protocolgroup-default");
				element.SetAttribute("procedureName", procedureName);

				Root.AppendChild(element);
			}

			element.SetAttribute("protocolGroupName", protocolGroupName);
		}

		private XmlDocument GetXmlDocument()
		{
			if (_xmlDoc == null)
			{
				try
				{
					_xmlDoc = new XmlDocument();
					_xmlDoc.LoadXml(this.DefaultProtocolGroupsXml);
				}
				catch (Exception)
				{
					this.DefaultProtocolGroupsXml = @"<?xml version=""1.0"" encoding=""utf-8"" ?><procedure-protocolgroup-defaults></procedure-protocolgroup-defaults>";
					_xmlDoc.LoadXml(this.DefaultProtocolGroupsXml);
				}
			}

			return _xmlDoc;
		}

		private XmlNode Root
		{
			get
			{
				_root = _root ?? this.GetXmlDocument().SelectSingleNode("procedure-protocolgroup-defaults");

				if (_root == null)
				{
					// required element doesn't exist, so create it
					_root = this.GetXmlDocument().CreateElement("procedure-protocolgroup-defaults");
					this.GetXmlDocument().RemoveAll();
					this.GetXmlDocument().AppendChild(_root);
				}

				return _root;
			}
		}

		//public bool IsADefault(string protocolGroupName)
		//{
		//    return Root.SelectSingleNode(String.Format("procedure-protocolgroup-default[@protocolGroupName='{0}']", protocolGroupName)) != null;
		//}

		//internal IEnumerable<string> GetRankedDefaults()
		//{
		//    IDictionary<string, int> defaultProtocolGroups = new Dictionary<string, int>();

		//    foreach (XmlElement element in this.Root.ChildNodes)
		//    {
		//        string protocolGroup = element.GetAttribute("protocolGroupName");

		//        if(defaultProtocolGroups.ContainsKey(protocolGroup))
		//        {
		//            defaultProtocolGroups[protocolGroup]++;
		//        }
		//        else
		//        {
		//            defaultProtocolGroups[protocolGroup] = 1;
		//        }
		//    }

		//    List<KeyValuePair<string, int>> sortedDefaultProtocolGroups = CollectionUtils.Sort(
		//        defaultProtocolGroups, 
		//        delegate(KeyValuePair<string, int> x, KeyValuePair<string, int> y) { return x.Value.CompareTo(y.Value); });
		//    sortedDefaultProtocolGroups.Reverse();

		//    return CollectionUtils.Map<KeyValuePair<string, int>, string>(
		//        sortedDefaultProtocolGroups,
		//        delegate(KeyValuePair<string, int> defaultProtocolGroup) { return defaultProtocolGroup.Key; });
		//}
	}
}
