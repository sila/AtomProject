using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Web.UI;
using System.ComponentModel;

namespace Atom.Web.UI.WebControls.Tabs
{
    [
    ToolboxItem(false)
    ]
    public class Tab : Control, INamingContainer
    {
        //Fields
        private string _header;
        private bool _selected;
        private bool _enable;


        private string _url;
        private ITemplate _temlpate;


        //Methods
        protected override void CreateChildControls()
        {
            Controls.Clear();

            if (Template != null)
                Template.InstantiateIn(this);
        }


        //Properties
        [
        Browsable(false),
        DesignerSerializationVisibility(
            DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public ITemplate Template
        {
            get
            {
                return _temlpate;
            }
            set
            {
                _temlpate = value;
            }
        }

        [
        Category("Behavior"),
        DefaultValue(""),
        Description(""),
        NotifyParentProperty(true),
        ]
        public string Header
        {
            get
            {
                return _header;
            }
            set
            {
                _header = value;
            }
        }
        [
        Category("Behavior"),
        DefaultValue(""),
        Description(""),
        NotifyParentProperty(true),
        ]
        public bool Enable
        {
            get
            {
                return this._enable;
            }
            set
            {
                this._enable = value;
            }
        }

        [
        Category("Behavior"),
        DefaultValue(false),
        Description(""),
        NotifyParentProperty(true),
        ]
        public bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
            }
        }



        [
        Category("Behavior"),
        DefaultValue(""),
        Description(""),
        NotifyParentProperty(true),
        ]
        public string URL
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }


        //[EditorBrowsable(EditorBrowsableState.Never)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public override string ID
        //{
        //    get
        //    {
        //        return base.ID;
        //    }
        //    set
        //    {
        //        base.ID = value;
        //    }
        //}

        [
      EditorBrowsable(EditorBrowsableState.Never),
      DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
      ]
        public override bool EnableViewState
        {
            get
            {
                return base.EnableViewState;
            }
            set
            {
                base.EnableViewState = value;
            }
        }

        [
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }
    }
}
