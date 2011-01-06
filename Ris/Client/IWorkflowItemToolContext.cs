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
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.Tools;

namespace ClearCanvas.Ris.Client
{
	/// <summary>
	/// Delegate used to determine if a double-click handler is enabled.
	/// </summary>
	/// <returns></returns>
	public delegate bool DoubleClickHandlerEnablementDelegate();

	/// <summary>
	/// Defines a base tool context for tools that operate on workflow items.
	/// </summary>
	public interface IWorkflowItemToolContext : IToolContext
	{
		/// <summary>
		/// Gets the desktop window.
		/// </summary>
		IDesktopWindow DesktopWindow { get; }

		/// <summary>
		/// Gets the currently selected folder.
		/// </summary>
		IFolder SelectedFolder { get; }

		/// <summary>
		/// Gets the current selection of items.
		/// </summary>
		ISelection Selection { get; }

		/// <summary>
		/// Occurs when <see cref="Selection"/> changes.
		/// </summary>
		event EventHandler SelectionChanged;

		/// <summary>
		/// Invalidates all folders. Use this method judiciously,
		/// as invalidating all folders will increase load on the system.
		/// </summary>
		void InvalidateFolders();

		/// <summary>
		/// Invalidates the currently selected folder.
		/// </summary>
		void InvalidateSelectedFolder();

		/// <summary>
		/// Invalidates all folders of the specified class.
		/// </summary>
		/// <param name="folderClass"></param>
		void InvalidateFolders(Type folderClass);


		/// <summary>
		/// Allows the tool to register itself as a double-click handler for items,
		/// regardless of which folder they are in.
		/// </summary>
		/// <remarks>
		/// More than one tool may register a double-click handler, but at most one will receive
		/// the notification.  The first handler whose enablement function returns true will receive the call. 
		/// </remarks>
		/// <param name="clickAction"></param>
		void RegisterDoubleClickHandler(IClickAction clickAction);

		/// <summary>
		/// Allows the tool to un-register itself as a double-click handler for items,
		/// regardless of which folder they are in.
		/// </summary>
		/// <remarks>
		/// If the tool is not registered as a double-click handler, nothing will happen.
		/// </remarks>
		void UnregisterDoubleClickHandler(IClickAction clickAction);

		/// <summary>
		/// Allows the tool to register a workflow service with the folder system.  When the selection changes,
		/// the folder system queries the operation enablement of all registered workflow services, and the
		/// results are available to the tool by calling <see cref="GetOperationEnablement(string)"/>.
		/// </summary>
		/// <param name="serviceContract"></param>
		void RegisterWorkflowService(Type serviceContract);

		/// <summary>
		/// Allows the tool to unregister a previously registered workflow service.
		/// </summary>
		/// <param name="serviceContract"></param>
		void UnregisterWorkflowService(Type serviceContract);

		/// <summary>
		/// Gets a value indicating whether the specified operation is enabled for the current items selection.
		/// </summary>
		/// <param name="serviceContract"></param>
		/// <param name="operationClass"></param>
		/// <returns></returns>
		bool GetOperationEnablement(Type serviceContract, string operationClass);

		/// <summary>
		/// Gets a value indicating whether the specified operation is enabled for the current items selection.
		/// </summary>
		/// <param name="operationClass"></param>
		/// <returns></returns>
		bool GetOperationEnablement(string operationClass);
	}

	/// <summary>
	/// Defines a base tool context for tools that operate on workflow items.
	/// </summary>
	public interface IWorkflowItemToolContext<TItem> : IWorkflowItemToolContext
	{
		/// <summary>
		/// Gets the currently selection items as a strongly typed collection.
		/// </summary>
		ICollection<TItem> SelectedItems { get; }

		/// <summary>
		/// Allows the tool to register a drag-drop handler on the specified folder class.
		/// </summary>
		/// <param name="folderClass"></param>
		/// <param name="dropHandler"></param>
		void RegisterDropHandler(Type folderClass, IDropHandler<TItem> dropHandler);

		/// <summary>
		/// Allows the tool to un-register a drag-drop handler on the specified folder class.
		/// </summary>
		/// <param name="folderClass"></param>
		/// <param name="dropHandler"></param>
		void UnregisterDropHandler(Type folderClass, IDropHandler<TItem> dropHandler);
	}
}
