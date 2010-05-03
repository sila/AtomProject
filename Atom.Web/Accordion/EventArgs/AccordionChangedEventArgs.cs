using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Permissions;

namespace Atom.Web.UI.WebControls.Accordion
{
    [
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class AccordionChangedEventArgs : EventArgs
    {
        public int NewIndex { get; set; }
        public int OldIndex { get; set; }
        public Item CurrentItem { get; set; }

        public AccordionChangedEventArgs(int newIndex, int oldIndex, Item currentItem, JQAccordion accordion)
        {
            this.NewIndex = newIndex;
            this.OldIndex = oldIndex;
            this.CurrentItem = currentItem;
        }
    }
}
