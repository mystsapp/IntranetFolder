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
    public class DanhGiaGolfController : BaseController
    {
        private readonly IDanhGiaGolfService _danhGiaGolfService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public DanhGiaGolfViewModel DanhGiaGolfVM { get; set; }

        public DanhGiaGolfController(IDanhGiaGolfService danhGiaGolfService, IWebHostEnvironment webHostEnvironment)
        {
            _danhGiaGolfService = danhGiaGolfService;
            _webHostEnvironment = webHostEnvironment;
            DanhGiaGolfVM = new DanhGiaGolfViewModel()
            {
                DanhGiaGolfDTO = new DanhGiaGolfDTO(),
                StrUrl = ""
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Golf_Partial(string supplierId)
        {
            SupplierDTO supplierDTO = await _danhGiaGolfService.GetBySupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaGolfVM.SupplierDTO = supplierDTO;
            DanhGiaGolfVM.DanhGiaGolfDTOs = await _danhGiaGolfService.GetDanhGiaGolfBy_SupplierId(supplierId);

            return PartialView(DanhGiaGolfVM);
        }

        public async Task<IActionResult> ThemMoiGolf_Partial(string supplierId) // code
        {
            DanhGiaGolfVM.SupplierDTO = await _danhGiaGolfService.GetSupplierByIdAsync(supplierId);
            DanhGiaGolfVM.DanhGiaGolfDTO.SupplierId = supplierId;
            DanhGiaGolfVM.DanhGiaGolfDTO.TenNcu = DanhGiaGolfVM.SupplierDTO.Tengiaodich;
            return PartialView(DanhGiaGolfVM);
        }

        [HttpPost, ActionName("ThemMoiGolf_Partial")]
        public async Task<IActionResult> ThemMoiGolf_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaGolfVM = new DanhGiaGolfViewModel()
                {
                    DanhGiaGolfDTO = new DanhGiaGolfDTO(),
                    StrUrl = strUrl
                };

                return View(DanhGiaGolfVM);
            }

            //DanhGiaGolfVM.DanhGiaGolfDTO.TenNcu = DanhGiaGolfVM.TenCreate;
            DanhGiaGolfVM.DanhGiaGolfDTO.NguoiTao = user.Username;
            DanhGiaGolfVM.DanhGiaGolfDTO.NgayTao = DateTime.Now;
            DanhGiaGolfVM.DanhGiaGolfDTO.LoaiDvid = 8; // MaLoai = GLF

            try
            {
                await _danhGiaGolfService.CreateAsync(DanhGiaGolfVM.DanhGiaGolfDTO); // save

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
                await _danhGiaGolfService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatGolf_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaGolfVM.DanhGiaGolfDTO = await _danhGiaGolfService.GetByIdAsync(id);
            DanhGiaGolfVM.SupplierDTO = await _danhGiaGolfService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaGolfVM.DanhGiaGolfDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return PartialView(DanhGiaGolfVM);
        }

        [HttpPost, ActionName("CapNhatGolf_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatGolf_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaGolfVM.DanhGiaGolfDTO.NgaySua = DateTime.Now;
                DanhGiaGolfVM.DanhGiaGolfDTO.NguoiSua = user.Username;

                try
                {
                    await _danhGiaGolfService.UpdateAsync(DanhGiaGolfVM.DanhGiaGolfDTO);

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
                    await _danhGiaGolfService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Thêm mới không thành công!"
                    });
                }
            }
            // not valid
            //DanhGiaGolfVM.LoaiDvDTOs = _danhGiaGolfService.GetAllLoaiDv();

            return View(DanhGiaGolfVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaGolfVM.StrUrl = strUrl;

            var DanhGiaGolfDTO = _danhGiaGolfService.GetByIdAsNoTracking(id);
            if (DanhGiaGolfDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaGolfService.Delete(DanhGiaGolfDTO);

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
                await _danhGiaGolfService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }

        //public async Task<IActionResult> ExportToWord_KhachSan(string supplierId, long id, string strUrl)
        //{
        //    // from login session
        //    var user = HttpContext.Session.GetSingle<User>("loginUser");

        //    if (id == 0)
        //    {
        //        ViewBag.ErrorMessage = "Khách sạn này không tồn tại.";
        //        return View("~/Views/Shared/NotFound.cshtml");
        //    }
        //    var supplierDTO = await _danhGiaGolfService.GetSupplierByIdAsync(supplierId);
        //    if (string.IsNullOrEmpty(supplierId) || supplierDTO == null)
        //    {
        //        ViewBag.ErrorMessage = "Supplier này không tồn tại.";
        //        return View("~/Views/Shared/NotFound.cshtml");
        //    }

        //    TapDoanDTO tapDoanDTO = await _danhGiaGolfService.GetTapDoanByIdAsync(supplierDTO.TapDoanId);
        //    var DanhGiaGolfDTO = await _danhGiaGolfService.GetByIdAsync(id);

        //    if (DanhGiaGolfDTO == null)
        //    {
        //        ViewBag.ErrorMessage = "Khách sạn này không tồn tại.";
        //        return View("~/Views/Shared/NotFound.cshtml");
        //    }
        //    var loaiDvDTO = _danhGiaGolfService.GetAllLoaiDv().Where(x => x.Id == DanhGiaGolfDTO.LoaiDvid).FirstOrDefault();

        //    DocX doc = null;
        //    string webRootPath = _webHostEnvironment.WebRootPath;
        //    string fileName = webRootPath + @"\WordTemplates\M01-DGNCU-KS.docx";
        //    doc = DocX.Load(fileName);

        //    doc.AddCustomProperty(new CustomProperty("TenGiaoDich", supplierDTO.Tengiaodich));
        //    doc.AddCustomProperty(new CustomProperty("TenThuongMai", supplierDTO.Tenthuongmai));
        //    doc.AddCustomProperty(new CustomProperty("TapDoan", tapDoanDTO == null ? "" : tapDoanDTO.Ten));
        //    doc.AddCustomProperty(new CustomProperty("DiaChi", supplierDTO.Diachi));
        //    doc.AddCustomProperty(new CustomProperty("DienThoai/Email", supplierDTO.Dienthoai + "/" + supplierDTO.Email));
        //    doc.AddCustomProperty(new CustomProperty("LoaiHinhDV", loaiDvDTO.TenLoai));

        //    doc.AddCustomProperty(new CustomProperty("TieuChuanSao", DanhGiaGolfDTO.TieuChuanSao));
        //    doc.AddCustomProperty(new CustomProperty("GiayPhepKinhDoanh", DanhGiaGolfDTO.Gpkd == true ? "Có" : "Không"));
        //    doc.AddCustomProperty(new CustomProperty("VAT", DanhGiaGolfDTO.Vat == true ? "Có" : "Không"));
        //    doc.AddCustomProperty(new CustomProperty("HoBoi", DanhGiaGolfDTO.CoHoBoi == true ? "Có" : "Không"));
        //    doc.AddCustomProperty(new CustomProperty("BaiBienRieng", DanhGiaGolfDTO.CoBien == true ? "Có" : "Không"));
        //    doc.AddCustomProperty(new CustomProperty("BaiDoXe", !string.IsNullOrEmpty(DanhGiaGolfDTO.CoBaiDoXe) ? "Có" : "Không"));
        //    doc.AddCustomProperty(new CustomProperty("PhongNoiBo", DanhGiaGolfDTO.CoBoTriPhongChoNb));
        //    doc.AddCustomProperty(new CustomProperty("SoLuongPhong", DanhGiaGolfDTO.SoLuongPHong));
        //    doc.AddCustomProperty(new CustomProperty("SoLuongNhaHang", DanhGiaGolfDTO.SLNhaHang));
        //    doc.AddCustomProperty(new CustomProperty("SoChoPhongHop", DanhGiaGolfDTO.SoChoPhongHop));
        //    doc.AddCustomProperty(new CustomProperty("ViTri", DanhGiaGolfDTO.ViTri));
        //    doc.AddCustomProperty(new CustomProperty("KhaoSatThucTe", DanhGiaGolfDTO.DaCoKhaoSatThucTe == true ? "Có" : "Không"));
        //    doc.AddCustomProperty(new CustomProperty("DatYeuCau", DanhGiaGolfDTO.KqDat == true ? "Có" : ""));
        //    doc.AddCustomProperty(new CustomProperty("KhaoSatThem", !string.IsNullOrWhiteSpace(DanhGiaGolfDTO.KqKhaoSatThem) ? "Có" : ""));
        //    doc.AddCustomProperty(new CustomProperty("TaiKy", DanhGiaGolfDTO.TaiKy == true ? "Có" : ""));
        //    doc.AddCustomProperty(new CustomProperty("TiemNang", DanhGiaGolfDTO.TiemNang == true ? "Có" : ""));

        //    doc.AddCustomProperty(new CustomProperty("Ngay", DateTime.Now.Day));
        //    doc.AddCustomProperty(new CustomProperty("Thang", DateTime.Now.Month));

        //    doc.AddList("First Item", 0, ListItemType.Numbered);

        //    MemoryStream stream = new MemoryStream();

        //    // Saves the Word document to MemoryStream
        //    doc.SaveAs(stream);
        //    stream.Position = 0;
        //    // Download Word document in the browser
        //    return File(stream, "application/msword", "khachsan_" + user.Username + "_" + DateTime.Now + ".docx");
        //}
    }
}