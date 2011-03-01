﻿#region License

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
using System.IO;

namespace ClearCanvas.Common.Configuration
{
	//TODO (CR February 2011) - Low: we should somehow make it clearer that critical settings
	//cannot have user scoped settings.

	/// <summary>
	/// Provides local persistence for application-critical settings.
	/// </summary>
	/// <remarks>
	/// Usage of this <see cref="SettingsProvider"/> allows for certain application settings to be
	/// designated application-critical and stored in a file separate from the standard application
	/// configuration file. Application-critical settings are designated by their presence in the
	/// critical settings file, and may not be modified through this provider (similar to how
	/// application scoped settings may not be modified through the <see cref="LocalFileSettingsProvider"/>.
	/// If a setting is not explicitly defined in the critical settings file, then the setting
	/// is treated as a standard application setting and resolved through the application and user
	/// configuration files.
	/// </remarks>
	public sealed class ApplicationCriticalSettingsProvider : LocalFileSettingsProvider
	{
		private readonly string _criticalSettingsPath;
		private const string _defaultConfigFilename = "critical.config";

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationCriticalSettingsProvider"/> class.
		/// </summary>
		public ApplicationCriticalSettingsProvider()
		{
			try
			{
				var baseAppConfig = SystemConfigurationHelper.GetExeConfiguration().FilePath;

#if DEBUG

				const string vshostExeConfigExtension = ".vshost.exe.config";
				if (baseAppConfig.EndsWith(vshostExeConfigExtension, StringComparison.InvariantCultureIgnoreCase))
					baseAppConfig = string.Format("{0}.exe.config", baseAppConfig.Substring(0, baseAppConfig.Length - vshostExeConfigExtension.Length));

#endif

				_criticalSettingsPath = Path.ChangeExtension(baseAppConfig, "critical.config");
			}
			catch (Exception)
			{
				// SystemConfigurationHelper.GetExeConfiguration() will throw exception when used in ASP.NET
				// Use the default filename instead.
				_criticalSettingsPath = Path.Combine(Platform.InstallDirectory, _defaultConfigFilename);
			}
		}

		/// <summary>
		/// Gets the path to the application-critical settings file for the current application.
		/// </summary>
		public string CriticalSettingsFilePath
		{
			get { return _criticalSettingsPath; }
		}

		private IDictionary<string, string> GetCriticalSettingsValues(Type settingsClassType)
		{
			// if the critical settings file doesn't exist or we couldn't figure out the path for some reason, fail silently
			if (string.IsNullOrEmpty(_criticalSettingsPath) || !File.Exists(_criticalSettingsPath))
				return new Dictionary<string, string>(0);

			var criticalConfiguration = SystemConfigurationHelper.GetExeConfiguration(_criticalSettingsPath);
			return SystemConfigurationHelper.GetSettingsValues(criticalConfiguration, settingsClassType);
		}

		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
		{
			var criticalSettings = GetCriticalSettingsValues((Type) context["SettingsClassType"]);
			var standardSettingsValues = base.GetPropertyValues(context, collection);

			var mergedSettingsValues = new SettingsPropertyValueCollection();
			foreach (SettingsProperty property in collection)
			{
				// if a setting is defined in the critical settings file, that value always takes precedence over the local application settings value
				var key = property.Name;
				if (criticalSettings.ContainsKey(key))
					mergedSettingsValues.Add(new SettingsPropertyValue(property) {SerializedValue = criticalSettings[key]});
				else
					mergedSettingsValues.Add(standardSettingsValues[key]);
			}
			return mergedSettingsValues;
		}

		public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
		{
			var criticalSettings = GetCriticalSettingsValues((Type) context["SettingsClassType"]);

			var standardSettingsValues = new SettingsPropertyValueCollection();
			foreach (SettingsPropertyValue property in collection)
			{
				// only allow modification of settings not in the critical settings file (which is only user-scoped settings not in the critical settings file)
				var key = property.Name;
				if (!criticalSettings.ContainsKey(key))
					standardSettingsValues.Add(property);
			}

			base.SetPropertyValues(context, standardSettingsValues);
		}
	}
}