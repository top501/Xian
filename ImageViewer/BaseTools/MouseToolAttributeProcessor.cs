#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.ImageViewer.InputManagement;

namespace ClearCanvas.ImageViewer.BaseTools
{
	internal sealed class MouseToolAttributeProcessor
	{
		private MouseToolAttributeProcessor() {}

		public static void Process(MouseImageViewerTool mouseTool)
		{
			Platform.CheckForNullReference(mouseTool, "mouseTool");

			FindMouseToolActivatorAction(mouseTool);
			InitializeMouseToolButton(mouseTool);
			InitializeModifiedMouseToolButton(mouseTool);
			InitializeMouseWheel(mouseTool);
		}

		private static void FindMouseToolActivatorAction(MouseImageViewerTool mouseTool)
		{
			object[] clickActionAttributes = mouseTool.GetType().GetCustomAttributes(typeof(ClickActionAttribute), true);
			if (clickActionAttributes == null || clickActionAttributes.Length == 0)
			{
				Platform.Log(LogLevel.Debug, "The tool type {0} does not define an action to select the tool.");
				return;
			}

			foreach (ClickActionAttribute attribute in clickActionAttributes)
			{
				if (attribute.ClickHandler == "Select")
					MouseToolSettingsProfile.Current.RegisterActivationActionId(attribute.QualifiedActionID(mouseTool), mouseTool.GetType());
			}
		}

		private static void InitializeMouseToolButton(MouseImageViewerTool mouseTool)
		{
			XMouseButtons mouseButton = XMouseButtons.Left;
			bool initiallyActive = false;

			// check for hardcoded assembly default settings
			object[] buttonAssignment = mouseTool.GetType().GetCustomAttributes(typeof (MouseToolButtonAttribute), true);
			if (buttonAssignment != null && buttonAssignment.Length > 0)
			{
				MouseToolButtonAttribute attribute = (MouseToolButtonAttribute) buttonAssignment[0];
				if (attribute.MouseButton == XMouseButtons.None)
					Platform.Log(LogLevel.Warn, String.Format(SR.FormatMouseToolInvalidAssignment, mouseTool.GetType().FullName));
				mouseButton = attribute.MouseButton;
				initiallyActive = attribute.InitiallyActive;
			}

			// check settings profile for an override specific to this tool
			Type mouseToolType = mouseTool.GetType();
			if (MouseToolSettingsProfile.Current.HasEntry(mouseToolType))
			{
				MouseToolSettingsProfile.Setting value = MouseToolSettingsProfile.Current[mouseToolType];
				mouseButton = value.MouseButton.GetValueOrDefault(mouseButton);
				initiallyActive = value.InitiallyActive.GetValueOrDefault(initiallyActive);
			}

			mouseTool.MouseButton = mouseButton;
			mouseTool.Active = initiallyActive;

			MouseToolSettingsProfile.Current[mouseToolType].MouseButton = mouseButton;
			MouseToolSettingsProfile.Current[mouseToolType].InitiallyActive = initiallyActive;
		}

		private static void InitializeModifiedMouseToolButton(MouseImageViewerTool mouseTool)
		{
			MouseButtonShortcut defaultMouseButtonShortcut = null;

			// check for hardcoded assembly default settings
			object[] defaultButtonAssignments = mouseTool.GetType().GetCustomAttributes(typeof (DefaultMouseToolButtonAttribute), true);
			if (defaultButtonAssignments != null && defaultButtonAssignments.Length > 0)
			{
				DefaultMouseToolButtonAttribute attribute = (DefaultMouseToolButtonAttribute) defaultButtonAssignments[0];
				defaultMouseButtonShortcut = attribute.Shortcut;
			}

			// check settings profile for an override specific to this tool
			Type mouseToolType = mouseTool.GetType();
			if (MouseToolSettingsProfile.Current.HasEntry(mouseToolType))
			{
				MouseToolSettingsProfile.Setting value = MouseToolSettingsProfile.Current[mouseToolType];
				if (value.DefaultMouseButton.HasValue)
				{
					defaultMouseButtonShortcut = null;
					if (value.DefaultMouseButton.Value != XMouseButtons.None)
						defaultMouseButtonShortcut = new MouseButtonShortcut(value.DefaultMouseButton.Value, value.DefaultMouseButtonModifiers.GetValueOrDefault(ModifierFlags.None));
				}
			}

			// apply the selected value to the tool (don't write back to the profile! - that's not for us to decide)
			try
			{
				mouseTool.DefaultMouseButtonShortcut = defaultMouseButtonShortcut;
			}
			catch (Exception e)
			{
				// JY: what exactly are we catching here?
				Platform.Log(LogLevel.Warn, e);
			}

			MouseToolSettingsProfile.Current[mouseToolType].DefaultMouseButton = defaultMouseButtonShortcut != null ? defaultMouseButtonShortcut.MouseButton : XMouseButtons.None;
			MouseToolSettingsProfile.Current[mouseToolType].DefaultMouseButtonModifiers = defaultMouseButtonShortcut != null ? defaultMouseButtonShortcut.Modifiers.ModifierFlags : ModifierFlags.None;
		}

		private static void InitializeMouseWheel(MouseImageViewerTool mouseTool)
		{
			object[] attributes = mouseTool.GetType().GetCustomAttributes(typeof(MouseWheelHandlerAttribute), false);
			if (attributes == null || attributes.Length == 0)
				return;

			MouseWheelHandlerAttribute attribute = (MouseWheelHandlerAttribute)attributes[0];
			mouseTool.MouseWheelShortcut = attribute.Shortcut;
		}
	}
}