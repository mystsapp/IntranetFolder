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

        public async Task<IActionResult> Index(long id)
        {
            if (id == 0)
            {
                ViewBag.id = "";
            }

            DanhGiaNhaCungUngVM.StrUrl = UriHelper.GetDisplayUrl(Request);

            if (id != 0) // for redirect with id
            {
                DanhGiaNhaCungUngVM.DanhGiaNcuDTO = await _danhGiaNhaCungUngService.GetByIdAsync(id);
                ViewBag.id = DanhGiaNhaCungUngVM.DanhGiaNcuDTO.Id;
            }
            else
            {
                DanhGiaNhaCungUngVM.DanhGiaNcuDTO = new DanhGiaNcuDTO();
            }
            DanhGiaNhaCungUngVM.danhGiaNcuDTOs = await _tinhTPService.GetVTinhDTOs();
            return View(DanhGiaNhaCungUngVM);
        }

        public async Task<IActionResult> Create(string strUrl)
        {
            DanhGiaNhaCungUngVM.StrUrl = strUrl;

            DanhGiaNhaCungUngVM.Vungmiens = await _tinhTPService.GetVungmiens();
            //DanhGiaNhaCungUngVM.Thanhpho1s = await _tinhTPService.GetThanhpho1s();

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
                    TinhDTO = new TinhDTO(),
                    StrUrl = strUrl
                };

                return View(DanhGiaNhaCungUngVM);
            }

            var vungmiens = await _tinhTPService.GetVungmiens();
            DanhGiaNhaCungUngVM.TinhDTO.MienId = vungmiens.Where(x => x.VungId == DanhGiaNhaCungUngVM.TinhDTO.VungId)
                .FirstOrDefault().Mien;
            DanhGiaNhaCungUngVM.TinhDTO.Matinh = DanhGiaNhaCungUngVM.TenCreate;

            try
            {
                await _tinhTPService.CreateAsync(DanhGiaNhaCungUngVM.TinhDTO); // save

                SetAlert("Thêm mới thành công.", "success");

                return Redirect(strUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.InnerException.Message, "error");
                return View(DanhGiaNhaCungUngVM);
            }
        }

        public async Task<IActionResult> Edit(string id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaNhaCungUngVM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Tỉnh này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaNhaCungUngVM.TinhDTO = await _tinhTPService.GetByIdAsync(id);

            if (DanhGiaNhaCungUngVM.TinhDTO == null)
            {
                ViewBag.ErrorMessage = "Tỉnh này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaNhaCungUngVM.Vungmiens = await _tinhTPService.GetVungmiens();

            return View(DanhGiaNhaCungUngVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id != DanhGiaNhaCungUngVM.TinhDTO.Matinh)
            {
                ViewBag.ErrorMessage = "Tỉnh này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _tinhTPService.UpdateAsync(DanhGiaNhaCungUngVM.TinhDTO);
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
            DanhGiaNhaCungUngVM.Vungmiens = await _tinhTPService.GetVungmiens();

            return View(DanhGiaNhaCungUngVM);
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