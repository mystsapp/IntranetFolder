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
            DanhGiaKhachSanVM.DanhGiaKhachSanDTO.SupplierId = supplierId;
            return PartialView(DanhGiaKhachSanVM);
        }
    }
}