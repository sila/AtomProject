using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Atom.Web.UI.WebControls.Button.Desing;

namespace Atom.Web.UI.WebControls.Button
{
    [
    AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty(""),
    Designer(typeof(ButtonDesigner)),
    ToolboxData("<{0}:JQButton runat=\"server\"> </{0}:JQButton>")
    ]
    public class JQButton : WebControl, IButtonControl, IPostBackEventHandler
    {
        //MS convention:
        //Fields
        //Events
        //Methods
        //Properties

        //Fields
        private static readonly object EventClick = new object();

        //Events
        protected override void OnPreRender(EventArgs e)
        {
            string clientPostBackHyperlink = Page.GetPostBackClientHyperlink(this, string.Empty);
            string clientPostBackScript = Page.GetPostBackClientEvent(this, string.Empty);

            Page.ClientScript.RegisterStartupScript(typeof(Page), this.UniqueID + "startupscript", RenderStartupJavaScript());

            base.OnPreRender(e);
        }
        protected virtual void OnClick(EventArgs e)
        {
            EventHandler clickEvent = (EventHandler)Events[EventClick];
            if (clickEvent != null)
            {
                clickEvent(this, e);
            }
        }

        //Methods
        //clear span tag
        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderContents(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "button");
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.RenderEndTag();

            base.RenderContents(writer);
        }
        private string RenderStartupJavaScript()
        {
            StringBuilder startupScript = new StringBuilder();

            startupScript.AppendFormat("<script type=\"text/javascript\">");
            startupScript.AppendFormat("$(document).ready(function() {{ ");
            startupScript.AppendFormat("var button" + this.UniqueID + " = $('#" + this.UniqueID + "button');");
            startupScript.AppendFormat("button" + this.UniqueID + ".button({{");

            //Default:false
            if (this.TextVisible)
            {
                startupScript.AppendFormat("text: {0},", this.TextVisible.ToString().ToLower());
            }

            startupScript.AppendFormat("label: '{0}',", this.Text);
            startupScript.AppendFormat("}}) ");

            if (Events[EventClick] != null)
            {
                startupScript.AppendFormat(".click(function(){{ " + Page.GetPostBackEventReference(this, "click") + "}})");
            }
            else
            {
                startupScript.AppendFormat("; ");

            }

            //Default:true
            if (!this.Enabled)
            {
                startupScript.AppendFormat("button" + this.UniqueID + ".button('disable');");
            }

            startupScript.AppendFormat("}})");
            startupScript.AppendFormat("</script>");

            return startupScript.ToString();
        }
        public event EventHandler Click
        {
            add
            {
                Events.AddHandler(EventClick, value);
            }
            remove
            {
                Events.RemoveHandler(EventClick, value);
            }
        }
        public void RaisePostBackEvent(string eventArgument)
        {
            this.OnClick(EventArgs.Empty);
        }
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.RaisePostBackEvent(eventArgument);
        }

        
        //Properties
        //icons url properties
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(ButtonType.Button)
        ]
        public ButtonType ButtonTypes
        {
            get
            {
                object buttonTypeVisibleValue = ViewState["ButtonTypeViewState"];
                return (buttonTypeVisibleValue == null) ? ButtonType.Button : (ButtonType)buttonTypeVisibleValue;
            }
            set
            {
                ViewState["ButtonTypeViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(true)
        ]
        public bool TextVisible
        {
            get
            {
                object textVisibleValue = ViewState["TextVisibleViewState"];
                return (textVisibleValue == null) ? true : (bool)textVisibleValue;
            }
            set
            {
                ViewState["TextVisibleViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("")
        ]
        public string Text
        {
            get
            {
                object textValue = ViewState["TextViewState"];
                return (textValue == null) ? string.Empty : (string)textValue;
            }
            set
            {
                ViewState["TextViewState"] = value;
            }
        }

        #region inherit properties visibility
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
        #endregion

        #region IButtonControl Members

        public bool CausesValidation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public event CommandEventHandler Command;

        public string CommandArgument
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string CommandName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string PostBackUrl
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ValidationGroup
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool IButtonControl.CausesValidation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler IButtonControl.Click
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event CommandEventHandler IButtonControl.Command
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        string IButtonControl.CommandArgument
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        string IButtonControl.CommandName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        string IButtonControl.PostBackUrl
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        string IButtonControl.Text
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        string IButtonControl.ValidationGroup
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}