function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

var indexController = {
    init: function () {
        toastr.options = { // toastr options
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "2000",
            "timeOut": "2000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        indexController.registerEvent();
    },

    registerEvent: function () {
        // format .numbers
        $('input.numbers').keyup(function (event) {
            // Chỉ cho nhập số
            if (event.which >= 37 && event.which <= 40) return;

            $(this).val(function (index, value) {
                return addCommas(value);
            });
        });

        //$('tr .tdVal').click(function () {
        //    id = $(this).data('id');

        //    indexController.TdVal_Click(id);
        //});
    },
    Load_DichVuPartial: function (id) { // danhgia DV
        strUrl = $('#hidStrUrl').val();
        var url = '/Supplier/DichVuPartial';

        $.get(url, { id: id, strUrl: strUrl }, function (response) {
            $('#tabDichVu').html(response);
            $('#tabDichVu').show(500);
        });
    },
    Load_DichVu1Partial: function (id, page, searchString) {
        //strUrl = $('#hidStrUrl').val();
        var url = '/DichVu1/DichVu1Partial';

        $.get(url, { supplierId: id, page: page, searchString: searchString }, function (response) {
            $('#dichVu1_Tbl').html(response);
            $('#dichVu1_Tbl').show(500);
        });
    },
    //TdVal_Click: function (id) {
    //    // page
    //    var page = $('.active span').text();
    //    // hidSearchString
    //    var searchString = $('#hidSearchString').val();

    //    indexController.Load_DichVu1Partial(id, page, searchString);
    //    indexController.Load_DichVuPartial(id); // danhgia DV
    //},
    GetThanhPhoByTinh: function (tinhTPId) {
        $('#ddlThanhPho').html('');
        var option = '';

        $.ajax({
            url: '/Supplier/GetThanhPhoByTinh_Hong',
            type: 'GET',
            data: { tinhTPId: tinhTPId },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;

                    $.each(data, function (i, item) {
                        option = option + '<option value="' + item.matp + '">' + item.tentp + '</option>'; //chinhanh1
                    });

                    $('#ddlThanhPho').html(option);
                }
            }
        });
    }
};
indexController.init();