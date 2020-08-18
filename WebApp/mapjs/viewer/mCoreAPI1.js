define(["libs/w2ui/w2ui",
    "viewer/mMapControl"], function (w2, mapControl) {
        'use strict';
        var apiCmom = {};
        var mLayout = {};
        var mTools = {};
        var mGlobal = {};
        var mUtils = {};
        var mCoreAPI = {};
        var mApiMap = {};
        //1. Thiết lập biến Global
        var pstyle = 'border: px solid #dfdfdf; padding: 1px;';
        var mGlobal_init = function (opts) {
            if (opts) {
                mGlobal.gisEndPoint = opts.gisEndPoint;
                mGlobal.webEndPoint = opts.gisEndPoint;
                mGlobal.mMap = opts.mMap;
                mGlobal.mToc = opts.mToc;
                mGlobal.mToolbar = opts.mToolbar;
                mGlobal.divLayout = opts.divLayout;
                mGlobal.divMap = (opts.divMap) ? opts.divMap : "map_view";
                mGlobal.divToc = (opts.divToc) ? opts.divToc : "toc_layer";
            }
            mGlobal.mLayerIds = [];
            mGlobal.layout_main_name = "map_layout";
            mGlobal.tool_main_name = "map_layout_main_toolbar";
            mGlobal.tool_left_name = "map_layout_left_toolbar";
            mGlobal.tool_right_name = "map_layout_right_toolbar";
        };
        //2. Xây dựng các hàm dùng chung
        var apiCmom_init = function () {
            apiCmom.configs = {
                layout: {
                    name: mGlobal.layout_main_name,
                    panels: [
                        {
                            type: 'top',
                            style: pstyle,
                            size: 50,
                            resizable: true,
                            content: ''
                        },
                        {
                            type: 'right',
                            style: pstyle,
                            resizable: true,
                            content: ''
                        },
                        {
                            type: 'left',
                            style: pstyle,
                            size: 200,
                            resizable: true,
                            content: '',
                            toolbar: {
                                name: mGlobal.tool_left_name,
                                tooltip: 'bottom',
                                items: [
                                    {
                                        type: 'radio',
                                        id: 'item-left-layers',
                                        text: '',
                                        check: true,
                                        icon: 'icon-stack3',
                                        tooltip: 'Lớp bản đồ'
                                    },
                                    {
                                        type: 'radio',
                                        id: 'item-left-legend',
                                        text: '',
                                        check: false,
                                        icon: 'icon-gradient',
                                        tooltip: 'Chú giải'
                                    },
                                    {
                                        type: 'radio',
                                        id: 'item-left-search',
                                        text: '',
                                        check: false,
                                        icon: 'icon-folder-search',
                                        tooltip: 'Tìm kiếm'
                                    },
                                    {
                                        type: 'buton',
                                        id: 'item-left-file',
                                        text: '',
                                        icon: 'icon-folder',
                                        tooltip: 'Quản lý'
                                    },
                                ],
                                onClick: function (event) {
                                    //Thực hiện các sự kiện click của toolbar
                                    //Thiết lập các biến môi trường
                                    //mTools.configs.tool_left = this;
                                    //Thực hiện các sự kiện click của toolbar
                                    //mTools_left_click(event);
                                }
                            }
                        },
                        {
                            type: 'main',
                            style: pstyle,
                            content: '',
                            toolbar: {
                                name: mGlobal.tool_main_name,
                                tooltip: 'bottom',
                                items: [
                                    {
                                        type: 'button',
                                        id: 'item-left-hide',
                                        text: '',
                                        icon: 'icon-circle-left2',
                                        //tooltip: 'Ẩn công cụ trái'
                                    },
                                    {
                                        type: 'button',
                                        id: 'item-left-show',
                                        text: '',
                                        hidden: true,
                                        icon: 'icon-circle-right2',
                                        //tooltip: 'Hiển thị công cụ trái'
                                    },
                                    {
                                        type: 'button',
                                        id: 'item-map-arrow',
                                        text: '',
                                        icon: 'icon-cursor',
                                        tooltip: ''
                                    },
                                    {
                                        type: 'radio',
                                        id: 'item-map-zoomin',
                                        text: '',
                                        check: false,
                                        icon: 'icon-zoomin3',
                                        tooltip: 'Phóng to bản đồ'
                                    },
                                    {
                                        type: 'radio',
                                        id: 'item-map-zoomout',
                                        text: '',
                                        check: false,
                                        icon: 'icon-zoomout3',
                                        tooltip: 'Thu nhỏ bản đồ'
                                    },
                                    {
                                        type: 'radio',
                                        id: 'item-map-pan',
                                        text: '',
                                        check: false,
                                        icon: 'icon-hand',
                                        tooltip: 'Di chuyển bản đồ'
                                    },
                                    {
                                        type: 'button',
                                        id: 'item-map-fitview',
                                        text: '',
                                        icon: 'icon-enlarge',
                                        tooltip: 'Hiển thị toàn bản đồ'
                                    },
                                    {
                                        type: 'radio',
                                        id: 'item-map-info',
                                        text: '',
                                        check: false,
                                        icon: 'icon-info22',
                                        tooltip: 'Xem thông tin đối tượng'
                                    },
                                    {
                                        type: 'button',
                                        id: 'item-map-prev',
                                        text: '',
                                        icon: 'icon-undo2',
                                        tooltip: 'Quay lại khung nhìn trước'
                                    },
                                    {
                                        type: 'button',
                                        id: 'item-map-next',
                                        text: '',
                                        icon: 'icon-redo2',
                                        tooltip: 'Quay lại khung nhìn sau'
                                    },
                                ],
                            }
                        }
                    ],
                    onResize: function (event) {
                        if (mapapi.Global.mMap) {
                            if (mapapi.Global.mMap.loaded) {
                                try {
                                    //console.log(_mMap);
                                    mapapi.Global.mMap.autoResize = true;
                                    mapapi.Global.mMap.resize();
                                    mapapi.Global.mMap.reposition();
                                } catch (e) {
                                }
                            }
                        }
                    }
                }
            }
        };
        apiCmom.ShowPopup = function (opts) {
            w2popup.open({
                title: opts.title,
                body: opts.message,
                width: 500,
                height: 300,
                overflow: 'hidden',
                color: '#333',
                speed: '0.3',
                opacity: '0.8',
                modal: true,
                showClose: true,
                showMax: true,
                onOpen: function (event) { console.log('open'); },
                onClose: function (event) { console.log('close'); },
                onMax: function (event) { console.log('max'); },
                onMin: function (event) { console.log('min'); },
                onKeydown: function (event) { console.log('keydown'); }
            });
        };
        apiCmom.ShowWait = function () {

        };
        apiCmom.HideWait = function () {

        };
        //3. Xây dựng các module liên quan đến Layout map
        var mLayout_init = function () {
            return new Promise(function (resolve) {
                var divlayout = document.getElementById(mGlobal.divLayout);
                if (w2ui.hasOwnProperty(mGlobal.layout_main_name)) {
                    w2ui[mGlobal.layout_main_name].destroy();
                }
                $(divlayout).w2layout(apiCmom.configs.layout);
                w2ui[mGlobal.layout_main_name].load('main', '/mapjs/html/mapview.html', null, function () {
                    w2ui[mGlobal.layout_main_name].hide('top', window.instant);
                    w2ui[mGlobal.layout_main_name].hide('right', window.instant);
                    w2ui[mGlobal.layout_main_name].load('left', '/mapjs/html/leftview.html', null, function () {
                        //Xử lý các sự kiện toolbar map
                        mTools_main_click();
                        mTools_left_click();
                        resolve(true);
                    });
                });
            });
        };
        //4. Xây dựng các module liên quan đến Tools
        mTools.configs = {};
        var mTools_main_checked_clear = function () {
            w2ui[mGlobal.tool_main_name].items.forEach(item => item.checked = false);
            w2ui[mGlobal.tool_main_name].refresh();
        }
        var mTools_main_left_toggle = function (isinstant) {
            if (isinstant) {
                w2ui[mGlobal.layout_main_name].show('left', window.instant);
                w2ui[mGlobal.tool_main_name].show('item-left-hide');
                w2ui[mGlobal.tool_main_name].hide('item-left-show');
            }
            else {
                w2.w2ui[mGlobal.layout_main_name].hide('left', window.instant);
                w2ui[mGlobal.tool_main_name].show('item-left-show');
                w2ui[mGlobal.tool_main_name].hide('item-left-hide');
            }
        };
        var mTools_main_right_toggle = function (isinstant) {
            if (isinstant) {
                w2ui[mGlobal.layout_main_name].show('right', window.instant);
            }
            else {
                w2.w2ui[mGlobal.layout_main_name].hide('right', window.instant);
            }
        }
        var mTools_main_click = function () {
            w2ui[mGlobal.tool_main_name].on('click', function (event) {
                switch (event.target) {
                    case 'item-left-hide':
                        mTools_main_left_toggle(false);
                        break;
                    case 'item-left-show':
                        mTools_main_left_toggle(true);
                        break;
                    case 'item-map-arrow':
                        if (mapapi.Global.mNavToolbar) {
                            mapapi.Global.mNavToolbar.deactivate();
                        }
                        //clear các button đã chọn
                        mTools_main_checked_clear();
                        break;
                    case "item-map-zoomin":
                        if (mapapi.Global.mNavToolbar) {
                            mapapi.Global.mNavToolbar.activate(esri.toolbars.Navigation.ZOOM_IN);
                        }
                        break;
                    case "item-map-zoomout":
                        if (mapapi.Global.mNavToolbar) {
                            mapapi.Global.mNavToolbar.activate(esri.toolbars.Navigation.ZOOM_OUT);
                        }
                        break;
                    case "item-map-pan":
                        if (mapapi.Global.mNavToolbar) {
                            mapapi.Global.mNavToolbar.activate(esri.toolbars.Navigation.PAN);
                        }
                        break;
                    case "item-map-fitview":
                        mapapi.Global.mNavToolbar.zoomToFullExtent();
                        break;
                    case "item-map-prev":
                        if (mapapi.Global.mNavToolbar) {
                            mapapi.Global.mNavToolbar.zoomToPrevExtent();
                        }
                        break;
                    case "item-map-next":
                        if (mapapi.Global.mNavToolbar) {
                            mapapi.Global.mNavToolbar.zoomToNextExtent();
                        }
                        break;
                    case "item-map-info":
                        break;
                    default:
                }
            });
        };
        var mTools_left_click = function () {
            w2ui[mGlobal.tool_left_name].on('click', function (event) {
                switch (event.target) {
                    case 'item-left-layers':
                        apiCmom.ShowPopup({
                            title: "Danh sách lớp bản đồ",
                            message: "Danh sách lớp bản đồ"
                        });
                        break;
                    case 'item-left-legend':
                        break;
                    case 'item-map-search':
                        break;
                    case 'item-map-tools':
                        break;
                    default:
                }
            });
        };
        var mTools_right_click = function (event) {
            switch (event.target) {
                case 'item-left-hide':
                    break;
                case 'item-left-show':
                    break;
                case 'item-map-arrow':
                    break;
                default:
            }
        };
        //5. Xây dựng các module liên quan đến Utils
        //6. Xây dựng các module liên quan đến MapApi
        var mMapApi_LoadLayer = function (url, opts) {
            mapControl.Layers.LoadLayer(url, opts);
        }
        var mMapApi_AddToMap = function (arrlayers) {

        }
        var mMapApi_Create = function () {
            var extent = {
                XMin: 205746.84020000044,
                YMin: 880185.2008999996,
                XMax: 1691327.9940999998,
                YMax: 2587136.6558,
                SpatialReference: 'PROJCS["_MI_0", GEOGCS["GCS_Unknown", DATUM["D_Unknown", SPHEROID["World_Geodetic_System_of_1984_GEM_10C", 6378137.0, 298.257223563]], PRIMEM["Greenwich", 0.0], UNIT["Degree", 0.0174532925199433]], PROJECTION["Transverse_Mercator"], PARAMETER["false_easting", 500000.0], PARAMETER["false_northing", 0.0], PARAMETER["central_meridian", 105.0], PARAMETER["scale_factor", 0.9996], PARAMETER["latitude_of_origin", 0.0], UNIT["Meter", 1.0]]'
            }

        }
        var mMapApi_ShowInfo = function (obj) {
            //Lấy thông tin thuộc tính đối tượng obj
            //Hiển thị thông tin lên form thông tin
            mTools_main_right_toggle(true);
        }
        //Thiết lập Modules cho Api
        mCoreAPI.Cmon = apiCmom;
        mCoreAPI.Tools = mTools;
        mCoreAPI.Global = mGlobal;
        mCoreAPI.Utils = mUtils;
        mCoreAPI.Init = function (opts) {
            //Thiết lập các control
            apiCmom_init();
            mGlobal_init(opts);
            apiCmom_init();
            mLayout_init().then(function (result) {
                //Khởi tạo map
                mapControl.Create({ divMap: mGlobal.divMap, divToc: mGlobal.divToc }).then(function (result) {
                    //Test load layer
                    var url = "http://localhost:6080/arcgis/rest/services/TCDCKS/NenHanhChinhVN/MapServer";
                    mapControl.Layers.LoadLayer(url, {
                        type: "DynamicMapServiceLayer",
                        id: "NenHanhChinHVN",
                        title: "Nền Việt Nam",
                        showGroupCount: false,
                        slider: false,
                        collapsed: false,
                        autoToggle: false,
                        noICON: false,
                        showLabels: false
                    });
                });
            });
        }
        return mCoreAPI;
    })