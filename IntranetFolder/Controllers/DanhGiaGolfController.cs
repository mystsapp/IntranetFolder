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
            DanhGiaGolfVM.LoaiSaos = SD.LoaiSao();
            DanhGiaGolfVM.MucGiaPhis = SD.MucGiaPhi();
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

            // ghi log
            DanhGiaGolfVM.DanhGiaGolfDTO.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

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
            DanhGiaGolfVM.LoaiSaos = SD.LoaiSao();
            DanhGiaGolfVM.MucGiaPhis = SD.MucGiaPhi();

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

                #region log file

                string temp = "", log = "";

                var t = _danhGiaGolfService.GetByIdAsNoTracking(DanhGiaGolfVM.DanhGiaGolfDTO.Id);

                if (t.Gpkd != DanhGiaGolfVM.DanhGiaGolfDTO.Gpkd)
                {
                    temp += String.Format("- Gpkd thay đổi: {0}->{1}", t.Gpkd, DanhGiaGolfVM.DanhGiaGolfDTO.Gpkd);
                }

                if (t.Vat != DanhGiaGolfVM.DanhGiaGolfDTO.Vat)
                {
                    temp += String.Format("- Vat thay đổi: {0}->{1}", t.Vat, DanhGiaGolfVM.DanhGiaGolfDTO.Vat);
                }

                if (t.CoNhaHang != DanhGiaGolfVM.DanhGiaGolfDTO.CoNhaHang)
                {
                    temp += String.Format("- CoNhaHang thay đổi: {0}->{1}", t.CoNhaHang, DanhGiaGolfVM.DanhGiaGolfDTO.CoNhaHang);
                }

                if (t.CoXeDien != DanhGiaGolfVM.DanhGiaGolfDTO.CoXeDien)
                {
                    temp += String.Format("- CoXeDien thay đổi: {0}->{1}", t.CoXeDien, DanhGiaGolfVM.DanhGiaGolfDTO.CoXeDien);
                }

                if (t.CoHoTroTot != DanhGiaGolfVM.DanhGiaGolfDTO.CoHoTroTot)
                {
                    temp += String.Format("- CoHoTroTot thay đổi: {0}->{1}", t.CoHoTroTot, DanhGiaGolfVM.DanhGiaGolfDTO.CoHoTroTot);
                }

                if (t.KhaoSatThucTe != DanhGiaGolfVM.DanhGiaGolfDTO.KhaoSatThucTe)
                {
                    temp += String.Format("- KhaoSatThucTe thay đổi: {0}->{1}", t.KhaoSatThucTe, DanhGiaGolfVM.DanhGiaGolfDTO.KhaoSatThucTe);
                }

                if (t.TieuChuanSao != DanhGiaGolfVM.DanhGiaGolfDTO.TieuChuanSao)
                {
                    temp += String.Format("- TieuChuanSao thay đổi: {0}->{1}", t.TieuChuanSao, DanhGiaGolfVM.DanhGiaGolfDTO.TieuChuanSao);
                }

                if (t.ViTri != DanhGiaGolfVM.DanhGiaGolfDTO.ViTri)
                {
                    temp += String.Format("- ViTri thay đổi: {0}->{1}", t.ViTri, DanhGiaGolfVM.DanhGiaGolfDTO.ViTri);
                }

                if (t.SoLuongSanGolf != DanhGiaGolfVM.DanhGiaGolfDTO.SoLuongSanGolf)
                {
                    temp += String.Format("- SoLuongSanGolf thay đổi: {0}->{1}", t.SoLuongSanGolf, DanhGiaGolfVM.DanhGiaGolfDTO.SoLuongSanGolf);
                }

                if (t.DienTichSanGolf != DanhGiaGolfVM.DanhGiaGolfDTO.DienTichSanGolf)
                {
                    temp += String.Format("- DienTichSanGolf thay đổi: {0}->{1}", t.DienTichSanGolf, DanhGiaGolfVM.DanhGiaGolfDTO.DienTichSanGolf);
                }

                if (t.MucGiaPhi != DanhGiaGolfVM.DanhGiaGolfDTO.MucGiaPhi)
                {
                    temp += String.Format("- MucGiaPhi thay đổi: {0}->{1}", t.MucGiaPhi, DanhGiaGolfVM.DanhGiaGolfDTO.MucGiaPhi);
                }

                if (t.KqDat != DanhGiaGolfVM.DanhGiaGolfDTO.KqDat)
                {
                    temp += String.Format("- KqDat thay đổi: {0}->{1}", t.KqDat, DanhGiaGolfVM.DanhGiaGolfDTO.KqDat);
                }

                if (t.KqKhaoSatThem != DanhGiaGolfVM.DanhGiaGolfDTO.KqKhaoSatThem)
                {
                    temp += String.Format("- KqKhaoSatThem thay đổi: {0}->{1}", t.KqKhaoSatThem, DanhGiaGolfVM.DanhGiaGolfDTO.KqKhaoSatThem);
                }

                if (t.TiemNang != DanhGiaGolfVM.DanhGiaGolfDTO.TiemNang)
                {
                    temp += String.Format("- TiemNang thay đổi: {0}->{1}", t.TiemNang, DanhGiaGolfVM.DanhGiaGolfDTO.TiemNang);
                }

                if (t.TaiKy != DanhGiaGolfVM.DanhGiaGolfDTO.TaiKy)
                {
                    temp += String.Format("- TaiKy thay đổi: {0}->{1}", t.TaiKy, DanhGiaGolfVM.DanhGiaGolfDTO.TaiKy);
                }

                if (t.NguoiDanhGia != DanhGiaGolfVM.DanhGiaGolfDTO.NguoiDanhGia)
                {
                    temp += String.Format("- NguoiDanhGia thay đổi: {0}->{1}", t.NguoiDanhGia, DanhGiaGolfVM.DanhGiaGolfDTO.NguoiDanhGia);
                }

                if (t.NgayDanhGia != DanhGiaGolfVM.DanhGiaGolfDTO.NgayDanhGia)
                {
                    temp += String.Format("- NgayDanhGia thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", t.NgayDanhGia, DanhGiaGolfVM.DanhGiaGolfDTO.NgayDanhGia);
                }

                #endregion log file

                // kiem tra thay doi
                if (temp.Length > 0)
                {
                    log = System.Environment.NewLine;
                    log += "=============";
                    log += System.Environment.NewLine;
                    log += temp + " -User cập nhật tour: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // username
                    t.LogFile = t.LogFile + log;
                    DanhGiaGolfVM.DanhGiaGolfDTO.LogFile = t.LogFile;
                }

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
                        message = "Cập nhật không thành công!"
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

        public async Task<IActionResult> ExportToWord_Golf(string supplierId, long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var supplierDTO = await _danhGiaGolfService.GetSupplierByIdAsync(supplierId);
            if (string.IsNullOrEmpty(supplierId) || supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            TapDoanDTO tapDoanDTO = await _danhGiaGolfService.GetTapDoanByIdAsync(supplierDTO.TapDoanId);
            var DanhGiaGolfDTO = await _danhGiaGolfService.GetByIdAsync(id);

            if (DanhGiaGolfDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var loaiDvDTO = _danhGiaGolfService.GetAllLoaiDv().Where(x => x.Id == DanhGiaGolfDTO.LoaiDvid).FirstOrDefault();

            DocX doc = null;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = webRootPath + @"\WordTemplates\M01g-DGNCU-GOLF.docx";
            doc = DocX.Load(fileName);

            doc.AddCustomProperty(new CustomProperty("TenGiaoDich", supplierDTO.Code + " - " + supplierDTO.Tengiaodich));
            doc.AddCustomProperty(new CustomProperty("TenThuongMai", supplierDTO.Tenthuongmai));
            doc.AddCustomProperty(new CustomProperty("TapDoan", tapDoanDTO == null ? "" : tapDoanDTO.Ten));
            doc.AddCustomProperty(new CustomProperty("DiaChi", supplierDTO.Diachi));
            doc.AddCustomProperty(new CustomProperty("DienThoai/Email", supplierDTO.Dienthoai + "/" + supplierDTO.Email));
            doc.AddCustomProperty(new CustomProperty("LoaiHinhDichVu", loaiDvDTO.TenLoai));

            doc.AddCustomProperty(new CustomProperty("GiayPhepKinhDoanh", DanhGiaGolfDTO.Gpkd == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("VAT", DanhGiaGolfDTO.Vat == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("CoNhaHang", DanhGiaGolfDTO.CoNhaHang ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("CoXeDien", DanhGiaGolfDTO.CoXeDien ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("CoHoTroTot", DanhGiaGolfDTO.CoHoTroTot ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThucTe", DanhGiaGolfDTO.KhaoSatThucTe ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("TieuChuanSao", DanhGiaGolfDTO.TieuChuanSao));
            doc.AddCustomProperty(new CustomProperty("ViTri", DanhGiaGolfDTO.ViTri));
            doc.AddCustomProperty(new CustomProperty("SoLuongSanGolf", DanhGiaGolfDTO.SoLuongSanGolf.Value.ToString()));
            doc.AddCustomProperty(new CustomProperty("DienTichSanGolf", DanhGiaGolfDTO.DienTichSanGolf)); // ?
            doc.AddCustomProperty(new CustomProperty("MucGiaPhi", DanhGiaGolfDTO.MucGiaPhi));
            doc.AddCustomProperty(new CustomProperty("DatYeuCau", DanhGiaGolfDTO.KqDat == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThem", DanhGiaGolfDTO.KqKhaoSatThem ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TaiKy", DanhGiaGolfDTO.TaiKy ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TiemNang", DanhGiaGolfDTO.TiemNang ? "Có" : ""));

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