jQuery(function () {

});

function del(id) {
    jQuery.confirm({
        title: '<i class="fa fa-trash" aria-hidden="true"></i> WARNING',
        content: '<i class="fa fa-arrow-right" aria-hidden="true"></i> Bạn muốn xóa lịch họp này?',
        type: 'red',
        async: false,
        buttons: {
            confirm: {
                text: 'Đồng ý',
                btnClass: 'btn-red',
                action: function () {
                    var pJson = { 'id': id };
                    jQuery.ajax({
                        url: SiteUrl + "/Admin/CMS_Schedules/Delete",
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

function export_excel() {
    var date = jQuery('#date').val();
    var pJson = { 'date': date };
    jQuery.ajax({
        url: SiteUrlAdmin + "/cms_schedules/export_excel",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(pJson),
        type: "POST",
        success: function (r) {
            if (r.state == true) {
                console.log(r);
            }
        }
    });

}