using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing.Design;
using Atom.Web.UI.WebControls.Accordion;

namespace Atom.Web.UI.WebControls.Accordion // System.Wen.UI.DropDownList - Baev.Web.UI
{
    [
    AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("Items"),
    ParseChildren(true, "Items"),
    Designer(typeof(AccordionDesigner)),
    ToolboxData("<{0}:JQAccordion runat=\"server\"> </{0}:JQAccordion>")
    ]
    public class JQAccordion : WebControl, IPostBackEventHandler
    {
        //MS convention:
        //Fields
        //Events
        //Methods
        //Properties


        //Fields 
        #region Fields

        private int _selectedIndex;
        private int _selectedOldIndex;
        private List<Item> _itemList;
        private static readonly object EventSelectedIndexChanged = new object();
        private static readonly object EventSelectedIndexChanging = new object();

        public delegate void AccordionChangingEventHandler(object sender, AccordionChangingEventArgs e);
        public delegate void AccordionChangedEventHandler(object sender, AccordionChangedEventArgs e);

        #endregion

        [
        Category("Action"),
        Description("OnSelectedItemIndexChanging")
        ]
        public event AccordionChangingEventHandler SelectedIndexChanging
        {
            add
            {
                Events.AddHandler(EventSelectedIndexChanging, value);
            }
            remove
            {
                Events.RemoveHandler(EventSelectedIndexChanging, value);
            }
        }
        [
        Category("Action"),
        Description("OnSelectedItemIndexChanged")
        ]
        public event AccordionChangedEventHandler SelectedIndexChanged
        {
            add
            {
                Events.AddHandler(EventSelectedIndexChanged, value);
            }
            remove
            {
                Events.RemoveHandler(EventSelectedIndexChanged, value);
            }
        }

        //Methods
        #region Methods

        [
        Category("Action"),
        Description("")
        ]
        protected virtual void OnSelectedIndexChanging(AccordionChangingEventArgs e)
        {
            AccordionChangingEventHandler eventHandler = (AccordionChangingEventHandler)Events[EventSelectedIndexChanging];
            if (eventHandler != null)
            {
                this.SelectedIndex = e.OldIndex;
                eventHandler(this, e);

                if (!e.Cancel)
                {
                    this.SelectedIndex = e.NewIndex;
                    this.OnSelectedIndexChanged(new AccordionChangedEventArgs(e.NewIndex, e.OldIndex, this.SelectedItem, this));
                }
            }
        }

        [
        Category("Action"),
        Description("")
        ]
        protected virtual void OnSelectedIndexChanged(AccordionChangedEventArgs e)
        {
            AccordionChangedEventHandler eventHandler = (AccordionChangedEventHandler)Events[EventSelectedIndexChanged];
            if (eventHandler != null)
            {
                this.SelectedIndex = e.NewIndex;
                eventHandler(this, e);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            //Page.RegisterRequiresPostBack(this);
            string _clientPostBackHyperlink = Page.GetPostBackClientHyperlink(this, string.Empty);
            string _clientPostBackScript = Page.GetPostBackClientEvent(this, string.Empty);

            Page.ClientScript.RegisterStartupScript(typeof(Page), this.UniqueID + "startupscript", RenderStartupJavaScript());

            if ((!string.IsNullOrEmpty(this.IconSelectedUrl)) || (!string.IsNullOrEmpty(this.IconUrl)))
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), this.UniqueID + "startupstyle", RenderStartupStyle());
            }

            base.OnPreRender(e);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "hiddenValue");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "hiddenValue");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "selectedindex:" + this.SelectedIndex.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            if (this.Resizable)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "accResizer");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-widget-content");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "accordion");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            foreach (Item item in this.Items)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.H3);
                writer.AddAttribute("index", this.Items.IndexOf(item).ToString());
                writer.AddAttribute("href", "#");
                writer.RenderBeginTag(HtmlTextWriterTag.A);

                writer.Write(item.Header);

                writer.RenderEndTag();
                writer.RenderEndTag();

                //render item content
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                foreach (Control control in item.Controls)
                {
                    control.RenderControl(writer);
                }

                writer.RenderEndTag();
            }

            writer.RenderEndTag();

            if (this.Resizable)
            {
                writer.RenderEndTag();
            }

            //base.RenderContents(writer);
        }

        //clear span tag
        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderContents(writer);
        }

        private string RenderStartupStyle()
        {
            StringBuilder startupScript = new StringBuilder();
            startupScript.AppendFormat("<style type=\"text/css\">");

            if (!string.IsNullOrEmpty(this.IconSelectedUrl))
            {
                startupScript.AppendFormat("." + this.UniqueID + "IconSelectedStyle{{");
                startupScript.AppendFormat("background-image: url(" + this.IconSelectedUrl + ") !important;");
                startupScript.AppendFormat("}}");
            }

            if (!string.IsNullOrEmpty(this.IconUrl))
            {
                startupScript.AppendFormat("." + this.UniqueID + "IconStyle{{");
                startupScript.AppendFormat("background-image: url(" + this.IconUrl + ") !important;");
                startupScript.AppendFormat("}}");
            }

            startupScript.AppendFormat("</style>");
            return startupScript.ToString();
        }

        private string RenderStartupJavaScript()
        {
            StringBuilder startupScript = new StringBuilder();

            startupScript.AppendFormat("<script type=\"text/javascript\">");
            startupScript.AppendFormat("var " + this.UniqueID + "; ");
            startupScript.AppendFormat("$(document).ready(function() {{ ");
            startupScript.AppendFormat("var acc" + this.UniqueID + " = $('#" + this.UniqueID + "accordion'); " + this.UniqueID + "=acc" + this.UniqueID + ";");
            startupScript.AppendFormat("acc" + this.UniqueID + ".accordion({{");
            //Event
            //changestart
            startupScript.AppendFormat(" changestart: function(event, ui) {{ $('#" + this.UniqueID + "hiddenValue').val('selectedindex:'+ui.options.active); ");
            if (Events[EventSelectedIndexChanging] != null)
            {
                startupScript.AppendFormat(" __doPostBack('" + this.UniqueID + "', 'selectedindexchanging:'+ui.options.active+':'+ui.oldHeader.children('a#').attr('index')); ");
            }
            startupScript.AppendFormat("}}, ");
            //change
            if (Events[EventSelectedIndexChanged] != null)
            {
                if (Events[EventSelectedIndexChanging] == null)
                {
                    startupScript.AppendFormat(" changestart: function(event, ui) {{ __doPostBack('" + this.UniqueID + "', 'selectedindexchanged:'+ui.options.active+':'+ui.oldHeader.children('a#').attr('index'));}}, ");
                }
            }
            //AnimationType
            if (this.AnimationType == Animation.None)
            {
                startupScript.AppendFormat("animated: false, ");
            }
            if (this.AnimationType == Animation.Bounceslide)
            {
                startupScript.AppendFormat("animated: 'bounceslide', ");
            }
            if (this.AnimationType == Animation.Slide)
            {
                startupScript.AppendFormat("animated: 'slide', ");
            }
            //ItemChangeType
            if (this.ChangeType == ItemChangeType.MouseClick)
            {
                startupScript.AppendFormat("event: 'click', ");
            }
            if (this.ChangeType == ItemChangeType.MouseHover)
            {
                startupScript.AppendFormat("event: 'mouseover',");
            }
            //Icons
            startupScript.AppendFormat("icons:{{");
            if (!string.IsNullOrEmpty(this.IconUrl))
            {
                startupScript.AppendFormat(" 'header': '" + this.UniqueID + "IconStyle', ");
            }
            if (!string.IsNullOrEmpty(this.IconSelectedUrl))
            {
                startupScript.AppendFormat(" 'headerSelected': '" + this.UniqueID + "IconSelectedStyle' ");
            }
            else
            {
                startupScript.AppendFormat(" 'headerSelected': '" + this.UniqueID + "IconStyle', ");
            }
            startupScript.AppendFormat("}}, ");
            //SelectItem
            //startupScript.AppendFormat(" active: {0}, ", this.SelectedIndex);
            startupScript.AppendFormat(" active: parseInt($('#" + this.UniqueID + "hiddenValue').val().split(':')[1]), ");
            //Default:false
            if (this.FillSpace)
            {
                startupScript.AppendFormat("fillSpace: {0}, ", this.FillSpace.ToString().ToLower());
            }
            //Default:false
            if (this.Collapsible)
            {
                startupScript.AppendFormat("collapsible: {0}, ", this.Collapsible.ToString().ToLower());
            }
            //Default:false
            if (this.Navigation)
            {
                startupScript.AppendFormat("navigation: {0}, ", this.Navigation.ToString().ToLower());
            }
            //add event with if statement

            //Default:true
            if (!this.AutoHeight)
            {
                startupScript.AppendFormat("autoHeight: {0}, ", this.AutoHeight.ToString().ToLower());
            }
            startupScript.AppendFormat("}}); ");

            //Default:true
            if (!this.Enabled)
            {
                startupScript.AppendFormat("acc" + this.UniqueID + ".accordion('disable');");
            }
            if (this.Resizable)
            {
                startupScript.AppendFormat("$('#" + this.UniqueID + "accResizer" + "').resizable({{");
                startupScript.AppendFormat("resize: function() {{ acc" + this.UniqueID + ".accordion('resize');}},");
                startupScript.AppendFormat("minHeight: {0},", this.MinHeight.ToString());
                startupScript.AppendFormat("minWidth: {0},", this.MinWidth.ToString());
                startupScript.AppendFormat("maxHeight: {0},", this.MaxHeight.ToString());
                startupScript.AppendFormat("maxWidth: {0},", this.MaxWidth.ToString());
                startupScript.AppendFormat("}});");
            }
            //SelectItem
            //startupScript.AppendFormat("acc" + this.UniqueID + ".accordion('activate', " + this.SelectedIndex + ");");
            startupScript.AppendFormat("}})");
            startupScript.AppendFormat("</script>");

            return startupScript.ToString();

            //if (this.Page.Header.Controls.Add(new LiteralControl("<link rel="
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument != null)
            {
                if (eventArgument.StartsWith("selectedindexchanging"))
                {
                    string[] values = eventArgument.Split(':');

                    int selectedIndex = Convert.ToInt32(values[1]);
                    int selectedOldIndex = Convert.ToInt32(values[2]);

                    AccordionChangingEventArgs args = new AccordionChangingEventArgs(selectedIndex, selectedOldIndex);
                    this.OnSelectedIndexChanging(args);
                }
                if (eventArgument.StartsWith("selectedindexchanged"))
                {
                    string[] values = eventArgument.Split(':');

                    int selectedIndex = Convert.ToInt32(values[1]);
                    int selectedOldIndex = Convert.ToInt32(values[2]);

                    AccordionChangedEventArgs args = new AccordionChangedEventArgs(selectedIndex, selectedOldIndex, this.SelectedItem, this);
                    this.OnSelectedIndexChanged(args);
                }
            }
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.RaisePostBackEvent(eventArgument);
        }

        public virtual void ClearSelection()
        {
            foreach (Item item in this.Items)
            {
                item.Selected = false;
            }
        }

        protected override void OnInit(EventArgs e)
        {

            string hiddenValue = Page.Request.Params[this.UniqueID + "hiddenValue"];

            if ((!string.IsNullOrEmpty(hiddenValue)) && (hiddenValue.StartsWith("selectedindex:")))
            {
                this.SelectedIndex = Convert.ToInt32(hiddenValue.Split(':')[1]);
            }

            if (this.Resizable)
            {
                if (!this.FillSpace)
                {
                    throw new ArgumentException("Please set  FillSpace=\"True\"");
                }
            }

            bool isSelected = false;
            foreach (Item item in this.Items)
            {
                if (item.Selected)
                {
                    if (isSelected)
                    {
                        throw new ArgumentException("MultipleSelectionOfItems");
                    }
                    isSelected = true;
                    this._selectedIndex = this._itemList.IndexOf(item);
                }
            }

            base.OnInit(e);

        }

        #endregion

        //Properties
        #region Properties

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(0)
        ]
        public int MinHeight
        {
            get
            {
                object minHeight = ViewState["MinHeightViewState"];
                return (minHeight == null) ? 0 : Convert.ToInt32(minHeight);
            }
            set { ViewState["MinHeightViewState"] = value; }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(true)
        ]
        public int MinWidth
        {
            get
            {
                object minWidth = ViewState["MinWidthViewState"];
                return (minWidth == null) ? 0 : Convert.ToInt32(minWidth);
            }
            set { ViewState["MinWidthViewState"] = value; }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(true)
        ]
        public int MaxHeight
        {
            get
            {
                object maxHeight = ViewState["MaxHeightViewState"];
                return (maxHeight == null) ? 0 : Convert.ToInt32(maxHeight);
            }
            set { ViewState["MaxHeightViewState"] = value; }
        }
        [
        Category("Behavior"),
        Description(""),
        DefaultValue(true)
        ]
        public int MaxWidth
        {
            get
            {
                object maxWidth = ViewState["MaxWidthViewState"];
                return (maxWidth == null) ? 0 : Convert.ToInt32(maxWidth);
            }
            set { ViewState["MaxWidthViewState"] = value; }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(true)
        ]
        public bool AutoHeight
        {
            get
            {
                object autoHeightValue = ViewState["AutoHeightViewState"];
                return (autoHeightValue == null) ? true : (bool)autoHeightValue;
            }
            set
            {
                ViewState["AutoHeightViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue("")
        ]
        public string IconUrl
        {
            get
            {
                object icon = ViewState["IconUrlViewState"];
                return (icon == null) ? string.Empty : icon.ToString();
            }
            set
            {
                ViewState["IconUrlViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue("")
        ]
        public string IconSelectedUrl
        {
            get
            {
                object iconSelected = ViewState["IconSelectedUrlViewState"];
                return (iconSelected == null) ? string.Empty : iconSelected.ToString();
            }
            set
            {
                ViewState["IconSelectedUrlViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool Collapsible
        {
            get
            {
                object collapsibleValue = ViewState["CollapsibleViewState"];
                return (collapsibleValue == null) ? false : (bool)collapsibleValue;
            }
            set
            {
                ViewState["CollapsibleViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool Navigation
        {
            get
            {
                object navigationValue = ViewState["NavigationViewState"];
                return (navigationValue == null) ? false : (bool)navigationValue;
            }
            set
            {
                ViewState["NavigationViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(ItemChangeType.MouseClick)
        ]
        public ItemChangeType ChangeType
        {
            get
            {
                object fillSpaceValue = (object)ViewState["ItemChangeTypeViewState"];
                return (fillSpaceValue == null) ? ItemChangeType.MouseClick : (ItemChangeType)fillSpaceValue;
            }
            set
            {
                ViewState["ItemChangeTypeViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(Animation.None)
        ]
        public Animation AnimationType
        {
            get
            {
                object fillSpaceValue = (object)ViewState["AnimationTypeViewState"];
                return (fillSpaceValue == null) ? Animation.None : (Animation)fillSpaceValue;
            }
            set
            {
                ViewState["AnimationTypeViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool FillSpace
        {
            get
            {
                object fillSpaceValue = (object)ViewState["FillSpaceViewState"];
                return (fillSpaceValue == null) ? false : (bool)fillSpaceValue;
            }
            set
            {
                ViewState["FillSpaceViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DesignerSerializationVisibility(
            DesignerSerializationVisibility.Content),
        Editor(typeof(ItemCollectionEditor), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerDefaultProperty)
        ]
        public List<Item> Items
        {
            get
            {
                if (_itemList == null)
                {
                    _itemList = new List<Item>();
                }
                return _itemList;
            }
        }

        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Category(""),
        Description("")
        ]
        public int SelectedIndex
        {
            get
            {
                return this._selectedIndex;
            }
            set
            {
                this.ClearSelection();
                this._itemList[value].Selected = true;
                this._selectedIndex = value;
            }
        }

        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Category(""),
        Description("")
        ]
        public Item SelectedItem
        {
            get
            {
                int selectedIndex = this.SelectedIndex;
                if (selectedIndex >= 0)
                {
                    return this._itemList[selectedIndex];
                }
                return null;
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
                object resizableValue = (object)ViewState["ResizableViewState"];
                return (resizableValue == null) ? false : (bool)resizableValue;
            }
            set
            {
                ViewState["ResizableViewState"] = value;
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
        #endregion
    }
}
