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
        public bool? Gpkd { get; set; }
        public bool? Vat { get; set; }
        public string DoiXeCuNhatMoiNhat { get; set; }
        public string LoaiXeCoNhieuNhat { get; set; }
        public bool? GiaCaPhuHop { get; set; }
        public string DanhSachDoiTac { get; set; }
        public bool LanDauOrTaiKy { get; set; }
        public string PhapNhan { get; set; }
        public string SoXeChinhThuc { get; set; }
        public string KhaNangHuyDong { get; set; }
        public string DoiXeOrLoaiXe { get; set; }
        public decimal? Gia { get; set; }
        public string KinhNghiem { get; set; }
        public int LoaiDvid { get; set; }
        public bool? KhaoSatThucTe { get; set; }
        public bool? KqDat { get; set; }
        public bool? KqKhaoSatThem { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public string SupplierId { get; set; }
        public bool? TiemNang { get; set; }
        public bool? TaiKy { get; set; }
        public string NguoiDanhGia { get; set; }
        public DateTime? NgayDanhGia { get; set; }
        public string LogFile { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
