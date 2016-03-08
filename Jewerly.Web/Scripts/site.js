﻿
    //корзина
    $(function (e) {
        $('[data-toggle="popover"].cart').popover({
            html: true,
            content: function () {
                return $('#popover_content_wrapper').html();
            }
        });
    });

//валюта
$(function (e) {
    $('[data-toggle="popover"].exchange').popover({
        html: true,
        content: function () {
            return $('#popover_content_exchange').html();
        }
    });
});

   
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

//счетчик товаров
$('.product-info button').click(function () {
    var countT = $(".countProduct").text();
    var countN = parseFloat(countT);
    var number = countN + 1;
    $(".countProduct").text(number);
    $('.countProduct').css('opacity', 0).animate({
        opacity: 1
    });
});

$(function () {
    $('#myAlert').on('show.bs.modal', function () {
        var myAlert = $(this);
        clearTimeout(myAlert.data('hideInterval'));
        myAlert.data('hideInterval', setTimeout(function () {
            myAlert.modal('hide');
        }, 1500));
    });
});
$(function () {
    $('#myAlert').on('shown.bs.modal', function () {
        SetScrollPadding();
    });
});




//скидка
$(window).load(function () {
    SetDiscountValueOnProductPicture();
    $('#myAlert').on('shown.bs.modal', function () {
        SetScrollPadding();
    });
});

$(window).resize(function () {
    SetDiscountValueOnProductPicture();
    $('#myAlert').on('shown.bs.modal', function () {
        SetScrollPadding();
    });
});

function SetDiscountValueOnProductPicture() {
    var mg = $(".product-image").width();
    var mgr = $(".product-image img").width();
    var result = (mg - mgr) / 2;
    $(".discount-img").css("right", result);
    $(".discount-nm").css("right", result);
}

//скролл для модальных окон
function SetScrollPadding() {

    var height_document = $(document).height();
    var height_client = document.body.clientHeight;

    if (height_document > height_client) {
        $('body.modal-open, .modal-open .navbar-fixed-top, .modal-open .navbar-fixed-bottom').css({ 'margin-right': 0 });
    }
}

//dropdown center

$(document).ready(function () {
    $("button.dropdown-toggle").click(function (ev) {
       var t = $(this);
       var g = t.next(".dropdown-menu");
       var tPadding = t.css('padding-left');
       var tPaddingNumber = parseFloat(tPadding);
       var width = t.width();
       var twidth = width + tPaddingNumber*2;
       var gwidth = g.width();
         var result = ((twidth - gwidth) / 2) + "px";
            g.css("left", result);
    });
});
