using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class LoaiDv
    {
        public int Id { get; set; }
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public string GhiChu { get; set; }
    }
}
