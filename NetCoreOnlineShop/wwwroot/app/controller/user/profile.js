var profile = {
    init: function () {
        this.loadData();
        this.registerEvents();
    },

    registerEvents: function () {
        var validator = $('#frm-profile').validate({
            errorClass: 'red',
            rules: {
                txFullname: "required",
                txtUserAvatar: "required",
                Address: "required",
                PhoneNumber: "required",
                oldPassword: {
                    required: true,
                },
                NewPassword: {
                    minlength: 6,
                    maxlength: 20,
                },
            },
            messages: {
                txFullname: {
                    required: 'Username is required'
                },
                txtUserAvatar: {
                    required: 'Avatar is required'
                },
                Address: {
                    required: 'Address is required',
                },
                PhoneNumber: {
                    required: 'Phone number is required',
                },
                oldPassword: {
                    required: 'Please enter your current password',
                },
                NewPassword: {
                    minLenghth: "minimum length of 8 characters",
                    maxLength: "maximum length of 20 characters",
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

        $('#btn-edit').on('click', function () {
            $('#txt-user-oldpass').val('');
            $('#txt-user-newpass').val('');
            profile.loadUserDetail();
        });

        $('#btn-select-avatar').off('click').on('click', function () {
            $('#file-user-avatar').val('');
            $('#file-user-avatar').click();
        });

        $('#file-user-avatar').on('change', function (e) {
            profile.avatar = $(this)[0].files[0];
            $('#txt-user-avatar').val('/images/avatar/' + profile.avatar.name);
        });

        $('#btn-save-profile').on('click', function () {
            if (validator.form()) {
                var fd = new FormData();
                if (profile.avatar != undefined && profile.avatar!= '') {
                    fd.append('Avatar', profile.avatar);
                }

                fd.append('FullName', $('#txt-fullname').val());
                fd.append('Address', $('#txt-user-address').val());
                fd.append('Avatar', $('#txt-user-avatar').val());
                fd.append('PhoneNumber', $('#txt-user-phone').val());

                fd.append('OldPassword', $('#txt-user-oldpass').val());
                fd.append('NewPassword', $('#txt-user-newpass').val());

                $.ajax({
                    type: 'POST',
                    url: '/Admin/Account/UpdateProfile',
                    data: fd,
                    processData: false,
                    contentType: false,
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        if (res.Success == false) {
                            common.notify(res.Message, 'success');
                            $('#edit-profile').modal('hide');
                            profile.loadData();
                            common.stopLoadingIndicator();
                        } else {
                            common.notify(res.Message, 'error');
                            common.stopLoadingIndicator();
                        }
                    }
                });
            }
        });
    },

    avatar: [],

    loadData: function () {
        var render = '';
        var template = $('#tbl-activity-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Account/LoadData',
            data: {
                page: common.config.pageIndex,
                pageSize: common.config.pageSize
            },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $('#user-avatar').attr('src', res.User.Avatar);
                $('#user-avatar').attr('alt', res.User.FullName);
                $('#user-avatar').attr('title', res.User.FullName);

                $('#user-name').html(' ' + res.User.FullName);
                $('#user-location').html(' ' + res.User.Address);
                $('#user-role').html(' ' + res.User.Roles[0]);
                $('#user-email').html(' ' + res.User.Email);
                $('#user-date').html(' ' + common.formatDate(res.User.CreatedDate));
                $('#user-phone').html(' ' + res.User.PhoneNumber);

                $.each(res.Activities.Results, function (i, item) {
                    var activity = {};
                    activity.Username = item.Username;
                    activity.LastSession = common.formatDateTime(item.LastSession);
                    activity.Device = item.Device;
                    activity.IpAddress = item.IPAddress;

                    render += Mustache.render(template, activity);
                });

                $('#tbl-activity-content').html(render);
                $('#lbl-total-activity').html(res.Activities.RowCount);
                if (res.Activities.RowCount > 0) {
                    profile.paginationActivity(res.Activities.RowCount);
                }
                common.stopLoadingIndicator();
            }
        });
    },

    loadUserDetail: function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Account/GetUserDetail',
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $('#txt-fullname').val(res.FullName);
                $('#txt-user-address').val(res.Address);
                $('#txt-user-avatar').val(res.Avatar);
                $('#txt-user-phone').val(res.PhoneNumber);

                $('#edit-profile').modal('show');
                common.stopLoadingIndicator();
            }
        })
    },

    paginationActivity: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-activity').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    profile.loadData();
                }
            }
        });
    },
};
profile.init();