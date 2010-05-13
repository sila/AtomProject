using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace Atom.Web.UI.WebControls.Accordion
{
    public class AccordionDesigner : ControlDesigner
    {
        private DesignerActionListCollection _actionList = null;
        //public bool _isDirty;

        public override DesignerActionListCollection ActionLists
        {
            get
            {

                if (this._actionList == null)
                {
                    this._actionList = new DesignerActionListCollection();
                    this._actionList.AddRange(base.ActionLists);
                    this._actionList.Add(new ActionList(this));
                }

                return this._actionList;
            }
        }

        public void AddNewItem()
        {
            JQAccordion acc = (JQAccordion)this.Component;
            acc.Items.Add(new Item("NewHeader_" + acc.Items.Count));
            Tag.SetDirty(true);
        }

        public override string GetDesignTimeHtml()
        {
            return base.CreatePlaceHolderDesignTimeHtml();
        }
    }
    public class ActionList : DesignerActionList
    {
        private AccordionDesigner _parent;
        private DesignerActionItemCollection _items;

        public ActionList(AccordionDesigner parent)
            : base(parent.Component)
        {
            this._parent = parent;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            if (this._items == null)
            {
                this._items = new DesignerActionItemCollection();

                this._items.Add(new DesignerActionMethodItem(this, "AddNewItem", "Add new item", "", true));
                this._items.Add(new DesignerActionPropertyItem("FillSpace", "FillSpace", ""));
                this._items.Add(new DesignerActionPropertyItem("Collapsible", "Collapsible", ""));
                this._items.Add(new DesignerActionPropertyItem("Resizable", "Resizable", ""));
                this._items.Add(new DesignerActionPropertyItem("ChangeType", "ChangeType", ""));
                this._items.Add(new DesignerActionPropertyItem("AnimationType", "Animation Type", ""));
            }
            return this._items;
        }

        public void AddNewItem()
        {
            _parent.AddNewItem();
        }

        public bool FillSpace
        {
            get
            {
                JQAccordion acc = (JQAccordion)_parent.Component;
                return acc.FillSpace;
            }
            set
            {
                JQAccordion acc = (JQAccordion)_parent.Component;
                TypeDescriptor.GetProperties(acc)["FillSpace"].SetValue(acc, value);
            }
        }
        public bool Resizable
        {
            get
            {
                JQAccordion acc = (JQAccordion)_parent.Component;
                return acc.Resizable;
            }
            set
            {
                JQAccordion acc = (JQAccordion)_parent.Component;

                TypeDescriptor.GetProperties(acc)["Resizable"].SetValue(acc, value);
                TypeDescriptor.GetProperties(acc)["FillSpace"].SetValue(acc, value);
            }
        }
        public bool Collapsible
        {
            get
            {
                JQAccordion acc = (JQAccordion)_parent.Component;
                return acc.Collapsible;
            }
            set
            {
                JQAccordion acc = (JQAccordion)_parent.Component;
                TypeDescriptor.GetProperties(acc)["Collapsible"].SetValue(acc, value);
            }
        }
        public ItemChangeType ChangeType
        {
            get
            {
                JQAccordion acc = (JQAccordion)_parent.Component;
                return acc.ChangeType;
            }
            set
            {
                JQAccordion acc = (JQAccordion)_parent.Component;
                TypeDescriptor.GetProperties(acc)["ChangeType"].SetValue(acc, value);
            }
        }
        public AccordionDuration AnimationType
        {
            get
            {
                JQAccordion acc = (JQAccordion)_parent.Component;
                return acc.AnimationType;
            }
            set
            {
                JQAccordion acc = (JQAccordion)_parent.Component;
                TypeDescriptor.GetProperties(acc)["AnimationType"].SetValue(acc, value);
            }
        }


    }
}
