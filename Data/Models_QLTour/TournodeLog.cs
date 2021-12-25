using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class TournodeLog
    {
        public decimal Id { get; set; }
        public string Sgtcode { get; set; }
        public string Headernode { get; set; }
        public string Footernode { get; set; }
        public DateTime Ngaynhap { get; set; }
    }
}
