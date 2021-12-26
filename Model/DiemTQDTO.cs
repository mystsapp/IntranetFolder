using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DiemTQDTO
    {
        public string Code { get; set; }

        [Display(Name = "Điểm tham quan")]
        public string Diemtq { get; set; }

        public string Tinhtp { get; set; }

        [Display(Name = "Thành phố / Thị xã")]
        public string Thanhpho { get; set; }

        [Display(Name = "Giá vé người lớn")]
        public decimal Giave { get; set; }

        [Display(Name = "Giá vé trẻ em")]
        public decimal Giatreem { get; set; }

        [Display(Name = "Đơn vị triển khai")]
        public string Congno { get; set; }

        [Display(Name = "Vat vào")]
        public int Vatvao { get; set; }

        [Display(Name = "Vat ra")]
        public int Vatra { get; set; }

        public decimal Tilelai { get; set; }
        public string Logfile { get; set; }
    }
}