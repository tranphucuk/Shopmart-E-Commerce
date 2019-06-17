var feedback = {
    init: function () {
        this.loadData();
        this.registerEvents();
    },

    registerEvents: function () {
        $('#ck-checkall-feedback').off('click').on('click', function () {
            var isChecked = $('#ck-checkall-feedback').is(':checked');
            $('#tbl-feedback-content input[type="checkbox"]').prop('checked', isChecked);
        });

        $('body').on('click', '.ck-delete-single', function () {
            var totalCheckbox = $('#tbl-feedback-content input[type="checkbox"]').length;
            var numberChecked = $('#tbl-feedback-content input[type="checkbox"]:checked').length;
            if (numberChecked === totalCheckbox) {
                $('#ck-checkall-feedback').prop('checked', true);
            } else {
                $('#ck-checkall-feedback').prop('checked', false);
            }
        });

        $('#ddlShowPage').on('change', function () {
            common.config.pageSize = $('#ddlShowPage :selected').text();
            common.config.pageIndex = 1;
            $('#pagination-feedback').twbsPagination('destroy');
            feedback.loadData();
        });

        $('#txt-feedback-keyword').on('keypress', function (e) {
            if (e.which === 13) {
                $('#btnSearch').click();
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            $('#pagination-feedback').twbsPagination('destroy');
            $('#ddlShowPage').prop('selectedIndex', 0);
            common.config.pageIndex = 1;
            common.config.pageSize = 10;
            feedback.loadData();
        });

        $('body').on('click', '.btnUpdateFeedback', function () {
            var feedbackId = $(this).attr('data-id');
            $('#hid-feedback-id').val(feedbackId);
            $.ajax({
                type: 'GET',
                url: '/Admin/Feedback/GetFeedbackDetail',
                data: { id: feedbackId },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#view-feedback-modal').modal('show');
                    $('#txt-feedback-name').val(res.Name);
                    $('#txt-feedback-email').val(res.Email);
                    $('#txt-feedback-address').val(res.Address);
                    $('#txt-feedback-message').val(res.Message);
                    $('#txt-feedback-date').val(common.formatDateTime(res.CreatedDate))
                    $('#ckStatus').prop('checked', res.Status);
                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    common.notify('Error: ' + err, 'error');
                    common.stopLoadingIndicator();
                }
            });
        });

        $('#btn-save-feedback').off('click').on('click', function () {
            var feedbackVm = {};
            feedbackVm.Id = $('#hid-feedback-id').val();
            feedbackVm.Name = $('#txt-feedback-name').val();
            feedbackVm.Email = $('#txt-feedback-email').val();
            feedbackVm.Address = $('#txt-feedback-address').val();
            feedbackVm.Message = $('#txt-feedback-message').val();
            feedbackVm.CreatedDate = $('#txt-feedback-date').val()
            feedbackVm.Status = $('#ckStatus').is(':checked') ? 1 : 0;
            $.ajax({
                type: 'POST',
                url: '/Admin/Feedback/SaveFeedback',
                data: { feedbackViewModel: feedbackVm },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#view-feedback-modal').modal('hide');
                    feedback.loadData();
                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    common.notify('Error: ' + err, 'error');
                    common.stopLoadingIndicator();
                }
            });
        });

        $('#btn-delete-multi').off('click').on('click', function () {
            var listId = [];
            $.each($('#tbl-feedback-content input[type="checkbox"]:checked'), function (i, item) {
                listId.push($(item).val());
            });
            if (listId.length > 0) {
                common.confirm('Are you sure to delete ' + listId.length + ' feedback?', function () {
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/Feedback/DeleteMultiFeedback',
                        data: { ids: JSON.stringify(listId) },
                        beforeSend: common.runLoadingIndicator(),
                        success: function (res) {
                            common.notify('Removed success: ' + listId.length + ' feedback.', 'success');
                            feedback.loadData();
                            common.stopLoadingIndicator();
                        },
                        error: function (err) {
                            common.notify('Error: ' + err.Message, 'error');
                            common.stopLoadingIndicator();
                        }
                    });

                });
            }
        });

        $('body').on('click', '.btnDeleteFeedback', function () {
            var feedbackId = $(this).attr('data-id');
            common.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Feedback/Delete',
                    data: { id: feedbackId },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        common.notify('Removed success feedback ID: ' + feedbackId, 'success');
                        feedback.loadData();
                        common.stopLoadingIndicator();
                    },
                    error: function (err) {
                        common.notify('Error: ' + err.Message, 'error');
                        common.stopLoadingIndicator();
                    }
                });
            });
        });
    },

    loadData: function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Feedback/GetAllPagination',
            data: {
                keyword: $('#txt-feedback-keyword').val(),
                page: common.config.pageIndex,
                pageSize: common.config.pageSize
            },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                var render = '';
                var template = $('#table-feedback-template').html();
                $.each(res.Results, function (i, item) {
                    var obj = {};
                    obj.Id = item.Id;
                    obj.Name = item.Name;
                    obj.Address = item.Address;
                    obj.Email = item.Email;
                    obj.Message = item.Message;
                    obj.CreatedDate = common.formatDateTime(item.CreatedDate);
                    obj.Status = common.getStatusLabel(item.Status);
                    render += Mustache.render(template, obj);
                });
                $('#tbl-feedback-content').html(render);
                if (res.RowCount > 0) {
                    feedback.pagination(res.RowCount);
                }
                $('#lblTotalRecords').text(res.RowCount);
                common.stopLoadingIndicator();
            },
            error: function (err) {
                common.notify('Error: ' + err, 'error');
                common.stopLoadingIndicator();
            }
        });
    },

    pagination: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-feedback').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    feedback.loadData();
                }
            }
        });
    }
};
feedback.init();