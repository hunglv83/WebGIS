function clearSearch() {
    jQuery('#txtSearch').val("");
    jQuery("#btnTimKiem").click();
}
function del(id) {
    jQuery.confirm({
        title: '<i class="fa fa-trash" aria-hidden="true"></i> WARNING',
        content: '<i class="fa fa-arrow-right" aria-hidden="true"></i> Bạn muốn xóa bản đồ này?',
        type: 'red',
        async: false,
        buttons: {
            confirm: {
                text: 'Đồng ý',
                btnClass: 'btn-red',
                action: function () {
                    var pJson = { 'id': id };
                    jQuery.ajax({
                        url: SiteUrl + "/Admin/CMS_Maps/Delete",
                        contentType: 'application/json',
                        data: pJson,
                        dataType: 'json',
                        type: "GET",
                        success: function (response) {
                            if (response.status == true) {
                                jQuery("#btnTimKiem").click();
                            }
                            else {
                                alertError(response.message);
                            }
                        }
                    });
                }
            },
            cancel: {
                text: 'Hủy',
                action: function () {
                }
            }
        }
    });
}

function checkChon(id) {
    try {

        //var checkedValues = jQuery('input[name="chkServices_"]:checked').map(function () {
        //    return this.value;
        //}).get();
        var abc = '#ThuTu_' + id;
        var abc1 = '#chkServices_' + id;
        var checked1 = jQuery(abc1).prop( "checked" );
        if (checked1 == false) {
            jQuery(abc).hide();
        } else {
            jQuery(abc).show();
        }
        return true;
    } catch (e) {
        alertError(e);
        return false;
    }
}