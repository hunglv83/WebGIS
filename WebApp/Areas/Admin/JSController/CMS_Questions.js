jQuery(function () {
    jQuery('input[type="checkbox"]').on('click', function () {
        jQuery(this).val(this.checked ? true : false);
    });
});

function clearSearch() {
    jQuery('#txtSearch').val("");
    jQuery("#btnTimKiem").click();
}

function del(id) {
    jQuery.confirm({
        title: '<i class="fa fa-trash" aria-hidden="true"></i> WARNING',
        content: '<i class="fa fa-arrow-right" aria-hidden="true"></i> Bạn muốn xóa bản ghi này?',
        type: 'red',
        async: false,
        buttons: {
            confirm: {
                text: 'Đồng ý',
                btnClass: 'btn-red',
                action: function () {
                    var pJson = { 'id': id };
                    jQuery.ajax({
                        url: SiteUrlAdmin + "/CMS_Questions/Delete",
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

function view(idquestion) {
    var pJson = { 'idquestion': idquestion };
    jQuery.ajax({
        url: SiteUrlAdmin + "/CMS_Questions/Detail",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(pJson),
        type: "POST",
        success: function (r) {
            console.log(r);
            if (r.state == true) {
                var Q = r.objQ;
                jQuery('#modal-title').text(Q.Title);
                jQuery('#q').html(Q.Contents);
                jQuery('#a').html(Q.Answer);
                if (Q.FileName != null && Q.FileName != '') {
                    var sName = Q.FileName.split('/');
                    var content = '<span style="color:brown;">' + sName[sName.length - 1] + '</span> <a href="' + SiteUrlImgCKFinder + Q.FileName + '" target="blank" title="tải về"><i class="fas fa-download"></i></a>';
                    jQuery('#f').html(content);
                }
                else {
                    jQuery('#f').html('');
                }
                jQuery('#qModal').modal('show');
            }
            else {
                alert(r.mess);
            }
        }
    });
}


function removeFile() {
    jQuery('#FileName').val('');
}