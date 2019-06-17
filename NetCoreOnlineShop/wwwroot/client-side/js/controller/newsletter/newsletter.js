var newsletter = {
    init: function () {
        this.registerEvents();
    },

    registerEvents: function () {
        $('#btn-subscribe').off('click').on('click', function () {
            var sub = {};
            sub.Email = $('#txt-email-sub').val();

            $.ajax({
                type: 'POST',
                url: '/NewsLetter/SubscribeEmail',
                data: {
                    subscription: sub,
                },
                success: function (res) {
                    if (res == true) {
                        $('#subscription-popup').modal('show');
                    } else {
                        $('#txt-email-sub').attr('title', 'Your email is inaccurate.');
                        $('#txt-email-sub').tooltip('show');
                    }
                }
            });
        });

        $('#subscription-popup').on('hidden.bs.modal', function () {
            location.href = '/';
        });
    },
};
newsletter.init();