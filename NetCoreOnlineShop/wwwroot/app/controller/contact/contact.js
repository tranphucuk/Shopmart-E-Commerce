var contact = {
    init: function () {
        this.loadData();
        this.registerEvents();
    },

    registerEvents: function () {
        var validator = $('#frm-contact').validate({
            errorClass: 'red',
            rules: {
                name: 'required',
                website: 'required',
                address: 'required',
                email: {
                    required: true,
                    email: true,
                },
                phone: {
                    required: true,
                    number: true,
                    minlength: 10,
                    maxlength: 15
                },
            },
            messages: {
                name: {
                    required: '*Please enter name',
                },
                website: {
                    required: '*Please enter website',
                },
                address: {
                    required: '*Please enter address',
                },
                email: {
                    required: '*Please enter email',
                    email: '*Please enter an accurate email',
                },
                phone: {
                    required: '*Please enter phone number',
                    number: '*Please enter an accurate phone number',
                    minlength: '*Please enter an accurate phone number',
                    maxlength: '*Please enter an accurate phone number',
                },
            }
        });

        $('#btn-save').on('click', function () {
            if (validator.form()) {
                var contact = {};
                contact.Id = $('#txt-contact-id').val();
                contact.Name = $('#txt-name').val();
                contact.Website = $('#txt-website').val();
                contact.Address = $('#txt-address').val();
                contact.Phone = $('#txt-phone').val();
                contact.Email = $('#txt-email').val();
                contact.Other = $('#txt-others').val();

                $.ajax({
                    type: 'POST',
                    url: '/Admin/Contact/SaveContact',
                    data: {
                        contactVm: contact
                    },
                    beforeSend: common.stopLoadingIndicator(),
                    success: function (res) {
                        if (res == true) {
                            common.notify('Save success', 'success');
                        } else {
                            common.notify('Some fields are incorrect. Please check again.', 'error');
                        }
                        common.stopLoadingIndicator();
                    }
                });
            }
        });
    },

    loadData: function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Contact/LoadContact',
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $('#txt-contact-id').val(res.Id);
                $('#txt-name').val(res.Name);
                $('#txt-website').val(res.Website);
                $('#txt-address').val(res.Address);
                $('#txt-phone').val(res.Phone);
                $('#txt-email').val(res.Email);
                $('#txt-others').val(res.Other);
                common.stopLoadingIndicator();
            }
        });
    }
};
contact.init();