var contact = {
    init: function () {
        this.registerEvents();
    },

    registerEvents: function () {
        var validator = $('#frm-feedback').validate({
            errorClass: 'error-validation',
            rules: {
                Name: 'required',
                Email: 'required',
                Message: 'required'
            },
            messages: {
                Name: {
                    required: '*Please enter your name'
                },
                Email: {
                    required: '*Please enter your email'
                },
                Message: {
                    required: '*Please enter your message'
                },
            }
        });

        $('#btn-send-feedback').on('click', function () {
            if (validator.form()) {
            }
        });
    },
};
contact.init();