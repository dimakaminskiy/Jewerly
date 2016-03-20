
 
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

////счетчик товаров
//$('.product-info button').click(function () {
//    var countT = $(".countProduct").text();
//    var countN = parseFloat(countT);
//    var number = countN + 1;
//    $(".countProduct").text(number);
//    $('.countProduct').css('opacity', 0).animate({
//        opacity: 1
//    });
//});

//$(function () {
//    $('#myAlert').on('show.bs.modal', function () {
//        var myAlert = $("#myAlert");
//        clearTimeout(myAlert.data('hideInterval'));
//        myAlert.data('hideInterval', setTimeout(function () {
//            myAlert.modal('hide');
//        }, 1500));
//    });
//});
//$(function () {
//    $('#myAlert').on('shown.bs.modal', function () {
//        SetScrollPadding();
//    });
//});





//$(window).load(function () {
//    SetDiscountValueOnProductPicture();
//    $('#myAlert').on('shown.bs.modal', function () {
//        SetScrollPadding();
//    });
//});

//$(window).resize(function () {
//    SetDiscountValueOnProductPicture();
//    $('#myAlert').on('shown.bs.modal', function () {
//        SetScrollPadding();
//    });
//});

//скролл для модальных окон и попавер
//function SetScrollPadding() {

//    var height_document = $(document).height();
//    var height_client = document.body.clientHeight;

//    if (height_document > height_client) {
//        $('body.modal-open, .modal-open .navbar-fixed-top, .modal-open .navbar-fixed-bottom').css({ 'margin-right': 0 });
//    }
//}

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
