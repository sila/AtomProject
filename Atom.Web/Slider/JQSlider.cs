using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;
using Atom.Web.UI.WebControls.Dialog;
using System.Web.UI.WebControls;

namespace Atom.Web.UI.WebControls.Slider
{

    [
    AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty(""),
    Designer(typeof(SliderDesigner)),
    ToolboxData("<{0}:JQSlider runat=\"server\"> </{0}:JQSlider>")
    ]
    public class JQSlider : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderContents(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "hiddenValue");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "hiddenValue");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "selectedtab:");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
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
            startupScript.AppendFormat("var " + this.UniqueID + " = $('#" + this.UniqueID + "');");
            startupScript.AppendFormat(this.UniqueID + ".slider({{");

            startupScript.AppendFormat(" max: {0},", this.MaxValue);
            startupScript.AppendFormat(" min: {0},", this.MinValue);
            startupScript.AppendFormat(" animate: {0},", this.Animation.ToString().ToLower());
            startupScript.AppendFormat(" orientation: {0},", this.Orientation.ToString().ToLower());
            startupScript.AppendFormat(" range: {0},", this.Range.ToString().ToLower());
            startupScript.AppendFormat(" step: {0},", this.Step);

            if (string.IsNullOrEmpty(this.RangeValue.ToString()))
            {
                startupScript.AppendFormat(" value: {0},", this.Value);
            }
            else
            {
                startupScript.AppendFormat(" values: [{0},{1}],", this.Value, this.RangeValue);
            }
            if (this.Enabled)
            {
                startupScript.AppendFormat(" disabled: true,");
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
        DefaultValue(100)
        ]
        public int MaxValue
        {
            get
            {
                object max = ViewState["MaxValueViewState"];
                return (max == null) ? 0 : Convert.ToInt32(max);
            }
            set
            {
                ViewState["MaxValueViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(0)
        ]
        public int MinValue
        {
            get
            {
                object mix = ViewState["MixValueViewState"];
                return (mix == null) ? 0 : Convert.ToInt32(mix);
            }
            set
            {
                ViewState["MixValueViewState"] = value;
            }
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
                object val = (object)ViewState["ValueViewState"];
                return (val == null) ? 0 : Convert.ToInt32(val);
            }
            set
            {
                ViewState["ValueViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue("Value")
        ]
        public int RangeValue
        {
            get
            {
                object rangeValue = (object)ViewState["RangeValueViewState"];
                return (rangeValue == null) ? this.Value : Convert.ToInt32(rangeValue);
            }
            set
            {
                ViewState["RangeValueViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(1)
        ]
        public int Step
        {
            get
            {
                object step = (object)ViewState["StepTypeViewState"];
                return (step == null) ? 1 : Convert.ToInt32(step);
            }
            set
            {
                ViewState["StepTypeViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(SliderOrientation.Horizontal)
        ]
        public SliderOrientation Orientation
        {
            get
            {
                object orientation = (object)ViewState["OrientationViewState"];
                return (orientation == null) ? SliderOrientation.Horizontal : (SliderOrientation)orientation;
            }
            set
            {
                ViewState["OrientationViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(SliderAnimation.Normal)
        ]
        public SliderAnimation Animation
        {
            get
            {
                object animation = (object)ViewState["AnimationViewState"];
                return (animation == null) ? SliderAnimation.Normal : (SliderAnimation)animation;
            }
            set
            {
                ViewState["AnimationViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(SliderRange.False)
        ]
        public SliderRange Range
        {
            get
            {
                object range = (object)ViewState["RangeViewState"];
                return (range == null) ? SliderRange.False : (SliderRange)range;
            }
            set
            {
                ViewState["RangeViewState"] = value;
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
