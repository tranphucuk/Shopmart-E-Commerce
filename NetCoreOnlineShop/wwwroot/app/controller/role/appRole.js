var appRole = {
    init: function () {
        this.loadData();
        this.registerEvents();
    },
    registerEvents: function () {
        var validator = $("#frmRole").validate({
            errorClass: 'red',
            rules: {
                roleName: "required",
                roleDescription: "required",
            },
        });

        $('body').on('click', '#btnCreate', function () {
            $('#add_edit_Role').modal('show');
            $('.hideBorder').val('Create New Role');
            appRole.clearForm();
            validator.resetForm();
        });

        $('body').on('click', '.btnUpdateRole', function () {
            var roleId = $(this).attr('data-id');
            $.ajax({
                type: 'GET',
                url: '/Admin/Role/GetRoleById',
                dataType: 'json',
                data: { id: roleId },
                success: function (res) {
                    validator.resetForm();
                    $('#add_edit_Role').modal('show');
                    $('.hideBorder').val('Update Role: ' + res.Name);
                    $('#hidRoleId').val(res.Id);
                    $('#txtRoleName').val(res.Name);
                    $('#txtDescription').val(res.Description);
                },
                error: function (err) {
                    common.notify('Error getting role detail', 'error');
                    console.log(err);
                }
            });
        });

        $('body').on('click', '.btnDeleteRole', function () {
            var roleId = $(this).attr('data-id');
            common.confirm('Are you sure to delete ?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Role/DeleteAsync',
                    data: { id: roleId },
                    dataType: 'json',
                    success: function (res) {
                        common.notify('Removed role: ' + res.Message, 'success');
                        appRole.loadData();
                    },
                    error: function (err) {
                        common.notify(err, 'error');
                    }
                });
            });
        });

        $('#btnSave').off('click').on('click', function (e) {
            e.preventDefault();
            validator = $("#frmRole").validate();
            if (validator.form()) {
                var roleViewModel = {};
                if ($('#hidRoleId').val() !== '') {
                    roleViewModel.Id = $('#hidRoleId').val();
                }
                roleViewModel.Name = $('#txtRoleName').val();
                roleViewModel.Description = $('#txtDescription').val();
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Role/SaveRoleAsync',
                    data: { appRoleVm: roleViewModel },
                    success: function (res) {
                        if (res.Message == 'add') {
                            common.notify('Created role: ' + res.Data, 'success');
                        } else if (res.Message == 'update') {
                            common.notify('Update role: ' + res.Data, 'success');
                        }
                        $('#add_edit_Role').modal('hide');
                        appRole.loadData();
                    },
                    error: function (err) {
                        $('#add_edit_Role').modal('hide');
                        appRole.loadData();
                        common.notify(err.responseText, 'error');
                        console.log(err);
                    }
                });
            }
        });

        $('#txtKeyword').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                appRole.loadData();
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            appRole.loadData();
        });

        $('body').on('click', '.btnGrantPermission', function () {
            var roleId = $(this).attr('data-id');
            $('#hiddenRoleId').val(roleId);
            var request = $.ajax(appRole.loadFunctions());
            request.done(function () {
                $.ajax({
                    type: 'GET',
                    url: '/Admin/Role/LoadPermission',
                    data: { roleId: roleId },
                    dataType: 'json',
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        $('#tblPermisson tbody tr').each(function (i, item) {
                            $.each(res, function () {
                                if ($(item).attr('data-id') == this.FunctionId) {
                                    $(item).find('.ckView').prop('checked', this.CanRead);
                                    $(item).find('.ckCreate').prop('checked', this.CanCreate);
                                    $(item).find('.ckUpdate').prop('checked', this.CanUpdate);
                                    $(item).find('.ckDelete').prop('checked', this.CanDelete);
                                }
                            });
                            if ($('.ckView:checked').length == $('#tblPermisson tbody .ckView').length) {
                                $('.ckCheckAllView').prop('checked', true);
                            } else {
                                $('.ckCheckAllView').prop('checked', false);
                            }

                            if ($('.ckCreate:checked').length == $('#tblPermisson tbody .ckCreate').length) {
                                $('.ckCheckAllCreate').prop('checked', true);
                            } else {
                                $('.ckCheckAllCreate').prop('checked', false);
                            }

                            if ($('.ckUpdate:checked').length == $('#tblPermisson tbody .ckUpdate').length) {
                                $('.ckCheckAllUpdate').prop('checked', true);
                            } else {
                                $('.ckCheckAllUpdate').prop('checked', false);
                            }

                            if ($('.ckDelete:checked').length == $('#tblPermisson tbody .ckDelete').length) {
                                $('.ckCheckAllDelete').prop('checked', true);
                            } else {
                                $('.ckCheckAllDelete').prop('checked', false);
                            }
                        });
                        $('#add_edit_Permission').modal('show');
                        common.stopLoadingIndicator();
                    },
                    error: function (err) {
                        common.notify('Error: ' + err, 'error');
                    }
                });
            });
        });

        $('#btnSavePermission').off('click').on('click', function () {
            var roleId = $('#hiddenRoleId').val();
            var listPermission = [];
            $('#tblPermisson tbody tr').each(function (i, item) {
                listPermission.push({
                    RoleId: $('#hiddenRoleId').val(),
                    FunctionId: $(item).attr('data-id'),
                    CanRead: $(item).find('.ckView').is(':checked'),
                    CanCreate: $(item).find('.ckCreate').is(':checked'),
                    CanUpdate: $(item).find('.ckUpdate').is(':checked'),
                    CanDelete: $(item).find('.ckDelete').is(':checked'),
                });
            });
            $.ajax({
                type: 'POST',
                url: '/Admin/Role/SavePermission',
                data: {
                    roleId: roleId,
                    listPermissionVm: listPermission
                },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    if (res.Success == false) {
                        common.notify('Error: ' + res.Message, 'error');
                        common.stopLoadingIndicator();
                    } else {
                        $('#add_edit_Permission').modal('hide');
                        common.notify('Update Permission successfully', 'success');
                        common.stopLoadingIndicator();
                    }
                }
            });
        });

        $('.ckCheckAllView').off('change').on('change', function () {
            if ($(this).is(':checked')) {
                $('#tblPermisson tbody tr').each(function (i, item) {
                    $(item).find('.ckView').prop('checked', true);
                });
            } else {
                $('#tblPermisson tbody tr').each(function (i, item) {
                    $(item).find('.ckView').prop('checked', false);
                });
            }
        });

        $('.ckCheckAllCreate').off('change').on('change', function () {
            if ($(this).is(':checked')) {
                $('#tblPermisson tbody tr').each(function (i, item) {
                    $(item).find('.ckCreate').prop('checked', true);
                });
            } else {
                $('#tblPermisson tbody tr').each(function (i, item) {
                    $(item).find('.ckCreate').prop('checked', false);
                });
            }
        });

        $('.ckCheckAllUpdate').off('change').on('change', function () {
            if ($(this).is(':checked')) {
                $('#tblPermisson tbody tr').each(function (i, item) {
                    $(item).find('.ckUpdate').prop('checked', true);
                });
            } else {
                $('#tblPermisson tbody tr').each(function (i, item) {
                    $(item).find('.ckUpdate').prop('checked', false);
                });
            }
        });

        $('.ckCheckAllDelete').off('change').on('change', function () {
            if ($(this).is(':checked')) {
                $('#tblPermisson tbody tr').each(function (i, item) {
                    $(item).find('.ckDelete').prop('checked', true);
                });
            } else {
                $('#tblPermisson tbody tr').each(function (i, item) {
                    $(item).find('.ckDelete').prop('checked', false);
                });
            }
        });

        $('body').on('click', '.ckView', function () {
            appRole.isReadAll();
        });

        $('body').on('click', '.ckCreate', function () {
            appRole.isCreateAll();
        });

        $('body').on('click', '.ckUpdate', function () {
            appRole.isEditAll();
        });

        $('body').on('click', '.ckDelete', function () {
            appRole.isDeleteAll();
        });
    },

    loadData: function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Role/GetAllPaging',
            data: {
                keyword: $('#txtKeyword').val(),
                pageSize: common.config.pageSize,
                page: common.config.pageIndex
            },
            success: function (res) {
                var template = $('#table-template').html();
                var render = '';
                $.each(res.Results, function (i, item) {
                    var obj = {};
                    obj.Id = item.Id;
                    obj.Name = item.Name;
                    obj.Description = item.Description;
                    render += Mustache.render(template, obj);
                    if (render != null) {
                        $('#tbl-content').html(render);
                        $('#lblTotalRecords').text(res.Results.length);
                    }
                });
            },
            error: function (err) {
                console.log(err);
                common.notify('An error has occurred', 'error');
            }
        });
    },

    loadFunctions: function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Function/GetAll',
            success: function (res) {
                $('#add_edit_Permission').modal('show');
                var render = '';
                var template = $('#list-function-template').html();
                $.each(res, function (i, item) {
                    var obj = {};
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Tree: 'treegrid-' + item.Id,
                        Name: item.Name,
                        TreeParent: item.ParentId == null ? '' : 'treegrid-parent-' + item.ParentId
                    });
                });
                if (render != '') {
                    $('#tbl-function-content').html(render);
                }
                $('.tree').treegrid({
                    'initialState': 'collapsed'
                });
            },
            error: function (err) {
                common.notify('Error getting functions list/n' + err, 'error');
            }
        });
    },

    clearForm: function () {
        $('#hidUserId').val('');
        $('#txtRoleName').val('');
        $('#txtDescription').val('');
    },

    isReadAll: function () {
        var listReads = $('#tbl-function-content .ckView').length;
        totalChecked = $('#tbl-function-content .ckView:checked').length;

        if (totalChecked < listReads) {
            $('.ckCheckAllView').prop('checked', false);
        } else {
            $('.ckCheckAllView').prop('checked', true);
        }
    },

    isCreateAll: function () {
        var listReads = $('#tbl-function-content .ckCreate').length;
        totalChecked = $('#tbl-function-content .ckCreate:checked').length;

        if (totalChecked < listReads) {
            $('.ckCheckAllCreate').prop('checked', false);
        } else {
            $('.ckCheckAllCreate').prop('checked', true);
        }
    },

    isEditAll: function () {
        var listReads = $('#tbl-function-content .ckUpdate').length;
        totalChecked = $('#tbl-function-content .ckUpdate:checked').length;

        if (totalChecked < listReads) {
            $('.ckCheckAllUpdate').prop('checked', false);
        } else {
            $('.ckCheckAllUpdate').prop('checked', true);
        }
    },

    isDeleteAll: function () {
        var listReads = $('#tbl-function-content .ckDelete').length;
        totalChecked = $('#tbl-function-content .ckDelete:checked').length;

        if (totalChecked < listReads) {
            $('.ckCheckAllDelete').prop('checked', false);
        } else {
            $('.ckCheckAllDelete').prop('checked', true);
        }
    },

    clearPermission: function () {
        $('.tbl-permission input[type="checkbox"]').prop('checked', false);
    },
};
appRole.init();