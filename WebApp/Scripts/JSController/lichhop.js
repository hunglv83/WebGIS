ok_lichhop();
function ok_lichhop() {
    if (window.jQuery) {
        jQuery('#date').datetimepicker({
            format: 'DD/MM/YYYY'
        });
    }
    else {
        window.setTimeout("ok_lichhop();", 1000);
    }
}