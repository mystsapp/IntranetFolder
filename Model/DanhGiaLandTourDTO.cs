using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DanhGiaLandTourDTO
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

        [Display(Name = "KM thi trường ND")]
        public string KinhNghiemThiTruongNd { get; set; }

        [Display(Name = "NL khai thác DV DP")]
        public string NlkhaiThacDvdiaPhuong { get; set; }

        [Display(Name = "Chất lượng DV HDV")]
        public string CldvvaHdv { get; set; }

        [Display(Name = "Sản phẩm")]
        public string SanPham { get; set; }

        [Display(Name = "Giá cả")]
        public string GiaCa { get; set; }

        [Display(Name = "Mức độ kip thời trong GD")]
        public string MucDoKipThoiTrongGd { get; set; }

        [Display(Name = "Mức độ hổ trợ XL sự cố")]
        public string MucDoHoTroXuLySuCo { get; set; }

        [Display(Name = "Đã có KS thực tế")]
        public bool DaCoKhaoSatThucTe { get; set; }

        [Display(Name = "KQ đạt")]
        public bool KqDat { get; set; }

        [Display(Name = "KQ khảo sát thêm")]
        public bool KqKhaoSatThem { get; set; }

        [Display(Name = "Đồng ý đưa vào DS NCU")]
        public bool DongYduaVaoDsncu { get; set; }

        public int LoaiDvid { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public string SupplierId { get; set; } // khong can cung dc

        public SupplierDTO SupplierDTO { get; set; } // chi can cho nay
    }
}