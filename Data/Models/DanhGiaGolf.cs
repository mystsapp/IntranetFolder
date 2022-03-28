using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class DanhGiaGolf
    {
        public long Id { get; set; }
        public string TenNcu { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public int? TieuChuanSao { get; set; }
        public bool Gpkd { get; set; }
        public bool Vat { get; set; }
        public string ViTri { get; set; }
        public int? SoLuongSanGolf { get; set; }
        public string DienTichSanGolf { get; set; }
        public string MucGiaPhi { get; set; }
        public bool CoNhaHang { get; set; }
        public bool CoXeDien { get; set; }
        public bool CoHoTroTot { get; set; }
        public bool KhaoSatThucTe { get; set; }
        public bool KqDat { get; set; }
        public bool KqKhaoSatThem { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public int? LoaiDvid { get; set; }
        public string SupplierId { get; set; }
        public bool TiemNang { get; set; }
        public bool TaiKy { get; set; }
        public string NguoiDanhGia { get; set; }
        public string LogFile { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
