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
using System.Windows.Forms;
using ClearCanvas.Common;

namespace ClearCanvas.Desktop.View.WinForms
{
	[ClearCanvas.Common.ExtensionOf(typeof(ClearCanvas.Common.MessageBoxExtensionPoint))]
    [GuiToolkit(GuiToolkitID.WinForms)]
	public class MessageBox : IMessageBox
	{
        /// <summary>
        /// Maps <see cref="ClearCanvas.Common.MessageBoxButtons"/> values to 
        /// <see cref="System.Windows.Forms.MessageBoxButtons"/> values.
        /// </summary>
        private static Dictionary<int, System.Windows.Forms.MessageBoxButtons> _buttonMap;
        private static Dictionary<DialogResult, int> _resultMap;

        static MessageBox()
        {

            _buttonMap = new Dictionary<int, System.Windows.Forms.MessageBoxButtons>();
            _buttonMap.Add((int)MessageBoxActions.Ok, MessageBoxButtons.OK);
            _buttonMap.Add((int)MessageBoxActions.OkCancel, MessageBoxButtons.OKCancel);
            _buttonMap.Add((int)MessageBoxActions.YesNo, MessageBoxButtons.YesNo);
            _buttonMap.Add((int)MessageBoxActions.YesNoCancel, MessageBoxButtons.YesNoCancel);

            _resultMap = new Dictionary<DialogResult, int>();
            _resultMap.Add(DialogResult.OK, (int)DialogBoxAction.Ok);
            _resultMap.Add(DialogResult.Cancel, (int)DialogBoxAction.Cancel);
            _resultMap.Add(DialogResult.Yes, (int)DialogBoxAction.Yes);
            _resultMap.Add(DialogResult.No, (int)DialogBoxAction.No);
        }

        public MessageBox()
        {
            // better not assume that SWF exists on a non-windows platform
            if (!Platform.IsWin32Platform)
                throw new NotSupportedException();
        }

		public void Show(string message)
		{
            Show(message, MessageBoxActions.Ok);
		}

        public DialogBoxAction Show(string message, ClearCanvas.Common.MessageBoxActions buttons)
        {
            return Show(message, null, buttons, null);
        }

        internal DialogBoxAction Show(string message, string title, ClearCanvas.Common.MessageBoxActions buttons, IWin32Window owner)
        {
#if !MONO 
			// Ticket #7285:  messages boxes should not be obscured by the splashscreen
			// If no splash screen is being displayed, this is effectively a no-op.
			SplashScreenManager.DismissSplashScreen(owner as Form);
#endif 

            title = string.IsNullOrEmpty(title) ? Application.Name : string.Format("{0} - {1}", Application.Name, title);

        	using (var hook = new CommonDialogHook())
        	{
        		var mbButtons = _buttonMap[(int) buttons];

        		// The OK-only message box has a single button whose control ID is actually "Cancel"
        		if (mbButtons == MessageBoxButtons.OK)
        			hook.ButtonCaptions[2] = SR.ButtonOk;

        		var dr = System.Windows.Forms.MessageBox.Show(owner, message, title, mbButtons);
        		return (DialogBoxAction) _resultMap[dr];
        	}
        }
    }
}
