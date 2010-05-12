using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;

namespace Atom.Web.UI.WebControls.DatePicker
{
    [
    AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    Designer(typeof(DatePickerDesigner)),
    ToolboxData("<{0}:JQDatePicker runat=\"server\"> </{0}:JQDatePicker>")
    ]
    public class JQDatePicker
    {
    }
}
