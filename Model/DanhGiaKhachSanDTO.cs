using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DanhGiaKhachSanDTO
    {
        public long Id { get; set; }

        [Display(Name = "Tên NCU")]
        [Remote("IsStringNameAvailable", "DanhGiaNhaHang", ErrorMessage = "Tên này đã tồn tại.", AdditionalFields = "Id")]
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
        public int? TieuChuanSao { get; set; }

        [Display(Name = "Có GPKD")]
        public bool? Gpkd { get; set; }

        [Display(Name = "Có HDVAT")]
        public bool? Vat { get; set; }

        [Display(Name = "Sức chứa tối đa")]
        public string SucChuaToiDa { get; set; }

        [Display(Name = "Vị trí")]
        public string ViTri { get; set; }

        [Display(Name = "Có nhà hàng")]
        public string CoNhaHang { get; set; }

        [Display(Name = "Có hồ bơi")]
        public bool? CoHoBoi { get; set; }

        [Display(Name = "Có biển")]
        public bool? CoBien { get; set; }

        [Display(Name = "Có phòng hộp")]
        public bool? CoPhongHop { get; set; }

        [Display(Name = "Thái độ NV")]
        public string ThaiDoPvcuaNv { get; set; }

        [Display(Name = "Có bố trí phòng NB")]
        public string CoBoTriPhongChoNb { get; set; }

        [Display(Name = "Có bãi đỗ xe")]
        public string CoBaiDoXe { get; set; }

        [Display(Name = "Đã có khảo sát thực tế")]
        public bool? DaCoKhaoSatThucTe { get; set; }

        [Display(Name = "KQ đạt")]
        public bool? KqDat { get; set; }

        [Display(Name = "KQ khảo sát thêm")]
        public string KqKhaoSatThem { get; set; }

        [Display(Name = "Đồng ý đưa vào DS NCU")]
        public bool? DongYduaVaoDsncu { get; set; }

        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public int LoaiDvid { get; set; }
        public string SupplierId { get; set; } // khong can cung dc
        public SupplierDTO SupplierDTO { get; set; } // chi can cho nay
    }
}