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

        public async Task<IActionResult> DichVu1Partial(string supplierId)
        {
            var supplierDTO = await _dichVu1Service.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DichVu1VM.SupplierDTO = supplierDTO;
            DichVu1VM.DichVu1DTOs = await _dichVu1Service.GetDichVu1By_SupplierId(supplierId);

            return PartialView(DichVu1VM);
        }
    }
}