function checkDuyet() {
    try {

        var checkedValues = jQuery('input[name="chkDuyet"]:checked').map(function () {
            return this.value;
        }).get();

        if (checkedValues.length == 0) {
            alertError("Bạn chưa chọn tin tức cần duyệt!");
            return false;
        }
        jQuery('#popupDuyet').modal();
        return true;
    } catch (e) {
        alertError(e);
        return false;
    }
}
function setDuyet() {
    try {
        var checkedValues = jQuery('input[name="chkDuyet"]:checked').map(function () {
            return this.value;
        }).get();
        var YKien = jQuery('#YKien').val();
        //return;
        //if (checkedValues.length == 0 ) {
        //    alertError("Bạn chưa chọn người dùng hoặc nhóm người dùng!");
        //    return false;
        //}

        var pJson = { 'arrChecked': checkedValues, 'abc': YKien };
        jQuery.ajax({
            url: SiteUrlAdmin + "/CMS_DuyetTinTuc/SetDuyet",
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(pJson),
            type: "POST",
            success: function (r) {
                console.log(r);
                if (r.status == true) {
                    jQuery('#popupDuyet').modal('hide');
                    alertSuccess(r.message);
         
                    setTimeout(function () {
                        
                        jQuery("#btnTimKiem").click();
                    }, 2000);

                }
                else {
                    alertSuccess(r.message);
                    // alertError("Lỗi gán người dùng");
                    jQuery('#popupDuyet').modal('hide');

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
            alertError("Bạn chưa chọn tin tức không duyệt!");
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
        //if (checkedValues.length == 0 ) {
        //    alertError("Bạn chưa chọn người dùng hoặc nhóm người dùng!");
        //    return false;
        //}

        var pJson = { 'arrChecked': checkedValues, 'abc': YKien };
        jQuery.ajax({
            url: SiteUrlAdmin + "/CMS_DuyetTinTuc/SetTuChoi",
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
                    alertSuccess(r.message);
                    // alertError("Lỗi gán người dùng");
                    jQuery('#popupTuChoi').modal('hide');

                }
            }
        });
    } catch (e) {
        alertError(e);
        return false;
    }
}
function clearSearch() {

    jQuery('#txtSearch').val("");
    jQuery("#btnTimKiem").click();
}