
 
//слайдер
$('.carousel').carousel({
    interval: 5000,
    pause: 'hover',
    wrap: true
});

//наверх
$("#top-link").hide();
$(window).scroll(function() {
    if ($(this).scrollTop() > 100) {
        $('#top-link').fadeIn();
    } else {
        $('#top-link').fadeOut();
    }
});
$('#top-link a').click(function() {
    $('body,html').animate(
    { scrollTop: 0 }, 800);
    return false;
});

$(window).load(function () {
    SetDiscountValueOnProductPicture();

    ////сдвиг при модальном окне
    function getScrollBarMargin() {
        var height_document = $(document).height();
        var height_client = document.body.clientHeight;

        getScrollBarWidth();
        var widthScroll = getScrollBarWidth();
        var plusWidthSroll = widthScroll + "px";
        $('body.modal-open, .modal-open .navbar-fixed-top, .modal-open .navbar-fixed-bottom').css({ 'margin-right': plusWidthSroll });


        if (height_document > height_client) {
            $('body.modal-open, .modal-open .navbar-fixed-top, .modal-open .navbar-fixed-bottom').css({ 'margin-right': 0 });
        }
    };

    $('#myModal').on('shown.bs.modal', function () {
        getScrollBarMargin();
    });
    $('#myModal').on('hide.bs.modal', function () {
        $('body.modal-open, .modal-open .navbar-fixed-top, .modal-open .navbar-fixed-bottom').css({ 'margin-right': 0 });
    });

    $("#myAlert").on('shown.bs.modal', function () {
        getScrollBarMargin();
    });
    $('#myAlert').on('hide.bs.modal', function () {
        $('body.modal-open, .modal-open .navbar-fixed-top, .modal-open .navbar-fixed-bottom').css({ 'margin-right': 0 });
    });

});

$(window).resize(function () {
    SetDiscountValueOnProductPicture();
});

//ширина скролла
function getScrollBarWidth() {
    var $outer = $('<div>').css({ visibility: 'hidden', width: 100, overflow: 'scroll' }).appendTo('body'),
        widthWithScroll = $('<div>').css({ width: '100%' }).appendTo($outer).outerWidth();
    $outer.remove();
    return 100 - widthWithScroll;
};

//скидка
function SetDiscountValueOnProductPicture() {
    var mg = $(".product-image").width();
    var mgr = $(".product-image img").width();
    var result = (mg - mgr) / 2;
    $(".discount-img").css("right", result);
    $(".discount-nm").css("right", result);
}


//dropdown center

$(document).ready(function () {
    $("button.dropdown-toggle").click(function (ev) {
       var t = $(this);
       var g = t.next(".dropdown-menu");
       var tPadding = t.css('padding-left');
       var tPaddingNumber = parseFloat(tPadding);
       var width = t.width();
       var twidth = width + tPaddingNumber * 2;

       var gPadding = g.css('padding-left');
       var gPaddingNumber = parseFloat(gPadding);
       var ggwidth = g.width();
       var gwidth = ggwidth + gPaddingNumber * 2;

         var result = ((twidth - gwidth) / 2) + "px";
            g.css("left", result);
    });
});