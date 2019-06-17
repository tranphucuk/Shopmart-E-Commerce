var common = {
    config: {
        pageSize: 10,
        pageSizeSmall: 5,
        pageSizeBig: 15,
        pageIndex: 1
    },
    notify: function (message, type) {
        $.notify(message, {
            // whether to hide the notification on click
            clickToHide: true,
            // whether to auto-hide the notification
            autoHide: true,
            // if autoHide, hide after milliseconds
            autoHideDelay: 5000,
            // show the arrow pointing at the element
            arrowShow: true,
            // arrow size in pixels
            arrowSize: 5,
            // position defines the notification position though uses the defaults below
            position: 'bottom right',
            // default positions
            elementPosition: 'bottom left',
            globalPosition: 'top right',
            // default style
            style: 'bootstrap',
            // default class (string or [string])
            className: type,
            // show animation
            showAnimation: 'slideDown',
            // show animation duration
            showDuration: 400,
            // hide animation
            hideAnimation: 'slideUp',
            // hide animation duration
            hideDuration: 200,
            // padding between element and notification
            gap: 2
        });
    }, // library notify.js

    confirm: function (message, okCallback) {
        bootbox.confirm({
            size: "small",
            message: message,
            buttons: {
                cancel: {
                    label: 'Cancel',
                    className: 'btn-secondary'
                },
                confirm: {
                    label: 'OK',
                    className: 'btn-primary'
                }
            },
            callback: function (result) {
                if (result === true) {
                    okCallback();
                }
            }
        });
    }, // library bootbox.js

    formatDate: function (dateInput) {
        if (dateInput != null || dateInput != '') {
            var date = new Date(Date.parse(dateInput)).toLocaleDateString();
            return date;
        } else {
            return '';
        }
    },

    formatDateTime: function (dateTimeInput) {
        if (dateTimeInput != null && dateTimeInput !== '') {
            var date = new Date(Date.parse(dateTimeInput)).toLocaleDateString();
            var time = new Date(Date.parse(dateTimeInput)).toLocaleTimeString();
            var dateTime = date + ' ' + time;
            return dateTime;
        } else {
            return '';
        }
    },

    formartNumber: function (number, format) {
        if (!isFinite(number)) {
            return number.toString();
        }
        var string = numeral(number).format(format); // library numeral.js
        return string;
    },

    formatImageWithSize: function (img, name, size) {
        var defaultImg = '/admin-side/images/loading.gif';
        return img != null ? '<img class="setImg" src="' + img + '" width="' + size + '" alt="' + name + '" />' : '<img class="setImg" src="' + defaultImg + '"width="' + size + '" />';
    },

    formatImage: function (img, name) {
        var defaultImg = '/admin-side/images/loading.gif';
        return img != null ? '<img src="' + img + '" width="40" alt="' + name + '" />' : '<img src="' + defaultImg + '" width="40" />';
    },

    runLoadingBar: function () {
        if ($('.dv-loading').length > 0) {
            $('.dv-loading').removeClass('hide')
        }
    },

    stopLoadingBar: function () {
        if ($('.dv-loading').length > 0) {
            $('.dv-loading').addClass('hide')
        }
    },

    getStatusLabel: function (status) {
        if (status === 1) {
            return '<span class="badge bg-green ">Activated</span>'
        } else {
            return '<span class="badge bg-red ">Blocked</span>'
        }
    },

    getStatusFeedback: function (status) {
        if (status === 1) {
            return '<span class="badge bg-green ">Responded</span>'
        } else {
            return '<span class="badge bg-orange ">Waiting</span>'
        }
    },

    getStatusTicket: function (status) {
        if (status === 1) {
            return '<span class="badge bg-green ">Solved</span>'
        } else {
            return '<span class="badge bg-orange ">Open</span>'
        }
    },

    getStatusUpload: function (status) {
        if (status === 1) {
            return '<span class="badge bg-green">Uploaded</span>'
        } else {
            return '<span class="badge bg-orange">Waiting</span>'
        }
    },

    unflattern: function (arr) {
        'use strict';
        var map = {};
        var roots = [];
        for (var i = 0; i < arr.length; i++) {
            arr[i].children = [];
            map[arr[i].id] = i;    // 1 key : 1 value ==>> add this obj to a dictionary
        }
        for (var i = 0; i < arr.length; i += 1) {
            var node = arr[i];
            // use map to look-up the parents
            if (node.parentId !== null) {
                arr[map[node.parentId]].children.push(node);
            } else {
                roots.push(node);
            }
        }
        return roots;
    },

    validation: function (arr, form) {
        $.extend($.fn.validatebox.defaults.rules, {
            range: {
                validator: function (value, param) {
                    var min = parseInt(param[0]);
                    var max = parseInt(param[1]);
                    return value >= min && value <= max;
                },
                message: 'The value must be between {0} and {1}'
            }
        });

        $.extend($.fn.validatebox.defaults.rules, {
            justText: {
                validator: function (value, param) {
                    return isNaN(value);
                },
                message: 'Please enter a text.'
            }
        });

        $.extend($.fn.validatebox.defaults.rules, {
            justPositive: {
                validator: function (value, param) {
                    return value >= 0;
                },
                message: 'Value must be a positive number.'
            }
        });

        $.extend($.fn.validatebox.defaults.rules, {
            justNumber: {
                validator: function (value, param) {
                    return $.isNumeric(value);
                },
                message: 'Please enter a valid number.'
            }
        });

        $.extend($.fn.validatebox.defaults.rules, {
            equals: {
                validator: function (value, param) {
                    return value == $(param[0]).val();
                },
                message: 'Password does not match.'
            }
        });

        for (var i = 0; i < arr.length; i++) {
            if ($('#' + arr[i]).is(':disabled') == true || $('#' + arr[i]).is('[readonly]') == true && $('#' + arr[i]).selector.includes('easyui') == false) { //easyui  selector
                continue;
            }
            var type = $('#' + arr[i]).attr('type');
            if (type == 'number') {
                $('#' + arr[i]).validatebox({
                    required: true,
                    validType: {
                        number: true,
                        justPositive: $(arr[i]).val(),
                        range: [0, 999999999]
                    }
                });
            }
            else if (type == 'password') {
                continue;
            }
            else if (type == 'email') {
                $('#' + arr[i]).validatebox({
                    required: true,
                    validType: {
                        email: true
                    }
                });
            }
            else if (type == 'text') {
                $('#' + arr[i]).validatebox({
                    required: true,
                });
            } else if ($('#' + arr[i]).is('textarea')) {
                $('#' + arr[i]).val(CKEDITOR.instances.txtProductContent.getData());
                $('#' + arr[i]).validatebox({
                    required: true,
                });
            }
        }
        var isValid = $(form).form('validate');
        return isValid;
    },

    readImg: function (file, onLoadCallback) {
        var reader = new FileReader();
        reader.onload = onLoadCallback;
        reader.readAsDataURL(file);
    },

    runLoadingIndicator: function () {
        if (Math.random() >= 0.5) {
            $('body').loading({
                theme: 'dark',
            });
        }
        else {
            $('body').loading({
                theme: 'light'
            });
        }
    },

    stopLoadingIndicator: function () {
        $('body').loading('stop');
    },

    definePaymentMethod: function (number) {
        switch (number) {
            case 0:
                return 'Cash on delivery';
            case 1:
                return 'Online banking';
            case 2:
                return 'Payment gateway';
            case 3:
                return 'Visa';
            case 4:
                return 'Master card';
            case 5:
                return 'Paypal';
            case 6:
                return 'Atm';
            default:
                break;
        }
    },

    definePaymentStatus: function (number) {
        switch (number) {
            case 0:
                return '<span class="badge bg-blue">New</span>';
            case 1:
                return '<span class="badge bg-orange">In progress</span>';
            case 2:
                return '<span class="badge bg-orange">Returned</span>';
            case 3:
                return '<span class="badge bg-red">Cancelled</span>';
            case 4:
                return '<span class="badge bg-green">Completed</span>';
            default:
                break;
        }
    },
};

$(document).ajaxSend(function (e, xhr, options) {
    if (options.type.toUpperCase() == "POST" || options.type.toUpperCase() == "PUT") {
        var token = $('form').find("input[name='__RequestVerificationToken']").val();
        xhr.setRequestHeader("RequestVerificationToken", token);
    }
});
