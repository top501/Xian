using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.View.WinForms;

namespace ClearCanvas.Ris.Client.Adt.View.WinForms
{
    /// <summary>
    /// Provides a Windows Forms view onto <see cref="VisitPractitionerSummaryComponent"/>
    /// </summary>
    [ExtensionOf(typeof(VisitPractitionerSummaryComponentViewExtensionPoint))]
    public class VisitPractitionersSummaryComponentView : WinFormsView, IApplicationComponentView
    {
        private VisitPractitionersSummaryComponent _component;
        private VisitPractitionersSummaryComponentControl _control;


        #region IApplicationComponentView Members

        public void SetComponent(IApplicationComponent component)
        {
            _component = (VisitPractitionersSummaryComponent)component;
        }

        #endregion

        public override object GuiElement
        {
            get
            {
                if (_control == null)
                {
                    _control = new VisitPractitionersSummaryComponentControl(_component);
                }
                return _control;
            }
        }
    }
}
