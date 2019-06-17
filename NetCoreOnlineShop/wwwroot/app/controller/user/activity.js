var activity = {
    init: function () {
        $.when(
            activity.loadRoleToDropDown()
        ).then(function () {
            activity.loadDataUser();
            activity.registerEvents();
        });
    },

    registerEvents: function () {
        $('#ddl-role').on('change', function () {
            var role = $('#ddl-role :selected').text();
            common.config.pageIndex = 1;
            $('#pagination-users-table').twbsPagination('destroy');
            $('#pagination-activity').twbsPagination('destroy');
            activity.loadDataUser(role);
        });

        $('body').on('click', '.btn-view-activity', function () {
            var id = $(this).attr('data-id');
            $('#hid-user-id').val(id);
            $('#pagination-activity').twbsPagination('destroy');
            activity.loadActivities(id);
        });

        $('#txt-search').on('keypress', function (e) {
            if (e.which === 13) {
                $('#pagination-users-table').twbsPagination('destroy');
                $('#pagination-activity').twbsPagination('destroy');
                activity.loadDataUser();
            }
        });
    },

    loadRoleToDropDown: function () {
        var render = '';
        return $.ajax({
            type: 'GET',
            url: '/Admin/Role/GetAllAsync',
            success: function (res) {
                $.each(res, function (i, item) {
                    if (i == 0) {
                        render += '<option value="' + item.Name + '" selected="selected">' + item.Name + '</option>'
                    } else {
                        render += '<option value="' + item.Name + '">' + item.Name + '</option>'
                    }
                });
                $('#ddl-role').html(render);
            }
        });
    },

    loadDataUser: function (role) {
        role = role || $('#ddl-role :selected').text();
        var render = '';
        var template = $('#tbl-load-user-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Activity/GetUsersByRole',
            data: {
                roleName: role,
                page: common.config.pageIndex,
                pageSize: common.config.pageSize,
                keyword: $('#txt-search').val(),
            },
            dataType: 'json',
            success: function (res) {
                $.each(res.Results, function (i, item) {
                    var user = {};
                    user.Id = item.Id;
                    user.Username = item.UserName;
                    user.CreatedDate = common.formatDate(item.CreatedDate);
                    render += Mustache.render(template, user);
                });
                $('#tbl-users-content').html(render);
                activity.paginationUser(res.RowCount);

                var id = $('#tbl-users-content tr:first').attr('data-id');
                if (id != undefined && id != '') {
                    $('#hid-user-id').val(id);
                    activity.loadActivities(id);
                }
            }
        });
    },

    loadActivities: function (id) {
        id = id || $('#hid-user-id').val();
        var render = '';
        var template = $('#tbl-activity-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Activity/GetActivityById',
            data: {
                userId: id,
                page: common.config.pageIndex,
                pageSize: common.config.pageSize,
            },
            beforeSend: common.runLoadingIndicator(),
            dataType: 'json',
            success: function (res) {
                $.each(res.Results, function (i, item) {
                    var detail = {};
                    detail.Username = item.Username;
                    detail.LastSession = common.formatDateTime(item.LastSession);
                    detail.Device = item.Device;
                    detail.IpAddress = item.IPAddress;

                    render += Mustache.render(template, detail);
                });
                $('#tbl-activity-content').html(render);
                $('#lbl-total-activity').html(res.RowCount);
                if (res.RowCount > 0) {
                    activity.paginationActivity(res.RowCount);
                }
                common.stopLoadingIndicator();
            }
        });
    },

    paginationUser: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-users-table').twbsPagination({
            totalPages: totalPages,
            visiblePages: 3,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    var role = role || $('#ddl-role :selected').text();
                    $('#pagination-activity').twbsPagination('destroy');
                    activity.loadDataUser(role);
                }
            }
        });
    },

    paginationActivity: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-activity').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    activity.loadActivities();
                }
            }
        });
    },

}
activity.init();