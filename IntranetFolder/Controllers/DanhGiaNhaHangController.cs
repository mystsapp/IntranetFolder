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
    public class DanhGiaNhaHangController : BaseController
    {
        [BindProperty]
        public DanhGiaNhaHangViewModel DanhGiaNhaHangVM { get; set; }

        public DanhGiaNhaHangController()
        {
            DanhGiaNhaHangVM = new DanhGiaNhaHangViewModel()
            {
                DanhGiaNhaHangDTO = new DanhGiaNhaHangDTO()
            };
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, long id, int page = 1)
        {
            if (id == 0)
            {
                ViewBag.id = "";
            }

            DanhGiaNhaHangVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            DanhGiaNhaHangVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (id != 0) // for redirect with id
            {
                DanhGiaNhaHangVM.DanhGiaNcuDTO = await _danhGiaNhaCungUngService.GetByIdAsync(id);
                ViewBag.id = DanhGiaNhaHangVM.DanhGiaNcuDTO.Id;
            }
            else
            {
                DanhGiaNhaHangVM.DanhGiaNcuDTO = new DanhGiaNcuDTO();
            }
            DanhGiaNhaHangVM.danhGiaNcuDTOs = await _danhGiaNhaCungUngService.ListDanhGiaNCU(searchString, searchFromDate, searchToDate, page);
            return View(DanhGiaNhaHangVM);
        }

        public IActionResult Create(string strUrl)
        {
            DanhGiaNhaHangVM.StrUrl = strUrl;

            DanhGiaNhaHangVM.LoaiDvDTOs = _danhGiaNhaCungUngService.GetAllLoaiDv();

            return View(DanhGiaNhaHangVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaNhaHangVM = new DanhGiaNhaCungUngViewModel()
                {
                    DanhGiaNcuDTO = new DanhGiaNcuDTO(),
                    StrUrl = strUrl
                };

                return View(DanhGiaNhaHangVM);
            }

            DanhGiaNhaHangVM.DanhGiaNcuDTO.TenNcu = DanhGiaNhaHangVM.TenCreate;

            try
            {
                await _danhGiaNhaCungUngService.CreateAsync(DanhGiaNhaHangVM.DanhGiaNcuDTO); // save

                SetAlert("Thêm mới thành công.", "success");

                return Redirect(strUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.InnerException.Message, "error");
                return View(DanhGiaNhaHangVM);
            }
        }

        public async Task<IActionResult> Edit(long id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaNhaHangVM.StrUrl = strUrl;
            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaNhaHangVM.DanhGiaNcuDTO = await _danhGiaNhaCungUngService.GetByIdAsync(id);

            if (DanhGiaNhaHangVM.DanhGiaNcuDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaNhaHangVM.LoaiDvDTOs = _danhGiaNhaCungUngService.GetAllLoaiDv();

            return View(DanhGiaNhaHangVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id != DanhGiaNhaHangVM.DanhGiaNcuDTO.Id)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _danhGiaNhaCungUngService.UpdateAsync(DanhGiaNhaHangVM.DanhGiaNcuDTO);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(DanhGiaNhaHangVM);
                }
            }
            // not valid
            DanhGiaNhaHangVM.LoaiDvDTOs = _danhGiaNhaCungUngService.GetAllLoaiDv();

            return View(DanhGiaNhaHangVM);
        }

        public JsonResult IsStringNameAvailable(string TenCreate)
        {
            var boolName = _danhGiaNhaCungUngService.GetAll()
                .Where(x => x.TenNcu.Trim().ToLower() == TenCreate.Trim().ToLower())
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
        //    DanhGiaNhaHangVM.StrUrl = strUrl;

        //    var TinhDTO = await _tinhTPService.GetByIdAsync(id);
        //    if (TinhDTO == null)
        //        return NotFound();
        //    try
        //    {
        //        await _tinhTPService.Delete(TinhDTO);

        //        SetAlert("Xóa thành công.", "success");
        //        return Redirect(DanhGiaNhaHangVM.StrUrl);
        //    }
        //    catch (Exception ex)
        //    {
        //        SetAlert(ex.Message, "error");
        //        ModelState.AddModelError("", ex.Message);
        //        return Redirect(DanhGiaNhaHangVM.StrUrl);
        //    }
        //}
    }
}