using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class Tourprogtemp
    {
        public decimal Id { get; set; }
        public string Code { get; set; }
        public int? Stt { get; set; }
        public int? Date { get; set; }
        public string Time { get; set; }
        public string Srvtype { get; set; }
        public string Supplierid { get; set; }
        public string Srvcode { get; set; }
        public string TourItem { get; set; }
        public string Srvnode { get; set; }
        public string Currency { get; set; }
        public string Arr { get; set; }
        public string Dep { get; set; }
        public string Carrier { get; set; }
        public string Airtype { get; set; }
        public string Pickuptime { get; set; }
        public decimal Unitpricea { get; set; }
        public decimal Unitpricec { get; set; }
        public decimal Unitpricei { get; set; }
        public int? Foc { get; set; }
        public string Carguide { get; set; }
        public decimal? Amount { get; set; }
        public bool? Debit { get; set; }
        public int? Vatin { get; set; }
        public int? Vatout { get; set; }
        public string Chinhanh { get; set; }
    }
}
