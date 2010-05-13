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
            startupScript.AppendFormat(this.UniqueID + ".datepicker({{");
            if (!this.Enabled)
            {
                startupScript.AppendFormat(" disabled: true,");
            }
            if (this.AutoSize)
            {
                startupScript.AppendFormat(" autoSize: true,");
            }
            if (this.ButtonImageOnly)
            {
                startupScript.AppendFormat(" buttonImageOnly: true,");
            }
            if (this.ChangeMonth)
            {
                startupScript.AppendFormat(" changeMonth: true,");
            }
            if (this.ChangeYear)
            {
                startupScript.AppendFormat(" changeYear: true,");
            }
            if (!this.ConstrainInput)
            {
                startupScript.AppendFormat(" constrainInput: false,");
            }
            if (this.GotoCurrent)
            {
                startupScript.AppendFormat(" gotoCurrent: true,");
            }
            if (this.HideIfNoPrevNext)
            {
                startupScript.AppendFormat(" hideIfNoPrevNext: true,");
            }
            if (this.IsRTL)
            {
                startupScript.AppendFormat(" isRTL: true,");
            }
            if (this.NavigationAsDateFormat)
            {
                startupScript.AppendFormat(" navigationAsDateFormat: true,");
            }
            if (this.SelectOtherMonths)
            {
                startupScript.AppendFormat(" selectOtherMonths: true,");
            }
            if (this.ShowButtonPanel)
            {
                startupScript.AppendFormat(" showButtonPanel: true,");
            }
            if (this.ShowMonthAfterYear)
            {
                startupScript.AppendFormat(" showMonthAfterYear: true,");
            }
            if (this.ShowWeek)
            {
                startupScript.AppendFormat(" showWeek: true,");
            }
            if (this.ShowOtherMonths)
            {
                startupScript.AppendFormat(" showOtherMonths: true,");
            }
            if (this.FirstDay != 0)
            {
                startupScript.AppendFormat(" firstDay: {0},", this.FirstDay);
            }
            if (this.StepMonths != 1)
            {
                startupScript.AppendFormat(" stepMonths: {0},", this.StepMonths);
            }
            if (this.ShowCurrentAtPos != 0)
            {
                startupScript.AppendFormat(" showCurrentAtPos: {0},", this.ShowCurrentAtPos);
            }


            startupScript.AppendFormat("}})");
            if ((this.Mode == DatePickerMode.Calendar) && (this.Draggable))
            {
                startupScript.AppendFormat(" .draggable()");
            }
            startupScript.AppendFormat(";");
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

            if (this.Mode == DatePickerMode.DatePicker)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            string clientPostBackHyperlink = Page.GetPostBackClientHyperlink(this, string.Empty);
            string clientPostBackScript = Page.GetPostBackClientEvent(this, string.Empty);

            Page.ClientScript.RegisterStartupScript(typeof(Page), this.UniqueID + "startupscript", RenderStartupJavaScript());

            base.OnPreRender(e);
        }

        //properties
        #region bool

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool AutoSize
        {
            get
            {
                object autoSize = ViewState["AutoSizeViewState"];
                return (autoSize == null) ? false : Convert.ToBoolean(autoSize);
            }
            set
            {
                ViewState["AutoSizeViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool ButtonImageOnly
        {
            get
            {
                object buttonImageOnly = ViewState["ButtonImageOnlyViewState"];
                return (buttonImageOnly == null) ? false : Convert.ToBoolean(buttonImageOnly);
            }
            set
            {
                ViewState["ButtonImageOnlyViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool ChangeMonth
        {
            get
            {
                object changeMonth = ViewState["ChangeMonthViewState"];
                return (changeMonth == null) ? false : Convert.ToBoolean(changeMonth);
            }
            set
            {
                ViewState["ChangeMonthViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool ChangeYear
        {
            get
            {
                object changeYear = ViewState["ChangeYearViewState"];
                return (changeYear == null) ? false : Convert.ToBoolean(changeYear);
            }
            set
            {
                ViewState["ChangeYearViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(true)
        ]
        public bool ConstrainInput
        {
            get
            {
                object changeYear = ViewState["ConstrainInputViewState"];
                return (changeYear == null) ? true : Convert.ToBoolean(changeYear);
            }
            set
            {
                ViewState["ConstrainInputViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool GotoCurrent
        {
            get
            {
                object gotoCurrent = ViewState["GotoCurrentViewState"];
                return (gotoCurrent == null) ? false : Convert.ToBoolean(gotoCurrent);
            }
            set
            {
                ViewState["GotoCurrentViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool HideIfNoPrevNext
        {
            get
            {
                object hideIfNoPrevNext = ViewState["HideIfNoPrevNextViewState"];
                return (hideIfNoPrevNext == null) ? false : Convert.ToBoolean(hideIfNoPrevNext);
            }
            set
            {
                ViewState["HideIfNoPrevNextViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool IsRTL
        {
            get
            {
                object rtl = ViewState["IsRTLViewState"];
                return (rtl == null) ? false : Convert.ToBoolean(rtl);
            }
            set
            {
                ViewState["IsRTLViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool NavigationAsDateFormat
        {
            get
            {
                object navigationAsDateFormat = ViewState["NavigationAsDateFormatViewState"];
                return (navigationAsDateFormat == null) ? false : Convert.ToBoolean(navigationAsDateFormat);
            }
            set
            {
                ViewState["NavigationAsDateFormatViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool SelectOtherMonths
        {
            get
            {
                object selectOtherMonths = ViewState["SelectOtherMonthsViewState"];
                return (selectOtherMonths == null) ? false : Convert.ToBoolean(selectOtherMonths);
            }
            set
            {
                ViewState["SelectOtherMonthsViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool ShowButtonPanel
        {
            get
            {
                object showButtonPanel = ViewState["ShowButtonPanelViewState"];
                return (showButtonPanel == null) ? false : Convert.ToBoolean(showButtonPanel);
            }
            set
            {
                ViewState["ShowButtonPanelViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool ShowMonthAfterYear
        {
            get
            {
                object showMonthAfterYear = ViewState["ShowMonthAfterYearViewState"];
                return (showMonthAfterYear == null) ? false : Convert.ToBoolean(showMonthAfterYear);
            }
            set
            {
                ViewState["ShowMonthAfterYearViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool ShowWeek
        {
            get
            {
                object showWeek = ViewState["ShowWeekViewState"];
                return (showWeek == null) ? false : Convert.ToBoolean(showWeek);
            }
            set
            {
                ViewState["ShowWeekViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool ShowOtherMonths
        {
            get
            {
                object showOtherMonths = ViewState["ShowOtherMonthsViewState"];
                return (showOtherMonths == null) ? false : Convert.ToBoolean(showOtherMonths);
            }
            set
            {
                ViewState["ShowOtherMonthsViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool Draggable
        {
            get
            {
                object draggable = ViewState["DraggableViewState"];
                return (draggable == null) ? false : Convert.ToBoolean(draggable);
            }
            set
            {
                ViewState["DraggableViewState"] = value;
            }
        }

        #endregion
        #region str
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("")
        ]
        public string AltField
        {
            get
            {
                object autoSize = ViewState["AltFieldViewState"];
                return (autoSize == null) ? string.Empty : autoSize.ToString();
            }
            set
            {
                ViewState["AltFieldViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("")
        ]
        public string AltFormat
        {
            get
            {
                object altFormat = ViewState["AltFormatViewState"];
                return (altFormat == null) ? string.Empty : altFormat.ToString();
            }
            set
            {
                ViewState["AltFormatViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("")
        ]
        public string AppendText
        {
            get
            {
                object appendText = ViewState["AppendTextViewState"];
                return (appendText == null) ? string.Empty : appendText.ToString();
            }
            set
            {
                ViewState["AppendTextViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("")
        ]
        public string ButtonImage
        {
            get
            {
                object buttonImage = ViewState["ButtonImageViewState"];
                return (buttonImage == null) ? string.Empty : buttonImage.ToString();
            }
            set
            {
                ViewState["ButtonImageViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("...")
        ]
        public string ButtonText
        {
            get
            {
                object buttonText = ViewState["ButtonTextViewState"];
                return (buttonText == null) ? "..." : buttonText.ToString();
            }
            set
            {
                ViewState["ButtonTextViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("Done")
        ]
        public string CloseText
        {
            get
            {
                object closeText = ViewState["CloseTextViewState"];
                return (closeText == null) ? "Done" : closeText.ToString();
            }
            set
            {
                ViewState["CloseTextViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("Today")
        ]
        public string CurrentText
        {
            get
            {
                object currentText = ViewState["CurrentTextViewState"];
                return (currentText == null) ? "Today" : currentText.ToString();
            }
            set
            {
                ViewState["CurrentTextViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("mm/dd/yy")
        ]
        public string DateFormat
        {
            get
            {
                object dateFormat = ViewState["DateFormatViewState"];
                return (dateFormat == null) ? "mm/dd/yy" : dateFormat.ToString();
            }
            set
            {
                ViewState["DateFormatViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("Wk")
        ]
        public string WeekHeader
        {
            get
            {
                object weekHeader = ViewState["WeekHeaderViewState"];
                return (weekHeader == null) ? "Wk" : weekHeader.ToString();
            }
            set
            {
                ViewState["WeekHeaderViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("c-10:c+10")
        ]
        public string YearRange
        {
            get
            {
                object yearRange = ViewState["YearRangeViewState"];
                return (yearRange == null) ? "c-10:c+10" : yearRange.ToString();
            }
            set
            {
                ViewState["YearRangeViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("")
        ]
        public string YearSuffix
        {
            get
            {
                object yearSuffix = ViewState["YearSuffixViewState"];
                return (yearSuffix == null) ? string.Empty : yearSuffix.ToString();
            }
            set
            {
                ViewState["YearSuffixViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("Next")
        ]
        public string NextText
        {
            get
            {
                object nextText = ViewState["NextTextViewState"];
                return (nextText == null) ? "Next" : nextText.ToString();
            }
            set
            {
                ViewState["NextTextViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue("Prev")
        ]
        public string PrevText
        {
            get
            {
                object prevText = ViewState["PrevTextViewState"];
                return (prevText == null) ? "Prev" : prevText.ToString();
            }
            set
            {
                ViewState["PrevTextViewState"] = value;
            }
        }

        //enum
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(DatePickerShowOn.Focus)
        ]
        public DatePickerShowOn ShowOn
        {
            get
            {
                object showOn = ViewState["ShowOnViewState"];
                return (showOn == null) ? DatePickerShowOn.Focus : (DatePickerShowOn)showOn;
            }
            set
            {
                ViewState["ShowOnViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(DatePickerDuration.Normal)
        ]
        public DatePickerDuration Duration
        {
            get
            {
                object duration = ViewState["DurationViewState"];
                return (duration == null) ? DatePickerDuration.Normal : (DatePickerDuration)duration;
            }
            set
            {
                ViewState["DurationViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(DatePickerAnimation.Show)
        ]
        public DatePickerAnimation ShowAnimation
        {
            get
            {
                object showAnimation = ViewState["ShowAnimationViewState"];
                return (showAnimation == null) ? DatePickerAnimation.Show : (DatePickerAnimation)showAnimation;
            }
            set
            {
                ViewState["ShowAnimationViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(DatePickerMode.DatePicker)
        ]
        public DatePickerMode Mode
        {
            get
            {
                object mode = ViewState["ModeViewState"];
                return (mode == null) ? DatePickerMode.DatePicker : (DatePickerMode)mode;
            }
            set
            {
                ViewState["ModeViewState"] = value;
            }
        }
        #endregion
        #region int
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(0)
        ]
        public int FirstDay
        {
            get
            {
                object firstDay = ViewState["FirstDayViewState"];
                return (firstDay == null) ? 0 : Convert.ToInt32(firstDay);
            }
            set
            {
                ViewState["FirstDayViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(1)
        ]
        public int StepMonths
        {
            get
            {
                object stepMonths = ViewState["StepMonthsViewState"];
                return (stepMonths == null) ? 1 : Convert.ToInt32(stepMonths);
            }
            set
            {
                ViewState["StepMonthsViewState"] = value;
            }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(0)
        ]
        public int ShowCurrentAtPos
        {
            get
            {
                object showCurrentAtPos = ViewState["ShowCurrentAtPosViewState"];
                return (showCurrentAtPos == null) ? 0 : Convert.ToInt32(showCurrentAtPos);
            }
            set
            {
                ViewState["ShowCurrentAtPosViewState"] = value;
            }
        }
        #endregion
        #region arr

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

        #endregion
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
