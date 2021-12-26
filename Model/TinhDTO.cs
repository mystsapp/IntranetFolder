using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class TinhDTO
    {
        [MaxLength(3, ErrorMessage = "Mã tỉnh tối đa 3 ký tự")]
        [Display(Name = "Mã")]
        public string Matinh { get; set; }

        [Display(Name = "Tên thành phố / Quốc gia")]
        public string Tentinh { get; set; }

        [Display(Name = "Khu vực")]
        public string VungId { get; set; }

        [Display(Name = "Bộ")]
        public string MienId { get; set; }
    }
}