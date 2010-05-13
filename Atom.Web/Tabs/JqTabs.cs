using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Security.Permissions;
using System.Web;
using System.Web.UI.WebControls;
using System.Drawing.Design;

namespace Atom.Web.UI.WebControls.Tabs
{
    [
    AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("Tabs"),
    ParseChildren(true, "Tabs"),
    Designer(typeof(TabsDesigner)),
    ToolboxData("<{0}:JQTabs runat=\"server\"> </{0}:JQTabs>")
    ]
    public class JQTabs : WebControl, IPostBackEventHandler
    {
        private int _selectedIndex;
        private int _selectedOldIndex;
        private List<Tab> _tabsList;

        private static readonly object EventSelectedIndexChanged = new object();
        private static readonly object EventSelectedIndexChanging = new object();

        private static readonly object EventTabRemoving = new object();
        private static readonly object EventTabRemoved = new object();

        private static readonly object EventTabMoved = new object();


        public delegate void TabsChangingEventHandler(object sender, TabsChangingEventArgs e);
        public delegate void TabsChangedEventHandler(object sender, TabsChangedEventArgs e);

        public delegate void TabsRemovingEventHandler(object sender, TabsRemovingEventArgs e);
        public delegate void TabsRemovedEventHandler(object sender, TabsRemovedEventArgs e);

        public delegate void TabsMovedEventHandler(object sender, EventArgs e);



        [
        Category("Action"),
        Description("OnSelectedItemIndexChanging")
        ]
        public event TabsChangingEventHandler SelectedIndexChanging
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
        public event TabsChangedEventHandler SelectedIndexChanged
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
        [
        Category("Action"),
        Description("OnTabRemoving")
        ]
        public event TabsRemovingEventHandler TabRemoving
        {
            add
            {
                Events.AddHandler(EventTabRemoving, value);
            }
            remove
            {
                Events.RemoveHandler(EventTabRemoving, value);
            }
        }
        [
        Category("Action"),
        Description("OnTabRemoved")
        ]
        public event TabsRemovedEventHandler TabRemoved
        {
            add
            {
                Events.AddHandler(EventTabRemoved, value);
            }
            remove
            {
                Events.RemoveHandler(EventTabRemoved, value);
            }
        }
        [
        Category("Action"),
        Description("OnTabMoved")
        ]
        public event EventHandler TabMoved
        {
            add
            {
                Events.AddHandler(EventTabMoved, value);
            }
            remove
            {
                Events.RemoveHandler(EventTabMoved, value);
            }
        }




        //Methods
        [
        Category("Action"),
        Description("")
        ]
        protected virtual void OnSelectedIndexChanging(TabsChangingEventArgs e)
        {
            TabsChangingEventHandler eventHandler = (TabsChangingEventHandler)Events[EventSelectedIndexChanging];
            if (eventHandler != null)
            {
                this.SelectedIndex = e.OldIndex;
                eventHandler(this, e);
                if (!e.Cancel)
                {
                    this.OnSelectedIndexChanged(new TabsChangedEventArgs(e.NewIndex, e.OldIndex, this.SelectedTab, this));
                }
            }
        }

        [
        Category("Action"),
        Description("")
        ]
        protected virtual void OnSelectedIndexChanged(TabsChangedEventArgs e)
        {
            TabsChangedEventHandler eventHandler = (TabsChangedEventHandler)Events[EventSelectedIndexChanged];
            if (eventHandler != null)
            {
                this.SelectedIndex = e.TabIndex;
                eventHandler(this, e);
            }
        }

        [
        Category("Action"),
        Description("")
        ]
        protected virtual void OnTabRemoving(TabsRemovingEventArgs e)
        {
            TabsRemovingEventHandler eventHandler = (TabsRemovingEventHandler)Events[EventTabRemoving];
            if (eventHandler != null)
            {
                eventHandler(this, e);
                if (!e.Cancel)
                {
                    this.OnTabRemoved(new TabsRemovedEventArgs(e.TabIndex, e.CurrentTab, e.JQTabs));
                }
            }
        }
        [
        Category("Action"),
        Description("")
        ]
        protected virtual void OnTabRemoved(TabsRemovedEventArgs e)
        {
            TabsRemovedEventHandler eventHandler = (TabsRemovedEventHandler)Events[EventTabRemoved];
            if (eventHandler != null)
            {
                this._tabsList.RemoveAt(e.TabIndex);
                eventHandler(this, e);
            }
        }
        [
        Category("Action"),
        Description("")
        ]
        protected virtual void OnTabMoved(EventArgs e)
        {
            EventHandler eventHandler = (EventHandler)Events[EventTabMoved];
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {

            string _clientPostBackHyperlink = Page.GetPostBackClientHyperlink(this, string.Empty);
            string _clientPostBackScript = Page.GetPostBackClientEvent(this, string.Empty);

            Page.ClientScript.RegisterStartupScript(typeof(Page), this.UniqueID + "startupscript", RenderStartupJavaScript());

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), this.UniqueID + "startupstyle", RenderStartupStyle());


            base.OnPreRender(e);
        }
        protected override void OnInit(EventArgs e)
        {
            string hiddenValue = Page.Request.Params[this.UniqueID + "hiddenValue"];

            if ((!string.IsNullOrEmpty(hiddenValue)) && (hiddenValue.StartsWith("selectedtab:")))
            {
                this.SelectedIndex = Convert.ToInt32(hiddenValue.Split(':')[1]);
            }

            base.OnInit(e);
        }
        private string RenderStartupStyle()
        {
            StringBuilder startupScript = new StringBuilder();

            startupScript.AppendFormat("<style type=\"text/css\">");
            if (this.TabAppearanceType == TabOrientation.Bottom)
            {
                startupScript.AppendFormat(".tabs-bottom {{ position: relative; }} ");
                startupScript.AppendFormat(".tabs-bottom .ui-tabs-panel {{ height: 140px; overflow: auto; }} ");
                startupScript.AppendFormat(".tabs-bottom .ui-tabs-nav {{ position: absolute !important; left: 0; bottom: 0; right:0; padding: 0 0.2em 0.2em 0; }} ");
                startupScript.AppendFormat(".tabs-bottom .ui-tabs-nav li {{ margin-top: -2px !important; margin-bottom: 1px !important; border-top: none; border-bottom-width: 1px; }}");
                startupScript.AppendFormat(".ui-tabs-selected {{ margin-top: -3px !important; }}");
            }
            if (this.TabAppearanceType == TabOrientation.Vertical)
            {
                startupScript.AppendFormat(".ui-tabs-vertical .ui-tabs-nav {{ padding: .2em .1em .2em .2em; float: left; width: 12em; }}");
                startupScript.AppendFormat(".ui-tabs-vertical .ui-tabs-nav li {{ clear: left; width: 100%; border-bottom-width: 1px !important; border-right-width: 0 !important; margin: 0 -1px .2em 0; }}");
                startupScript.AppendFormat(".ui-tabs-vertical .ui-tabs-nav li a {{ display:block; }}");
                startupScript.AppendFormat(".ui-tabs-vertical .ui-tabs-nav li.ui-tabs-selected {{ padding-bottom: 0; padding-right: .1em; border-right-width: 1px; border-right-width: 1px; }}");
                startupScript.AppendFormat(".ui-tabs-vertical .ui-tabs-panel {{ padding: 1em; float: right; width: 40em;}}");
            }
            if (this.TabsRemove)
            {
                if (string.IsNullOrEmpty(this.CloseIconUrl))
                {
                    startupScript.AppendFormat("#" + this.UniqueID + " li .ui-icon-close {{float: left; margin: 0.4em 0.2em 0 0;}}");
                }
                else
                {
                    startupScript.AppendFormat("#" + this.UniqueID + " li .ui-icon-close {{float: left; margin: 0.4em 0.2em 0 0; background-image:url('"
                        + this.CloseIconUrl + "')!important; background-position:center;}}");
                }
            }
            startupScript.AppendFormat("</style>");

            return startupScript.ToString();
        }

        private string RenderStartupJavaScript()
        {
            StringBuilder startupScript = new StringBuilder();

            startupScript.AppendFormat("<script type=\"text/javascript\">");
            startupScript.AppendFormat("$(document).ready(function() {{ ");
            startupScript.AppendFormat("var " + this.UniqueID + " = $('#" + this.UniqueID + "'); ");
            //tab appearance
            if (this.TabAppearanceType == TabOrientation.Bottom)
            {
                startupScript.AppendFormat(this.UniqueID + ".attr('class','tabs-bottom');");
            }
            startupScript.AppendFormat(this.UniqueID + ".tabs({{");
            //events
            //remove
            startupScript.AppendFormat(" remove: function(event, ui) {{ ");
            if (Events[EventTabRemoving] != null)
            {
                startupScript.AppendFormat(" __doPostBack('"
                    + this.UniqueID + "','tabremoving:'+$(ui.tab).attr('href').split('-')[1]); ");
            }
            if (Events[EventTabRemoved] != null)
            {
                if (Events[EventTabRemoving] == null)
                {
                    startupScript.AppendFormat(" __doPostBack('"
                        + this.UniqueID + "','tabremoved:'+$(ui.tab).attr('href').split('-')[1]); ");
                }
            }
            startupScript.AppendFormat(" }},");
            //select
            startupScript.AppendFormat(" select: function(event, ui){{ $('#"
                + this.UniqueID + "hiddenValue').val('selectedtab:'+$(ui.panel).attr('id').split('-')[1]); ");
            if (Events[EventSelectedIndexChanging] != null)
            {
                startupScript.AppendFormat(" __doPostBack('" + this.UniqueID + "','selectedindexchanging:'+parseInt($('#"
                    + this.UniqueID + "hiddenValue').val().split(':')[1])+':'+$('#"
                    + this.UniqueID + "').tabs('option', 'selected'));");
            }
            if (Events[EventSelectedIndexChanged] != null)
            {
                if (Events[EventSelectedIndexChanging] == null)
                {
                    startupScript.AppendFormat("__doPostBack('" + this.UniqueID + "','selectedindexchanged:'+parseInt($('#"
                        + this.UniqueID + "hiddenValue').val().split(':')[1])+':'+$('#"
                        + this.UniqueID + "').tabs('option', 'selected'));");
                }
            }
            startupScript.AppendFormat(" }},");
            //tabs remove
            if (this.TabsRemove)
            {
                startupScript.AppendFormat(" tabTemplate:\"<li><a href='#{{href}}'>#{{label}}</a> <span class='ui-icon ui-icon-close'>Remove Tab</span></li>\",");
            }
            //collapsible
            if (this.Collapsible)
            {
                startupScript.AppendFormat("collapsible: {0},", this.Collapsible.ToString().ToLower());
            }
            //animation and animation speed
            if (this.Animate == TabAnimationType.Content)
            {
                startupScript.AppendFormat(" fx:{{opacity: 'toggle' ,duration: {0} }},", this.AnimationSpeed.ToString().ToLower());
            }
            if (this.Animate == TabAnimationType.Height)
            {
                startupScript.AppendFormat(" fx:{{height: 'toggle' ,duration: {0} }},", this.AnimationSpeed.ToString().ToLower());
            }
            if (this.Animate == TabAnimationType.HeightAndContent)
            {
                startupScript.AppendFormat(" fx:{{height: 'toggle', opacity: 'toggle' ,duration: {0} }},", this.AnimationSpeed.ToString().ToLower());
            }
            //tab change type
            if (this.ChangeType == TabChangeType.MouseHover)
            {
                startupScript.AppendFormat(" event: 'mouseover',");
            }
            //selected tab
            startupScript.AppendFormat(" selected: parseInt($('#" + this.UniqueID + "hiddenValue').val().split(':')[1]), ");
            startupScript.AppendFormat("}})");

            //tab appearance
            if (this.TabAppearanceType == TabOrientation.Top)
            {
                startupScript.AppendFormat(";");
            }
            if (this.TabAppearanceType == TabOrientation.Bottom)
            {
                startupScript.AppendFormat(";");
                startupScript.AppendFormat(" $('.tabs-bottom .ui-tabs-nav, .tabs-bottom .ui-tabs-nav > *')");
                startupScript.AppendFormat(".removeClass('ui-corner-all ui-corner-top')");
                startupScript.AppendFormat(".addClass('ui-corner-bottom'); ");
            }
            if (this.TabAppearanceType == TabOrientation.Vertical)
            {
                startupScript.AppendFormat(".addClass('ui-tabs-vertical ui-helper-clearfix'); $('#"
                    + this.UniqueID + " li').removeClass('ui-corner-top').addClass('ui-corner-left'); ");
            }
            //tab remove js code
            if (this.TabsRemove)
            {
                startupScript.AppendFormat("$('#" + this.UniqueID + " span.ui-icon-close').live('click', function() {{");
                startupScript.AppendFormat("var index = $('li'," + this.UniqueID + ").index($(this).parent());");
                startupScript.AppendFormat(this.UniqueID + ".tabs('remove', index);}});");
            }

            //tabs moving
            if (this.TabsMoving)
            {
                startupScript.AppendFormat("$('#" + this.UniqueID + "').find('.ui-tabs-nav').sortable({{ ");

                if (Events[EventTabMoved] != null)
                {
                    startupScript.AppendFormat("stop: function(event, ui) {{ var oldPostion=$(ui.item).children('a').attr('href').split('-')[1];");
                    startupScript.AppendFormat("var newPostion=$(" + this.UniqueID + ").tabs().find('.ui-tabs-nav li').sortable('toArray').index(ui.item);");
                    startupScript.AppendFormat("__doPostBack('" + this.UniqueID + "','tabmoved:'+oldPostion+':'+newPostion);}},");
                }

                //moving orientation
                if (this.TabAppearanceType == TabOrientation.Vertical)
                {
                    startupScript.AppendFormat(" axis:'y'}})");
                }
                else
                {
                    startupScript.AppendFormat(" axis:'x'}})");
                }
            }

            startupScript.AppendFormat("}})");
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
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "selectedtab:" + this.SelectedIndex.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            foreach (Tab tab in this._tabsList)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + this.UniqueID + "-" + this.Tabs.IndexOf(tab).ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.A);

                writer.Write(tab.Header);

                writer.RenderEndTag();
                if (this.TabsRemove)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-icon ui-icon-close");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write("Remove Tab");
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();

            }
            writer.RenderEndTag();

            foreach (Tab tab in this._tabsList)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "-" + this.Tabs.IndexOf(tab).ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                foreach (Control control in tab.Controls)
                {
                    control.RenderControl(writer);
                }
                writer.RenderEndTag();
            }
            writer.RenderEndTag();


            base.RenderContents(writer);
        }

        public virtual void ClearSelection()
        {
            foreach (Tab item in this._tabsList)
            {
                item.Selected = false;
            }
        }

        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument != null)
            {
                //selecting event
                if (eventArgument.StartsWith("selectedindexchanging"))
                {
                    string[] values = eventArgument.Split(':');

                    int newIndex = Convert.ToInt32(values[1]);
                    int oldIndex = Convert.ToInt32(values[2]);

                    TabsChangingEventArgs args = new TabsChangingEventArgs(newIndex, oldIndex, this.SelectedTab, this);
                    this.OnSelectedIndexChanging(args);
                }
                //selected event
                if (eventArgument.StartsWith("selectedindexchanged"))
                {
                    string[] values = eventArgument.Split(':');

                    int index = Convert.ToInt32(values[1]);
                    int oldIndex = Convert.ToInt32(values[2]);

                    TabsChangedEventArgs args = new TabsChangedEventArgs(index, oldIndex, this.SelectedTab, this);
                    this.OnSelectedIndexChanged(args);
                }
                //removing event
                if (eventArgument.StartsWith("tabremoving"))
                {
                    string[] values = eventArgument.Split(':');
                    int tabIndex = Convert.ToInt32(values[1]);

                    TabsRemovingEventArgs args = new TabsRemovingEventArgs(tabIndex, this.Tabs[tabIndex], this);
                    this.OnTabRemoving(args);
                }
                //removed event
                if (eventArgument.StartsWith("tabremoved"))
                {
                    string[] values = eventArgument.Split(':');
                    int tabIndex = Convert.ToInt32(values[1]);
                    TabsRemovedEventArgs args = new TabsRemovedEventArgs(tabIndex, this.Tabs[tabIndex], this);
                    this.OnTabRemoved(args);
                }
                //moved event
                if (eventArgument.StartsWith("tabmoved"))
                {
                    string[] values = eventArgument.Split(':');

                    int oldPosition = Convert.ToInt32(values[1]);
                    int newPosition = Convert.ToInt32(values[2]);

                    ReorderTabItems(oldPosition, newPosition);

                    this.OnTabMoved(new EventArgs());
                }
            }
        }

        private void ReorderTabItems(int oldPosition, int newPosition)
        {
            Tab temp = this._tabsList[oldPosition];
            this._tabsList[oldPosition] = this._tabsList[newPosition];
            this._tabsList[newPosition] = temp;
        }



        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.RaisePostBackEvent(eventArgument);
        }

        #endregion

        //Properties
        [
        Category("Behavior"),
        Description(""),
        DesignerSerializationVisibility(
            DesignerSerializationVisibility.Content),
        Editor(typeof(TabCollectionEditor), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerDefaultProperty)
        ]
        public List<Tab> Tabs
        {
            get
            {
                if (this._tabsList == null)
                {
                    this._tabsList = new List<Tab>();
                }
                return _tabsList;
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
                this._tabsList[value].Selected = true;
                this._selectedIndex = value;
            }
        }

        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Category(""),
        Description("")
        ]
        public Tab SelectedTab
        {
            get
            {
                int selectedIndex = this.SelectedIndex;
                if (selectedIndex >= 0)
                {
                    return this._tabsList[selectedIndex];
                }
                return null;
            }
        }




        [
        Category("Behavior"),
        Description(""),
        UrlProperty(""),
        DefaultValue("")
        ]
        public string CloseIconUrl
        {
            get
            {
                object icon = ViewState["CloseIconUrlViewState"];
                return (icon == null) ? string.Empty : icon.ToString();
            }
            set
            {
                ViewState["CloseIconUrlViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(TabAnimationType.None)
        ]
        public TabAnimationType Animate
        {
            get
            {
                object animation = (object)ViewState["AnimateViewState"];
                return (animation == null) ? TabAnimationType.None : (TabAnimationType)animation;
            }
            set
            {
                ViewState["AnimateViewState"] = value;
            }
        }


        [
        Category("Behavior"),
        Description(""),
        DefaultValue(TabOrientation.Top)
        ]
        public TabOrientation TabAppearanceType
        {
            get
            {
                object appearance = (object)ViewState["TabAppearanceTypeViewState"];
                return (appearance == null) ? TabOrientation.Top : (TabOrientation)appearance;
            }
            set
            {
                ViewState["TabAppearanceTypeViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(TabChangeType.MouseClick)
        ]
        public TabChangeType ChangeType
        {
            get
            {
                object changeType = (object)ViewState["TabChangeTypeViewState"];
                return (changeType == null) ? TabChangeType.MouseClick : (TabChangeType)changeType;
            }
            set
            {
                ViewState["TabChangeTypeViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(TabDuration.Normal)
        ]
        public TabDuration AnimationSpeed
        {
            get
            {
                object animationSpeed = (object)ViewState["AnimationSpeedViewState"];
                return (animationSpeed == null) ? TabDuration.Normal : (TabDuration)animationSpeed;
            }
            set
            {
                ViewState["AnimationSpeedViewState"] = value;
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
                object collapsible = (object)ViewState["CollapsibleViewState"];
                return (collapsible == null) ? false : (bool)collapsible;
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
        public bool TabsRemove
        {
            get
            {
                object tabsRemove = (object)ViewState["TabsRemoveViewState"];
                return (tabsRemove == null) ? false : (bool)tabsRemove;
            }
            set
            {
                ViewState["TabsRemoveViewState"] = value;
            }
        }

        [
        Category("Behavior"),
        Description(""),
        DefaultValue(false)
        ]
        public bool TabsMoving
        {
            get
            {
                object tabsMoving = (object)ViewState["TabsMovingViewState"];
                return (tabsMoving == null) ? false : (bool)tabsMoving;
            }
            set
            {
                ViewState["TabsMovingViewState"] = value;
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
