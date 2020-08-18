function del(id) {
    var obj = jQuery('#td_' + id).html();
    Confirm('Xóa dữ liệu', 'Bạn thật sự muốn xóa "<b>' + obj + '</b>"?', 'Đồng ý', 'Hủy bỏ', id, SiteUrlAdmin + "/QT_Users/Delete");
}

function Confirm(title, msg, $true, $false, id, url) {
    var $content = "<div class='dialog-ovelay'>" +
        "<div class='dialog'><header>" +
        " <h3> " + title + " </h3> " +
        "<i class='fa fa-close'></i>" +
        "</header>" +
        "<div class='dialog-msg'>" +
        " <p> " + msg + " </p> " +
        "</div>" +
        "<footer>" +
        "<div class='controls'>" +
        " <button class='button button-default cancelAction'>" + $false + "</button> " +
        " <button class='button button-danger doAction'>" + $true + "</button> " +
        "</div>" +
        "</footer>" +
        "</div>" +
        "</div>";
    $('body').prepend($content);
    $('.doAction').click(function () {
        var pJson = { 'id': id };
        jQuery.ajax({
            url: url,
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

        $(this).parents('.dialog-ovelay').fadeOut(500, function () {
            $(this).remove();
        });
    });
    $('.cancelAction, .fa-close').click(function () {
        $(this).parents('.dialog-ovelay').fadeOut(500, function () {
            $(this).remove();
        });
    });

}

function clearSearch() {
    jQuery('#txtSearch').val("");
    jQuery("#btnTimKiem").click();
}