using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Permissions;

namespace Atom.Web.UI.WebControls.Progressbar
{
    [
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class ProgressbarValueChangedEventArgs : EventArgs
    {
        public int Value
        {
            get
            {
                return this.Progressbar.Value;
            }
            set
            {
                this.Progressbar.Value = value;
            }
        }
        public JQProgressbar Progressbar { get; set; }

        public ProgressbarValueChangedEventArgs(int value, JQProgressbar progressbar)
        {
            this.Progressbar = progressbar;
            this.Value = value;
        }
    }
}
