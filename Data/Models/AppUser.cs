using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class AppUser
    {
        public long Id { get; set; }
        public string Mact { get; set; }
        public string Chuongtrinh { get; set; }
        public string Mota { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
