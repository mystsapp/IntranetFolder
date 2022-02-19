using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class LoginModel
    {
        public string Username { get; set; }
        public string Mact { get; set; }
        public string Password { get; set; }
        public string Hoten { get; set; }
        public string Dienthoai { get; set; }
        public string Email { get; set; }
        public string Macn { get; set; }
        public string RoleId { get; set; }
        public bool Trangthai { get; set; }
        public bool? Doimk { get; set; }
        public DateTime? Ngaydoimk { get; set; }
        public string Maphong { get; set; }
        public string Macode { get; set; }
        public string Tencn { get; set; }
        public string Diachi { get; set; }
        public string Masothue { get; set; }
        public string Thanhpho { get; set; }
    }
}
