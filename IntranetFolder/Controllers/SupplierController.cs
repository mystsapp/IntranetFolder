using Data.Models;
using Data.Repository;
using Data.Utilities;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Threading.Tasks;

namespace IntranetFolder.Controllers
{
    public class SupplierController : BaseController
    {
        private readonly ISupplierService _supplierService;

        [BindProperty]
        public SupplierViewModel SupplierVM { get; set; }

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
            SupplierVM = new SupplierViewModel()
            {
                SupplierDTO = new SupplierDTO()
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
                SupplierVM.SupplierDTO = await _supplierService.GetByIdAsync(id);
                ViewBag.id = SupplierVM.SupplierDTO.Code;
            }
            else
            {
                SupplierVM.SupplierDTO = new SupplierDTO();
            }
            SupplierVM.SupplierDTOs = await _supplierService.ListSupplier(searchString, searchFromDate, searchToDate, page);
            return View(SupplierVM);
        }

        public IActionResult Create(string strUrl)
        {
            SupplierVM.StrUrl = strUrl;

            return View(SupplierVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                SupplierVM = new SupplierViewModel()
                {
                    SupplierDTO = new SupplierDTO(),
                    StrUrl = strUrl
                };

                return View(SupplierVM);
            }

            try
            {
                await _supplierService.CreateAsync(SupplierVM.SupplierDTO); // save

                SetAlert("Thêm mới thành công.", "success");

                return Redirect(strUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(SupplierVM);
            }
        }

        public async Task<IActionResult> Edit(string id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            SupplierVM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            SupplierVM.SupplierDTO = await _supplierService.GetByIdAsync(id);

            if (SupplierVM.SupplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return View(SupplierVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id != SupplierVM.SupplierDTO.Code)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _supplierService.UpdateAsync(SupplierVM.SupplierDTO);
                    SetAlert("Cập nhật thành công", "success");

                    return Redirect(strUrl);
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(SupplierVM);
                }
            }
            // not valid

            return View(SupplierVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, string strUrl/*, string tabActive*/)
        {
            SupplierVM.StrUrl = strUrl;// + "&tabActive=" + tabActive; // for redirect tab

            var supplierDTO = await _supplierService.GetByIdAsync(id);
            if (supplierDTO == null)
                return NotFound();
            try
            {
                await _supplierService.Delete(supplierDTO);

                SetAlert("Xóa thành công.", "success");
                return Redirect(SupplierVM.StrUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                ModelState.AddModelError("", ex.Message);
                return Redirect(SupplierVM.StrUrl);
            }
        }
    }
}