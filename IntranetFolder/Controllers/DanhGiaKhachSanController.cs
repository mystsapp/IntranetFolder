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
    public class DanhGiaKhachSanController : BaseController
    {
        private readonly IDanhGiaKhachSanService _danhGiaKhachSanService;

        [BindProperty]
        public DanhGiaKhachSanViewModel DanhGiaKhachSanVM { get; set; }

        public DanhGiaKhachSanController(IDanhGiaKhachSanService danhGiaKhachSanService)
        {
            _danhGiaKhachSanService = danhGiaKhachSanService;
            DanhGiaKhachSanVM = new DanhGiaKhachSanViewModel()
            {
                DanhGiaKhachSanDTO = new DanhGiaKhachSanDTO(),
                StrUrl = ""
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> KhachSan_Partial(string supplierId)
        {
            SupplierDTO supplierDTO = await _danhGiaKhachSanService.GetBySupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaKhachSanVM.SupplierDTO = supplierDTO;
            DanhGiaKhachSanVM.DanhGiaKhachSanDTOs = await _danhGiaKhachSanService.GetDanhGiaKhachSanBy_SupplierId(supplierId);

            return PartialView(DanhGiaKhachSanVM);
        }

        public async Task<IActionResult> ThemMoiKhachSan_Partial(string supplierId) // code
        {
            DanhGiaKhachSanVM.SupplierDTO = await _danhGiaKhachSanService.GetSupplierByIdAsync(supplierId);
            DanhGiaKhachSanVM.DanhGiaKhachSanDTO.TenNcu = DanhGiaKhachSanVM.SupplierDTO.Tengiaodich;
            return PartialView(DanhGiaKhachSanVM);
        }

        [HttpPost, ActionName("ThemMoiKhachSan_Partial")]
        public async Task<IActionResult> ThemMoiKhachSan_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaKhachSanVM = new DanhGiaKhachSanViewModel()
                {
                    DanhGiaKhachSanDTO = new DanhGiaKhachSanDTO(),
                    StrUrl = strUrl
                };

                return View(DanhGiaKhachSanVM);
            }

            //DanhGiaKhachSanVM.DanhGiaKhachSanDTO.TenNcu = DanhGiaKhachSanVM.TenCreate;
            DanhGiaKhachSanVM.DanhGiaKhachSanDTO.NguoiTao = user.Username;
            DanhGiaKhachSanVM.DanhGiaKhachSanDTO.NgayTao = DateTime.Now;
            DanhGiaKhachSanVM.DanhGiaKhachSanDTO.LoaiDvid = 1; // MaLoai = HTL

            try
            {
                await _danhGiaKhachSanService.CreateAsync(DanhGiaKhachSanVM.DanhGiaKhachSanDTO); // save

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
                await _danhGiaKhachSanService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatKhachSan_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaKhachSanVM.DanhGiaKhachSanDTO = await _danhGiaKhachSanService.GetByIdAsync(id);
            DanhGiaKhachSanVM.SupplierDTO = await _danhGiaKhachSanService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaKhachSanVM.DanhGiaKhachSanDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return PartialView(DanhGiaKhachSanVM);
        }

        [HttpPost, ActionName("CapNhatKhachSan_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatNhaHang_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaKhachSanVM.DanhGiaKhachSanDTO.NgaySua = DateTime.Now;
                DanhGiaKhachSanVM.DanhGiaKhachSanDTO.NguoiSua = user.Username;

                try
                {
                    await _danhGiaKhachSanService.UpdateAsync(DanhGiaKhachSanVM.DanhGiaKhachSanDTO);

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
                    await _danhGiaKhachSanService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Thêm mới không thành công!"
                    });
                }
            }
            // not valid
            //DanhGiaKhachSanVM.LoaiDvDTOs = _danhGiaKhachSanService.GetAllLoaiDv();

            return View(DanhGiaKhachSanVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaKhachSanVM.StrUrl = strUrl;

            var DanhGiaKhachSanDTO = _danhGiaKhachSanService.GetByIdAsNoTracking(id);
            if (DanhGiaKhachSanDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaKhachSanService.Delete(DanhGiaKhachSanDTO);

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
                await _danhGiaKhachSanService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }
    }
}