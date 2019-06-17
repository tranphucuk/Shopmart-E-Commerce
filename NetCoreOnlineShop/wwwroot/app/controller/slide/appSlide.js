var slide = {
    init: function () {
        this.loadData();
        this.registerEvents();
    },

    registerEvents: function () {
        var validator = $('#frm-slide').validate({
            errorClass: 'red',
            rules: {
                txtslideName: "required",
                txtSlideUrl: "required",
                txtSliderOrder: {
                    required: true,
                    number: true,
                    min: 1,
                },
                txtSlideDescription: "required",
                txtGroupAlias: "required",
                txtSlideImage: "required",
            },
            messages: {
                txtslideName: {
                    required: 'Slide name is required'
                },
                txtSlideUrl: {
                    required: 'Url is required'
                },
                txtSliderOrder: {
                    required: 'Order is required',
                    number: 'Please enter correct slide order'
                },
                txtSlideDescription: {
                    required: 'Description is required'
                },
                txtGroupAlias: {
                    required: 'Alias is required'
                },
                txtSlideImage: {
                    required: 'Slide image is required'
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

        $('#btn-search-slide').on('click', function () {
            $('#pagination-slide').twbsPagination('destroy');
            common.config.pageIndex = 1;
            slide.loadData();
        });

        $('#txt-slide-keyword').on('keypress', function (e) {
            if (e.which == 13) {
                $('#pagination-slide').twbsPagination('destroy');
                common.config.pageIndex = 1;
                slide.loadData();
            }
        });

        $('#btn-create-slide').off('click').on('click', function () {
            $('#add_edit_slide').modal('show');
            slide.clearForm();
            validator.resetForm();
        });

        $('#btn-slide-image').off('click').on('click', function () {
            $('#btn-file-slide').val('');
            $('#btn-file-slide').click();
        });

        $('#btn-file-slide').on('change', function () {
            var file = $(this)[0].files[0];
            slide.imageData = file;

            $('#txt-slide-image').val('/admin-side/images/' + file.name);
            var url = window.URL || window.webkitURL;
            var src = url.createObjectURL(file);
            $('#blob-slide-img').attr('src', src);
            $('#blob-slide-img').attr('alt', file.name);
        });

        $('#btn-save-slide').off('click').on('click', function () {
            if (validator.form()) {
                var fd = new FormData();
                fd.append('files', slide.imageData);

                var slideVm = {};
                var id = $('#hidSlideId').val();
                if (id != '') {
                    var createdDate = $('#hidCreatedDate').val();
                    slideVm.Id = id;
                    slideVm.CreatedDate = createdDate;
                }
                slideVm.Name = $('#txt-slide-name').val();
                slideVm.Url = $('#txt-slide-url').val();
                slideVm.Image = $('#txt-slide-image').val();
                slideVm.Text = $('#txt-slide-description').val();
                slideVm.SortOrder = $('#txt-slide-order').val();
                slideVm.Status = $('#ck-slide-status').is(':checked') ? 1 : 0;
                slideVm.GroupAlias = $('#txt-slide-alias').val();

                for (var key in slideVm) {
                    fd.append(key, slideVm[key]);
                }
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Slide/SaveSlide',
                    data: fd,
                    processData: false,
                    contentType: false,
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        common.notify('Save slide success', 'success');
                        $('#add_edit_slide').modal('hide');
                        slide.loadData();
                    },
                    error: function (err) {
                        console.log(err);
                        common.notify('Error: ' + err, 'error');
                        common.stopLoadingIndicator();
                    }
                })
            }
        });

        $('body').on('click', '.btnUpdateSlide', function () {
            var slideId = $(this).attr('data-id');
            $('#hidSlideId').val(slideId);
            $.ajax({
                type: 'GET',
                url: '/Admin/Slide/GetSlideDetail',
                data: { id: slideId },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#add_edit_slide').modal('show');
                    $('#txt-slide-name').val(res.Name);
                    $('#txt-slide-url').val(res.Url);
                    $('#txt-slide-image').val(res.Image);
                    $('#blob-slide-img').attr('src', res.Image);
                    $('#blob-slide-img').attr('alt', res.Name);
                    $('#txt-slide-description').val(res.Text);
                    $('#txt-slide-order').val(res.SortOrder);
                    $('#ck-slide-status').prop('checked', res.Status);
                    $('#txt-slide-alias').val(res.GroupAlias);
                    $('#hidCreatedDate').val(res.CreatedDate);

                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    console.log(err);
                    common.notify('Error: ' + err, 'error');
                    common.stopLoadingIndicator();
                }
            });
        });

        $('body').on('click', '.btnDeleteSlide', function () {
            var slideId = $(this).data('id');
            common.confirm('Are you sure to remove this slide?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Slide/DeleteSlide',
                    data: { id: slideId },
                    success: function (res) {
                        common.notify('Removed success', 'success');
                        slide.loadData();
                    },
                    error: function (err) {
                        console.log(err);
                        common.notify('Error: ' + err, 'error');
                        common.stopLoadingIndicator();
                    }
                });
            })
        });
    },

    imageData: [],

    loadData: function () {
        var render = '';
        var template = $('#table-slide-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Slide/GetAllPaging',
            data: {
                page: common.config.pageIndex,
                keyword: $('#txt-slide-keyword').val(),
                pageSize: common.config.pageSizeSmall
            },
            beforeSend: common.runLoadingIndicator(),
            dataType: 'json',
            success: function (res) {
                $.each(res.Results, function (i, item) {
                    var obj = {};
                    obj.Id = item.Id;
                    obj.CreatedDate = common.formatDateTime(item.CreatedDate);
                    obj.Name = item.Name;
                    obj.Url = item.Url;
                    obj.Image = common.formatImageWithSize(item.Image, item.Name, 100);
                    obj.Description = item.Text;
                    obj.Status = common.getStatusLabel(item.Status);

                    render += Mustache.render(template, obj);
                });
                $('#lbl-total-slide').text(res.RowCount);
                $('#tbl-slide-content').html(render);
                if (res.RowCount > 0) {
                    slide.configPaging(res.RowCount);
                    slide.bindSlideToTree(res.Results)
                }
                common.stopLoadingIndicator();
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
                common.stopLoadingIndicator();
            }
        });
    },

    sliderGlobal: [],

    bindSlideToTree: function (data) {
        if (slide.sliderGlobal.length > 0) {
            slide.sliderGlobal.destroySlider();
        }
        var render = '';
        var template = $('#ul-slide-template').html();
        $.each(data, function (i, item) {
            if (item.Status === 0) {
                return;
            }
            var obj = {};
            obj.Src = item.Image;
            obj.Title = 'Name: ' + item.Name + ' - Created date: ' + common.formatDate(item.CreatedDate);
            obj.Name = item.Name;
            render += Mustache.render(template, obj);
        });
        $('.slide-bxslider').html(render);

        slide.sliderGlobal = $('.slide-bxslider').bxSlider({
            auto: true,
            mode: 'fade',
            captions: true,
            slideWidth: 800,
        });
    },

    configPaging: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSizeSmall);

        $('#pagination-slide').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    slide.loadData();
                }
            }
        });
    },

    clearForm: function () {
        $('#txt-slide-name').val('');
        $('#txt-slide-url').val('');
        $('#txt-slide-description').val('');
        $('#txt-slide-order').val(1);
        $('#txt-slide-image').val('');
        $('#blob-slide-img').attr('src', '/admin-side/images/default-image.png')
        $('#blob-slide-img').attr('alt', 'default-image')
    }
};
slide.init();