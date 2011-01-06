#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Security.Permissions;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Enterprise.Authentication;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Web.Application.Controls;
using ClearCanvas.ImageServer.Web.Application.Pages.Common;
using ClearCanvas.ImageServer.Web.Common.Data;

namespace ClearCanvas.ImageServer.Web.Application.Pages.Admin.Configure.PartitionArchive
{
    [PrincipalPermission(SecurityAction.Demand, Role = AuthorityTokens.Admin.Configuration.PartitionArchive)]
    public partial class Default : BasePage
    {
        #region Private Members
        // used for database interaction
        private PartitionArchiveConfigController _controller = new PartitionArchiveConfigController();

        #endregion

        #region Protected Methods

        protected void SetupEventHandlers()
        {
            AddEditPartitionDialog.OKClicked += AddEditPartitionDialog_OKClicked;
            DeleteConfirmDialog.Confirmed += DeleteConfirmDialog_Confirmed;
        }


        public void UpdateUI()
        {
			foreach (ServerPartition partition in ServerPartitionTabs.ServerPartitionList)
			{
				ServerPartitionTabs.Update(partition.Key);
			}
            UpdatePanel.Update();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SetupEventHandlers();

            ServerPartitionTabs.SetupLoadPartitionTabs(delegate(ServerPartition partition)
                                                           {
                                                               PartitionArchivePanel panel =
                                                                   LoadControl("PartitionArchivePanel.ascx") as PartitionArchivePanel;

                                                               if (panel != null)
                                                               {
                                                                   panel.ID = "PartitionArchivePanel_" +
                                                                              partition.AeTitle;
                                                                   panel.ServerPartition = partition;
                                                               }
                                                               return panel;
                                                           });

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UpdateUI();

            SetPageTitle(App_GlobalResources.Titles.PartitionArchivesPageTitle);
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddEditPartitionDialog_OKClicked(Model.PartitionArchive partition)
        {
            if (AddEditPartitionDialog.EditMode)
            {
                // Add partition into db and refresh the list
                if (_controller.UpdatePartition(partition))
                {
                    UpdateUI();
                }
            }
            else
            {
                // Add partition into db and refresh the list
                if (_controller.AddPartition(partition))
                {
                    UpdateUI();
                }
            }
        }

        private void DeleteConfirmDialog_Confirmed(object data)
        {
            ServerEntityKey key = data as ServerEntityKey;

            Model.PartitionArchive pa = Model.PartitionArchive.Load(key);

            _controller.Delete(pa);

            ServerPartitionTabs.Update(pa.ServerPartitionKey);
        }

        #endregion

        #region Public Methods

        public void AddPartition(ServerPartition partition)
        {
            // display the add dialog
            AddEditPartitionDialog.PartitionArchive = null;
            AddEditPartitionDialog.EditMode = false;
            AddEditPartitionDialog.Show(true);
			AddEditPartitionDialog.Partition = partition;
		}

        public void EditPartition(Model.PartitionArchive partitionArchive)
        {
            AddEditPartitionDialog.PartitionArchive = partitionArchive;
            AddEditPartitionDialog.EditMode = true;
            AddEditPartitionDialog.Show(true);
        	AddEditPartitionDialog.Partition = ServerPartition.Load(partitionArchive.ServerPartitionKey);
        }

        public void DeletePartition(Model.PartitionArchive partitionArchive)
        {
            DeleteConfirmDialog.Message = String.Format(
                    "Are you sure you want to delete partition archive \"{0}\" and all related settings permanently?", partitionArchive.Description);
            DeleteConfirmDialog.MessageType = MessageBox.MessageTypeEnum.YESNO;
            DeleteConfirmDialog.Data = partitionArchive.GetKey();
            DeleteConfirmDialog.Show();
        }

        #endregion
 
    }
}