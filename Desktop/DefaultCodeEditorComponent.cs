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
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Desktop
{


    /// <summary>
    /// Extension point for views onto <see cref="DefaultCodeEditorComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class DefaultCodeEditorComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    /// <summary>
    /// A default code editor class component class.  This is a no frills editor and is typically
    /// used only when a better code editor does not exist in the installed plugin base.
    /// </summary>
    [AssociateView(typeof(DefaultCodeEditorComponentViewExtensionPoint))]
    public class DefaultCodeEditorComponent : ApplicationComponent, ICodeEditor
    {
        /// <summary>
        /// 
        /// </summary>
        public class InsertTextEventArgs : EventArgs
        {
            private string _text;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="text"></param>
            public InsertTextEventArgs(string text)
            {
                _text = text;
            }

            /// <summary>
            /// Text to insert.
            /// </summary>
            public string Text
            {
                get { return _text; }
            }
        }

        private string _text;
        private event EventHandler<InsertTextEventArgs> _insertTextRequested;


        /// <summary>
        /// Constructor
        /// </summary>
        internal DefaultCodeEditorComponent()
        {
        }

        #region ICodeEditor Members

        IApplicationComponent ICodeEditor.GetComponent()
        {
            return this;
        }

        string ICodeEditor.Text
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        void ICodeEditor.InsertText(string text)
        {
            EventsHelper.Fire(_insertTextRequested, this, new InsertTextEventArgs(text));
        }

        string ICodeEditor.Language
        {
            get { return null; }
            set { /* not supported */ }
        }

        bool ICodeEditor.Modified
        {
            get { return this.Modified; }
            set { this.Modified = value; }
        }

        event EventHandler ICodeEditor.ModifiedChanged
        {
            add { this.ModifiedChanged += value; }
            remove { this.ModifiedChanged -= value; }
        }

        #endregion

        #region overrides

        /// <summary>
        /// Starts the component.
        /// </summary>
        public override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Stops the component.
        /// </summary>
        public override void Stop()
        {
            base.Stop();
        }

        #endregion

        #region Presentation Model

        /// <summary>
        /// Notifies the view that it should insert the specified text at the current location.
        /// </summary>
        public event EventHandler<InsertTextEventArgs> InsertTextRequested
        {
            add { _insertTextRequested += value; }
            remove { _insertTextRequested -= value; }
        }

        /// <summary>
        /// Gets or sets the text that is displayed in the editor.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                this.Modified = true;
                NotifyPropertyChanged("Text");
            }
        }

        #endregion

    }
}
