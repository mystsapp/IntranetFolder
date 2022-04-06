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
        [Remote("IsStringNameAvailable", "DanhGiaKhachSan", ErrorMessage = "Tên này đã tồn tại.", AdditionalFields = "Id")]
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
        public string TieuChuanSao { get; set; }

        [Display(Name = "Có GPKD")]
        public bool Gpkd { get; set; }

        [Display(Name = "Có HDVAT")]
        public bool Vat { get; set; }

        [Display(Name = "Sức chứa tối đa")]
        public string SucChuaToiDa { get; set; }

        [Display(Name = "Vị trí")]
        public string ViTri { get; set; }

        [Display(Name = "Có nhà hàng")]
        public string CoNhaHang { get; set; }

        [Display(Name = "Hồ bơi")]
        public bool HoBoi { get; set; }

        [Display(Name = "Bãi biển riêng")]
        public bool BaiBienRieng { get; set; }

        [Display(Name = "Số chỗ phòng họp")]
        public string SoChoPhongHop { get; set; }

        [Display(Name = "Thái độ NV")]
        public string ThaiDoPvcuaNv { get; set; }

        [Display(Name = "Phòng nội bộ")]
        public bool PhongNoiBo { get; set; }

        [Display(Name = "Bãi đỗ xe")]
        public string BaiDoXe { get; set; }

        [Display(Name = "Khao sát thực tế")]
        public bool KhaoSatThucTe { get; set; }

        [Display(Name = "KQ đạt")]
        public bool KqDat { get; set; }

        [Display(Name = "Khảo sát thêm")]
        public string KqKhaoSatThem { get; set; }

        [Display(Name = "Đồng ý đưa vào DS NCU")]
        public bool DongYduaVaoDsncu { get; set; }

        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public int LoaiDvid { get; set; }
        public string SupplierId { get; set; } // khong can cung dc

        [Display(Name = "Tiềm năng")]
        public bool TiemNang { get; set; }

        [Display(Name = "Tái ký")]
        public bool TaiKy { get; set; }

        [Display(Name = "Người đánh giá")]
        public string NguoiDanhGia { get; set; }

        [Display(Name = "Ngày đánh giá")]
        public DateTime? NgayDanhGia { get; set; }

        [Display(Name = "SL phòng")]
        public int SLPhong { get; set; }

        [Display(Name = "SL nhà hàng")]
        public int SLNhaHang { get; set; }

        public SupplierDTO SupplierDTO { get; set; } // chi can cho nay
    }
}