$('.button-minus, .button-plus').on('click', function () {
    var qualityField = $(this).closest('.input-group').find('.quantity-field');
    var currentQuantity = parseInt(qualityField.val());
    var productId = parseInt($(this).parent().find("#quantity").attr("data-productId"));

    if ($(this).hasClass('button-minus')) {
        if (currentQuantity > 1) {
            var updatedQuantity = 0;
            if (currentQuantity == 1) {
                updatedQuantity = 1;
            }
            else {
                updatedQuantity = currentQuantity - 1;
            }
            qualityField.val(updatedQuantity);
            var unitPrice = parseFloat($(this).closest('tr').find('.UnitPrice').text());
            var totalPrice = updatedQuantity * unitPrice;
            $(this).closest('tr').find('.itemPrice').text(totalPrice.toFixed(2));
            quantityUpdate(productId,updatedQuantity);
        }
    } else {
        var updatedQuantity = currentQuantity + 1;
        qualityField.val(updatedQuantity);
        var unitPrice = parseFloat($(this).closest('tr').find('.UnitPrice').text());
        var totalPrice = updatedQuantity * unitPrice;
        $(this).closest('tr').find('.itemPrice').text(totalPrice.toFixed(2));
        quantityUpdate(productId, updatedQuantity);     
    }
    var grandTotal = 0;
    $('.itemPrice').each(function () {
        var quantity = parseInt($(this).siblings("#quantity").val());
        var unitPrice = parseFloat($(this).parent().closest("td").prev().text());
        /*  var itemPrice = parseFloat($(this).closest('tr').find('#itemPrice').text());*/
        grandTotal += parseFloat($(this).text());
    });
    $('.TotalPrice').text(grandTotal.toFixed(2));

});

$(function () {
    $("#checkOut").on("click", function (e) {
        e.preventDefault();

        Swal.fire({
            title: "Proceed with Checkout?",
            text: "Your order will be finalized and a confirmation email will be sent.",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Yes, continue",
            cancelButtonText: "Cancel",
            reverseButtons: true
        }).then((result) => {

            if (result.isConfirmed) {

                Swal.fire({
                    title: "Processing...",
                    text: "Please wait while we finalize your order.",
                    allowOutsideClick: false,
                    didOpen: () => Swal.showLoading()
                });

                $.ajax({
                    url: "/Cart/CheckOut",
                    type: "POST",
                    success: function (response) {

                        Swal.close();

                        if (response.status) {
                            Swal.fire({
                                title: "Order Placed Successfully!",
                                text: "Redirecting to your cart summary...",
                                icon: "success",
                                timer: 1500,
                                showConfirmButton: false
                            });

                            setTimeout(() => {
                                window.location.href = response.redirect;
                            }, 1500);
                        }
                        else {
                            Swal.fire({
                                title: "Login Required",
                                text: "You must be logged in to place an order.",
                                icon: "warning"
                            }).then(() => {
                                window.location.href = response.redirect;
                            });
                        }
                    },
                    error: function () {
                        Swal.fire({
                            title: "Error",
                            text: "Checkout failed. Please try again.",
                            icon: "error"
                        });
                    }
                });
            }

        });

    });
});
