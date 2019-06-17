var announcement = {
    init: function () {
        this.loadData();
        this.registerEvents();
        this.loadCkEditor();
    },

    registerEvents: function () {
        var validator = $('#frm-announ').validate({
            errorClass: 'red',
            rules: {
                txtAnnounTitle: 'required',
                txtAnnounContent: 'required',
            },
            messages: {
                txtAnnounTitle: {
                    required: '*Please enter title'
                },
                txtAnnounContent: {
                    required: '*Please enter content',
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
            $('#pagination-announ').twbsPagination('destroy');
            announcement.loadData();
        });

        $('#txt-announ-keyword').on('keypress', function (e) {
            if (e.which === 13) {
                $('#btn-search-announ').click();
            }
        });

        $('#btn-search-announ').off('click').on('click', function () {
            $('#pagination-announ').twbsPagination('destroy');
            $('#ddlShowPage').prop('selectedIndex', 0);
            common.config.pageIndex = 1;
            common.config.pageSize = 10;
            announcement.loadData();
        });

        $('#btn-create-announ').off('click').on('click', function () {
            $('#add-announ-modal').modal('show');
            announcement.clearData();
        });

        $('#btn-save-announ').off('click').on('click', function () {
            if (validator.form()) {
                var announId = $('#hid-announ-id').val();
                var announ = {};
                if (announId != undefined && announId != '') {
                    announ.Id = announId;
                    announ.CreatedDate = $('#hid-announ-date').val();
                }
                announ.Title = $('#txt-announ-title').val();
                announ.Content = CKEDITOR.instances['txt-announ-content'].getData();
                announ.Status = $('#ck-announ-status').is(':checked') ? 1 : 0;

                $.ajax({
                    type: 'POST',
                    url: '/Admin/Announcement/SaveAnnouncement',
                    data: {
                        announcementViewModel: announ,
                    },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        if (res == true) {
                            common.notify('Save announcement success', 'success');
                            common.stopLoadingIndicator();
                            $('#add-announ-modal').modal('hide');
                            announcement.loadData();
                        } else {
                            common.notify('Save announcement failed', 'error');
                            common.stopLoadingIndicator();
                        }
                    }
                });
            }
        });

        $('body').on('click', '.btnUpdateAnnoun', function () {
            var id = $(this).attr('data-id');
            $('#hid-announ-id').val(id);
            $.ajax({
                type: 'GET',
                url: '/Admin/Announcement/GetDetail',
                data: { announcementId: id },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#add-announ-modal').modal('show');
                    $('#txt-announ-title').val(res.Title);
                    CKEDITOR.instances['txt-announ-content'].setData(res.Content);
                    $('#ck-announ-status').prop('checked', res.Status);

                    $('#hid-announ-date').val(res.CreatedDate);
                    common.stopLoadingIndicator();
                },
            });
        });

        $('body').on('click', '.btnSendAnnoun', function () {
            var id = $(this).attr('data-id');
            common.confirm("<b>Are you sure to send this?</b></br></br>***This announcement will be sent to all users.", function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Announcement/SendAnnouncement',
                    data: { announId: id },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        if (res == true) {
                            common.notify('Sent announcement succeeded', 'success');
                            common.stopLoadingIndicator();
                        } else {
                            common.notify('This announcement is already sent to all users.', 'warn');
                            common.stopLoadingIndicator();
                        }
                    }
                });
            });
        });

        $('body').on('click', '.btnDeleteAnnoun', function () {
            var announId = $(this).data('id');
            common.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Announcement/Delete',
                    data: { id: announId },
                    success: function (res) {
                        if (res == true) {
                            common.notify('Remove announcement success', 'success');
                            common.config.pageIndex = 1;
                            $('#pagination-announ').twbsPagination('destroy');
                            announcement.loadData();
                        } else {
                            common.notify('Remove page failed', 'error');
                        }
                    }
                });
            });
        });
    },

    loadData: function () {
        var render = '';
        var template = $('#tbl-announ-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Announcement/GetAllPaging',
            data: {
                keyword: $('#txt-announ-keyword').val(),
                page: common.config.pageIndex,
                pageSize: common.config.pageSize,
            },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res.Results, function (i, item) {
                    var announ = {};
                    announ.Id = item.Id;
                    announ.Title = item.Title;
                    announ.Status = common.getStatusLabel(item.Status);
                    announ.CreatedDate = common.formatDateTime(item.CreatedDate);
                    announ.Sender = item.Username;

                    render += Mustache.render(template, announ);
                });
                $('#tbl-announ-content').html(render);
                $('#lbl-total-announ').html(res.RowCount);

                if (res.RowCount > 0) {
                    announcement.pagination(res.RowCount);
                }
                common.stopLoadingIndicator();
            }
        });
    },

    loadCkEditor: function () {
        CKEDITOR.replace('txtAnnounContent', {
            language: 'en',
            width: '715px',
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

    clearData: function () {
        $('#hid-announ-id').val('');
        $('#txt-announ-title').val('');
        CKEDITOR.instances['txt-announ-content'].setData('');
        $('#hid-announ-date').val('');
    },

    pagination: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-announ').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    announcement.loadData();
                }
            }
        });
    },
}
announcement.init();