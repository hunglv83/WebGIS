
function setCongBo() {
    try {
        var checkedValues = jQuery('input[name="chkDuyet"]:checked').map(function () {
            return this.value;
        }).get();
        var checkedTieuDiem = jQuery('input[name="chkTieuDiem"]:checked').map(function () {
            return this.value;
        }).get();
        if (checkedValues.length == 0) {
            alertError("Bạn chưa chọn tin tức được công bố!");
            return false;
        }

        var pJson = { 'arrChecked': checkedValues, 'checkedTieuDiem': checkedTieuDiem };
        jQuery.ajax({
            url: SiteUrlAdmin + "/CMS_CongBoTinTuc/SetCongBo",
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(pJson),
            type: "POST",
            success: function (r) {
                console.log(r);
                if (r.status == true) {
                    alertSuccess(r.message);
                    setTimeout(function () {
                        jQuery("#btnTimKiem").click();
                    }, 2000);
                }
                else {
                    alertSuccess(r.message);

                }
            }
        });
    } catch (e) {
        alertError(e);
        return false;
    }
}
function checkTuChoi() {
    try {

        var checkedValues = jQuery('input[name="chkDuyet"]:checked').map(function () {
            return this.value;
        }).get();

        if (checkedValues.length == 0) {
            alertError("Bạn chưa chọn tin tức ngừng công bố!");
            return false;
        }
        jQuery('#popupTuChoi').modal();
        return true;
    } catch (e) {
        alertError(e);
        return false;
    }
}
function setTuChoi() {
    try {
        var checkedValues = jQuery('input[name="chkDuyet"]:checked').map(function () {
            return this.value;
        }).get();
        var YKien = jQuery('#YKienTuChoi').val();
        //return;
        //if (checkedValues.length == 0 ) {
        //    alertError("Bạn chưa chọn người dùng hoặc nhóm người dùng!");
        //    return false;
        //}

        var pJson = { 'arrChecked': checkedValues, 'abc': YKien };
        jQuery.ajax({
            url: SiteUrlAdmin + "/CMS_CongBoTinTuc/SetNgungCongBo",
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(pJson),
            type: "POST",
            success: function (r) {
                console.log(r);
                if (r.status == true) {

                    jQuery('#popupTuChoi').modal('hide');
                    alertSuccess(r.message);
                    setTimeout(function () {
                        jQuery("#btnTimKiem").click();
                    }, 2000);
                   
                }
                else {

                    // alertError("Lỗi gán người dùng");
                    jQuery('#popupTuChoi').modal('hide');
                    alertSuccess(r.message);

                }
            }
        });
    } catch (e) {
        alertError(e);
        return false;
    }
}