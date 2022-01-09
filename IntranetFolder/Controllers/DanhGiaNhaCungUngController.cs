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
    public class DanhGiaNhaCungUngController : BaseController
    {
        private readonly IDanhGiaNhaCungUngService _danhGiaNhaCungUngService;

        [BindProperty]
        public DanhGiaNhaCungUngViewModel DanhGiaNhaCungUngVM { get; set; }

        public DanhGiaNhaCungUngController(IDanhGiaNhaCungUngService danhGiaNhaCungUngService)
        {
            DanhGiaNhaCungUngVM = new DanhGiaNhaCungUngViewModel()
            {
                DanhGiaNcuDTO = new DanhGiaNcuDTO()
            };
            _danhGiaNhaCungUngService = danhGiaNhaCungUngService;
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, long id, int page = 1)
        {
            if (id == 0)
            {
                ViewBag.id = "";
            }

            DanhGiaNhaCungUngVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            DanhGiaNhaCungUngVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (id != 0) // for redirect with id
            {
                DanhGiaNhaCungUngVM.DanhGiaNcuDTO = await _danhGiaNhaCungUngService.GetByIdAsync(id);
                ViewBag.id = DanhGiaNhaCungUngVM.DanhGiaNcuDTO.Id;
            }
            else
            {
                DanhGiaNhaCungUngVM.DanhGiaNcuDTO = new DanhGiaNcuDTO();
            }
            DanhGiaNhaCungUngVM.danhGiaNcuDTOs = await _danhGiaNhaCungUngService.ListDanhGiaNCU(searchString, searchFromDate, searchToDate, page);
            return View(DanhGiaNhaCungUngVM);
        }

        public IActionResult Create(string strUrl)
        {
            DanhGiaNhaCungUngVM.StrUrl = strUrl;

            DanhGiaNhaCungUngVM.LoaiDvDTOs = _danhGiaNhaCungUngService.GetAllLoaiDv();

            return View(DanhGiaNhaCungUngVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaNhaCungUngVM = new DanhGiaNhaCungUngViewModel()
                {
                    DanhGiaNcuDTO = new DanhGiaNcuDTO(),
                    StrUrl = strUrl
                };

                return View(DanhGiaNhaCungUngVM);
            }

            DanhGiaNhaCungUngVM.DanhGiaNcuDTO.TenNcu = DanhGiaNhaCungUngVM.TenCreate;

            try
            {
                await _danhGiaNhaCungUngService.CreateAsync(DanhGiaNhaCungUngVM.DanhGiaNcuDTO); // save

                SetAlert("Thêm mới thành công.", "success");

                return Redirect(strUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.InnerException.Message, "error");
                return View(DanhGiaNhaCungUngVM);
            }
        }

        public async Task<IActionResult> Edit(long id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaNhaCungUngVM.StrUrl = strUrl;
            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaNhaCungUngVM.DanhGiaNcuDTO = await _danhGiaNhaCungUngService.GetByIdAsync(id);

            if (DanhGiaNhaCungUngVM.DanhGiaNcuDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaNhaCungUngVM.LoaiDvDTOs = _danhGiaNhaCungUngService.GetAllLoaiDv();

            return View(DanhGiaNhaCungUngVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id != DanhGiaNhaCungUngVM.DanhGiaNcuDTO.Id)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _danhGiaNhaCungUngService.UpdateAsync(DanhGiaNhaCungUngVM.DanhGiaNcuDTO);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(DanhGiaNhaCungUngVM);
                }
            }
            // not valid
            DanhGiaNhaCungUngVM.LoaiDvDTOs = _danhGiaNhaCungUngService.GetAllLoaiDv();

            return View(DanhGiaNhaCungUngVM);
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
        //    DanhGiaNhaCungUngVM.StrUrl = strUrl;

        //    var TinhDTO = await _tinhTPService.GetByIdAsync(id);
        //    if (TinhDTO == null)
        //        return NotFound();
        //    try
        //    {
        //        await _tinhTPService.Delete(TinhDTO);

        //        SetAlert("Xóa thành công.", "success");
        //        return Redirect(DanhGiaNhaCungUngVM.StrUrl);
        //    }
        //    catch (Exception ex)
        //    {
        //        SetAlert(ex.Message, "error");
        //        ModelState.AddModelError("", ex.Message);
        //        return Redirect(DanhGiaNhaCungUngVM.StrUrl);
        //    }
        //}
    }
}