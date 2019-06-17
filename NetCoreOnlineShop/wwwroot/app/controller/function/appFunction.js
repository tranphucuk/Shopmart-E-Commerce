var appFunc = {
    init: function () {
        this.loadData();
        this.registerEvent();
    },

    registerEvent: function () {
        var validator = $('#frm-function').validate({
            errorClass: 'red',
            rules: {
                txtFunctionName: 'required',
                txtFunctionOrder: {
                    required: true,
                    number: true,
                    min: 0
                },
            }
        });

        $('#ddlShowPage').on('change', function () {
            common.config.pageSize = $('#ddlShowPage :selected').text();
            common.config.pageIndex = 1;
            $('#pagination-function').twbsPagination('destroy');
            appFunc.loadData();
        });

        $('#txt-function-keyword').on('keypress', function (e) {
            if (e.which === 13) {
                $('#btn-search-func').click();
            }
        });

        $('#btn-search-func').off('click').on('click', function () {
            $('#pagination-function').twbsPagination('destroy');
            $('#ddlShowPage').prop('selectedIndex', 0);
            common.config.pageIndex = 1;
            common.config.pageSize = 10;
            appFunc.loadData();
        });

        $('body').on('click', '.btnUpdateFunction', function () {
            var funcId = $(this).attr('data-id');
            $('#hid-function-id').val(funcId);
            $.ajax({
                type: 'GET',
                url: '/Admin/Function/GetFunctionDetail',
                data: { id: funcId },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#add_edit_function').modal('show');
                    validator.resetForm();
                    appFunc.disableFunction();
                    $('#txt-func-id').val(res.Id);
                    $('#txt-func-name').val(res.Name);
                    $('#txt-func-icon').val(res.IconCss);
                    $('#txt-func-order').val(res.SortOrder);
                    $('#function-preview-icon').removeClass();
                    $('#function-preview-icon').addClass(res.IconCss + ' form-control-feedback right');
                    $('#txt-url-func').val(res.Url);
                    $('#txt-func-Parent').val(res.ParentId);
                    $('#ck-func-status').prop('checked', res.Status);

                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    common.notify('Error: ' + err, "error");
                    common.stopLoadingIndicator();
                }
            });
        });

        $('#btn-save-function').off('click').on('click', function () {
            if (validator.form()) {
                var functionVm = {};
                functionVm.Id = $('#txt-func-id').val();
                functionVm.Name = $('#txt-func-name').val();
                functionVm.IconCss = $('#txt-func-icon').val();
                functionVm.ParentId = $('#txt-func-Parent').val();
                functionVm.SortOrder = $('#txt-func-order').val();
                functionVm.Url = $('#txt-url-func').val();
                functionVm.Status = $('#ck-func-status').is(':checked') ? 1 : 0;
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Function/SaveFunction',
                    data: {
                        functionViewModel: functionVm
                    },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        common.notify('Save success!', 'success');
                        $('#add_edit_function').modal('hide');
                        appFunc.loadData();
                        common.stopLoadingIndicator();
                    },
                    error: function (err) {
                        common.notify('Error: ' + err.responseText, "error");
                        common.stopLoadingIndicator();
                    }
                });
            }
        });
    },

    loadData: function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Function/GetAllPaging',
            data: {
                keyword: $('#txt-function-keyword').val(),
                page: common.config.pageIndex,
                pageSize: common.config.pageSize
            },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                var render = '';
                var template = $('#tbl-function-template').html();
                $.each(res.Results, function (i, item) {
                    var obj = {};
                    obj.Id = item.Id;
                    obj.Name = item.Name;
                    obj.Status = common.getStatusLabel(item.Status);
                    render += Mustache.render(template, obj);
                });
                $('#tbl-function-content').html(render);
                if (res.RowCount > 0) {
                    appFunc.pagination(res.RowCount)
                }
                $('#lbl-total-function').text(res.RowCount);
                common.stopLoadingIndicator();
            },
            error: function (err) {
                common.notify('Error: ' + err, "error");
                common.stopLoadingIndicator();
            }
        });
    },

    pagination: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-function').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    appFunc.loadData();
                }
            }
        });
    },

    disableFunction: function () {
        $('#txt-func-id').prop('readonly', true);
        $('#txt-func-Parent').prop('readonly', true);
        $('#txt-url-func').prop('readonly', true);
    },
};
appFunc.init();