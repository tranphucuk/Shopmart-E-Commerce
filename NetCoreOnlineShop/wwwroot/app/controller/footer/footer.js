var footer = {
    init: function () {
        $.when(
            footer.loadPageToDropDown(),
        ).then(function () {
            footer.registerEvents();
            footer.loadData();
        });
    },

    registerEvents: function () {
        var validator = $('#frm-footer').validate({
            errorClass: 'red',
            rules: {
                txtFooterName: 'required',
                txtFooterOrder: {
                    required: true,
                    number: true,
                    min: 1,
                },
                txtFooterDes: 'required',
            },
            messages: {
                txtFooterName: {
                    required: '*Please enter name'
                },
                txtFooterOrder: {
                    required: '*Please enter footer order',
                    number: '*Order must be a number',
                    min: '*Order must be equal or greater than 1',
                },
                txtFooterDes: {
                    required: '*Please enter description'
                }
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

        $('body').on('click', '.btnUpdateFooter', function () {
            var footerId = $(this).attr('data-id');
            $.ajax({
                type: 'GET',
                url: '/Admin/Footer/GetDetail',
                data: { id: footerId },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#txt-blog-name').val(res.Name);
                    $('#txt-footer-order').val(res.Order);
                    $('#txt-blog-des').val(res.Description);
                    $('#hid-footer-id').val(res.Id);
                    common.stopLoadingIndicator();
                    $('#add-edit-footer').modal('show');
                }
            });
        });

        $('#btn-save-footer').on('click', function () {
            var id = $('#hid-footer-id').val();
            if (validator.form()) {
                var footerVm = {};
                footerVm.Name = $('#txt-blog-name').val();
                footerVm.Order = $('#txt-footer-order').val();
                footerVm.Description = $('#txt-blog-des').val();
                footerVm.Id = id;
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Footer/Save',
                    data: { footerViewModel: footerVm },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        if (res == true) {
                            common.notify('Save success', 'success');
                            common.stopLoadingIndicator();
                            $('#add-edit-footer').modal('hide');
                            footer.loadData();
                        } else {
                            common.notify('Save failed. Please check again', 'error');
                            common.stopLoadingIndicator();
                        }
                    }
                });
            }
        });

        $('body').on('click', '.btnFooterPage', function () {
            var footerId = $(this).attr('data-id');
            $('#footerId').val(footerId);
            var option = '';
            $.each(footer.pageData, function (i, item) {
                option += '<option value="' + item.Id + '">' + item.Name + '</option>';
            });
            $('#ddl-pages').html(option);
            $('#footer-page-modal').modal('show');
            footer.loadFooterPages(footerId);
        });

        $('.btn-add-page').on('click', function () {
            var pageId = $('#ddl-pages').val();
            var footerId = $('#footerId').val();
            $.ajax({
                type: 'POST',
                url: '/Admin/Footer/AddPageToFooter',
                data: {
                    pageId: pageId,
                    footerId: footerId,
                },
                success: function (res) {
                    if (res == true) {
                        footer.loadFooterPages(footerId);
                    } else {
                        common.notify(res, 'warn');
                    }
                }
            });
        });

        $('body').on('click', '.btn-remove-page', function () {
            var pageFooterId = $(this).attr('data-id');
            var footerId = $('#footerId').val();
            $.ajax({
                type: 'POST',
                url: '/Admin/Footer/RemovePageFooter',
                data: { pageFooterId: pageFooterId },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    if (res == false) {
                        common.notify('Remove page failed', 'error');
                    } else {
                        footer.loadFooterPages(footerId);
                        common.stopLoadingIndicator();
                    }
                }
            });
        });
    },

    pageData: [],

    loadData: function () {
        var render = '';
        var template = $('#tbl-footer-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Footer/GetAll',
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res, function (i, item) {
                    var obj = {};
                    obj.Id = item.Id;
                    obj.Name = item.Name;
                    obj.Description = item.Description;
                    obj.Order = item.Order;
                    render += Mustache.render(template, obj);
                });
                $('#tbl-footer-content').html(render);
                common.stopLoadingIndicator();
            }
        })
    },

    loadPageToDropDown: function () {
        return $.ajax({
            type: 'GET',
            url: '/Admin/Page/GetAll',
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                footer.pageData = res;
                common.stopLoadingIndicator();
            }
        });
    },

    loadFooterPages: function (footerId) {
        var render = '';
        var template = $('#template-footer-links').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Footer/GetPagesByFooterId',
            data: { id: footerId },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res, function (i, item) {
                    var link = {};
                    link.Id = item.Id;
                    link.Name = item.Page.Name;
                    link.Status = common.getStatusLabel(item.Page.Status);
                    render += Mustache.render(template, link);
                });
                $('#tbl-body-links').html(render);
                common.stopLoadingIndicator();
            }
        });
    },
};
footer.init();