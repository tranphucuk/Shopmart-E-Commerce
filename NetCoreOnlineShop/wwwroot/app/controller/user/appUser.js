/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../shared/common.js" />
var appUser = {
    init: function () {
        this.registerEvents();
        this.loadData();
    },

    registerEvents: function () {
        var validator = $('#frmUser').validate({
            errorClass: 'red',
            rules: {
                Username: "required",
                UserPassword: "required",
                ConfirmPassword: "required",
                UserPassword: {
                    required: true,
                    minlength: 6,
                },
                ConfirmPassword: {
                    required: true,
                    minlength: 6,
                    equalTo: "#txtPassword"
                },
                EmailAddress: {
                    email: true,
                    required: true
                },
                FullName: "required",
                PhoneNumber: {
                    required: true,
                    number: true
                },
                Balance: {
                    required: true,
                    number: true
                },
            },
            messages: {
                Username: 'Username is required',
                UserPassword: {
                    required: 'Password is required',
                    minlength: 'Password must least 6 characters',
                },
                ConfirmPassword: {
                    required: 'Confirm password is required',
                    equalTo: "Password is not matched"
                },
                EmailAddress: {
                    required: 'Email address is required',
                    email: 'Please enter an accurate email address'
                },
                FullName: {
                    required: 'Full name is required',
                },
                PhoneNumber: {
                    required: 'Phone number is required',
                    number: 'please enter correct phone number'
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

        $('body').on('click', '.btnUpdateUser', function (e) {
            e.preventDefault();
            var userId = $(this).attr('data-id');
            validator.resetForm();
            $.ajax({
                type: 'GET',
                url: '/Admin/User/GetUserByIdAsync',
                data: {
                    id: userId
                },
                dataType: 'json',
                success: function (res) {
                    $('#add_edit_user').modal('show');
                    appUser.commonConfig('update');
                    $('#hidUserId').val(res.Id);
                    $('#txtUsername').val(res.UserName);
                    $('#txtEmailaddress').val(res.Email);
                    $('#txtPhoneNumber').val(res.PhoneNumber);
                    $('#txtFullName').val(res.FullName);
                    $('#txtBalance').val(res.Balance);
                    $('#txtAvatarImg').val(res.Avatar);
                    $('#txtCreatedDate').val(common.formatDateTime(res.CreatedDate));
                    $('#txtModifiedDate').val(common.formatDateTime(res.ModifiedDate));
                    $('#ckStatus').prop('checked', res.Status);

                    var request = $.ajax(appUser.initRoles());
                    request.done(function () {
                        $('#frmUser input[data-id]').each(function (i, item) {
                            if (res.Roles.indexOf($(item).attr('data-id')) !== -1) {
                                item.checked = true;
                            }
                        })
                    });
                    $('.hideBorder').val('Update User: ' + res.UserName);
                }
            });
        });

        $('body').on('click', '#btnCreate', function () {
            $('#add_edit_user').modal('show');
            $('.hideBorder').val('Create New User');
            appUser.clearForm();
            validator.resetForm();
            appUser.initRoles();
            appUser.commonConfig('create');
        });

        $('#btnSave').off('click').on('click', function (e) {
            e.preventDefault();
            validator = $("#frmUser").validate();
            if (validator.form()) {
                var userVm = {};
                if ($('#hidUserId').val() != '') {
                    userVm.Id = $('#hidUserId').val();
                    userVm.CreatedDate = $('#txtCreatedDate').val();
                }
                var roles = [];
                userVm.UserName = $('#txtUsername').val();
                userVm.Password = $('#txtPassword').val();
                userVm.Email = $('#txtEmailaddress').val();
                userVm.PhoneNumber = $('#txtPhoneNumber').val();
                userVm.FullName = $('#txtFullName').val();
                userVm.Balance = $('#txtBalance').val();
                userVm.Avatar = $('#txtAvatarImg').val();
                userVm.Status = $('#ckStatus').is(':checked') ? 1 : 0;
                $('#frmUser input[data-id]').each(function (i, item) {
                    if ($(item).is(':checked')) {
                        roles.push($(item).attr('data-id'));
                    }
                });
                userVm.Roles = roles;
                $.ajax({
                    type: 'POST',
                    url: '/Admin/User/AccountAsync',
                    data: {
                        appUserVm: userVm
                    },
                    success: function (res) {
                        appUser.loadData();
                        $('#add_edit_user').modal('hide');
                        if (res.HttpType == 'update') {
                            common.notify('Update user: "' + res.ObjReturn + '" succeeded.', 'success');
                        } else {
                            common.notify('Create user: "' + res.ObjReturn + '" succeeded.', 'success');
                        }
                    },
                    error: function (err) {
                        $('#add_edit_user').modal('hide');
                        appUser.loadData();
                        common.notify(err.responseText, 'error');
                        console.log(err);
                    }
                });
            }
        });

        $('body').on('click', '.btnDeleteUser', function (e) {
            e.preventDefault();
            var userId = $(this).attr('data-id');
            common.confirm('Are you sure to delete ?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/User/DeleteAsync',
                    data: { id: userId },
                    dataType: 'json',
                    success: function (res) {
                        common.notify('Removed user: ' + res.Message, 'success');
                        appUser.loadData();
                    },
                    error: function (res) {
                        common.notify('An error has occurred while removing: ' + res.Message, 'error');
                        appUser.loadData();
                    }
                });
            });
        });

        $('#ddlShowPage').on('change', function () {
            common.config.pageSize = $('#ddlShowPage :selected').text();
            common.config.pageIndex = 1;
            $('#pagination-user').twbsPagination('destroy');
            appUser.loadData();
        });

        $('#txtKeyword').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                $('#btnSearch').click();
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            $('#pagination-user').twbsPagination('destroy');
            appUser.loadData();
        });

        $('#btnSelectImg').off('click').on('click', function () {
            $('#fileUserAvatar').click();
        });

        $('#fileUserAvatar').off('change').on('change', function () {
            var ImgObj = {
                FileSize: this.files[0].size,
                FileName: this.files[0].name
            };
            common.readImg(this.files[0], function (e) {
                ImgObj.ImgValueBase64 = e.target.result;
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Upload/UploadImage',
                    data: { imageVm: ImgObj },
                    success: function (res) {
                        $('#txtAvatarImg').val(res.ImgPath);
                        common.notify('Saved an image name: "  ' + res.FileName + '  " to root folder', 'success');
                    },
                    error: function (err) {
                        common.notify('An error has occurred, please check console', 'error');
                        console.log(err);
                    }
                });
            });
        });
    },

    commonConfig: function (type) {
        if (type == 'update') {
            $('#txtUsername').prop('disabled', true);
            $('#txtPassword').prop('disabled', true);
            $('#txtConfirmPassword').prop('disabled', true);
            $('#frmConfirmPassword,#frmPassword').hide();
            $('#frmCreatedDate,#frmModifiedDate').show();
        }
        else if (type == 'create') {
            $('#frmConfirmPassword,#frmPassword').show();
            $('#txtUsername').prop('disabled', false);
            $('#txtPassword').prop('disabled', false);
            $('#txtConfirmPassword').prop('disabled', false);
            $('#txtUsername').prop('disabled', false);
            $('#frmCreatedDate,#frmModifiedDate').hide();
        }
    },

    loadData: function () {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/Admin/User/GetAllPaging',
            dataType: 'json',
            data: {
                keyword: $('#txtKeyword').val(),
                pageSize: common.config.pageSize,
                page: common.config.pageIndex
            },
            success: function (res) {
                $.each(res.Results, function (i, item) {
                    var obj = {
                        Id: item.Id,
                        Username: item.UserName,
                        Fullname: item.FullName,
                        Avatar: common.formatImage(item.Avatar),
                        EmailAddress: item.Email,
                        PhoneNumber: item.PhoneNumber,
                        Balance: common.formartNumber(item.Balance),
                        CreatedDate: common.formatDate(item.CreatedDate),
                        Status: common.getStatusLabel(item.Status)
                    }
                    render += Mustache.render(template, obj);
                });
                $('#tbl-content').html(render);
                $('#lblTotalRecords').text(res.RowCount);
                if (res.RowCount > 0) {
                    appUser.pagination(res.RowCount);
                }
            },
            error: function (err) {
                console.log(err);
                common.notify('An error has occurred', 'error');
            }
        });
    },

    initRoles: function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Role/GetAllAsync',
            success: function (res) {
                var render = "";
                var template = $('#role-checkbox-template').html();
                $.each(res, function (i, item) {
                    var obj = {};
                    obj.Name = item.Name;
                    obj.Description = item.Description;
                    render += Mustache.render(template, obj);
                });
                if (render != "") {
                    $('#render-checkbox').html(render);
                }
            },
            error: function (err) {
                console.log(err);
                common.notify('Error loading data', 'error');
            }
        });
    },

    clearForm: function () {
        $('#hidUserId').val('');
        $('#txtUsername').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');
        $('#txtEmailaddress').val('');
        $('#txtPhoneNumber').val('');
        $('#txtFullName').val('');
        $('#txtBalance').val(0);
        $('#txtAvatarImg').val('');
        $('#ckStatus').prop('checked', true);
    },

    pagination: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-user').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    appUser.loadData();
                }
            }
        });
    },
};
appUser.init();