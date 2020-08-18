window.onload = function () {
    jQuery('#example5').sliderPro({
        width: 670,
        height: 500,
        orientation: 'vertical',
        loop: false,
        arrows: true,
        buttons: false,
        thumbnailsPosition: 'right',
        thumbnailPointer: true,
        thumbnailWidth: 290,
        breakpoints: {
            800: {
                thumbnailsPosition: 'bottom',
                thumbnailWidth: 270,
                thumbnailHeight: 100
            },
            500: {
                thumbnailsPosition: 'bottom',
                thumbnailWidth: 120,
                thumbnailHeight: 50
            }
        }
    });
};