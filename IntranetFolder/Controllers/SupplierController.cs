using Data.Models;
using Data.Repository;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IntranetFolder.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        [BindProperty]
        public SupplierViewModel SupplierVM { get; set; }

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
            SupplierVM = new SupplierViewModel()
            {
                Supplier = new Supplier()
            };
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, string boolSgtcode, string id, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.id = "";
            }

            SupplierVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            SupplierVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;
            ViewBag.boolSgtcode = boolSgtcode;

            if (!string.IsNullOrEmpty(id)) // for redirect with id
            {
                SupplierVM.Supplier = await _supplierService.GetByIdAsync(id);
                ViewBag.id = SupplierVM.Supplier.Code;
            }
            else
            {
                SupplierVM.Supplier = new Supplier();
            }
            SupplierVM.Suppliers = await _supplierService.ListSupplier(searchString, searchFromDate, searchToDate, page);
            return View(SupplierVM);
        }
    }
}