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

