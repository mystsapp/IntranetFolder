using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DanhGiaDTQDTO
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
        public bool? CoGpkd { get; set; }

        [Display(Name = "Có HDVAT")]
        public bool? CoHdvat { get; set; }

        [Display(Name = "Vị trí")]
        public string ViTri { get; set; }

        [Display(Name = "Sức chứa tối đa")]
        public string SucChuaToiDa { get; set; }

        [Display(Name = "Múc độ đáp ứng")]
        public string MucDoHapDan { get; set; }

        [Display(Name = "Có nhà hàng")]
        public string CoNhaHang { get; set; }

        [Display(Name = "Có nhà vệ sinh")]
        public string CoNhaVeSinh { get; set; }

        [Display(Name = "Thái độ PV của NV")]
        public string ThaiDoPvcuaNv { get; set; }

        [Display(Name = "Phương tiện PV vui chơi")]
        public string PhuongTienPvvuiChoi { get; set; }

        [Display(Name = "Có bãi xe")]
        public string CoBaiDoXe { get; set; }

        [Display(Name = "Đã có KS thực tế")]
        public bool? DaCoKhaoSatThucTe { get; set; }

        [Display(Name = "KQ đạt")]
        public bool? KqDat { get; set; }

        [Display(Name = "KQ khảo sát thêm")]
        public bool? KqKhaoSatThem { get; set; }

        [Display(Name = "Đồng ý đưa vào DS NCU")]
        public bool? DongYduaVaoDsncu { get; set; }

        public int LoaiDvid { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public string SupplierId { get; set; }

        public SupplierDTO SupplierDTO { get; set; }
    }
}