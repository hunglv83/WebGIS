define(["viewer/mCommon",
    "dojo/dom",
    "dojo/on",
    "dojo/parser",
    "dojo/dom-construct",
    "esri/map",
    "dojo/promise/all",
    "dojo/_base/connect",
    "dojo/_base/array",
    "esri/toolbars/navigation",
    "agsjs/TOC",
    "esri/dijit/Popup",
    "dojo/_base/Color",
    "esri/SnappingManager",
    "esri/dijit/Measurement",
    "dojo/i18n!esri/nls/jsapi",
    "esri/units",
    "esri/symbols/SimpleFillSymbol",
    "esri/symbols/SimpleLineSymbol",
    "esri/SpatialReference",
    "esri/dijit/Basemap",
    "esri/InfoTemplate",
    "esri/tasks/query",
    "esri/toolbars/draw",
    "esri/geometry/Circle",
    "esri/graphic",
    "esri/layers/FeatureLayer",
    "esri/tasks/GeometryService",
    "esri/layers/ArcGISDynamicMapServiceLayer",
    "esri/layers/ArcGISTiledMapServiceLayer",
], function (mCom, dom, on, parser,
    domConstruct, Map, All, connect, arrayUtils, Navigation, TOC, Popup, Color,
    SnappingManager, Measurement, bundle, Units,
    SimpleFillSymbol, SimpleLineSymbol,
    SpatialReference, Basemaps, InfoTemplate,
    Query, Draw,
    Circle,
    Graphic,
    FeatureLayer,
    GeometryService,
    ArcGISDynamicMapServiceLayer,
    ArcGISTiledMapServiceLayer, ) {
    'use strict';
    parser.parse();
    var mCore = {};
    mCore.VERSION = 1.0;
    var popup = new Popup({
        fillSymbol: new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID,
            new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID,
                new Color([255, 0, 0]), 2), new Color([255, 255, 0, 0.25]))
    }, domConstruct.create("div"));
	esriConfig.defaults.io.proxyUrl = "/proxy/";
    esriConfig.defaults.io.alwaysUseProxy = false;
    //This service is for development and testing purposes only. We recommend that you create your own geometry service for use within your applications
    esriConfig.defaults.geometryService = new GeometryService("https://utility.arcgisonline.com/ArcGIS/rest/services/Geometry/GeometryServer");
    mCore.MapInit = function () {
        esriConfig.defaults.map.panDuration = 350; // time in milliseconds, default panDuration: 350
        esriConfig.defaults.map.panRate = 25; // default panRate: 25
        esriConfig.defaults.map.zoomDuration = 500; // default zoomDuration: 500
        esriConfig.defaults.map.zoomRate = 25; // default zoomRate: 25,
        //esriConfig.defaults.map.zoomAnimationThrottled = true;

        var map = new Map("map_view", {
            //basemap: "topo",
            logo: false,
            infoWindow: popup,
            slider: false,
            showLabels: false,
            //maxScale: 1000,
            //minScale: 1000000,
        });
        mCom.Global.mNavToolbar = new Navigation(map);
        mCom.Global.mMap = map;
        var toc = new TOC({
            map: map,
            layerInfos: [],
        }, "toc_layer");
        toc.startup();
        mCom.Global.mToc = toc;
        map.on("load", function () {
            if (map.loaded) {
                map.autoResize = true;
                map.resize();
                map.reposition();
            }
            ////Xử lý popup
            //var popup = map.infoWindow;
            //connect.connect(popup, "onSelectionChange", function () {
            //    displayPopupContent(popup.getSelectedFeature());
            //});
            //connect.connect(map.infoWindow, "onClearFeatures", function () {
            //    console.log('features cleared');
            //});
            //connect.connect(popup, "onSetFeatures", function () {
            //    console.log("features set");
            //});
            //connect.connect(popup, "onShow", function () {
            //    console.log('info window is showing');
            //});
        });
        map.on("mouse-move", map_ShowCoordinates);
        map.on("layer-add-result", function (evt) {
            //var toc = new TOC({
            //    map: map,
            //    layerInfos: mCom.Global.mLayersTOC,
            //}, mCom.Global.mDivToc);
            //toc.startup();
        });
        map.on("layers-add-result", function (lyrs) {
            //var toc = new TOC({
            //    map: map,
            //    layerInfos: mCom.Global.mLayersTOC,
            //}, mCom.Global.mDivToc);
            //toc.startup();
        });
        map.on("update-end", function () {
            $("#map_loading").hide();
        });
        map.on("update-start", function () {
            //$("#map_loading").show();
        });
        map.on("click", function (evt) {
            if (mCom.Global.mIdentity) {
                var circle = new Circle({
                    center: evt.mapPoint,
                    radius: 0.5,
                });
                var query = new Query();
                query.geometry = circle.getExtent();
                map_executeQueryTask(query);
            }
        })
    };
	mCore.MapMeasurement = function () {
        bundle.widgets.measurement.NLS_area_sq_kilometers = "km<sup>2</sup>";
        bundle.widgets.measurement.NLS_sq_kilometers = "km";
        bundle.widgets.measurement.NLS_area = "Đo diện tích";
        bundle.widgets.measurement.NLS_distance = "Đo khoảng cách";
        bundle.widgets.measurement.NLS_location = "Tọa độ điểm";
        bundle.widgets.measurement.NLS_resultLabel = "Kết quả đo";
        bundle.toolbars.draw.addPoint = "Thêm điểm";
        bundle.toolbars.draw.start = "Bắt đầu";
        bundle.toolbars.draw.resume = "Làm lại";
        bundle.toolbars.draw.start = "Điểm đầu";
        bundle.toolbars.draw.complete = "Kết thúc";
        mCom.Global.mMapMeasurement = new Measurement({
            map: mCom.Global.mMap,
            advancedLocationUnits: true,
            defaultAreaUnit: Units.SQUARE_KILOMETERS,
            defaultLengthUnit: Units.KILOMETERS
        }, dom.byId("measurementDiv"));
        mCom.Global.mMapMeasurement.startup();
    };
    function displayPopupContent(feature) {
        if (feature) {
            var content = feature.getContent();
            alert(content);
        }
    }

    function map_executeQueryTask(query) {
        var promises = [];
        for (var i = 0; i < mCom.Global.mMap.layerIds.length; i++) {
            var layername = mCom.Global.mMap.layerIds[i];
            var layerServices = mCom.Global.mMap._layers[layername];
            if (layerServices) {
                for (var j = 0; j < layerServices.layerInfos.length; j++) {
                    var layer = layerServices.layerInfos[j];
                    if (layer.visible && layer.subLayerIds === null) {
                        //Check xem layer nào được cấu hình tìm kiếm
                        var fLayer = new FeatureLayer(layerServices.url + "/" + j, {
                            outFields: ["*"],
                            mode: FeatureLayer.MODE_SELECTION,
                        });
                        promises.push(fLayer.selectFeatures(query, FeatureLayer.SELECTION_NEW));
                        fLayer.setSelectionSymbol(mCom.Global.mMap.infoWindow.fillSymbol);
                    }
                }
            }
        }
        var allPromises = new All(promises);
        allPromises.then(function (result) {
            map_info_result_show(result);
        });
    }

    function map_info_result_grid() {
        var objResult = document.getElementById("ObjResults");
        if (objResult === undefined || objResult === null) {
            return false;
        }
        $('#ObjResults').w2grid({
            name: 'grid_result',
            header: 'Thông tin dối tượng',
            fixedBody: false,
            show: {
                header: true,
                toolbar: true,
                columnHeaders: false,
                toolbarReload: false,
                toolbarColumns: false,
                toolbarSearch: false,
                toolbarAdd: false,
                toolbarDelete: false,
                toolbarSave: false
            },
            toolbar: {
                items: [{
                    type: 'button',
                    id: 'btnPrev',
                    caption: '',
                    img: 'icon-backward'
                },
                {
                    type: 'html',
                    id: 'item5',
                    html: function (item) {
                        var html =
                            '<div id="fcount">' +
                            ' 4' +
                            '</div>';
                        return html;
                    }
                },
                {
                    type: 'button',
                    id: 'btnNext',
                    caption: '',
                    img: 'icon-forward2'
                },
                ]
            },
            columns: [{
                field: 'name',
                caption: 'Name',
                size: '100px',
                resizable: true,
                style: 'background-color: #efefef; border-bottom: 1px solid white; padding-right: 5px;',
                attr: "align=left"
            },
            {
                field: 'value',
                caption: 'Value',
                size: '100%'
            }
            ]
        });
        w2ui['grid_result'].toolbar.hide('w2ui-reload', 'w2ui-search', 'w2ui-column-on-off', 'w2ui-break0');
        return true;
    }

    function map_info_result_show(results) {
        var count = 0;
        for (var i = 0; i < results.length; i++) {
            count = count + results[i].length;
        }
        var items = arrayUtils.map(results, function (result) {
            //highlight item
            if ((result) && (result.length > 0)) {
                mCom.Global.mMap.infoWindow.setFeatures(result);
            }
            return result;
        });
        //Duyệt danh sách các đối tượng
        let objfeatures = [];
        for (var i = 0; i < items.length; i++) {
            let lyrs = items[i];
            for (var j = 0; j < lyrs.length; j++) {
                let layername = lyrs[j]._layer._name;
                let feature = lyrs[j].attributes;
                feature.Layer = layername;
                feature.geo = lyrs;
                objfeatures.push(feature);
            }
        }
        if (w2ui.hasOwnProperty('grid_result')) {
            w2ui['grid_result'].destroy();
        }
        let objgrid = map_info_result_grid();
        //Khởi tạo grid thành công????
        if (objgrid) {
            //Khởi tạo record cho grid
            let records = [];
            objfeatures.forEach(function (features) {
                let arrFeature = [];
                let arrfields = Object.getOwnPropertyNames(features)
                let idx = 0;
                let layer = mMapConfig.mMap_config_layer_search(features.Layer);
                if (layer) {
                    layer.fields.forEach(function (f) {
                        if (f.finfo) {
                            idx++;
                            let obj = {};
                            obj.recid = idx;
                            //Gọi lấy tên tiếng việt ở đây
                            obj.name = f.falias;
                            if (f.fname.toLowerCase().includes("dientich")) {
                                obj.value = w2utils.formatNumber(features[f.fname] / 10000) + " (ha)"; //đổi ra Ha
                            } else {
                                obj.value = features[f.fname];
                            }
                            obj.geo = features.geo;
                            arrFeature.push(obj);
                        }
                    })
                } else {
                    arrfields.forEach(function (f) {
                        idx++;
                        let obj = {};
                        obj.recid = idx;
                        //Gọi lấy tên tiếng việt ở đây
                        obj.name = f;
                        obj.value = features[f];
                        obj.geo = features.geo;
                        arrFeature.push(obj);
                    })
                }
                records.push(arrFeature);
            })
            //Trường hợp không có đối tượng nào
            w2ui['grid_result'].clear();
            if (count == 0) {
                $("#fcount").html("0");
                w2ui['grid_result'].toolbar.disable('btnPrev', 'btnNext');
            }
            //Trường hợp chỉ có một đối tượng
            else {
                w2ui['grid_result'].add(records[0]);
                if (count == 1) {
                    $("#fcount").html("1/" + count);
                    w2ui['grid_result'].toolbar.disable('btnPrev', 'btnNext');
                }
                //Trường hợp có hơn một đối tượng
                else {
                    $("#fcount").html("1/" + count);
                    w2ui['grid_result'].toolbar.disable('btnPrev');
                }
            }
            let incount = 1;
            w2ui['grid_result'].toolbar.on("click", function (event) {
                let geo = null;
                switch (event.target) {
                    case "btnPrev":
                        incount--;
                        if (incount === 1) {
                            w2ui['grid_result'].toolbar.disable(event.target);
                            w2ui['grid_result'].toolbar.enable("btnNext");
                        }
                        $("#fcount").html(incount + "/" + count);
                        w2ui['grid_result'].records = [];
                        w2ui['grid_result'].add(records[incount - 1]);
                        geo = w2ui['grid_result'].get(1)["geo"];
                        if (geo) {
                            //select geo
                            mCom.Global.mMap.infoWindow.setFeatures(geo);
                            //Zoom to geo
                            mCom.Global.mMap.setExtent(geo[0].geometry.getExtent(), true);
                        }
                        break;
                    case "btnNext":
                        incount++;
                        if (incount === count) {
                            w2ui['grid_result'].toolbar.disable(event.target);
                            w2ui['grid_result'].toolbar.enable("btnPrev");
                        }
                        $("#fcount").html(incount + "/" + count);
                        w2ui['grid_result'].records = [];
                        w2ui['grid_result'].add(records[incount - 1]);
                        geo = w2ui['grid_result'].get(1)["geo"];
                        if (geo) {
                            //select geo
                            mCom.Global.mMap.infoWindow.setFeatures(geo);
                            //Zoom to geo
                            mCom.Global.mMap.setExtent(geo[0].geometry.getExtent(), true);
                        }
                        break;
                    default:
                }
            });
        }
        w2ui[mCom.Global.layout_main_name].show('right', window.instant);
    }

    function map_ShowCoordinates(evt) {
        if (evt.mapPoint.x != undefined && evt.mapPoint.y != undefined) {
            dojo.byId("map_coord").innerHTML = "Tọa độ: " + evt.mapPoint.y.toFixed(3) + ", " + evt.mapPoint.x.toFixed(3);
        }
    }
    //Các hàm liên quan đến add lớp bản đồ
    var mLayers = {};
    mLayers.AddLayers = function () {
        var flayer;
        if (opts.type === "DynamicMapServiceLayer") {
            flayer = mLayers.loadARCDynamicMapServiceLayer(url, opts);
        } else if (opts.type === "FeatureService") {
            flayer = mLayers.loadFeatureService(url, opts);
        } else if (opts.type === "ImageServiceLayer") {
            flayer = mLayers.loadARCImageServiceLayer(url, opts);
        } else if (opts.type === "TiledMapServiceLayer") {
            flayer = mLayers.loadARCTiledMapServiceLayer(url, opts);
        } else if (opts.type === "Shapefile") {
            flayer = mLayers.loadShapefile(opts.url, opts);
        } else if (opts.type === "geoJSON") {
            flayer = mLayers.loadGeoJSON(opts.url, opts);
        } else if (opts.type === "EsiGeoJSON") {
            flayer = mLayers.loadESIGeoJSON(opts.url, opts);
        }
        opts.noICON = false;
        var layerToc = {
            layer: flayer,
            title: opts.title,
            noICON: (opts.noICON) ? opts.noICON : true,
            noLegend: false,
            collapsed: true,
            suppressGroup: true,
        };
        if (mCom.Global.mLayersTOC) {
            mCom.Global.mLayersTOC.push(layerToc);
        }
        mCom.Global.mLayers.push(flayer);
        mCom.Global.mMap.addLayers(mCom.Global.mLayers);

        var added = flayer.on("load", function () {
            mCom.Global.mToc.layerInfos.splice(0, 0, {
                layer: flayer,
                title: opts.title,
                noICON: true,
                noLegend: false,
                collapsed: true,
                suppressGroup: true,
            });
            mCom.Global.mToc.refresh();
            added.remove();
        });
        mCom.Global.mMap.addLayer(flayer);
    };
    mLayers.AddLayer = function (url, opts) {
        return new Promise(function (resole) {
            var flayer;
            if (opts.type === "DynamicMapServiceLayer") {
                flayer = mLayers.loadARCDynamicMapServiceLayer(url, opts);
            } else if (opts.type === "FeatureService") {
                flayer = mLayers.loadFeatureService(url, opts);
            } else if (opts.type === "ImageServiceLayer") {
                flayer = mLayers.loadARCImageServiceLayer(url, opts);
            } else if (opts.type === "TiledMapServiceLayer") {
                flayer = mLayers.loadARCTiledMapServiceLayer(url, opts);
            } else if (opts.type === "Shapefile") {
                flayer = mLayers.loadShapefile(opts.url, opts);
            } else if (opts.type === "geoJSON") {
                flayer = mLayers.loadGeoJSON(opts.url, opts);
            } else if (opts.type === "EsiGeoJSON") {
                flayer = mLayers.loadESIGeoJSON(opts.url, opts);
            }
            mCom.Global.mLayers.push(flayer);
            mCom.Global.mMap.addLayers(mCom.Global.mLayers);
            var added = flayer.on("load", function () {
                mCom.Global.mToc.layerInfos.splice(0, 0, {
                    id: opts.id,
                    layer: flayer,
                    title: opts.title,
                    noICON: true,
                    noLegend: false,
                    collapsed: true,
                    suppressGroup: true,
                });
                mCom.Global.mToc.refresh();
                added.remove();
                resole = true;
            });
        });
    };
    mLayers.loadESIGeoJSON = function () {

    };
    mLayers.loadGeoJSON = function (url, opts) {

    };
    mLayers.geoJSON2ESRI = function () {

    };
    mLayers.esri2GeoJSON = function () {

    };
    mLayers.loadShapefile = function (url, opts) {

    };
    mLayers.loadFeatureService = function (url, opts) {

    };
    mLayers.loadARCDynamicMapServiceLayer = function (url, opts) {
        var flayer;
        if (opts) {
            flayer = new ArcGISDynamicMapServiceLayer(url, {
                id: opts.id,
                type: "ArcGISDynamicMapServiceLayer",
                opacity: opts.opacity,
                disableClientCaching: true,
                outFields: ["*"],
                infoTemplate: new InfoTemplate(),
            });
        } else {
            flayer = new ArcGISDynamicMapServiceLayer(url);
        }
        return flayer;
    };
    mLayers.loadARCImageServiceLayer = function (url, opts) {

    };
    mLayers.loadARCTiledMapServiceLayer = function (url, opts) {
        var flayer;
        if (opts) {
            flayer = new ArcGISTiledMapServiceLayer(url, {
                id: opts.id,
                type: "ArcGISTiledMapServiceLayer",
                opacity: opts.opacity,
                disableClientCaching: true,
                outFields: ["*"],
                infoTemplate: new InfoTemplate(),
            });
        } else {
            flayer = new ArcGISTiledMapServiceLayer(url);
        }
        return flayer;
    };
    mLayers.mapGetAllLayer = function () {
        var arrLayers = [];
        if (mCom.Global.mToc) {
            for (var i = 0; i < mCom.Global.mToc.layerInfos.length; i++) {
                var titlesv = mCom.Global.mToc.layerInfos[i].title;
                var layersv = mCom.Global.mToc.layerInfos[i].layer;
                var layer_info = {};
                layer_info.id = i;
                layer_info.url = layersv.url;
                layer_info.title = titlesv;
                layer_info.layers = [];
                if (layersv.layerInfos) {
                    for (var j = 0; j < layersv.layerInfos.length; j++) {
                        var flayer = layersv.layerInfos[j];
                        if (flayer.visible && flayer.subLayerIds === null) {
                            var objLayer = {
                                id: flayer.id,
                                url: layersv.url + "/" + flayer.id,
                                name: flayer.name,
                                title: flayer.name,
                            }
                            layer_info.layers.push(objLayer);
                        }
                    }
                }
                arrLayers.push(layer_info);
            }
        }
        return arrLayers;
    };
    //Các hàm liên quan đến xử lý layer
    mCore.Layers = mLayers;
    //Các hàm liên quan đến search, tìm kiếm
    var mFSearchs = {};
    mFSearchs.FeatureLayer = function (url, strquery, fcresults) {
        //Tìm kiếm dữ liệu
        //var flayer = new FeatureLayer(url, {
        //    outFields: ["*"],
        //    mode: FeatureLayer.SELECTION_NEW,
        //});
        //flayer.setDefinitionExpression(strquery);
        //var query = new Query();
        //query.where = "maLoaiDat ='ODT'";
        ////mCom.Global.mMap.graphics.clear();
        //flayer.setSelectionSymbol(mCom.Global.mMap.infoWindow.fillSymbol);
        //flayer.selectFeatures(query, FeatureLayer.SELECTION_NEW, fcresults);

        var queryTask = new QueryTask(url);
        var query = new Query();
        query.returnGeometry = true;
        query.outFields = ["*"];
        query.where = strquery;
        queryTask.execute(query, fcresults);
    };
    mFSearchs.FeatureHighligh = function (geometry) {
        mCom.Global.mMap.graphics.clear();
        var highlightGraphic = new Graphic(geometry, mCom.Global.mMap.infoWindow.fillSymbol);
        mCom.Global.mMap.graphics.add(highlightGraphic);
    };
    mCore.Searchs = mFSearchs;
    /*Các hàm liên quan đến config map
     * - Config hiển thị thông tin các trường thuộc layer
     * - Config các lớp tìm kiếm
     *Lấy danh sách các lớp trên bản đồ: json cấu hình=mCom.Global.mLayersJSON
     *1.Trường hợp file layer cấu hình đã tồn tại trong csdl hoặc file
     * -Load file cấu hình
     * -Kiểm tra xem các lớp trên bản đồ đã tồn tại hay chưa
     * +Nếu tồn tại => load cấu hình
     * +Nếu chưa => gọi theo service để load thông tin default
     * 2.Chưa có file config
     * -Load thông tin default để tạo file config
     */
    var mMapConfig = {};

    mMapConfig.mMap_config_tools = function () {
        w2popup.open({
            title: 'Cấu hình Bản đồ/Lớp dữ liệu',
            width: 800,
            height: 600,
            showMax: true,
            modal: true,
            showClose: true,
            body: '<div id="main" style="position: absolute; left: 0px; top: 0px; right: 0px; bottom: 0px;"></div>',
            onOpen: function (event) {
                event.onComplete = function () {
                    $('#w2ui-popup #main').w2render(mCom.Layouts.layout_layers.name);
                    $('#toolbar_config').w2toolbar({
                        name: 'toolbar_config',
                        items: [{
                            type: 'button',
                            id: 'btnAddMap',
                            text: 'Thêm mới bản đồ',
                            icon: 'icomoon icon-map'
                        },
                        {
                            type: 'button',
                            id: 'btnAddSrv',
                            text: 'Thêm mới dịch vụ',
                            icon: 'icomoon icon-stack'
                        },
                        {
                            type: 'break'
                        },
                        {
                            type: 'button',
                            id: 'btnDelMap',
                            text: 'Xóa bản đồ',
                            icon: 'icomoon icon-remove'
                        },
                        {
                            type: 'button',
                            id: 'btnDelSrv',
                            text: 'Xóa dịch vụ',
                            icon: 'fas fa-trash-alt'
                        },
                        {
                            type: 'break'
                        },
                        {
                            type: 'button',
                            id: 'btnSave',
                            text: 'Lưu',
                            icon: 'icomoon icon-checkmark-circle'
                        },
                        {
                            type: 'button',
                            id: 'btnSaveFile',
                            text: 'Lưu cấu hình',
                            icon: 'icon-file-download2'
                        },
                        ],
                        onClick: function (event) {
                            console.log('Target: ' + event.target, event);
                        }
                    });
                    w2ui.layout_layers.content('left', w2ui.sidebar_layers);
                    w2ui.layout_layers.content('main', w2ui.grid_layers);
                    w2ui[mCom.Layouts.grid_layers.name].toolbar.hide('w2ui-reload', 'w2ui-search', 'w2ui-column-on-off', 'w2ui-break0');
                    //load dữ liệu từ bản đồ
                    mCore.MapConfig.mMap_config_layers_getall();
                    //let tabs = w2ui.layout_layers_main_tabs;
                    //tabs.on("click", mMapConfig.Tabs_onClick)
                }
            },
            onToggle: function (event) {
                event.onComplete = function () {
                    w2ui[mCom.Layouts.layout_layers.name].resize();
                }
            }
        });
        //xử lý sự kiện button
        w2ui[mCom.Layouts.grid_layers.name].toolbar.on('click', function (event) {
            switch (event.target) {
                case 'btnAddMap':
                    mCore.MapConfig.mMap_config_add();
                    break;
                case 'btnAddLayer':
                    mCore.MapConfig.mMap_config_service_add();
                    break;
                case 'btnSaveConfig':
                    mCore.MapConfig.mMap_config_save_config();
                    break;
                case 'btnSaveFile':
                    mCore.MapConfig.mMap_config_save_file_local();
                    break;
                default:
            }
        });
    };
    mMapConfig.Tabs_onClick = function (event) {
        let nodeSelected = w2ui[mCom.Layouts.sidebar_layers.name].selected;
        switch (event.target) {
            case "tab_cauhinh":
                break;
            case "tab_bando":
                //Lấy thông tin thiết lập bản đồ theo node
                if (nodeSelected != undefined || nodeSelected != null) {
                    if (nodeSelected.includes("bd_")) {
                        w2ui.frmAddMap_toolbar.on('click', mMapConfig.frmAddMap_onClick);
                    }
                    if (nodeSelected.includes("sv_")) {
                        $().w2form(mCom.Layouts.frmAddSrv);
                        w2ui.layout_layers.content('main', w2ui.frmAddSrv);
                        w2ui.frmAddSrv.on('click', mMapConfig.frmAddSrv_onClick);
                    }
                }
                break;
        }
    };
    mMapConfig.mMap_config_load = function () {
        let nodDmBanDo = {
            id: 'nod_dmbando',
            text: 'Danh mục bản đồ',
            img: 'folder-open',
            expanded: true,
            group: true
        }
        w2ui[mCom.Layouts.sidebar_layers.name].add(nodDmBanDo);
        w2ui[mCom.Layouts.sidebar_layers.name].refresh();
        $().w2form(mCom.Layouts.frmAddMap);
        w2ui.layout_layers.content('main', w2ui.frmAddMap);
        w2ui.frmAddMap.on('render', function (event) {
            event.done(function () {
                $(document).ready(function () {
                    w2ui.frmAddMap_toolbar.on('click', mMapConfig.frmAddMap_onClick);
                });
            })
        });
    };
    mMapConfig.frmAddMap_onClick = function (event) {
        let frm = w2ui.frmAddMap;
        if (event.target === "btnSave") {
            let nodbando = w2ui[mCom.Layouts.sidebar_layers.name].get("nod_dmbando");
            let nodMap = {};
            nodMap.id = "bd_" + frm.record.fmapid;
            nodMap.text = frm.record.fmapname;
            nodMap.name = frm.record.fmapname;
            nodMap.img = "icon-folder";
            nodMap.expanded = true;
            nodMap.nodes = [];
            w2ui[mCom.Layouts.sidebar_layers.name].insert(nodbando.id, null, nodMap);
            w2ui[mCom.Layouts.sidebar_layers.name].refresh();
        }
        if (event.target === "btnReset") {
            w2ui.frmAddMap.clear();
        }
        if (event.target === "btnAdd") {
            w2ui.frmAddMap.clear();
        }
    };
    mMapConfig.frmAddSrv_onClick = function (event) {

    };
    /*
     * Thêm mới một bản đồ: một bản đồ sẽ gồm nhiều dịch vụ
     * Sau đó thêm các dịch vụ bản đồ cần sử dụng
     */
    mMapConfig.mMap_config_add = function () {
        $().w2form(mCom.Layouts.frmAddMap);
        w2ui.layout_layers.content('main', w2ui.frmAddMap);
        w2ui.frmAddMap.on('action', function (event) {
            this.save();
            if (event.target === "save") {
                this.save();
                let bando = {
                    id: this.record.fmapid,
                    text: this.record.fmapname,
                    group: true,
                    expanded: true,
                    nodes: []
                }
                w2ui[mCom.Layouts.sidebar_layers.name].add(bando);
                w2ui[mCom.Layouts.sidebar_layers.name].refresh();
            }
            if (event.target === "reset") {
                this.clear();
            }
        });
    };
    /**
     * Thêm mới một dịch vụ bản bản đồ vào bản đồ
     * Các dịch vụ có thể là: ArcGISDynamicMapServiceLayer... của Esri
     * Các dịch vụ bên ngoài: WMS, WFS của Geoserver
     **/
    mMapConfig.mMap_config_service_add = function () {
        $().w2form(mCom.Layouts.frmAddSrv);
        w2ui.layout_layers.content('main', w2ui.frmAddSrv);
        w2ui.frmAddSrv.on('render', function (event) {
            event.done(function () {
                $(document).ready(function () {
                    var bando = [];
                    let nodes = w2ui[mCom.Layouts.sidebar_layers.name].nodes;
                    for (var i = 0; i < nodes.length; i++) {
                        let obj = {
                            id: nodes[i].id,
                            text: nodes[i].text
                        }
                        bando.push(obj);
                    }
                    $('input[name=fbandoid]').w2field('list', {
                        items: bando
                    });
                    var servicetype = ['ArcGISDynamicMapServiceLayer (ESRI)', 'FeatureLayer (ESRI)', 'WMSLayer', 'WMSLayerInfo', 'WFSLayer'];
                    $('input[name=fservicetype]').w2field('list', {
                        items: servicetype
                    });
                });
            })
        });
        w2ui.frmAddSrv.on('action', function (event) {
            //check trùng mã hoặc url đã có trên danh sách bản đồ
            this.save();
            if (event.target === "save") {
                let mabando = this.record.fbandoid.id;
                let nodbando = w2ui[mCom.Layouts.sidebar_layers.name].get(mabando);
                let nodService = {};
                nodService.id = nodbando.id + "/svr_" + this.record.fbandoid.id;
                nodService.text = this.record.fservicename;
                nodService.name = this.record.fserviceurl;
                //nodService.img = "icon-folder6";
                nodService.expanded = true;
                nodService.nodes = [];
                w2ui[mCom.Layouts.sidebar_layers.name].insert(nodbando.id, null, nodService);
                w2ui[mCom.Layouts.sidebar_layers.name].refresh();
            }
            if (event.target === "reset") {
                this.clear();
            }
        });
    };
    /**
     * Load tất cả các lớp trên bản đồ => tham chiếu với config lớp dữ liệu đã lưu
     * Cung cấp cho quản trị có thể cấu hình thông tin các lớp bản đồ
     **/

    mMapConfig.mMap_config_layers_getall = function () {
        let map_cofg = mMapConfig.mMap_config({
            id: mCom.Global.mBanDoId,
            name: mCom.Global.mBanDoId,
            title: mCom.Global.mBanDoTitle,
            services: [],
        })
        var arrlayer = mLayers.mapGetAllLayer();
        if (arrlayer) {
            for (var i = 0; i < arrlayer.length; i++) {
                let service = arrlayer[i];
                //lấy thông tin dịch vụ bản đồ (ARCDynamicMapServiceLayer)
                let mservice = mMapConfig.mMap_config_service({
                    id: map_cofg.id + "/svr_" + service.id,
                    name: map_cofg.id + "/svr_" + service.id,
                    img: 'icon-folder',
                    title: service.title,
                    url: service.url,
                    layers: [],
                });
                if (service.layers) {
                    for (var j = 0; j < service.layers.length; j++) {
                        let layer = service.layers[j];
                        //Lưu thông tin lớp dữ liệu (Featurelayer)
                        let mlayer = mMapConfig.mMap_config_layer({
                            id: map_cofg.id + "/lyr_" + layer.id,
                            name: map_cofg.id + "/lyr_" + layer.id,
                            fsearch: true,
                            title: layer.title,
                            url: layer.url,
                            fields: [],
                        });
                        //Duyệt danh sác các fields thuộc lớp bản đồ
                        let promise = mMapConfig.mMap_config_fields_get_byservice(layer);
                        promise.then(function (fields) {
                            //Trường hợp load mới
                            mlayer.fields = fields;
                        })
                        //Thêm ds lớp vào dịch vụ
                        mservice.layers.push(mlayer);
                    }
                }
                //Gán các dịch vụ cho bản đồ
                map_cofg.services.push(mservice);
            }
        }
        //Thiết lập thông tin cho biến toàn cục
        if (mCom.Global.mLayersJSON === null) {
            mCom.Global.mLayersJSON = [];
        }
        //Check đã tồn tại bản đồ trong file config chưa
        let isexit = false;
        if (mCom.Global.mLayersJSON.length > 0) {
            for (var i = 0; i < mCom.Global.mLayersJSON.length; i++) {
                let bando = mCom.Global.mLayersJSON[i];
                if (bando.name === map_cofg.name) {
                    isexit = true;
                    break;
                }
            }
        }
        if (!isexit) {
            mCom.Global.mLayersJSON.push(map_cofg);
        }
        //build form config
        mMapConfig.mMap_config_showedit();
    }
    mMapConfig.mMap_config = function (mcofg) {
        let mapconfig = {};
        //thông tin bản đồ
        mapconfig.id = mcofg.id || mCom.Global.mBanDoId;
        mapconfig.name = mcofg.name;
        mapconfig.title = mcofg.title;
        mapconfig.services = mcofg.services;
        return mapconfig;
    };
    mMapConfig.mMap_config_service = function (gsv) {
        //thông tin các nhóm dịch vụ
        let gservice = {};
        gservice.id = gsv.id || "sv_1";
        gservice.name = gsv.name || "sv_htsdd";
        gservice.title = gsv.title || "Hiện trạng sử dụng đất";
        gservice.url = gsv.url;
        gservice.layers = gsv.layers;
        return gservice;
    };
    mMapConfig.mMap_config_layer = function (lyr) {
        return {
            id: lyr.id,
            fsearch: lyr.fsearch,
            name: lyr.name,
            title: lyr.title,
            url: lyr.url,
            fields: lyr.fields,
        };
    };
    mMapConfig.mMap_config_layer_search = function (lyrname) {
        if (mCom.Global.mLayersJSON) {
            for (var i = 0; i < mCom.Global.mLayersJSON.length; i++) {
                let fbando = mCom.Global.mLayersJSON[i];
                if (fbando.id === mCom.Global.mBanDoId) {
                    for (var j = 0; j < fbando.services.length; j++) {
                        let fservice = fbando.services[j];
                        for (var k = 0; k < fservice.layers.length; k++) {
                            let flayer = fservice.layers[k];
                            if (flayer.title === lyrname) {
                                return flayer;
                            }
                        }
                    }
                }
            }
        }
    };
    mMapConfig.mMap_config_layer_search_byid = function (lyrid) {
        if (mCom.Global.mLayersJSON) {
            for (var i = 0; i < mCom.Global.mLayersJSON.length; i++) {
                let fbando = mCom.Global.mLayersJSON[i];
                for (var j = 0; j < fbando.services.length; j++) {
                    let fservice = fbando.services[j];
                    for (var k = 0; k < fservice.layers.length; k++) {
                        let flayer = fservice.layers[k];
                        if (flayer.id == lyrid) {
                            return flayer;
                        }
                    }
                }
            }
        }
    };
    mMapConfig.mMap_config_showedit = function () {
        var deferred = $.Deferred();
        if (mCom.Global.mLayersJSON) {
            let nodBanDo = [];
            for (var i = 0; i < mCom.Global.mLayersJSON.length; i++) {
                let itemBD = mCom.Global.mLayersJSON[i];
                let bando = {
                    id: itemBD.name,
                    text: itemBD.title,
                    img: 'icomoon icon-map',
                    group: true,
                    expanded: true,
                    nodes: []
                }
                for (var k = 0; k < itemBD.services.length; k++) {
                    let service = itemBD.services[k];
                    let nodService = {};
                    nodService.id = service.name;
                    nodService.text = service.title;
                    nodService.name = service.url;
                    //nodService.img = "icon-folder6";
                    nodService.expanded = true;
                    nodService.nodes = [];
                    for (var j = 0; j < service.layers.length; j++) {
                        let nodLayer = service.layers[j];
                        var subnode = {};
                        subnode.id = nodLayer.name;
                        subnode.name = nodLayer.url;
                        subnode.text = nodLayer.title;
                        subnode.img = 'icon-page';
                        nodService.nodes.push(subnode);
                    }
                    bando.nodes.push(nodService);
                }
                nodBanDo.push(bando);
            }
            w2ui[mCom.Layouts.sidebar_layers.name].nodes = nodBanDo;
            w2ui[mCom.Layouts.sidebar_layers.name].refresh();
        }
        w2ui[mCom.Layouts.sidebar_layers.name].on("click", mMapConfig.mMap_config_fields_get_bynode);
        return deferred;
    };
    mMapConfig.mMap_config_fields_get_bynode = function (event) {
        w2ui.layout_layers.content('main', w2ui.grid_layers);
        if (mCom.Global.mLayersJSON) {
            var node_current = event.node;
            if (node_current.id.includes("lyr_")) {
                w2ui[mCom.Layouts.grid_layers.name].showColumn("falias", "fshow", "finfo", "ftype");
                let fields = [];
                for (var i = 0; i < mCom.Global.mLayersJSON.length; i++) {
                    let item = mCom.Global.mLayersJSON[i];
                    if (item.name === node_current.parent.parent.id) {
                        for (var j = 0; j < item.services.length; j++) {
                            let service = item.services[j];
                            for (var k = 0; k < service.layers.length; k++) {
                                var flayer = service.layers[k];
                                if (flayer.name === node_current.id) {
                                    fields = flayer.fields;
                                    break;
                                }
                            }
                        }
                    }

                }
                w2ui[mCom.Layouts.grid_layers.name].records = fields;
                w2ui[mCom.Layouts.grid_layers.name].refresh();
            } else {
                w2ui[mCom.Layouts.grid_layers.name].records = [];
                w2ui[mCom.Layouts.grid_layers.name].hideColumn("falias", "fshow", "finfo", "ftype");
                w2ui[mCom.Layouts.grid_layers.name].getColumn("fname").size = "100%";
                for (var i = 0; i < mCom.Global.mLayersJSON.length; i++) {
                    let item = mCom.Global.mLayersJSON[i];
                    if (item.id === node_current.parent.id) {
                        for (var j = 0; j < item.services.length; j++) {
                            let service = item.services[j];
                            if (service.name === node_current.id) {
                                for (var k = 0; k < service.layers.length; k++) {
                                    let layer = service.layers[k];
                                    let red = {
                                        recid: k,
                                        id: k,
                                        fsearch: layer.fsearch,
                                        fname: layer.title
                                    };
                                    w2ui[mCom.Layouts.grid_layers.name].records.push(red);
                                }
                                break;
                            }

                        }
                    }
                }
                w2ui[mCom.Layouts.grid_layers.name].refresh();
            }
        }
    };
    mMapConfig.mMap_config_fields_get_byservice = function (flayer) {
        var deferred = $.Deferred();
        var arrfield = [];
        $.getJSON(flayer.url + "?f=pjson", function (data) {
            /*
             * Duyệt các field theo Url publish của ESRI
             */
            for (var i = 0; i < data.fields.length; i++) {
                var field = data.fields[i];
                let obj = {};
                obj.recid = i;
                obj.id = i;
                obj.fsearch = false;
                obj.fshow = true;
                obj.finfo = true;
                obj.fname = field.name;
                obj.falias = (field.alias) ? field.alias : field.name;
                obj.ftype = field.type;
                arrfield.push(obj);
            }
        });
        deferred.resolve(arrfield);
        return deferred.promise();
    };
    mMapConfig.mMap_config_map_add = function () {
        w2prompt({
            label: 'Layer Url',
            value: '',
            attrs: 'style="width: 100%;"',
            title: w2utils.lang('Thêm mới lớp dữ liệu'),
            ok_text: w2utils.lang('Đồng ý'),
            cancel_text: w2utils.lang('Bỏ qua'),
            width: 600,
            height: 200
        }).change(function (event) {
            //console.log('change', event);
        }).ok(function (event) {
            mTools_layers_load();
        });
    };
    mMapConfig.mMap_config_load_file_local = function () {
        $.ajax({
            async: false, //_async,
            type: "GET",
            url: "/cs/Map/MapConfig_Get",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (respone) {
                if (respone.Success) {
                    mCom.Global.mLayersJSON = JSON.parse(respone.Data);
                }
            }
        });
    };
    mMapConfig.mMap_config_save_file_local = function () {
        if (mCom.Global.mLayersJSON) {
            $.ajax({
                async: false, //_async,
                type: "POST",
                url: "/cs/Map/MapConfig_Save",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    config: mCom.Global.mLayersJSON
                }),
                success: function (respone) {
                    if (respone.Success) {
                        w2alert('Lưu cấu hình thông tin bản đồ thành công!');
                    } else {
                        w2alert('Lưu cấu hình thông tin bản đồ không thành công!');
                    }
                },
                error: function (err) {
                    w2alert('Lưu cấu hình thông tin bản đồ không thành công!');
                }
            });
        }
    };
    mMapConfig.mMap_config_save_config = function () {
        //Lưu trên grid
        w2ui[mCom.Layouts.grid_layers.name].save();
        //Lưu vào file config
        var node_current = w2ui[mCom.Layouts.sidebar_layers.name].get(w2ui[mCom.Layouts.sidebar_layers.name].selected);
        if (node_current.id.includes("lyr_")) {
            for (var i = 0; i < mCom.Global.mLayersJSON.length; i++) {
                let fbando = mCom.Global.mLayersJSON[i];
                //Duyệt danh sách các dịch vụ
                for (var j = 0; j < fbando.services.length; j++) {
                    let fservice = fbando.services[j];
                    //Duyệt danh sách các lớp bản đồ
                    for (var k = 0; k < fservice.layers.length; k++) {
                        var flayer = fservice.layers[k];
                        if (flayer.name === node_current.id) {
                            mCom.Global.mLayersJSON[i].fields = w2ui[mCom.Layouts.grid_layers.name].records;
                            return;
                        }
                    }
                }
            }
        } else {
            for (var i = 0; i < mCom.Global.mLayersJSON.length; i++) {
                let fbando = mCom.Global.mLayersJSON[i];
                //Duyệt danh sách các dịch vụ
                for (var j = 0; j < fbando.services.length; j++) {
                    let fservice = fbando.services[j];
                    if (fservice.name === node_current.id) {
                        //Duyệt danh sách các lớp bản đồ
                        for (var k = 0; k < fservice.layers.length; k++) {
                            var flayer = fservice.layers[k];
                            let recid = w2ui[mCom.Layouts.grid_layers.name].find({
                                fname: flayer.title
                            });
                            let value = w2ui[mCom.Layouts.grid_layers.name].records[recid];
                            mCom.Global.mLayersJSON[i].services[j].layers[k].fsearch = (value.fsearch) ? value.fsearch : false;
                        }
                        return;
                    }
                }
            }
        }
    };
    mCore.MapConfig = mMapConfig;
    return mCore;
})