using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.Design;

namespace Atom.Web.UI.WebControls.DatePicker
{
    class DatePickerDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            return base.CreatePlaceHolderDesignTimeHtml();
        }
    }
}
