using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;

namespace Atom.Web.UI.WebControls.Dialog
{
    [
    AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty(""),
    Designer(typeof(DialogDesigner)),
    ToolboxData("<{0}:JQDialog runat=\"server\"> </{0}:JQDialog>")
    ]
    public class JQDialog : WebControl
    {
        private string _header;
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        private ITemplate _temlpate;
        [
        Browsable(false),
        DesignerSerializationVisibility(
           DesignerSerializationVisibility.Content),
         PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public ITemplate Template
        {
            get { return _temlpate; }
            set { _temlpate = value; }
        }

        //Methods
        protected override void CreateChildControls()
        {
            Controls.Clear();

            if (Template != null)
                Template.InstantiateIn(this);
        }

        //clear span tag
        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderContents(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "dialog");
            writer.AddAttribute(HtmlTextWriterAttribute.Title, this.Header);

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            foreach (Control control in this.Controls)
            {
                control.RenderControl(writer);
            }
            writer.RenderEndTag();

            base.RenderContents(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
            string clientPostBackHyperlink = Page.GetPostBackClientHyperlink(this, string.Empty);
            string clientPostBackScript = Page.GetPostBackClientEvent(this, string.Empty);

            Page.ClientScript.RegisterStartupScript(typeof(Page), this.UniqueID + "startupscript", RenderStartupJavaScript());

            base.OnPreRender(e);
        }

        private string RenderStartupJavaScript()
        {
            StringBuilder startupScript = new StringBuilder();

            startupScript.AppendFormat("<script type=\"text/javascript\">");
            startupScript.AppendFormat("$(document).ready(function() {{ ");
            startupScript.AppendFormat("var dialog" + this.UniqueID + " = $('#" + this.UniqueID + "dialog');");
            startupScript.AppendFormat("dialog" + this.UniqueID + ".dialog({{");

            if (!this.AutoOpen)
            {
                startupScript.AppendFormat(" autoOpen: false");
            }
            if (!this.CloseOnEsc)
            {
                startupScript.AppendFormat(" closeOnEscape: false");
            }
            startupScript.AppendFormat(" closeText: true");
            startupScript.AppendFormat(" dialogClass: true");
            if (!this.Draggable)
            {
                startupScript.AppendFormat(" draggable: false");
            }
            startupScript.AppendFormat(" height: true");
            startupScript.AppendFormat(" hide: true");
            startupScript.AppendFormat(" height: true");
            startupScript.AppendFormat(" maxHeight: true");
            startupScript.AppendFormat(" maxWidth: true");
            startupScript.AppendFormat(" minHeight: true");
            startupScript.AppendFormat(" minWidth: true");
            if (this.Modal)
            {
                startupScript.AppendFormat(" modal: true");
            }
            //postion
            if ((this.Position == DialogPosition.Center)
                || (this.Position == DialogPosition.Bottom)
                || (this.Position == DialogPosition.Top)
                || (this.Position == DialogPosition.Left)
                || (this.Position == DialogPosition.Right))
            {
                startupScript.AppendFormat(" position: '{0}'", this.Position.ToString().ToLower());
            }
            if (this.Position == DialogPosition.CenterLeft)
            {

            }
            if (this.Position == DialogPosition.CenterRight) { }
            if (this.Position == DialogPosition.BottomLeft) { }
            if (this.Position == DialogPosition.BottomRight) { }
            if (this.Position == DialogPosition.TopLeft) { }
            if (this.Position == DialogPosition.TopRight) { }

            if (!this.Resizable)
            {
                startupScript.AppendFormat(" resizable: false");
            }
            startupScript.AppendFormat(" show: true");
            if (!this.Stack)
            {
                startupScript.AppendFormat(" stack: false");
            }
            startupScript.AppendFormat(" title: '{0}'", this.Title);
            startupScript.AppendFormat(" width: true");
            startupScript.AppendFormat(" zIndex: true");

            if (!this.Enabled)
            {
                startupScript.AppendFormat(" disabled: true");
            }

            startupScript.AppendFormat("}})");
            startupScript.AppendFormat("}})");
            startupScript.AppendFormat("</script>");

            return startupScript.ToString();
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue(true)
        ]
        public bool AutoOpen
        {
            get
            {
                object autoOpen = ViewState["AutoOpenViewState"];
                return (autoOpen == null) ? true : Convert.ToBoolean(autoOpen);
            }
            set
            {
                ViewState["AutoOpenViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue(true)
        ]
        public bool CloseOnEsc
        {
            get
            {
                object closeOnEsc = ViewState["CloseOnEscViewState"];
                return (closeOnEsc == null) ? true : Convert.ToBoolean(closeOnEsc);
            }
            set
            {
                ViewState["CloseOnEscViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue(true)
        ]
        public bool Draggable
        {
            get
            {
                object draggable = ViewState["DraggableViewState"];
                return (draggable == null) ? true : Convert.ToBoolean(draggable);
            }
            set
            {
                ViewState["DraggableViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue(false)
        ]
        public bool Modal
        {
            get
            {
                object modal = ViewState["ModalViewState"];
                return (modal == null) ? false : Convert.ToBoolean(modal);
            }
            set
            {
                ViewState["ModalViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue(true)
        ]
        public bool Resizable
        {
            get
            {
                object resizable = ViewState["ResizableViewState"];
                return (resizable == null) ? true : Convert.ToBoolean(resizable);
            }
            set
            {
                ViewState["ResizableViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue(true)
        ]
        public bool Stack
        {
            get
            {
                object stack = ViewState["StackViewState"];
                return (stack == null) ? true : Convert.ToBoolean(stack);
            }
            set
            {
                ViewState["StackViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue(DialogPosition.Center)
        ]
        public DialogPosition Position
        {
            get
            {
                object position = ViewState["PostionViewState"];
                return (position == null) ? DialogPosition.Center : (DialogPosition)position;
            }
            set
            {
                ViewState["PostionViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue("")
        ]
        public string Title
        {
            get
            {
                object title = ViewState["TitleViewState"];
                return (title == null) ? "" : title.ToString();
            }
            set
            {
                ViewState["TitleViewState"] = value;
            }
        }



        [
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override System.Drawing.Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        [
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override BorderStyle BorderStyle
        {
            get
            {
                return base.BorderStyle;
            }
            set
            {
                base.BorderStyle = value;
            }
        }

        [
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override Unit BorderWidth
        {
            get
            {
                return base.BorderWidth;
            }
            set
            {
                base.BorderWidth = value;
            }
        }

        [
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override System.Drawing.Color BorderColor
        {
            get
            {
                return base.BorderColor;
            }
            set
            {
                base.BorderColor = value;
            }
        }

        [
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override FontInfo Font
        {
            get
            {
                return base.Font;
            }
        }

        [
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override bool EnableTheming
        {
            get
            {
                return base.EnableTheming;
            }
            set
            {
                base.EnableTheming = value;
            }
        }

        [
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override string ToolTip
        {
            get
            {
                return base.ToolTip;
            }
            set
            {
                base.ToolTip = value;
            }
        }

        [
        EditorBrowsable(EditorBrowsableState.Never),
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override string SkinID
        {
            get
            {
                return base.SkinID;
            }
            set
            {
                base.SkinID = value;
            }
        }

        [
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        [
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override string CssClass
        {
            get
            {
                return base.CssClass;
            }
            set
            {
                base.CssClass = value;
            }
        }


    }
}
