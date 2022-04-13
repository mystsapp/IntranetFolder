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

        [Display(Name = "GPKD")]
        public bool Gpkd { get; set; }

        [Display(Name = "VAT")]
        public bool Vat { get; set; }

        [Display(Name = "Nhà vệ sinh")]
        public string NhaVeSinh { get; set; }

        [Display(Name = "Thái độ PV của NV")]
        public string ThaiDoPvcuaNv { get; set; }

        [Display(Name = "Phương tiện PV vui chơi")]
        public string PhuongTienPvvuiChoi { get; set; }

        [Display(Name = "Bãi đổ xe")]
        public bool BaiDoXe { get; set; }

        [Display(Name = "Chế độ HDV")]
        public bool CoCheDoHdv { get; set; }

        [Display(Name = "KS thực tế")]
        public bool KhaoSatThucTe { get; set; }

        [Display(Name = "Số chỗ tối đa")]
        public int SoChoToiDa { get; set; }

        [Display(Name = "Mức độ hấp dẫn")]
        public string MucDoHapDan { get; set; }

        [Display(Name = "SL nhà hàng")]
        public int SoLuongNhaHang { get; set; }

        [Display(Name = "Đ/T khách phù hợp")]
        public string DoiTuongKhachPhuHop { get; set; }

        [Display(Name = "Vị trí")]
        public string ViTri { get; set; }

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
        public string SupplierId { get; set; }

        [Display(Name = "Tiềm năng")]
        public bool TiemNang { get; set; }

        [Display(Name = "Tái ký")]
        public bool TaiKy { get; set; }

        [Display(Name = "Người đánh giá")]
        public string NguoiDanhGia { get; set; }

        [Display(Name = "Ngày đánh giá")]
        public DateTime? NgayDanhGia { get; set; }

        public SupplierDTO SupplierDTO { get; set; }
    }
}