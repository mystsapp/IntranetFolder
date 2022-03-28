using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class SupplierDTO
    {
        public string Code { get; set; }

        //[Display(Name = "Group / Tập đoàn")]
        //public string Tapdoan { get; set; }

        [Display(Name = "Tên giao dịch")]
        public string Tengiaodich { get; set; }

        [Display(Name = "Tên thương mại")]
        public string Tenthuongmai { get; set; }

        [Display(Name = "Tỉnh")]
        public string Tinhtp { get; set; }

        [Display(Name = "Tỉnh")]
        public string TinhtpName { get; set; }

        [Display(Name = "TP / TX")]
        public string Thanhpho { get; set; }

        [Display(Name = "Quốc gia")]
        public string Quocgia { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Diachi { get; set; }

        [Display(Name = "Điện thoại")]
        public string Dienthoai { get; set; }

        public string Fax { get; set; }

        [Display(Name = "Mã số thuế")]
        public string Masothue { get; set; }

        [Display(Name = "Ngành nghề")]
        public string Nganhnghe { get; set; }

        [Display(Name = "Người liên hệ")]
        public string Nguoilienhe { get; set; }

        public string Website { get; set; }
        public string Email { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Trangthai { get; set; }

        public DateTime Ngaytao { get; set; }
        public string Nguoitao { get; set; }
        public string Chinhanh { get; set; }

        [Display(Name = "Ngày hết hạn HĐ")]
        public DateTime? Ngayhethan { get; set; }

        public string Logfile { get; set; }

        [Display(Name = "Tài khoản ngân hàng")]
        public string Tknganhang { get; set; }

        [Display(Name = "Tên ngân hàng")]
        public string Tennganhang { get; set; }

        public string Tour { get; set; }

        [Display(Name = "Nhân sự cudv")]
        public string NguoiTrinhKyHd { get; set; }

        [Display(Name = "T/G đóng mở")]
        public string ThoiGianDongMo { get; set; }

        [Display(Name = "ND đóng mở")]
        public string NoiDungDongMo { get; set; }

        [Display(Name = "Khuyến nghị")]
        public bool KhuyenNghi { get; set; }

        [Display(Name = "Loại sao")]
        public string LoaiSao { get; set; }

        public virtual ICollection<HinhAnhDTO> HinhAnhDTOs { get; set; }
        public virtual ICollection<DanhGiaNhaHangDTO> DanhGiaNhaHangDTOs { get; set; }
        public virtual ICollection<DanhGiaKhachSanDTO> DanhGiaKhachSanDTOs { get; set; }
        public virtual ICollection<DanhGiaLandTourDTO> DanhGiaLandTourDTOs { get; set; }
        public virtual ICollection<DanhGiaDTQDTO> DanhGiaDTQDTOs { get; set; }
        public virtual ICollection<DanhGiaCamLaoDTO> DanhGiaCamLaoDTOs { get; set; }
        public virtual ICollection<DanhGiaVanChuyenDTO> DanhGiaVanChuyenDTOs { get; set; }
        public virtual ICollection<DanhGiaGolfDTO> DanhGiaGolfDTOs { get; set; }
        public virtual ICollection<DanhGiaCruiseDTO> DanhGiaCruiseDTOs { get; set; }
        public virtual ICollection<DichVu1DTO> DichVu1DTOs { get; set; }

        [Display(Name = "Tập đoàn")]
        public int TapDoanId { get; set; }

        public TapDoanDTO TapDoanDTO { get; set; }
    }
}