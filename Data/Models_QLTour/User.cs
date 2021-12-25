using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class User
    {
        public string Username { get; set; }
        public string Hoten { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Khachle { get; set; }
        public bool? Khachdoan { get; set; }
        public string Maphong { get; set; }
        public bool Newtour { get; set; }
        public bool Dongtour { get; set; }
        public bool Catalogue { get; set; }
        public bool Booking { get; set; }
        public bool Report { get; set; }
        public bool Showprice { get; set; }
        public bool Print { get; set; }
        public bool Doixe { get; set; }
        public bool Maybay { get; set; }
        public bool Huongdan { get; set; }
        public bool Sales { get; set; }
        public bool Vetq { get; set; }
        public bool Admin { get; set; }
        public bool? Active { get; set; }
        public string Chinhanh { get; set; }
    }
}
