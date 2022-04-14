using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class SD
    {
        public const string Intranets = "Intranets";
        public const string admin = "admin";
        public const string superadmin = "superadmin";
        public const string dieuhanh = "operator";
        public const string sales = "sales";

        public const string CUDV = "CUDV";

        public static List<string> LoaiSao()
        {
            return new List<string>()
            {
                "1*", "2*", "3*", "4*", "5*", "Không rõ", "Homestay"
            };
        }

        public static List<string> LoaiMenu()
        {
            return new List<string>()
            {
                "Âu", "Việt", "Buffet"
            };
        }

        public static List<string> ChatLuongMonAn()
        {
            return new List<string>()
            {
                "Tốt", "Khá", "TB"
            };
        }

        public static List<string> ChatLuongDV()
        {
            return new List<string>()
            {
                "Tốt", "Khá", "TB"
            };
        }

        public static List<string> SanPham()
        {
            return new List<string>()
            {
                "Tốt", "Khá", "TB"
            };
        }

        public static List<string> GiaCa()
        {
            return new List<string>()
            {
                "Cao", "Thấp", "TB"
            };
        }

        public static List<string> MucDoHapDan()
        {
            return new List<string>()
            {
                "Cao", "Thấp", "TB"
            };
        }

        public static List<string> NhaVeSinh()
        {
            return new List<string>()
            {
                "Tốt", "Khá", "TB", "Xấu"
            };
        }

        public static List<string> DoiTuongKhachPhuHop()
        {
            return new List<string>()
            {
                "Nội địa", "Quốc tế"
            };
        }

        public static List<string> KhaNangHuyDong()
        {
            return new List<string>()
            {
                "Cao", "Thấp", "TB"
            };
        }

        //public static List<string> DonViKyKet()
        //{
        //    return new List<string>()
        //    {
        //        "Outbound", "Inbound", "Noidia", "4*", "5*", "Không rõ", "Homestay"
        //    };
        //}
    }
}