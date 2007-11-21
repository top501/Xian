using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ClearCanvas.ImageServer.Model;
using System.Collections.Generic;

namespace ClearCanvas.ImageServer.Web.Application.Admin.Configuration.FileSystems
{
    /// <summary>
    /// Filesystem Filtering Panel.
    /// </summary>
    public partial class FileSystemsFilterPanel : System.Web.UI.UserControl
    {
        /// <summary>
        /// Used to store the filesystem filter settings.
        /// </summary>
        public class FilterSettings
        {
            #region private members

            private string _description;
            private FilesystemTierEnum _selectedTier;
            #endregion

            #region public properties
            /// <summary>
            /// The description filter
            /// </summary>
            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }
            /// <summary>
            ///  The filesystem tier filter
            /// </summary>
            public FilesystemTierEnum SelectedTier 
            {
                get { return _selectedTier; }
                set { _selectedTier = value; }
            }

           
            #endregion
        }

        #region Private members
        // list of filesystem tiers users can filter on
        private IList<FilesystemTierEnum> _tiers;
        #endregion Private members


        #region protected methods
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int prevSelectIndex = TiersDropDownList.SelectedIndex;
            TiersDropDownList.Items.Clear();
            TiersDropDownList.Items.Add(new ListItem("All"));
            foreach (FilesystemTierEnum tier in _tiers)
            {
                TiersDropDownList.Items.Add(new ListItem(tier.Description));
            }
            TiersDropDownList.SelectedIndex = prevSelectIndex; 
        }

        /// <summary>
        /// Handle user clicking the "Apply Filter" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FilterButton_Click(object sender, ImageClickEventArgs e)
        {
            if (ApplyFiltersClicked != null)
                ApplyFiltersClicked(Filters);
        }

        /// <summary>
        /// Determines if filters are being specified.
        /// </summary>
        /// <returns></returns>
        protected bool HasFilters()
        {
            //if (AETitleFilter.Text.Length > 0 || IPAddressFilter.Text.Length > 0 || EnabledOnlyFilter.Checked || DHCPOnlyFilter.Checked)
            //    return true;
            //else
            //    return false;

            return false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (Page.IsPostBack)
            {
                // Change the image of the "Apply Filter" button based on the filter settings
                if (HasFilters())
                    FilterButton.ImageUrl = "~/images/icons/QueryEnabled.png";
                else
                    FilterButton.ImageUrl = "~/images/icons/QueryEnabled.png";
            }
        }
        #endregion protected methods


        #region public members

        /// <summary>
        /// Remove all filter settings.
        /// </summary>
        public void Clear()
        {
            DescriptionFilter.Text = "";
            TiersDropDownList.SelectedIndex = -1;
            
        }
        #endregion

        #region public properties

        /// <summary>
        /// Retrieves the current filter settings.
        /// </summary>
        public FilterSettings Filters
        {
            get
            {
                FilterSettings settings = new FilterSettings();
                settings.Description = DescriptionFilter.Text;
                if (TiersDropDownList.SelectedIndex < 1)
                    settings.SelectedTier = null;
                else
                    settings.SelectedTier = this.Tiers[TiersDropDownList.SelectedIndex-1];

                return settings;
            }
        }

        /// <summary>
        /// Sets or gets the list of filesystems users can filter.
        /// </summary>
        public IList<FilesystemTierEnum> Tiers
        {
            get { return _tiers; }
            set
            {
                _tiers = value;

            }
        }

 

        #endregion // public properties

        #region Events
        /// <summary>
        /// Defines the event handler for <seealso cref="ApplyFiltersClicked"/>
        /// </summary>
        /// <param name="filters">The current settings.</param>
        /// <remarks>
        /// </remarks>
        public delegate void OnApplyFilterSettingsClickedEventHandler(FilterSettings filters);
        
        /// <summary>
        /// Occurs when the filter settings users click on "Apply" on the filter panel.
        /// </summary>
        /// <remarks>
        /// This event is fired on the server side.
        /// </remarks>
        public event OnApplyFilterSettingsClickedEventHandler ApplyFiltersClicked;
        #endregion // Events

       

    }

}

