var home = {
    init: function () {
        home.initDatePicker();
        home.getDetailsThisMonth();
        home.getLatestPurchases();
        home.sortPaymentType();
        home.billStatus();
        home.registerEvents();
    },

    registerEvents: function () {
        $('#reportrange').on('cancel.daterangepicker', function (ev, picker) {
            $('#reportrange span').html(moment().subtract(29, 'days').format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
        });

        $('#toggleController input[type="checkbox"]').on('click', function () {
            home.doToggling();
        });
    },

    initDatePicker: function () {
        var start = moment().subtract(30, 'days');
        var end = moment();

        function cb(start, end) {
            $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
            home.bindData(common.formatDateTime(start._d), common.formatDateTime(end._d));
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

    calculateDay: function () {
        var dateSize = $('#reportrange span').html();
        var firstDate = new Date(dateSize.split('-')[0]);
        var secondDate = new Date(dateSize.split('-')[1]);
        var days = Math.ceil(parseInt(secondDate - firstDate) / (24 * 3600 * 1000));
        return days;
    },

    bindData: function (startDate, endDate) {
        $.ajax({
            type: 'GET',
            url: '/Admin/Home/GetRevenue',
            data: {
                fromDate: startDate,
                toDate: endDate
            },
            success: function (res) {
                home.dataCollection = [];
                home.revenue = [];
                home.profit = [];
                home.funds = [];
                home.session = [];
                $.each(res.Finance, function (i, item) {
                    home.revenue.push([new Date(item.Date).getTime(), item.Revenue]);
                    home.profit.push([new Date(item.Date).getTime(), item.Profit]);
                    home.funds.push([new Date(item.Date).getTime(), item.Funds]);
                });

                $.each(res.Session, function (i, item) {
                    home.session.push(item.TotalSession);
                });
                if (home.revenue.length > 0) {
                    home.dataCollection.push(home.revenue, home.profit, home.funds);
                    if ($("#toggleController").find("input[type='checkbox']:checked").length == 0) {
                        $("#toggleController").find("input[type='checkbox']").click();
                        home.doToggling();
                        home.doSparklineTotalSession(home.session);
                        home.doSparklineTotalRevenue(home.revenue);

                    } else {
                        home.doToggling();
                        home.doSparklineTotalSession(home.session);
                        home.doSparklineTotalRevenue(home.revenue);
                    }
                }
            }
        });
    },

    doToggling: function () {
        home.dataSet = [];
        $.each($('#toggleController input[type="checkbox"]'), function (i, item) {
            var isChecked = $(item).is(':checked');
            if (isChecked) {
                var position = parseInt($(item).attr('data-id'));
                home.dataSet.push({
                    label: home.labelCollection[position],
                    data: home.dataCollection[position],
                    color: home.lineChartColorCollection[position]
                });
            }
        });

        var days = home.calculateDay();
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

        $.plot($("#chart_plot_02"), home.dataSet, settings);
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

    getDetailsThisMonth: function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Home/GetDetailThisMonth',
            success: function (res) {
                var date = common.formatDate(new Date());

                $('#new-account').html(res.NewAccount);
                $('#new-acc-detail').html(res.NewAccount + ' new sign ups in this month.');

                $('#new-product').html(res.NewProduct);
                $('#new-product-detail').html(res.NewProduct + ' new products added in this month.');

                $('#total-sold').html(res.Sold);
                $('#sold-detail').html('There have been ' + res.Sold + ' solds in this month.');

                $('#bill-delivered').html(res.CompletedBill);
                $('#bill-delivered-detail').html(res.CompletedBill + ' bills delivered successfully in this month.');
            }
        });
    },

    getLatestPurchases: function () {
        var render = '';
        var template = $('#tbl-latest-purchases-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Home/LatestPurchase',
            dataType: 'json',
            success: function (res) {
                $.each(res, function (i, item) {
                    var detail = {};
                    detail.BorderColor = i % 2 == 0 ? 'border-aero' : 'border-green';
                    detail.Icon = i % 2 == 0 ? 'fa-user aero' : 'fa-user green';
                    detail.Name = item.Bill.CustomerName;
                    detail.Address = item.Bill.CustomerAddress;
                    detail.TotalCost = common.formartNumber(item.TotalCost, '0,0');
                    detail.Quantity = item.Quantity;
                    detail.Date = common.formatDate(item.Bill.CreatedDate)

                    render += Mustache.render(template, detail);
                });

                $('#latest-purchases-body').html(render);
            }
        });
    },

    initPieChart: function (data, label) {
        if (typeof (Chart) === 'undefined') { return; }

        if ($('.canvasDoughnut').length) {

            var chart_doughnut_settings = {
                type: 'doughnut',
                tooltipFillColor: "rgba(51, 51, 51, 0.55)",
                data: {
                    labels: label,
                    datasets: [{
                        data: data,
                        backgroundColor: home.doughnutChartColorCollection,
                        hoverBackgroundColor: home.doughnutChartColorCollection
                    }]
                },
                options: {
                    legend: false,
                    responsive: false
                }
            }

            $('.canvasDoughnut').each(function () {

                var chart_element = $(this);
                var chart_doughnut = new Chart(chart_element, chart_doughnut_settings);

            });

        }
    },

    sortPaymentType: function () {
        var label = [];
        var data = [];
        var render = '';
        var template = $('#tbl-payment-pie-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Home/PaymentReport',
            dataType: 'json',
            success: function (res) {
                var index = 0;
                $.each(res.Payments, function (i, item) {
                    label.push(i);
                    data.push(item);

                    var payment = {};
                    payment.Color = home.doughnutChartColorCollection[index];
                    index++;
                    payment.Name = i;
                    payment.Percent = ((item / res.Total) * 100).toFixed(2);
                    render += Mustache.render(template, payment);
                });
                home.initPieChart(data, label);
                $('#tbl-pie-body').html(render);
            }
        });
    },

    billStatus: function () {
        var render = '';
        var template = $('#bill-cindition-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Home/BillReport',
            dataType: 'json',
            success: function (res) {
                render += '<h4>Bill condition all time</h4>';
                $.each(res, function (i, item) {
                    var bill = {};
                    bill.Name = item.BillName;
                    bill.Percent = item.Percent;

                    render += Mustache.render(template, bill);
                });
                $('#bill-condition-body').html(render);
            }
        });
    },

    labelCollection: ['Revenue Report', 'Profit Report', 'Funds Report',],
    lineChartColorCollection: ['#3F97EB', '#72c380', '#f7cb38'],
    doughnutChartColorCollection: ["#ed6868", "#efb65f", "#5fe22f", "#7de8db", "#0dbce8", "#c98bef", "#ad4391"],
    dataCollection: [],
    dataSet: [],

    revenue: [],
    profit: [],
    funds: [],
    session: [],
};
home.init();