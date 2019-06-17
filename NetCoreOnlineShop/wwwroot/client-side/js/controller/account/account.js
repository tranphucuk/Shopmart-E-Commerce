var account = {
    init: function () {
        this.registerEvents();
    },

    registerEvents: function () {
        var validator = $('#frm-register').validate({
            errorClass: 'error-validation',
            rules: {
                UserName: 'required',
                Email: {
                    email: true,
                    required: true
                },
                FullName: 'required',
                Address: 'required',
                PhoneNumber: {
                    number: true,
                    required: true,
                    minlength: 10,
                    maxlength: 15
                },
                BirthDay: 'required',
                Password: 'required',
                ConfirmPassword: 'required',
            },
            messages: {
                UserName: {
                    required: '*Please enter username',
                },
                Email: {
                    required: '*Please enter email',
                    email:'*Please enter an accurate email'
                },
                FullName: {
                    required: '*Please enter fullname',
                },
                Address: {
                    required: '*Please enter address',
                },
                PhoneNumber: {
                    required: '*Please enter phone number',
                    number:'*Please enter an accurate phone number',
                    minlength:'*Please enter an accurate phone number',
                    maxlength:'*Please enter an accurate phone number'
                },
                BirthDay: {
                    required: '*Please enter birthday',
                },
                Password: {
                    required: '*Please enter password',
                },
                ConfirmPassword: {
                    required: '*Please enter confirm password',
                },
            }
        });

        $('.btn-register').on('click', function () {
            validator.form();
        });
    }
};
account.init();