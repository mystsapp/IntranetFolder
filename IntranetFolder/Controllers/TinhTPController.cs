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
    public class TinhTPController : BaseController
    {
        private readonly ITinhTPService _tinhTPService;

        [BindProperty]
        public TinhTPViewModel TinhTPVM { get; set; }

        public TinhTPController(ITinhTPService tinhTPService)
        {
            TinhTPVM = new TinhTPViewModel()
            {
                TinhDTO = new TinhDTO(),
                VTinhDTO = new VTinhDTO()
            };
            _tinhTPService = tinhTPService;
        }

        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.id = "";
            }

            TinhTPVM.StrUrl = UriHelper.GetDisplayUrl(Request);

            if (!string.IsNullOrEmpty(id)) // for redirect with id
            {
                TinhTPVM.TinhDTO = await _tinhTPService.GetByIdAsync(id);
                ViewBag.id = TinhTPVM.TinhDTO.Matinh;
            }
            else
            {
                TinhTPVM.TinhDTO = new TinhDTO();
            }
            TinhTPVM.VTinhDTOs = await _tinhTPService.GetVTinhDTOs();
            return View(TinhTPVM);
        }

        public async Task<IActionResult> Create(string strUrl)
        {
            TinhTPVM.StrUrl = strUrl;

            TinhTPVM.Vungmiens = await _tinhTPService.GetVungmiens();
            //TinhTPVM.Thanhpho1s = await _tinhTPService.GetThanhpho1s();

            return View(TinhTPVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                TinhTPVM = new TinhTPViewModel()
                {
                    TinhDTO = new TinhDTO(),
                    StrUrl = strUrl
                };

                return View(TinhTPVM);
            }

            var vungmiens = await _tinhTPService.GetVungmiens();
            TinhTPVM.TinhDTO.MienId = vungmiens.Where(x => x.VungId == TinhTPVM.TinhDTO.VungId)
                .FirstOrDefault().Mien;
            TinhTPVM.TinhDTO.Matinh = TinhTPVM.TenCreate;

            try
            {
                await _tinhTPService.CreateAsync(TinhTPVM.TinhDTO); // save

                SetAlert("Thêm mới thành công.", "success");

                return Redirect(strUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.InnerException.Message, "error");
                return View(TinhTPVM);
            }
        }

        public async Task<IActionResult> Edit(string id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            TinhTPVM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Tỉnh này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            TinhTPVM.TinhDTO = await _tinhTPService.GetByIdAsync(id);

            if (TinhTPVM.TinhDTO == null)
            {
                ViewBag.ErrorMessage = "Tỉnh này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            TinhTPVM.Vungmiens = await _tinhTPService.GetVungmiens();

            return View(TinhTPVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id != TinhTPVM.TinhDTO.Matinh)
            {
                ViewBag.ErrorMessage = "Tỉnh này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _tinhTPService.UpdateAsync(TinhTPVM.TinhDTO);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(TinhTPVM);
                }
            }
            // not valid
            TinhTPVM.Vungmiens = await _tinhTPService.GetVungmiens();

            return View(TinhTPVM);
        }

        public JsonResult IsStringNameAvailable(string TenCreate)
        {
            var boolName = _tinhTPService.GetAllTinhs()
                .Where(x => x.Matinh.Trim().ToLower() == TenCreate.Trim().ToLower())
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
        //    TinhTPVM.StrUrl = strUrl;

        //    var TinhDTO = await _tinhTPService.GetByIdAsync(id);
        //    if (TinhDTO == null)
        //        return NotFound();
        //    try
        //    {
        //        await _tinhTPService.Delete(TinhDTO);

        //        SetAlert("Xóa thành công.", "success");
        //        return Redirect(TinhTPVM.StrUrl);
        //    }
        //    catch (Exception ex)
        //    {
        //        SetAlert(ex.Message, "error");
        //        ModelState.AddModelError("", ex.Message);
        //        return Redirect(TinhTPVM.StrUrl);
        //    }
        //}
    }
}