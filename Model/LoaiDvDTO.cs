using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class LoaiDvDTO
    {
        public int Id { get; set; }
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public string GhiChu { get; set; }
        public virtual ICollection<DichVu1DTO> DichVu1DTOs { get; set; }
    }
}