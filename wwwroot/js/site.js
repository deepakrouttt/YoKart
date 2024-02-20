$(document).ready(function () {

    $(".sidebar-item > .sidebar-link").click(function (event) {
        event.preventDefault();
        var ul = $(this).next("ul");
        $(".sidebar-item > ul").not(ul).slideUp();
        ul.slideToggle();
    });

    $(".sidebar-link").click(function (event) {
        event.preventDefault();
        var subcategoryId = $(this).data("subcategory-id");
        showProductsForSubcategory(subcategoryId);
    });

    let subcategoryIndex = 1;
    $("#addSubCategories").click(function () {
        $("#subcategories-container").append(`<div class="form-group"><label>Subcategory</label>
        <input name="SubCategories[${subcategoryIndex}].SubCategoryName" class="form-control p-1"/>
        <span class="text-danger small" asp-validation-for="SubCategories[${subcategoryIndex}].SubCategoryName"></span></div>`);
        subcategoryIndex++;
    });



    $('#addExist').click(function () {
        $.ajax({
            url: 'https://localhost:44373/api/CategoryApi/categories',
            type: 'GET',
            success: function (data) {
                var dropdown = $('<select>').addClass('form-control p-1').attr('id', 'CategoryName').attr('name', 'CategoryName');
                dropdown.append($('<option>').text('Choose Here').prop('selected', true).prop('disabled', true));
                $.each(data, function (index, value) {
                    dropdown.append($('<option>').text(value.categoryName).attr('data-category-id', value.categoryId));
                });
                $('#categoryDropdown').html(dropdown);
                $('form').attr('action', '/Category/Exist');
            },
            error: function () {
                // Handle error
            }
        });
    });
    
    $(document).on('change', 'select', function () {
        var selectedCategoryId = $(this).find(':selected').data('category-id');
        $('#ExistId').val(selectedCategoryId);
    });

    $(document).on("change", ".CategoryDropDown", function () {
        var categoryId = $(this).find(':selected').val();
        var subcategoryOption = "<option disabled selected>Choose Here</option>";

        if (categoryId) {
            $.ajax({
                url: 'https://localhost:44373/api/CategoryApi/' + categoryId,
                type: 'GET',
                success: function (data) {
                    data.subCategories.forEach(function (subcategory) {
                        subcategoryOption += `<option value="${subcategory.subCategoryId}">${subcategory.subCategoryName}</option>`;
                    });
                    $(".SubCategoryDropDown").empty().html(subcategoryOption);
                },
                error: function (xhr, status, error) {
                    console.error("An error occurred: " + status + ", " + error);
                }
            });
        }
    });
});

function showProductsForSubcategory(subcategoryId) {

    $.ajax({
        url: "https://localhost:44373/api/ProductApi/GetProduct?subcategoryId=" + subcategoryId,
        method: "GET",
        success: function (data) {
            console.log(data);
            $.each(data, function (index, product) {
                console.log(data);
                $("#product-list").html(`<div class="col-sm-6 col-xl-3"><div class="card overflow-hidden rounded-2"><div class="position-relative"><a href="javascript:void(0)"><img src=` + product.productImage + `class="card-img-top rounded-0" alt="..."></a><a href="javascript:void(0)" class="bg-primary rounded-circle p-2 text-white d-inline-flex position-absolute bottom-0 end-0 mb-n3 me-3" data-bstoggle="tooltip" data-bs-placement="top" data-bs-title="Add To Cart"><i class="ti ti-basket fs-4"></i></a></div><div class="card-body pt-3 p-4"><h6 class="fw-semibold fs-4">` + product.productName + `</h6><div class="d-flex align-items-center justify-content-between"><h6 class="fw-semibold fs-4 mb-0">` + product.productPrice + `</h6></div></div></div></div>`);
            });
        },
        error: function (error) {
            console.log("Error fetching products:", error);
        }
    });
}

function getSubCategories() {
    var categoryId = $(".CategoryDropDown").val();  
    $.ajax({
        type: "GET",
        url: 'https://localhost:44373/api/CategoryApi/' + categoryId,
        success: function (data) {
            $(".SubCategoryDropDown").append('<option value="">--Select SubCategory--</option>');
            data.subCategories.forEach(function (subcategory) {
                subcategoryOption += `<option value="${subcategory.subCategoryId}">${subcategory.subCategoryName}</option>`;
            });
            $(".SubCategoryDropDown").empty().html(subcategoryOption);
        }
    });
}