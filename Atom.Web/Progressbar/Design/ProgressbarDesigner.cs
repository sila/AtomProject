using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace Atom.Web.UI.WebControls.Progressbar
{
    public class ProgressbarDesigner : ControlDesigner
    {
        private DesignerActionListCollection _actionList = null;

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
        public override string GetDesignTimeHtml()
        {
            return base.CreatePlaceHolderDesignTimeHtml();
        }

    }

    public class ActionList : DesignerActionList
    {
        private ProgressbarDesigner _parent;
        private DesignerActionItemCollection _items;

        public ActionList(ProgressbarDesigner parent)
            : base(parent.Component)
        {
            this._parent = parent;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            if (this._items == null)
            {
                this._items = new DesignerActionItemCollection();

                this._items.Add(new DesignerActionPropertyItem("Value", "Value", ""));
                this._items.Add(new DesignerActionPropertyItem("Resizable", "Resizable", ""));
               
                //this._items.Add(new DesignerActionMethodItem(this, "Value", "Value", true));
                //this._items.Add(new DesignerActionMethodItem(this, "Resizable", "Resizable", true));
                //_items.Add(new DesignerActionTextItem("TextItem", ""));
                // this._items.Add(new DesignerActionPropertyItem("Resizable", "Resizable1"));
                //this._items.Add(new DesignerActionTextItem("Value", "Value"));
            }
            return this._items;
        }
        public bool Resizable
        {
            get
            {
                JQProgressbar progressbar = (JQProgressbar)_parent.Component;

                return progressbar.Resizable;
            }
            set
            {
                JQProgressbar progressbar = (JQProgressbar)_parent.Component;

                TypeDescriptor.GetProperties(progressbar)["Resizable"].SetValue(progressbar, value);
            }
        }
        public int Value
        {
            get
            {
                JQProgressbar progressbar = (JQProgressbar)_parent.Component;

                return progressbar.Value;
            }
            set
            {
                JQProgressbar progressbar = (JQProgressbar)_parent.Component;

                TypeDescriptor.GetProperties(progressbar)["Value"].SetValue(progressbar, value);
            }
        }

        //private void Resizable()
        //{
        //    JQProgressbar progressbar = (JQProgressbar)_parent.Component;
        //    PropertyDescriptor propDesc = TypeDescriptor.GetProperties(progressbar)["Resizable"];
        //    bool value = (bool)propDesc.GetValue(progressbar);
        //    propDesc.SetValue(progressbar, !value);
        //}
        //private void Value()
        //{
        //    JQProgressbar progressbar = (JQProgressbar)_parent.Component;
        //    PropertyDescriptor propDesc = TypeDescriptor.GetProperties(progressbar)["Value"];
        //    propDesc.SetValue(progressbar, 50);
        //}
    }
}
