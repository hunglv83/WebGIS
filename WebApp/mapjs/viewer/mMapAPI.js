define(["viewer/mCommon",
    "viewer/mCoreApi",
    "libs/w2ui/w2ui"
], function (mCom, mCore, w2) {
    var mMapAPI = {};
    mMapAPI.Init = function (divmap, opts) {
        //Load layout cho map
        $("#" + divmap).addClass("map-viewer");
        $("#" + divmap).append(mCom.Global.mDivMain);
        //Init layout cho main
        init_Layout();
        mCore.MapInit();
        mCore.MapConfig.mMap_config_load_file_local();
        //Test các chức năng
        init_test();
		
		mCore.MapMeasurement();

		$(".btn-close").on('click', function () {
			$(".widget-right").hide();
		})

		$(document).on('click', '.panel-heading span.clickable', function (e) {
			var $this = $(this);
			if (!$this.hasClass('panel-collapsed')) {
				$this.parents('.panel').find('.panel-body').slideUp();
				$this.addClass('panel-collapsed');
				$this.find('i').removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
				//$(".widget-right").css("width","100px");
			} else {
				$this.parents('.panel').find('.panel-body').slideDown();
				$this.removeClass('panel-collapsed');
				$this.find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
				//$(".widget-right").css("width", "auto");
			}
		})
    };
    var init_test = function () {
        //console.log(mapurl);
        //console.log(mapname);
        //Test load layer
        $("#map_loading").show();
        //const urlParams = new URLSearchParams(window.location.search);
        const serviceid = 1;
        //var url = mapurl;
        var url = "http://113.160.244.118:6080/arcgis/rest/services/BDHT_QH_SDD_2020/BDKH_SDD_QN_2020_05_13/MapServer";
        mCom.Global.mBanDoId = "bd_2";
        mCom.Global.mBanDoTitle = "Bản đồ hiện trạng SDD 2020";
        mCore.Layers.AddLayer(url, { type: "TiledMapServiceLayer", id: "bd_2", title: "Bản đồ hiện trạng SDD 2020" });
		$("#map_loading").hide();
    };
    var init_Layout = function () {
        //Khởi tạo div layout map
        var divlayout = document.getElementById("main_content");
        if (w2ui.hasOwnProperty(mCom.Global.layout_main_name)) {
            w2ui[mCom.Global.layout_main_name].destroy();
        }
        $(divlayout).w2layout(mCom.Layouts.layout);
        //Hide top, bottom content
        w2ui[mCom.Global.layout_main_name].hide('top', window.instant);
        w2ui[mCom.Global.layout_main_name].hide('right', window.instant);
        //Add divmap to main content
        w2ui[mCom.Global.layout_main_name].content("main", mCom.Global.mDivMainContent);
        //Thêm các div content cho các layout
        w2ui[mCom.Global.layout_main_name].content("left", mCom.Global.mDivLeftContent);
        w2ui[mCom.Global.layout_main_name].content("right", mCom.Global.mDivRinghtContent);
        //Toolbar main click
        mTools_main_click();
        mTools_left_click();
        mTools_right_click();
        //Xử lý sự kiện resize layout => resize map content
        w2ui[mCom.Global.layout_main_name].on("resize", function (event) {
            if (mCom.Global.mMap) {
                if (mCom.Global.mMap.loaded) {
                    try {
                        mCom.Global.mMap.autoResize = true;
                        mCom.Global.mMap.reposition();
                        mCom.Global.mMap.resize();
                        mCom.Global.mMap.autoResize = false;
                    } catch (e) {
                        console.log(e);
                    }
                }
            }
        })
        //Set selected to Layers of left toolbar
        w2ui[mCom.Global.tool_left_name].hide("item-left-legend");
        w2ui[mCom.Global.tool_left_name].refresh();
        //set layout cho cong cu thiet lap layers
        $().w2layout(mCom.Layouts.layout_layers);
        $().w2sidebar(mCom.Layouts.sidebar_layers);
        $().w2grid(mCom.Layouts.grid_layers);

    };

    function mTools_main_checked_clear() {
        w2ui[mCom.Global.tool_main_name].items.forEach(item => item.checked = false);
        w2ui[mCom.Global.tool_main_name].refresh();
    }

    function mTools_left_checked_clear() {
        w2ui[mCom.Global.tool_left_name].items.forEach(item => item.checked = false);
        w2ui[mCom.Global.tool_left_name].refresh();
    }

    function mTools_main_left_toggle(isinstant) {
        if (isinstant) {
            w2ui[mCom.Global.layout_main_name].show('left', window.instant);
            w2ui[mCom.Global.tool_main_name].show('item-left-hide');
            w2ui[mCom.Global.tool_main_name].hide('item-left-show');
        } else {
            w2.w2ui[mCom.Global.layout_main_name].hide('left', window.instant);
            w2ui[mCom.Global.tool_main_name].show('item-left-show');
            w2ui[mCom.Global.tool_main_name].hide('item-left-hide');
        }
    }

    function mTools_main_right_toggle(isinstant) {
        if (isinstant) {
            w2ui[mCom.Global.layout_main_name].show('right', window.instant);
        } else {
            w2.w2ui[mCom.Global.layout_main_name].hide('right', window.instant);
        }
    }

    function mTools_main_click() {
        w2ui[mCom.Global.tool_main_name].on('click', function (event) {
            mCom.Global.mIdentity = false;
            switch (event.target) {
                case 'item-left-hide':
                    mTools_main_left_toggle(false);
                    break;
                case 'item-left-show':
                    mTools_main_left_toggle(true);
                    break;
                case 'item-map-arrow':
                    if (mCom.Global.mNavToolbar) {
                        mCom.Global.mNavToolbar.deactivate();
                    }
                    //clear các button đã chọn
                    mTools_main_checked_clear();
                    mCom.Global.mMap.graphics.clear();
                    break;
                case "item-map-zoomin":
                    if (mCom.Global.mNavToolbar) {
                        mCom.Global.mNavToolbar.activate(esri.toolbars.Navigation.ZOOM_IN);
                    }
                    break;
                case "item-map-zoomout":
                    if (mCom.Global.mNavToolbar) {
                        mCom.Global.mNavToolbar.activate(esri.toolbars.Navigation.ZOOM_OUT);
                    }
                    break;
                case "item-map-pan":
                    if (mCom.Global.mNavToolbar) {
                        mCom.Global.mMap.setMapCursor("hand");
                        mCom.Global.mNavToolbar.activate(esri.toolbars.Navigation.PAN);
                    }
                    break;
                case "item-map-fitview":
                    mCom.Global.mNavToolbar.zoomToFullExtent();
                    break;
                case "item-map-prev":
                    if (mCom.Global.mNavToolbar) {
                        mCom.Global.mNavToolbar.zoomToPrevExtent();
                    }
                    break;
                case "item-map-next":
                    if (mCom.Global.mNavToolbar) {
                        mCom.Global.mNavToolbar.zoomToNextExtent();
                    }
                    break;
                case "item-map-info":
                    mCom.Global.mIdentity = true;
                    break;
				case "item-map-measurement":
                        $(".widget-right").show();
                        break;
                default:
            }
        });
    }

    function mTools_left_click() {
        w2ui[mCom.Global.tool_left_name].on('click', function (event) {
            switch (event.target) {
                case 'item-left-layers':
                    mTools_left_layers();
                    break;
                case 'item-left-legend':
                    break;
                case 'item-left-search':
                    mTools_left_Search();
                    break;
                case 'item-left-tools':
                    //clear các button đã chọn
                    mTools_left_checked_clear();
                    mCore.MapConfig.mMap_config_tools();
                    break;
                default:
            }
        });
    }

    function mTools_right_click(event) {
        w2ui[mCom.Global.tool_right_name].on('click', function (event) {
            switch (event.target) {
                case 'item-right-exit':
                    mTools_main_right_toggle(false);
                    break;
                default:
            }
        });
    }
    //function mTools_layers_config() {
    //    w2popup.open({
    //        title: 'Cấu hình lớp dữ liệu bản đồ',
    //        width: 800,
    //        height: 600,
    //        showMax: true,
    //        modal: true,
    //        showClose: true,
    //        body: '<div id="main" style="position: absolute; left: 0px; top: 0px; right: 0px; bottom: 0px;"></div>',
    //        onOpen: function (event) {
    //            event.onComplete = function () {
    //                $('#w2ui-popup #main').w2render(mCom.Layouts.layout_layers.name);
    //                w2ui.layout_layers.content('left', w2ui.sidebar_layers);
    //                w2ui.layout_layers.content('main', w2ui.grid_layers);
    //                w2ui[mCom.Layouts.grid_layers.name].toolbar.hide('w2ui-reload', 'w2ui-search', 'w2ui-column-on-off', 'w2ui-break0');
    //                //load dữ liệu từ bản đồ
    //                mCore.MapConfig.mMap_config_layers_getall();
    //            }
    //        },
    //        onToggle: function (event) {
    //            event.onComplete = function () {
    //                w2ui[mCom.Layouts.layout_layers.name].resize();
    //            }
    //        }
    //    });
    //    //xử lý sự kiện button
    //    w2ui[mCom.Layouts.grid_layers.name].toolbar.on('click', function (event) {
    //        switch (event.target) {
    //            case 'btnAddLayer':
    //                mCore.MapConfig.mMap_config_map_add();
    //                break;
    //            case 'btnSaveConfig':
    //                mCore.MapConfig.mMap_config_save_config();
    //                break;
    //            case 'btnSaveFile':
    //                mCore.MapConfig.mMap_config_save_file_local();
    //                break;
    //            default:
    //        }
    //    });
    //};

    function mTools_layers_load(url) {
        url = "http://localhost:6080/arcgis/rest/services/GIS_VN/Ban_do_hanh_chinh_VN_20200406/MapServer";
        var nodService = {};
        nodService.id = "gp_1";
        nodService.text = "Bản đồ quy hoạch";
        nodService.name = url;
        nodService.group = true;
        nodService.expanded = true;
        nodService.nodes = [];
        $.getJSON(url + "?f=pjson", function (data) {
            $(data.layers).each(function (idx, layer) {
                var subnode = {};
                subnode.id = nodService.id + "/" + layer.id;
                subnode.name = layer.id;
                subnode.text = layer.name;
                subnode.img = 'icon-page';
                nodService.nodes.push(subnode);
            })
            w2ui[mCom.Layouts.sidebar_layers.name].add(nodService);
            w2ui[mCom.Layouts.sidebar_layers.name].refresh();
        });
        //sự kiện click slidebar
        w2ui[mCom.Layouts.sidebar_layers.name].on("click", mTools_layer_load_field);
    }

    function mTools_layer_load_field(event) {
        //lấy danh sách các field
        var urlfield = event.node.parent.name + "/" + event.node.name;
        $.getJSON(urlfield + "?f=pjson", function (data) {
            //thêm vào grid
            var arrfield = [];
            $(data.fields).each(function (idx, field) {
                var obj = {};
                obj.recid = idx;
                obj.fsearch = false;
                obj.fshow = true;
                obj.fname = field.alias;
                obj.falias = (field.alias) ? field.alias : field.name;
                obj.fType = field.type;
                arrfield.push(obj);
            }).promise().done(function () {
                w2ui[mCom.Layouts.grid_layers.name].records = arrfield;
                w2ui[mCom.Layouts.grid_layers.name].refresh();
            });
        });
    }

    function mTools_left_layers() {
        $("#layers_search").hide();
        $("#toc_layer").show();
    }

    function mTools_left_Search() {
        $("#toc_layer").hide();
        $("#layers_search").show();
        $("#cbolayer").empty();
        var o = new Option("--Chọn--", "chon");
        $(o).html("--Chọn--");
        $("#cbolayer").append(o);
        let map_cofg = {
            id: mCom.Global.mBanDoId,
            name: mCom.Global.mBanDoId,
            title: mCom.Global.mBanDoTitle,
        };
        var arrlayer = mCore.Layers.mapGetAllLayer();
        //Thêm dữ liệu vào combo layer để tìm kiếm
        if (arrlayer) {
            for (var i = 0; i < arrlayer.length; i++) {
                var item = arrlayer[i];
                if (item.layers) {
                    for (var j = 0; j < item.layers.length; j++) {
                        var flayer = item.layers[j];
                        let mservice = {
                            id: map_cofg.id + "/lyr_" + flayer.id,
                            name: map_cofg.id + "/lyr_" + flayer.id,
                            img: 'icon-folder',
                            title: flayer.title,
                            url: flayer.url,
                        };
                        //Kiểm tra trong mapconfig xem đã cấu hình layer chưa
                        let layer_cofg = mCore.MapConfig.mMap_config_layer_search_byid(mservice.id);
                        if (layer_cofg) {
                            if (layer_cofg.fsearch) {
                                var o = new Option(layer_cofg.title, layer_cofg.id + "_@@_" + layer_cofg.url);
                                $(o).html(layer_cofg.title);
                                $("#cbolayer").append(o);
                            }
                        } else {
                            var o = new Option(flayer.title, mservice.id + "_@@_" + flayer.url);
                            $(o).html(flayer.title);
                            $("#cbolayer").append(o);
                        }
                    }
                }
            }
            var input = document.getElementById("txtsearch");
            input.addEventListener("keyup", function (event) {
                if (event.keyCode === 13) {
                    event.preventDefault();
                    document.getElementById("btnSearch").click();
                }
            });
            $("#btnSearch").on("click", mTools_search_map_layer);
        }
    }

    function mTools_search_map_layer() {
        try {
            if (!$("#txtsearch").val()) {
                w2alert('Bạn chưa nhập thông tin tìm kiếm!', 'Thông báo');
                $("#txtsearch").focus();
                return;
            }
            mCom.Global.mMap.graphics.clear();
            let cbolayer = $('select#cbolayer option:selected').val();
            if (cbolayer === "chon") {
                w2alert('Bạn chưa chọn lớp để tìm kiếm. Xin vui lòng chọn lớp tìm kiếm!', 'Thông báo');
                $('#cbolayer').focus().select()
                return;
            }
            //Split by _@@_
            const srvValue = cbolayer.split('_@@_');
            let url = srvValue[1];
            let srvId = srvValue[0];
            let layername = $('select#cbolayer option:selected').text();
            let layer_cofg = mCore.MapConfig.mMap_config_layer_search_byid(srvId);
            let strquery = null;
            let strsearch = $("#txtsearch").val().trim();
            $("#map_loading").show();
            if (layer_cofg) {
                for (var i = 0; i < layer_cofg.fields.length; i++) {
                    let field = layer_cofg.fields[i];
                    if (field.fsearch) {
                        if (strquery === null) {
                            if (field.ftype === "esriFieldTypeString") {
                                strquery = field.fname + "='" + strsearch + "'";
                            } else {
                                strquery = field.fname + "=" + strsearch;
                            }
                        } else {
                            if (field.ftype === "esriFieldTypeString") {
                                strquery += " OR " + field.fname + "='" + strsearch + "'";
                            } else {
                                strquery = " OR " + field.fname + "=" + strsearch;
                            }
                        }
                    }
                }
            }
            if (strquery) {
                mCore.Searchs.FeatureLayer(url, strquery, mTools_search_show_results);
            }
        } catch (e) {
            $("#map_loading").hide();
        }
    }

    function mTools_search_show_results(featureSet) {
        var data = featureSet.features;
        console.log(data);
        if (w2ui.hasOwnProperty(mCom.Layouts.grid_search_result.name)) {
            w2ui[mCom.Layouts.grid_search_result.name].destroy();
        }
        $("#resultsCount").html("Tìm thấy: <b>" + data.length);
        $('#search-show-results').w2grid({
            name: mCom.Layouts.grid_search_result.name,
            fixedBody: false,
            show: {
                toolbar: false,
                footer: true,
                lineNumbers: true,
            },
            reorderColumns: true,
            columns: [{ field: 'recid', caption: 'Rec. Id', size: '50px', resizable: true, hidden: true }],
            records: []
        });
        let layername = $('select#cbolayer option:selected').text();
        let layer_cofg = mCore.MapConfig.mMap_config_layer_search(layername);
        for (var i = 0; i < layer_cofg.fields.length; i++) {
            let field = layer_cofg.fields[i];
            let col = {
                field: field.fname,
                caption: field.falias,
                size: '50px',
                resizable: true,
            };
            w2ui[mCom.Layouts.grid_search_result.name].addColumn(col);
        }
        let arrRecord = [];
        for (var i = 0; i < data.length; i++) {
            var feature = data[i];
            feature.attributes.recid = i + 1;
            feature.attributes.geo = feature;
            arrRecord.push(feature.attributes);
        }
        let hidcol = [];
        w2ui[mCom.Layouts.grid_search_result.name].add(arrRecord);
        for (var i = 0; i < layer_cofg.fields.length; i++) {
            let field = layer_cofg.fields[i];
            if (!field.fshow) {
                //hidcol.push(field.fname);
                w2ui[mCom.Layouts.grid_search_result.name].hideColumn(field.fname);
            }
        }
        $("#map_loading").hide();
        w2ui[mCom.Layouts.grid_search_result.name].hideColumn("geo");
        w2ui[mCom.Layouts.grid_search_result.name].on("select", mTools_search_result_click);

    }

    function mTools_search_result_click(event) {
        let geo = w2ui[mCom.Layouts.grid_search_result.name].get(event.recid).geo;
        if (geo) {
            //Zoom to geo
            mCom.Global.mMap.setExtent(geo.geometry.getExtent(1.2),true);
            //select geo
            mCom.Global.mMap.infoWindow.setFeatures(geo);
            mCore.Searchs.FeatureHighligh(geo.geometry);
        }
    }
    //Gán lại các biến dùng chung
    mMapAPI.Global = mCom.Global;
    return mMapAPI;
})