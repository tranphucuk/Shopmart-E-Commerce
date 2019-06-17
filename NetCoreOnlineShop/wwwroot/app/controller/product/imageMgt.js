var image = {
    init: function () {
        this.registerEvents();
    },

    registerEvents: function () {


        $('body').on('click', '.btn-add-images', function () {
            $('.btn-upload-img').val('');
            $('.btn-upload-img').click();
        });

        $('body').on('change', '.btn-upload-img', function (e) {
            image.imgData = [];
            for (var i = 0; i < this.files.length; i++) {
                image.imgData.push(this.files[i]);
            }
            image.imgPreview(this);
        });

        $('body').on('click', '.btn-remove-quantity', function () {
            var imageId = $(this).attr('data-id');
            var selector = this;
            if (imageId !== '') {
                common.confirm("Are you sure to delete?", function () {
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/Product/DeleteImage',
                        data: { id: imageId },
                        success: function (res) {
                            common.notify('Removed image success', 'success');
                            $(selector).closest('tr').remove();
                        },
                        error: function (err) {
                            console.log(err);
                            common.notify('Error: ' + err, 'error');
                        }
                    });
                });
            } else {
                $(this).closest('tr').remove();
            }
        });

        $('.btn-start-upload').on('click', function () {
            let productId = $('#productId').val();
            let imgsTosave = [];
            image.getImagesToSave(imgsTosave);

            let formData = new FormData();
            for (var i = 0; i < imgsTosave.length; i++) {
                formData.append(imgsTosave.name, imgsTosave[i]);
            }
            formData.append('id', productId);

            $.ajax({
                type: 'POST',
                url: '/Admin/Product/SaveImage',
                data: formData,
                processData: false,
                contentType: false,
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#product-image-manager').modal('hide');
                    common.notify('Upload image success', 'success');
                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    console.log(err);
                    common.notify('Maximum 10 images');
                    common.stopLoadingIndicator();
                }
            });
        });
    },

    imgData: [],

    getImagesToSave: function (imgsTosave) {
        $('#tbl-body-images').find('tr').each(function (i, item) {
            for (var i = 0; i < image.imgData.length; i++) {
                if ($(item).find('img').attr('alt') === image.imgData[i].name && $(item).attr('data-id') === '') {
                    imgsTosave.push(image.imgData[i]);
                }
            }
        });
    },

    imgPreview: function (input) {
        var render = '';
        var template = $('#template-product-image-management').html();
        var files = input.files;
        for (i = 0; i < files.length; i++) {
            window.URL = window.URL || window.webkitURL;
            var blobURL = window.URL.createObjectURL(files[i]);
            var obj = {
                Image: common.formatImage(blobURL, files[i].name),
                Size: (files[i].size * 0.000001).toFixed(3),
                Status: common.getStatusUpload(0)
            }
            render += Mustache.render(template, obj);
        }
        $('#tbl-body-images').append(render);
    },

    loadData: function (productId) {
        $('#productId').val(productId);
        var render = '';
        var template = $('#template-product-image-management').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Product/Images',
            data: { id: productId },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res, function (i, item) {
                    var obj = {
                        Id: item.Id,
                        Image: common.formatImage(item.Path, item.Caption),
                        Size: item.Size,
                        Status: common.getStatusUpload(item.Status)
                    }
                    render += Mustache.render(template, obj);
                });
                if (render !== '') {
                    $('#tbl-body-images').html(render);
                } else {
                    $('#tbl-body-images').empty();
                }
                $('#product-image-manager').modal('show');
                $('#total-image').text('Total: ' + res.length)
                common.stopLoadingIndicator();
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
                common.stopLoadingIndicator();
            },
        });
    }
};
image.init();