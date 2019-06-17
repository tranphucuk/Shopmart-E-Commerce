var ticket = {
    init: function () {
        this.loadData();
        this.registerEvents();
    },

    registerEvents: function () {
        $('#ddlShowPage').on('change', function () {
            common.config.pageSize = $('#ddlShowPage :selected').text();
            common.config.pageIndex = 1;
            $('#pagination-ticket').twbsPagination('destroy');
            ticket.loadData();
        });

        $('#txt-ticket-keyword').on('keypress', function (e) {
            if (e.which === 13) {
                $('#btn-search-ticket').click();
            }
        });

        $('#btn-search-ticket').off('click').on('click', function () {
            $('#pagination-ticket').twbsPagination('destroy');
            $('#ddlShowPage').prop('selectedIndex', 0);
            common.config.pageIndex = 1;
            ticket.loadData();
        });

        $('body').on('click', '.btnUpdateTicket', function () {
            var id = $(this).attr('data-id');
            ticket.loadDetail(id);
        });

        $('#btn-save-ticket').on('click', function () {
            ticket.updateTicket();
        });
    },

    loadData: function () {
        var render = '';
        var template = $('#tbl-ticket-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Ticket/GetAllPaging',
            data: {
                page: common.config.pageIndex,
                pageSize: common.config.pageSize,
                keyword: $('#txt-ticket-keyword').val(),
            },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $.each(res.Results, function (i, item) {
                    var st = {};
                    st.Id = item.Id;
                    st.BillId = item.BillId;
                    st.Title = item.Title;
                    st.Email = item.Email;
                    st.CreatedDate = common.formatDateTime(item.CreatedDate);
                    st.Status = common.getStatusTicket(item.Status);

                    render += Mustache.render(template, st);
                });
                $('#tbl-ticket-content').html(render);
                $('#lbl-total-ticket').html(res.RowCount);
                if (res.RowCount > 0) {
                    ticket.pagination(res.RowCount);
                }
                common.stopLoadingIndicator();
            }
        });
    },

    loadDetail: function (ticketId) {
        $.ajax({
            type: 'GET',
            url: '/Admin/Ticket/LoadDetail',
            data: { id: ticketId },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                $('#txt-bill-number').val(res.BillId);
                $('#txt-ticket-email').val(res.Email);
                $('#txt-ticket-title').val(res.Title);
                $('#txt-ticket-message').val(res.Content);
                $('#txt-ticket-date').val(common.formatDateTime(res.CreatedDate));
                $('#ckStatus').prop('checked', res.Status);

                $('#hid-ticket-id').val(ticketId);
                $('#view-ticket-modal').modal('show');
                common.stopLoadingIndicator();
            }
        });
    },

    updateTicket: function () {
        var st = {};
        st.id = $('#hid-ticket-id').val();
        st.BillId = $('#txt-bill-number').val();
        st.Email = $('#txt-ticket-email').val();
        st.Title = $('#txt-ticket-title').val();
        st.Content = $('#txt-ticket-message').val();
        st.CreatedDate = $('#txt-ticket-date').val();
        st.Status = $('#ckStatus').is(':checked') ? 1 : 0;

        $.ajax({
            type: 'POST',
            url: '/Admin/Ticket/UpdateTicket',
            data: {
                ticket: st,
            },
            beforeSend: common.runLoadingIndicator(),
            success: function (res) {
                if (res == true) {
                    common.notify('Update ticket success', 'success');
                    $('#view-ticket-modal').modal('hide');
                    common.stopLoadingIndicator();
                    ticket.loadData();
                } else {
                    common.notify('Update ticket failed', 'error');
                }
            }
        });
    },

    pagination: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-ticket').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    ticket.loadData();
                }
            }
        });
    },
};
ticket.init();