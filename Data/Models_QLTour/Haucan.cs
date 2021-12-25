using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class Haucan
    {
        public decimal Id { get; set; }
        public long? Maphieud { get; set; }
        public long? Maphieu { get; set; }
        public string Mahh { get; set; }
        public string Donvitinh { get; set; }
        public int? Soluong { get; set; }
        public decimal? Dongia { get; set; }
        public decimal? Thanhtien { get; set; }
        public DateTime? Ngayyeucau { get; set; }
        public string Nguoiyeucau { get; set; }
        public string Sgtcode { get; set; }
        public string Ghichu { get; set; }
        public string Chinhanh { get; set; }
        public bool? Daxuat { get; set; }
        public string Maphieudx { get; set; }
        public string Logfile { get; set; }
        public DateTime? Locktour { get; set; }
    }
}
