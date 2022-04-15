using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DanhGiaCruiseDTO
    {
        public long Id { get; set; }

        [Display(Name = "Tên NCU")]
        [Remote("IsStringNameAvailable", "DanhGiaCruise", ErrorMessage = "Tên này đã tồn tại.", AdditionalFields = "Id")]
        [Required(ErrorMessage = "Tên không được để trống.")]
        public string TenNcu { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Điện thoại")]
        public string DienThoai { get; set; }

        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        [Display(Name = "Tiêu chuẩn sao")]
        public string TieuChuanSao { get; set; }

        [Display(Name = "GPKD")]
        public bool Gpkd { get; set; }

        [Display(Name = "VAT")]
        public bool Vat { get; set; }

        [Display(Name = "SL tàu ngủ đêm")]
        public string SoLuongTauNguDem { get; set; }

        [Display(Name = "Chương trình / hành trình / hải trình")]
        public string ChuongTrinh { get; set; }

        [Display(Name = "Sức chứa tàu ngủ đêm")]
        public string SucChuaTauNguDem { get; set; }

        [Display(Name = "Loại tàu")]
        public string LoaiTau { get; set; }

        [Display(Name = "SL tau TQ ngày")]
        public string SoLuongTauTqngay { get; set; }

        [Display(Name = "Sức chứa tàu TQ ngày")]
        public string SucChuaTauTqngay { get; set; }

        [Display(Name = "Giá cả")]
        public string GiaCa { get; set; }

        [Display(Name = "Cabine có ban công")]
        public bool CabineCoBanCong { get; set; }

        [Display(Name = "Cảng đón khách")]
        public string CangDonKhach { get; set; }

        [Display(Name = "Có HT tốt")]
        public bool CoHoTroTot { get; set; }

        [Display(Name = "Khảo sát thự tế")]
        public bool KhaoSatThucTe { get; set; }

        [Display(Name = "KQ đạt")]
        public bool KqDat { get; set; }

        [Display(Name = "KQ KS thêm")]
        public bool KqKhaoSatThem { get; set; }

        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }

        [Display(Name = "Loai dv")]
        public int LoaiDvid { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierId { get; set; }

        [Display(Name = "Tiềm năng")]
        public bool TiemNang { get; set; }

        [Display(Name = "Tái ký")]
        public bool TaiKy { get; set; }

        [Display(Name = "Người đánh giá")]
        public string NguoiDanhGia { get; set; }

        [Display(Name = "Ngày đánh giá")]
        public DateTime? NgayDanhGia { get; set; }

        public string LogFile { get; set; }

        public SupplierDTO SupplierDTO { get; set; } // chi can cho nay
    }
}