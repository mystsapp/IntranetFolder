using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class LoaiDvDTO
    {
        public int Id { get; set; }

        [Display(Name = "Mã loại")]
        [MaxLength(3, ErrorMessage = "Chiều dài tối đa 3 ký tự.")]
        [Required(ErrorMessage = "Mã không được để trống.")]
        public string MaLoai { get; set; }

        [Display(Name = "Tên loại")]
        [Required(ErrorMessage = "Tên không được để trống.")]
        public string TenLoai { get; set; }

        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }

        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public string LogFile { get; set; }
        public virtual ICollection<DichVu1DTO> DichVu1DTOs { get; set; }
        public virtual ICollection<SupplierDTO> SupplierDTOs { get; set; }
    }
}