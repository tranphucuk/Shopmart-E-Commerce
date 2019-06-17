var base = {
    init: function () {
        this.registerEvents();
    },

    registerEvents: function () {
        $('.quick-view-product').on('click', function () {
            var id = $(this).attr('data-id');
            base.displayProductInfo(id);

        });

        $('body').on('click', 'li.sub-item-quick-view', function () {
            var id = $(this).attr('data-id');
            $(this).css({
                'border-bottom': '3px solid orange'
            });
            base.clearCssClick(id);
            base.changeProduct(id);
        });

        $('.see-product-details').on('click', function () {
            window.open('/product-' + base.seoAlias + '-' + base.productId + '.html');
        });
    },

    seoAlias: [],
    productId: [],

    clearCssClick: function (id) {
        $.each($('ul li.sub-item-quick-view'), function (i, item) {
            if ($(item).attr('data-id') != id) {
                $(item).css({
                    'border-bottom': '0px'
                });
            }
        });
    },

    displayProductInfo: function (productId) {
        $.ajax({
            type: 'GET',
            url: '/Product/QuickView',
            data: { productId: productId },
            success: function (res) {
                var render = '';
                var template = $('#sub-items-template').html();
                var mainProduct = res.ProductViewModel;
                var relatedProducts = res.RelatedProducts;
                base.productId = mainProduct.Id;
                base.seoAlias = mainProduct.SeoAlias;

                $('.quick-view-name').text(mainProduct.Name);
                var img = '<img src="' + mainProduct.Image + '" alt="' + mainProduct.Name + '">'
                $('.quick-view-img').html(img);
                if (mainProduct.PromotionPrice != null) {
                    $('#sale-price').html('$' + mainProduct.PromotionPrice);
                    $('#price-before').html('$' + mainProduct.Price);
                    $('#regular-price').html('');
                } else {
                    $('#sale-price').html('');
                    $('#price-before').html('');
                    $('#regular-price').html('$' + mainProduct.Price);
                }
                $('.quick-view-description').html(mainProduct.Description.substring(1, 200));

                $.each(relatedProducts, function (i, item) {
                    var obj = {};
                    obj.Id = item.Id;
                    obj.Path = item.Image;
                    obj.Name = item.Name;

                    render += Mustache.render(template, obj);
                });
                $('#sub-item-content').html(render);
                $('#quick-view-modal').modal('show');
            }
        });
    },

    changeProduct: function (id) {
        $.ajax({
            type: 'GET',
            url: '/Product/ChangeProduct',
            data: { productId: id },
            success: function (res) {
                base.productId = res.Id;
                base.seoAlias = res.SeoAlias;

                $('.quick-view-name').text(res.Name);
                var img = '<img src="' + res.Image + '" alt="' + res.Name + '">'
                $('.quick-view-img').html(img);
                if (res.PromotionPrice != null) {
                    $('#sale-price').html('$' + res.PromotionPrice);
                    $('#price-before').html('$' + res.Price);
                    $('#regular-price').html('');
                } else {
                    $('#sale-price').html('');
                    $('#price-before').html('');
                    $('#regular-price').html('$' + res.Price);
                }
                $('.quick-view-description').html(res.Description.substring(1, 200));
            }
        });
    },
};
base.init();