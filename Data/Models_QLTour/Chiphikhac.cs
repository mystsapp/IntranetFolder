using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class Chiphikhac
    {
        public decimal Idorthercost { get; set; }
        public string Sgtcode { get; set; }
        public int? Fromdate { get; set; }
        public int? Todate { get; set; }
        public string Srvtype { get; set; }
        public string Srvcode { get; set; }
        public string TourItem { get; set; }
        public int Quantity { get; set; }
        public decimal Unitprice { get; set; }
        public int Km { get; set; }
        public int Guidedays { get; set; }
        public decimal Amount { get; set; }
        public bool? Debit { get; set; }
        public bool? Credit { get; set; }
        public string Currency { get; set; }
        public int Vatin { get; set; }
        public int Vatout { get; set; }
        public decimal? Srvprofit { get; set; }
        public string Srvnode { get; set; }
        public string Logfile { get; set; }
        public string Chinhanh { get; set; }
        public bool? Del { get; set; }
        public DateTime? Locktour { get; set; }
    }
}
