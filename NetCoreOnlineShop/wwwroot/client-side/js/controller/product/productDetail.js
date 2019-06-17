var detail = {
    init: function () {
        this.loadData();
        this.registerEvents();
    },

    registerEvents: function () {
        $('#btn-add-to-cart').on('click', function (e) {
            var id = $(this).attr('data-id');
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: '/Cart/AddToCart',
                data: {
                    productId: id,
                    quantity: $('#txt-quantity').val(),
                    colorId: detail.colorId,
                    sizeId: detail.sizeId
                },
                success: function (res) {
                    $('#frm-popup-basket').modal('show');

                    $('.basket-header-detail').html(res.Quantity + ' item added to basket');
                    var img = '<img src="' + res.Product.Image + '" alt="' + res.Product.Name + '">';
                    $('.basket-img').html(img);
                    $('.basket-product-name').html(res.Product.Name);
                    $('.basket-product-color').html('Exact Colour: ' + res.Color.Name);
                    $('.basket-product-size').html('Exact Size: ' + res.Size.Name);
                    if (res.Product.PromotionPrice != null) {
                        $('.basket-product-price').html('$' + res.Product.PromotionPrice + ' <span class="bask-price-style">($' + res.Product.Price + ')</span>');
                    } else {
                        $('.basket-product-price').html('$' + res.Product.Price);
                    }
                }
            });
        });

        $('ul.button-colors input[type="button"]').on('click', function () {
            $(this).css({
                'width': '29px',
                'height': '27px',
            });
            detail.colorId = $(this).attr('data-id');
            detail.clearCssColorButton($(this).attr('data-id'));
        });

        $('ul.button-size input[type="button"]').on('click', function () {
            $(this).css({
                'background-color': 'lightgray',
            });
            detail.sizeId = $(this).attr('data-id');
            detail.clearCssSizeButton($(this).attr('data-id'));
        });

        $('#btn-go-cart').on('click', function () {
            window.location.href = '/my-cart.html';
        });

        $('#btn-checkout').on('click', function () {
            window.location.href = '/checkout.html';
        });

        $('#frm-popup-basket').on('hidden.bs.modal', function () {
            $('#header-info').load('/ReloadHeader/HeaderDetail');
        })

        $('#btn-add-wishlist').on('click', function (e) {
            e.preventDefault();
            var productId = $(this).data('id');
            var url = $(this).data('url');
            $.ajax({
                type: 'POST',
                url: '/Product/AddToWishlist',
                data: {
                    productId: productId,
                    returnUrl: url
                },
                success: function (res) {
                    if (res == true) {
                        location.reload();
                    } else {
                        window.location.href = "/login.html";
                    }
                }
            });
        });
    },
    colorId: [],
    sizeId: [],

    loadData: function () {
        detail.colorId = $('ul.button-colors input[type="button"]').first().data('id');
        $('ul.button-colors input[type="button"]').first().css({
            'width': '29px',
            'height': '27px',
        });

        detail.sizeId = $('ul.button-size input[type="button"]').first().data('id');
        $('ul.button-size input[type="button"]').first().css({
            'background-color': 'lightgray',
        });
    },

    clearCssColorButton: function (dataId) {
        $.each($('ul.button-colors input[type="button"]'), function (i, item) {
            if ($(item).attr('data-id') != dataId) {
                $(item).css({
                    'width': '22px',
                    'height': '22px',
                });
            }
        });
    },

    clearCssSizeButton: function (dataId) {
        $.each($('ul.button-size input[type="button"]'), function (i, item) {
            if ($(item).attr('data-id') != dataId) {
                $(item).css({
                    'background-color': 'white',
                });
            }
        });
    },
};
detail.init();