using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class SupplierDTO
    {
        public string Code { get; set; }

        [Display(Name = "Group / Tập đoàn")]
        public string Tapdoan { get; set; }

        [Display(Name = "Tên giao dịch")]
        public string Tengiaodich { get; set; }

        [Display(Name = "Tên thương mại")]
        public string Tenthuongmai { get; set; }

        [Display(Name = "Tỉnh")]
        public string Tinhtp { get; set; }

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

        [Display(Name = "Người trình ký HĐ")]
        public string NguoiTrinhKyHd { get; set; }

        public virtual ICollection<HinhAnhDTO> HinhAnhDTOs { get; set; }
        public virtual ICollection<DanhGiaNhaHangDTO> DanhGiaNhaHangDTOs { get; set; }
        public virtual ICollection<DanhGiaKhachSanDTO> DanhGiaKhachSanDTOs { get; set; }
        public virtual ICollection<DanhGiaLandTourDTO> DanhGiaLandTourDTOs { get; set; }
        public virtual ICollection<DanhGiaDTQDTO> DanhGiaDTQDTOs { get; set; }
    }
}