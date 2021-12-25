using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class Khachtour
    {
        public decimal Idkhach { get; set; }
        public string Sgtcode { get; set; }
        public int? Stt { get; set; }
        public string Makh { get; set; }
        public string Hoten { get; set; }
        public DateTime? Ngaysinh { get; set; }
        public bool? Phai { get; set; }
        public string Dienthoai { get; set; }
        public string Diachi { get; set; }
        public string Quoctich { get; set; }
        public string Loaiphong { get; set; }
        public string Cmnd { get; set; }
        public string Hochieu { get; set; }
        public DateTime? Ngaycaphc { get; set; }
        public DateTime? Hieuluchc { get; set; }
        public bool Vmb { get; set; }
        public string Prn { get; set; }
        public string Ghichu { get; set; }
        public string Logfile { get; set; }
        public bool? Del { get; set; }
        public string Visa { get; set; }
        public string YeuCauVisa { get; set; }
    }
}
