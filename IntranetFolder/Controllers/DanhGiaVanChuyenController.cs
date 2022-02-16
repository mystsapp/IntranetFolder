using Data.Models;
using Data.Utilities;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetFolder.Controllers
{
    public class DanhGiaVanChuyenController : BaseController
    {
        private readonly IDanhGiaVanChuyenService _danhGiaVanChuyenService;

        [BindProperty]
        public DanhGiaVanChuyenViewModel DanhGiaVanChuyenVM { get; set; }

        public DanhGiaVanChuyenController(IDanhGiaVanChuyenService danhGiaVanChuyenService)
        {
            DanhGiaVanChuyenVM = new DanhGiaVanChuyenViewModel()
            {
                DanhGiaVanChuyenDTO = new DanhGiaVanChuyenDTO(),
                StrUrl = ""
            };
            _danhGiaVanChuyenService = danhGiaVanChuyenService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> VanChuyen_Partial(string supplierId)
        {
            SupplierDTO supplierDTO = await _danhGiaVanChuyenService.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaVanChuyenVM.SupplierDTO = supplierDTO;
            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTOs = await _danhGiaVanChuyenService.GetDanhGiaVanChuyenBy_SupplierId(supplierId);

            return PartialView(DanhGiaVanChuyenVM);
        }

        public async Task<IActionResult> ThemMoiVanChuyen_Partial(string supplierId) // code
        {
            DanhGiaVanChuyenVM.SupplierDTO = await _danhGiaVanChuyenService.GetSupplierByIdAsync(supplierId);
            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.SupplierId = supplierId;
            return PartialView(DanhGiaVanChuyenVM);
        }

        [HttpPost, ActionName("ThemMoiVanChuyen_Partial")]
        public async Task<IActionResult> ThemMoiVanChuyen_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaVanChuyenVM = new DanhGiaVanChuyenViewModel()
                {
                    DanhGiaVanChuyenDTO = new DanhGiaVanChuyenDTO(),
                    StrUrl = strUrl
                };

                return Json(new
                {
                    status = false,
                    message = "Model state is not valid"
                });
            }

            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.NguoiTao = user.Username;
            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.NgayTao = DateTime.Now;
            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.LoaiDvid = 7; // MaLoai = CAR

            try
            {
                await _danhGiaVanChuyenService.CreateAsync(DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO); // save

                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                ErrorLog errorLog = new ErrorLog()
                {
                    InnerMessage = ex.InnerException.Message,
                    Message = ex.Message,
                    NgayTao = DateTime.Now,
                    NguoiTao = user.Nguoitao
                };
                await _danhGiaVanChuyenService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatVanChuyen_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO = await _danhGiaVanChuyenService.GetByIdAsync(id);
            DanhGiaVanChuyenVM.SupplierDTO = await _danhGiaVanChuyenService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return PartialView(DanhGiaVanChuyenVM);
        }

        [HttpPost, ActionName("CapNhatVanChuyen_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatVanChuyen_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.NgaySua = DateTime.Now;
                DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.NguoiSua = user.Username;

                try
                {
                    await _danhGiaVanChuyenService.UpdateAsync(DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO);

                    return Json(new { status = true });
                }
                catch (Exception ex)
                {
                    ErrorLog errorLog = new ErrorLog()
                    {
                        //InnerMessage = ex.InnerException.Message,
                        Message = ex.Message,
                        NgayTao = DateTime.Now,
                        NguoiTao = user.Nguoitao
                    };
                    await _danhGiaVanChuyenService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Thêm mới không thành công!"
                    });
                }
            }
            // not valid

            return View(DanhGiaVanChuyenVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaVanChuyenVM.StrUrl = strUrl;

            var DanhGiaVanChuyenDTO = _danhGiaVanChuyenService.GetByIdAsNoTracking(id);
            if (DanhGiaVanChuyenDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaVanChuyenService.Delete(DanhGiaVanChuyenDTO);

                return Json(true);
            }
            catch (Exception ex)
            {
                ErrorLog errorLog = new ErrorLog()
                {
                    InnerMessage = ex.InnerException.Message,
                    Message = ex.Message,
                    NgayTao = DateTime.Now,
                    NguoiTao = user.Nguoitao
                };
                await _danhGiaVanChuyenService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }
    }
}