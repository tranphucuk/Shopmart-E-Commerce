var revenue = {
    init: function () {
        this.registerEvents();
        this.initDatePicker();
    },

    registerEvents: function () {
        $('#reportrange').on('cancel.daterangepicker', function (ev, picker) {
            $('#reportrange span').html(moment().subtract(29, 'days').format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
        });

        $('#toggleController input[type="checkbox"]').on('click', function () {
            revenue.doToggling();
        });

        $('#ddlShowPage').on('change', function () {
            common.config.pageSize = $('#ddlShowPage :selected').text();
            common.config.pageIndex = 1;
            $('#pagination-revenue').twbsPagination('destroy');
            revenue.getDataForPaging();
        });

        $('#sort-order').on('change', function () {
            revenue.configPagination();
            revenue.getDataForPaging();
        });
    },

    initDatePicker: function () {
        var start = moment().subtract(30, 'days');
        var end = moment();

        function cb(start, end) {
            $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
            revenue.bindData(common.formatDateTime(start._d), common.formatDateTime(end._d));
        }

        $('#reportrange').daterangepicker({
            startDate: start,
            endDate: end,
            opens: 'left',
            autoUpdateInput: false,
            locale: {
                cancelLabel: 'Clear'
            },
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                'Last 3 Months': [moment().subtract(3, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                'Last 6 Months': [moment().subtract(6, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
            },
        }, cb);
        cb(start, end);
    },

    bindData: function (startDate, endDate) {
        $.ajax({
            type: 'GET',
            url: '/Admin/Revenues/GetRevenue',
            data: {
                fromDate: startDate,
                toDate: endDate
            },
            success: function (res) {
                revenue.dataCollection = [];
                revenue.revenue = [];
                revenue.profit = [];
                revenue.funds = [];
                revenue.session = [];
                $.each(res.Finance, function (i, item) {
                    revenue.revenue.push([new Date(item.Date).getTime(), item.Revenue]);
                    revenue.profit.push([new Date(item.Date).getTime(), item.Profit]);
                    revenue.funds.push([new Date(item.Date).getTime(), item.Funds]);
                });
                $.each(res.Session, function (i, item) {
                    revenue.session.push(item.TotalSession);
                });

                revenue.configPagination();
                revenue.getDataForPaging();
                if (revenue.revenue.length > 0) {
                    revenue.dataCollection.push(revenue.revenue, revenue.profit, revenue.funds);
                    if ($("#toggleController").find("input[type='checkbox']:checked").length == 0) {
                        $("#toggleController").find("input[type='checkbox']").click();
                        revenue.doToggling();
                        revenue.doSparklineTotalSession(revenue.session);
                        revenue.doSparklineTotalRevenue(revenue.revenue);

                    } else {
                        revenue.doToggling();
                        revenue.doSparklineTotalSession(revenue.session);
                        revenue.doSparklineTotalRevenue(revenue.revenue);
                    }
                }
            }
        });
    },

    getDataForPaging: function () {
        var render = '';
        var template = $('#tbl-revenue-template').html();

        var date = $('#reportrange span').html().split('-');
        var startDate = date[0];
        var endDate = date[1];
        $.ajax({
            type: 'GET',
            url: '/Admin/Revenues/RevenueReportPaging',
            data: {
                fromDate: startDate,
                toDate: endDate,
                page: common.config.pageIndex,
                pageSize: common.config.pageSize,
                sortOrder: $('#sort-order').val()
            },
            dataType: 'json',
            success: function (res) {
                $.each(res.Results, function (i, item) {
                    var revenueDetail = {};
                    revenueDetail.Date = common.formatDate(item.Date);
                    revenueDetail.Sesion = item.TotalSession;
                    revenueDetail.Revenue = common.formartNumber(item.Revenue);
                    revenueDetail.Funds = common.formartNumber(item.Funds);
                    revenueDetail.Profit = common.formartNumber(item.Profit);
                    revenueDetail.Flat = item.FlatPercent.toFixed(2);

                    render += Mustache.render(template, revenueDetail);
                });
                $('#tbl-revenue-content').html(render);
                $('#lbl-total-report').html(res.RowCount);
                if (res.RowCount > 0) {
                    revenue.pagination(res.RowCount);
                }
            }
        });
    },
    pagination: function (total) {
        var totalPages = Math.ceil(total / common.config.pageSize);

        $('#pagination-revenue').twbsPagination({
            totalPages: totalPages,
            visiblePages: 5,
            onPageClick: function (event, page) {
                if (page != common.config.pageIndex) {
                    common.config.pageIndex = page;
                    revenue.getDataForPaging();
                }
            }
        });
    },
    configPagination: function () {
        $('#pagination-revenue').twbsPagination('destroy');
        $('#ddlShowPage').prop('selectedIndex', 0);
        common.config.pageSize = 10;
        common.config.pageIndex = 1;
    },
    calculateDay: function () {
        var dateSize = $('#reportrange span').html();
        var firstDate = new Date(dateSize.split('-')[0]);
        var secondDate = new Date(dateSize.split('-')[1]);
        var days = Math.ceil(parseInt(secondDate - firstDate) / (24 * 3600 * 1000));
        return days;
    },
    doToggling: function () {
        revenue.dataSet = [];
        $.each($('#toggleController input[type="checkbox"]'), function (i, item) {
            var isChecked = $(item).is(':checked');
            if (isChecked) {
                var position = parseInt($(item).attr('data-id'));
                revenue.dataSet.push({
                    label: revenue.labelCollection[position],
                    data: revenue.dataCollection[position],
                    color: revenue.lineChartColorCollection[position]
                });
            }
        });

        var days = revenue.calculateDay();
        var type = days >= 89 ? [1, 'month'] : [4, 'day'];
        var xaxisDateFormat = days >= 89 ? "%b/%y" : "%d/%m";

        var settings = {
            series: {
                lines: {
                    show: true,
                    fill: true,
                    lineWidth: 1,
                },
                points: {
                    radius: 3,
                    fill: true,
                    show: true
                },
            },
            colors: ['#3F97EB', '#72c380', '#f7cb38'],
            xaxis: {
                mode: "time",
                tickSize: type, //[2, 'day']
                timeformat: xaxisDateFormat,
                axisLabelUseCanvas: true,
                axisLabelFontSizePixels: 12,
                axisLabelFontFamily: 'Verdana, Arial',
                axisLabelPadding: 10
            },
            yaxes: [{

            }],
            grid: {
                borderWidth: 0,
                borderHeight: 0,
                hoverable: true,
                clickable: true,
                backgroundColor: { colors: ["#ffffff", "#EDF5FF"] },
                autoHighlight: true,
                mouseActiveRadius: 30
            },
            legend: {
                position: "ne",
                margin: [0, -18],
                noColumns: 0,
                labelFormatter: function (label, series) {
                    return label + '&nbsp;&nbsp;';
                },
                width: 40,
                height: 1
            },
        };
        $.fn.UseTooltip = function () {
            var previousPoint = null;

            $(this).bind("plothover", function (event, pos, item) {
                if (item) {
                    if (previousPoint != item.dataIndex) {
                        previousPoint = item.dataIndex;

                        $("#tooltip").remove();

                        var x = item.datapoint[0];
                        var y = item.datapoint[1];

                        showTooltip(item.pageX, item.pageY,
                            new Date(x).toString("MMM/dd/yyyy") + "<br/>" + "<strong>$" + common.formartNumber(y, '0,0') + "</strong> (" + item.series.label + ")");
                    }
                }
                else {
                    $("#tooltip").remove();
                    previousPoint = null;
                }
            });
        };
        function showTooltip(x, y, contents) {
            $('<div id="tooltip">' + contents + '</div>').css({
                position: 'absolute',
                display: 'none',
                top: y + 5,
                left: x + 20,
                border: '2px solid #4572A7',
                padding: '2px',
                size: '10',
                'background-color': '#fff',
                opacity: 0.80
            }).appendTo("body").fadeIn(200);
        }

        $.plot($("#chart_plot_02"), revenue.dataSet, settings);
        $("#chart_plot_02").UseTooltip();
    },
    doSparklineTotalSession: function (data) {
        $(".sparkline11").sparkline(data, {
            type: 'bar',
            height: '40',
            barWidth: 8,
            colorMap: {
                '7': '#a1a1a1'
            },
            barSpacing: 2,
            barColor: '#26B99A'
        });
        var total = 0;
        $.each(data, function (i, item) {
            total += item;
        });
        $('#total-session').html(total);
    },
    doSparklineTotalRevenue: function (data) {
        $(".sparkline22").sparkline(data, {
            type: 'line',
            numberFormatter: function (num) {
                return '$' + common.formartNumber(num, '0,0');
            },
            height: '40',
            width: '200',
            lineColor: '#26B99A',
            fillColor: '#ffffff',
            lineWidth: 3,
            spotColor: '#34495E',
            minSpotColor: '#34495E'
        });
        var total = 0;
        $.each(data, function (i, item) {
            total += item[1];
        });
        $('#total-revenue').html('$' + common.formartNumber(total, '0,0'));
    },

    labelCollection: ['Revenue Report', 'Profit Report', 'Funds Report',],
    lineChartColorCollection: ['#3F97EB', '#72c380', '#f7cb38'],
    dataCollection: [],
    dataSet: [],

    revenue: [],
    profit: [],
    funds: [],
    session: [],
};
revenue.init();