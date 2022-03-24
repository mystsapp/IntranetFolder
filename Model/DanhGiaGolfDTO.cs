using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DanhGiaGolfDTO
    {
        public long Id { get; set; }

        [Display(Name = "Tên NCU")]
        [Remote("IsStringNameAvailable", "DanhGiaGolf", ErrorMessage = "Tên này đã tồn tại.", AdditionalFields = "Id")]
        [Required(ErrorMessage = "Tên không được để trống.")]
        public string TenNcu { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DienThoai { get; set; }

        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        [Display(Name = "Tiêu chuẩn sao")]
        public int? TieuChuanSao { get; set; }

        [Display(Name = "GPKD")]
        public bool Gpkd { get; set; }

        [Display(Name = "VAT")]
        public bool Vat { get; set; }

        [Display(Name = "Vị trí")]
        public string ViTri { get; set; }

        [Display(Name = "SL sân Golf")]
        public int? SoLuongSanGolf { get; set; }

        [Display(Name = "DT sân Golf")]
        public string DienTichSanGolf { get; set; }

        [Display(Name = "Mức giá phí")]
        public string MucGiaPhi { get; set; }

        [Display(Name = "Có nhà hàng")]
        public bool CoNhaHang { get; set; }

        [Display(Name = "Có xe điện")]
        public bool CoXeDien { get; set; }

        [Display(Name = "Có hổ trợ tốt")]
        public bool CoHoTroTot { get; set; }

        [Display(Name = "Khảo sát thực tế")]
        public bool KhaoSatThucTe { get; set; }

        [Display(Name = "KQ đạt")]
        public bool KqDat { get; set; }

        [Display(Name = "KQ khảo sát thêm")]
        public bool KqKhaoSatThem { get; set; }

        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }

        [Display(Name = "Loại dv")]
        public int? LoaiDvid { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierId { get; set; }

        [Display(Name = "Tiềm năng")]
        public bool TiemNang { get; set; }

        [Display(Name = "Tái ký")]
        public bool TaiKy { get; set; }

        [Display(Name = "Người đánh giá")]
        public string NguoiDanhGia { get; set; }

        public SupplierDTO SupplierDTO { get; set; } // chi can cho nay
    }
}