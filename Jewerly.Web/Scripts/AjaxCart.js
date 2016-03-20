var AjaxCart = {
    loadWaiting: false,
    cartcountitems: '', //кол-во товаров к корзине
    carttotalprice: '', // общая цена
    cartitems:'', // список покупок
   
    init: function (cartcountitems, carttotalprice, cartitems) {
        this.loadWaiting = false;
        this.cartcountitems = cartcountitems;
        this.carttotalprice = carttotalprice;
        this.cartitems = cartitems;
    },

    setLoadWaiting: function (display) {
        displayAjaxLoading(display);
        this.loadWaiting = display;
    },


    addproducttocart_catalog: function (urladd) {
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);

        $.ajax({
            cache: false,
            url: urladd,
            type: 'post',
            success: this.success_process,
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },


    addproducttocart_details: function (urladd, formselector) {
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);

        $.ajax({
            cache: false,
            url: urladd,
            data: $(formselector).serialize(),
            type: 'post',
            success: this.success_process,
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    success_process: function (response) {


       


        if (response.cartcountitems) { // обновить кол-во товара в корзине
            $(AjaxCart.cartcountitems).html("("+response.cartcountitems+")").css('opacity', 0).animate({opacity: 1});;
        }
        if (response.cartitems) { // обновление списка покупок
            $(AjaxCart.cartitems).replaceWith(response.cartitems);
        }
        if (response.carttotalprice) { // общая цена
            $(AjaxCart.carttotalprice).html(response.carttotalprice);
        }

        if (response.message) {
            //display notification
            if (response.success == true) {
                //success

                    //specify timeout for success messages
                    displayBarNotification(response.message, 'success', 3500);
            }
            else {
                    //no timeout for errors
                    displayBarNotification(response.message, 'error', 0);
            }
            return false;
        }
        if (response.redirect) {
            location.href = response.redirect;
            return true;
        }
        return false;
    },

   resetLoadWaiting: function () {
        AjaxCart.setLoadWaiting(false);
    },

    ajaxFailure: function () {
        alert("Не удалось добавить продукт. Пожалуйста, обновите страницу и попробуйте еще раз.");
    }
};

function displayAjaxLoading(display) {
    if (display) {
        $('.ajax-loading-block-window').show();
    }
    else {
        $('.ajax-loading-block-window').hide('slow');
    }
}


var barNotificationTimeout;
function displayBarNotification(message, messagetype, timeout) {
    clearTimeout(barNotificationTimeout);

    //types: success, error
    var cssclass = 'success';
    if (messagetype == 'success') {
        cssclass = 'success';
    }
    else if (messagetype == 'error') {
        cssclass = 'error';
    }
    //remove previous CSS classes and notifications
    $('#bar-notification')
        .removeClass('success')
        .removeClass('error');
    $('#bar-notification .content').remove();

    //we do not encode displayed message

    //add new notifications
    var htmlcode = '';
    if ((typeof message) == 'string') {
        htmlcode = '<p class="content">' + message + '</p>';
    } else {
        for (var i = 0; i < message.length; i++) {
            htmlcode = htmlcode + '<p class="content">' + message[i] + '</p>';
        }
    }
    $('#bar-notification').append(htmlcode)
        .addClass(cssclass)
        .fadeIn('slow')
        .mouseenter(function () {
            clearTimeout(barNotificationTimeout);
        });

    $('#bar-notification .close').unbind('click').click(function () {
        $('#bar-notification').fadeOut('slow');
    });

    //timeout (if set)
    if (timeout > 0) {
        barNotificationTimeout = setTimeout(function () {
            $('#bar-notification').fadeOut('slow');
        }, timeout);
    }
}


