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
    public class LoaiDVController : BaseController
    {
        private readonly ILoaiDvService _tapDoanService;

        [BindProperty]
        public LoaiDvViewModel LoaiDvVM { get; set; }

        public LoaiDVController(ILoaiDvService tapDoanService)
        {
            LoaiDvVM = new LoaiDvViewModel()
            {
                LoaiDvDTO = new LoaiDvDTO()
            };
            _tapDoanService = tapDoanService;
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, int id, int page = 1)
        {
            if (id == 0)
            {
                ViewBag.id = "";
            }

            LoaiDvVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            LoaiDvVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (id != 0) // for redirect with id
            {
                LoaiDvVM.LoaiDvDTO = await _tapDoanService.GetByIdAsync(id);
                ViewBag.id = LoaiDvVM.LoaiDvDTO.Id;
            }
            else
            {
                LoaiDvVM.LoaiDvDTO = new LoaiDvDTO();
            }
            LoaiDvVM.LoaiDvDTOs = await _tapDoanService.ListLoaiDv(searchString, searchFromDate, searchToDate, page);
            return View(LoaiDvVM);
        }

        public IActionResult Create(string strUrl)
        {
            LoaiDvVM.StrUrl = strUrl;

            return View(LoaiDvVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                LoaiDvVM = new LoaiDvViewModel()
                {
                    LoaiDvDTO = new LoaiDvDTO(),
                    StrUrl = strUrl
                };

                return View(LoaiDvVM);
            }

            // ghi log
            LoaiDvVM.LoaiDvDTO.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username
            LoaiDvVM.LoaiDvDTO.MaLoai = LoaiDvVM.LoaiDvDTO.MaLoai.ToUpper();

            try
            {
                await _tapDoanService.CreateAsync(LoaiDvVM.LoaiDvDTO); // save

                SetAlert("Thêm mới thành công.", "success");

                return Redirect(strUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.InnerException.Message, "error");
                return View(LoaiDvVM);
            }
        }

        public async Task<IActionResult> Edit(int id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            LoaiDvVM.StrUrl = strUrl;
            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            LoaiDvVM.LoaiDvDTO = await _tapDoanService.GetByIdAsync(id);

            if (LoaiDvVM.LoaiDvDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return View(LoaiDvVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id != LoaiDvVM.LoaiDvDTO.Id)
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

                if (t.TenLoai != LoaiDvVM.LoaiDvDTO.TenLoai)
                {
                    temp += String.Format("- TenLoai thay đổi: {0}->{1}", t.TenLoai, LoaiDvVM.LoaiDvDTO.TenLoai);
                }

                if (t.GhiChu != LoaiDvVM.LoaiDvDTO.GhiChu)
                {
                    temp += String.Format("- GhiChu thay đổi: {0}->{1}", t.GhiChu, LoaiDvVM.LoaiDvDTO.GhiChu);
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
                    LoaiDvVM.LoaiDvDTO.LogFile = t.LogFile;
                }

                try
                {
                    await _tapDoanService.UpdateAsync(LoaiDvVM.LoaiDvDTO);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(LoaiDvVM);
                }
            }
            // not valid

            return View(LoaiDvVM);
        }

        public JsonResult IsStringNameAvailable(string TenCreate)
        {
            var boolName = _tapDoanService.GetAll()
                .Where(x => x.TenLoai.Trim().ToLower() == TenCreate.Trim().ToLower())
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
        //    LoaiDvVM.StrUrl = strUrl;

        //    var TinhDTO = await _tinhTPService.GetByIdAsync(id);
        //    if (TinhDTO == null)
        //        return NotFound();
        //    try
        //    {
        //        await _tinhTPService.Delete(TinhDTO);

        //        SetAlert("Xóa thành công.", "success");
        //        return Redirect(LoaiDvVM.StrUrl);
        //    }
        //    catch (Exception ex)
        //    {
        //        SetAlert(ex.Message, "error");
        //        ModelState.AddModelError("", ex.Message);
        //        return Redirect(LoaiDvVM.StrUrl);
        //    }
        //}
    }
}