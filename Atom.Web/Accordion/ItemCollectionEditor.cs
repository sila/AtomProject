using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace Atom.Web.UI.WebControls.Accordion
{
    public class ItemCollectionEditor : CollectionEditor
    {
        public ItemCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(Item);
        }
    }
}
