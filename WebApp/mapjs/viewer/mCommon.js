define([], function () {
    'use strict';
    var pstyle = 'border: px solid #dfdfdf; padding: 1px;';
    var mCommon = {};
    mCommon.VERSION = 1.0;
    var mGlobal = {
        mMap: null, //Obj Mapcontrol
        mToc: null, //Obj LayerControl
        mNavToolbar: null, //Obj ToolBar Control
		mMapMeasurement: null,
        mLayers: [], //Danh sách các lớp bản đồ
        mLayersTOC: [], //Danh sách các lớp bản đồ trên TOC
        mCurrentLayer: null, //Layer hiện tại, đang selected
        mIdentify: null, //Trạng thái xem thông tin đối tượng
        mLayersJSON: null, //Cấu hình hiển thị lớp bản đồ, cấu hình các field
        mBanDoId: null,
        mBanDoTitle: null,
        mDivMain:
            '<div id="map_loading" class="loading" style="display:none!important"></div>'
            + '<div id="main_content" class="map-view"></div>',
        mDivMainContent:
            '<div id="map_view" class="map-view"></div>'
            + '<div id="map_coord" class="map-coord"></div>',
        mDivLeftContent:
            '<div id="left_content" class="left-view">' //Div left-content
            + '<div id="toc_layer" class="left-view" style="display:block!important"></div>'
            + '<div id="layers_search" class="left-view" style="display:none">'//Div layer Search
            + '<div class="search-option">'
            + '<div class="form-group" style="margin-bottom:0px;">'
            + '<label>Tìm kiếm: </label>'
            + '<input type="text" id="txtsearch" class="form-control" />'
            + '</div>'
            + '<div class="form-group" style="margin-bottom:0px;">'
            + '<label>Lớp dữ liệu: </label>'
            + '<select id="cbolayer" class="form-control select2"></select>'
            + '</div>'
            + '<div class="form-group" style="margin-top:10px;text-align: center;">'
            + '<input type="button" id="btnSearch" class= "btn btn-primary" value="Tìm kiếm" />'
            + '</div>'
            + '</div>'
            + '<div id="resultsCount"></div>'
            + '<div class="search-result">'
            + '<div class= "form-group">'
            + '<div id="search-show-results" class="search-result-grid"></div>'
            + '</div>'
            + '</div>'
            + '</div>'//End div layer_search
            + '</div>',//End div left_content
        mDivRinghtContent:
            '<div id="right_content" class="right-view">'
            + '<div class= "form-group">'
            + '<div id="ObjResults" class="infomations"></div>'
            + '</div>'
            + '</div>',
        layout_main_name: "map_layout",
        tool_main_name: "map_layout_main_toolbar",
        tool_left_name: "map_layout_left_toolbar",
        tool_right_name: "map_layout_right_toolbar",
        mGisEndPoint: "",
        mWebEndPoint: "",
    };
    var mLayout = {
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
                    size: 200,
                    resizable: true,
                    content: '',
                    toolbar: {
                        name: mGlobal.tool_right_name,
                        tooltip: 'bottom',
                        items: [
                            {
                                type: 'button',
                                id: 'item-right-exit',
                                text: '',
                                icon: 'icon-exit',
                                tooltip: 'Đóng'
                            },
                        ]
                    }
                },
                {
                    type: 'left',
                    style: pstyle,
                    size: 250,
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
                                checked: true,
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
                            /*{
                                type: 'button',
                                id: 'item-left-tools',
                                text: '',
                                icon: 'icomoon icon-equalizer',
                                tooltip: 'Cấu hình bản đồ'
                            },*/
                        ]
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
						    {
                                type: 'button',
                                id: 'item-map-measurement',
                                text: '',
                                icon: 'icomoon icon-steam',
                                tooltip: 'Công cụ đo'
                            },
                        ],
                    }
                }
            ]
        },
        layout_layers: {
            name: 'layout_layers',
            padding: 0,
            panels: [
                //{ type: 'top', size: 35, content: '<div id="toolbar_config" style="padding: 4px;"></div>', style: 'border-bottom: 1px solid silver;' },
                { type: 'left', size: 200, resizable: true, minSize: 120 },
                { type: 'main', minSize: 550, overflow: 'hidden' }
            ]
        },
        sidebar_layers: {
            name: 'sidebar_layers',
            nodes: [],
        },
        grid_layers: {
            name: 'grid_layers',
            show: {
                toolbar: true,
                footer: true,
                lineNumbers: true,
                //selectColumn: true
            },
            toolbar: {
                items: [
                    //{ type: 'button', id: 'btnAddMap', caption: 'Thêm bản đồ', img: 'icon-file-plus' },
                    //{ type: 'button', id: 'btnAddLayer', caption: 'Thêm lớp', img: 'icon-file-plus' },
                    { type: 'button', id: 'btnSaveConfig', caption: 'Lưu', img: 'icon-file-check' },
                    { type: 'button', id: 'btnSaveFile', caption: 'Lưu cấu hình', img: 'icon-file-download2' },
                ]
            },
            reorderColumns: true,
            columns: [
                { field: 'recid', caption: 'Rec. Id', size: '50px', resizable: true, hidden: true },
                {
                    field: 'fsearch', caption: 'Tìm kiếm', size: '60px', resizable: false, sortable: false,
                    editable: { type: 'checkbox', style: 'text-align: center' }
                },
                {
                    field: 'fshow', caption: 'Hiển thị', size: '60px', resizable: false, sortable: false,
                    editable: { type: 'checkbox', style: 'text-align: center' }
                },
                {
                    field: 'finfo', caption: 'Xem thông tin', size: '60px', resizable: false, sortable: false,
                    editable: { type: 'checkbox', style: 'text-align: center' }
                },
                { field: 'fname', caption: 'Tên trường', size: '150px', resizable: true, sortable: false, },
                {
                    field: 'falias', caption: 'Tên hiển thị', size: '150px', resizable: true, type: 'text', sortable: false,
                    editable: { type: 'text', style: 'text-align: left' }
                },
                { field: 'ftype', caption: 'Kiểu giá trị', size: '100%', resizable: true, sortable: false, },
            ],
            records: []
        },
        grid_search_result: {
            name: 'grid_search_result',
            show: {
                toolbar: false,
                footer: true,
                lineNumbers: true,
            },
            reorderColumns: true,
            columns: [],
            records: []
        },
        popup: {
            title: "Thông báo",
            body: "",
            width: 500,
            height: 300,
            overflow: 'hidden',
            color: '#333',
            speed: '0.3',
            opacity: '0.8',
            modal: true,
            showClose: true,
            showMax: true,
        },
        frmAddMap: {
            name: "frmAddMap",
            formHTML:
                '<div class="w2ui-page page-0">' +
                '    <div class="w2ui-field">' +
                '        <label>Mã bản đồ</label>' +
                '        <div>' +
                '            <input name="fmapid" type="text" maxlength="100" size="60"/>' +
                '        </div>' +
                '    </div>' +
                '    <div class="w2ui-field">' +
                '        <label>Tên bản đồ</label>' +
                '        <div>' +
                '            <input name="fmapname" type="text" maxlength="100" size="60"/>' +
                '        </div>' +
                '    </div>' +
                '    <div class="w2ui-field">' +
                '        <label>Mô tả</label>' +
                '        <div>' +
                '            <input name="fmapdesc" type="textarea" maxlength="100" size="60"/>' +
                '        </div>' +
                '    </div>' +
                '</div>',
            fields: [
                { name: 'fmapid', type: 'text', required: true },
                { name: 'fmapname', type: 'text', required: true },
                { name: 'fmapdesc', type: 'textarea' },
            ],
            toolbar: {
                items: [
                    { id: 'btnAdd', type: 'button', caption: 'Thêm mới', img: 'icon-folder' },
                    { id: 'bt3', type: 'spacer' },
                    { id: 'btnReset', type: 'button', caption: 'Làm lại', img: 'icon-page' },
                    { id: 'btnSave', type: 'button', caption: 'Lưu', img: 'icon-page' }
                ]
            }
        },
        frmAddSrv: {
            name: "frmAddSrv",
            formHTML:
                '<div class="w2ui-page page-0">' +
                '    <div class="w2ui-field">' +
                '        <label>Mã dịch vụ:</label>' +
                '        <div>' +
                '            <input name="fserviceid" type="text" maxlength="100" size="60"/>' +
                '        </div>' +
                '    </div>' +
                '    <div class="w2ui-field">' +
                '        <label>Bản đồ:</label>' +
                '        <div>' +
                '            <div><input name="fbandoid" type="text" maxlength="100" style="width: 300px !important"></div>' +
                '        </div>' +
                '    </div>' +
                '    <div class="w2ui-field">' +
                '        <label>Tên dịch vụ:</label>' +
                '        <div>' +
                '            <input name="fservicename" type="text" maxlength="100" size="60"/>' +
                '        </div>' +
                '    </div>' +
                '    <div class="w2ui-field">' +
                '        <label>Dịch vụ URL:</label>' +
                '        <div>' +
                '            <input name="fserviceurl" type="text" maxlength="300" size="250"/>' +
                '        </div>' +
                '    </div>' +
                '    <div class="w2ui-field">' +
                '        <label>Loại dịch vụ:</label>' +
                '        <div>' +
                '            <div><input name ="fservicetype" type="text" maxlength="100" style="width: 300px !important"></div>' +
                '        </div>' +
                '    </div>' +
                '</div>',
            fields: [
                { name: 'fserviceid', type: 'text', required: true },
                { name: 'fbandoid', type: 'list', required: true },
                { name: 'fservicename', type: 'text', required: true },
                { name: 'fserviceurl', type: 'text', required: true },
                { name: 'fservicetype', type: 'list', required: true },
            ],
            toolbar: {
                items: [
                    { id: 'btnAdd', type: 'button', caption: 'Thêm mới', img: 'icon-folder' },
                    { id: 'bt3', type: 'spacer' },
                    { id: 'btnReset', type: 'button', caption: 'Làm lại', img: 'icon-page' },
                    { id: 'btnSave', type: 'button', caption: 'Lưu', img: 'icon-page' }
                ]
            }
        },
    };
    mCommon.Layouts = mLayout;
    mCommon.Global = mGlobal;
    return mCommon;
})