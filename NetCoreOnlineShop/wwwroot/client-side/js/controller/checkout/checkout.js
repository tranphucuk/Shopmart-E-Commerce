﻿var checkout = {
    init: function () {
        this.registerEvents();
    },

    registerEvents: function () {
        var validator = $('#frm-check-out').validate({
            errorClass: 'error-validation',
            rules: {
                CustomerName: 'required',
                CustomerEmail: 'required',
                CustomerAddress: 'required',
                CustomerPhone: {
                    required: true,
                    number: true,
                    minlength: 10,
                    maxlength: 16
                },
                CustomerMessage: 'required',
            },
            messages: {
                CustomerName: {
                    required: '*Please enter your name'
                },
                CustomerEmail: {
                    required: '*Please enter your email'
                },
                CustomerAddress: {
                    required: '*Please enter your address'
                },
                CustomerPhone: {
                    required: '*Please enter your phone number',
                    number: '*Please enter a correct phone number',
                    minlength: '*Please enter a correct phone number',
                    maxlength: '*Please enter a correct phone number',
                },
                CustomerMessage: {
                    required: '*Please note down your message'
                },
            }
        });

        $('.check-out-guest').on('click', function () {
            $('.checkout-step1').css({
                'display': 'none'
            });

            $('.checkout-step2').css({
                'display': ''
            })
        });

        $('.btn-step-3').on('click', function () {
            $('.checkout-step3').css({
                'display': 'none'
            })

            $('.checkout-step4').css({
                'display': ''
            })
        });

        $('.back-step2').on('click', function () {
            $('.checkout-step3').css({
                'display': 'none'
            })

            $('.checkout-step2').css({
                'display': ''
            })
        });

        $('.back-step3').on('click', function () {
            $('.checkout-step3').css({
                'display': ''
            })

            $('.checkout-step4').css({
                'display': 'none'
            })
        });

        $('.btn-step-2').on('click', function () {
            if (validator.form()) {
                $('.checkout-step2').css({
                    'display': 'none'
                })

                $('.checkout-step3').css({
                    'display': ''
                })
            }
        }); // validate from input before moving to next step
    }
};
checkout.init();