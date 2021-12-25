using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class Passenger
    {
        public string Sgtcode { get; set; }
        public string Stt { get; set; }
        public string Hoten { get; set; }
        public bool Gioitinh { get; set; }
        public string Quoctich { get; set; }
        public DateTime? Ngaysinh { get; set; }
        public string Hochieu { get; set; }
        public DateTime? NgaycapHc { get; set; }
        public DateTime? HieulucHc { get; set; }
        public string Roomtype { get; set; }
        public DateTime Capnhat { get; set; }
        public string Loginname { get; set; }
        public string Logfile { get; set; }
    }
}
