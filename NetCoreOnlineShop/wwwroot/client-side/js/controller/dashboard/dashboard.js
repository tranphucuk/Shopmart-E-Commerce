var dashboard = {
    init: function () {
        this.registerEvents();
    },

    registerEvents: function () {
        $('body').on('click', '.btn-delete-product', function (e) {
            var id = $(this).data('id');
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: '/Dashboard/RemoveProduct',
                data: { productId: id },
                success: function (res) {
                    location.reload();
                }
            });
        });
    },
};
dashboard.init();