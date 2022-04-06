using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class DanhGiaKhachSan
    {
        public long Id { get; set; }
        public string TenNcu { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string TieuChuanSao { get; set; }
        public bool Gpkd { get; set; }
        public bool Vat { get; set; }
        public string SucChuaToiDa { get; set; }
        public string ViTri { get; set; }
        public int? SlnhaHang { get; set; }
        public int? Slphong { get; set; }
        public bool HoBoi { get; set; }
        public bool BaiBienRieng { get; set; }
        public string SoChoPhongHop { get; set; }
        public string ThaiDoPvcuaNv { get; set; }
        public bool PhongNoiBo { get; set; }
        public bool BaiDoXe { get; set; }
        public bool KhaoSatThucTe { get; set; }
        public bool KqDat { get; set; }
        public string KqKhaoSatThem { get; set; }
        public bool DongYduaVaoDsncu { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public int LoaiDvid { get; set; }
        public string SupplierId { get; set; }
        public bool? TiemNang { get; set; }
        public bool? TaiKy { get; set; }
        public string NguoiDanhGia { get; set; }
        public DateTime? NgayDanhGia { get; set; }
        public string LogFile { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
