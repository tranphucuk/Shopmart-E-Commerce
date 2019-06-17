var product = {
    init: function () {
        this.loadData();
        this.registerEvents();
    },
    registerEvents: function () {
        $('#frm-price-short').on('click', 'input[type="checkbox"]', function () {
            $('input[type="checkbox"]').attr('disabled', 'disabled');
        });

    },

    loadData: function () {
        $.each($('#frm-price-short input[type="checkbox"]'), function (i, item) {
            $(item).removeAttr('disabled');
        });
    },
};
product.init();