using Data.Models;
using Data.Repository;
using Data.Utilities;
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
                    Supplier = new Supplier(),
                    StrUrl = strUrl
                };

                return View(SupplierVM);
            }

            try
            {
                await _supplierService.CreateAsync(DMTaiKhoanVM.DmTk); // save

                SetAlert("Thêm mới thành công.", "success");

                return Redirect(strUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(DMTaiKhoanVM);
            }
        }

        public async Task<IActionResult> Edit(int id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DMTaiKhoanVM.StrUrl = strUrl;
            if (id == 0)
            {
                ViewBag.ErrorMessage = "TK này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DMTaiKhoanVM.DmTk = await _dmTkService.GetById(id);

            if (DMTaiKhoanVM.DmTk == null)
            {
                ViewBag.ErrorMessage = "TK này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return View(DMTaiKhoanVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id != DMTaiKhoanVM.DmTk.Id)
            {
                ViewBag.ErrorMessage = "TK này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _dmTkService.UpdateAsync(DMTaiKhoanVM.DmTk);
                    SetAlert("Cập nhật thành công", "success");

                    return Redirect(strUrl);
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(DMTaiKhoanVM);
                }
            }
            // not valid

            return View(DMTaiKhoanVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string strUrl/*, string tabActive*/)
        {
            DMTaiKhoanVM.StrUrl = strUrl;// + "&tabActive=" + tabActive; // for redirect tab

            var dmTk = await _dmTkService.GetById(id);
            if (dmTk == null)
                return NotFound();
            try
            {
                await _dmTkService.Delete(dmTk);

                SetAlert("Xóa thành công.", "success");
                return Redirect(DMTaiKhoanVM.StrUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                ModelState.AddModelError("", ex.Message);
                return Redirect(DMTaiKhoanVM.StrUrl);
            }
        }
    }
}