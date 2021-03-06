﻿var pageIndex = {
    init: function () {
        this.registerEvents();
        this.loadData();
        this.loadCkEditor();
    },

    registerEvents: function () {
        var validator = $('#frm-page').validate({
            errorClass: 'red',
            rules: {
                txtPageName: 'required',
                txtPageAlias: 'required',
                txtPageContent: 'required'
            },
            messages: {
                txtPageName: {
                    required: '*Please enter page name'
                },
                txtPageAlias: {
                    required: '*Please enter page alias',
                },
                txtPageContent: {
                    required: '*Please enter page content'
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
        $('#ddlShowPage').on('change', function () {
            common.config.pageSize = $('#ddlShowPage :selected').text();
            common.config.pageIndex = 1;
            $('#pagination-page').twbsPagination('destroy');
            pageIndex.loadData();
        });

        $('#txt-page-keyword').on('keypress', function (e) {
            if (e.which === 13) {
                $('#btn-search-page').click();
            }
        });

        $('#btn-search-page').off('click').on('click', function () {
            $('#pagination-page').twbsPagination('destroy');
            $('#ddlShowPage').prop('selectedIndex', 0);
            common.config.pageIndex = 1;
            common.config.pageSize = 10;
            pageIndex.loadData();
        });

        $('body').on('click', '.btnUpdatePage', function () {
            var pageId = $(this).data('id');
            $('#hid-page-id').val(pageId);
            $.ajax({
                type: 'GET',
                url: '/Admin/Page/GetPageDetail',
                data: { id: pageId },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#txt-page-name').val(res.Name);
                    CKEDITOR.instances['txt-page-content'].setData(res.Content);
                    $('#txt-page-alias').val(res.Alias);
                    $('#ck-page-status').prop('checked', res.Status);

                    $('#hid-page-date').val(res.CreatedDate);
                    $('#add-edit-page').modal('show');
                    common.stopLoadingIndicator();
                }
            });
        });

        $('#btn-save-page').on('click', function () {
            var id = $('#hid-page-id').val();
            var date = $('#hid-page-date').val();
            if (validator.form()) {
                var pageVm = {};
                if (id != null && id != '') {
                    pageVm.Id = id;
                    pageVm.CreatedDate = date;
                }
                pageVm.Name = $('#txt-page-name').val();
                pageVm.Alias = $('#txt-page-alias').val();
                pageVm.Content = CKEDITOR.instances['txt-page-content'].getData();
                pageVm.Status = $('#ck-page-status').is(':checked') ? 1 : 0;

                $.ajax({
                    type: 'POST',
                    url: '/Admin/Page/SavePage',
                    data: { pageViewModel: pageVm },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        if (res == true) {
                            common.notify('Save page success', 'success');
                            $('#pagination-page').twbsPagination('destroy');
                            pageIndex.loadData();
                            $('#add-edit-page').modal('hide');
                        } else {
                            common.notify("Save page failed. Please check 'content'.", 'error');
                        }
                        common.stopLoadingIndicator();
                    }
                });
            }
        });

        $('#btn-create-page').on('click', function () {
            $('#add-edit-page').modal('show');
            pageIndex.clearForm();
        });

        $('body').on('click', '.btnDeletePage', function () {
            var pageId = $(this).data('id');
            common.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Page/Delete',
                    data: { id: pageId },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        if (res == true) {
                            common.notify('Remove page success', 'success');

                            common.config.pageIndex = 1;
                            $('#pagination-page').twbsPagination('destroy');
                            pageIndex.loadData();
                        } else {
                            common.notify('Remove page failed', 'error');
                        }
                        common.stopLoadingIndicator();
                    }
                });
            });
        });
    },

    loadData: function () {
        var render = '';
        var template = $('#tbl-page-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Page/GetAllPaging',
            data: {
                keyword: $('#txt-page-keyword').val(),
                page: common.config.pageIndex,
                pageSize: common.config.pageSize,
            },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res.Results, function (i, item) {
                    var obj = {};
                    obj.Id = item.Id;
                    obj.Name = item.Name;
                    obj.Content = item.Content.substring(0, 200);
                    obj.Status = common.getStatusLabel(item.Status);
                    obj.CreatedDate = common.formatDateTime(item.CreatedDate);
                    render += Mustache.render(template, obj);
                });
                $('#tbl-page-content').html(render);
                $('#lbl-total-page').html(res.RowCount);

                if (res.RowCount > 0) {
                    pageIndex.pagination(res.RowCount);
                }
                common.stopLoadingIndicator();
            }
        });
    },

    pagination: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-page').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    pageIndex.loadData();
                }
            }
        });
    },

    loadCkEditor: function () {
        CKEDITOR.replace('txtPageContent', {
            language: 'en',
            width: '615px',
            height: '310px'
        });
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    },

    clearForm: function () {
        $('#hid-page-id').val('');
        $('#txt-page-name').val('');
        CKEDITOR.instances['txt-page-content'].setData('');
        $('#txt-page-alias').val('');
        $('#ck-page-status').prop('checked', false);
        $('#hid-page-date').val('');
    },
};
pageIndex.init();