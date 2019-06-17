var ticket = {
    init: function () {
        this.registerEvents();
    },

    registerEvents: function () {
        var validator = $('#frm-ticket').validate({
            errorClass: 'error-validation',
            rules: {
                Email: 'required',
                BillId: {
                    number: true,
                    required: true,
                    min: 1,
                },
                Title: 'required',
                Content: {
                    required: true,
                    maxlength: 500,
                },
            },
            messages: {
                Email: {
                    required: '*Please enter email address',
                },
                BillId: {
                    required: '*You do not have any bill to select.',
                    min: '*You do not have any bill to select.',
                    number: '*You do not have any bill to select.',
                },
                Title: {
                    required: '*Please enter title',
                },
                Content: {
                    required: '*Please enter your inquiry',
                    maxlength:'*Maximum 500 word for your inquiry.'
                },
            }
        });

        $('#btn-send-ticket').off('click').on('click', function () {
            validator.form();
        });
    }
};
ticket.init();