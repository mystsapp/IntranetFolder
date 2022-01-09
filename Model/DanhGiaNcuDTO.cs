using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DanhGiaNcuDTO
    {
        public long Id { get; set; }

        [Display(Name = "Tên NCU")]
        [Required(ErrorMessage = "Tên không được để trống.")]
        public string TenNcu { get; set; }

        public string KnngheNghiep { get; set; }
        public string KntaiThiTruongVn { get; set; }
        public string NlkhaiThacDvtaiDiaPhuong { get; set; }
        public string CldvvaHdvtiengViet { get; set; }
        public string SanPham { get; set; }
        public string GiaCa { get; set; }
        public string MucDoKipThoiTrongGd { get; set; }
        public string MucDoHtxuLySuCo { get; set; }
        public string DaCoKhaoSatThucTe { get; set; }
        public bool? Kqdat { get; set; }
        public bool? KqkhaoSatThem { get; set; }
        public bool? DongYduaVaoDsncu { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string LanDauOrTaiKy { get; set; }
        public string PhapNhan { get; set; }
        public string SoXeChinhThuc { get; set; }
        public string KhaNangHuyDong { get; set; }
        public string DoiXeOrLoaiXe { get; set; }
        public decimal? Gia { get; set; }
        public string KinhNghiem { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public int LoaiDvid { get; set; }
        public string TenDv { get; set; }
    }
}