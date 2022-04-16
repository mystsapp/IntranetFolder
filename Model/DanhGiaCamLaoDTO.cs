using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DanhGiaCamLaoDTO
    {
        public long Id { get; set; }

        [Display(Name = "Tên NCU")]
        [Remote("IsStringNameAvailable", "DanhGiaNhaHang", ErrorMessage = "Tên này đã tồn tại.", AdditionalFields = "Id")]
        [Required(ErrorMessage = "Tên không được để trống.")]
        public string TenNcu { get; set; }

        [Display(Name = "T/G hoạt động")]
        public string ThoiGianHoatDong { get; set; }

        [Display(Name = "Cac đối tác VN")]
        public string CacDoiTacVn { get; set; }

        [Display(Name = "Tuyến")]
        public string Tuyen { get; set; }

        [Display(Name = "CL DV và HDV")]
        public string CldvvaHdv { get; set; }

        [Display(Name = "Sản phẩm")]
        public string SanPham { get; set; }

        [Display(Name = "Giá cả")]
        public string GiaCa { get; set; }

        [Display(Name = "HT xử lý sự cố")]
        public bool CoHtxuLySuCo { get; set; }

        [Display(Name = "KS thực tế")]
        public bool KhaoSatThucTe { get; set; }

        [Display(Name = "KQ đạt")]
        public bool Kqdat { get; set; }

        [Display(Name = "KQ KS thêm")]
        public bool KqkhaoSatThem { get; set; }

        [Display(Name = "Đồng ý đưa vào DS NCU")]
        public bool DongYduaVaoDsncu { get; set; }

        [Display(Name = "Địa chỉ")]
        public DateTime? NgayTao { get; set; }

        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public int LoaiDvid { get; set; }
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
        public SupplierDTO SupplierDTO { get; set; }
    }
}