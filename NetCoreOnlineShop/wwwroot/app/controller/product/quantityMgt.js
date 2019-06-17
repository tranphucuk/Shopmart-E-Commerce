var quantity = {
    init: function () {
        $.when(
            quantity.loadColorToDropDown(),
            quantity.loadSizeToDropDown()
        ).then(function () {
            quantity.registerEvents();
        });
    },

    registerEvents: function () {
        $('#add-new-quantity').on('click', function () {
            var template = $('#template-quantity-product-management').html();
            var render = Mustache.render(template, {
                Color: quantity.colorData,
                Size: quantity.sizeData,
                Quantity: 0,
            });
            $('#tbl-body-content').append(render);
        });

        $('#tbl-body-content').on('click', '.btn-remove-quantity', function () {
            $(this).closest('tr').remove();
        });

        $('#btn-save-quantity').off('click').on('click', function () {
            var id = $('#productId').val();
            var quantities = [];
            $('#tbl-body-content tr').each(function (i, item) {
                var quantity = {};
                quantity.Id = $(item).attr('data-id');
                quantity.ProductId = id;
                quantity.ColorId = $(item).find('.ddlColor').val();
                quantity.SizeId = $(item).find('.ddlSize').val();
                quantity.Quantity = $(item).find('.txtQuantity').val();
                quantities.push(quantity);
            });
            $.ajax({
                type: 'POST',
                url: '/Admin/Product/SaveQuantity',
                data: {
                    productId: id,
                    productQuantityVms: quantities
                },
                success: function (res) {
                    if (res.Message == 'add') {
                        common.notify('Add success', 'success');
                    } else {
                        common.notify('Update success', 'success');
                    }
                    $('#product-quantity-manager').modal('hide');
                    product.loadData(true);
                },
                error: function (err) {
                    console.log(err);
                    common.notify('Error: ' + err, 'error');
                }
            });
        });
    },

    colorData: [],
    sizeData: [],

    loadData: function (id) {
        $('#product-quantity-manager').modal('show');
        var render = '';
        var template = $('#template-quantity-product-management').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Product/GetQuantity',
            data: { productId: id },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res, function (i, item) {
                    $.when(
                        quantity.loadColorToDropDown(item.ColorId),
                        quantity.loadSizeToDropDown(item.SizeId),
                    ).then(function () {
                        render += Mustache.render(template, {
                            Id: item.Id,
                            Color: quantity.colorData,
                            Size: quantity.sizeData,
                            Quantity: item.Quantity,
                        });
                    });
                });
                if (render != null) {
                    $('#tbl-body-content').html(render);
                } else {
                    $('#tbl-body-content').empty();
                }
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
            }
        });
        common.stopLoadingIndicator();
    },

    loadColorToDropDown: function (ColorId) {
        $.ajax({
            async: false,
            type: 'GET',
            url: '/Admin/Bill/GetAllColor',
            success: function (res) {
                var select = '<select class="form-control ddlColor">';
                $.each(res, function (i, item) {
                    if (item.Id == ColorId) {
                        select += '<option value="' + item.Id + '" selected="selected">' + item.Name + '</option>';
                    } else {
                        select += '<option value="' + item.Id + '">' + item.Name + '</option>';
                    }
                });
                quantity.colorData = select + '</select>';
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
            }
        });
    },

    loadSizeToDropDown: function (sizeId) {
        $.ajax({
            async: false,
            type: 'GET',
            url: '/Admin/Bill/GetAllSize',
            success: function (res) {
                var select = '<select class="form-control ddlSize">';
                $.each(res, function (i, item) {
                    if (sizeId == item.Id) {
                        select += '<option value="' + item.Id + '" selected="selected">' + item.Name + '</option>';
                    }
                    else {
                        select += '<option value="' + item.Id + '">' + item.Name + '</option>';
                    }
                });
                quantity.sizeData = select + '</select>';
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
            }
        });
    }
};
quantity.init();