using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class SightseeingTemp
    {
        public decimal Id { get; set; }
        public string Code { get; set; }
        public int Stt { get; set; }
        public string Codedtq { get; set; }
        public string Serial { get; set; }
        public string Debit { get; set; }
        public decimal Paxprice { get; set; }
        public decimal Childernprice { get; set; }
        public decimal Amount { get; set; }
        public int? Vatin { get; set; }
        public int? Vatout { get; set; }
        public decimal? Srvprofit { get; set; }
        public string Httt { get; set; }
        public string Chinhanh { get; set; }
        public decimal? Idtourprogtemp { get; set; }
    }
}
