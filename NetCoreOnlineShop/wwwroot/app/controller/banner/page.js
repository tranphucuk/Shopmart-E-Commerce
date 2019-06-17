var page = {
    init: function () {
        this.registerEvents();
    },

    registerEvents: function () {
        var validator = $('#frmPage').validate({
            errorClass: 'red',
            rules: {
                pageName: "required",
            },
            messages: {
                pageName: {
                    required: 'Name is required'
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

        $('#btn-create-page').on('click', function () {
            page.clearData();
            $('#add_page').modal('show');
        });

        $('#btnSave').on('click', function () {
            if (validator.form()) {
                var page = {};
                page.Name = $('#txtPageName').val();
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Banner/AddNewPage',
                    data: {
                        pageName: page
                    },
                    success: function (res) {
                        if (res == false) {
                            common.notify('Page name is existed. Please use another name', 'error');
                        } else {
                            common.notify('Create page success', 'success');
                            $('#add_page').modal('hide');
                        }
                    }
                });
            }
        });
    },

    clearData: function () {
        $('#txtPageName').val('');
    },
}
page.init();