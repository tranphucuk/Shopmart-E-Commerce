var productCategory = {
    init: function () {
        this.loadData();
        this.registerEvents();
    },

    registerEvents: function () {
        var validator = $('#frmProductCategory').validate({
            errorClass: 'red',
            rules: {
                categoryName: "required",
                categoryDescription: "required",
                categoryOrder: {
                    required: true,
                    number: true,
                    min: 1
                },
                categorySeoTitle: "required",
                categoryKeyword: "required",
            },
            messages: {
                categoryName: {
                    required: 'Category name is required'
                },
                categoryDescription: {
                    required: 'Category description is required'
                },
                categoryOrder: {
                    required: 'Order is required',
                    number: 'Order must be a number',
                    min: 'Order must be equal or greater than 1'
                },
                categorySeoTitle: {
                    required: 'Seo title is required'
                },
                categoryKeyword: {
                    required: 'Seo keyword is required'
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

        $('#btnCreate').off('click').on('click', function () {
            $.when(
                productCategory.GetParent()
            ).then(function () {
                $('#add_edit_modal').modal('show');
                $('.hideBorder').val('Create Category');
                productCategory.clearForm();
            });
        });

        $('body').on('click', '#btnRemove', function (e) {
            e.preventDefault();
            var that = $('#hidCategory').val();
            common.confirm('Are you sure to delete ?', function () {
                $.ajax({
                    url: '/Admin/ProductCategory/Delete',
                    type: 'POST',
                    data: {
                        id: that
                    },
                    success: function (res) {
                        common.notify('Removed category success.', 'success');
                        productCategory.loadData();
                    },
                    error: function (error) {
                        common.notify('An error has occurred. Please check console', 'error');
                        console.log(error);
                        productCategory.loadData();
                    }
                });
            })
        });

        $('body').on('click', '#btnUpdate', function (e) {
            e.preventDefault();
            var that = $('#hidCategory').val();
            $.ajax({
                url: '/Admin/ProductCategory/GetById',
                type: 'GET',
                data: {
                    id: that
                },
                dataType: 'json',
                beforeSend: function () {
                    common.runLoadingIndicator()
                },
                success: function (res) {
                    $.when(
                        productCategory.GetParent(res.ParentId)
                    ).then(function () {
                        $('#add_edit_modal').modal('show');
                        $('#txtAlias').val(res.SeoAlias);
                        $('#hidCategory').val(res.Id);
                        $('.hideBorder').val('Update Category: ' + res.Name);
                        $('#txtName').val(res.Name);
                        $('#txtDescription').val(res.Description);
                        $('#txtOrder').val(res.SortOrder);
                        $('#txtImg').val(res.Image);
                        $('#txtSeoDescription').val(res.SeoDescription);
                        $('#txtSeoKeyword').val(res.SeoKeywords);
                        $('#txtSeoTitle').val(res.SeoPageTitle);
                        $('#ckStatus').prop('checked', res.Status);
                        $('#ckShowHome').prop('checked', res.HomeFlag);
                        $('#frmCreatedDate').show();
                        $('#frmModifiedDate').show();
                        $('#txtModifiedDate').val(common.formatDateTime(res.ModifiedDate));
                        $('#txtCreatedDate').val(common.formatDateTime(res.CreatedDate));
                        common.stopLoadingIndicator();
                    });
                },
                error: function (res) {
                    common.notify('An error has occured', "error");
                    common.stopLoadingIndicator();
                }
            });
        })

        $('#btnSave').off('click').on('click', function () {
            if (validator.form()) {
                var category = {};
                if ($('#hidCategory').val() != '') {
                    category.Id = $('#hidCategory').val();
                    category.CreatedDate = $('#txtCreatedDate').val();
                }
                category.Name = $('#txtName').val();
                category.ParentId = $('#ddlCategory').val();
                category.Description = $('#txtDescription').val();
                category.SortOrder = $('#txtOrder').val();
                category.Image = $('#txtImg').val();
                category.SeoDescription = $('#txtSeoDescription').val();
                category.SeoKeywords = $('#txtSeoKeyword').val();
                category.SeoPageTitle = $('#txtSeoTitle').val();
                category.HomeFlag = $('#ckShowHome').is(":checked") ? 1 : 0;
                category.Status = $('#ckStatus').is(":checked") ? 1 : 0;
                $.ajax({
                    url: '/Admin/ProductCategory/SaveCategory',
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        categoryVm: category
                    },
                    success: function (res) {
                        common.notify('Save success category: ' + res.Name, 'success');
                        $('#add_edit_modal').modal('hide');
                        productCategory.loadData();
                    },
                    error: function (error) {
                        common.notify('Error: ' + error.Message, 'error');
                        productCategory.loadData();
                    }
                });
            }
        });

        $('#btn-tree-switch').off('click').on('click', function () {
            productCategory.toggle = !productCategory.toggle;
            if (productCategory.toggle == true) {
                $('#treeProductCategory').tree('collapseAll');
                $('#btn-tree-switch').text('Expand');
            }
            else {
                $('#treeProductCategory').tree('expandAll');
                $('#btn-tree-switch').text('Collapse');
            }
        });

        $('#btnSelectImg').off('click').on('click', function () {
            $('#categoryImage').val('');
            $('#categoryImage').click();
        });

        $('#categoryImage').on('change', function () {
            var image = $(this)[0].files;
            var fd = new FormData();
            fd.append(image.Filename, image[0]);
            $.ajax({
                type: 'POST',
                url: '/Admin/ProductCategory/UploadCategoryImage',
                data: fd,
                contentType: false,
                processData: false,
                success: function (res) {
                    common.notify('Uploaded success image: ' + image[0].name, 'success');
                    $('#txtImg').val(res);
                },
                error: function (err) {
                    common.notify('Error: ' + err, 'error');
                    console.log(err);
                }
            });
        });
    },

    toggle: false,

    clearForm: function () {
        $('#txtName').val('');
        $('#txtAlias').val('');
        $('#txtDescription').val('');
        $('#txtOrder').val(1);
        $('#txtImg').val('');
        $('#txtSeoDescription').val('');
        $('#txtSeoKeyword').val('');
        $('#txtSeoTitle').val('');
        $('#ckStatus').prop('checked', true);
        $('#ckShowHome').prop('checked', false);
        $('#frmCreatedDate').hide();
        $('#frmModifiedDate').hide();
        $('#txtCreatedDate').val(common.formatDateTime(new Date()));
    },

    GetParent: function (id) {
        return $.ajax({
            type: 'GET',
            url: '/Admin/ProductCategory/GetAll',
            dataType: 'json',
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                var data = [];
                $.each(res, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    })
                });
                var treeArr = common.unflattern(data);
                $('#ddlCategory').combotree({
                    data: treeArr
                });
                if (id != null) {
                    $('#ddlCategory').combotree('setValue', id);
                }
                common.stopLoadingIndicator();
            }
        });
    },

    loadData: function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/ProductCategory/GetAll',
            dataType: 'json',
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                var data = [];
                $.each(res, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    })
                });

                var treeArr = common.unflattern(data);
                $('#treeProductCategory').tree({
                    data: treeArr,
                    animate: true,
                    lines: true,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        // select the node
                        $('#tt').tree('select', node.target);
                        $('#hidCategory').val(node.id);       // Assign categoryId value to hidden field
                        // display context menu
                        $('#contextMenu').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    onDrop: function (target, source, point) {
                        var targetNode = $(this).tree('getNode', target);
                        if (point === 'append') {
                            var children = [];
                            $.each(targetNode.children, function (i, item) {
                                children.push({
                                    key: item.id,
                                    value: i
                                });
                            });
                            //update to database
                            $.ajax({
                                url: '/Admin/ProductCategory/UpdateParentId',
                                type: 'POST',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                dataType: 'json',
                                success: function (res) {
                                    productCategory.loadData();
                                }
                            });
                        }
                        else if (point === 'top' || point === 'bottom') {
                            $.ajax({
                                url: '/Admin/ProductCategory/ReOrder',
                                type: 'POST',
                                data: {
                                    point: point,
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                dataType: 'json',
                                success: function (res) {
                                    productCategory.loadData();
                                }
                            });
                        }
                    },
                    formatter: function (node) {
                        var s = node.text;
                        if (node.children) {
                            s += '&nbsp;<span style=\'color:blue\'>(' + node.children.length + ')</span>';
                        }
                        return s;
                    }
                });
                $('#treeProductCategory').tree('collapseAll');
                //$('#btn-tree-switch').text('Collapse');
                productCategory.toggle = true;
                common.stopLoadingIndicator();
            }
        });
    }
};
productCategory.init();