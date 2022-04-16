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
    public class DanhGiaCamLaoController : BaseController
    {
        private readonly IDanhGiaCamLaoService _danhGiaCamLaoService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public DanhGiaCamLaoViewModel DanhGiaCamLaoVM { get; set; }

        public DanhGiaCamLaoController(IDanhGiaCamLaoService danhGiaCamLaoService, IWebHostEnvironment webHostEnvironment)
        {
            DanhGiaCamLaoVM = new DanhGiaCamLaoViewModel()
            {
                DanhGiaCamLaoDTO = new DanhGiaCamLaoDTO(),
                StrUrl = ""
            };
            _danhGiaCamLaoService = danhGiaCamLaoService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CamLao_Partial(string supplierId)
        {
            SupplierDTO supplierDTO = await _danhGiaCamLaoService.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaCamLaoVM.SupplierDTO = supplierDTO;
            DanhGiaCamLaoVM.DanhGiaCamLaoDTOs = await _danhGiaCamLaoService.GetDanhGiaCamLaoBy_SupplierId(supplierId);

            return PartialView(DanhGiaCamLaoVM);
        }

        public async Task<IActionResult> ThemMoiCamLao_Partial(string supplierId) // code
        {
            DanhGiaCamLaoVM.SupplierDTO = await _danhGiaCamLaoService.GetSupplierByIdAsync(supplierId);
            DanhGiaCamLaoVM.DanhGiaCamLaoDTO.SupplierId = supplierId;
            DanhGiaCamLaoVM.DanhGiaCamLaoDTO.TenNcu = DanhGiaCamLaoVM.SupplierDTO.Tengiaodich;
            DanhGiaCamLaoVM.ChatLuongDVs = SD.ChatLuongDV();
            DanhGiaCamLaoVM.SanPhams = SD.SanPham();
            DanhGiaCamLaoVM.GiaCas = SD.GiaCa();
            return PartialView(DanhGiaCamLaoVM);
        }

        [HttpPost, ActionName("ThemMoiCamLao_Partial")]
        public async Task<IActionResult> ThemMoiCamLao_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaCamLaoVM = new DanhGiaCamLaoViewModel()
                {
                    DanhGiaCamLaoDTO = new DanhGiaCamLaoDTO(),
                    StrUrl = strUrl
                };

                return Json(new
                {
                    status = false,
                    message = "Model state is not valid"
                });
            }

            DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NguoiTao = user.Username;
            DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NgayTao = DateTime.Now;
            DanhGiaCamLaoVM.DanhGiaCamLaoDTO.LoaiDvid = 5; // MaLoai = SSE

            // ghi log
            DanhGiaCamLaoVM.DanhGiaCamLaoDTO.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _danhGiaCamLaoService.CreateAsync(DanhGiaCamLaoVM.DanhGiaCamLaoDTO); // save

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
                await _danhGiaCamLaoService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatCamLao_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaCamLaoVM.DanhGiaCamLaoDTO = await _danhGiaCamLaoService.GetByIdAsync(id);
            DanhGiaCamLaoVM.SupplierDTO = await _danhGiaCamLaoService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaCamLaoVM.DanhGiaCamLaoDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaCamLaoVM.ChatLuongDVs = SD.ChatLuongDV();
            DanhGiaCamLaoVM.SanPhams = SD.SanPham();
            DanhGiaCamLaoVM.GiaCas = SD.GiaCa();

            return PartialView(DanhGiaCamLaoVM);
        }

        [HttpPost, ActionName("CapNhatCamLao_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatCamLao_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NgaySua = DateTime.Now;
                DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NguoiSua = user.Username;

                #region log file

                string temp = "", log = "";

                var t = _danhGiaCamLaoService.GetByIdAsNoTracking(DanhGiaCamLaoVM.DanhGiaCamLaoDTO.Id);

                if (t.ThoiGianHoatDong != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.ThoiGianHoatDong)
                {
                    temp += String.Format("- ThoiGianHoatDong thay đổi: {0}->{1}", t.ThoiGianHoatDong, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.ThoiGianHoatDong);
                }

                if (t.CacDoiTacVn != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.CacDoiTacVn)
                {
                    temp += String.Format("- CacDoiTacVn thay đổi: {0}->{1}", t.CacDoiTacVn, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.CacDoiTacVn);
                }

                if (t.Tuyen != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.Tuyen)
                {
                    temp += String.Format("- Tuyen thay đổi: {0}->{1}", t.Tuyen, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.Tuyen);
                }

                if (t.CldvvaHdv != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.CldvvaHdv)
                {
                    temp += String.Format("- CldvvaHdv thay đổi: {0}->{1}", t.CldvvaHdv, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.CldvvaHdv);
                }

                if (t.SanPham != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.SanPham)
                {
                    temp += String.Format("- SanPham thay đổi: {0}->{1}", t.SanPham, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.SanPham);
                }

                if (t.GiaCa != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.GiaCa)
                {
                    temp += String.Format("- GiaCa thay đổi: {0}->{1}", t.GiaCa, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.GiaCa);
                }

                if (t.CoHtxuLySuCo != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.CoHtxuLySuCo)
                {
                    temp += String.Format("- CoHtxuLySuCo thay đổi: {0}->{1}", t.CoHtxuLySuCo, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.CoHtxuLySuCo);
                }

                if (t.KhaoSatThucTe != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.KhaoSatThucTe)
                {
                    temp += String.Format("- KhaoSatThucTe thay đổi: {0}->{1}", t.KhaoSatThucTe, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.KhaoSatThucTe);
                }

                if (t.Kqdat != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.Kqdat)
                {
                    temp += String.Format("- Kqdat thay đổi: {0}->{1}", t.Kqdat, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.Kqdat);
                }

                if (t.KqkhaoSatThem != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.KqkhaoSatThem)
                {
                    temp += String.Format("- KqkhaoSatThem thay đổi: {0}->{1}", t.KqkhaoSatThem, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.KqkhaoSatThem);
                }

                if (t.TiemNang != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.TiemNang)
                {
                    temp += String.Format("- TiemNang thay đổi: {0}->{1}", t.TiemNang, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.TiemNang);
                }

                if (t.TaiKy != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.TaiKy)
                {
                    temp += String.Format("- TaiKy thay đổi: {0}->{1}", t.TaiKy, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.TaiKy);
                }

                if (t.NguoiDanhGia != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NguoiDanhGia)
                {
                    temp += String.Format("- NguoiDanhGia thay đổi: {0}->{1}", t.NguoiDanhGia, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NguoiDanhGia);
                }

                if (t.NgayDanhGia != DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NgayDanhGia)
                {
                    temp += String.Format("- NgayDanhGia thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", t.NgayDanhGia, DanhGiaCamLaoVM.DanhGiaCamLaoDTO.NgayDanhGia);
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
                    DanhGiaCamLaoVM.DanhGiaCamLaoDTO.LogFile = t.LogFile;
                }

                try
                {
                    await _danhGiaCamLaoService.UpdateAsync(DanhGiaCamLaoVM.DanhGiaCamLaoDTO);

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
                    await _danhGiaCamLaoService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Cập nhật không thành công!"
                    });
                }
            }
            // not valid

            return View(DanhGiaCamLaoVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaCamLaoVM.StrUrl = strUrl;

            var DanhGiaCamLaoDTO = _danhGiaCamLaoService.GetByIdAsNoTracking(id);
            if (DanhGiaCamLaoDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaCamLaoService.Delete(DanhGiaCamLaoDTO);

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
                await _danhGiaCamLaoService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }

        public async Task<IActionResult> ExportToWord_LandTourNN(string supplierId, long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var supplierDTO = await _danhGiaCamLaoService.GetSupplierByIdAsync(supplierId);
            if (string.IsNullOrEmpty(supplierId) || supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            TapDoanDTO tapDoanDTO = await _danhGiaCamLaoService.GetTapDoanByIdAsync(supplierDTO.TapDoanId);
            var DanhGiaCamLaoDTO = await _danhGiaCamLaoService.GetByIdAsync(id);

            if (DanhGiaCamLaoDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var loaiDvDTO = _danhGiaCamLaoService.GetAllLoaiDv().Where(x => x.Id == DanhGiaCamLaoDTO.LoaiDvid).FirstOrDefault();

            DocX doc = null;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = webRootPath + @"\WordTemplates\M01d-DGNCU-LANDTOURNN.docx";
            doc = DocX.Load(fileName);

            doc.AddCustomProperty(new CustomProperty("TenGiaoDich", supplierDTO.Code + " - " + supplierDTO.Tengiaodich));
            doc.AddCustomProperty(new CustomProperty("TenThuongMai", supplierDTO.Tenthuongmai));
            doc.AddCustomProperty(new CustomProperty("TapDoan", tapDoanDTO == null ? "" : tapDoanDTO.Ten));
            doc.AddCustomProperty(new CustomProperty("DiaChi", supplierDTO.Diachi));
            doc.AddCustomProperty(new CustomProperty("DienThoai/Email", supplierDTO.Dienthoai + "/" + supplierDTO.Email));
            doc.AddCustomProperty(new CustomProperty("LoaiHinhDV", loaiDvDTO.TenLoai));

            doc.AddCustomProperty(new CustomProperty("ThoiGianHoatDong", DanhGiaCamLaoDTO.ThoiGianHoatDong));
            doc.AddCustomProperty(new CustomProperty("CacDoiTacVN", DanhGiaCamLaoDTO.CacDoiTacVn));
            doc.AddCustomProperty(new CustomProperty("Tuyen", DanhGiaCamLaoDTO.Tuyen));
            doc.AddCustomProperty(new CustomProperty("CLDVVaHDV", DanhGiaCamLaoDTO.CldvvaHdv));
            doc.AddCustomProperty(new CustomProperty("SanPham", DanhGiaCamLaoDTO.SanPham));
            doc.AddCustomProperty(new CustomProperty("GiaCa", DanhGiaCamLaoDTO.GiaCa));
            doc.AddCustomProperty(new CustomProperty("CoHTXuLySuCo", DanhGiaCamLaoDTO.CoHtxuLySuCo ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThucTe", DanhGiaCamLaoDTO.KhaoSatThucTe ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("DatYeuCau", DanhGiaCamLaoDTO.Kqdat == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThem", DanhGiaCamLaoDTO.KqkhaoSatThem == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TaiKy", DanhGiaCamLaoDTO.TaiKy == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TiemNang", DanhGiaCamLaoDTO.TiemNang == true ? "Có" : ""));

            doc.AddCustomProperty(new CustomProperty("Ngay", DateTime.Now.Day));
            doc.AddCustomProperty(new CustomProperty("Thang", DateTime.Now.Month));
            doc.AddCustomProperty(new CustomProperty("Nam", DateTime.Now.Year));

            doc.AddList("First Item", 0, ListItemType.Numbered);

            MemoryStream stream = new MemoryStream();

            // Saves the Word document to MemoryStream
            doc.SaveAs(stream);
            stream.Position = 0;
            // Download Word document in the browser
            return File(stream, "application/msword", "LandTourNN_" + user.Username + "_" + DateTime.Now + ".docx");
        }
    }
}