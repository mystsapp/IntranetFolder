using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class VTourinf
    {
        public string Sgtcode { get; set; }
        public bool Khachle { get; set; }
        public string CompanyId { get; set; }
        public DateTime Arr { get; set; }
        public DateTime Dep { get; set; }
        public int? Pax { get; set; }
        public int? Childern { get; set; }
        public string Concernto { get; set; }
        public string Operators { get; set; }
        public string Departoperator { get; set; }
        public string Departcreate { get; set; }
        public string Reference { get; set; }
        public string Routing { get; set; }
        public DateTime? Cancel { get; set; }
        public string Cancelnote { get; set; }
        public string PasstypeId { get; set; }
        public decimal? Revenue { get; set; }
        public string Currency { get; set; }
        public decimal? Rate { get; set; }
        public DateTime? Createtour { get; set; }
        public DateTime? Locktour { get; set; }
        public string Userlock { get; set; }
        public string Chinhanh { get; set; }
        public string Chinhanh2 { get; set; }
        public string Chinhanhtao { get; set; }
        public string Dieuhanh2 { get; set; }
        public string Logfile { get; set; }
    }
}
