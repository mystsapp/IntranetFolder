using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class DanhGiaVanChuyen
    {
        public long Id { get; set; }
        public string TenNcu { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public bool? LanDauOrTaiKy { get; set; }
        public string PhapNhan { get; set; }
        public string SoXeChinhThuc { get; set; }
        public string KhaNangHuyDong { get; set; }
        public string DoiXeOrLoaiXe { get; set; }
        public decimal? Gia { get; set; }
        public string KinhNghiem { get; set; }
        public int LoaiDvid { get; set; }
    }
}
