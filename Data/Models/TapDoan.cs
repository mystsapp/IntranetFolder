using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class TapDoan
    {
        public TapDoan()
        {
            Suppliers = new HashSet<Supplier>();
        }

        public int Id { get; set; }
        public string Ten { get; set; }
        public string Chuoi { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public string LogFile { get; set; }

        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
