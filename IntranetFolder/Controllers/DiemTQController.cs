using Data.Models;
using Data.Repository;
using Data.Utilities;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetFolder.Controllers
{
    public class DiemTQController : BaseController
    {
        private readonly IDiemTQService _diemTQService;

        [BindProperty]
        public DiemTQViewModel DiemTQVM { get; set; }

        public DiemTQController(IDiemTQService diemTQService)
        {
            DiemTQVM = new DiemTQViewModel()
            {
                DiemTQDTO = new DiemTQDTO()
            };
            _diemTQService = diemTQService;
        }

        //public async Task<IActionResult> Index(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        ViewBag.id = "";
        //    }

        //    DiemTQVM.StrUrl = UriHelper.GetDisplayUrl(Request);

        //    if (!string.IsNullOrEmpty(id)) // for redirect with id
        //    {
        //        DiemTQVM.DiemTQDTO = await _diemTQService.GetByIdAsync(id);
        //        ViewBag.id = DiemTQVM.DiemTQDTO.Matp;
        //    }
        //    else
        //    {
        //        DiemTQVM.DiemTQDTO = new DiemTQDTO();
        //    }
        //    DiemTQVM.DiemTQDTOs = await _diemTQService.get();
        //    return View(DiemTQVM);
        //}

        public async Task<IActionResult> DiemTQPartial(string maTinh, string strUrl)
        {
            DiemTQVM.StrUrl = strUrl;
            var DiemTQDTOs = await _diemTQService.GetDiemTQs_By_Tinh(maTinh);
            DiemTQVM.DiemTQDTOs = DiemTQDTOs;
            DiemTQVM.TinhDTO = await _diemTQService.GetTinhByIdAsync(maTinh);

            return PartialView(DiemTQVM);
        }

        public async Task<IActionResult> Create_Partial(string tinhid/*, string strUrl*/)
        {
            //DiemTQVM.StrUrl = strUrl;
            DiemTQVM.TinhDTO = await _diemTQService.GetTinhByIdAsync(tinhid);
            DiemTQVM.ThanhPho1DTOs = await _diemTQService.GetThanhPho1DTOs_By_Tinh(tinhid);
            DiemTQVM.SupplierDTOs = await _diemTQService.GetSuppliers(); // get trangthai == true
            DiemTQVM.DiemTQDTO.Code = await _diemTQService.GetNextId(tinhid);
            return PartialView(DiemTQVM);
        }

        [HttpPost, ActionName("Create_Partial")]
        public async Task<IActionResult> Create_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DiemTQVM = new DiemTQViewModel()
                {
                    DiemTQDTO = new DiemTQDTO(),
                    StrUrl = strUrl
                };

                return Json(new
                {
                    status = false,
                    message = "Not valid"
                });
            }

            DiemTQVM.DiemTQDTO.Tinhtp = DiemTQVM.TinhDTO.Matinh;
            // get nextId
            DiemTQVM.DiemTQDTO.Code = await _diemTQService.GetNextId(DiemTQVM.TinhDTO.Matinh);
            DiemTQVM.DiemTQDTO.Congno ??= "";

            try
            {
                await _diemTQService.CreateAsync(DiemTQVM.DiemTQDTO); // save

                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }

        public async Task<IActionResult> Edit_Partial(string id, string strUrl, string tinhid)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DiemTQVM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Điểm TQ này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DiemTQVM.DiemTQDTO = await _diemTQService.GetByIdAsync(id);

            if (DiemTQVM.DiemTQDTO == null)
            {
                ViewBag.ErrorMessage = "Điểm TQ này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DiemTQVM.TinhDTO = await _diemTQService.GetTinhByIdAsync(tinhid);
            DiemTQVM.ThanhPho1DTOs = await _diemTQService.GetThanhPho1DTOs_By_Tinh(tinhid);
            DiemTQVM.SupplierDTOs = await _diemTQService.GetSuppliers(); // get trangthai == true

            return PartialView(DiemTQVM);
        }

        [HttpPost, ActionName("Edit_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                try
                {
                    await _diemTQService.UpdateAsync(DiemTQVM.DiemTQDTO);
                    //SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return Json(new
                    {
                        status = true
                    });
                }
                catch (Exception ex)
                {
                    //SetAlert(ex.Message, "error");

                    return Json(new
                    {
                        status = false,
                        message = ex.Message
                    });
                }
            }
            // not valid

            return PartialView(DiemTQVM);
        }

        public JsonResult IsStringNameAvailable(string TenCreate)
        {
            var boolName = _diemTQService.GetDiemTQs()
                .Where(x => x.Diemtq.Trim().ToLower() == TenCreate.Trim().ToLower())
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

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id, string strUrl)
        {
            DiemTQVM.StrUrl = strUrl;

            var DiemTQDTO = _diemTQService.GetByIdAsNoTracking(id);
            if (DiemTQDTO == null)
                return NotFound();
            try
            {
                _diemTQService.Delete(DiemTQDTO);

                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }

        public IActionResult DetailsRedirect(string tinhId)
        {
            return RedirectToAction("Index", "TinhDiemTQ", new { id = tinhId });
        }
    }
}