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

        //public static List<string> DonViKyKet()
        //{
        //    return new List<string>()
        //    {
        //        "Outbound", "Inbound", "Noidia", "4*", "5*", "Không rõ", "Homestay"
        //    };
        //}
    }
}