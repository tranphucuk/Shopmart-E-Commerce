var appBlog = {
    init: function () {
        this.loadData();
        this.registerEvents();
        this.loadCkEditor();
    },

    registerEvents: function () {
        var validator = $('#frm-blog').validate({
            errorClass: 'red',
            rules: {
                txtBlogName: "required",
                txtBlogThumbnail: "required",
                txtBlogDescription: "required",
                txtBlogContent: "required",
                blogTag: "required",
            },
            messages: {
                txtBlogName: {
                    required: 'Name is required'
                },
                txtBlogThumbnail: {
                    required: 'Thumbnail is required'
                },
                txtBlogDescription: {
                    required: 'Description is required',
                },
                txtBlogContent: {
                    required: 'Content is required'
                },
                blogTag: {
                    required: 'Content is required'
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
            common.config.pageSizeSmall = $('#ddlShowPage :selected').text();
            common.config.pageIndex = 1;
            $('#pagination-blog').twbsPagination('destroy');
            appBlog.loadData();
        });

        $('#txt-blog-keyword').on('keypress', function (e) {
            if (e.which === 13) {
                $('#btn-search-blog').click();
            }
        });

        $('#btn-search-blog').off('click').on('click', function () {
            $('#pagination-blog').twbsPagination('destroy');
            $('#ddlShowPage').prop('selectedIndex', 0);
            common.config.pageIndex = 1;
            common.config.pageSizeSmall = 5;
            appBlog.loadData();
        });

        $('#ddl-blog-options').off('change').on('change', function () {
            common.config.pageSizeSmall = $('#ddlShowPage :selected').text();
            common.config.pageIndex = 1;
            $('#pagination-blog').twbsPagination('destroy');
            appBlog.loadData();
        });

        $('#btn-create-blog').off('click').on('click', function () {
            $('#add-edit-blog').modal('show');
            validator.resetForm();
            appBlog.clearForm();
        });

        $('body').on('click', '.btnUpdateBlog', function () {
            var blogId = $(this).data('id');
            $('#hid-blog-id').val(blogId);
            $.ajax({
                type: 'GET',
                url: '/Admin/Blog/GetBlogDetails',
                data: { id: blogId },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#add-edit-blog').modal('show');
                    $('#txt-blog-name').val(res.Name);
                    $('#txt-blog-thumbnail').val(res.Thumbnail);
                    $('#txt-blog-description').val(res.Description);
                    $('#txt-blog-view').val(res.ViewCount);
                    $('#txt-blog-tag').val(res.Tags);
                    CKEDITOR.instances['txt-blog-content'].setData(res.Content);
                    $('#ck-blog-status').prop('checked', res.Status);
                    $('#ck-blog-show-home').prop('checked', res.HomeFlag);
                    $('#ck-blog-hot').prop('checked', res.HotFlag);
                    $('#hid-blog-date').val(res.CreatedDate);
                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    common.notify('Error: ' + err, 'error');
                    common.stopLoadingIndicator();
                }
            });
        });

        $('#btn-select-thumbnail').off('click').on('click', function () {
            $('#file-blog-thumbnail').val('');
            $('#file-blog-thumbnail').click();
        });

        $('#file-blog-thumbnail').on('change', function (e) {
            appBlog.thumbnail = $(this)[0].files[0];
            $('#txt-blog-thumbnail').val('/client-side/images/blog/' + appBlog.thumbnail.name);
        });

        $('#btn-save-blog').off('click').on('click', function () {
            var id = $('#hid-blog-id').val();
            if (validator.form()) {
                var fd = new FormData();
                if (id != '' && id != undefined) {
                    fd.append('Id', id);
                    fd.append('CreatedDate', $('#hid-blog-date').val());
                }
                fd.append('image', appBlog.thumbnail);
                fd.append('Name', $('#txt-blog-name').val());
                fd.append('Description', $('#txt-blog-description').val());
                fd.append('Thumbnail', $('#txt-blog-thumbnail').val());
                fd.append('Content', CKEDITOR.instances['txt-blog-content'].getData());
                fd.append('ViewCount', $('#txt-blog-view').val());
                fd.append('Tags', $('#txt-blog-tag').val());
                fd.append('Status', $('#ck-blog-status').is(':checked') ? 1 : 0);
                fd.append('HomeFlag', $('#ck-blog-show-home').is(':checked') ? 1 : 0);
                fd.append('HotFlag', $('#ck-blog-hot').is(':checked') ? 1 : 0);
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Blog/SaveBlog',
                    data: fd,
                    processData: false,
                    contentType: false,
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        common.notify('Save blog success.', 'success');
                        $('#add-edit-blog').modal('hide');
                        appBlog.loadData();
                        common.stopLoadingIndicator();
                    },
                    error: function (err) {
                        common.notify('Error: ' + err, 'error');
                        common.stopLoadingIndicator();
                    }
                });
            }
        });

        $('body').on('click', '.btnBlogImage', function () {
            $('#tbl-gallery-content').html('');
            $('#blog-img-gallery').modal('show');
            var blogId = $(this).attr('data-id');
            $('#hid-blog-id').val(blogId);
            appBlog.galerry = [];
            $.ajax({
                type: 'GET',
                url: '/Admin/Blog/GetGalleryImage',
                data: { id: blogId },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    var render = '';
                    var template = $('#tbl-gallery-template').html();
                    $.each(res, function (i, item) {
                        var obj = {};
                        obj.Id = item.Id;
                        obj.Image = common.formatImageWithSize(item.Path, item.Caption, 80);
                        obj.Size = (item.Size * 0.000001).toFixed(2);

                        render += Mustache.render(template, obj);
                    });
                    if (render != '') {
                        $('#tbl-gallery-content').html(render);
                    }
                    $('#lbl-blog-total-image').text('Uploaded image: ' + res.length);
                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    common.notify('Error: ' + err, 'error');
                    common.stopLoadingIndicator();
                }
            });
        });

        $('#btn-import-imgs').off('click').on('click', function () {
            $('#file-blog-imgs-import').val('');
            $('#file-blog-imgs-import').click();
        });

        $('#file-blog-imgs-import').off('change').on('change', function () {
            var files = $(this)[0].files;
            appBlog.galerry = files;

            var render = '';
            var template = $('#tbl-gallery-template').html();
            $.each(files, function (i, item) {
                var urlCreator = window.URL || window.webkitURL;
                var imageUrl = urlCreator.createObjectURL(item);
                var obj = {};
                obj.Image = common.formatImageWithSize(imageUrl, item.name, 80);
                obj.Size = (item.size * 0.000001).toFixed(2);
                render += Mustache.render(template, obj);
            });
            if (render != '') {
                $('#tbl-gallery-content').append(render);
            }
        });

        $('body').on('click', '.btn-remove-img', function () {
            var imageId = $(this).attr('data-id');
            var selector = this;
            if (imageId != '' && imageId != undefined) {
                common.confirm('Are you sure to remove?', function () {
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/Blog/RemoveImage',
                        data: { id: imageId },
                        beforeSend: common.runLoadingIndicator(),
                        success: function (res) {
                            common.notify('Remove success!', 'success');
                            $(selector).closest('tr').remove();
                            common.stopLoadingIndicator();
                        },
                        error: function (err) {
                            common.notify('Error: ' + err, 'error');
                            common.stopLoadingIndicator();
                        }
                    });
                });
            } else {
                $(this).closest('tr').remove();
            }
        });

        $('#btn-upload-imgs').off('click').on('click', function () {
            var id = $('#hid-blog-id').val();
            if (appBlog.galerry.length == 0) {
                return;
            }
            var fd = new FormData();
            fd.append('blogId', id);
            $.each(appBlog.galerry, function (i, item) {
                fd.append(item.name, item);
            });

            $.ajax({
                type: 'POST',
                url: '/Admin/Blog/UploadMultiImages',
                data: fd,
                beforeSend: common.runLoadingIndicator(),
                processData: false,
                contentType: false,
                success: function (res) {
                    if (res.Success) {
                        common.notify(res.Message, 'success');
                    } else {
                        common.notify(res.Message, 'error');
                    }
                    $('#blog-img-gallery').modal('hide');
                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    common.notify('Error: ' + err, 'error');
                    common.stopLoadingIndicator();
                }
            });
        });

        $('body').on('click', '.btnDeleteBlog', function () {
            var blogId = $(this).attr('data-id');
            common.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Blog/RemoveBlog',
                    data: { id: blogId },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        common.notify('Remove success!', 'success');
                        appBlog.loadData();
                        common.stopLoadingIndicator();
                    },
                    error: function (err) {
                        common.notify('Error: ' + err, 'error');
                        common.stopLoadingIndicator();
                    }
                });
            });
        });
    },

    thumbnail: [],
    galerry: [],

    loadData: function () {
        var render = '';
        var template = $('#tbl-blog-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Blog/GetAllPaging',
            data: {
                page: common.config.pageIndex,
                pageSize: common.config.pageSizeSmall,
                keyword: $('#txt-blog-keyword').val(),
                option: $('#ddl-blog-options :selected').val(),
            },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res.Results, function (i, item) {
                    var obj = {};
                    obj.Id = item.Id;
                    obj.Name = item.Name;
                    obj.Thumbnail = common.formatImageWithSize(item.Thumbnail, item.Name, 120);
                    obj.Description = item.Description;
                    obj.CreatedDate = common.formatDateTime(item.CreatedDate);
                    obj.Status = common.getStatusLabel(item.Status);

                    render += Mustache.render(template, obj);
                });
                $('#tbl-blog-content').html(render);
                $('#lbl-total-blog').text(res.RowCount);
                if (res.RowCount > 0) {
                    appBlog.pagination(res.RowCount)
                }
                common.stopLoadingIndicator();
            },
            error: function (err) {
                common.notify('An error has occurred while loading data', 'error');
                common.stopLoadingIndicator();
            }
        });
    },

    pagination: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSizeSmall);

        $('#pagination-blog').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    appBlog.loadData();
                }
            }
        });
    },

    loadCkEditor: function () {
        CKEDITOR.replace('txtBlogContent', {
            language: 'en',
            width: '615px',
            height: '180px'
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
        $('#txt-blog-name').val('');
        $('#txt-blog-thumbnail').val('');
        $('#txt-blog-view').val(0);
        $('#txt-blog-tag').val('');
        $('#txt-blog-description').val('');
        CKEDITOR.instances['txt-blog-content'].setData('');
        $('#ck-blog-status').prop('checked', true);
        $('#ck-blog-show-home').prop('checked', false);
        $('#ck-blog-hot').prop('checked', false);
    }
};
appBlog.init();