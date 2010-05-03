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
    public class TabsChangingEventArgs : CancelEventArgs
    {
        public int NewIndex { get; set; }
        public int OldIndex { get; set; }
        public Tab CurrentTab { get; set; }
        public JQTabs JQTabs { get; set; }

        public TabsChangingEventArgs(int newIndex, int oldIndex, Tab currentTab, JQTabs jqTabs)
        {
            this.NewIndex = newIndex;
            this.OldIndex = oldIndex;
            this.CurrentTab = currentTab;
            this.JQTabs = jqTabs;
        }
    }
}
