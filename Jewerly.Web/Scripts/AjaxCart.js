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
            $(AjaxCart.cartcountitems).html("("+response.cartcountitems+")").css('opacity', 0).animate({opacity: 1});
        }
        if (response.cartitems) { // обновление списка покупок
            $(AjaxCart.cartitems).replaceWith(response.cartitems).css('opacity', 0).animate({ opacity: 1 });
        }
        if (response.carttotalprice) { // общая цена
            $(AjaxCart.carttotalprice).html(response.carttotalprice).css('opacity', 0).animate({ opacity: 1 });
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
        $("#myAlert .modal-body").text("Не удалось добавить продукт. Пожалуйста, обновите страницу и попробуйте еще раз.");
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
     $("#myAlert .modal-body").text(message);
 
}

