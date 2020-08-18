$(function () {
    var api = {};
    api.Global = {}
    //var path = location.pathname.replace(/[^\/]+$/, '');
    var path = location.origin + "/";
    window.dojoConfig = {
        async: true,
		locale: 'vi',
        packages: [{
            name: 'libs',
            location: path + 'mapjs/libs'
        }, {
            name: 'viewer',
            location: path + 'mapjs/viewer'
        }, {
            name: 'agsjs',
            location: path + 'mapjs/viewer/dijit'
        }, {
            name: 'proj4js',
            location: path + 'mapjs/libs/proj4'
        }]
    };
    api.init = function () {
        require(window.dojoConfig, [
            "viewer/mMapAPI",
        ], function (mapapi) {
                mapapi.Init("bando");
                api.Global = mapapi.Global;
        });
    };
    mapapi = api;
    if (typeof define === "function" && define.amd) {
        define("mapapi", api);
    } else if (typeof module === "object" && module.exports) {
        module.exports = api;
    }
}());