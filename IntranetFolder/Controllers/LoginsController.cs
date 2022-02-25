using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;
using Data.Utilities;
using IntranetFolder.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoginModel = IntranetFolder.Models.LoginModel;

namespace IntranetFolder.Controllers
{
    public class LoginsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.userRepository.Login(model.Username, model.Password);

                if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản này không tồn tại");
                    return View();
                }
                else
                {
                    if (result == -1)
                    {
                        ModelState.AddModelError("", "Tài khoản này đã bị khóa");
                        return View();
                    }
                    string modelPass = MaHoaSHA1.EncodeSHA1(model.Password);
                    if (result == -2)
                    {
                        ModelState.AddModelError("", "Mật khẩu không đúng");
                    }
                    if (result == 1)
                    {
                        //var user = _userRepository.GetById(model.Username);
                        var user = await _unitOfWork.userRepository.FindAsync(x => x.Username == model.Username);
                        HttpContext.Session.Set("loginUser", user.FirstOrDefault());

                        //HttpContext.Session.SetString("username", model.Username);
                        HttpContext.Session.SetString("password", model.Password);
                        //HttpContext.Session.SetString("hoten", result.Hoten);
                        //HttpContext.Session.SetString("phong", result.Maphong);
                        HttpContext.Session.SetString("chinhanh", user.SingleOrDefault().Macn);
                        HttpContext.Session.SetString("userId", user.SingleOrDefault().Username.ToString());
                        //HttpContext.Session.SetString("dienthoai", String.IsNullOrEmpty(result.Dienthoai) ? "" : result.Dienthoai);
                        //HttpContext.Session.SetString("macode", result.Macode);
                        //HttpContext.Session.SetString("userRole", user.FirstOrDefault().Role.RoleName);
                        //HttpContext.Session.SetString("Newtour", user.Newtour.ToString());
                        //HttpContext.Session.SetString("Dongtour", user.Dongtour.ToString());
                        //HttpContext.Session.SetString("Danhmuc", user.Catalogue.ToString());
                        //HttpContext.Session.SetString("Booking", user.Booking.ToString());
                        //HttpContext.Session.SetString("Report", user.Report.ToString());
                        //HttpContext.Session.SetString("Showprice", user.Showprice.ToString());
                        //HttpContext.Session.SetString("Print", user.Print.ToString());
                        //HttpContext.Session.SetString("Doixe", user.Doixe.ToString());
                        //HttpContext.Session.SetString("Maybay", user.Maybay.ToString());
                        //HttpContext.Session.SetString("Huongdan", user.Huongdan.ToString());
                        //HttpContext.Session.SetString("Sales", user.Sales.ToString());
                        //HttpContext.Session.SetString("Vetq", user.Vetq.ToString());
                        //HttpContext.Session.SetString("Admin", user.Admin.ToString());
                        //HttpContext.Session.SetString("khachle", user.khachle.ToString());
                        //HttpContext.Session.SetString("khachdoan", user.khachdoan.ToString());

                        //if (!string.IsNullOrEmpty(user.Email))
                        //{
                        //    HttpContext.Session.SetString("Email", user.Email.ToString());
                        //}

                        //DateTime ngaydoimk = Convert.ToDateTime(result.Ngaydoimk);
                        //int kq = (DateTime.Now.Month - ngaydoimk.Month) + 12 * (DateTime.Now.Year - ngaydoimk.Year);
                        //if (kq >= 3)
                        //{
                        //    return View("changepass");
                        //}

                        //if (result.Doimk == true)
                        //{
                        //    return View("changepass");
                        //}
                        //else
                        //{
                        //    return RedirectToAction("Index", "Home");
                        //}

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ChangePass(string strUrl)
        {
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            ChangePassModel changpassmodel = new ChangePassModel
            {
                Username = user.Username,
                StrUrl = strUrl
            };
            return View(changpassmodel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePass(ChangePassModel model, string strUrl)
        {
            if (ModelState.IsValid)
            {
                string oldpass = HttpContext.Session.GetString("password");
                if (MaHoaSHA1.EncodeSHA1(oldpass) != MaHoaSHA1.EncodeSHA1(model.Password))
                {
                    ModelState.AddModelError("", "Mật khẩu cũ không đúng");
                }
                else if (model.Newpassword != model.Confirmpassword)
                {
                    ModelState.AddModelError("", "Mật khẩu nhập lại không đúng.");
                }
                else
                {
                    // change pass

                    // for qltk user
                    var qltkUser = await _unitOfWork.userRepository.GetByIdAsync(model.Username);
                    qltkUser.Password = MaHoaSHA1.EncodeSHA1(model.Newpassword);
                    qltkUser.Ngaydoimk = DateTime.Now;
                    qltkUser.Doimk = false;
                    _unitOfWork.userRepository.Update(qltkUser);
                    // for qltk user

                    var result = await _unitOfWork.Complete();
                    // change pass

                    if (result > 0)
                    {
                        SetAlert("Đổi mật khẩu thành công", "success");
                        return LocalRedirect(strUrl); // /Users/Create : effect with local url
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể đổi mật khẩu.");
                    }
                }
            }
            return View();
        }

        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}