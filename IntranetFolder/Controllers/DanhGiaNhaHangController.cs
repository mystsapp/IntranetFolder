using Common;
using Data.Models;
using Data.Repository;
using Data.Utilities;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Model;
using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetFolder.Controllers
{
    public class DanhGiaNhaHangController : BaseController
    {
        private readonly IDanhGiaNhaHangService _danhGiaNhaHangService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public DanhGiaNhaHangViewModel DanhGiaNhaHangVM { get; set; }

        public DanhGiaNhaHangController(IDanhGiaNhaHangService danhGiaNhaHangService, IWebHostEnvironment webHostEnvironment)
        {
            DanhGiaNhaHangVM = new DanhGiaNhaHangViewModel()
            {
                DanhGiaNhaHangDTO = new DanhGiaNhaHangDTO()
            };
            _danhGiaNhaHangService = danhGiaNhaHangService;
            _webHostEnvironment = webHostEnvironment;
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

        /// <summary>
        /// //////////////////////////////////////// Partial /////////////////////////////////////////
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<IActionResult> ThemMoiNhaHang_Partial(string supplierId) // code
        {
            DanhGiaNhaHangVM.SupplierDTO = await _danhGiaNhaHangService.GetSupplierByIdAsync(supplierId);
            DanhGiaNhaHangVM.DanhGiaNhaHangDTO.SupplierId = supplierId;
            DanhGiaNhaHangVM.DanhGiaNhaHangDTO.TenNcu = DanhGiaNhaHangVM.SupplierDTO.Tengiaodich;
            DanhGiaNhaHangVM.LoaiMenus = SD.LoaiMenu();
            DanhGiaNhaHangVM.ChatLuongMonAns = SD.LoaiMenu();
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
                    InnerMessage = ex.InnerException.Message,
                    Message = ex.Message,
                    NgayTao = DateTime.Now,
                    NguoiTao = user.Nguoitao
                };
                await _danhGiaNhaHangService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatNhaHang_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaNhaHangVM.DanhGiaNhaHangDTO = await _danhGiaNhaHangService.GetByIdAsync(id);
            DanhGiaNhaHangVM.SupplierDTO = await _danhGiaNhaHangService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaNhaHangVM.DanhGiaNhaHangDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            //DanhGiaNhaHangVM.LoaiDvDTOs = _danhGiaNhaHangService.GetAllLoaiDv();

            return PartialView(DanhGiaNhaHangVM);
        }

        [HttpPost, ActionName("CapNhatNhaHang_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatNhaHang_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaNhaHangVM.DanhGiaNhaHangDTO.NgaySua = DateTime.Now;
                DanhGiaNhaHangVM.DanhGiaNhaHangDTO.NguoiSua = user.Username;

                try
                {
                    await _danhGiaNhaHangService.UpdateAsync(DanhGiaNhaHangVM.DanhGiaNhaHangDTO);

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
                    await _danhGiaNhaHangService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Thêm mới không thành công!"
                    });
                }
            }
            // not valid
            //DanhGiaNhaHangVM.LoaiDvDTOs = _danhGiaNhaHangService.GetAllLoaiDv();

            return View(DanhGiaNhaHangVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaNhaHangVM.StrUrl = strUrl;

            var danhGiaNhaHangDTO = _danhGiaNhaHangService.GetByIdAsNoTracking(id);
            if (danhGiaNhaHangDTO == null)
                return NotFound();
            try
            {
                await _danhGiaNhaHangService.Delete(danhGiaNhaHangDTO);

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
                await _danhGiaNhaHangService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }

        public async Task<IActionResult> ExportToWord_Nhahang(string supplierId, long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Nhà hàng này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var supplierDTO = await _danhGiaNhaHangService.GetSupplierByIdAsync(supplierId);
            if (string.IsNullOrEmpty(supplierId) || supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var tapDoanDTO = await _danhGiaNhaHangService.GetTapDoanByIdAsync(supplierDTO.TapDoanId);
            var danhGiaNhaHangDTO = await _danhGiaNhaHangService.GetByIdAsync(id);

            if (danhGiaNhaHangDTO == null)
            {
                ViewBag.ErrorMessage = "Nhà hàng này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var loaiDvDTO = _danhGiaNhaHangService.GetAllLoaiDv().Where(x => x.Id == danhGiaNhaHangDTO.LoaiDvid).FirstOrDefault();

            DocX doc = null;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = webRootPath + @"\WordTemplates\M01b-DGNCU-NH.docx";
            doc = DocX.Load(fileName);

            doc.AddCustomProperty(new CustomProperty("TenGiaoDich", supplierDTO.Code + " - " + supplierDTO.Tengiaodich));
            doc.AddCustomProperty(new CustomProperty("TenThuongMai", supplierDTO.Tenthuongmai));
            doc.AddCustomProperty(new CustomProperty("TapDoan", tapDoanDTO == null ? "***" : tapDoanDTO.Ten));
            doc.AddCustomProperty(new CustomProperty("DiaChi", supplierDTO.Diachi));
            doc.AddCustomProperty(new CustomProperty("DienThoai/Email", supplierDTO.Dienthoai + "/" + supplierDTO.Email));
            doc.AddCustomProperty(new CustomProperty("LoaiHinhDV", loaiDvDTO.TenLoai));

            doc.AddCustomProperty(new CustomProperty("GiayPhepKinhDoanh", danhGiaNhaHangDTO.CoGpkd == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("VAT", danhGiaNhaHangDTO.CoHdvat == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("DinhLuongMonAn", danhGiaNhaHangDTO.DinhLuongMonAn));
            doc.AddCustomProperty(new CustomProperty("BaiDoXe", !string.IsNullOrEmpty(danhGiaNhaHangDTO.BaiDoXe) ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("CoTChuanNoiBo", danhGiaNhaHangDTO.CoTieuChuanNoiBo == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("CoPhongKhuVucRieng", danhGiaNhaHangDTO.PhongKVRieng ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("ViTri", danhGiaNhaHangDTO.ViTri));
            doc.AddCustomProperty(new CustomProperty("SoChoToiDa", danhGiaNhaHangDTO.SoChoToiDa));
            doc.AddCustomProperty(new CustomProperty("Menu", danhGiaNhaHangDTO.Menu));
            doc.AddCustomProperty(new CustomProperty("ChatLuongMonAn", danhGiaNhaHangDTO.ChatLuongMonAn));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThucTe", danhGiaNhaHangDTO.CoKhaoSatThucTe == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("DatYeuCau", danhGiaNhaHangDTO.KqDat == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThem", danhGiaNhaHangDTO.KqKhaoSatThem == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TaiKy", danhGiaNhaHangDTO.TaiKy == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TiemNang", danhGiaNhaHangDTO.TiemNang == true ? "Có" : ""));

            doc.AddCustomProperty(new CustomProperty("Ngay", DateTime.Now.Day));
            doc.AddCustomProperty(new CustomProperty("Thang", DateTime.Now.Month));
            doc.AddCustomProperty(new CustomProperty("Nam", DateTime.Now.Year));

            doc.AddList("First Item", 0, ListItemType.Numbered);

            MemoryStream stream = new MemoryStream();

            // Saves the Word document to MemoryStream
            doc.SaveAs(stream);
            stream.Position = 0;
            // Download Word document in the browser
            return File(stream, "application/msword", "nhahang_" + user.Username + "_" + DateTime.Now + ".docx");
        }
    }
}