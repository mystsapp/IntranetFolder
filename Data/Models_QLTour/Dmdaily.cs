using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class Dmdaily
    {
        public int Id { get; set; }
        public string Daily { get; set; }
        public string Tendaily { get; set; }
        public string Diachi { get; set; }
        public string Dienthoai { get; set; }
        public string Fax { get; set; }
        public string Macn { get; set; }
        public bool Trangthai { get; set; }
    }
}
