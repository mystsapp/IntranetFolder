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
    public class ThanhPho1Controller : BaseController
    {
        private readonly IThanhPho1Service _thanhPho1Service;

        [BindProperty]
        public ThanhPho1ViewModel ThanhPho1VM { get; set; }

        public ThanhPho1Controller(IThanhPho1Service thanhPho1Service)
        {
            ThanhPho1VM = new ThanhPho1ViewModel()
            {
                ThanhPho1DTO = new ThanhPho1DTO()
            };
            _thanhPho1Service = thanhPho1Service;
        }

        //public async Task<IActionResult> Index(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        ViewBag.id = "";
        //    }

        //    ThanhPho1VM.StrUrl = UriHelper.GetDisplayUrl(Request);

        //    if (!string.IsNullOrEmpty(id)) // for redirect with id
        //    {
        //        ThanhPho1VM.ThanhPho1DTO = await _thanhPho1Service.GetByIdAsync(id);
        //        ViewBag.id = ThanhPho1VM.ThanhPho1DTO.Matp;
        //    }
        //    else
        //    {
        //        ThanhPho1VM.ThanhPho1DTO = new ThanhPho1DTO();
        //    }
        //    ThanhPho1VM.ThanhPho1DTOs = await _thanhPho1Service.get();
        //    return View(ThanhPho1VM);
        //}

        public async Task<IActionResult> ThanhPho1Partial(string maTinh, string strUrl)
        {
            ThanhPho1VM.StrUrl = strUrl;
            var thanhPho1DTOs = await _thanhPho1Service.GetThanhPho1s_By_Tinh(maTinh);
            ThanhPho1VM.ThanhPho1DTOs = thanhPho1DTOs;
            ThanhPho1VM.TinhDTO = await _thanhPho1Service.GetTinhByIdAsync(maTinh);

            return PartialView(ThanhPho1VM);
        }

        public async Task<IActionResult> Create_Partial(string tinhid, string strUrl)
        {
            ThanhPho1VM.StrUrl = strUrl;
            ThanhPho1VM.TinhDTO = await _thanhPho1Service.GetTinhByIdAsync(tinhid);
            ThanhPho1VM.ThanhPho1DTO.Matp = await _thanhPho1Service.GetNextId(tinhid);
            return PartialView(ThanhPho1VM);
        }

        [HttpPost, ActionName("Create_Partial")]
        public async Task<IActionResult> Create_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                ThanhPho1VM = new ThanhPho1ViewModel()
                {
                    ThanhPho1DTO = new ThanhPho1DTO(),
                    StrUrl = strUrl
                };

                return Json(new
                {
                    status = false,
                    message = "Not valid"
                });
            }

            ThanhPho1VM.ThanhPho1DTO.Matinh = ThanhPho1VM.TinhDTO.Matinh;
            // get nextId
            ThanhPho1VM.ThanhPho1DTO.Matp = await _thanhPho1Service.GetNextId(ThanhPho1VM.TinhDTO.Matinh);

            try
            {
                await _thanhPho1Service.CreateAsync(ThanhPho1VM.ThanhPho1DTO); // save

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

            ThanhPho1VM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Thành phố này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            ThanhPho1VM.ThanhPho1DTO = await _thanhPho1Service.GetByIdAsync(id);

            if (ThanhPho1VM.ThanhPho1DTO == null)
            {
                ViewBag.ErrorMessage = "Thành phố này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            ThanhPho1VM.TinhDTO = await _thanhPho1Service.GetTinhByIdAsync(tinhid);

            return PartialView(ThanhPho1VM);
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
                    await _thanhPho1Service.UpdateAsync(ThanhPho1VM.ThanhPho1DTO);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return Json(new
                    {
                        status = true
                    });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return Json(new
                    {
                        status = false,
                        message = ex.Message
                    });
                }
            }
            // not valid

            return PartialView(ThanhPho1VM);
        }

        public JsonResult IsStringNameAvailable(string TenCreate)
        {
            var boolName = _thanhPho1Service.GetThanhPho1s()
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

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id, string strUrl)
        {
            ThanhPho1VM.StrUrl = strUrl;

            var ThanhPho1DTO = _thanhPho1Service.GetByIdAsNoTracking(id);
            if (ThanhPho1DTO == null)
                return NotFound();
            try
            {
                await _thanhPho1Service.Delete(ThanhPho1DTO);

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
            return RedirectToAction("Index", "TinhTP", new { id = tinhId });
        }
    }
}