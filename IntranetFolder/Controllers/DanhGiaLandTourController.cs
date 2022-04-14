using Common;
using Data.Models;
using Data.Utilities;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Model;
using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetFolder.Controllers
{
    public class DanhGiaLandTourController : BaseController
    {
        private readonly IDanhGiaLandTourService _danhGiaLandTourService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public DanhGiaLandTourViewModel DanhGiaLandTourVM { get; set; }

        public DanhGiaLandTourController(IDanhGiaLandTourService danhGiaLandTourService, IWebHostEnvironment webHostEnvironment)
        {
            DanhGiaLandTourVM = new DanhGiaLandTourViewModel()
            {
                DanhGiaLandTourDTO = new DanhGiaLandTourDTO(),
                StrUrl = ""
            };
            _danhGiaLandTourService = danhGiaLandTourService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LandTour_Partial(string supplierId)
        {
            SupplierDTO supplierDTO = await _danhGiaLandTourService.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaLandTourVM.SupplierDTO = supplierDTO;
            DanhGiaLandTourVM.DanhGiaLandTourDTOs = await _danhGiaLandTourService.GetDanhGiaLandTourBy_SupplierId(supplierId);

            return PartialView(DanhGiaLandTourVM);
        }

        public async Task<IActionResult> ThemMoiLandTour_Partial(string supplierId) // code
        {
            DanhGiaLandTourVM.SupplierDTO = await _danhGiaLandTourService.GetSupplierByIdAsync(supplierId);
            DanhGiaLandTourVM.DanhGiaLandTourDTO.SupplierId = supplierId;
            DanhGiaLandTourVM.DanhGiaLandTourDTO.TenNcu = DanhGiaLandTourVM.SupplierDTO.Tengiaodich;
            DanhGiaLandTourVM.ChatLuongDVs = SD.ChatLuongDV();
            DanhGiaLandTourVM.SanPhams = SD.SanPham();
            DanhGiaLandTourVM.GiaCas = SD.GiaCa();
            return PartialView(DanhGiaLandTourVM);
        }

        [HttpPost, ActionName("ThemMoiLandTour_Partial")]
        public async Task<IActionResult> ThemMoiLandTour_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaLandTourVM = new DanhGiaLandTourViewModel()
                {
                    DanhGiaLandTourDTO = new DanhGiaLandTourDTO(),
                    StrUrl = strUrl
                };

                return Json(new
                {
                    status = false,
                    message = "Model state is not valid"
                });
            }

            DanhGiaLandTourVM.DanhGiaLandTourDTO.NguoiTao = user.Username;
            DanhGiaLandTourVM.DanhGiaLandTourDTO.NgayTao = DateTime.Now;
            DanhGiaLandTourVM.DanhGiaLandTourDTO.LoaiDvid = 6; // MaLoai = LCT

            try
            {
                await _danhGiaLandTourService.CreateAsync(DanhGiaLandTourVM.DanhGiaLandTourDTO); // save

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
                await _danhGiaLandTourService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatLandTour_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaLandTourVM.DanhGiaLandTourDTO = await _danhGiaLandTourService.GetByIdAsync(id);
            DanhGiaLandTourVM.SupplierDTO = await _danhGiaLandTourService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaLandTourVM.DanhGiaLandTourDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaLandTourVM.ChatLuongDVs = SD.ChatLuongDV();
            DanhGiaLandTourVM.SanPhams = SD.SanPham();
            DanhGiaLandTourVM.GiaCas = SD.GiaCa();

            return PartialView(DanhGiaLandTourVM);
        }

        [HttpPost, ActionName("CapNhatLandTour_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatLandTour_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaLandTourVM.DanhGiaLandTourDTO.NgaySua = DateTime.Now;
                DanhGiaLandTourVM.DanhGiaLandTourDTO.NguoiSua = user.Username;

                try
                {
                    await _danhGiaLandTourService.UpdateAsync(DanhGiaLandTourVM.DanhGiaLandTourDTO);

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
                    await _danhGiaLandTourService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Thêm mới không thành công!"
                    });
                }
            }
            // not valid

            return View(DanhGiaLandTourVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaLandTourVM.StrUrl = strUrl;

            var danhGiaLandTourDTO = _danhGiaLandTourService.GetByIdAsNoTracking(id);
            if (danhGiaLandTourDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaLandTourService.Delete(danhGiaLandTourDTO);

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
                await _danhGiaLandTourService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }

        public async Task<IActionResult> ExportToWord_LandTour(string supplierId, long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var supplierDTO = await _danhGiaLandTourService.GetSupplierByIdAsync(supplierId);
            if (string.IsNullOrEmpty(supplierId) || supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            TapDoanDTO tapDoanDTO = await _danhGiaLandTourService.GetTapDoanByIdAsync(supplierDTO.TapDoanId);
            var danhGiaLandTourDTO = await _danhGiaLandTourService.GetByIdAsync(id);

            if (danhGiaLandTourDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var loaiDvDTO = _danhGiaLandTourService.GetAllLoaiDv().Where(x => x.Id == danhGiaLandTourDTO.LoaiDvid).FirstOrDefault();

            DocX doc = null;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = webRootPath + @"\WordTemplates\M01-DGNCU-KS.docx";
            doc = DocX.Load(fileName);

            doc.AddCustomProperty(new CustomProperty("TenGiaoDich", supplierDTO.Code + " - " + supplierDTO.Tengiaodich));
            doc.AddCustomProperty(new CustomProperty("TenThuongMai", supplierDTO.Tenthuongmai));
            doc.AddCustomProperty(new CustomProperty("TapDoan", tapDoanDTO == null ? "" : tapDoanDTO.Ten));
            doc.AddCustomProperty(new CustomProperty("DiaChi", supplierDTO.Diachi));
            doc.AddCustomProperty(new CustomProperty("DienThoai/Email", supplierDTO.Dienthoai + "/" + supplierDTO.Email));
            doc.AddCustomProperty(new CustomProperty("LoaiHinhDV", loaiDvDTO.TenLoai));

            doc.AddCustomProperty(new CustomProperty("GiayPhepKinhDoanh", danhGiaLandTourDTO.Gpkd == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("VAT", danhGiaLandTourDTO.Vat == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("CoHoTroXuLySuCo", danhGiaLandTourDTO.CoHoTroXuLySuCo == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("CoKhaNangHuyDong", danhGiaLandTourDTO.CoKhaNangHuyDong == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThucTe", danhGiaLandTourDTO.KhaoSatThucTe == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("Tuyen", danhGiaLandTourDTO.Tuyen));
            doc.AddCustomProperty(new CustomProperty("ThoiGianHoatDong", danhGiaLandTourDTO.ThoiGianHoatDong));
            doc.AddCustomProperty(new CustomProperty("CacDoiTacLon", danhGiaLandTourDTO.CacDoiTacLon));
            doc.AddCustomProperty(new CustomProperty("ChatLuongDichVu", danhGiaLandTourDTO.ChatLuongDichVu));
            doc.AddCustomProperty(new CustomProperty("SanPham", danhGiaLandTourDTO.SanPham));
            doc.AddCustomProperty(new CustomProperty("GiaCa", danhGiaLandTourDTO.GiaCa));
            doc.AddCustomProperty(new CustomProperty("DatYeuCau", danhGiaLandTourDTO.KqDat == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThem", danhGiaLandTourDTO.KqKhaoSatThem == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TaiKy", danhGiaLandTourDTO.TaiKy == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TiemNang", danhGiaLandTourDTO.TiemNang == true ? "Có" : ""));

            doc.AddCustomProperty(new CustomProperty("Ngay", DateTime.Now.Day));
            doc.AddCustomProperty(new CustomProperty("Thang", DateTime.Now.Month));
            doc.AddCustomProperty(new CustomProperty("Nam", DateTime.Now.Year));

            doc.AddList("First Item", 0, ListItemType.Numbered);

            MemoryStream stream = new MemoryStream();

            // Saves the Word document to MemoryStream
            doc.SaveAs(stream);
            stream.Position = 0;
            // Download Word document in the browser
            return File(stream, "application/msword", "lantourND_" + user.Username + "_" + DateTime.Now + ".docx");
        }
    }
}