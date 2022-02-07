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

        public async Task<IActionResult> Create(string strUrl)
        {
            SupplierVM.StrUrl = strUrl;
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            SupplierVM.VTinhs = await _supplierService.GetTinhs();
            SupplierVM.Thanhpho1s = await _supplierService.GetThanhpho1s();
            SupplierVM.Quocgias = await _supplierService.GetQuocgias();

            // next Id
            SupplierVM.SupplierDTO.Code = _supplierService.GetNextId("", user.Macn);
            // next Id

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

            SupplierVM.SupplierDTO.Ngaytao = DateTime.Now;
            SupplierVM.SupplierDTO.Nguoitao = user.Username;

            // next Id
            SupplierVM.SupplierDTO.Code = _supplierService.GetNextId("", user.Macn);
            // next Id

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
                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                string temp = "", log = "";

                var t = _supplierService.GetByIdAsNoTracking(id);

                if (t.Diachi != SupplierVM.SupplierDTO.Diachi)
                {
                    temp += String.Format("- Diachi thay đổi: {0}->{1}", t.Diachi, SupplierVM.SupplierDTO.Diachi);
                }

                if (t.Dienthoai != SupplierVM.SupplierDTO.Dienthoai)
                {
                    temp += String.Format("- Dienthoai thay đổi: {0}->{1}", t.Dienthoai, SupplierVM.SupplierDTO.Dienthoai);
                }

                if (t.Email != SupplierVM.SupplierDTO.Email)
                {
                    temp += String.Format("- Email thay đổi: {0}->{1}", t.Email, SupplierVM.SupplierDTO.Email);
                }

                if (t.Fax != SupplierVM.SupplierDTO.Fax)
                {
                    temp += String.Format("- Fax thay đổi: {0}->{1}", t.Fax, SupplierVM.SupplierDTO.Fax);
                }

                if (t.Masothue != SupplierVM.SupplierDTO.Masothue)
                {
                    temp += String.Format("- Masothue thay đổi: {0}->{1}", t.Masothue, SupplierVM.SupplierDTO.Masothue);
                }

                if (t.Nganhnghe != SupplierVM.SupplierDTO.Nganhnghe)
                {
                    temp += String.Format("- Nganhnghe thay đổi: {0}->{1}", t.Nganhnghe, SupplierVM.SupplierDTO.Nganhnghe);
                }

                if (t.Ngayhethan != SupplierVM.SupplierDTO.Ngayhethan)
                {
                    temp += String.Format("- Ngayhethan thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", t.Ngayhethan, SupplierVM.SupplierDTO.Ngayhethan);
                }

                if (t.Nguoilienhe != SupplierVM.SupplierDTO.Nguoilienhe)
                {
                    temp += String.Format("- Nguoilienhe thay đổi: {0}->{1}", t.Nguoilienhe, SupplierVM.SupplierDTO.Nguoilienhe);
                }

                if (t.Quocgia != SupplierVM.SupplierDTO.Quocgia)
                {
                    temp += String.Format("- Quocgia thay đổi: {0}->{1}", t.Quocgia, SupplierVM.SupplierDTO.Quocgia);
                }

                if (t.Tapdoan != SupplierVM.SupplierDTO.Tapdoan)
                {
                    temp += String.Format("- Tapdoan thay đổi: {0}->{1}", t.Tapdoan, SupplierVM.SupplierDTO.Tapdoan);
                }

                if (t.Tengiaodich != SupplierVM.SupplierDTO.Tengiaodich)
                {
                    temp += String.Format("- Tengiaodich thay đổi: {0}->{1}", t.Tengiaodich, SupplierVM.SupplierDTO.Tengiaodich);
                }

                if (t.Tennganhang != SupplierVM.SupplierDTO.Tennganhang)
                {
                    temp += String.Format("- Tennganhang thay đổi: {0}->{1}", t.Tennganhang, SupplierVM.SupplierDTO.Tennganhang);
                }

                if (t.Tenthuongmai != SupplierVM.SupplierDTO.Tenthuongmai)
                {
                    temp += String.Format("- Tenthuongmai thay đổi: {0}->{1}", t.Tenthuongmai, SupplierVM.SupplierDTO.Tenthuongmai);
                }

                if (t.Thanhpho != SupplierVM.SupplierDTO.Thanhpho)
                {
                    temp += String.Format("- Thanhpho thay đổi: {0}->{1}", t.Thanhpho, SupplierVM.SupplierDTO.Thanhpho);
                }

                if (t.Tinhtp != SupplierVM.SupplierDTO.Tinhtp)
                {
                    temp += String.Format("- Tinhtp thay đổi: {0}->{1}", t.Tinhtp, SupplierVM.SupplierDTO.Tinhtp);
                }

                if (t.Tknganhang != SupplierVM.SupplierDTO.Tknganhang)
                {
                    temp += String.Format("- Tknganhang thay đổi: {0}->{1}", t.Tknganhang, SupplierVM.SupplierDTO.Tknganhang);
                }

                if (t.Tour != SupplierVM.SupplierDTO.Tour)
                {
                    temp += String.Format("- Tour thay đổi: {0}->{1}", t.Tour, SupplierVM.SupplierDTO.Tour);
                }

                if (t.Trangthai != SupplierVM.SupplierDTO.Trangthai)
                {
                    temp += String.Format("- Trangthai thay đổi: {0}->{1}", t.Tinhtp, SupplierVM.SupplierDTO.Trangthai);
                }

                if (t.Website != SupplierVM.SupplierDTO.Website)
                {
                    temp += String.Format("- Website thay đổi: {0}->{1}", t.Website, SupplierVM.SupplierDTO.Website);
                }

                #endregion log file

                // kiem tra thay doi
                if (temp.Length > 0)
                {
                    log = System.Environment.NewLine;
                    log += "=============";
                    log += System.Environment.NewLine;
                    log += temp + " -User cập nhật tour: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // username
                    t.Logfile = t.Logfile + log;
                    SupplierVM.SupplierDTO.Logfile = t.Logfile;
                }

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

        public async Task<IActionResult> DichVuPartial(string id, string strUrl)
        {
            SupplierVM.StrUrl = strUrl;// + "&tabActive=" + tabActive; // for redirect tab

            var supplierDTO = await _supplierService.GetByIdAsync(id);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return PartialView(SupplierVM);
        }
    }
}