using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DichVu1DTO
    {
        public string MaDv { get; set; }
        public string TenDv { get; set; }
        public string DonViKyKet { get; set; }
        public string QuocGia { get; set; }
        public string Mien { get; set; }
        public string Tinh { get; set; }
        public string ThanhPho { get; set; }
        public string LoaiSao { get; set; }
        public string ThongTinLien { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public string NoiDung { get; set; }
        public string NguoiLienHe { get; set; }
        public string SupplierId { get; set; }
        public string Website { get; set; }
        public bool DaKy { get; set; }
        public bool HoatDong { get; set; }
        public string Tuyen { get; set; }
        public string LoaiTau { get; set; }
        public bool DauXe { get; set; }
        public string GhiChu { get; set; }
        public decimal? GiaHd { get; set; }
        public string LoaiHd { get; set; }
        public int? LoaiDvid { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public string LogFile { get; set; }

        public LoaiDvDTO LoaiDvDTO { get; set; }
        public SupplierDTO SupplierDTO { get; set; }
        public virtual ICollection<HinhAnhDTO> HinhAnhDTOs { get; set; }
    }
}