var cart = {
    init: function () {
        $.when(
            cart.loadColors(),
            cart.loadSize(),
        ).then(function () {
            cart.loadData();
            cart.registerEvents();
        });
    },

    registerEvents: function () {
        $('body').on('click', '.btn-minus-quantity', function () {
            var input = $(this).closest('.qty').find('input.txt-quantity');
            if ($(input).val() >= 2) {
                $(input).val($(input).val() - 1);
            }
        });

        $('body').on('click', '.btn-plus-quantity', function () {
            var input = $(this).closest('.qty').find('input.txt-quantity');
            $(input).val(parseInt($(input).val()) + 1);
        });

        $('body').on('mouseout', '.btn-plus-quantity', function () {
            var quantity = $(this).closest('.qty').find('input.txt-quantity').val();
            var colorId = $(this).closest('tr').find('.select-color-list').val();
            var sizeId = $(this).closest('tr').find('.select-size-list').val();
            var productId = $(this).closest('tr').attr('data-id');

            var productPrice = $(this).closest('tr').find('span.single-price').html();
            var oldTotalPrice = $(this).closest('tr').find('span.total-price').html();
            var newTotalPrice = productPrice * quantity;
            $(this).closest('tr').find('span.total-price').html(newTotalPrice);
            $.ajax({
                type: 'GET',
                url: '/Cart/UpdateCart',
                data: {
                    productId: productId,
                    quantity: quantity,
                    colorId: colorId,
                    sizeId: sizeId
                },
                success: function (res) {
                    var oldBill = parseFloat($('#total-bill').html());
                    $('#total-bill').html(oldBill + (newTotalPrice - oldTotalPrice));
                }
            })
        });

        $('body').on('mouseout', '.btn-minus-quantity', function () {
            var quantity = $(this).closest('.qty').find('input.txt-quantity').val();
            var colorId = $(this).closest('tr').find('.select-color-list').val();
            var sizeId = $(this).closest('tr').find('.select-size-list').val();
            var productId = $(this).closest('tr').attr('data-id');

            var productPrice = $(this).closest('tr').find('span.single-price').html();
            var oldTotalPrice = $(this).closest('tr').find('span.total-price').html();
            var newTotalPrice = productPrice * quantity;
            $(this).closest('tr').find('span.total-price').html(newTotalPrice);
            $.ajax({
                type: 'GET',
                url: '/Cart/UpdateCart',
                data: {
                    productId: productId,
                    quantity: quantity,
                    colorId: colorId,
                    sizeId: sizeId
                },
                success: function (res) {
                    var oldBill = parseFloat($('#total-bill').html());
                    $('#total-bill').html(oldBill - (oldTotalPrice - newTotalPrice));
                }
            })
        });

        $('body').on('change', '.select-color-list', function () {
            var colorId = $(this).val();
            var quantity = $(this).closest('tr').find('input.txt-quantity').val();
            var sizeId = $(this).closest('tr').find('.select-size-list').val();
            var productId = $(this).closest('tr').attr('data-id');
            $(this).closest('tr').find('span.total-price').html();

            $.ajax({
                type: 'GET',
                url: '/Cart/UpdateCart',
                data: {
                    productId: productId,
                    quantity: quantity,
                    colorId: colorId,
                    sizeId: sizeId
                },
                success: function (res) {
                }
            });
        });

        $('body').on('change', '.select-size-list', function () {
            var sizeId = $(this).val();
            var quantity = $(this).closest('tr').find('input.txt-quantity').val();
            var colorId = $(this).closest('tr').find('.select-color-list').val();
            var productId = $(this).closest('tr').attr('data-id');
            $(this).closest('tr').find('span.total-price').html();

            $.ajax({
                type: 'GET',
                url: '/Cart/UpdateCart',
                data: {
                    productId: productId,
                    quantity: quantity,
                    colorId: colorId,
                    sizeId: sizeId
                },
                success: function (res) {
                }
            });
        });

        $('body').on('click', '#remove-cart-item', function (e) {
            e.preventDefault();
            var productId = $(this).closest('tr').data('id');
            $.ajax({
                type: 'GET',
                url: '/Cart/RemoveFromCart',
                data: { productId: productId },
                success: function (res) {
                    cart.loadData();
                }
            });
        });
    },

    loadData: function () {
        $.ajax({
            type: 'GET',
            url: '/Cart/GetCart',
            success: function (res) {
                var render = '';
                var template = $('#tbl-cart-template').html();
                var totalBill = 0;
                $.each(res, function (i, item) {
                    var product = item.Product;
                    var obj = {};
                    obj.Id = product.Id;
                    obj.Url = '/product-' + product.SeoAlias + '-' + product.Id + '.html'
                    obj.Path = product.Image;
                    obj.Name = product.Name;
                    obj.Price = item.Price;
                    obj.Value = item.Quantity;
                    obj.TotalPrice = (item.Quantity * item.Price).toFixed(2);
                    totalBill += parseFloat(obj.TotalPrice);
                    obj.Color = cart.setSelectColor(item.ColorId);
                    obj.Size = cart.setSelectSize(item.SizeId);
                    render += Mustache.render(template, obj);
                });
                if (res.length == 0) {
                    $('.checkout-btn').removeAttr("href");
                    $('.checkout-btn').attr("title", '0 item available for checkout');
                } else {
                    $('.checkout-btn').removeAttr("title");
                    $('.checkout-btn').attr("href", '/checkout.html');
                }
                $('#total-bill').text(totalBill);
                $('#tbl-cart-content').html(render);

                $('#header-info').load('/ReloadHeader/HeaderDetail');
            }
        });
    },

    setSelectColor: function (colorId) {
        var select = '<select class="select-color-list">';
        $.each(cart.colorData, function (i, item) {
            if (colorId != null && item.Id == colorId) {
                select += '<option value="' + colorId + '" selected>' + item.Name + '</option>';
            } else {
                select += '<option value="' + item.Id + '">' + item.Name + '</option>';
            }
        });
        select += '</select>';
        return select;
    },

    setSelectSize: function (sizeId) {
        var select = '<select class="select-size-list">';
        $.each(cart.sizeData, function (i, item) {
            if (sizeId != null && item.Id == sizeId) {
                select += '<option value="' + sizeId + '" selected>' + item.Name + '</option>';
            } else {
                select += '<option value="' + item.Id + '">' + item.Name + '</option>';
            }
        });
        select += '</select>';
        return select;
    },

    loadColors: function () {
        var opt = [];
        return $.ajax({
            type: 'GET',
            url: '/Cart/LoadColor',
            success: function (res) {
                cart.colorData = res;
            }
        });
    },

    loadSize: function () {
        var opt = [];
        return $.ajax({
            type: 'GET',
            url: '/Cart/LoadSize',
            success: function (res) {
                cart.sizeData = res;
            },
        });
    },

    colorData: [],
    sizeData: [],
};
cart.init();