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
    ParseChildren(true),
    PersistChildren(false),
    Designer(typeof(DialogDesigner)),
    ToolboxData("<{0}:JQDialog runat=\"server\"> </{0}:JQDialog>")
    ]
    public class JQDialog : WebControl, INamingContainer
    {
        private ITemplate _template;

        //Methods
        protected override void CreateChildControls()
        {
            Controls.Clear();

            if (Template != null)
            {
                Template.InstantiateIn(this);
            }
        }

        //clear span tag
        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "hiddenValue");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "hiddenValue");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Title, this.Title);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            foreach (Control control in this.Controls)
            {
                control.RenderControl(writer);
            }
            writer.RenderEndTag();
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
            startupScript.AppendFormat("var " + this.UniqueID + "; ");
            startupScript.AppendFormat("$(document).ready(function() {{ ");
            startupScript.AppendFormat(this.UniqueID + " = $('#" + this.UniqueID + "');");
            startupScript.AppendFormat(this.UniqueID + ".dialog({{");

            if (!this.AutoOpen)
            {
                startupScript.AppendFormat(" autoOpen: false,");
            }

            if (!this.CloseOnEsc)
            {
                startupScript.AppendFormat(" closeOnEscape: false,");
            }

            if (!this.Draggable)
            {
                startupScript.AppendFormat(" draggable: false,");
            }

            if (this.HideAnimation != DialogHideAnimation.None)
            {
                startupScript.AppendFormat(" hide: '{0}',", this.HideAnimation.ToString().ToLower());
            }
            if (!string.IsNullOrEmpty(this.Height.ToString()))
            {
                startupScript.AppendFormat(" height: {0},", this.Height);
            }
            if (!string.IsNullOrEmpty(this.Width.ToString()))
            {
                startupScript.AppendFormat(" width: {0},", this.Width);
            }
            if (!string.IsNullOrEmpty(this.MaxHeight.ToString()))
            {
                startupScript.AppendFormat(" maxHeight: {0},", this.MaxHeight);
            }
            if (!string.IsNullOrEmpty(this.MaxWidth.ToString()))
            {
                startupScript.AppendFormat(" maxWidth: {0},", this.MaxWidth);
            }
            if (!string.IsNullOrEmpty(this.MinHeight.ToString()))
            {
                startupScript.AppendFormat(" minHeight: {0},", this.MinHeight);
            }
            if (!string.IsNullOrEmpty(this.MinWidth.ToString()))
            {
                startupScript.AppendFormat(" minWidth: {0},", this.MinWidth);
            }
            if (this.Modal)
            {
                startupScript.AppendFormat(" modal: true,");
            }
            //postion
            if ((this.Position == DialogPosition.Center)
                || (this.Position == DialogPosition.Bottom)
                || (this.Position == DialogPosition.Top)
                || (this.Position == DialogPosition.Left)
                || (this.Position == DialogPosition.Right))
            {
                startupScript.AppendFormat(" position: '{0}',", this.Position.ToString().ToLower());
            }
            else
            {
                if (this.Position == DialogPosition.CenterLeft)
                { startupScript.AppendFormat(" position: ['center','left'],"); }

                if (this.Position == DialogPosition.CenterRight)
                { startupScript.AppendFormat(" position: ['center','right'],"); }

                if (this.Position == DialogPosition.BottomLeft)
                { startupScript.AppendFormat(" position: ['bottom','left'],"); }

                if (this.Position == DialogPosition.BottomRight)
                { startupScript.AppendFormat(" position: ['bottom','right'],"); }

                if (this.Position == DialogPosition.TopLeft)
                { startupScript.AppendFormat(" position: ['top','left'],"); }

                if (this.Position == DialogPosition.TopRight)
                { startupScript.AppendFormat(" position: ['top','right'],"); }

            }
            if (!this.Resizable)
            {
                startupScript.AppendFormat(" resizable: false");
            }
            if (this.ShowAnimation != DialogShowAnimation.None)
            {
                startupScript.AppendFormat(" show: '{0}',", this.ShowAnimation.ToString().ToLower());
            }

            if (!this.Stack)
            {
                startupScript.AppendFormat(" stack: false");
            }
            startupScript.AppendFormat(" title: '{0}',", this.Title);
            startupScript.AppendFormat(" zIndex: {0},", this.ZIndex);

            if (!this.Enabled)
            {
                startupScript.AppendFormat(" disabled: true,");
            }

            startupScript.AppendFormat("}})");
            startupScript.AppendFormat("}})");
            startupScript.AppendFormat("</script>");

            return startupScript.ToString();
        }

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
                return this._template;
            }
            set
            {
                this._template = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(DialogShowAnimation.None)
        ]
        public DialogShowAnimation ShowAnimation
        {
            get
            {
                object show = ViewState["ShowAnimationViewState"];
                return (show == null) ? DialogShowAnimation.None : (DialogShowAnimation)show;
            }
            set
            {
                ViewState["ShowAnimationViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(DialogHideAnimation.None)
        ]
        public DialogHideAnimation HideAnimation
        {
            get
            {
                object hide = ViewState["HideAnimationViewState"];
                return (hide == null) ? DialogHideAnimation.None : (DialogHideAnimation)hide;
            }
            set
            {
                ViewState["HideAnimationViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(1000)
        ]
        public int ZIndex
        {
            get
            {
                object zIndex = ViewState["ZIndexViewState"];
                return (zIndex == null) ? 1000 : Convert.ToInt32(zIndex);
            }
            set
            {
                ViewState["ZIndexViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
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
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue("")
        ]
        public Unit MaxHeight
        {
            get
            {
                object maxHeight = ViewState["MaxHeightViewState"];
                return (maxHeight == null) ? Unit.Empty : (Unit)maxHeight;
            }
            set
            {
                ViewState["MaxHeightViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue("")
        ]
        public Unit MaxWidth
        {
            get
            {
                object maxWidth = ViewState["MaxWidthViewState"];
                return (maxWidth == null) ? Unit.Empty : (Unit)maxWidth;
            }
            set
            {
                ViewState["MaxWidthViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue("")
        ]
        public Unit MinHeight
        {
            get
            {
                object minHeight = ViewState["MinHeightViewState"];
                return (minHeight == null) ? Unit.Empty : (Unit)minHeight;
            }
            set
            {
                ViewState["MinHeightViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue("")
        ]
        public Unit MinWidth
        {
            get
            {
                object minWidth = ViewState["MinWidthViewState"];
                return (minWidth == null) ? Unit.Empty : (Unit)minWidth;
            }
            set
            {
                ViewState["MinWidthViewState"] = value;
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
