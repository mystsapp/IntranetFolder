using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            DanhGiaCamLaos = new HashSet<DanhGiaCamLao>();
            DanhGiaDiemThamQuans = new HashSet<DanhGiaDiemThamQuan>();
            DanhGiaKhachSans = new HashSet<DanhGiaKhachSan>();
            DanhGiaLandtours = new HashSet<DanhGiaLandtour>();
            DanhGiaNhaHangs = new HashSet<DanhGiaNhaHang>();
            DanhGiaVanChuyens = new HashSet<DanhGiaVanChuyen>();
            DichVu1s = new HashSet<DichVu1>();
        }

        public string Code { get; set; }
        public string Tapdoan { get; set; }
        public string Tengiaodich { get; set; }
        public string Tenthuongmai { get; set; }
        public string Tinhtp { get; set; }
        public string Thanhpho { get; set; }
        public string Quocgia { get; set; }
        public string Diachi { get; set; }
        public string Dienthoai { get; set; }
        public string Fax { get; set; }
        public string Masothue { get; set; }
        public string Nganhnghe { get; set; }
        public string Nguoilienhe { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public bool Trangthai { get; set; }
        public DateTime Ngaytao { get; set; }
        public string Nguoitao { get; set; }
        public string Chinhanh { get; set; }
        public DateTime? Ngayhethan { get; set; }
        public string Logfile { get; set; }
        public string Tknganhang { get; set; }
        public string Tennganhang { get; set; }
        public string Tour { get; set; }
        public string NguoiTrinhKyHd { get; set; }
        public string ThoiGianDongMo { get; set; }
        public string NoiDungDongMo { get; set; }
        public bool KhuyenNghi { get; set; }
        public string LoaiSao { get; set; }
        public int? TapDoanId { get; set; }

        public virtual TapDoan TapDoan { get; set; }
        public virtual ICollection<DanhGiaCamLao> DanhGiaCamLaos { get; set; }
        public virtual ICollection<DanhGiaDiemThamQuan> DanhGiaDiemThamQuans { get; set; }
        public virtual ICollection<DanhGiaKhachSan> DanhGiaKhachSans { get; set; }
        public virtual ICollection<DanhGiaLandtour> DanhGiaLandtours { get; set; }
        public virtual ICollection<DanhGiaNhaHang> DanhGiaNhaHangs { get; set; }
        public virtual ICollection<DanhGiaVanChuyen> DanhGiaVanChuyens { get; set; }
        public virtual ICollection<DichVu1> DichVu1s { get; set; }
    }
}
