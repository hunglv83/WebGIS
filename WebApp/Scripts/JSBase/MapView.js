var  visible = [];
var identifyTask, identifyParams;
var urlService;
var xmax, xmin, ymax, ymin;
var startExtent;
var search;
var map;
require([
    "dojo/ready",
    "esri/map",
    "esri/layers/ArcGISDynamicMapServiceLayer",
    "esri/symbols/SimpleFillSymbol",
    "esri/geometry/Extent",
    "esri/SpatialReference",
    "esri/InfoTemplate",
    "esri/dijit/LayerList",
    "esri/tasks/query",
    "esri/tasks/QueryTask",
    "esri/symbols/SimpleLineSymbol",
    "esri/tasks/IdentifyTask",
    "esri/tasks/IdentifyParameters",
    "esri/dijit/Search",
    "esri/layers/FeatureLayer",
    "esri/dijit/Popup",
    "dojo/_base/array",
    "esri/Color",
    "esri/basemaps",
    "dojo/dom-construct",
    "dojo/dom-class",
    "dojo/dom-style",
    "dojo/on",
    "dojo/dom",
    "dojo/domReady!"
], function (ready,
    Map, ArcGISDynamicMapServiceLayer,
    SimpleFillSymbol, Extent, SpatialReference,
    InfoTemplate, LayerList, query, QueryTask,
    SimpleLineSymbol, IdentifyTask, IdentifyParameters, Search, FeatureLayer, Popup,
    array, Color, Basemaps, domConstruct, domClass, domStyle, on, dom   
) {
    var popup = new Popup({
        fillSymbol: new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID,
            new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID,
                new Color([255, 0, 0]), 2), new Color([255, 255, 0, 0.25]))
    }, domConstruct.create("div"));
    var loading = dom.byId("loadingImg");

    map = new Map("map", {
        basemap: "topo",
        infoWindow: popup
    });
    map.on("load", init);
    map.on("load", getExtentService);
    map.on("update-start", showLoading);
    map.on("update-end", hideLoading);


        function showLoading() {
            esri.show(loading);
            map.disableMapNavigation();
            map.hideZoomSlider();
        }

        function hideLoading(error) {
            esri.hide(loading);
            map.enableMapNavigation();
            map.showZoomSlider();
        }

    

       function getExtentService() {
        $.ajax({
            type: "POST",
            url: SiteUrl + "/cs/Map/getExtent",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                startExtent = new Extent(parseFloat(response[0]), parseFloat(response[1]), parseFloat(response[2]), parseFloat(response[3]), new SpatialReference({ wkid: 102100, latestWkid: 3857 }));
                map.setExtent(startExtent);
                console.log(2);
            },
            error: function (e) {
                console.log(JSON.stringify(e));
            },
            done: function (rs) {
                console.log("hixhix");
            }
        });

    }

    function init() {
        console.log(urlService);
        var layer = new ArcGISDynamicMapServiceLayer(urlService);
        map.addLayer(layer);
        //var items = array.map(layer.layerInfos, function (info, index) {
        //    console.log(items);
        //    visible.push(info.id);
        //});
        //layer.setVisibleLayers(visible);


        dojo.connect(map, "onExtentChange", function (ext) {
            xmin = ext.xmin;
            ymin = ext.ymin;
            xmax = ext.xmax;
            ymax = ext.ymax;
        });


        var search = new Search({
            map: map,
            zoomScale: 5000000
        }, "search");

        document.getElementById("tickExtent").addEventListener('click', function () {
            if (confirm('Bạn có chắc chắn lưu hệ tọa độ vùng?')) {
                putExtentService(xmin, ymin, xmax, ymax);
            } else {
                // Do nothing!
            }

        })

        document.getElementById("mapZoomExtent").addEventListener('click', function () {
            map.setExtent(startExtent);
        })


        document.getElementById("tickFeature").addEventListener('click', function () {
            map.on("click", executeIdentifyTask);
            identifyTask = new IdentifyTask(urlService);
            identifyParams = new IdentifyParameters();
            identifyParams.tolerance = 3;
            identifyParams.returnGeometry = true;
            identifyParams.layerIds = visible;
            console.log(visible);
            identifyParams.layerOption = IdentifyParameters.LAYER_OPTION_ALL;
            identifyParams.width = map.width;
            identifyParams.height = map.height;
        })

        var llWidget = new LayerList({
            map: map,
            layers: [{
                layer: layer,
                id: "Danh sách Layer",
                subLayers: true
            }]
        }, "layerList");
        llWidget.startup();
        llWidget.on('load', function () {
            enhanceLayerList();
        });

        function enhanceLayer(layerNode) {
            layerExpand = domConstruct.create('div');
            domClass.add(layerExpand, 'esriLayerExpand collapse');
            domConstruct.place(layerExpand, layerNode, 'first');
            on(layerExpand, 'click', function (evt) {
                var LayerNodes = query('.esriLayer');
                var SubList = query('.esriSubList', evt.target.parentNode);
                if (domClass.contains(evt.target, 'collapse')) {
                    domClass.replace(evt.target, 'expand', 'collapse');
                    domStyle.set(SubList[0], 'display', 'none');
                } else {
                    domClass.replace(evt.target, 'collapse', 'expand');
                    domStyle.set(SubList[0], 'display', '');
                }
            });
            var subListNodes = query('.esriSubListLayer', layerNode);
            if (subListNodes.length > 0) {
                array.map(subListNodes, function (subListNode) {
                    enhanceSubList(subListNode);
                });
            }
        }

        function enhanceSubList(subLayerNode) {
            var subLayerExpand;
            var subListNodes = query('.esriSubListLayer', subLayerNode);
            if (subListNodes.length > 0) {
                subLayerExpand = domConstruct.create('div');
                domClass.add(subLayerExpand, 'esriLayerExpand collapse');
                domConstruct.place(subLayerExpand, subLayerNode, 'first');
                on(subLayerExpand, 'click', function (evt) {
                    var cState = '';
                    var subListLayerNodes = query('.esriSubListLayer', evt.target.parentNode);
                    if (domClass.contains(evt.target, 'collapse')) {
                        domClass.replace(evt.target, 'expand', 'collapse');
                        cState = 'collapse';
                    } else {
                        domClass.replace(evt.target, 'collapse', 'expand');
                        cState = 'expand';
                    }
                    array.map(subListLayerNodes, function (subListLayerNode) {
                        if (cState === 'collapse') {
                            domStyle.set(subListLayerNode, 'display', 'none');
                        } else {
                            domStyle.set(subListLayerNode, 'display', 'block');
                        }
                    });
                });
            } else {
                subLayerExpand = domConstruct.create('div');
                domClass.add(subLayerExpand, 'esriLayerExpand');
                domConstruct.place(subLayerExpand, subLayerNode, 'first');
            }
        }

        function enhanceLayerList() {
            var layerExpand, subLayerExpand;
            var LayerNodes = query('.esriLayer');
            array.map(LayerNodes, function (layerNode) {
                enhanceLayer(layerNode);
            });
        }

        function executeIdentifyTask(event) {
            identifyParams.geometry = event.mapPoint;
            identifyParams.mapExtent = map.extent;
            var deferred = identifyTask
                .execute(identifyParams)
                .addCallback(function (response) {
                    return array.map(response, function (result) {
                        //updateLayerVisibility();
                        var feature = result.feature;
                        var AttrObj = result.feature.attributes;
                        var line = Object.values(AttrObj);
                        var template = new InfoTemplate("");
                        template.setTitle("");
                        feature.setInfoTemplate(template);
                        return feature;
                    });
                });

            map.infoWindow.setFeatures([deferred]);
            map.infoWindow.show(event.mapPoint);
        }

        function putExtentService(xMin, yMin, xMax, yMax) {
            $.ajax({
                type: 'POST',
                url: SiteUrl + "/cs/Map/putExtent",
                data: JSON.stringify({ xMin: xMin, yMin: yMin, xMax: xMax, yMax: yMax }),
                contentType: "application/json",
                dataType: "json",
                success: function (response) {
                    console.log(response);
                },
                error: function (e) {
                    console.log(e);
                },
                done: function (rs) {
                    console.log("hixhix");
                }
            });
        }



        function buildLayerList() {
            console.log(3);

            //on(ll, "click", updateLayerVisibility);
        }





        //map.on("load", getMapService);



        //var layerUrl = new FeatureLayer("http://10.151.130.13/ArcGIS/rest/services/RGHanhChinh/MapServer/13", {
        //    outFields: ["*"]
        //});

        ////listen for the load event and set the source properties
        //search.on("load", function () {

        //    var sources = search.sources;
        //    sources.push({
        //        featureLayer: layerUrl,
        //        placeholder: "Spain",
        //        enableLabel: false,
        //        searchFields: ["*"],
        //        displayField: "diaDanh",
        //        exactMatch: false,
        //        outFields: ["*"],

        //    });
        //    //Set the sources above to the search widget
        //    search.set("sources", sources);
        //});
        //search.startup();


        //function updateLayerVisibility() {
        //    var inputs = query(".list_item");
        //    var input;
        //    visible = [];
        //    array.forEach(inputs, function (input) {
        //        if (input.checked) {
        //            visible.push(input.id);

        //        }
        //    });
        //    if (visible.length === 0) {
        //        visible.push(-1);
        //    }
        //    visibleLayer.push(visible);
        //    layer.setVisibleLayers(visible);
        //}

    }
    });

 $(document).ready(function () {    
    $.ajax({
        type: "POST",
        url: "http://localhost/WebApp/cs/Map/getService",
        contentType: "application/json",
        dataType: "json",
        success: function (response) {
            urlService = response;
            console.log(urlService);
        },
        error: function (e) {
            console.log(e);
        },
        done: function (rs) {
            console.log("hixhix");
        }
    });

});


