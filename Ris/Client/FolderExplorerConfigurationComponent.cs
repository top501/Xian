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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Configuration;
using ClearCanvas.Desktop.Trees;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.Ris.Client
{
	[ExtensionOf(typeof(ConfigurationPageProviderExtensionPoint))]
	public class FolderExplorerConfigurationPageProvider : IConfigurationPageProvider
	{
		#region IConfigurationPageProvider Members

		/// <summary>
		/// Gets all the <see cref="IConfigurationPage"/>s for this provider.
		/// </summary>
		public IEnumerable<IConfigurationPage> GetPages()
		{
			List<IConfigurationPage> listPages = new List<IConfigurationPage>();

			listPages.Add(new ConfigurationPage<FolderExplorerConfigurationComponent>(SR.TitleFolderExplorer));

			return listPages.AsReadOnly();
		}

		#endregion
	}

	/// <summary>
	/// Extension point for views onto <see cref="FolderExplorerConfigurationComponent"/>.
	/// </summary>
	[ExtensionPoint]
	public sealed class FolderExplorerConfigurationComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	/// <summary>
	/// FolderExplorerConfigurationComponent class.
	/// </summary>
	[AssociateView(typeof(FolderExplorerConfigurationComponentViewExtensionPoint))]
	public class FolderExplorerConfigurationComponent : ConfigurationApplicationComponent
	{
		private BindingList<FolderSystemConfigurationNode> _folderSystems;
		private int _selectedFolderSystemIndex;
		private SimpleActionModel _folderSystemsActionModel;
		private readonly Tree<DraggableTreeNode> _folderTree;
		private DraggableTreeNode _selectedFolderNode;
		private SimpleActionModel _foldersActionModel;

		private readonly string _moveFolderSystemUpKey = "MoveFolderSystemUp";
		private readonly string _moveFolderSystemDownKey = "MoveFolderSystemDown";
		private readonly string _addFolderKey = "AddFolder";
		private readonly string _editFolderKey = "EditFolder";
		private readonly string _deleteFolderKey = "DeleteFolder";
		private readonly string _moveFolderUpKey = "MoveFolderUp";
		private readonly string _moveFolderDownKey = "MoveFolderDown";

		private event EventHandler _onEditFolder;

		public FolderExplorerConfigurationComponent()
		{
			_folderTree = DraggableTreeNode.BuildTree();
		}

		public override void Start()
		{
			// establish default resource resolver on this assembly (not the assembly of the derived class)
			IResourceResolver resourceResolver = new ResourceResolver(typeof(DraggableTreeNode).Assembly);

			_folderSystemsActionModel = new SimpleActionModel(resourceResolver);
			_folderSystemsActionModel.AddAction(_moveFolderSystemUpKey, SR.TitleMoveUp, "Icons.UpToolSmall.png", SR.TitleMoveUp, MoveFolderSystemUp);
			_folderSystemsActionModel.AddAction(_moveFolderSystemDownKey, SR.TitleMoveDown, "Icons.DownToolSmall.png", SR.TitleMoveDown, MoveFolderSystemDown);
			_folderSystemsActionModel[_moveFolderSystemUpKey].Enabled = false;
			_folderSystemsActionModel[_moveFolderSystemDownKey].Enabled = false;

			_foldersActionModel = new SimpleActionModel(resourceResolver);
			_foldersActionModel.AddAction(_addFolderKey, SR.TitleAdd, "Icons.AddToolSmall.png", SR.TitleAddContainerFolder, AddFolder);
			_foldersActionModel.AddAction(_editFolderKey, SR.TitleEdit, "Icons.EditToolSmall.png", SR.TitleRenameFolder, EditFolder);
			_foldersActionModel.AddAction(_deleteFolderKey, SR.TitleDelete, "Icons.DeleteToolSmall.png", SR.TitleDeleteContainerFolder, DeleteFolder);
			_foldersActionModel.AddAction(_moveFolderUpKey, SR.TitleMoveUp, "Icons.UpToolSmall.png", SR.TitleMoveUp, MoveFolderUp);
			_foldersActionModel.AddAction(_moveFolderDownKey, SR.TitleMoveDown, "Icons.DownToolSmall.png", SR.TitleMoveDown, MoveFolderDown);
			_foldersActionModel[_addFolderKey].Enabled = false;
			_foldersActionModel[_editFolderKey].Enabled = false;
			_foldersActionModel[_deleteFolderKey].Enabled = false;
			_foldersActionModel[_moveFolderUpKey].Enabled = false;
			_foldersActionModel[_moveFolderDownKey].Enabled = false;

			LoadFolderSystems();

			base.Start();
		}

		#region IConfigurationApplicationComponent Members

		public override void Save()
		{
			// Save the ordering of the folder systems
			FolderExplorerComponentSettings.Default.SaveUserFolderSystemsOrder(
				CollectionUtils.Map<FolderSystemConfigurationNode, IFolderSystem>(
					_folderSystems, 
					delegate(FolderSystemConfigurationNode node) { return node.FolderSystem; }));

			CollectionUtils.ForEach(_folderTree.Items,
				delegate(DraggableTreeNode node)
					{
						FolderSystemConfigurationNode fsNode = (FolderSystemConfigurationNode) node;
						fsNode.UpdateFolderPath();
						FolderExplorerComponentSettings.Default.SaveUserFoldersCustomizations(fsNode.FolderSystem, fsNode.Folders);
					});

			FolderExplorerComponentSettings.Default.CompleteUserFolderSystemCustomizations();
		}
		
		#endregion

		#region Folder Systems Presentation Model

		public IBindingList FolderSystems
		{
			get { return _folderSystems; }
		}

		public string FormatFolderSystem(object item)
		{
			FolderSystemConfigurationNode node = (FolderSystemConfigurationNode)item;
			return node.Text;
		}

		public int SelectedFolderSystemIndex
		{
			get { return _selectedFolderSystemIndex; }
			set
			{
				_selectedFolderSystemIndex = value;
				UpdateFolderSystemActionModel();
				NotifyPropertyChanged("SelectedFolderSystemIndex");

				FolderSystemConfigurationNode folderSystemNode = _folderSystems[_selectedFolderSystemIndex];
				BuildFolderTreeIfNotExist(folderSystemNode);

				_folderTree.Items.Clear();
				_folderTree.Items.Add(folderSystemNode);
				this.SelectedFolderNode = new Selection(folderSystemNode);
			}
		}

		public void MoveFolderSystem(int index, int newIndex)
		{
			if (index >= 0 && newIndex >= 0 && index != newIndex)
			{
				FolderSystemConfigurationNode fs = _folderSystems[index];
				_folderSystems.RemoveAt(index);
				_folderSystems.Insert(newIndex, fs);

				this.Modified = true;
			}
		}

		/// <summary>
		/// Gets the folder systems action model.
		/// </summary>
		public ActionModelNode FolderSystemsActionModel
		{
			get { return _folderSystemsActionModel; }
		}

		#endregion

		#region Folders Presentation Model

		public ITree FolderTree
		{
			get { return _folderTree; }
		}

		public string SelectedFolderNodeText
		{
			get { return _selectedFolderNode.Text; }
			set { _selectedFolderNode.Text = value; }
		}

		public ISelection SelectedFolderNode
		{
			get { return new Selection(_selectedFolderNode); }
			set
			{
				DraggableTreeNode node = (DraggableTreeNode)value.Item;

				_selectedFolderNode = node;
				UpdateFolderActionModel();
				NotifyPropertyChanged("SelectedFolderNode");
			}
		}

		/// <summary>
		/// Gets the folders action model.
		/// </summary>
		public ActionModelNode FoldersActionModel
		{
			get { return _foldersActionModel; }
		}

		public event EventHandler OnEditFolder
		{
			add { _onEditFolder += value; }
			remove { _onEditFolder -= value; }
		}

		#endregion

		#region Folder Systems Helper

		private void LoadFolderSystems()
		{
			// Get a list of folder systems, initialize each of them so the folder list is populated
			List<IFolderSystem> folderSystems = CollectionUtils.Cast<IFolderSystem>(new FolderSystemExtensionPoint().CreateExtensions());
			CollectionUtils.ForEach(folderSystems,
				delegate(IFolderSystem fs) { fs.Initialize(); });

			List<IFolderSystem> remainder;
			FolderExplorerComponentSettings.Default.ApplyUserFolderSystemsOrder(folderSystems, out folderSystems, out remainder);
			// add the remainder to the end of the ordered list
			folderSystems.AddRange(remainder);

			IList<FolderSystemConfigurationNode> fsNodes = CollectionUtils.Map<IFolderSystem, FolderSystemConfigurationNode>(folderSystems,
				delegate(IFolderSystem fs) { return new FolderSystemConfigurationNode(fs); });

			CollectionUtils.ForEach(fsNodes,
				delegate(FolderSystemConfigurationNode node)
					{
						node.ModifiedChanged += delegate { this.Modified = true; };
					});

			_folderSystems = new BindingList<FolderSystemConfigurationNode>(fsNodes);

			this.SelectedFolderSystemIndex = _folderSystems.Count > 0 ? 0 : -1;
		}

		private void MoveFolderSystemUp()
		{
			if (_selectedFolderSystemIndex > 0)
			{
				int newIndex = _selectedFolderSystemIndex - 1;
				MoveFolderSystem(_selectedFolderSystemIndex, newIndex);
				this.SelectedFolderSystemIndex = newIndex;
				this.Modified = true;
			}
		}

		private void MoveFolderSystemDown()
		{
			if (_selectedFolderSystemIndex < _folderSystems.Count - 1)
            {
				int newIndex = _selectedFolderSystemIndex + 1;
				MoveFolderSystem(_selectedFolderSystemIndex, newIndex);
				this.SelectedFolderSystemIndex = newIndex;
				this.Modified = true;
			}
		}

		private void UpdateFolderSystemActionModel()
		{
			_folderSystemsActionModel[_moveFolderSystemUpKey].Enabled = _selectedFolderSystemIndex > 0;
			_folderSystemsActionModel[_moveFolderSystemDownKey].Enabled = _selectedFolderSystemIndex < _folderSystems.Count - 1;
		}

		#endregion

		#region Folders Helper

		private void BuildFolderTreeIfNotExist(FolderSystemConfigurationNode folderSystemNode)
		{
			if (folderSystemNode.SubTree != null)
				return;

			// put folders in correct insertion order from XML
			List<IFolder> orderedFolders;
			List<IFolder> remainderFolders;
			FolderExplorerComponentSettings.Default.ApplyUserFoldersCustomizations(folderSystemNode.FolderSystem, out orderedFolders, out remainderFolders);

			// add the remainder to the end of the ordered list
			orderedFolders.AddRange(remainderFolders);

			// add each ordered folder to the tree
			folderSystemNode.ClearSubTree();
			CollectionUtils.ForEach(orderedFolders,
				delegate(IFolder folder)
					{
						FolderConfigurationNode folderNode = new FolderConfigurationNode(folder);
						folderSystemNode.InsertNode(folderNode, folder.FolderPath);
					});
		}

		private void AddFolder()
		{
			DraggableTreeNode.ContainerNode newFolderNode = new DraggableTreeNode.ContainerNode("New Folder");
			_selectedFolderNode.AddChildNode(newFolderNode);
			this.SelectedFolderNode = new Selection(newFolderNode);
			EventsHelper.Fire(_onEditFolder, this, EventArgs.Empty);
		}

		private void EditFolder()
		{
			EventsHelper.Fire(_onEditFolder, this, EventArgs.Empty);
		}

		private void DeleteFolder()
		{
			DraggableTreeNode parentNode = _selectedFolderNode.Parent;
			DraggableTreeNode nextSelectedNode = parentNode.RemoveChildNode(_selectedFolderNode);
			this.SelectedFolderNode = new Selection(nextSelectedNode);
		}

		private void MoveFolderUp()
		{
			_selectedFolderNode.MoveUp();

			// Must update action model because the node index may have changed after moving node.
			UpdateFolderActionModel();
		}

		private void MoveFolderDown()
		{
			_selectedFolderNode.MoveDown();

			// Must update action model because the node index may have changed after moving node.
			UpdateFolderActionModel();
		}
		
		private void UpdateFolderActionModel()
		{
			_foldersActionModel[_addFolderKey].Enabled = _selectedFolderNode != null;
			_foldersActionModel[_editFolderKey].Enabled = _selectedFolderNode != null && _selectedFolderNode.CanEdit;
			_foldersActionModel[_deleteFolderKey].Enabled = _selectedFolderNode != null && _selectedFolderNode.CanDelete;
			_foldersActionModel[_moveFolderUpKey].Enabled = _selectedFolderNode != null && _selectedFolderNode.PreviousSibling != null;
			_foldersActionModel[_moveFolderDownKey].Enabled = _selectedFolderNode != null && _selectedFolderNode.NextSibling != null;
		}

		#endregion
	}
}
