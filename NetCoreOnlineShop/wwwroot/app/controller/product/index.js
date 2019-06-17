/// <reference path="../../../lib/jquery/dist/jquery.js" />
var product = {
    init: function () {
        this.loadCategory();
        this.registerEvents();
        this.loadData();
        this.loadCkEditor();
    },

    registerEvents: function () {
        var validator = $("#frmProduct").validate({
            errorClass: 'red',
            rules: {
                productName: "required",
                ddlCategory: "required",
                loadFileExcel: {
                    required: true,
                    extension: "xlsx|xls|csv"
                },
                price: {
                    required: true,
                    number: true,
                    min: 0
                },
                promotionPrice: {
                    number: true,
                    min: 0
                },
                originalPrice: {
                    required: true,
                    number: true,
                    min: 0
                },
                productDescription: "required",
                productContent: "required",
                Unit: {
                    required: true,
                    number: true,
                    min: 0
                },
                productTag: "required",
                productImage: "required",
            },
            messages: {
                price: {
                    required: "Price must has value",
                    number: "Price must be a number",
                    min: "Price must be a positive number",
                },
                promotionPrice: {
                    required: "Promotion Price must has value",
                    number: "Promotion Price must be a number",
                    min: "Promotion Price must be a positive number",
                },
                originalPrice: {
                    required: "Original Price must has value",
                    number: "Original Price must be a number",
                    min: "Original Price must be a positive number",
                },
                productName: {
                    required: "Product name is required"
                },
                productContent: {
                    required: "Content is required"
                },
                ddlCategory: {
                    required: "Chooese a category"
                },
                productDescription: {
                    required: "Product description is required"
                },
                productTag: {
                    required: "Product tags is required"
                },
                productImage: {
                    required: "Product image is required"
                },
                Unit: {
                    required: "Product tags is required",
                    number: 'Unit must be a number',
                    min: 'unit must be equal or greater than 0'
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
            $('#paginationUL').twbsPagination('destroy');
            product.loadData();
        });

        $('#btnSearch').on('click', function () {
            $('#paginationUL').twbsPagination('destroy');
            $('#ddlShowPage').prop('selectedIndex', 0);
            common.config.pageIndex = 1;
            common.config.pageSize = 10;
            product.loadData();
        });

        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                $('#btnSearch').click();
            }
        });

        $('#btnCreate').off('click').on('click', function () {
            product.loadParent();
            $('#add_edit_Product').modal('show');
            product.clearForm();
            validator.resetForm();
        });

        $('#btn-save-product').on('click', function () {
            if (validator.form()) {
                var productDetails = {};
                if ($('#hidProductId').val() != 0) {
                    productDetails.Id = $('#hidProductId').val();
                    productDetails.CreatedDate = $('#txtCreatedDate').val();
                }
                productDetails.Name = $('#txtProductName').val();
                productDetails.CategoryId = $('#ddlproductCategory').val();
                productDetails.Price = $('#txtProductPrice').val();
                productDetails.PromotionPrice = $('#txtPromotionPrice').val();
                productDetails.OriginalPrice = $('#txtOriginalPrice').val();
                productDetails.Image = $('#txtProductImg').val();
                productDetails.Description = $('#txtProductDescription').val();
                productDetails.Content = CKEDITOR.instances.txtProductContent.getData();
                productDetails.Unit = $('#txtProductUnit').val();
                productDetails.ViewCount = $('#txtProductViewCount').val();
                productDetails.Tags = $('#txtProductTag').val();
                productDetails.Status = $('#ckStatus').is(':checked') ? 1 : 0;
                productDetails.HomeFlag = $('#ckShowHome').is(':checked') ? 1 : 0;
                productDetails.HotFlag = $('#ckHotProduct').is(':checked') ? 1 : 0;

                $.ajax({
                    type: 'POST',
                    url: '/Admin/Product/SaveProduct',
                    data: {
                        productVm: productDetails,
                    },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        $('#add_edit_Product').modal('hide');
                        product.loadData(true);
                        if (res.HttpType == 'update') {
                            common.notify('Updated product: ' + res.ObjReturn.Name, 'success');
                        }
                        else {
                            common.notify('Created success product: ' + res.ObjReturn.Name, 'success');
                        }
                        common.stopLoadingIndicator();
                    },
                    error: function (err) {
                        common.notify('An error has occurred');
                        common.stopLoadingIndicator();
                    }
                });
            }
        });

        $('body').on('click', '.btnUpdateProduct', function () {
            var productId = $(this).attr('data-id');
            $.ajax({
                type: 'GET',
                url: '/Admin/Product/GetProductDetail',
                data: { id: productId },
                dataType: "json",
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#add_edit_Product').modal('show');

                    $('#hidProductId').val(res.Id);
                    $('#txtProductName').val(res.Name);
                    product.loadParent(res.CategoryId);
                    $('#txtProductPrice').val(res.Price);
                    $('#txtPromotionPrice').val(res.PromotionPrice);
                    $('#txtOriginalPrice').val(res.OriginalPrice);
                    $('#txtProductImg').val(res.Image);
                    $('#txtProductDescription').val(res.Description);
                    CKEDITOR.instances['txtProductContent'].setData(res.Content);
                    $('#txtProductUnit').val(res.Unit);
                    $('#txtProductViewCount').val(res.ViewCount);
                    $('#txtProductTag').val(res.Tags);
                    $('#frmCreatedDate').show();
                    $('#frmModifiedDate').show();
                    $('#txtModifiedDate').val(common.formatDateTime(res.ModifiedDate));
                    $('#txtCreatedDate').val(common.formatDateTime(res.CreatedDate));
                    $('#ckStatus').prop('checked', res.Status);
                    $('#ckShowHome').prop('checked', res.HomeFlag);
                    $('#ckHotProduct').prop('checked', res.HotFlag);

                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    common.notify('Error loading product detail.', 'error');
                    console.log(err);
                    common.stopLoadingIndicator();
                }

            });
        });

        $('body').on('click', '.btnDeleteProduct', function (e) {
            e.preventDefault();
            var productId = $(this).attr('data-id');
            common.confirm('Are you sure to delete?', function () {
                $.ajax({
                    url: '/Admin/Product/Delete',
                    type: 'POST',
                    data: {
                        id: productId
                    },
                    beforeSend: common.runLoadingIndicator(),
                    success: function () {
                        common.notify('Deleted success', 'success');
                        product.loadData(true);
                        common.stopLoadingIndicator();
                    },
                    error: function (err) {
                        common.notify('An error has occurred. Please check console tab', 'error');
                        product.loadData(true);
                        console.log(err);
                        common.stopLoadingIndicator();
                    }
                });
            });
        });

        $('body').on('click', '#checkAll', function (e) {
            var isChecked = $('#checkAll').is(':checked');
            $('#tbl-content input[type="checkbox"]').prop('checked', isChecked);
        });

        $('body').on('click', '.check-product-single', function () {
            var totalCheckbox = $('#tbl-content input[type="checkbox"]').length;
            var numberChecked = $('#tbl-content input[type="checkbox"]:checked').length;
            if (numberChecked === totalCheckbox) {
                $('#checkAll').prop('checked', true);
            } else {
                $('#checkAll').prop('checked', false);
            }
        });

        $('#btnSelectImg').off('click').on('click', function () {
            $('#fileProductImage').click();
        });

        $('#fileProductImage').on('change', function (e) {
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
                        $('#txtProductImg').val(res.ImgPath);
                        common.notify('Saved an image name: "  ' + res.FileName + '  " to root folder', 'success');
                    },
                    error: function (err) {
                        common.notify('An error has occurred, please check console', 'error');
                        console.log(err);
                    }
                });
            });
        });

        $('#btnDeleteMulti').off('click').on('click', function () {
            var checkedRows = [];
            $('.check-product-single:checked').each(function () {
                checkedRows.push($(this).attr('data-id'));
            });
            common.confirm('Are you sure to delete ' + checkedRows.length + ' products?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Product/DeleteMulti',
                    data: { ids: JSON.stringify(checkedRows) },
                    dataType: 'json',
                    success: function (res) {
                        common.notify('Deleted success ' + res.length + ' products', 'success');
                        product.loadData(true);
                    },
                    error: function () {
                        common.notify('Incorrect formart product details', 'error');
                        product.loadData(true);
                        console.log(err);
                    }
                });
            });
        });

        $('#btnExcelProduct').off('click').on('click', function () {
            product.loadParentExcel();
        });

        $('#btnSaveExcel').off('click').on('click', function () {
            var fileUpload = $("#producstExcelFile")[0].files[0];
            var fileData = new FormData();
            fileData.append('files', fileUpload);
            fileData.append('categoryId', $('#ddlLoadCategory').val());
            $.ajax({
                type: 'POST',
                url: '/Admin/Product/ImportExcel',
                data: fileData,
                processData: false,
                contentType: false,
                success: function (res) {
                    common.notify('Imported success ' + res + ' new products', 'success');
                    $('#import_excel_product').modal('hide');
                    product.loadData(true);
                },
                error: function (err) {
                    common.notify('Incorrect format product details', 'error');
                    product.loadData(true);
                    console.log(err);
                }
            });
        });

        $('#btn-save-Products').on('click', function () {
            $.ajax({
                type: 'POST',
                url: '/Admin/Product/ExportExcel',
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    window.location.href = res;
                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    common.notify('Error: ' + err, 'error');
                    common.stopLoadingIndicator();
                }
            });
        });

        $('body').on('click', '.btnProductOption', function (e) {
            e.preventDefault();
            $.contextMenu({
                selector: '.btnProductOption',
                trigger: 'left',
                callback: function (key, options) {
                    var id = $(this).attr('data-id');
                    $('#productId').val(id);
                    switch (key) {
                        case 'Quantity':
                            quantity.loadData(id);
                            break;
                        case 'Price':
                            price.loadData(id);
                            break;
                        case 'Image':
                            image.loadData(id);
                            break;
                        default:
                    }
                },
                items: {
                    "Quantity": { name: "Quantity", icon: "fa-tasks" },
                    "Price": { name: "Price", icon: "fa-money" },
                    "Image": { name: "Images", icon: "fa-picture-o" },
                    "sep1": "---------",
                    "quit": { name: "Quit", icon: 'context-menu-icon context-menu-icon-quit' }
                }
            });
        });
    },

    category: [],
    errors: '',

    loadParentExcel: function (id) {
        var treArr = common.unflattern(product.category);

        $('#ddlLoadCategory').combotree({
            data: treArr,
        })
        $('#import_excel_product').modal('show');
    },

    loadCkEditor: function () {
        CKEDITOR.replace('productContent', {
            language: 'en',
            width: '765px'
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
        $('#hidProductId').val(0);
        $('#txtProductName').val('');
        $('#ddlproductCategory').val('');
        $('#txtProductPrice').val(0);
        $('#txtPromotionPrice').val(0);
        $('#txtOriginalPrice').val(0);
        $('#txtProductImg').val('');
        $('#txtProductDescription').val('');
        CKEDITOR.instances['txtProductContent'].setData('');
        $('#txtProductUnit').val('0');
        $('#txtProductViewCount').val(0);
        $('#txtProductTag').val('');
        $('#frmCreatedDate').hide();
        $('#frmModifiedDate').hide();
        $('#ckStatus').prop('checked', true);
        $('#ckShowHome').prop('checked', false);
        $('#ckHotProduct').prop('checked', false);
    },

    loadParent: function (id) {
        var treArr = common.unflattern(product.category);

        $('#ddlproductCategory').combotree({
            data: treArr
        });

        $('#ddlCategory').combotree({
            data: treArr,
        })

        if (id != null) {
            $('#ddlproductCategory').combotree('setValue', id);
        }
    },

    loadCategory: function () {
        $.ajax({
            url: '/Admin/Product/GetAllCategories',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                $.each(res, function (i, item) {
                    product.category.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                product.loadParent();
            },
            error: function (err) {
                common.notify('Error loading category', 'error');
                console.log(err);
            }
        });
        //var render = '<option class="disabled" value="">---Category---</option>';
        //$.ajax({
        //    type: 'GET',
        //    url: '/Admin/Product/GetAllCategories',
        //    dataType: 'json',
        //    success: function (response) {
        //        $.each(response, function (i, item) {
        //            render += '<option value= "' + item.Id + '"> ' + item.Name + '</option>'
        //        });
        //        $('#ddlCategory').html(render);
        //    },
        //    error: function (status) {
        //        console.log(status);
        //        notify('Error loading data', 'error');
        //    }
        //});
    },

    loadData: function () {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/Admin/Product/GetAllPaging',
            data: {
                categoryId: $('#ddlCategory').val(),
                keyword: $('#txtKeyword').val(),
                page: common.config.pageIndex,
                pageSize: common.config.pageSize
            },
            beforeSend: common.runLoadingIndicator(),
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Category: item.ProductCategory.Name,
                        Price: common.formartNumber(item.Price, '0,0'),
                        Image: common.formatImage(item.Image, item.Name),
                        CreatedDate: common.formatDateTime(item.CreatedDate),
                        Status: common.getStatusLabel(item.Status)
                    });
                });
                $('#lblTotalRecords').text(response.RowCount);
                $('#tbl-content').html(render);
                product.config();
                if (response.RowCount > 0) {
                    product.wrapPaging(response.RowCount);
                }
                common.stopLoadingIndicator();
            },
            error: function (status) {
                console.log(status);
                common.notify('Error loading data', 'error');
                common.stopLoadingIndicator();
            }
        });
    },

    config: function () {
        $('#checkAll').prop('checked', false);
    },

    wrapPaging: function (recordCount) {
        var totalPage = Math.ceil(recordCount / common.config.pageSize);

        $('#paginationUL').twbsPagination({
            totalPages: totalPage,
            visiblePages: 7,
            onPageClick: function (event, page) {
                if (common.config.pageIndex !== page) {
                    common.config.pageIndex = page;
                    product.loadData();
                }
            }
        });
    }
}
product.init();
