var userInfo = {
    init: function () {
        this.registerEvents();
    },

    registerEvents: function () {
        var validator = $('#frm-user-dasboard').validate({
            errorClass: 'error-validation',
            rules: {
                FullName: 'required',
                Address: 'required',
                PhoneNumber: {
                    number: true,
                    required: true,
                    minlength: 10,
                    maxlength: 15
                },
                BirthDay: 'required',
                OldPassword: 'required',
            },
            messages: {
                FullName: {
                    required: '*Please enter fullname',
                },
                Address: {
                    required: '*Please enter address',
                },
                PhoneNumber: {
                    required: '*Please enter phone number',
                    number: '*Please enter an accurate phone number',
                    minlength: '*Please enter an accurate phone number',
                    maxlength: '*Please enter an accurate phone number'
                },
                BirthDay: {
                    required: '*Please enter birthday',
                },
                OldPassword: {
                    required: '*Please enter password to save changes',
                },
            }
        });

        $('.btn-save-change').off('click').on('click', function () {
            validator.form();
        });
    }
};
userInfo.init();