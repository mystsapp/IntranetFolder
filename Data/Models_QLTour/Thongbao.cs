using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class Thongbao
    {
        public decimal Id { get; set; }
        public decimal? IdTourProg { get; set; }
        public string Sgtcode { get; set; }
        public string Iddichvu { get; set; }
        public DateTime? Ngaydv { get; set; }
        public string Diengiai { get; set; }
        public string SupplierIdOld { get; set; }
        public string SupplierIdNew { get; set; }
        public string SrvcodeOld { get; set; }
        public string SrvcodeNew { get; set; }
        public string Nguoinhap { get; set; }
        public string Nguoinhan { get; set; }
        public bool Daxem { get; set; }
        public DateTime Ngaycapnhat { get; set; }
    }
}
