using Data.Models;
using Data.Repository;
using Data.Utilities;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetFolder.Controllers
{
    public class TapDoanController : BaseController
    {
        private readonly ITapDoanService _tapDoanService;

        [BindProperty]
        public TapDoanViewModel TapDoanVM { get; set; }

        public TapDoanController(ITapDoanService tapDoanService)
        {
            TapDoanVM = new TapDoanViewModel()
            {
                TapDoanDTO = new TapDoanDTO()
            };
            _tapDoanService = tapDoanService;
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, int id, int page = 1)
        {
            if (id == 0)
            {
                ViewBag.id = "";
            }

            TapDoanVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            TapDoanVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (id != 0) // for redirect with id
            {
                TapDoanVM.TapDoanDTO = await _tapDoanService.GetByIdAsync(id);
                ViewBag.id = TapDoanVM.TapDoanDTO.Id;
            }
            else
            {
                TapDoanVM.TapDoanDTO = new TapDoanDTO();
            }
            TapDoanVM.TapDoanDTOs = await _tapDoanService.ListTapDoan(searchString, searchFromDate, searchToDate, page);
            return View(TapDoanVM);
        }

        public IActionResult Create(string strUrl)
        {
            TapDoanVM.StrUrl = strUrl;

            return View(TapDoanVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                TapDoanVM = new TapDoanViewModel()
                {
                    TapDoanDTO = new TapDoanDTO(),
                    StrUrl = strUrl
                };

                return View(TapDoanVM);
            }

            // ghi log
            TapDoanVM.TapDoanDTO.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _tapDoanService.CreateAsync(TapDoanVM.TapDoanDTO); // save

                SetAlert("Thêm mới thành công.", "success");

                return Redirect(strUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.InnerException.Message, "error");
                return View(TapDoanVM);
            }
        }

        public async Task<IActionResult> Edit(int id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            TapDoanVM.StrUrl = strUrl;
            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            TapDoanVM.TapDoanDTO = await _tapDoanService.GetByIdAsync(id);

            if (TapDoanVM.TapDoanDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return View(TapDoanVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id != TapDoanVM.TapDoanDTO.Id)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                #region log file

                string temp = "", log = "";

                //var t = _unitOfWork.tourRepository.GetById(id);
                var t = _tapDoanService.GetByIdAsNoTracking(id);

                if (t.Ten != TapDoanVM.TapDoanDTO.Ten)
                {
                    temp += String.Format("- Ten thay đổi: {0}->{1}", t.Ten, TapDoanVM.TapDoanDTO.Ten);
                }

                if (t.Chuoi != TapDoanVM.TapDoanDTO.Chuoi)
                {
                    temp += String.Format("- Chuoi thay đổi: {0}->{1}", t.Chuoi, TapDoanVM.TapDoanDTO.Chuoi);
                }

                #endregion log file

                // kiem tra thay doi
                if (temp.Length > 0)
                {
                    log = System.Environment.NewLine;
                    log += "=============";
                    log += System.Environment.NewLine;
                    log += temp + " -User cập nhật : " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // username
                    t.LogFile = t.LogFile + log;
                    TapDoanVM.TapDoanDTO.LogFile = t.LogFile;
                }

                try
                {
                    await _tapDoanService.UpdateAsync(TapDoanVM.TapDoanDTO);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(TapDoanVM);
                }
            }
            // not valid

            return View(TapDoanVM);
        }

        public JsonResult IsStringNameAvailable(string TenCreate)
        {
            var boolName = _tapDoanService.GetAll()
                .Where(x => x.Ten.Trim().ToLower() == TenCreate.Trim().ToLower())
                .FirstOrDefault();
            if (boolName == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id, string strUrl)
        //{
        //    TapDoanVM.StrUrl = strUrl;

        //    var TinhDTO = await _tinhTPService.GetByIdAsync(id);
        //    if (TinhDTO == null)
        //        return NotFound();
        //    try
        //    {
        //        await _tinhTPService.Delete(TinhDTO);

        //        SetAlert("Xóa thành công.", "success");
        //        return Redirect(TapDoanVM.StrUrl);
        //    }
        //    catch (Exception ex)
        //    {
        //        SetAlert(ex.Message, "error");
        //        ModelState.AddModelError("", ex.Message);
        //        return Redirect(TapDoanVM.StrUrl);
        //    }
        //}
    }
}