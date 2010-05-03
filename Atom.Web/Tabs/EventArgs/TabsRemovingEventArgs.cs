using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;

namespace Atom.Web.UI.WebControls.Tabs
{
    [
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class TabsRemovingEventArgs : CancelEventArgs
    {
        public int TabIndex { get; set; }
        public Tab CurrentTab { get; set; }
        public JQTabs JQTabs { get; set; }

        public TabsRemovingEventArgs(int tabIndex, Tab currentTab, JQTabs jqTabs)
        {
            this.TabIndex = tabIndex;
            this.CurrentTab = currentTab;
            this.JQTabs = jqTabs;
        }
        public TabsRemovingEventArgs() { }
    }
}
