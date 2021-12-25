using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class Huongdan
    {
        public decimal IdHuongdan { get; set; }
        public string Sgtcode { get; set; }
        public int? Stt { get; set; }
        public DateTime Ngayyeucau { get; set; }
        public string Tenhd { get; set; }
        public string Dienthoai { get; set; }
        public string Ngoaingu { get; set; }
        public string Hopdongcty { get; set; }
        public DateTime? Batdau { get; set; }
        public string Batdautai { get; set; }
        public DateTime? Ketthuc { get; set; }
        public string Ketthuctai { get; set; }
        public bool Suottuyen { get; set; }
        public string Ghichu { get; set; }
        public string Ndcongviec { get; set; }
        public string Loaitien { get; set; }
        public decimal Phidontien { get; set; }
        public decimal Phididoan { get; set; }
        public decimal Traphi { get; set; }
        public string Chinhanh { get; set; }
        public string Logfile { get; set; }
        public bool Del { get; set; }
        public string Hochieu { get; set; }
        public DateTime? Hieuluchc { get; set; }
        public DateTime? Ngaysinh { get; set; }
        public bool? Phai { get; set; }
        public bool Vemaybay { get; set; }
        public string Phongks { get; set; }
        public string Quoctich { get; set; }
        public DateTime? Locktour { get; set; }
    }
}
