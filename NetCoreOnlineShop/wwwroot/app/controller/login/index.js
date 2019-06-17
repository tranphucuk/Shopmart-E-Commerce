/// <reference path="../../../lib/jquery/dist/jquery.js" />
var loginController = {
    init: function () {
        this.registerEvent();
    },

    registerEvent: function () {
        var validator = $('#frmLoginForm').validate({
            errorClass: 'red',
            rules: {
                username: "required",
                password: "required",
            },
            messages: {
                username: {
                    required: 'Username is required'
                },
                password: {
                    required: 'Password is required'
                },
            },
            showErrors: function (errorMap, errorList) {
                if (validator.submitted) {
                    var summary = "";
                    $.each(errorList, function () { summary += '* ' + this.message + "\n"; });
                    common.notify(summary, 'warn');
                    submitted = false;
                }
            },
        });
        $('#btnLogin').on('click', function (e) {
            e.preventDefault();
            if ($('#frmLoginForm').valid()) {
                var user = $('#txtUsername').val();
                var pass = $('#txtPassword').val();
                loginController.login(user, pass);
            }
        });
    },

    login: function (user, pass) {
        $.ajax({
            type: "POST",
            data: {
                Username: user,
                Password: pass
            },
            dataType: "json",
            url: "/admin/login/authen",
            success: function (res) {
                if (res.Success) {
                    window.location.href = "/Admin/Home/Index";
                } else {
                    common.notify(res.Message, "error");
                }
            }
        });
    },
};
loginController.init();
