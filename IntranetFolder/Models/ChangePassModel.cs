using System.ComponentModel.DataAnnotations;

namespace IntranetFolder.Models
{
    public class ChangePassModel
    {
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập password cũ")]
        public string Password { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Vui lòng nhập password mới")]
        public string Newpassword { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Vui lòng nhập lại password mới")]
        [Compare("Newpassword", ErrorMessage = "Mật khẩu củ và mật khẩu mới không trùng khớp")]
        public string Confirmpassword { get; set; }

        public string StrUrl { get; set; }
    }
}