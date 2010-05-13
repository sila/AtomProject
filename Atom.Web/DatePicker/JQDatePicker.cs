using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace Atom.Web.UI.WebControls.DatePicker
{
    [
    AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    Designer(typeof(DatePickerDesigner)),
    ToolboxData("<{0}:JQDatePicker runat=\"server\"> </{0}:JQDatePicker>")
    ]
    public class JQDatePicker : WebControl
    {
        private string RenderStartupJavaScript()
        {
            StringBuilder startupScript = new StringBuilder();

            startupScript.AppendFormat("<script type=\"text/javascript\">");
            startupScript.AppendFormat("var " + this.UniqueID + "; ");
            startupScript.AppendFormat("$(document).ready(function() {{ ");
            startupScript.AppendFormat(this.UniqueID + " = $('#" + this.UniqueID + "');");


            startupScript.AppendFormat("}})");
            startupScript.AppendFormat("}})");
            startupScript.AppendFormat("</script>");

            return startupScript.ToString();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "hiddenValue");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "hiddenValue");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();



        }

        protected override void OnPreRender(EventArgs e)
        {
            string clientPostBackHyperlink = Page.GetPostBackClientHyperlink(this, string.Empty);
            string clientPostBackScript = Page.GetPostBackClientEvent(this, string.Empty);

            Page.ClientScript.RegisterStartupScript(typeof(Page), this.UniqueID + "startupscript", RenderStartupJavaScript());

            base.OnPreRender(e);
        }


        //properties

        //[
        //Category("Behavior"),
        //Description(""),
        //DefaultValue("")
        //]
        //public string Range
        //{
        //    get
        //    {
        //        object range = (object)ViewState["ViewState"];
        //        return (range == null) ? "" : range.ToString();
        //    }
        //    set
        //    {
        //        ViewState["ViewState"] = value;
        //    }
        //}
        //bool
        public bool AutoSize
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool ButtonImageOnly
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool ChangeMonth
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool ChangeYear
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool ConstrainInput
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool GotoCurrent
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool HideIfNoPrevNext
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool IsRTL
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool NavigationAsDateFormat
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool SelectOtherMonths
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool ShowButtonPanel
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool ShowMonthAfterYear
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool ShowWeek
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public bool ShowOtherMonths { get; set; }
        public bool Draggable { get; set; }
        
        //str
        public string AltField
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string AltFormat
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string AppendText
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string ButtonImage
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string ButtonText
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string CloseText
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string CurrentText
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string DateFormat
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string WeekHeader
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string YearRange
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string YearSuffix
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string ShowOn
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string ShowAnimation
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string NextText
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string Duration
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string PrevText
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }

        //int
        public int FirstDay
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public int StepMonths
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public int ShowCurrentAtPos
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        
        //arr
        public string DayNames
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string DayNamesMin
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string DayNamesShort
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string DefaultDate
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string MaxDate
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string MinDate
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string MonthNames
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string MonthNamesShort
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string NumberOfMonths
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string ShortYearCutoff
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }
        public string ShowOptions
        {
            get { return ""; }
            set { ViewState["ViewState"] = value; }
        }



        #region inherit properties

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

        #endregion

    }
}
