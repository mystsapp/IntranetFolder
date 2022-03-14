var CodeSupplierControl = {
    init: function () {
        CodeSupplierControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {
            $('.select2').select2();
        });
        CodeSupplierControl.AddAutoComplet();

        $('.ddlTinh').off('change').on('change', function () {
            debugger;
            var matinh = $('.ddlTinh option:selected').val();
            var url1 = "/Supplier/GetThanhphoByTinh";
            var ddlThanhpho = $('.ddlThanhpho');
            ddlThanhpho.empty();
            $.getJSON(url1, { matinh: matinh }, function (thanhphos) {
                $.each(thanhphos, function (index, thanhpho) {
                    ddlThanhpho.append($('<option/>', {
                        value: thanhpho.matp,
                        text: thanhpho.tentp
                    })).trigger('change');
                });
            });
        });
        $('.btSearchkh').on('click', function () {
            var search = $('#txtTenthuongmai').val();
            var search1 = $('#txtTengiaodich').val();
            var url = "/Supplier/listTenthuongmai";
            $.get(url, { search: search, search1: search1 }, function (data) {
                $("#iModalTimkh").modal('show')
                $('.ilisttimkh').html(data);
            });
        });

        $('.btSearchkh_').on('click', function () {
            if ($('#frmListTenthuongmai').valid()) {
                var search = $('#txtSearch').val();
                //var search1 = $('#txtTengiaodich').val();
                var url = "/Supplier/listTenthuongmai";
                $.get(url, { search: search }, function (data) {
                    $("#iModalTimkh").modal('show')
                    $('.ilisttimkh').html(data);
                });
            }
        });

        $('#btConfirmHuyCapcode').on('click', function () {
            var id = $('#txtCapcodeId').val();
            var url = '/Supplier/DeleteCapcode';
            $.get(url, { id: id }, function (data) {
                $("#iModalTimkh").modal('show')
                $('.ilisttimkh').html(data);
            });
        });
        $('#frmHuySupplier').validate({
            rules: {
                Lydo: {
                    required: true
                }
            },
            messages: {
                Lydo: {
                    required: "Vui lòng nhập lý do"
                }
            }
        });
        $('#frmListTenthuongmai').validate({
            rules: {
                search: {
                    required: true
                }
            },
            messages: {
                search: {
                    required: "Vui lòng nhập thông tin cần tìm"
                }
            }
        });
        $('#btHuyCode').off('click').on('click', function () {
            if ($('#frmHuySupplier').valid()) {
                $('#frmHuySupplier').submit();
            }
        });
        $('.modal').draggable();
    },
    AddAutoComplet: function () {
        var availableTags = [];
        $.ajax({
            url: '/Supplier/getGroup',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                availableTags = JSON.parse(JSON.stringify(response));
                $(".txtGroup").autocomplete({
                    source: availableTags
                });
            }
        });
    }

    // Multi AutoComplet
    //AddAutoComplet: function () {
    //    var availableTags = [];
    //    $.ajax({
    //        url: '/Supplier/getGroup',
    //        type: 'GET',
    //        dataType: 'json',
    //        success: function (response) {
    //            availableTags = JSON.parse(JSON.stringify(response));
    //        }
    //    });
    //    function split(val) {
    //        //return val.split(/,\s*/);
    //        return val.split(/,\s*/);
    //    }
    //    function extractLast(term) {
    //        return split(term).pop();
    //    }

    //    $(".txtGroup")
    //        // don't navigate away from the field on tab when selecting an item
    //        .on("keydown", function (event) {
    //            if (event.keyCode === $.ui.keyCode.TAB &&
    //                $(this).autocomplete("instance").menu.active) {
    //                event.preventDefault();
    //            }
    //        })
    //        .autocomplete({
    //            minLength: 0,
    //            source: function (request, response) {
    //                // delegate back to autocomplete, but extract the last term
    //                response($.ui.autocomplete.filter(
    //                    availableTags, extractLast(request.term)));
    //            },
    //            focus: function () {
    //                // prevent value inserted on focus
    //                return false;
    //            },
    //            select: function (event, ui) {
    //                var terms = split(this.value);
    //                // remove the current input
    //                terms.pop();
    //                // add the selected item
    //                terms.push(ui.item.value);
    //                // add placeholder to get the comma-and-space at the end
    //                terms.push("");
    //                this.value = terms.join(", ");
    //                return false;
    //            }
    //        });
    //}
};
CodeSupplierControl.init();