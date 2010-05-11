{
/**
 * jQuery 1.2 based grid view plugin
 *
 * Project code name: jqGridView
 * Copyright (c) 2007 Victor Bartel
 *
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 *
 * - Project description:
 * jqGridView is client-rich XML-based, 100% ajaxed gridview plugin for jQuery library.
 * jqGridView provides professional solution for representing and editing tabular data on the web.
 * Carefully designed, with powerful script API,
 * this editable DHTML grid is easy configurable with XML,
 * and shows convincing results working with large amounts of data.
 * jqGridView allows easy implementation of nice looking(managed through css).
 *
 * - Features:
 * Column resizing,
 * Column Sorting,
 * Paging,
 * Filtering,
 * Row eding
 *
 * - Browsers support:
 * Opera 9.0+
 * Firefox 1.5+
 * Safari 2.0 +
 * IE 6.0+
 *
 * God bless Russia!
 *
 * @author Victor Bartel
 * @version 1.0.0
 *
 */
}
$.fn.jqGridView = function(cfg) {
	var gridView = function(){
		return  {
			build : function(div,cfg) {
				// check edit mode
				if(cfg.editMode) return;
				if(cfg.modifiedRows!=undefined) {
					if(cfg.modifiedRows.length>0) {
						$(cfg.messagesLayer)
							.find('.gridInsertPanel_body').empty()
							.append('<b>'+cfg.messages['saveChanges']+'</b>');
						$(cfg.messagesLayer)
							.find('#btnYes').unbind('click')
							.bind('click',function(e){
								$(cfg.messagesLayer).hide('fast');
								gridView.saveModifications(e,cfg);
							});
						$(cfg.messagesLayer)
							.find('#btnNo').unbind('click')
							.bind('click',function(e){
								$(cfg.messagesLayer).hide('fast');
								cfg.modifiedRows = null;
								gridView.build($(cfg.table).parent()[0],cfg);
								return false;
							});
						$(cfg.messagesLayer).show('slow');
						return;
					}
				}
				var config = {
					'width' : cfg.width,
					'url' : cfg.url,
					'minWidth' : cfg.minWidth,
					'splitBar' : cfg.splitBar,
					'headLayer' : cfg.headLayer,
					'loadingLayer' : cfg.loadingLayer,
					'filterLayer' : cfg.filterLayer,
					'editLayer' : cfg.editLayer,
					'insertLayer' : cfg.insertLayer,
					'table': cfg.table,
					'imagesUrl': cfg.imagesUrl||'./',
					'save' : cfg.save,
					'itemInsert': cfg.itemInsert,
					'itemDelete': cfg.itemDelete
				};
				// delete modification indicators
				$(div).find('div[@id^="dirty"]').remove();
				// create loading layer
				if(!config.loadingLayer) {
					$(div).empty();
					var table = document.createElement('table');
					var tr = document.createElement('tr');
					var td = document.createElement('td');
					$(tr).append(td);
					$(table).append(tr);
					$(table).attr('id',gridView.generateId('tab'));
					$(div).append(table);
					config.loadingLayer = table;
					$(config.loadingLayer).addClass('gridLoader');
					// create inside div
					var newDiv = document.createElement('div');
					$(newDiv).append('<img src="'+config.imagesUrl+'ajax-loader.gif">');
					$(td).append(newDiv);
					$(config.loadingLayer)
						.bind('selectstart',gridView.stopSelect)
						.bind('mousedown',gridView.stopSelect);
					//$(config.loadingLayer).hide();
				}
				// parameter loading indicator
				$(config.loadingLayer).css({
					cursor: 'default',
					//width: ((config.table)?$(config.table).width():config.width)+'px',
					height:((config.table && $(config.table).get(0).offsetHeight > 50)
									?$(config.table).get(0).offsetHeight:300)+'px',
					width: ((config.table && $(config.table).get(0).offsetWidth > 50)
									?$(config.table).get(0).offsetWidth:config.width)+'px'
				});
				// check if main table exists yet
				if($(div).find("table[@id^='jqGridView']").length > 0) {
					var previousGrid = $(div).find("table[@id^='jqGridView']").get(0);
					$(config.loadingLayer).insertAfter(previousGrid);
					$(previousGrid).attr('id','previousGrid');
					$(previousGrid).hide();
					// remove previous message
					$(div).find('#message').remove();
					// show loading indicator
					$(config.loadingLayer).show();
				}
				// send ajax request
				$.ajax({
					data: cfg.params,
					type: "get",
					url: config.url,
					success: function(xml){
						// destroy previous config object
						cfg = null;
						// get previous columns width
						getPrevWidths(config);
						// console.info('after getPrevWidths');
						buildSkeleton(div,config);
						buildHeader(xml,config);
						buildBody(xml,config);
						buildGridTools(xml,config);
						// hide loading indicator
						$(config.loadingLayer).hide();
						$('#'+config.tableId).show();
						// check response
						checkResponseState(xml,config);
						gridView.init(config);
					}
				});
				function getPrevWidths(config) {
					if(!config.table) {return;}
					config.prevWidths = [];
					$(config.table).find('tr:first').children().each(function(i){
						config.prevWidths[i] = parseInt($(this).css('width'),10);
					});
				}
				function buildSkeleton(div,config) {
					// build table skeleton
					var table = document.createElement("table");
					//var newDiv = document.createElement('div');
					var newId = gridView.generateId("jqGridView");
					config.tableId = newId;
					$(table).addClass("grid");
					$(table).attr("id",newId);
					$(table).attr("cellspacing","0");
					$(table).attr("cellpadding","0");
					$(table).hide();
					$(table).append(document.createElement("thead"));
					$(table).append(document.createElement("tbody"));
					$(table).css({width : config.width||500+'px'});

					// check if main table exists yet
					if($(div).find("table[@id='previousGrid']").length < 1) {
						$(table).css({display:'none'});
						$(div).append(table);
						// preload dirty image
						var img = document.createElement('img');
						$(img).attr('src',config.imagesUrl+'dirty.gif');
						$(img).css({
							position: 'absolute',
							left: '-10',
							top: '-10'
						});
						//TODO Image preloading
						// add hidden "order by" describer
						input = document.createElement('input');
						$(input).attr('type','hidden');
						$(input).attr('id',gridView.generateId('orderby'));
						$(div).append(input);
					} else {
						$(table).insertAfter($(div).find("table[@id='previousGrid']")[0]);
						$(div).find("table[@id='previousGrid']").remove();
					}
				}
				function buildHeader(xml,config) {
					// build grid header
					var tr = document.createElement('tr');
					var thead = $('#'+config.tableId).find('thead');
					config.headers = [];

					$(xml).find('header>element').each(function(i){
						// restore previous width of each column
						if(config.prevWidths) {
							var prevWidth = config.prevWidths[i];
						}
						// extend config object
						config.headers[i] = {
							title : $(this).attr('title'),
							'name' : $(this).attr('name'),
							initWidth : prevWidth||$(this).attr('width'),
							resizable : parseInt($(this).attr('resizable')||1,10),
							sortable : parseInt($(this).attr('sortable')||1,10),
							isId : parseInt($(this).attr('isId')||0,10),
							contentAlign : $(this).attr('contentAlign')||'left',
							editTemplateId: $(this).attr('editTemplateId'),
							itemTemplateId: $(this).attr('itemTemplateId'),
							order:
								($(this).attr('name') ==
								$(xml).find('parameters>orderBy').text())
								? $(xml).find('parameters>order').text()
								: 'desc'
						};
						// create new th element
						var th = document.createElement('th');
						var div = document.createElement('div');
						$(th).append(div);
						$(div).css({
							'overflow': 'hidden',
							'white-space': 'nowrap'
						});
						// check if the header column is resizable
						if(config.headers[i].resizable == 1) {
							// create resize handle
							var sizeDiv = document.createElement('div');
							$(sizeDiv).attr('id',gridView.generateId('size_div'));
							$(sizeDiv).css({'float': 'right'});
							$(sizeDiv).addClass('gridResizer');
							$(div).before(sizeDiv);
						}
						$(div).attr('align','center');
						// assign width to th style
						if(config.headers[i].initWidth){
							$(th).css({width:config.headers[i].initWidth+'px'});
						}
						// add content to header
						$(div).append($(this).attr('title')||"&nbsp;");
						// add order image
						if(config.headers[i].name ==
							$('#'+config.tableId).parent().find("input[@id^='orderby']").val()) {
							$(div).css({
								'background': 'url('+
								config.imagesUrl+config.headers[i].order.toLowerCase()+
								'.gif) no-repeat center right'
							});
						}
						// add new th
						$(th).addClass('grid_th');
						$(tr).append(th);
					});
					// append generated header to DOM tree
					$(thead).append(tr);
				}
				function buildBody(xml,config) {
					// get additional parameters
					config.orderBy = $(xml).find('orderBy').text();
					config.order = $(xml).find('order').text();
					config.currentPage = parseInt($(xml).find('currentPage').text())||1;
					config.totalPages = parseInt($(xml).find('totalPages').text())||1;
					config.totalItems = parseInt($(xml).find('totalItems').text());
					config.currentFilter = {
					  name: $(xml).find('currentFilter').attr('name'),
					  operator: $(xml).find('currentFilter').attr('operator'),
					  value: $(xml).find('currentFilter').attr('value'),
					  isQuoted :$(xml).find('currentFilter').attr('isQuoted') || 0
					};
					config.rows = [];

					var tbody = $('#'+config.tableId).find('tbody');
					// build table row
					$(xml).find('row').each(function(i){
						var tr = document.createElement('tr');
						var row = $(this);
						// fill rows array
						config.rows[i] = [];
						$(this).find('cell').each(function(j) {
							config.rows[i][$(this).attr('name')] = $(this).text();
						});
						for(var j=0;j<config.headers.length;j++) {
							// build table cell
							var td = document.createElement('td');
							var div = document.createElement('div');
							$(div).attr('align',config.headers[j].contentAlign);
							// check if row is editable
							if(config.headers[j].editTemplateId) {
								$(div).css({'cursor':'pointer'});
								$(div).bind('dblclick',buildEditTemplate);
							}
							$(div).css({
								'overflow': 'hidden',
								'white-space': 'nowrap'
							});
							// check for template column
							//debugger
							if(config.headers[j].itemTemplateId) {
								var templateId = config.headers[j].itemTemplateId;
								var template = $(xml)
									.find("itemTemplates>template[@id='"+templateId+"']>body")
									.text();
								var templateNode = $(xml)
									.find("itemTemplates>template[@id='"+templateId+"']");
								// assign params value
								$(templateNode).find('param').each(function(k){
									// prepare template params
									//var value = $(row).find("cell[name='"+$(this).attr('value')+"']").text();
									var value = config.rows[i][$(this).attr('value')];
									var keyRegEx = new RegExp('{'+$(this).attr('key')+'}','gi');
									template = template.replace(keyRegEx,value);
								});
								$(div).append(template);
							}
							else{
								$(div).append($(row).find("cell[@name='"+config.headers[j].name+"']").text());
							}
							$(td).append(div);
							$(tr).append(td);
							$(td).addClass('grid_td');
						}
						$(tbody).append(tr);
					});
				}
				function buildGridTools(xml,config) {
					// get messages
					config.messages = [];
					config.messages['saveChanges'] =
						$(xml).find('messages>saveChanges').text() || 'Save changes?';
					config.messages['deleteLine'] =
						$(xml).find('messages>deleteLine').text() || 'Delete line ?'
					// create filters container
					config.filters = [];
					// build filter descriptor
					$(xml).find('filter').each(function(i){
						config.filters[i] = {
							'id': $(this).attr('id'),
							value: $(this).attr('value'),
							title: $(this).attr('title')||$(this).attr('value'),
							isQuoted: $(this).attr('isQuoted')||0
						};
						// create operators container
						config.filters[i].operators = [];
						$(this).find('operator').each(function(j){
							config.filters[i].operators[j] = {
								title : $(this).attr('title')||$(this).attr('value'),
								value : $(this).attr('value')
							};
						});
					});
					// create panel entries list
					config.panelEntries = [];
					$(xml).find('topPanel>entry').each(function(i){
						var entry = {
							type : $(this).attr('type') || 'custom',
							value: $(this).text()
						};
						config.panelEntries[i] = entry;
					});
					// footer translations
					config.translations = [];
					$(xml).find('translations>add').each(function(i){
						config.translations[$(this).attr('key').toLowerCase()] = $(this).attr('value');
					});
					// retrive edit templates
					config.editTemplates = [];
					$(xml).find('editTemplates>template').each(function(i){
						var tid = $(this).attr('id');
						config.editTemplates[tid] = {
							type: $(this).attr('type')
						};
						switch(config.editTemplates[tid].type) {
							case 'textBox' :
								break;
							case 'checkBox' :
								config.editTemplates[tid].entries = [];
								$(this).find('content entry').each(function(j){
									config.editTemplates[tid].entries[j] = {
										text: $(this).attr('text'),
										value: $(this).attr('value')
									}
								});
								break;
							case 'dropDownList' :
								config.editTemplates[tid].entries = [];
								config.editTemplates[tid].valueColumn = $(this).attr('valueColumn');
								$(this).find('content entry').each(function(j){
									config.editTemplates[tid].entries[j] = {
										text: $(this).attr('text')||'...',
										value: $(this).attr('value')
									}
								});
								break;
						}
					})
					// retrive insert template
					$(xml).find('insertTemplates>template').each(function(i){
						config.insertTemplate = $.trim($(this).text());
					});
				}
				function checkResponseState(xml,config) {
					// clone loading layer
					var messageLayer = $(config.loadingLayer).clone();
					// modify layer content
					$(messageLayer).find('div').empty();
					$(messageLayer).attr('id','message');
					// check if result is well-formed
					if($(xml).find('gridViewData').children().size() == 0){
						// add message text
						$(messageLayer).find('div')
							.append('<b style="color:red;">Error: XML data file is not well-formed !</b>');
						// hide main table
						$('#'+config.tableId).hide();
						$('#'+config.tableId).after(messageLayer);
						$(messageLayer).show();
					}
					// check for serverside errors
					else if($(xml).find('messages>errorMessage').text().length > 0) {
						// add message text
						$(messageLayer).find('div').append($(xml).find('messages>errorMessage').text());
						// hide main table
						$('#'+config.tableId).hide();
						$('#'+config.tableId).after(messageLayer);
						$(messageLayer).show();
					}
					else if($(xml).find('parameters>totalItems').text() == '0') {
						// add message text
						$(messageLayer).find('div').append($(xml).find('messages>noResultsMessage').text());
						// hide main table
						$('#'+config.tableId).hide();
						$('#'+config.tableId).after(messageLayer);
						$(messageLayer).show();
					}
				}
				function buildEditTemplate(e) {
					if(!config.editMode){
						// get current column index
						var eTarget = e.target;
						var cellIndex = $(eTarget).parent().get(0).cellIndex;
						var rowIndex = $(eTarget).parent().parent().parent().children()
							.index($(eTarget).parent().parent()[0]);
						// retrive current template
						var template = config.editTemplates[config.headers[cellIndex].editTemplateId];
						// set edit mode
						config.editMode = true;
						// change cell content
						var tmpContent = $(eTarget).text();
						// prepare cell
						$(eTarget).empty();
						// prepare item object
						config.cancelEditMode = function(e){
							switch(template.type) {
								case 'textBox':
									var value = $(eTarget).find(':input').val();
									$(eTarget).empty();
									$(eTarget).append(value);
									// change local datasource
									break;
								case 'checkBox':
									var checkBox = $(eTarget).find(':input')[0];
									var values = [];
									$(eTarget).empty();
									$.each(template.entries,function(i,n){
										values[n.value] = n.text;
									});
									if(checkBox.checked) {
										$(eTarget).append(values['checked']);
									} else {
										$(eTarget).append(values['unchecked']);
									}
									break;
								case 'dropDownList':
									var option = $(eTarget).find('option:selected')[0];
									$(eTarget).empty();
									$(eTarget).append(option.text);
									if(template.valueColumn) {
										config.rows[rowIndex][template.valueColumn] = option.value;
									}
									break;
							}
							// change local datasource
							config.rows[rowIndex][config.headers[cellIndex].name] = $.trim($(eTarget).text());
							config.editMode = false;
						};
						config.startModication = function(e){
							if(typeof(config.save)=='function') {
								if(!config.dirties)config.dirties = {td:[],div:[]};
								gridView.showDirtyIcon(config,$(eTarget).parent().get(0));
								if(!config.modifiedRows) config.modifiedRows = [];
								config.modifiedRows[rowIndex] = rowIndex;
							}
						};
						var css = {
							width:($(eTarget).width()) + 'px',
							'font-size': $(eTarget).css('font-size')
						};
						// check template type
						switch(template.type) {
							case 'textBox':
								var input = document.createElement('input');
								$(input).attr('type','text');
								$(input).addClass('gridInlineEditor');
								$(input).css(css);
								$(input).val(config.rows[rowIndex][config.headers[cellIndex].name]);
								$(eTarget).append(input);
								$(input).bind('blur',config.cancelEditMode);
								$(input).bind('change',config.startModication);
								input.focus();
								break;
							case 'checkBox':
								var input = document.createElement('input');
								$(input).attr('type','checkbox');
								$(input).css(css);
								$(input).css({width:'auto'});
								$.each(template.entries,function(i,n){
									if($.trim(tmpContent) == template.entries[i].text) {
										if(template.entries[i].value == 'checked') {
											$(input).attr('checked','checked');
										}
										$(eTarget).append(input);
										$(input).bind('blur',config.cancelEditMode);
										$(input).bind('change',config.startModication);
										input.focus();
									}
								});
								break;
							case 'dropDownList':
								var select = document.createElement('select');
								$(select).css(css);
								$(select).css({width:($(eTarget).width()-5) + 'px'})
								$.each(template.entries,function(i,n){
									var option = document.createElement('option');
									$(option).text(template.entries[i].text);
									$(option).val(template.entries[i].value);
									$(select).append(option);
									if($.trim(tmpContent) == template.entries[i].text){
										$(select).val(template.entries[i].value);
									}
								});
								$(select).bind('blur',config.cancelEditMode);
								$(select).bind('change',config.startModication);
								$(eTarget).append(select);
								select.focus();
								break;
						}
					}
				}
			},
			init : function(config) {
				config.table = $('#'+config.tableId);
				config.tableLeft = $(config.table)[0].offsetLeft;
				config.tableRight = $(config.table)[0].offsetLeft
				+ parseInt($(config.table).css('width'),10);
				initTable();
				initChildElements();
				config.reload = reload;
				function initChildElements() {
					var div = null;
					// Add header and footer
					createTopPanel();
					createBottomPanel();
					if(!config.splitBar) {
						// create resizer bar
						var div = document.createElement('div');
						$(div).attr('id',gridView.generateId('div'));
						$(config.table).parent().append(div);
						config.splitBar = div;
						$(config.splitBar).hide();
						$(config.splitBar).css({
							cursor:'e-resize',
							position:'absolute',
							'background': '#000',
							'width':'1px'
						});
					}
					if(!config.headLayer) {
						// create additional header layer
						var div = document.createElement('div');
						$(div).attr('id',gridView.generateId('div'));
						$(config.table).parent().append(div);
						config.headLayer = div;
						$(config.headLayer).css({
							'background':'url('+config.imagesUrl+'transparent.gif)',
							cursor: 'e-resize',
							position: 'absolute',
							width: $(config.table).width()+'px',
							height: ($(config.table).find('tr:first').height()*2)+'px',
							top: $(config.table)[0].offsetTop,
							left: $(config.table)[0].offsetLeft,
							display: 'none'
						});
						//$(config.headLayer).hide();
					}
					createFilterLayer();
					createInsertLayer();
					createMessagesLayer();
				}
				function initTable() {
					// init resize handles
					$(config.table).find('div[@id^=size_div]').each(function(i){
						$(this).css({
							//"background-color": 'gray',
							height: $(this).parent().height()+'px'
						});
						// remove for non-resizable columns
						if($(this).parent().next().find('div[@id^=size_div]').length == 0
							 && i <=($(config.table).find('div[@id^=size_div]').length-1)){
							$(this).hide();
						}
					});
					$(config.table).find('.gridResizer').bind('mousedown',startResize);
					$(config.table).find('th').each(function(i){
						// setting th width to style
						var width = parseInt($(this).css('width'),10)||$(this).width();
						var index = $(this).parent().children().index($(this)[0]);
						// Add header class
						$(this).addClass('grid_th_split');
						// bind column with click handler function
						if(config.headers[i].sortable == 1) {
							$(this).find('div[@id=""]').bind('click',sort);
						}
						$(this).find('div[@id=""]').css({cursor:'pointer'});
						// setting td width to style
						$(config.table).find('tr').each(function(j){
							$($(this).children().get(index))
								.css({'width':width+'px'});
						});
					});
					$(config.table).find('th:last').removeClass('grid_th_split');
					$(config.table).find('th:last').addClass('grid_th_last');
					$(config.table).find("tr:nth-child(odd)").addClass("grid_tr_odd");
					$(config.table).find('tbody>tr').bind('mouseover',function(e){
						$(this).addClass('grid_tr_hover');
					});
					$(config.table).find('tbody>tr').bind('mouseout',function(e){
						$(this).removeClass('grid_tr_hover');
					});
					$(config.table).find('tbody tr').bind('click',function(e){
						$(config.table).find('tbody tr').removeClass('grid_tr_selected');
						$(this).addClass('grid_tr_selected');
						config.selectedRow = config.rows[this.sectionRowIndex];
					});
					$(config.table).find('td:last-child').addClass('grid_td_right');
					$(config.table).find('th:first-child').addClass('grid_td_left');
					$(config.table).find('td:first-child').addClass('grid_td_left');
					$(config.table).find('tr:last>td').addClass('grid_td_bottom');

					// fix opera bug
					$(config.table).parent().find('table').css({width:$(config.table).width()+'px'});
					// fix ie bug
					if($.browser.msie){
						$(config.table).css({'border-collapse' : 'collapse'})
					}
				}
				function createInsertLayer() {
					if(!config.insertTemplate) return;
					if(config.insertLayer) {
						$(config.insertLayer).remove();
					}
					// create new layer for insert template
					var div = document.createElement('div');
					$(div).attr('id',gridView.generateId('insert_panel'));
					$(div).attr('align','center');
					$(div).css({
						'background':'url('+config.imagesUrl+'transparent.gif)',
						position:'absolute',
						display:'none',
						top: $(config.table).parent()[0].offsetTop+'px',
						left: $(config.table).parent()[0].offsetLeft+'px',
						height: $(config.table).parent().height()+'px',
						width: $(config.table).width()+'px'
					});
					$(config.table).parent().append(div);
					config.insertLayer = div;
					//$(div).addClass('gridInsertPanel');
					div = document.createElement('div');
					$(div).addClass('gridInsertPanel');
					$(config.insertLayer).append(div);
					// create main panel
					div = document.createElement('div');
					$(config.insertLayer).find('div:first').append(div);
					$(div).addClass('gridInsertPanel_body');
					$(div).append(config.insertTemplate);
					div = document.createElement('div');
					$(config.insertLayer).find('div:first').append(div);
					$(div).attr('align','center');
					$(div).addClass('gridEditPanel_footer');
					// add buttons
					var button = document.createElement('input');
					$(button).attr('type','button');
					$(button).val(config.translations['ok']||'Ok');
					$(button).click(function(e){
						if(typeof(config.itemInsert)=='function') {
							// get edited values
							config.itemInsert(e,$(config.insertLayer).find('.gridInsertPanel_body'));
						}
					});
					$(div).append(button);
					button = document.createElement('input');
					$(button).attr('type','button');
					$(button).val(config.translations['cancel']||'Cancel');
					$(button).click(function(e){
						$(config.insertLayer).hide('slow');
					});
					$(div).append(button);
					$(config.insertLayer).find('.gridInsertPanel').css({
						'margin-top':Math.round(($(config.insertLayer).height()/2))-50+ 'px'
					});
				}
				function createMessagesLayer() {
					if(config.messagesLayer) {
						$(config.messagesLayer).remove();
					}
					// create new layer for insert template
					var div = document.createElement('div');
					$(div).attr('id',gridView.generateId('message_panel'));
					$(div).attr('align','center');
					$(div).css({
						'background':'url('+config.imagesUrl+'transparent.gif)',
						position:'absolute',
						'z-index': 10000,
						display:'none',
						top: $(config.table).parent()[0].offsetTop+'px',
						left: $(config.table).parent()[0].offsetLeft+'px',
						height: $(config.table).parent().height()+'px',
						width: $(config.table).width()+'px'
					});
					$(config.table).parent().append(div);
					config.messagesLayer = div;
					//$(div).addClass('gridInsertPanel');
					div = document.createElement('div');
					$(div).addClass('gridInsertPanel');
					$(config.messagesLayer).append(div);
					// create main panel
					div = document.createElement('div');
					$(config.messagesLayer).find('div:first').append(div);
					$(div).addClass('gridInsertPanel_body');
					$(div).css({
						'color': 'red',
						'margin-top':'10px',
						'margin-bottom': '10px'
					});
					//$(div).append('<b>'+config.messages['saveChanges']+'</b>');
					div = document.createElement('div');
					$(config.messagesLayer).find('div:first').append(div);
					$(div).attr('align','center');
					$(div).addClass('gridEditPanel_footer');
					// add buttons
					var button = document.createElement('input');
					$(button).attr('id','btnYes');
					$(button).attr('type','button');
					$(button).val(config.translations['yes']||'Yes');
					$(div).append(button);
					button = document.createElement('input');
					$(button).attr('id','btnNo');
					$(button).attr('type','button');
					$(button).val(config.translations['no']||'No');
					$(button).click(function(e){
						$(config.messagesLayer).hide('fast');
						config.modifiedRows = null;
						gridView.build($(config.table).parent()[0],config);
						return false;
					});
					$(div).append(button);
					$(config.messagesLayer).find('.gridInsertPanel').css({
						'margin-top':Math.round(($(config.messagesLayer).height()/2))-20+ 'px'
					});
				}
				function createFilterLayer() {
					if(config.filterLayer) {
						$(config.filterLayer).remove();
					}
					// create filter layer
					var div = document.createElement('div');
					$(div).attr('id',gridView.generateId('div'));
					$(div).addClass('gridFilterPanel');
					// create select elements
					var selectFilters = document.createElement('select');
					// generate id
					$(selectFilters).attr('id',gridView.generateId('sl_fl'));
					// add first empty option
					var option = document.createElement('option');
					$(option).text('...');
					$(selectFilters).append(option);
					$(selectFilters).change(changeFilterOptions);
					// create option elements
					for(var i=0;i<config.filters.length;i++) {
						option = document.createElement('option');
						$(option).attr('value', config.filters[i].value);
						$(option).attr('id',config.filters[i].id);
						$(option).text(config.filters[i].title);
						// add new option to select element
						$(selectFilters).append(option);
					}
					$(div).append(selectFilters);
					// create operator select
					var selectOp = document.createElement('select');
					$(selectOp).attr('id',gridView.generateId('sl_op'));
					$(div).append(selectOp);
					// create textbox element
					var input = document.createElement('input');
					$(input).attr('id',gridView.generateId('tx_flt'));
					$(input).attr('type','text');
					$(input).attr('disabled','disabled');
					$(div).append(input);
					// create button element
					input = document.createElement('input');
					$(input).attr('id',gridView.generateId('btn_flt'));
					$(input).attr('type','button');
					$(input).attr('value','ok');
					$(input).bind('click',filter);
					$(input).width(40);
					$(div).append(input);
					// create parent element
					config.filterLayer = document.createElement('div');
					$(config.filterLayer).css({
						position: 'absolute',
						top: $(config.table).parent()[0].offsetTop+'px',
						left: $(config.table).parent()[0].offsetLeft+'px',
						height: $(config.table).parent().height()+'px',
						width: $(config.table).width()+'px',
						'background':'url('+config.imagesUrl+'transparent.gif)',
						display: 'none'
					});
					//$(config.filterLayer).dblclick(gridView.stopSelect);
					//$(config.filterLayer).click(gridView.stopSelect);
					//$(config.filterLayer).mousedown(gridView.stopSelect);
					$(config.filterLayer).attr('align','center');
					$(config.filterLayer).append(div);
					$(config.table).parent().append(config.filterLayer);
					//$(config.filterLayer).hide();
					// Place filter form
					$(div).css({
						opacity: '1',
						padding: '2px',
						'margin-top':Math.round(($(config.filterLayer).height()/2)-20) + 'px'
					});
				}
				function createBottomPanel() {
					if(config.totalPages<2){
						$(config.table).parent().find('.gridFooter').remove();
						return;
					}
					// create foot table
					var table = document.createElement("table");
					var tr = document.createElement("tr");
					var td = document.createElement("td");
					var div = document.createElement("div");

					//$(div).attr('id','ftContainer');
					$(table).append(tr);
					$(tr).append(td);
					$(td).append(div);

					$(table).addClass('gridFooter');
					$(table).width($(config.table)[0].offsetWidth||$(config.table).width());
					//$(table).css({width:$(config.table).width()+'px'});
					// manage pages numbers
					if(config.currentPage > 3) {
						var a = document.createElement('a');
						$(a).append(config.translations['first']||'First');
						$(a).attr('href','javascript:void(0);');
						$(a).click(function(){
							config.page = 1;
							serializeParams();
							gridView.build($(config.table).parent()[0],config);
						});
						$(div).append(a);
						$(div).append('&nbsp;...&nbsp;');
						a = document.createElement('a');
						$(a).append('&lt;');
						$(a).attr('href','javascript:void(0);');
						$(a).click(function(){
							config.page = config.currentPage - 1;
							serializeParams();
							gridView.build($(config.table).parent()[0],config);
						});
						$(div).append(a);
						$(div).append('&nbsp;');
					}
					if((config.currentPage - 2) >= 1) {
						var a = document.createElement('a');
						$(a).append(config.currentPage - 2);
						$(a).attr('href','javascript:void(0);');
						$(a).click(function(){
							config.page = config.currentPage - 2;
							serializeParams();
							gridView.build($(config.table).parent()[0],config);
						});
						$(div).append(a);
						$(div).append('&nbsp;');
					}
					if((config.currentPage - 1) >= 1) {
						var a = document.createElement('a');
						$(a).append(config.currentPage - 1);
						$(a).attr('href','javascript:void(0);');
						$(a).click(function(){
							config.page = config.currentPage - 1;
							serializeParams();
							gridView.build($(config.table).parent()[0],config);
						});
						$(div).append(a);
						$(div).append('&nbsp;');
					}
					// curent page number
					$(div).append(config.currentPage);
					$(div).append('&nbsp;');
					if((config.currentPage + 1) <= config.totalPages) {
						var a = document.createElement('a');
						$(a).append(config.currentPage + 1);
						$(a).attr('href','javascript:void(0);');
						$(a).click(function(){
							config.page = config.currentPage + 1;
							serializeParams();
							gridView.build($(config.table).parent()[0],config);
						});
						$(div).append(a);
						$(div).append('&nbsp;');
					}
					if((config.currentPage + 2) <= config.totalPages) {
						var a = document.createElement('a');
						$(a).append(config.currentPage + 2);
						$(a).attr('href','javascript:void(0);');
						$(a).click(function(){
							config.page = config.currentPage + 2;
							serializeParams();
							gridView.build($(config.table).parent()[0],config);
						});
						$(div).append(a);
						$(div).append('&nbsp;');
					}
					if((config.currentPage + 3) <= config.totalPages) {
						var a = document.createElement('a');
						$(a).append('&gt;');
						$(a).attr('href','javascript:void(0);');
						$(a).click(function(){
							config.page = config.currentPage + 1;
							serializeParams();
							gridView.build($(config.table).parent()[0],config);
						});
						$(div).append(a);
						$(div).append('&nbsp;...&nbsp;');
						a = document.createElement('a');
						$(a).append((config.translations['last']||'Last'));
						$(a).attr('href','javascript:void(0);');
						$(a).click(function(){
							config.page = config.totalPages;
							serializeParams();
							gridView.build($(config.table).parent()[0],config);
						});
						$(div).append(a);
					}

					var rgDiv = document.createElement('div');
					$(rgDiv).css({'float':'right'});
					$(div).before(rgDiv);
					$(rgDiv).append(config.translations['page']||'Page');
					$(rgDiv).append('&nbsp;'+config.currentPage+'&nbsp;');
					$(rgDiv).append(config.translations['of']||'of');
					$(rgDiv).append('&nbsp;'+config.totalPages+'&nbsp;('+config.totalItems+'&nbsp;');
					$(rgDiv).append(config.translations['items']||'items');
					$(rgDiv).append(')');
					$(config.table).parent().find('.gridFooter').remove();
					$(config.table).after(table);
				}
				function createTopPanel() {
					if(config.panelEntries.length<1)return;
					// create dom element
					var table = document.createElement("table");
					var tr = document.createElement('tr');
					var td = document.createElement('td');
					var content = document.createElement('div');

					for(var i=0;i<config.panelEntries.length;i++) {
						// TODO assign funcion by entry type
						var span = document.createElement('span');
						$(span).append(config.panelEntries[i].value);
						$(content).append(span);
						$(span).css({cursor:'pointer'});
						bindEventByType(span,config.panelEntries[i]);
					}
					$(td).append(content);
					$(tr).append(td);
					$(table).append(tr);

					$(table).width($(config.table)[0].offsetWidth||$(config.table).width());
					$(table).addClass('gridHeader');
					$(table).attr('id',gridView.generateId('tab'));
					$(config.table).parent().find('.gridHeader').remove();
					$(config.table).before(table);
					//return false;
				}
				function bindEventByType(element,entity) {
					switch(entity.type) {
						case 'filter':
							$(element).bind('click',showFilterLayer);
							break;
						case 'reload':
							$(element).bind('click',reload);
							break;
						case 'insert':
							$(element).bind('click',showInsertLayer);
							break;
						case 'delete':
							if(typeof(config.itemDelete)=='function') {
								$(element).bind('click',function(e) {
									if(!config.selectedRow) return;
									var item = {
										parentRow: config.selectedRow
									};
									$(config.messagesLayer)
										.find('.gridInsertPanel_body').empty()
										.append('<b>'+config.messages['deleteLine']+'</b>');
									$(config.messagesLayer)
										.find('#btnYes').unbind('click')
										.bind('click',function(e){
											$(config.messagesLayer).hide('fast');
											config.itemDelete(e,item);
											return false;
										});
									$(config.messagesLayer)
										.find('#btnNo').unbind('click')
										.bind('click',function(e){
											$(config.messagesLayer).hide('fast');
											return false;
										});
									$(config.messagesLayer).show('slow');
								});
							}
							break;
						case 'save' :
							$(element).bind('click',function(e){
								gridView.saveModifications(e,config);
							});
							break;
						case 'custom':
							break;
					}
				}
				function sort(e) {
					var index = $(e.target).parent().parent().children().index($(e.target).parent()[0]);
					config.orderBy = config.headers[index].name||'';
					// calculate sort order
					config.order = (config.headers[index].order.toLowerCase() == 'asc')?'DESC':'ASC';
					$(config.table).parent()
						.find("input[@id^='orderby']")
						.val(config.headers[index].name);
					// prepare request parameters
					serializeParams();
					// rebuild grid view
					gridView.build($(config.table).parent()[0],config);
					return false;
				}
				function filter(e) {
					if($(e.target).parent().find('input[@id^=tx_flt]').val().length > 0) {
						var index = $(e.target).parent()
													.find('select[@id^=sl_fl]')
													.find('option:selected')[0].index-1;
						// assign compose filter value
  					config.currentFilter = {
  					  name: $(e.target).parent().find('select[@id^=sl_fl]').attr('value'),
  					  operator: $(e.target).parent().find('select[@id^=sl_op]').attr('value'),
  					  value: $(e.target).parent().find('input[@id^=tx_flt]').val(),
  					  isQuoted: config.filters[index].isQuoted
  					};
  						
						config.page = 1;
						// prepare request parameters
						serializeParams();
						// rebuild grid view
						gridView.build($(config.table).parent()[0],config);
					}else {
						$(config.filterLayer).hide('slow');
					}
					return false;
				}
				function moveSizeBar(e) {
					config.resizing = true;
					// TO DO reduce splitbar's movements
					//$.log(e.clientX);
					if(e.pageX < config.ltColLimit || e.pageX > config.rgColLimit){
						return false;
					}
					else if(e.pageX > config.tableLeft && e.pageX < config.tableRight) {
						$(config.splitBar).css({left:e.clientX + 'px'});
						//config.splitBar.style.left = (gridView.getAbsoluteLeft(e.clientX) - config.target.offsetX) +'px';
						return false;
					}
					else {
						stopResize(e);
						return false;
					}
					return false
				}
				function startResize(e) {
					if(config.editMode) {
						if(typeof(config.cancelEditMode)=='function') {
							config.cancelEditMode();
						}else {
							return false;
						}
					}
					config.target = {
						me: e.target,
						parent: $(e.target).parent(),
						offsetX: gridView.getAbsoluteLeft(e.target) + e.target.offsetWidth
					};
					// define resizing limits
					config.ltColLimit = $(config.target.parent)[0].offsetLeft+config.tableLeft+config.minWidth;
					config.rgColLimit = $(config.target.parent).next()[0].offsetLeft+config.tableLeft+( parseInt($(config.target.parent).next().css('width'),10)-config.minWidth);
					// customize document style
					$('body').css({cursor:'e-resize'});
					// customize split bar style
					$(config.splitBar).css({
						'left' : $(e.target).parent().next()[0].offsetLeft + config.tableLeft,
						'height' : parseInt($(config.table).height(),10),
						'top' : parseInt($(config.table)[0].offsetTop,10)
					});
					$(config.splitBar).show();
					$(config.headLayer).show();
					$(document)
						.bind('mousemove',moveSizeBar)
						.bind('mouseup',stopResize)
						.bind('selectstart',gridView.stopSelect)
						.bind('mousedown',gridView.stopSelect);
					return false;
				}
				function stopResize(e) {
					if(!config.resizing) return false;
					resizeTableCells(e);
					$(config.table).parent().find('div[@id^="dirty"]').each(function(i){
						$(this).css({
							left : gridView.getAbsoluteLeft($('#'+config.dirties.div[this.id])[0]) + 'px',
							top : gridView.getAbsoluteTop($('#'+config.dirties.div[this.id])[0]) + 'px'
						});
					});
					$('body').css({cursor:'default'});
					$(config.splitBar).css({left:'0px',display: 'none'});
					//$(config.splitBar).hide();
					$(config.headLayer).hide();
					config.resizing = false;

					$(document)
						.unbind('mousemove',moveSizeBar)
						.unbind('mouseup',stopResize)
						.unbind('selectstart',gridView.stopSelect)
						.unbind('mousedown',gridView.stopSelect);

					return false;
					//console.dir($('body')[0].$events);
				}
				function serializeParams() {
					config.params = "";
					config.params += "order="+gridView.encodeUrl(config.order||'ASC');
					config.params += "&orderby="+gridView.encodeUrl(config.orderBy);
					config.params += "&page="+gridView.encodeUrl(config.page||config.currentPage);
					
					if(config.currentFilter.name)
					  config.params += "&filtername="+gridView.encodeUrl(config.currentFilter.name);
					if(config.currentFilter.operator)
					  config.params += "&filterop="+gridView.encodeUrl(config.currentFilter.operator);
					if(config.currentFilter.value)
					  config.params += "&filterval="+gridView.encodeUrl(config.currentFilter.value);
					if(config.currentFilter.isQuoted)
					  config.params += "&filterisq="+gridView.encodeUrl(config.currentFilter.isQuoted);
				}
				function resizeTableCells(e) {
					var resizable = {
						diffWidth : 0,
						parentNextLeft  : 0,
						parentWidth  : 0,
						splitBarLeft : 0,
						leftWidth : 0,
						rightWidth : 0,
						currentIndex : 0
					};
					resizable.parentNextLeft = $(config.target.parent).next()[0].offsetLeft;
					resizable.splitBarLeft = $(config.splitBar)[0].offsetLeft;
					resizable.diffWidth = ((resizable.parentNextLeft + config.tableLeft)-resizable.splitBarLeft);

					resizable.leftWidth = parseInt($(config.target.parent).css('width'),10);
					resizable.rightWidth = parseInt($(config.target.parent).next().css('width'),10);

					resizable.currentIndex = $(config.target.parent).parent().children()
						.index($(config.target.parent)[0]);

					var newLeftWidth = resizable.leftWidth-resizable.diffWidth-$(config.splitBar).width();
					var newRightWidth = resizable.rightWidth+resizable.diffWidth+$(config.splitBar).width();

					if(newLeftWidth < config.minWidth) {
						resizable.diffWidth = resizable.diffWidth - (config.minWidth - newLeftWidth);
					}
					else if(newRightWidth < config.minWidth) {
						resizable.diffWidth = resizable.diffWidth + (config.minWidth - newRightWidth);
					}
					$(config.table).find('tr').each(function(i){
						$($(this).children()[resizable.currentIndex])
							.css({
								'width':resizable.leftWidth-resizable.diffWidth-$(config.splitBar).width()+'px'
							});
						$($(this).children()[resizable.currentIndex+1])
							.css({
								'width':resizable.rightWidth+resizable.diffWidth+$(config.splitBar).width()+'px'
							});
					});
				}
				function changeFilterOptions(e) {
					var selectOp = $(e.target).next();
					var id = e.target.options[e.target.selectedIndex].id;
					$(selectOp).empty();
					if(!id) {
					// disable button
					$(e.target).siblings("input[@id^=btn_flt]");
					$(e.target).siblings("input[@type='text']")
						.attr('disabled','disabled')
						.val('');
					} else {
						// enable button
						$(e.target)
							.siblings("input[@type='text']")
							.attr('disabled','');
						var operators = getOperatorsByFilterId(id);

						for(var i=0;i<operators.length;i++) {
							option = document.createElement('option');
							$(option).attr('value', operators[i].value);
							$(option).text(operators[i].title);
							$(selectOp).append(option);
						}
					}
				}
				function getOperatorsByFilterId(filterId) {
					var result = null;
					for(var i=0;i<config.filters.length;i++){
						if(config.filters[i].id == filterId){
							result = config.filters[i].operators;
							break;
						}
					}
					return result;
				}
				function showFilterLayer(e) {
					$(config.filterLayer).show('slow');
				}
				function showInsertLayer(e) {
					$(config.insertLayer).show('slow');
				}
				function reload(e,local) {
					$("input[@id^='orderby']").val('');
					if(local == true){
						config.editMode = false;
						serializeParams();
					}
					gridView.build($(config.table).parent()[0],config);
					return false;
				}
			},
			generateId : function(prefix) {
				prefix = prefix || 'jquery-gen';
				var newId = prefix+'_'+(Math.round(100000*Math.random()));
				if($('#'+newId)[0] !== undefined) {
					return gridView.generateId(prefix);
				}
				else {
					return newId;
				}
			},
			stopSelect : function (e) {
				return;
			},
			encodeUrl : function(str) {
				str = escape(str);
				str = str.replace(/\+/g,'%2B');
				str = str.replace(/\"/g,'%22');
				str = str.replace(/\'/g,'%27');
				str = str.replace(/\>/g,'%3E');
				str = str.replace(/\</g,'%3C');
				str = str.replace(/\=/g,'%3D');
				str = str.replace(/\!/g,'%21');
				str = str.replace(/:>/g,'%3A');
				return str;
			},
			getAbsoluteTop : function(object) {
				// Get top position from the parent object
				var top = $(object)[0].offsetTop;
				// Parse the parent hierarchy up to the document element
				while($(object)[0].offsetParent) {
					// Add parent top position
					object = $(object)[0].offsetParent;
					top += $(object)[0].offsetTop;
				}
				// Return top position
				return top
			},
			getAbsoluteLeft : function(object) {
				// Get left position from the parent object
				var left = $(object)[0].offsetLeft;
				// Parsfe the parent hierarchy up to the document element
				while($(object)[0].offsetParent) {
					// Add parent left position
					object = $(object)[0].offsetParent;
					left += $(object)[0].offsetLeft;
				}
				// Return left postion
				return left
			},
			showDirtyIcon : function(config,td) {
				if(!config.dirties.td[td.id]) {
					var div = document.createElement('div');
					$(div).css({
						'z-index': 10,
						position : 'absolute',
						top: gridView.getAbsoluteTop(td) +'px',
						left: gridView.getAbsoluteLeft(td) +'px'
					});
					var image = document.createElement('img');
					$(image).attr('src',config.imagesUrl+'dirty.gif');
					$(div).attr('id',gridView.generateId('dirty'));
					$(td).attr('id',gridView.generateId('td'));
					$(div).append(image);
					$(config.table).parent().append(div);
					config.dirties.div[div.id] = td.id;
					config.dirties.td[td.id] = div.id;
				}
			},
			saveModifications : function(e,config){
				if(config.modifiedRows && typeof(config.save)=='function'){
					var rows = [];
					$.each(config.modifiedRows,function(i,n){
						if(n!=undefined){
							rows.push(config.rows[n]);
						}
					});
					config.modifiedRows = null;
					config.save(e,rows);
				}
				return false;
			}
		}
	}();
  return this.each(function(i){
		var config = $.extend(cfg,{
			minWidth : 50,
			tableLeft : 0,
			tableRight : 0,
			resizeLimit : 0,
			resizing : false,
			target: null
		});
    gridView.build($(this)[0],config);
  });
};