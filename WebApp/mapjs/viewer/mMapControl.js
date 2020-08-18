define([
    "dojo/dom-construct",
    "esri/map",
    "esri/toolbars/navigation",
    "agsjs/TOC",
    "esri/dijit/Popup",
    "dojo/_base/Color",
    "esri/symbols/SimpleFillSymbol",
    "esri/symbols/SimpleLineSymbol",
    "esri/dijit/Basemap",
    "esri/layers/ArcGISDynamicMapServiceLayer",
    "esri/SpatialReference",
], function (domConstruct, Map, Navigation, TOC, Popup, Color,
    SimpleFillSymbol, SimpleLineSymbol,
    Basemaps,
    ArcGISDynamicMapServiceLayer,
    SpatialReference) {
    'use strict';
    var mapModule = {};
    mapModule.VERSION = 1.0;
    var popup = new Popup({
        fillSymbol: new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID,
            new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID,
                new Color([255, 0, 0]), 2), new Color([255, 255, 0, 0.25]))
    }, domConstruct.create("div"));
    //1.Các hàm liên quan đến khởi tạo map
    var objMap = {};
    //Khởi tạo base map
    var map_setBasemap = function (base) {
        console.log(base.type);
        if (base.type) {
            //Lấy base map theo id
            if (base.type == "id") {
                mapapi.Global.mMap.setBasemap(url);
            }
            //Thiết lập basemap theo Url
            else {
                Basemaps.mybasemap = {
                    title: base.title,
                    thumbnailUrl: base.thumbnail,
                    //itemId: 'ulas',
                    baseMapLayers: [
                        { url: base.url }
                    ]
                };
                mapapi.Global.mMap.setBasemap("mybasemap");
            }
        }
        else {
            //Thiết lập mặc định
            mapapi.Global.mMap.setBasemap("streets");
        }
    };
    mapModule.Create = function (opts) {
        return new Promise(function (resolve) {
            esriConfig.defaults.map.panDuration = 350; // time in milliseconds, default panDuration: 350
            esriConfig.defaults.map.panRate = 25; // default panRate: 25
            esriConfig.defaults.map.zoomDuration = 1; // default zoomDuration: 500
            esriConfig.defaults.map.zoomRate = 1; // default zoomRate: 25,
            esriConfig.defaults.map.zoomAnimationThrottled = true;
            var src = new SpatialReference(4326);
            objMap.mMap = new Map(opts.divMap, {
                logo: false,
                infoWindow: popup,
                slider: false,
                showLabels: false,
                autoResize: false,
            });
            //Khởi tạo TOC layers
            //var mToc = new TOC({
            //    map: objMap.mMap,
            //    layerInfos: []
            //}, opts.divToc);
            //mToc.startup();
            //objMap.mToc = mToc;
            objMap.mMap.on("load", function () {
                if (objMap.mMap.loaded) {
                    objMap.mMap.autoResize = true;
                    objMap.mMap.resize();
                    objMap.mMap.reposition();
                }
                popup.popupWindow = true;
            });
            objMap.mMap.on("layer-add-result", function (layer) {
                var toc = new TOC({
                    map: map,
                    layerInfos: [{
                        layer: layer,
                        title: "Bản đồ hiện trạng",
                        noICON: true,
                        noLegend: false,
                        collapsed: true,
                        suppressGroup: true,
                    }]
                }, opts.divToc);
                toc.startup();
            })
            objMap.mMap.on("layers-add-result", function (layers) {
                var toc = new TOC({
                    map: map,
                    layerInfos: [{
                        layer: layers,
                        title: "Bản đồ hiện trạng",
                        noICON: true,
                        noLegend: false,
                        collapsed: true,
                        suppressGroup: true,
                    }]
                }, opts.divToc);
                toc.startup();
            })
            objMap.mMap.on("mouse-move", map_showCoordinates);
            //Khởi tạo navigation
            objMap.mNavToolbar = new Navigation(objMap.mMap);
            mapapi.Global.push(objMap);
            if (opts.basemap) {
                map_setBasemap(opts.basemap);
            }
            resolve(mapapi.Global);
        });
    };
    var map_showCoordinates = function (evt) {
        if (evt.mapPoint.x && evt.mapPoint.y) {
            dojo.byId("map_coord").innerHTML = "Tọa độ: " + evt.mapPoint.y.toFixed(3) + ", " + evt.mapPoint.x.toFixed(3);
        }
    };
    //2. Các hàm liên quan đến basemap
    //3. Các hàm liên quan đến load layer
    var mLayers = {};
    mLayers.LoadLayers = function (arrlayers) {
        objMap.mMap.addLayers(arrlayers);
    }
    mLayers.LoadLayer = function (url, opts) {
        var flayer;
        if (opts.type === "DynamicMapServiceLayer") {
            flayer = mLayers.loadARCDynamicMapServiceLayer(url, opts);
        }
        else if (opts.type === "FeatureService") {
            flayer = mLayers.loadFeatureService(url, opts);
        }
        else if (opts.type === "ImageServiceLayer") {
            flayer = mLayers.loadARCImageServiceLayer(url, opts);
        }
        else if (opts.type === "TiledMapServiceLayer") {
            flayer = mLayers.loadARCTiledMapServiceLayer(url, opts);
        }
        else if (opts.type === "Shapefile") {
            flayer = mLayers.loadShapefile(opts.url, opts);
        }
        else if (opts.type === "geoJSON") {
            flayer = mLayers.loadGeoJSON(opts.url, opts);
        }
        else if (opts.type === "EsiGeoJSON") {
            flayer = mLayers.loadESIGeoJSON(opts.url, opts);
        }
        //Thêm vào danh sách lớp bản đồ
        mapapi.Global.mLayerIds.push(flayer);
        objMap.mMap.addLayers(mapapi.mGlobal.mLayerIds);
    }
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
                "id": opts.id,
                "opacity": opts.opacity,
            });
        }
        else {
            flayer = new ArcGISDynamicMapServiceLayer(url);
        }
        return flayer;
    };
    mLayers.loadARCImageServiceLayer = function (url, opts) {

    };
    mLayers.loadARCTiledMapServiceLayer = function (url, opts) {

    };
    //4. Các hàm liên quan đến map label
    var mLabels = {};
    mLayers.FeatureLayerLabel = function () {

    }
    mapModule.Maps = objMap;
    mapModule.Layers = mLayers;
    mapModule.Labels = mLabels;
    return mapModule;
})