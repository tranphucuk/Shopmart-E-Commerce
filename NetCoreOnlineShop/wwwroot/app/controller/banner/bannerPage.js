var bannerPage = {
    init: function () {
        $.when(
            bannerPage.loadPosition(),
            bannerPage.loadPage(),
        ).then(function () {
            bannerPage.registerEvents();
        });
    },

    registerEvents: function () {
        $('body').on('click', '.btnManagerAdv', function () {
            var id = $(this).attr('data-id');
            $('#hid-adv-id').val(id);
            $('#tbl-gallery-content').html('');
            bannerPage.loadAdsPage(id);
        });

        $('#btn-add-ads').on('click', function () {
            var render = '';
            var template = $('#add-ads-template').html();

            var ads = {};
            ads.PageName = bannerPage.assignPagesToDropDown();
            ads.Position = bannerPage.assignPositionsToDropDown();
            render = Mustache.render(template, ads);
            $('#tbl-gallery-content').append(render);
        });

        $('#btn-save-ads').on('click', function () {
            var advId = $('#hid-adv-id').val();
            var adsPages = [];
            $.each($('#tbl-gallery-content tr'), function (i, item) {
                var ads = {};
                var id = $(item).find('button.btn-remove-img').attr('data-id');
                if (id != undefined && id != 0 && id != '') {
                    ads.Id = id;
                }
                ads.Position = $(item).find('select.ddl-position').val();
                ads.AdvertisementId = advId;
                ads.AdvertisementPageNameId = $(item).find('select.ddl-pages').val();
                adsPages.push(ads);
            });
            $.ajax({
                type: 'POST',
                url: '/Admin/Banner/SaveAdsPosition',
                data: {
                    adsPages: adsPages,
                    adsId: advId
                },
                beforeSend: common.runLoadingIndicator(),
                success: function (res) {
                    common.notify('Save advertisement success', 'success');
                    $('#banner-img-gallery').modal('hide');
                    common.stopLoadingIndicator();
                }
            });
        });

        $('body').on('click', '.btn-remove-img', function () {
            $(this).closest('tr').remove();
            //var id = $(this).data('id');
            //$.ajax({
            //    type: 'POST',
            //    url: '/Admin/Banner/RemoveAdsPage',
            //    data: { id: id },
            //    success: function (res) {
            //        if (res != false) {
            //            common.notify('Remove failed. Please try again', 'error');
            //        } else {
            //            bannerPage.loadAdsPage($('#hid-adv-id').val());
            //        }
            //    }
            //});
        });
    },

    positionData: [],
    positionDropDropDown: [],

    pageData: [],
    pageDropDown: [],

    loadPosition: function () {
        return $.ajax({
            type: 'GET',
            url: '/Admin/Banner/LoadPosition',
            success: function (res) {
                bannerPage.positionData = res;
            }
        });
    },
    assignPositionsToDropDown: function (id) {
        var select = '<select class="form-control ddl-position">';
        $.each(bannerPage.positionData, function (i, item) {
            if (id != undefined && id != '' && i == id) {
                select += '<option value="' + i + '" selected="selected" >' + item + '</option>';
            } else {
                select += '<option value="' + i + '" >' + item + '</option>';
            }
        });
        select += '</select>';
        return select;
    },

    loadPage: function () {
        return $.ajax({
            type: 'GET',
            url: '/Admin/Banner/LoadPage',
            success: function (res) {
                bannerPage.pageData = res;
            }
        });
    },
    assignPagesToDropDown: function (id) {
        var select = '<select class="form-control ddl-pages">';
        $.each(bannerPage.pageData, function (i, item) {
            if (id != undefined && id != '' && item.Id == id) {
                select += '<option value="' + id + '" selected="selected" >' + item.Name + '</option>';
            } else {
                select += '<option value="' + item.Id + '" >' + item.Name + '</option>';
            }
        });
        select += '</select>';
        return select;
    },

    loadAdsPage: function (id) {
        var render = '';
        var template = $('#add-ads-template').html();
        $.ajax({
            type: 'GET',
            url: '/Admin/Banner/GetListAdsPage',
            data: { id: id },
            dataType: 'json',
            success: function (res) {
                $.each(res, function (i, item) {
                    var adsPage = {};
                    adsPage.Id = item.Id;
                    adsPage.PageName = bannerPage.assignPagesToDropDown(item.AdvertisementPageNameId);
                    adsPage.Position = bannerPage.assignPositionsToDropDown(item.Position);

                    render += Mustache.render(template, adsPage);
                });
                $('#tbl-gallery-content').html(render);
                $('#banner-img-gallery').modal('show');
            }
        });
    }
}
bannerPage.init();