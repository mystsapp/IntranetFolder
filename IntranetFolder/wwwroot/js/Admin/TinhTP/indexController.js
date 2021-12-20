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

        // phieu click --> load kvctpct
        $('tr .tdVal').click(function () {
            id = $(this).data('id'); // Tinh id: matinh

            indexController.TdVal_Click(id);
        });
        // phieu click --> load kvctpct

        // create new KVCTPCT
        $('#btn_New_KVCTPCT').off('click').on('click', function () {
            kvpctid = $(this).data('kvpctid');

            $('#KVCTPCT_Tbl').hide(500);
            $('#KVCTPCT_Edit_Partial').hide(500);

            var url = '/KVCTPTCs/KVCTPCT_Create_Partial';
            $.get(url, { kvpctid: kvpctid }, function (response) {
                $('#KVCTPCT_Create_Partial').show(500);
                $('#KVCTPCT_Create_Partial').html(response);
            });
        });
        // create new KVCTPCT
    },
    Load_ThanhPho1Partial: function (id) { // tinh id
        var url = '/ThanhPho1/ThanhPho1Partial';
        $.get(url, { maTinh: id }, function (response) {
            $('#ThanhPho1_Tbl').html(response);
            $('#ThanhPho1_Tbl').show(500);
        });
    },
    TdVal_Click: function (id) { // tinh id
        $('#ThanhPho1_Create_Partial').hide(500);
        $('#ThanhPho1_Edit_Partial').hide(500);

        indexController.Load_ThanhPho1Partial(id); // tinh id
    },
    KhachHang_By_Code: function (code, txtMaKh) {
        $.ajax({
            url: '/KVCTPTCs/GetKhachHangs_By_Code',
            type: 'GET',
            data: { code: code },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    // console.log(data);

                    if (txtMaKh === 'txtMaKhNo') { // search of no
                        $('#txtMaKhNo').val(data.code);
                        $('#txtTenKhNo').val(data.name);
                    }
                    if (txtMaKh === 'txtMaKhCo') { // search of co
                        $('#txtMaKhCo').val(data.code);
                        $('#txtTenKhCo').val(data.name);
                    }

                    $('#txtKyHieu').val(data.taxsign);
                    $('#txtMauSoHD').val(data.taxform);
                    $('#txtMsThue').val(data.taxcode);
                    $('#txtTenKH').val(data.name);
                    $('#txtDiaChi').val(data.address);
                }
                else {// search ko co KH nao het => ...
                    if ($('#btnKhSearch').data('name') === 'maKhNo') { // search of no
                        $('#txtMaKhNo').val('');
                        $('#txtTenKhNo').val('');
                    }
                    if ($('#btnKhSearch').data('name') === 'maKhCo') { // search of co
                        $('#txtMaKhCo').val('');
                        $('#txtTenKhCo').val('');
                    }

                    $('#txtKyHieu').val('');
                    $('#txtMauSoHD').val('');
                    $('#txtMsThue').val('');
                    $('#txtTenKH').val('');
                    $('#txtDiaChi').val('');
                }
            }
        });
    },
    CheckTamUng: function (kVCTPCTId) {
        return $.post('/TamUngs/CheckTamUng', { kVCTPCTId: kVCTPCTId }, function (response) {
            // console.log(response);
            return response;
        })
    },
    CheckTT141: function (kVCTPCTId) {
        return $.post('/TamUngs/CheckTT141', { kVCTPCTId: kVCTPCTId }, function (response) {
            //console.log(response);
            return response;
        })
    }
};
indexController.init();