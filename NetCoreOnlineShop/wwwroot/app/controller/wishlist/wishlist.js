var wishlist = {
    init: function () {
        this.loadData();
        this.registerEvents();
    },

    registerEvents: function () {
        $('#ddlShowPage').on('change', function () {
            common.config.pageSize = $('#ddlShowPage :selected').text();
            common.config.pageIndex = 1;
            $('#pagination-wishlist').twbsPagination('destroy');
            wishlist.loadData();
        });

        $('#btn-template-options').on('click', function () {
            $('#import-export-email').modal('show');
        });

        $('#txt-product-keyword').on('keypress', function (e) {
            if (e.which === 13) {
                $('#btn-search-product').click();
            }
        });

        $('#btn-search-product').off('click').on('click', function () {
            $('#pagination-wishlist').twbsPagination('destroy');
            $('#ddlShowPage').prop('selectedIndex', 0);
            common.config.pageIndex = 1;
            wishlist.loadData();
        });

        $('#btn-import-template').off('click').on('click', function () {
            $('#file-template').val('');
            $('#file-template').click();
        });

        $('#file-template').on('change', function () {
            var html = $("#file-template")[0].files[0];
            var fd = new FormData();
            fd.append('wishlistTemplate.html', html);
            $.ajax({
                type: 'POST',
                url: '/Admin/Wishlist/ImportTemplate',
                data: fd,
                processData: false,
                contentType: false,
                success: function (res) {
                    if (res == true) {
                        common.notify("Imported email template success", 'success');
                        $('#import-export-email').modal('hide');
                    } else {
                        common.notify("Imported email template success", 'error');
                    }
                }
            })
        });

        $('body').on('click', '.btn-send-email', function () {
            var id = $(this).attr('data-id');
            common.confirm("<b>Are you sure to send this email?</b></br></br>***Send a reminder email to customers who added this product to their wishlist.", function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Wishlist/SendEmail',
                    data: { productId: id },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        if (res == true) {
                            common.notify('Sent email succeeded', 'success');
                            common.stopLoadingIndicator();
                        } else {
                            common.notify('Sent email failed', 'error');
                            common.stopLoadingIndicator();
                        }
                    }
                });
            });
        });
    },

    loadData: function () {
        var render = '';
        var template = $('#tbl-wishlist-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Wishlist/GetListProduct',
            data: {
                keyword: $('#txt-product-keyword').val(),
                page: common.config.pageIndex,
                pageSize: common.config.pageSize
            },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res.Results, function (i, item) {
                    var product = {};
                    product.Id = item.Product.Id;
                    product.Name = item.Product.Name;
                    product.Total = item.Total;

                    render += Mustache.render(template, product);
                });
                common.stopLoadingIndicator();
                $('#tbl-wishlist-content').html(render);
                $('#lbl-total-product').text(res.RowCount);
                if (res.RowCount > 0) {
                    wishlist.pagination(res.RowCount);
                }
            }
        });
    },

    pagination: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-wishlist').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    wishlist.loadData();
                }
            }
        });
    },
};
wishlist.init();