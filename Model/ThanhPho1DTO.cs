using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class ThanhPho1DTO
    {
        [Display(Name = "Mã TP / Thị xã")]
        public string Matp { get; set; }

        [Display(Name = "Tên Thành phố / Thị xã")]
        public string Tentp { get; set; }

        public string Matinh { get; set; }
    }
}