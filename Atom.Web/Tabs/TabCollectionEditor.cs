using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace Atom.Web.UI.WebControls.Tabs
{
    public class TabCollectionEditor : CollectionEditor
    {
        public TabCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(Tab);
        }
    }
}
