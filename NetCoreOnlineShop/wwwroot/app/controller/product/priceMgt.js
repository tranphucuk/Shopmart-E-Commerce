var price = {
    init: function () {
        this.registerEvents();
    },
    registerEvents: function () {
        $("#frm-product-Price").validate({
            errorClass: 'red',
            rules: {
                txtQuantityFrom: {
                    number: true,
                    min: 1
                },
                txtQuantityTo: {
                    number: true,
                    min: 2
                },
                txtPrice: {
                    number: true,
                    min: 1
                },
            }
        });

        $('#add-new-price').off('click').on('click', function () {
            var template = $('#template-price-product-management').html();
            var render = Mustache.render(template, {
                From: 1,
                To: 2,
                Price: 1
            });
            if (render != '') {
                $('#tbl-price-content').append(render);
            }
        });

        $('body').on('click', '.btn-remove-price', function () {
            $(this).closest('tr').remove();
        });

        $('#btn-save-price').off('click').on('click', function () {
            var validator = $("#frm-product-Price").validate();
            var productId = $('#productId').val();
            if (validator.form()) {
                var listPrice = [];
                $('#tbl-price-content').find('tr').each(function (i, item) {
                    var row = {};
                    row.Id = $(item).attr('data-id');
                    row.ProductId = productId;
                    row.FromQuantity = $(item).find('.txt-from-quantity').val();
                    row.ToQuantity = $(item).find('.txt-to-quantity').val();
                    row.Price = $(item).find('.txtPrice').val();
                    listPrice.push(row);
                });

                $.ajax({
                    type: 'POST',
                    url: '/Admin/Product/SaveProductPrice',
                    data: {
                        wholePriceViewModels: listPrice,
                        productId: productId
                    },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        common.notify('Save success', 'success');
                        common.stopLoadingIndicator();
                        $('#product-price-manager').modal('hide');
                    },
                    error: function (err) {
                        console.log(err);
                        common.notify('Error: ' + err, 'error');
                        common.stopLoadingIndicator();
                    }
                });
            }
        });
    },

    loadData: function (id) {
        $('#product-price-manager').modal('show');
        var render = '';
        var template = $('#template-price-product-management').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Product/GetPrice',
            data: { productId: id },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        From: item.FromQuantity,
                        To: item.ToQuantity,
                        Price: item.Price,
                    });
                });
                if (render != null) {
                    $('#tbl-price-content').html(render);
                } else {
                    $('#tbl-price-content').empty();
                }
                common.stopLoadingIndicator();
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
                common.stopLoadingIndicator();
            }
        });
    }
};
price.init();