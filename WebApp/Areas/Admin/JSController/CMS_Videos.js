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
        content: '<i class="fa fa-arrow-right" aria-hidden="true"></i> Bạn muốn xóa video này?',
        type: 'red',
        async: false,
        buttons: {
            confirm: {
                text: 'Đồng ý',
                btnClass: 'btn-red',
                action: function () {
                    var pJson = { 'id': id };
                    jQuery.ajax({
                        url: SiteUrlAdmin + "/CMS_Videos/Delete",
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

function playvideo(e) {
    var title = jQuery(e).attr('name');
    var source = jQuery(e).attr('data-video');
    jQuery('#modal-title').text(title);
    var content = '<video width="400" controls>';
    content += '<source src="' + source + '" type="video/mp4" id="play">';
    content += 'Your browser does not support HTML5 video.';
    content += '</video>';
    jQuery('#play').html(content);
    jQuery('#videoModal').modal('show');
}

function removeFile() {
    jQuery('#FileName').val('');
}


function removeFile() {
    jQuery('#pre_picture').attr('src', '');
    jQuery('#AvatarPicture').val('');
}