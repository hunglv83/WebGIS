
function getMap(id) {
    $.ajax({
        type: "POST",
        //url: '@Url.Action("getIdMap", "Map")',
        url: SiteUrl + "/cs/Map/getIdMap",
        data: JSON.stringify({ id: id }),
        contentType: "application/json",
        dataType: "json",
        success: function (response) {
            console.log(response);
            var url = SiteUrl + '/xem-ban-do'
            //alert('@Url.Action("getIdMap", "Map")');
            var win = window.open(url);
        }
    });
}