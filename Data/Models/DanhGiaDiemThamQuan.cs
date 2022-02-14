using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class DanhGiaDiemThamQuan
    {
        public long Id { get; set; }
        public string TenNcu { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public bool CoGpkd { get; set; }
        public bool CoHdvat { get; set; }
        public string ViTri { get; set; }
        public string SucChuaToiDa { get; set; }
        public string MucDoHapDan { get; set; }
        public string CoNhaHang { get; set; }
        public string CoNhaVeSinh { get; set; }
        public string ThaiDoPvcuaNv { get; set; }
        public string PhuongTienPvvuiChoi { get; set; }
        public string CoBaiDoXe { get; set; }
        public bool DaCoKhaoSatThucTe { get; set; }
        public bool KqDat { get; set; }
        public bool KqKhaoSatThem { get; set; }
        public bool DongYduaVaoDsncu { get; set; }
        public int LoaiDvid { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public string SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
