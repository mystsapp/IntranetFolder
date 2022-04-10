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

        [Display(Name = "GPKD")]
        public bool Gpkd { get; set; }

        [Display(Name = "VAT")]
        public bool Vat { get; set; }

        [Display(Name = "KM thi trường ND")]
        public string KinhNghiemThiTruongNd { get; set; }

        [Display(Name = "NL khai thác DV DP")]
        public string NlkhaiThacDvdiaPhuong { get; set; }

        [Display(Name = "Có Khả năng huy động")]
        public bool CoKhaNangHuyDong { get; set; }

        [Display(Name = "Co HT xử lý sự cố")]
        public bool CoHoTroXuLySuCo { get; set; }

        [Display(Name = "Khảo sát thực tế")]
        public bool KhaoSatThucTe { get; set; }

        [Display(Name = "Tuyến")]
        public string Tuyen { get; set; }

        [Display(Name = "T/G hoạt động")]
        public string ThoiGianHoatDong { get; set; }

        [Display(Name = "Các đối tác lớn")]
        public string CacDoiTacLon { get; set; }

        [Display(Name = "Chất lượng DV")]
        public string ChatLuongDichVu { get; set; }

        [Display(Name = "Sản phẩm")]
        public string SanPham { get; set; }

        [Display(Name = "Giá cả")]
        public string GiaCa { get; set; }

        [Display(Name = "KQ đạt")]
        public bool KqDat { get; set; }

        [Display(Name = "Khảo sát thêm")]
        public bool KqKhaoSatThem { get; set; }

        [Display(Name = "Đồng ý đưa vào DS NCU")]
        public bool DongYduaVaoDsncu { get; set; }

        public int LoaiDvid { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public string SupplierId { get; set; } // khong can cung dc

        [Display(Name = "Tiềm năng")]
        public bool TiemNang { get; set; }

        [Display(Name = "Tái ký")]
        public bool TaiKy { get; set; }

        [Display(Name = "Người đánh giá")]
        public string NguoiDanhGia { get; set; }

        public SupplierDTO SupplierDTO { get; set; } // chi can cho nay
    }
}