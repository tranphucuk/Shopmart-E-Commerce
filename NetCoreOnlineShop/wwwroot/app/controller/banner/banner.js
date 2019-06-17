var advertisement = {
    init: function () {
        this.loadData();
        this.registerEvents();
    },

    registerEvents: function () {
        var validator = $('#frm-adv').validate({
            errorClass: 'red',
            rules: {
                txtAdvName: "required",
                txtAdvImage: "required",
                description: "required",
                url: "required",
                order: {
                    required: true,
                    number: true,
                    min: 1
                },
                width: {
                    required: true,
                    number: true,
                    min: 1
                },
                height: {
                    required: true,
                    number: true,
                    min: 1
                },
            },
            messages: {
                txtAdvName: {
                    required: 'Advertisement name is required'
                },
                txtAdvImage: {
                    required: 'Image is required'
                },
                description: {
                    required: 'Description is required',
                },
                url: {
                    required: 'Url is required'
                },
                order: {
                    required: 'Order is required',
                    number: 'Please enter a valid number',
                    min: 'Please enter a positive number',
                },
                width: {
                    required: 'Width is required',
                    number: 'Please enter a valid number',
                    min: 'Please enter a positive number',
                },
                height: {
                    required: 'Height is required',
                    number: 'Please enter a valid number',
                    min: 'Please enter a positive number',
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

        $('body').on('click', '.btnUpdateAdv', function () {
            var bannerId = $(this).attr('data-id');
            $('#hid-adv-id').val(bannerId);
            $.ajax({
                type: 'GET',
                url: '/Admin/Banner/GetDetails',
                data: { id: bannerId },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#txt-adv-name').val(res.Name);
                    $('#txt-adv-image').val(res.Image);
                    $('#txt-banner-desc').val(res.Description);
                    $('#txt-banner-url').val(res.Url);
                    $('#txt-banner-order').val(res.SortOrder);
                    $('#txt-banner-width').val(res.Width);
                    $('#txt-banner-height').val(res.Height);
                    $('#ck-banner-status').prop('checked', res.Status);
                    $('#hid-adv-date').val(res.CreatedDate);

                    common.stopLoadingIndicator();
                    $('#add-edit-banner').modal('show');
                }
            });
        });

        $('#btn-create-adv').on('click', function () {
            advertisement.clearData();
            $('#add-edit-banner').modal('show');
        });

        $('#btn-save-adv').on('click', function () {
            var id = $('#hid-adv-id').val();
            if (validator.form()) {
                var fd = new FormData();
                if (id != undefined && id != '') {
                    fd.append('Id', id);
                    fd.append('CreatedDate', $('#hid-adv-date').val());
                }
                fd.append('picture', advertisement.imageData);
                fd.append('Name', $('#txt-adv-name').val());
                fd.append('Image', $('#txt-adv-image').val());
                fd.append('Description', $('#txt-banner-desc').val());
                fd.append('Url', $('#txt-banner-url').val());
                fd.append('SortOrder', $('#txt-banner-order').val());
                fd.append('Width', $('#txt-banner-width').val());
                fd.append('Height', $('#txt-banner-height').val());
                fd.append('Status', $('#ck-banner-status').is(':checked') ? 1 : 0);
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Banner/SaveAdvertisement',
                    data: fd,
                    processData: false,
                    contentType: false,
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        if (res == true) {
                            common.notify('Save success', 'success');
                            $('#add-edit-banner').modal('hide');
                            advertisement.loadData();
                            common.stopLoadingIndicator();
                        } else {
                            common.notify('Save failed', 'error');
                        }
                    }
                });
            }
        });

        $('#btn-select-image').on('click', function () {
            $('#file-adv-thumbnail').val('');
            $('#file-adv-thumbnail').click();
        });

        $('#file-adv-thumbnail').on('change', function () {
            advertisement.imageData = $(this)[0].files[0];
            $('#txt-adv-image').val('/admin-side/images/banner/' + advertisement.imageData.name);
        });

        $('body').on('click', '.btnDeleteAdv', function () {
            var bannerId = $(this).attr('data-id');
            common.confirm('Are you sure to remove this ads?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Banner/Delete',
                    data: {
                        id: bannerId
                    },
                    success: function (res) {
                        if (res == true) {
                            common.notify('Delete success', 'success');
                            advertisement.loadData();
                        } else {
                            common.notify('Delete failed', 'error');
                        }
                    }
                });
            });
        });
    },

    imageData: [],

    loadData: function () {
        var render = '';
        var template = $('#tbl-adv-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Banner/GetAll',
            dataType: 'json',
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res, function (i, item) {
                    var adv = {};
                    adv.Id = item.Id;
                    adv.Name = item.Name;
                    adv.Image = common.formatImage(item.Image, item.Name);
                    adv.Description = item.Description;
                    adv.CreatedDate = common.formatDateTime(item.CreatedDate);
                    adv.Status = common.getStatusLabel(item.Status);

                    render += Mustache.render(template, adv);
                });
                $('#tbl-adv-content').html(render);
                common.stopLoadingIndicator();
            }
        });
    },

    clearData: function () {
        $('#hid-adv-id').val('');
        $('#txt-adv-name').val('');
        $('#txt-adv-image').val('');
        $('#txt-banner-desc').val('');
        $('#txt-banner-url').val('');
        $('#txt-banner-width').val('');
        $('#txt-banner-height').val('');
        $('#ck-banner-status').prop('checked', true);
    },
}
advertisement.init();