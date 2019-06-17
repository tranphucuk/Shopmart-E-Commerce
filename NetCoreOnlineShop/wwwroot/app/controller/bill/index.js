var bill = {
    init: function () {
        $.when(
            bill.loadProduct(),
            bill.loadPaymentMethod(),
            bill.loadBillStatus(),
            bill.loadColor(),
            bill.loadSize(),
        ).then(function () {
            bill.loadData();
            bill.registerEvents();
        });
    },

    registerEvents: function () {
        var validator = $('#frmBill').validate({
            errorClass: 'red',
            rules: {
                customerName: "required",
                customerAddress: "required",
                customerPhone: {
                    required: true,
                    number: true
                },
                customerEmail: {
                    required: true,
                    email: true,
                },
                paymentMethod: "required",
                billStatus: "required",
                customerMessage: "required",
            },
            messages: {
                customerName: {
                    required: 'Customer name is required'
                },
                customerAddress: {
                    required: 'Customer address is required'
                },
                customerPhone: {
                    required: 'Customer phone is required',
                    number: 'Please enter correct phone number'
                },
                paymentMethod: {
                    required: 'Payment method is required'
                },
                billStatus: {
                    required: 'Payment method is required'
                },
                customerMessage: {
                    required: 'Customer message is required'
                },
                customerEmail: {
                    required: "Email address is required",
                    email: "Please provide an accurate email address",
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
        $('input[name="date-range"]').daterangepicker({
            opens: 'left',
            autoUpdateInput: false,
            locale: {
                cancelLabel: 'Clear'
            },
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                'Last 6 Months': [moment().subtract(180, 'day'), moment().subtract(1, 'day')],
            },
        }, function (start_date, end_date) {
            $('input[name="date-range"]').val(start_date.format('M/D/YYYY') + '-' + end_date.format('M/D/YYYY'));
            bill.configPagination();
        });
        $('input[name="date-range"]').on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
            bill.configPagination();
        });

        $('#btnSearch').off('click').on('click', function () {
            bill.configPagination();
        });

        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                bill.configPagination();
            }
        })

        $('#btnCreate').on('click', function () {
            $('#add_edit_bill_detail_modal').modal('show');
            bill.commonConfig();
            bill.clearForm();
        });

        $('#btnAddDetail').on('click', function () {
            var template = $('#template-table-bill-details').html();
            var render = '';
            common.runLoadingIndicator();
            $.when(
                bill.loadProductToDropDown(),
                bill.loadSizeToDropDown(),
                bill.loadColorToDropDown(),
            ).then(function () {
                render = Mustache.render(template, {
                    ProductName: bill.productDropDownData,
                    Color: bill.colorDropDownData,
                    Size: bill.sizeDropDownData,
                    Quantity: 0,
                });
            });

            if (render != '') {
                $('#tbl-bill-details').append(render);
                bill.dropdownConfig();
                common.stopLoadingIndicator();
            }
        });

        $("#tbl-bill-details").on('click', '.btn-delete-detail', function () {
            $(this).closest('tr').remove();
        });

        $('#btnSave').on('click', function () {
            var id = $('#hidBill').val();
            if (validator.form()) {
                var billVm = {};
                if (id != undefined && id != '') {
                    billVm.Id = id;
                    billVm.CreatedDate = $('#txtCreatedDate').val();
                }
                billVm.CustomerName = $('#txtName').val();
                billVm.CustomerAddress = $('#txtAddress').val();
                billVm.CustomerPhone = $('#txtPhoneNumber').val();
                billVm.CustomerMessage = $('#txtMessage').val();
                billVm.CustomerEmail = $('#txtEmail').val();
                billVm.PaymentMethod = $('#ddlPaymentMethod').val();
                billVm.BillStatus = $('#ddlBillStatus').val();
                billVm.Status = $('#ckStatus').is(':checked') ? 1 : 0;
                billVm.billDetails = [];
                $('#tbl-bill').find('tbody tr').each(function (i, item) {
                    var billDetailVm = {};
                    if ($(item).attr('data-id') != '' && $(item).attr('data-id') != undefined) {
                        billDetailVm.Id = $(item).attr('data-id');
                        billDetailVm.BillId = billVm.Id;
                    }
                    billDetailVm.ProductId = $(item).find('.ddlProducts option:selected').val();
                    billDetailVm.Quantity = $(item).find('#txtQuantity').val();
                    billDetailVm.ColorId = $(item).find('.ddlColor option:selected').val();
                    billDetailVm.SizeId = $(item).find('.ddlSize option:selected').val();
                    billVm.billDetails.push(billDetailVm);
                });
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Bill/SaveBill',
                    data: { billViewModel: billVm },
                    beforeSend: common.runLoadingIndicator(),
                    success: function (res) {
                        if (res.Message == 'create') {
                            common.notify('Create bill succeeded', 'success');
                        } else {
                            common.notify('Update bill: ' + res.Data.CustomerName + ' succeeded', 'success');
                        }
                        bill.loadData();
                        common.stopLoadingIndicator();
                        $('#add_edit_bill_detail_modal').modal('hide');
                    },
                    error: function (err) {
                        console.log(err);
                        common.notify('Error: ' + err, 'error');
                        common.stopLoadingIndicator();
                    }
                });
            }
        });

        $('body').on('click', '.btnUpdateBill', function () {
            var billId = $(this).attr('data-id');
            $.ajax({
                type: 'GET',
                url: '/Admin/Bill/GetBillDetail',
                data: { id: billId },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    $('#hidBill').val(res.Id);
                    $('#add_edit_bill_detail_modal').modal('show');
                    bill.commonConfig(billId);

                    $('#txtName').val(res.CustomerName);
                    $('#txtAddress').val(res.CustomerAddress);
                    $('#txtPhoneNumber').val(res.CustomerPhone);
                    $('#txtMessage').val(res.CustomerMessage);
                    $('#txtEmail').val(res.CustomerEmail);
                    $('#ddlPaymentMethod').val(res.PaymentMethod);
                    $('#ddlBillStatus').val(res.BillStatus);
                    $('#txtCreatedDate').val(common.formatDateTime(res.CreatedDate));
                    $('#txtModifiedDate').val(common.formatDateTime(res.ModifiedDate));
                    $('#ckStatus').prop('checked', res.Status);
                    var render = '';
                    var template = $('#template-table-bill-details').html();
                    $.each(res.BillDetails, function (i, item) {
                        $.when(
                            bill.loadColorToDropDown(item.ColorId),
                            bill.loadProductToDropDown(item.ProductId),
                            bill.loadSizeToDropDown(item.SizeId),
                        ).then(function () {
                            render += Mustache.render(template, {
                                Id: item.Id,
                                ProductName: bill.productDropDownData,
                                Color: bill.colorDropDownData,
                                Size: bill.sizeDropDownData,
                                Quantity: item.Quantity
                            });
                        });
                    });
                    $("#tbl-bill-details").html(render);
                    bill.dropdownConfig();
                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    console.log(err);
                    common.notify('Error: ' + err, 'error');
                    common.stopLoadingIndicator();
                }
            });
        });

        $('body').on('click', '.btnDeleteBill', function () {
            var billId = $(this).attr('data-id');
            common.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Bill/DeleteBill',
                    data: { id: billId },
                    success: function (res) {
                        bill.loadData();
                        common.notify('Removed: ' + res.Data.CustomerName, 'success');
                    },
                    error: function (err) {
                        console.log(err);
                        common.notify('Error: ' + err, 'error');
                    }
                });
            });
        });

        $('#btnExportExcel').off('click').on('click', function () {
            var id = $('#hidBill').val();
            $.ajax({
                type: 'POST',
                url: '/Admin/Bill/ExportBill',
                data: { billId: id },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    window.location.href = res;
                    common.stopLoadingIndicator();
                },
                error: function (err) {
                    console.log(err);
                    common.notify('Error: ' + err, 'error');
                }
            });
        });

        $('#ddl-bill-status').on('change', function () {
            bill.configPagination();
        });

        $('#ddlShowPage').on('change', function () {
            common.config.pageSize = $('#ddlShowPage :selected').text();
            common.config.pageIndex = 1;
            $('#pagination-bill').twbsPagination('destroy');
            bill.loadData();
        });
    },

    productData: [],
    productDropDownData: [],
    colorData: [],
    colorDropDownData: [],
    sizeData: [],
    sizeDropDownData: [],

    commonConfig: function (billId) {
        $('.hideBorder').val('Add/Edit Bill Detail');
        if (billId == null) {
            $('#frmModifiedDate').hide();
            $('#frmCreatedDate').hide();
        }
        else {
            $('#frmModifiedDate').show();
            $('#frmCreatedDate').show();
        }
    },

    configPagination: function () {
        common.config.pageSize = $('#ddlShowPage :selected').text();
        common.config.pageIndex = 1;
        $('#pagination-bill').twbsPagination('destroy');
        bill.loadData();
    },

    clearForm: function () {
        $('#txtName').val('');
        $('#txtAddress').val('');
        $('#txtPhoneNumber').val('');
        $('#txtMessage').val('');
        $('#txtEmail').val('');
        $('#ddlPaymentMethod').val(0);
        $('#ddlBillStatus').val(0);
        $("#tbl-bill-details").empty();
    },

    dropdownConfig: function () {
        $(".ddlProducts").chosen({
            disable_search_threshold: 10,
            no_results_text: "Oops, nothing found!",
            width: "95%"
        });
        $(".ddlSize").chosen({
            disable_search_threshold: 10,
            no_results_text: "Oops, nothing found!",
            width: "95%"
        });
        $(".ddlColor").chosen({
            disable_search_threshold: 10,
            no_results_text: "Oops, nothing found!",
            width: "95%"
        });
    },

    loadSize: function () {
        return $.ajax({
            // async: false,
            type: 'GET',
            url: '/Admin/Bill/GetAllSize',
            success: function (res) {
                bill.sizeData = res;
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
            }
        });
    },

    loadSizeToDropDown: function (sizeId) {
        var select = '<select class="form-control ddlSize">';
        $.each(bill.sizeData, function (i, item) {
            if (sizeId == item.Id) {
                select += '<option value="' + item.Id + '" selected="selected">' + item.Name + '</option>';
            }
            else {
                select += '<option value="' + item.Id + '">' + item.Name + '</option>';
            }
        });
        bill.sizeDropDownData = select + '</select>';
    },

    loadColor: function () {
        return $.ajax({
            //async: false,
            type: 'GET',
            url: '/Admin/Bill/GetAllColor',
            success: function (res) {
                bill.colorData = res;
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
            }
        });
    },

    loadColorToDropDown: function (ColorId) {
        var select = '<select class="form-control ddlColor">';
        $.each(bill.colorData, function (i, item) {
            if (item.Id == ColorId) {
                select += '<option value="' + item.Id + '" selected="selected">' + item.Name + '</option>';
            } else {
                select += '<option value="' + item.Id + '">' + item.Name + '</option>';
            }
        });
        bill.colorDropDownData = select + '</select>';
    },

    loadProduct: function () {
        return $.ajax({
            type: 'GET',
            url: '/Admin/Product/GetAllForBill',
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                bill.productData = res;
                common.stopLoadingIndicator();
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
                common.stopLoadingIndicator();
            }
        });
    },

    loadProductToDropDown: function (ProductId) {
        var select = '<select class="form-control ddlProducts">';
        $.each(bill.productData, function (i, item) {
            if (item.Id == ProductId) {
                select += '<option value="' + item.Id + '" selected="selected">' + item.Name + '</option>';
            } else {
                select += '<option value="' + item.Id + '">' + item.Name + '</option>';
            }
        });
        bill.productDropDownData = select + '</select>';
    },

    loadData: function () {
        var render = '';
        var template = $('#table-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Bill/GetAllBillPaging',
            data: {
                pageSize: common.config.pageSize,
                page: common.config.pageIndex,
                option: $('#ddl-bill-status').val(),
                fromDate: $('#txt-date-range').val().split('-')[0],
                toDate: $('#txt-date-range').val().split('-')[1],
                keyword: $('#txtKeyword').val()
            },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res.Results, function (i, item) {
                    var obj = {};
                    obj.Id = item.Id;
                    obj.CustomerName = item.CustomerName;
                    obj.PaymentMethod = common.definePaymentMethod(item.PaymentMethod);
                    obj.BillStatus = common.definePaymentStatus(item.BillStatus);
                    obj.OrderDate = common.formatDateTime(item.CreatedDate);
                    obj.Status = common.getStatusLabel(item.Status);
                    obj.Id = item.Id;

                    render += Mustache.render(template, obj);
                    if (render != null && render != '') {
                        $('#tbl-content').html(render);
                    }
                });
                if (render == '') {
                    $('#tbl-content').empty();
                }
                if (res.RowCount > 0) {
                    bill.pagination(res.RowCount);
                }
                $('#lblTotalRecords').html(res.RowCount);
                common.stopLoadingIndicator();
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
                common.stopLoadingIndicator();
            }
        });
    },

    loadPaymentMethod: function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Bill/GetPaymentMethod',
            success: function (res) {
                var options = [];
                $.each(res, function (i, item) {
                    options.push('<option value="' + item.Value + '">' + item.Name + '</option>')
                });
                $('#ddlPaymentMethod').html(options);
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
            }
        });
    },

    loadBillStatus: function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Bill/GetBillStatus',
            success: function (res) {
                var options = [];
                $.each(res, function (i, item) {
                    options.push('<option value="' + item.Value + '">' + item.Name + '</option>')
                });
                $('#ddlBillStatus').html(options);
            },
            error: function (err) {
                console.log(err);
                common.notify('Error: ' + err, 'error');
            }
        });
    },

    pagination: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-bill').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    bill.loadData();
                }
            }
        });
    }
};
bill.init();