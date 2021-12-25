using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class Tourtemplate
    {
        public string Code { get; set; }
        public string Tourkind { get; set; }
        public string Tentour { get; set; }
        public string Tuyentq { get; set; }
        public string Chudetour { get; set; }
        public int Songay { get; set; }
        public string Chinhanh { get; set; }
        public string Nguoitao { get; set; }
        public DateTime Ngaytao { get; set; }
    }
}
