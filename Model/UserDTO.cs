using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Hoten { get; set; }
        public string Dienthoai { get; set; }
        public string Email { get; set; }
        public string Maphong { get; set; }
        public string Macn { get; set; }
        public string RoleId { get; set; }
        public bool Trangthai { get; set; }
        public DateTime? Ngaytao { get; set; }
        public string Nguoitao { get; set; }
    }
}