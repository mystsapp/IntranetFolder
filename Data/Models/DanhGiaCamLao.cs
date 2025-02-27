﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class DanhGiaCamLao
    {
        public long Id { get; set; }
        public string TenNcu { get; set; }
        public string ThoiGianHoatDong { get; set; }
        public string CacDoiTacVn { get; set; }
        public string CldvvaHdvtiengViet { get; set; }
        public string Tuyen { get; set; }
        public string CldvvaHdv { get; set; }
        public string SanPham { get; set; }
        public string GiaCa { get; set; }
        public bool CoHtxuLySuCo { get; set; }
        public bool KhaoSatThucTe { get; set; }
        public bool Kqdat { get; set; }
        public bool KqkhaoSatThem { get; set; }
        public bool DongYduaVaoDsncu { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public int LoaiDvid { get; set; }
        public string SupplierId { get; set; }
        public bool? TiemNang { get; set; }
        public bool? TaiKy { get; set; }
        public string NguoiDanhGia { get; set; }
        public DateTime? NgayDanhGia { get; set; }
        public string LogFile { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
