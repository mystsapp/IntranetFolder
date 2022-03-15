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
    public class DanhGiaDTQController : BaseController
    {
        private readonly IDanhGiaDTQService _danhGiaDTQService;

        [BindProperty]
        public DanhGiaDTQViewModel DanhGiaDTQVM { get; set; }

        public DanhGiaDTQController(IDanhGiaDTQService danhGiaDTQService)
        {
            DanhGiaDTQVM = new DanhGiaDTQViewModel()
            {
                DanhGiaDTQDTO = new DanhGiaDTQDTO(),
                StrUrl = ""
            };
            _danhGiaDTQService = danhGiaDTQService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DiemThamQuan_Partial(string supplierId)
        {
            SupplierDTO supplierDTO = await _danhGiaDTQService.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaDTQVM.SupplierDTO = supplierDTO;
            DanhGiaDTQVM.DanhGiaDTQDTOs = await _danhGiaDTQService.GetDanhGiaDTQBy_SupplierId(supplierId);

            return PartialView(DanhGiaDTQVM);
        }

        public async Task<IActionResult> ThemMoiDTQ_Partial(string supplierId) // code
        {
            DanhGiaDTQVM.SupplierDTO = await _danhGiaDTQService.GetSupplierByIdAsync(supplierId);
            DanhGiaDTQVM.DanhGiaDTQDTO.SupplierId = supplierId;
            DanhGiaDTQVM.DanhGiaDTQDTO.TenNcu = DanhGiaDTQVM.SupplierDTO.Tengiaodich;
            return PartialView(DanhGiaDTQVM);
        }

        [HttpPost, ActionName("ThemMoiDTQ_Partial")]
        public async Task<IActionResult> ThemMoiDTQ_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaDTQVM = new DanhGiaDTQViewModel()
                {
                    DanhGiaDTQDTO = new DanhGiaDTQDTO(),
                    StrUrl = strUrl
                };

                return Json(new
                {
                    status = false,
                    message = "Model state is not valid"
                });
            }

            DanhGiaDTQVM.DanhGiaDTQDTO.NguoiTao = user.Username;
            DanhGiaDTQVM.DanhGiaDTQDTO.NgayTao = DateTime.Now;
            DanhGiaDTQVM.DanhGiaDTQDTO.LoaiDvid = 5; // MaLoai = SSE

            try
            {
                await _danhGiaDTQService.CreateAsync(DanhGiaDTQVM.DanhGiaDTQDTO); // save

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
                await _danhGiaDTQService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatDTQ_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaDTQVM.DanhGiaDTQDTO = await _danhGiaDTQService.GetByIdAsync(id);
            DanhGiaDTQVM.SupplierDTO = await _danhGiaDTQService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaDTQVM.DanhGiaDTQDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return PartialView(DanhGiaDTQVM);
        }

        [HttpPost, ActionName("CapNhatDTQ_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatDTQ_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaDTQVM.DanhGiaDTQDTO.NgaySua = DateTime.Now;
                DanhGiaDTQVM.DanhGiaDTQDTO.NguoiSua = user.Username;

                try
                {
                    await _danhGiaDTQService.UpdateAsync(DanhGiaDTQVM.DanhGiaDTQDTO);

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
                    await _danhGiaDTQService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Thêm mới không thành công!"
                    });
                }
            }
            // not valid

            return View(DanhGiaDTQVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaDTQVM.StrUrl = strUrl;

            var DanhGiaDTQDTO = _danhGiaDTQService.GetByIdAsNoTracking(id);
            if (DanhGiaDTQDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaDTQService.Delete(DanhGiaDTQDTO);

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
                await _danhGiaDTQService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }
    }
}