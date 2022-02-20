using Common;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetFolder.Controllers
{
    public class DichVu1Controller : Controller
    {
        private readonly IDichVu1Service _dichVu1Service;

        [BindProperty]
        public DichVu1ViewModel DichVu1VM { get; set; }

        public DichVu1Controller(IDichVu1Service dichVu1Service)
        {
            DichVu1VM = new DichVu1ViewModel()
            {
                DichVu1DTO = new Model.DichVu1DTO()
            };
            _dichVu1Service = dichVu1Service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DichVu1Partial(string supplierId, int page)
        {
            var supplierDTO = await _dichVu1Service.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                //ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return Content("Supplier này không tồn tại.");
            }
            DichVu1VM.Page = page;
            DichVu1VM.SupplierDTO = supplierDTO;
            DichVu1VM.DichVu1DTOs = await _dichVu1Service.GetDichVu1By_SupplierId(supplierId);

            return PartialView(DichVu1VM);
        }

        public async Task<IActionResult> ThemMoiDichVu1(string supplierId, int page)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var supplierDTO = await _dichVu1Service.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                //ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return Content("Supplier này không tồn tại.");
            }
            DichVu1VM.Page = page;
            DichVu1VM.SupplierDTO = supplierDTO;
            DichVu1VM.DichVu1DTO.SupplierId = supplierId;
            DichVu1VM.Vungmiens = await _dichVu1Service.Vungmiens();
            DichVu1VM.VTinhs = await _dichVu1Service.GetTinhs();
            DichVu1VM.Thanhpho1s = await _dichVu1Service.GetThanhpho1s();
            DichVu1VM.LoaiSaos = SD.LoaiSao();
            DichVu1VM.LoaiDvs = _dichVu1Service.GetAllLoaiDv();
            return View(DichVu1VM);
        }
    }
}