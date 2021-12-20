using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class TinhDTO
    {
        [MaxLength(3, ErrorMessage = "Mã tỉnh tối đa 3 ký tự")]
        public string Matinh { get; set; }

        public string Tentinh { get; set; }
        public string VungId { get; set; }
        public string MienId { get; set; }
    }
}