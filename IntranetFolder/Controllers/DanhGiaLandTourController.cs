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
    public class DanhGiaLandTourController : BaseController
    {
        private readonly IDanhGiaLandTourService _danhGiaLandTourService;

        [BindProperty]
        public DanhGiaLandTourViewModel DanhGiaLandTourVM { get; set; }

        public DanhGiaLandTourController(IDanhGiaLandTourService danhGiaLandTourService)
        {
            DanhGiaLandTourVM = new DanhGiaLandTourViewModel()
            {
                DanhGiaLandTourDTO = new DanhGiaLandTourDTO(),
                StrUrl = ""
            };
            _danhGiaLandTourService = danhGiaLandTourService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LandTour_Partial(string supplierId)
        {
            SupplierDTO supplierDTO = await _danhGiaLandTourService.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaLandTourVM.SupplierDTO = supplierDTO;
            DanhGiaLandTourVM.DanhGiaLandTourDTOs = await _danhGiaLandTourService.GetDanhGiaLandTourBy_SupplierId(supplierId);

            return PartialView(DanhGiaLandTourVM);
        }

        public async Task<IActionResult> ThemMoiLandTour_Partial(string supplierId) // code
        {
            DanhGiaLandTourVM.SupplierDTO = await _danhGiaLandTourService.GetSupplierByIdAsync(supplierId);
            DanhGiaLandTourVM.DanhGiaLandTourDTO.SupplierId = supplierId;
            DanhGiaLandTourVM.DanhGiaLandTourDTO.TenNcu = DanhGiaLandTourVM.SupplierDTO.Tengiaodich;
            return PartialView(DanhGiaLandTourVM);
        }

        [HttpPost, ActionName("ThemMoiLandTour_Partial")]
        public async Task<IActionResult> ThemMoiLandTour_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaLandTourVM = new DanhGiaLandTourViewModel()
                {
                    DanhGiaLandTourDTO = new DanhGiaLandTourDTO(),
                    StrUrl = strUrl
                };

                return Json(new
                {
                    status = false,
                    message = "Model state is not valid"
                });
            }

            DanhGiaLandTourVM.DanhGiaLandTourDTO.NguoiTao = user.Username;
            DanhGiaLandTourVM.DanhGiaLandTourDTO.NgayTao = DateTime.Now;
            DanhGiaLandTourVM.DanhGiaLandTourDTO.LoaiDvid = 6; // MaLoai = LCT

            try
            {
                await _danhGiaLandTourService.CreateAsync(DanhGiaLandTourVM.DanhGiaLandTourDTO); // save

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
                await _danhGiaLandTourService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatLandTour_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaLandTourVM.DanhGiaLandTourDTO = await _danhGiaLandTourService.GetByIdAsync(id);
            DanhGiaLandTourVM.SupplierDTO = await _danhGiaLandTourService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaLandTourVM.DanhGiaLandTourDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return PartialView(DanhGiaLandTourVM);
        }

        [HttpPost, ActionName("CapNhatLandTour_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatLandTour_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaLandTourVM.DanhGiaLandTourDTO.NgaySua = DateTime.Now;
                DanhGiaLandTourVM.DanhGiaLandTourDTO.NguoiSua = user.Username;

                try
                {
                    await _danhGiaLandTourService.UpdateAsync(DanhGiaLandTourVM.DanhGiaLandTourDTO);

                    return Json(new { status = true });
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
                    await _danhGiaLandTourService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Thêm mới không thành công!"
                    });
                }
            }
            // not valid

            return View(DanhGiaLandTourVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaLandTourVM.StrUrl = strUrl;

            var danhGiaLandTourDTO = _danhGiaLandTourService.GetByIdAsNoTracking(id);
            if (danhGiaLandTourDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaLandTourService.Delete(danhGiaLandTourDTO);

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
                await _danhGiaLandTourService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }
    }
}