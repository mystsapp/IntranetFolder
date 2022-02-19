using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class HinhAnh
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string DichVuId { get; set; }

        public virtual DichVu1 DichVu { get; set; }
    }
}
