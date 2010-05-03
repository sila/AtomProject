using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;

namespace Atom.Web.UI.WebControls.Accordion
{
    [
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class AccordionChangingEventArgs : CancelEventArgs
    {
        public int NewIndex { get; set; }
        public int OldIndex { get; set; }
       // public JQAccordion Accordion { get; set; }

        public AccordionChangingEventArgs(int newIndex, int oldIndex)//, JQAccordion accordion)
        {
           // this.Accordion = accordion;
            this.NewIndex = newIndex;
            this.OldIndex = oldIndex;
        }
    }
}
