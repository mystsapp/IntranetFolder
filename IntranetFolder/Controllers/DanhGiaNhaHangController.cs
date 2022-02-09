using Data.Models;
using Data.Repository;
using Data.Utilities;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetFolder.Controllers
{
    public class DanhGiaNhaHangController : BaseController
    {
        private readonly IDanhGiaNhaHangService _danhGiaNhaHangService;

        [BindProperty]
        public DanhGiaNhaHangViewModel DanhGiaNhaHangVM { get; set; }

        public DanhGiaNhaHangController(IDanhGiaNhaHangService danhGiaNhaHangService)
        {
            DanhGiaNhaHangVM = new DanhGiaNhaHangViewModel()
            {
                DanhGiaNhaHangDTO = new DanhGiaNhaHangDTO()
            };
            _danhGiaNhaHangService = danhGiaNhaHangService;
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, long id, int page = 1)
        {
            if (id == 0)
            {
                ViewBag.id = "";
            }

            DanhGiaNhaHangVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            DanhGiaNhaHangVM.Page = page;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            if (id != 0) // for redirect with id
            {
                DanhGiaNhaHangVM.DanhGiaNhaHangDTO = await _danhGiaNhaHangService.GetByIdAsync(id);
                ViewBag.id = DanhGiaNhaHangVM.DanhGiaNhaHangDTO.Id;
            }
            else
            {
                DanhGiaNhaHangVM.DanhGiaNhaHangDTO = new DanhGiaNhaHangDTO();
            }
            DanhGiaNhaHangVM.DanhGiaNhaHangDTOs = await _danhGiaNhaHangService.ListDanhGiaNCU(searchString, searchFromDate, searchToDate, page);
            return View(DanhGiaNhaHangVM);
        }

        public IActionResult Create(string strUrl)
        {
            DanhGiaNhaHangVM.StrUrl = strUrl;

            return View(DanhGiaNhaHangVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaNhaHangVM = new DanhGiaNhaHangViewModel()
                {
                    DanhGiaNhaHangDTO = new DanhGiaNhaHangDTO(),
                    StrUrl = strUrl
                };

                return View(DanhGiaNhaHangVM);
            }

            //DanhGiaNhaHangVM.DanhGiaNhaHangDTO.TenNcu = DanhGiaNhaHangVM.TenCreate;
            DanhGiaNhaHangVM.DanhGiaNhaHangDTO.NguoiTao = user.Username;
            DanhGiaNhaHangVM.DanhGiaNhaHangDTO.NgayTao = DateTime.Now;
            DanhGiaNhaHangVM.DanhGiaNhaHangDTO.LoaiDvid = 2; // MaLoai = RST

            try
            {
                await _danhGiaNhaHangService.CreateAsync(DanhGiaNhaHangVM.DanhGiaNhaHangDTO); // save

                SetAlert("Thêm mới thành công.", "success");

                return Redirect(strUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.InnerException.Message, "error");
                return View(DanhGiaNhaHangVM);
            }
        }

        public async Task<IActionResult> Edit(long id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaNhaHangVM.StrUrl = strUrl;
            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaNhaHangVM.DanhGiaNhaHangDTO = await _danhGiaNhaHangService.GetByIdAsync(id);

            if (DanhGiaNhaHangVM.DanhGiaNhaHangDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            //DanhGiaNhaHangVM.LoaiDvDTOs = _danhGiaNhaHangService.GetAllLoaiDv();

            return View(DanhGiaNhaHangVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id != DanhGiaNhaHangVM.DanhGiaNhaHangDTO.Id)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                DanhGiaNhaHangVM.DanhGiaNhaHangDTO.NgaySua = DateTime.Now;
                DanhGiaNhaHangVM.DanhGiaNhaHangDTO.NguoiSua = user.Username;

                try
                {
                    await _danhGiaNhaHangService.UpdateAsync(DanhGiaNhaHangVM.DanhGiaNhaHangDTO);
                    SetAlert("Cập nhật thành công", "success");

                    //return Redirect(strUrl);
                    return RedirectToAction(nameof(Index), new { id = id });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(DanhGiaNhaHangVM);
                }
            }
            // not valid
            //DanhGiaNhaHangVM.LoaiDvDTOs = _danhGiaNhaHangService.GetAllLoaiDv();

            return View(DanhGiaNhaHangVM);
        }

        public async Task<JsonResult> IsStringNameAvailable(DanhGiaNhaHangDTO DanhGiaNhaHangDTO)
        {
            if (DanhGiaNhaHangDTO.Id == 0) // create
            {
                var boolName = _danhGiaNhaHangService.GetAll()
                .Where(x => x.TenNcu.Trim().ToLower() == DanhGiaNhaHangDTO.TenNcu.Trim().ToLower())
                .FirstOrDefault();
                if (boolName == null)
                {
                    return Json(true); // duoc
                }
                else
                {
                    return Json(false); // ko duoc
                }
            }
            else // edit
            {
                var result = await _danhGiaNhaHangService.CheckNameExist(DanhGiaNhaHangDTO.Id, DanhGiaNhaHangDTO.TenNcu);
                return Json(result);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            DanhGiaNhaHangVM.StrUrl = strUrl;

            var danhGiaNhaHangDTO = _danhGiaNhaHangService.GetByIdAsNoTracking(id);
            if (danhGiaNhaHangDTO == null)
                return NotFound();
            try
            {
                await _danhGiaNhaHangService.Delete(danhGiaNhaHangDTO);

                SetAlert("Xóa thành công.", "success");
                return Redirect(DanhGiaNhaHangVM.StrUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                ModelState.AddModelError("", ex.Message);
                return Redirect(DanhGiaNhaHangVM.StrUrl);
            }
        }

        /// <summary>
        /// //////////////////////////////////////// Partial /////////////////////////////////////////
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<IActionResult> ThemMoiNhaHang_Partial(string supplierId) // code
        {
            DanhGiaNhaHangVM.SupplierDTO = await _danhGiaNhaHangService.GetSupplierByIdAsync(supplierId);
            DanhGiaNhaHangVM.DanhGiaNhaHangDTO.SupplierId = supplierId;
            return PartialView(DanhGiaNhaHangVM);
        }

        [HttpPost, ActionName("ThemMoiNhaHang_Partial")]
        public async Task<IActionResult> ThemMoiNhaHang_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaNhaHangVM = new DanhGiaNhaHangViewModel()
                {
                    DanhGiaNhaHangDTO = new DanhGiaNhaHangDTO(),
                    StrUrl = strUrl
                };

                return View(DanhGiaNhaHangVM);
            }

            //DanhGiaNhaHangVM.DanhGiaNhaHangDTO.TenNcu = DanhGiaNhaHangVM.TenCreate;
            DanhGiaNhaHangVM.DanhGiaNhaHangDTO.NguoiTao = user.Username;
            DanhGiaNhaHangVM.DanhGiaNhaHangDTO.NgayTao = DateTime.Now;
            DanhGiaNhaHangVM.DanhGiaNhaHangDTO.LoaiDvid = 2; // MaLoai = RST

            try
            {
                await _danhGiaNhaHangService.CreateAsync(DanhGiaNhaHangVM.DanhGiaNhaHangDTO); // save

                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                ErrorLog errorLog = new ErrorLog()
                {
                    Message = ex.Message
                };
                await _danhGiaNhaHangService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = true,
                    message = "Thêm mới không thành công!"
                });
            }
        }
    }
}