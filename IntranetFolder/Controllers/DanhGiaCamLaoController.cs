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
    public class DanhGiaCamLaoController : BaseController
    {
        private readonly IDanhGiaCamLaoService _danhGiaCamLaoService;

        [BindProperty]
        public DanhGiaCamLaoViewModel DanhGiaCamLaoVM { get; set; }

        public DanhGiaCamLaoController(IDanhGiaCamLaoService danhGiaCamLaoService)
        {
            DanhGiaCamLaoVM = new DanhGiaCamLaoViewModel()
            {
                DanhGiaCamLaoDTO = new DanhGiaCamLaoDTO(),
                StrUrl = ""
            };
            _danhGiaCamLaoService = danhGiaCamLaoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CamLao_Partial(string supplierId)
        {
            SupplierDTO supplierDTO = await _danhGiaCamLaoService.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaCamLaoVM.SupplierDTO = supplierDTO;
            DanhGiaCamLaoVM.DanhGiaCamLaoDTOs = await _danhGiaCamLaoService.GetDanhGiaCamLaoBy_SupplierId(supplierId);

            return PartialView(DanhGiaCamLaoVM);
        }

        public async Task<IActionResult> ThemMoiCamLao_Partial(string supplierId) // code
        {
            DanhGiaCamLaoVM.SupplierDTO = await _danhGiaCamLaoService.GetSupplierByIdAsync(supplierId);
            DanhGiaCamLaoVM.DanhGiaCamLaoDTO.SupplierId = supplierId;
            DanhGiaCamLaoVM.DanhGiaCamLaoDTO.TenNcu = DanhGiaCamLaoVM.SupplierDTO.Tengiaodich;
            return PartialView(DanhGiaCamLaoVM);
        }

        [HttpPost, ActionName("ThemMoiCamLao_Partial")]
        public async Task<IActionResult> ThemMoiCamLao_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaCamLaoVM = new DanhGiaCamLaoViewModel()
                {
                    DanhGiaCamLaoDTO = new DanhGiaCamLaoDTO(),
                    StrUrl = strUrl
                };

                return Json(new
                {
                    status = false,
                    message = "Model state is not valid"
                });
            }

            DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NguoiTao = user.Username;
            DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NgayTao = DateTime.Now;
            DanhGiaCamLaoVM.DanhGiaCamLaoDTO.LoaiDvid = 5; // MaLoai = SSE

            try
            {
                await _danhGiaCamLaoService.CreateAsync(DanhGiaCamLaoVM.DanhGiaCamLaoDTO); // save

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
                await _danhGiaCamLaoService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatCamLao_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaCamLaoVM.DanhGiaCamLaoDTO = await _danhGiaCamLaoService.GetByIdAsync(id);
            DanhGiaCamLaoVM.SupplierDTO = await _danhGiaCamLaoService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaCamLaoVM.DanhGiaCamLaoDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return PartialView(DanhGiaCamLaoVM);
        }

        [HttpPost, ActionName("CapNhatCamLao_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatCamLao_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NgaySua = DateTime.Now;
                DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NguoiSua = user.Username;

                try
                {
                    await _danhGiaCamLaoService.UpdateAsync(DanhGiaCamLaoVM.DanhGiaCamLaoDTO);

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
                    await _danhGiaCamLaoService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Thêm mới không thành công!"
                    });
                }
            }
            // not valid

            return View(DanhGiaCamLaoVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaCamLaoVM.StrUrl = strUrl;

            var DanhGiaCamLaoDTO = _danhGiaCamLaoService.GetByIdAsNoTracking(id);
            if (DanhGiaCamLaoDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaCamLaoService.Delete(DanhGiaCamLaoDTO);

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
                await _danhGiaCamLaoService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }
    }
}