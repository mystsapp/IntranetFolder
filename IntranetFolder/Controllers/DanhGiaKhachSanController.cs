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
    public class DanhGiaKhachSanController : BaseController
    {
        private readonly IDanhGiaKhachSanService _danhGiaKhachSanService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public DanhGiaKhachSanViewModel DanhGiaKhachSanVM { get; set; }

        public DanhGiaKhachSanController(IDanhGiaKhachSanService danhGiaKhachSanService, IWebHostEnvironment webHostEnvironment)
        {
            _danhGiaKhachSanService = danhGiaKhachSanService;
            _webHostEnvironment = webHostEnvironment;
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
            DanhGiaKhachSanVM.DanhGiaKhachSanDTO.TenNcu = DanhGiaKhachSanVM.SupplierDTO.Tengiaodich;
            DanhGiaKhachSanVM.LoaiSaos = SD.LoaiSao();
            return PartialView(DanhGiaKhachSanVM);
        }

        [HttpPost, ActionName("ThemMoiKhachSan_Partial")]
        public async Task<IActionResult> ThemMoiKhachSan_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaKhachSanVM = new DanhGiaKhachSanViewModel()
                {
                    DanhGiaKhachSanDTO = new DanhGiaKhachSanDTO(),
                    StrUrl = strUrl
                };

                return View(DanhGiaKhachSanVM);
            }

            //DanhGiaKhachSanVM.DanhGiaKhachSanDTO.TenNcu = DanhGiaKhachSanVM.TenCreate;
            DanhGiaKhachSanVM.DanhGiaKhachSanDTO.NguoiTao = user.Username;
            DanhGiaKhachSanVM.DanhGiaKhachSanDTO.NgayTao = DateTime.Now;
            DanhGiaKhachSanVM.DanhGiaKhachSanDTO.LoaiDvid = 1; // MaLoai = HTL

            try
            {
                await _danhGiaKhachSanService.CreateAsync(DanhGiaKhachSanVM.DanhGiaKhachSanDTO); // save

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
                await _danhGiaKhachSanService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatKhachSan_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaKhachSanVM.DanhGiaKhachSanDTO = await _danhGiaKhachSanService.GetByIdAsync(id);
            DanhGiaKhachSanVM.SupplierDTO = await _danhGiaKhachSanService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaKhachSanVM.DanhGiaKhachSanDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaKhachSanVM.LoaiSaos = SD.LoaiSao();

            return PartialView(DanhGiaKhachSanVM);
        }

        [HttpPost, ActionName("CapNhatKhachSan_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatNhaHang_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaKhachSanVM.DanhGiaKhachSanDTO.NgaySua = DateTime.Now;
                DanhGiaKhachSanVM.DanhGiaKhachSanDTO.NguoiSua = user.Username;

                try
                {
                    await _danhGiaKhachSanService.UpdateAsync(DanhGiaKhachSanVM.DanhGiaKhachSanDTO);

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
                    await _danhGiaKhachSanService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Thêm mới không thành công!"
                    });
                }
            }
            // not valid
            //DanhGiaKhachSanVM.LoaiDvDTOs = _danhGiaKhachSanService.GetAllLoaiDv();

            return View(DanhGiaKhachSanVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaKhachSanVM.StrUrl = strUrl;

            var DanhGiaKhachSanDTO = _danhGiaKhachSanService.GetByIdAsNoTracking(id);
            if (DanhGiaKhachSanDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaKhachSanService.Delete(DanhGiaKhachSanDTO);

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
                await _danhGiaKhachSanService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }

        public async Task<IActionResult> ExportToWord_KhachSan(string supplierId, long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Khách sạn này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var supplierDTO = await _danhGiaKhachSanService.GetSupplierByIdAsync(supplierId);
            if (string.IsNullOrEmpty(supplierId) || supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            TapDoanDTO tapDoanDTO = await _danhGiaKhachSanService.GetTapDoanByIdAsync(supplierDTO.TapDoanId);
            var danhGiaKhachSanDTO = await _danhGiaKhachSanService.GetByIdAsync(id);

            if (danhGiaKhachSanDTO == null)
            {
                ViewBag.ErrorMessage = "Khách sạn này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var loaiDvDTO = _danhGiaKhachSanService.GetAllLoaiDv().Where(x => x.Id == danhGiaKhachSanDTO.LoaiDvid).FirstOrDefault();

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

            doc.AddCustomProperty(new CustomProperty("TieuChuanSao", danhGiaKhachSanDTO.TieuChuanSao));
            doc.AddCustomProperty(new CustomProperty("GiayPhepKinhDoanh", danhGiaKhachSanDTO.Gpkd == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("VAT", danhGiaKhachSanDTO.Vat == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("HoBoi", danhGiaKhachSanDTO.HoBoi == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("BaiBienRieng", danhGiaKhachSanDTO.BaiBienRieng == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("BaiDoXe", !string.IsNullOrEmpty(danhGiaKhachSanDTO.BaiDoXe) ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("PhongNoiBo", danhGiaKhachSanDTO.PhongNoiBo ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("SoLuongPhong", danhGiaKhachSanDTO.SLPhong));
            doc.AddCustomProperty(new CustomProperty("SoLuongNhaHang", danhGiaKhachSanDTO.SLNhaHang));
            doc.AddCustomProperty(new CustomProperty("SoChoPhongHop", danhGiaKhachSanDTO.SoChoPhongHop));
            doc.AddCustomProperty(new CustomProperty("ViTri", danhGiaKhachSanDTO.ViTri));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThucTe", danhGiaKhachSanDTO.KhaoSatThucTe == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("DatYeuCau", danhGiaKhachSanDTO.KqDat == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThem", !string.IsNullOrWhiteSpace(danhGiaKhachSanDTO.KqKhaoSatThem) ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TaiKy", danhGiaKhachSanDTO.TaiKy == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TiemNang", danhGiaKhachSanDTO.TiemNang == true ? "Có" : ""));

            doc.AddCustomProperty(new CustomProperty("Ngay", DateTime.Now.Day));
            doc.AddCustomProperty(new CustomProperty("Thang", DateTime.Now.Month));
            doc.AddCustomProperty(new CustomProperty("Nam", DateTime.Now.Year));

            doc.AddList("First Item", 0, ListItemType.Numbered);

            MemoryStream stream = new MemoryStream();

            // Saves the Word document to MemoryStream
            doc.SaveAs(stream);
            stream.Position = 0;
            // Download Word document in the browser
            return File(stream, "application/msword", "khachsan_" + user.Username + "_" + DateTime.Now + ".docx");
        }
    }
}