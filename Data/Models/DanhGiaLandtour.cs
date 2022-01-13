using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class DanhGiaLandtour
    {
        public long Id { get; set; }
        public string TenNcu { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public bool? CoGpkd { get; set; }
        public bool? CoHdvat { get; set; }
        public string KinhNghiemThiTruongNd { get; set; }
        public string NlkhaiThacDvdiaPhuong { get; set; }
        public string CldvvaHdv { get; set; }
        public string SanPham { get; set; }
        public string GiaCa { get; set; }
        public string MucDoKipThoiTrongGd { get; set; }
        public string MucDoHoTroXuLySuCo { get; set; }
        public bool? DaCoKhaoSatThucTe1 { get; set; }
        public bool? KqDat1 { get; set; }
        public bool? KqKhaoSatThem1 { get; set; }
        public bool? DongYduaVaoDsncu { get; set; }
        public int LoaiDvid1 { get; set; }
        public string TenDv { get; set; }
    }
}
