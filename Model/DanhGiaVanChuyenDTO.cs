using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DanhGiaVanChuyenDTO
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

        [Display(Name = "Đời xe cũ nhất / mới nhất")]
        public string DoiXeCuNhatMoiNhat { get; set; }

        [Display(Name = "Loại xe nhiều nhất")]
        public string LoaiXeCoNhieuNhat { get; set; }

        [Display(Name = "Giá cả phù hợp")]
        public bool GiaCaPhuHop { get; set; }

        [Display(Name = "Danh sách đối tác")]
        public string DanhSachDoiTac { get; set; }

        [Display(Name = "Lần đâu / tái ký")]
        public bool LanDauOrTaiKy { get; set; }

        [Display(Name = "Pháp nhân")]
        public string PhapNhan { get; set; }

        [Display(Name = "Số xe chính thức")]
        public string SoXeChinhThuc { get; set; }

        [Display(Name = "Khả năng huy động")]
        public string KhaNangHuyDong { get; set; }

        [Display(Name = "Đời xe / loại xe")]
        public string DoiXeOrLoaiXe { get; set; }

        [Display(Name = "Giá")]
        public decimal? Gia { get; set; }

        [Display(Name = "Kinh nghiệm")]
        public string KinhNghiem { get; set; }

        [Display(Name = "Khảo sát thực tế")]
        public bool KhaoSatThucTe { get; set; }

        [Display(Name = "KQ đạt")]
        public bool KqDat { get; set; }

        [Display(Name = "KQ khảo sát thêm")]
        public bool KqKhaoSatThem { get; set; }

        public int LoaiDvid { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public string SupplierId { get; set; }

        [Display(Name = "Tiềm năng")]
        public bool TiemNang { get; set; }

        [Display(Name = "Tái ký")]
        public bool TaiKy { get; set; }

        [Display(Name = "Người đánh giá")]
        public string NguoiDanhGia { get; set; }

        public SupplierDTO SupplierDTO { get; set; }
    }
}