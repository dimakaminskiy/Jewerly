// modalform.js

$(function () {
    $.ajaxSetup({ cache: false });

    $("#ModalBtn a[data-toggle]").on("click", function (e) {
        // hide dropdown if any (this is used wehen invoking modal from link in bootstrap dropdown )
        //$(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');
        $('#myModalContent').load(this.href, function () {
            $('#myModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');
            bindForm(this);
        });
        return false;
    });
});


function bindForm(dialog) {
    $('form', dialog).submit(function () {
        //$("form button[type='submit']").click(function() {
        //    var form = $(this).parent("form");
        //    alert("form"+form);
        //    var errorForm = form.children(".input-validation-error");
        //    alert("errorForm"+errorForm);
        //    var error = errorForm.size();
        //    alert(error);
        //});
        var error = $(".input-validation-error").size();

        //alert("99");
            if (error === 0) {
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function(result) {
                       if (result.success) {
                            $('#myModal').modal('hide');
                           // location.reload();
                           //alert(result.url);
                           location.replace(result.url);
                       } else {
                            $('#myModalContent').html(result);
                            bindForm(dialog);
                        }
                    }
                });
            }
            return false;
        });   
}
