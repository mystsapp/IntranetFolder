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
        public int TieuChuanSao { get; set; }
        public bool Gpkd { get; set; }
        public bool Vat { get; set; }
        public string SucChuaToiDa { get; set; }
        public string ViTri { get; set; }
        public string CoNhaHang { get; set; }
        public bool CoHoBoi { get; set; }
        public bool CoBien { get; set; }
        public string CoPhongHop { get; set; }
        public string ThaiDoPvcuaNv { get; set; }
        public string CoBoTriPhongChoNb { get; set; }
        public string CoBaiDoXe { get; set; }
        public bool DaCoKhaoSatThucTe { get; set; }
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
        public string LogFile { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
