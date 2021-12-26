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

        // Tinh click
        $('tr .tdVal').click(function () {
            id = $(this).data('id'); // Tinh id: matinh

            indexController.TdVal_Click(id);
        });
        // Tinh click
    },
    Load_DiemTQPartial: function (id) { // tinh id
        strUrl = $('#hidStrUrl').val();
        var url = '/DiemTQ/DiemTQPartial';

        $.get(url, { maTinh: id, strUrl: strUrl }, function (response) {
            $('#DiemTQ_Tbl').html(response);
            $('#DiemTQ_Tbl').show(500);
        });
    },
    TdVal_Click: function (id) { // tinh id
        $('#DiemTQ_Create_Partial').hide(500);
        $('#DiemTQ_Edit_Partial').hide(500);

        indexController.Load_DiemTQPartial(id); // tinh id
    }
};
indexController.init();