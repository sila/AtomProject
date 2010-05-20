using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Atom.Web.UI.WebControls.Progressbar
{
    [
    AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    Designer(typeof(ProgressbarDesigner)),
    DesignerAttribute(typeof(ProgressbarDesigner)),
    ToolboxData("<{0}:JQProgressbar runat=\"server\"> </{0}:JQProgressbar>")
    ]
    public class JQProgressbar : WebControl, IPostBackEventHandler
    {
        private static readonly object EventValueChanged = new object();

        public delegate void ProgressbarValueChangedEventHandler(object sender, ProgressbarValueChangedEventArgs e);

        [
        Category("Action"),
        Description("OnSelectedItemIndexChanging")
        ]
        public event ProgressbarValueChangedEventHandler ValueChanged
        {
            add
            {
                Events.AddHandler(EventValueChanged, value);
            }
            remove
            {
                Events.RemoveHandler(EventValueChanged, value);
            }
        }

        [
        Category("Action"),
        Description("")
        ]
        protected virtual void OnValueChanged(ProgressbarValueChangedEventArgs e)
        {
            ProgressbarValueChangedEventHandler eventHandler = (ProgressbarValueChangedEventHandler)Events[EventValueChanged];
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }
        protected override void OnInit(EventArgs e)
        {
            string hiddenValue = Page.Request.Params[this.UniqueID + "hiddenValue"];

            if ((!string.IsNullOrEmpty(hiddenValue)) && (hiddenValue.StartsWith("progressbarValue:")))
            {
                this.Value = Convert.ToInt32(hiddenValue.Split(':')[1]);
            }

            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            string _clientPostBackHyperlink = Page.GetPostBackClientHyperlink(this, string.Empty);
            string _clientPostBackScript = Page.GetPostBackClientEvent(this, string.Empty);

            Page.ClientScript.RegisterStartupScript(typeof(Page), this.UniqueID + "startupscript", RenderStartupJavaScript());

            base.OnPreRender(e);
        }

        private string RenderStartupJavaScript()
        {
            StringBuilder startupScript = new StringBuilder();

            startupScript.AppendFormat("<script type=\"text/javascript\">");
            startupScript.AppendFormat("var " + this.UniqueID + ";");
            startupScript.AppendFormat("$(document).ready(function() {{ ");

            startupScript.AppendFormat("var progressbar" + this.UniqueID + " = $('#" + this.UniqueID + "progressbar'); " + this.UniqueID + "= progressbar" + this.UniqueID + ";");
            startupScript.AppendFormat("progressbar" + this.UniqueID + ".progressbar({{");
            startupScript.AppendFormat("value: parseInt($('#" + this.UniqueID + "hiddenValue').val().split(':')[1]), ");
            //Event
            startupScript.AppendFormat(" change: function(event, ui) {{ $('#" + this.UniqueID + "hiddenValue').val('progressbarValue:'+ progressbar"+this.UniqueID+".progressbar('option', 'value')); ");
            if (Events[EventValueChanged] != null)
            {
                startupScript.AppendFormat("__doPostBack('" + this.UniqueID + "', 'selectedvaluechanged:'+ progressbar" + this.UniqueID + ".progressbar('option', 'value')); ");
            }
            startupScript.AppendFormat("}}, ");
            startupScript.AppendFormat("}}); ");
            //Resizeble
            if (this.Resizable)
            {
                startupScript.AppendFormat("progressbar" + this.UniqueID + ".resizable();");
            }
            startupScript.AppendFormat("}}); ");

            //Default:true
            if (!this.Enabled)
            {
                startupScript.AppendFormat("progressbar" + this.UniqueID + ".progressbar('disable');");
            }

            startupScript.AppendFormat("</script>");

            return startupScript.ToString();

        }

        //clear span tag
        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderContents(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "hiddenValue");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "hiddenValue");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "progressbarValue:" + this.Value.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "progressbar");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();

            base.RenderContents(writer);
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument != null)
            {
                if (eventArgument.StartsWith("selectedvaluechanged"))
                {
                    string[] values = eventArgument.Split(':');

                    this.Value = Convert.ToInt32(values[1]);

                    ProgressbarValueChangedEventArgs args = new ProgressbarValueChangedEventArgs(this.Value, this);
                    this.OnValueChanged(args);
                }
            }
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.RaisePostBackEvent(eventArgument);
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(0)
        ]
        public int Value
        {
            get
            {
                object progressValue = (object)ViewState["ValueViewState"];
                return (progressValue == null) ? 0 : Convert.ToInt32(progressValue);
            }
            set
            {
                ViewState["ValueViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool Resizable
        {
            get
            {
                object resizable = (object)ViewState["ResizableViewState"];
                return (resizable == null) ? false : (bool)resizable;
            }
            set
            {
                ViewState["ResizableViewState"] = value;
            }
        }
    }
}
