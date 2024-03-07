$('.button-minus, .button-plus').on('click', function () {
    var qualityField = $(this).closest('.input-group').find('.quantity-field');
    var currentQuantity = parseInt(qualityField.val());

    if ($(this).hasClass('button-minus')) {
        if (currentQuantity > 1) {
            qualityField.val(currentQuantity - 1);
            var unitPrice = parseFloat($(this).closest('tr').find('.UnitPrice').text());
            var totalPrice = (currentQuantity - 1) * unitPrice;
            $('#itemPrice').text(totalPrice.toFixed(2));

        }
    } else {
        qualityField.val(currentQuantity + 1);
        var unitPrice = parseFloat($(this).closest('tr').find('.UnitPrice').text());
        var totalPrice = (currentQuantity + 1) * unitPrice;
        $('#itemPrice').text(totalPrice.toFixed(2));
    }
});