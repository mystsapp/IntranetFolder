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
    public class DanhGiaCruiseController : BaseController
    {
        private readonly IDanhGiaCruiseService _danhGiaCruiseService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public DanhGiaCruiseViewModel DanhGiaCruiseVM { get; set; }

        public DanhGiaCruiseController(IDanhGiaCruiseService danhGiaCruiseService, IWebHostEnvironment webHostEnvironment)
        {
            _danhGiaCruiseService = danhGiaCruiseService;
            _webHostEnvironment = webHostEnvironment;
            DanhGiaCruiseVM = new DanhGiaCruiseViewModel()
            {
                DanhGiaCruiseDTO = new DanhGiaCruiseDTO(),
                StrUrl = ""
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Cruise_Partial(string supplierId)
        {
            SupplierDTO supplierDTO = await _danhGiaCruiseService.GetBySupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaCruiseVM.SupplierDTO = supplierDTO;
            DanhGiaCruiseVM.DanhGiaCruiseDTOs = await _danhGiaCruiseService.GetDanhGiaCruiseBy_SupplierId(supplierId);

            return PartialView(DanhGiaCruiseVM);
        }

        public async Task<IActionResult> ThemMoiCruise_Partial(string supplierId) // code
        {
            DanhGiaCruiseVM.SupplierDTO = await _danhGiaCruiseService.GetSupplierByIdAsync(supplierId);
            DanhGiaCruiseVM.DanhGiaCruiseDTO.SupplierId = supplierId;
            DanhGiaCruiseVM.DanhGiaCruiseDTO.TenNcu = DanhGiaCruiseVM.SupplierDTO.Tengiaodich;
            DanhGiaCruiseVM.LoaiSaos = SD.LoaiSao();
            DanhGiaCruiseVM.GiaCas = SD.GiaCa();
            return PartialView(DanhGiaCruiseVM);
        }

        [HttpPost, ActionName("ThemMoiCruise_Partial")]
        public async Task<IActionResult> ThemMoiCruise_Partial_Post(string strUrl, DanhGiaCruiseViewModel model, string tieuChuanSao)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaCruiseVM = new DanhGiaCruiseViewModel()
                {
                    DanhGiaCruiseDTO = new DanhGiaCruiseDTO(),
                    StrUrl = strUrl
                };

                return View(DanhGiaCruiseVM);
            }

            //DanhGiaCruiseVM.DanhGiaCruiseDTO.TenNcu = DanhGiaCruiseVM.TenCreate;
            DanhGiaCruiseVM.DanhGiaCruiseDTO.NguoiTao = user.Username;
            DanhGiaCruiseVM.DanhGiaCruiseDTO.NgayTao = DateTime.Now;
            DanhGiaCruiseVM.DanhGiaCruiseDTO.LoaiDvid = 4; // MaLoai = CRU

            try
            {
                await _danhGiaCruiseService.CreateAsync(DanhGiaCruiseVM.DanhGiaCruiseDTO); // save

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
                await _danhGiaCruiseService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatCruise_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaCruiseVM.DanhGiaCruiseDTO = await _danhGiaCruiseService.GetByIdAsync(id);
            DanhGiaCruiseVM.SupplierDTO = await _danhGiaCruiseService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaCruiseVM.DanhGiaCruiseDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaCruiseVM.LoaiSaos = SD.LoaiSao();
            DanhGiaCruiseVM.GiaCas = SD.GiaCa();

            return PartialView(DanhGiaCruiseVM);
        }

        [HttpPost, ActionName("CapNhatCruise_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatCruise_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaCruiseVM.DanhGiaCruiseDTO.NgaySua = DateTime.Now;
                DanhGiaCruiseVM.DanhGiaCruiseDTO.NguoiSua = user.Username;

                #region log file

                string temp = "", log = "";

                var t = _danhGiaCruiseService.GetByIdAsNoTracking(DanhGiaCruiseVM.DanhGiaCruiseDTO.Id);

                if (t.TieuChuanSao != DanhGiaCruiseVM.DanhGiaCruiseDTO.TieuChuanSao)
                {
                    temp += String.Format("- TieuChuanSao thay đổi: {0}->{1}", t.TieuChuanSao, DanhGiaCruiseVM.DanhGiaCruiseDTO.TieuChuanSao);
                }

                if (t.SoLuongTauNguDem != DanhGiaCruiseVM.DanhGiaCruiseDTO.SoLuongTauNguDem)
                {
                    temp += String.Format("- SoLuongTauNguDem thay đổi: {0}->{1}", t.SoLuongTauNguDem, DanhGiaCruiseVM.DanhGiaCruiseDTO.SoLuongTauNguDem);
                }

                if (t.ChuongTrinh != DanhGiaCruiseVM.DanhGiaCruiseDTO.ChuongTrinh)
                {
                    temp += String.Format("- ChuongTrinh thay đổi: {0}->{1}", t.ChuongTrinh, DanhGiaCruiseVM.DanhGiaCruiseDTO.ChuongTrinh);
                }

                if (t.SoLuongTauTqngay != DanhGiaCruiseVM.DanhGiaCruiseDTO.SoLuongTauTqngay)
                {
                    temp += String.Format("- SoLuongTauTqngay thay đổi: {0}->{1}", t.SoLuongTauTqngay, DanhGiaCruiseVM.DanhGiaCruiseDTO.SoLuongTauTqngay);
                }

                if (t.GiaCa != DanhGiaCruiseVM.DanhGiaCruiseDTO.GiaCa)
                {
                    temp += String.Format("- GiaCa thay đổi: {0}->{1}", t.GiaCa, DanhGiaCruiseVM.DanhGiaCruiseDTO.GiaCa);
                }

                if (t.CangDonKhach != DanhGiaCruiseVM.DanhGiaCruiseDTO.CangDonKhach)
                {
                    temp += String.Format("- CangDonKhach thay đổi: {0}->{1}", t.CangDonKhach, DanhGiaCruiseVM.DanhGiaCruiseDTO.CangDonKhach);
                }

                if (t.CoHoTroTot != DanhGiaCruiseVM.DanhGiaCruiseDTO.CoHoTroTot)
                {
                    temp += String.Format("- CoHoTroTot thay đổi: {0}->{1}", t.CoHoTroTot, DanhGiaCruiseVM.DanhGiaCruiseDTO.CoHoTroTot);
                }

                if (t.Gpkd != DanhGiaCruiseVM.DanhGiaCruiseDTO.Gpkd)
                {
                    temp += String.Format("- Gpkd thay đổi: {0}->{1}", t.Gpkd, DanhGiaCruiseVM.DanhGiaCruiseDTO.Gpkd);
                }

                if (t.Vat != DanhGiaCruiseVM.DanhGiaCruiseDTO.Vat)
                {
                    temp += String.Format("- Vat thay đổi: {0}->{1}", t.Email, DanhGiaCruiseVM.DanhGiaCruiseDTO.Vat);
                }

                if (t.CabineCoBanCong != DanhGiaCruiseVM.DanhGiaCruiseDTO.CabineCoBanCong)
                {
                    temp += String.Format("- CabineCoBanCong thay đổi: {0}->{1}", t.CabineCoBanCong, DanhGiaCruiseVM.DanhGiaCruiseDTO.CabineCoBanCong);
                }

                if (t.KhaoSatThucTe != DanhGiaCruiseVM.DanhGiaCruiseDTO.KhaoSatThucTe)
                {
                    temp += String.Format("- KhaoSatThucTe thay đổi: {0}->{1}", t.KhaoSatThucTe, DanhGiaCruiseVM.DanhGiaCruiseDTO.KhaoSatThucTe);
                }

                if (t.KqDat != DanhGiaCruiseVM.DanhGiaCruiseDTO.KqDat)
                {
                    temp += String.Format("- KqDat thay đổi: {0}->{1}", t.KqDat, DanhGiaCruiseVM.DanhGiaCruiseDTO.KqDat);
                }

                if (t.KqKhaoSatThem != DanhGiaCruiseVM.DanhGiaCruiseDTO.KqKhaoSatThem)
                {
                    temp += String.Format("- KqKhaoSatThem thay đổi: {0}->{1}", t.KqKhaoSatThem, DanhGiaCruiseVM.DanhGiaCruiseDTO.KqKhaoSatThem);
                }

                if (t.TiemNang != DanhGiaCruiseVM.DanhGiaCruiseDTO.TiemNang)
                {
                    temp += String.Format("- TiemNang thay đổi: {0}->{1}", t.TiemNang, DanhGiaCruiseVM.DanhGiaCruiseDTO.TiemNang);
                }

                if (t.TaiKy != DanhGiaCruiseVM.DanhGiaCruiseDTO.TaiKy)
                {
                    temp += String.Format("- TaiKy thay đổi: {0}->{1}", t.TaiKy, DanhGiaCruiseVM.DanhGiaCruiseDTO.TaiKy);
                }

                if (t.NguoiDanhGia != DanhGiaCruiseVM.DanhGiaCruiseDTO.NguoiDanhGia)
                {
                    temp += String.Format("- NguoiDanhGia thay đổi: {0}->{1}", t.NguoiDanhGia, DanhGiaCruiseVM.DanhGiaCruiseDTO.NguoiDanhGia);
                }

                if (t.NgayDanhGia != DanhGiaCruiseVM.DanhGiaCruiseDTO.NgayDanhGia)
                {
                    temp += String.Format("- NgayDanhGia thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", t.NgayDanhGia, DanhGiaCruiseVM.DanhGiaCruiseDTO.NgayDanhGia);
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
                    DanhGiaCruiseVM.DanhGiaCruiseDTO.LogFile = t.LogFile;
                }

                try
                {
                    await _danhGiaCruiseService.UpdateAsync(DanhGiaCruiseVM.DanhGiaCruiseDTO);

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
                    await _danhGiaCruiseService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Cập nhật không thành công!"
                    });
                }
            }
            // not valid
            //DanhGiaCruiseVM.LoaiDvDTOs = _danhGiaCruiseService.GetAllLoaiDv();

            return View(DanhGiaCruiseVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaCruiseVM.StrUrl = strUrl;

            var DanhGiaCruiseDTO = _danhGiaCruiseService.GetByIdAsNoTracking(id);
            if (DanhGiaCruiseDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaCruiseService.Delete(DanhGiaCruiseDTO);

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
                await _danhGiaCruiseService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }

        public async Task<IActionResult> ExportToWord_Cruise(string supplierId, long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var supplierDTO = await _danhGiaCruiseService.GetSupplierByIdAsync(supplierId);
            if (string.IsNullOrEmpty(supplierId) || supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            TapDoanDTO tapDoanDTO = await _danhGiaCruiseService.GetTapDoanByIdAsync(supplierDTO.TapDoanId);
            var DanhGiaCruiseDTO = await _danhGiaCruiseService.GetByIdAsync(id);

            if (DanhGiaCruiseDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var loaiDvDTO = _danhGiaCruiseService.GetAllLoaiDv().Where(x => x.Id == DanhGiaCruiseDTO.LoaiDvid).FirstOrDefault();

            DocX doc = null;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = webRootPath + @"\WordTemplates\M01h-DGNCU-CRUISES.docx";
            doc = DocX.Load(fileName);

            doc.AddCustomProperty(new CustomProperty("TenGiaoDich", supplierDTO.Code + " - " + supplierDTO.Tengiaodich));
            doc.AddCustomProperty(new CustomProperty("TenThuongMai", supplierDTO.Tenthuongmai));
            doc.AddCustomProperty(new CustomProperty("TapDoan", tapDoanDTO == null ? "" : tapDoanDTO.Ten));
            doc.AddCustomProperty(new CustomProperty("DiaChi", supplierDTO.Diachi));
            doc.AddCustomProperty(new CustomProperty("DienThoai/Email", supplierDTO.Dienthoai + "/" + supplierDTO.Email));
            doc.AddCustomProperty(new CustomProperty("LoaiHinhDichVu", loaiDvDTO.TenLoai));

            doc.AddCustomProperty(new CustomProperty("TieuChuanSao", DanhGiaCruiseDTO.TieuChuanSao));
            doc.AddCustomProperty(new CustomProperty("SoLuongTauNguDem", DanhGiaCruiseDTO.SoLuongTauNguDem));
            doc.AddCustomProperty(new CustomProperty("ChuongTrinh", DanhGiaCruiseDTO.ChuongTrinh));
            doc.AddCustomProperty(new CustomProperty("SoLuongTauThamQuanNgay", DanhGiaCruiseDTO.SoLuongTauTqngay));
            doc.AddCustomProperty(new CustomProperty("GiaCa", DanhGiaCruiseDTO.GiaCa));
            doc.AddCustomProperty(new CustomProperty("CangDonKhach", DanhGiaCruiseDTO.CangDonKhach));
            doc.AddCustomProperty(new CustomProperty("CoHoTroTot", DanhGiaCruiseDTO.CoHoTroTot ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("GiayPhepKinhDoanh", DanhGiaCruiseDTO.Gpkd ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("VAT", DanhGiaCruiseDTO.Vat ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("CabineCoBanCong", DanhGiaCruiseDTO.CabineCoBanCong ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThucTe", DanhGiaCruiseDTO.KhaoSatThucTe ? "Có" : "Không"));

            doc.AddCustomProperty(new CustomProperty("DatYeuCau", DanhGiaCruiseDTO.KqDat == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThem", DanhGiaCruiseDTO.KqKhaoSatThem ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TaiKy", DanhGiaCruiseDTO.TaiKy ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TiemNang", DanhGiaCruiseDTO.TiemNang ? "Có" : ""));

            doc.AddCustomProperty(new CustomProperty("Ngay", DateTime.Now.Day));
            doc.AddCustomProperty(new CustomProperty("Thang", DateTime.Now.Month));
            doc.AddCustomProperty(new CustomProperty("Nam", DateTime.Now.Year));

            doc.AddList("First Item", 0, ListItemType.Numbered);

            MemoryStream stream = new MemoryStream();

            // Saves the Word document to MemoryStream
            doc.SaveAs(stream);
            stream.Position = 0;
            // Download Word document in the browser
            return File(stream, "application/msword", "cruise_" + user.Username + "_" + DateTime.Now + ".docx");
        }
    }
}