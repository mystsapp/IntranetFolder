using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class VListChiphitour
    {
        public long? Id { get; set; }
        public string Dv { get; set; }
        public string Sgtcode { get; set; }
        public string Srvtype { get; set; }
        public string Tengiaodich { get; set; }
        public string Diengiai { get; set; }
        public int? Vatin { get; set; }
        public int? Vatout { get; set; }
        public decimal? Cpusd { get; set; }
        public decimal? Cpvnd { get; set; }
        public bool? Debit { get; set; }
        public string Chinhanh { get; set; }
        public string Chinhanhtao { get; set; }
        public string Chinhanhdh { get; set; }
        public DateTime? Ngaythang { get; set; }
        public string Srvnode { get; set; }
    }
}
