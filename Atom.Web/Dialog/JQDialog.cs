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

            startupScript.AppendFormat(" autoOpen: true");

            startupScript.AppendFormat("}})");
            startupScript.AppendFormat("}})");
            startupScript.AppendFormat("</script>");

            return startupScript.ToString();
        }
    }
}
