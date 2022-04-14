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
    public class DanhGiaVanChuyenController : BaseController
    {
        private readonly IDanhGiaVanChuyenService _danhGiaVanChuyenService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public DanhGiaVanChuyenViewModel DanhGiaVanChuyenVM { get; set; }

        public DanhGiaVanChuyenController(IDanhGiaVanChuyenService danhGiaVanChuyenService, IWebHostEnvironment webHostEnvironment)
        {
            DanhGiaVanChuyenVM = new DanhGiaVanChuyenViewModel()
            {
                DanhGiaVanChuyenDTO = new DanhGiaVanChuyenDTO(),
                StrUrl = ""
            };
            _danhGiaVanChuyenService = danhGiaVanChuyenService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> VanChuyen_Partial(string supplierId)
        {
            SupplierDTO supplierDTO = await _danhGiaVanChuyenService.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaVanChuyenVM.SupplierDTO = supplierDTO;
            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTOs = await _danhGiaVanChuyenService.GetDanhGiaVanChuyenBy_SupplierId(supplierId);

            return PartialView(DanhGiaVanChuyenVM);
        }

        public async Task<IActionResult> ThemMoiVanChuyen_Partial(string supplierId) // code
        {
            DanhGiaVanChuyenVM.SupplierDTO = await _danhGiaVanChuyenService.GetSupplierByIdAsync(supplierId);
            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.TenNcu = DanhGiaVanChuyenVM.SupplierDTO.Tengiaodich;
            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.SupplierId = supplierId;
            DanhGiaVanChuyenVM.KhaNangHuyDongs = SD.KhaNangHuyDong();
            return PartialView(DanhGiaVanChuyenVM);
        }

        [HttpPost, ActionName("ThemMoiVanChuyen_Partial")]
        public async Task<IActionResult> ThemMoiVanChuyen_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaVanChuyenVM = new DanhGiaVanChuyenViewModel()
                {
                    DanhGiaVanChuyenDTO = new DanhGiaVanChuyenDTO(),
                    StrUrl = strUrl
                };

                return Json(new
                {
                    status = false,
                    message = "Model state is not valid"
                });
            }

            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.NguoiTao = user.Username;
            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.NgayTao = DateTime.Now;
            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.LoaiDvid = 7; // MaLoai = CAR

            try
            {
                await _danhGiaVanChuyenService.CreateAsync(DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO); // save

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
                await _danhGiaVanChuyenService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatVanChuyen_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO = await _danhGiaVanChuyenService.GetByIdAsync(id);
            DanhGiaVanChuyenVM.SupplierDTO = await _danhGiaVanChuyenService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaVanChuyenVM.KhaNangHuyDongs = SD.KhaNangHuyDong();

            return PartialView(DanhGiaVanChuyenVM);
        }

        [HttpPost, ActionName("CapNhatVanChuyen_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatVanChuyen_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.NgaySua = DateTime.Now;
                DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO.NguoiSua = user.Username;

                try
                {
                    await _danhGiaVanChuyenService.UpdateAsync(DanhGiaVanChuyenVM.DanhGiaVanChuyenDTO);

                    return Json(new { status = true });
                }
                catch (Exception ex)
                {
                    ErrorLog errorLog = new ErrorLog()
                    {
                        //InnerMessage = ex.InnerException.Message,
                        Message = ex.Message,
                        NgayTao = DateTime.Now,
                        NguoiTao = user.Nguoitao
                    };
                    await _danhGiaVanChuyenService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Thêm mới không thành công!"
                    });
                }
            }
            // not valid

            return View(DanhGiaVanChuyenVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaVanChuyenVM.StrUrl = strUrl;

            var DanhGiaVanChuyenDTO = _danhGiaVanChuyenService.GetByIdAsNoTracking(id);
            if (DanhGiaVanChuyenDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaVanChuyenService.Delete(DanhGiaVanChuyenDTO);

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
                await _danhGiaVanChuyenService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }

        public async Task<IActionResult> ExportToWord_VanChuyen(string supplierId, long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var supplierDTO = await _danhGiaVanChuyenService.GetSupplierByIdAsync(supplierId);
            if (string.IsNullOrEmpty(supplierId) || supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            TapDoanDTO tapDoanDTO = await _danhGiaVanChuyenService.GetTapDoanByIdAsync(supplierDTO.TapDoanId);
            var DanhGiaVanChuyenDTO = await _danhGiaVanChuyenService.GetByIdAsync(id);

            if (DanhGiaVanChuyenDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var loaiDvDTO = _danhGiaVanChuyenService.GetAllLoaiDv().Where(x => x.Id == DanhGiaVanChuyenDTO.LoaiDvid).FirstOrDefault();

            DocX doc = null;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = webRootPath + @"\WordTemplates\M01f-DGNCU-VAN CHUYEN.docx";
            doc = DocX.Load(fileName);

            doc.AddCustomProperty(new CustomProperty("TenGiaoDich", supplierDTO.Code + " - " + supplierDTO.Tengiaodich));
            doc.AddCustomProperty(new CustomProperty("TenThuongMai", supplierDTO.Tenthuongmai));
            doc.AddCustomProperty(new CustomProperty("TapDoan", tapDoanDTO == null ? "" : tapDoanDTO.Ten));
            doc.AddCustomProperty(new CustomProperty("DiaChi", supplierDTO.Diachi));
            doc.AddCustomProperty(new CustomProperty("DienThoai/Email", supplierDTO.Dienthoai + "/" + supplierDTO.Email));
            doc.AddCustomProperty(new CustomProperty("LoaiHinhDV", loaiDvDTO.TenLoai));

            //doc.AddCustomProperty(new CustomProperty("HopTacLanDau", DanhGiaVanChuyenDTO.LanDauOrTaiKy == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("GiayPhepKinhDoanh", DanhGiaVanChuyenDTO.Gpkd == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("VAT", DanhGiaVanChuyenDTO.Vat == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("GiaCaPhuHop", DanhGiaVanChuyenDTO.GiaCaPhuHop ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThucTe", DanhGiaVanChuyenDTO.KhaoSatThucTe ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("SoXeChinhThuc", DanhGiaVanChuyenDTO.SoXeChinhThuc));
            doc.AddCustomProperty(new CustomProperty("KhaNangHuyDong", DanhGiaVanChuyenDTO.KhaNangHuyDong));
            doc.AddCustomProperty(new CustomProperty("DoiXeCuNhat/MoiNhat", DanhGiaVanChuyenDTO.DoiXeCuNhatMoiNhat)); // ?
            doc.AddCustomProperty(new CustomProperty("LoaiXeCoNhieuNhat", DanhGiaVanChuyenDTO.LoaiXeCoNhieuNhat));
            doc.AddCustomProperty(new CustomProperty("KinhNghiem", DanhGiaVanChuyenDTO.KinhNghiem));
            doc.AddCustomProperty(new CustomProperty("DanhSachDoiTac", DanhGiaVanChuyenDTO.DanhSachDoiTac));

            doc.AddCustomProperty(new CustomProperty("DatYeuCau", DanhGiaVanChuyenDTO.KqDat == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThem", DanhGiaVanChuyenDTO.KqKhaoSatThem ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TaiKy", DanhGiaVanChuyenDTO.TaiKy ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TiemNang", DanhGiaVanChuyenDTO.TiemNang ? "Có" : ""));

            doc.AddCustomProperty(new CustomProperty("Ngay", DateTime.Now.Day));
            doc.AddCustomProperty(new CustomProperty("Thang", DateTime.Now.Month));
            doc.AddCustomProperty(new CustomProperty("Nam", DateTime.Now.Year));

            doc.AddList("First Item", 0, ListItemType.Numbered);

            MemoryStream stream = new MemoryStream();

            // Saves the Word document to MemoryStream
            doc.SaveAs(stream);
            stream.Position = 0;
            // Download Word document in the browser
            return File(stream, "application/msword", "golf_" + user.Username + "_" + DateTime.Now + ".docx");
        }
    }
}