//var SiteUrl = "http://localhost:8081";
//var SiteUrlAdmin = "http://localhost:8081/Admin";
//var SiteUrlImgCKFinder = "http://localhost:8081";

var SiteUrl = "http://localhost:8081";
var SiteUrlAdmin = "http://localhost:8081/Admin";
var SiteUrlImgCKFinder = "http://localhost:8081";

//---------------------START-HOANGND-------------------------

function alertSuccess(mess) {
    jQuery('#AlertBoxJS').removeClass().html(mess);
    jQuery('#AlertBoxJS').addClass("alert alert-success");
    jQuery('#AlertBoxJS').show(500);
    jQuery('#AlertBoxJS').delay(2000).hide(1000);
}

function alertError(mess) {
    jQuery('#AlertBoxJS').removeClass().html(mess);
    jQuery('#AlertBoxJS').addClass("alert alert-danger");
    jQuery('#AlertBoxJS').show(500);
    jQuery('#AlertBoxJS').delay(2000).hide(1000);
}
function alertWarning(mess) {
    jQuery('#AlertBoxJS').removeClass().html(mess);
    jQuery('#AlertBoxJS').addClass("alert alert-warning");
    jQuery('#AlertBoxJS').show(500);
    jQuery('#AlertBoxJS').delay(2000).hide(1000);
}


//hover dropdown menu
jQuery(document).ready(function () {
    var tDelay = 100;
    jQuery('ul.nav li.dropdown').hover(function () {
        jQuery(this).find('.dropdown-menu').eq(0).stop(true, true).delay(tDelay).fadeIn(500);
    }, function () {
        jQuery(this).find('.dropdown-menu').stop(true, true).delay(tDelay).fadeOut(500);
    });
    jQuery('ul.dropdown-menu li.dropdown-submenu').hover(function () {
        jQuery(this).find('.dropdown-menu').eq(0).stop(true, true).delay(tDelay).fadeIn(500);
    }, function () {
        jQuery(this).find('.dropdown-menu').stop(true, true).delay(tDelay).fadeOut(500);
    });
    //roleMenu();
});

//remove not role menu
function roleMenu() {
    var rolePageID = jQuery('#roleMenu').val();
    console.log(rolePageID);
}

//replace img "csf_mvc"
jQuery(function () {
    //replaceUrl();
});

function replaceUrl() {
    jQuery(".container.content img").each(function (index) {
        var src_img = jQuery(this).attr("src");
        src_img = src_img.replace('csf_mvc', 'vukhcn');
        jQuery(this).attr("src", src_img);
    });
}
//--------------------END-HOANGND----------------------------