using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class Trahaucan
    {
        public decimal IdTrahc { get; set; }
        public string Mahh { get; set; }
        public int Soluongtra { get; set; }
        public DateTime? Ngayyeucau { get; set; }
        public string Nguoiyeucau { get; set; }
        public string Sgtcode { get; set; }
        public string Ghichutra { get; set; }
        public string Chinhanh { get; set; }
        public bool Danhap { get; set; }
        public decimal Iddexuat { get; set; }
        public string Maphieutra { get; set; }
        public string Logfile { get; set; }
        public DateTime? Locktour { get; set; }
    }
}
