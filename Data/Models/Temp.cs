using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class Temp
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public DateTime? Date { get; set; }
        public string Field { get; set; }
        public string Supplierid { get; set; }
        public string Paymentcode { get; set; }
        public int? Room { get; set; }
        public int? Level { get; set; }
        public string Website { get; set; }
    }
}
