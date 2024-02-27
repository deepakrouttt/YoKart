$(document).ready(function () {
    PagerEvent();
    $(".sidebar-item > div > .sidebar").click(function (event) {
        event.preventDefault();
        if ($(location).attr('href') != "https://localhost:44387/Home/Index") {
            window.location.href = "https://localhost:44387/Home/Index";
        }
        else {
            var ul = $(this).next("ul");
            $(".sidebar-item > div > ul").not(ul).slideUp();
            ul.slideToggle();
        };
    });

    $(".subcat").click(function (event) {
        event.preventDefault();
        var subcategoryId = $(this).data("subcategory-id");
        showProductsForSubcategory(subcategoryId);
    });

    //Add Subcategory button for add Subcategory input
    let subcategoryIndex = 1;
    $("#addSubCategories").click(function () {
        $("#subcategories-container").append(`<div class="form-group"><label>Subcategory</label>
        <input name="SubCategories[${subcategoryIndex}].SubCategoryName" class="form-control p-1"/>
        <span class="text-danger small" asp-validation-for="SubCategories[${subcategoryIndex}].SubCategoryName"></span></div>`);
        subcategoryIndex++;
    });

    //click on Exist when Dynamic Subcategory Display
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

    $(document).on('change', '#CategoryName', function () {
        debugger;
        var selectedCategoryId = $(this).find(':selected').data('category-id');
        $('#ExistId').val(selectedCategoryId);
    });
    //Dynamic Subcategory Create
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

    //Price Filter
    $(document).on('change', '#priceRange', function () {
        var page = $(this).data('page');
        var LowRange = $(this).find(':selected').data('low');
        var HighRange = $(this).find(':selected').data('high');
        $.ajax({
            url: 'Index_Partial',
            type: 'GET',
            data: { page: page, LowRange: LowRange, HighRange: HighRange },
            success: function (data) {
                $('#partial-container').html(data);
                pagerCreate();
            }
        });
    });
});
//Show Product By there Subcategory Name
function showProductsForSubcategory(subcategoryId) {
    $("#product-list").empty();
    $.ajax({
        url: "https://localhost:44373/api/ProductApi/GetProductsForSubcategory?subcategoryId=" + subcategoryId,
        method: "GET",
        success: function (data) {
            $.each(data, function (index, product) {
                $("#product-list").append(`<div class="col-sm-6 col-xl-3" ><div class="card overflow-hidden rounded-2">
                <div class="position-relative text-center"><a href="javascript:void(0)"><img src=/images/products/` + product.productImage + ` ` +
                    `class="card-img-top rounded-0 p-1" alt="..." style="width: 80% !important;"></a><a href="javascript:void(0)" class="bg-primary rounded-circle p-2 text-white d-inline-flex position-absolute bottom-0 end-0 mb-n3 me-3"
                    data-bstoggle="tooltip" data-bs-placement="top" data-bs-title="Add To Cart"><i class="ti ti-basket fs-4">
                    </i></a></div><div class="card-body"><h6 class="fw-semibold fs-4">` + product.productName +
                    `</h6><div class="d-flex align-items-center justify-content-between"><h6 class="fw-semibold fs-4 mb-0">`
                    + product.productPrice + ` &#8377;</h6></div></div></div></div>`);
            });
        },
        error: function (error) {
            console.log("Error fetching products:", error);
        }
    });
}
//Pager link for paging 
function PagerEvent() {
    $('.pager-link').click(function (e) {
        e.preventDefault();
        debugger;
        var page = $(this).data('page');
        var LowRange = $("#priceRange").find(':selected').data('low');
        var HighRange = $("#priceRange").find(':selected').data('high');
        $.ajax({
            url: 'Index_Partial',
            type: 'GET',
            data: { page: page, LowRange: LowRange, HighRange: HighRange },
            success: function (data) {
                $('#partial-container').html(data);
            },
            error: function (error) {
                $("body").html(error);
            }
        });
    });
}

//Edit when I select Category then dynamic Subcategory display
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
    //$('input[type=file]')[0].files[0].name
}
//Pager creation function
function pagerCreate() {
    var html = "";
    var pageCount = $("#pageCount").val();
    var page = $(this).data('page');
    for (i = 1; i <= pageCount; i++) {
        html += `<a href="#" data-page="${i}" class="pager-link  ${i == page ? "active" : ""} border btn p-1 m-1">${i}</a>`;
    }
    $("#pager-container").html(html);
    PagerEvent();//paging function calling if we create a pager dynamic dynamic binding
}