using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class DanhGiaNhaHangDTO
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

        [Display(Name = "Có GPKD")]
        public bool CoGpkd { get; set; }

        [Display(Name = "Có HDVAT")]
        public bool CoHdvat { get; set; }

        [Display(Name = "Vị trí")]
        public string ViTri { get; set; }

        [Display(Name = "Sức chứa tối đa")]
        public string SucChuaToiDa { get; set; }

        [Display(Name = "Định lượng")]
        public string DinhLuong { get; set; }

        [Display(Name = "Chất lượng")]
        public string ChatLuong { get; set; }

        [Display(Name = "Nhà vệ sinh")]
        public string NhaVeSinh { get; set; }

        [Display(Name = "Thái độ pv của nv")]
        public string ThaiDoPvcuaNv { get; set; }

        [Display(Name = "PV miễn phí NB")]
        public string CoPvmienPhiNoiBo { get; set; }

        [Display(Name = "Có bãi đỗ xe")]
        public string CoBaiDoXe { get; set; }

        [Display(Name = "Đã có KS thực tế")]
        public bool DaCoKhaoSatThucTe { get; set; }

        [Display(Name = "Kq đạt")]
        public bool KqDat { get; set; }

        [Display(Name = "Kq khảo sát thêm")]
        public bool KqKhaoSatThem { get; set; }

        [Display(Name = "Đua vào DS NCU")]
        public bool DongYduaVaoDsncu { get; set; }

        public int LoaiDvid { get; set; }
        public string TenDv { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }

        public SupplierDTO SupplierDTO { get; set; }
    }
}