using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class DanhGiaNhaHangDTO
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
        public string ViTri { get; set; }
        public string SucChuaToiDa { get; set; }
        public string DinhLuong { get; set; }
        public string ChatLuong { get; set; }
        public string NhaVeSinh { get; set; }
        public string ThaiDoPvcuaNv { get; set; }
        public string CoPvmienPhiNoiBo { get; set; }
        public string CoBaiDoXe { get; set; }
        public bool? DaCoKhaoSatThucTe { get; set; }
        public bool? KqDat { get; set; }
        public bool? KqKhaoSatThem { get; set; }
        public bool? DongYduaVaoDsncu { get; set; }
    }
}