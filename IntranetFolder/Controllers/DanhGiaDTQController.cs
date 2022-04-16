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
    public class DanhGiaDTQController : BaseController
    {
        private readonly IDanhGiaDTQService _danhGiaDTQService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public DanhGiaDTQViewModel DanhGiaDTQVM { get; set; }

        public DanhGiaDTQController(IDanhGiaDTQService danhGiaDTQService, IWebHostEnvironment webHostEnvironment)
        {
            DanhGiaDTQVM = new DanhGiaDTQViewModel()
            {
                DanhGiaDTQDTO = new DanhGiaDTQDTO(),
                StrUrl = ""
            };
            _danhGiaDTQService = danhGiaDTQService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DiemThamQuan_Partial(string supplierId)
        {
            SupplierDTO supplierDTO = await _danhGiaDTQService.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaDTQVM.SupplierDTO = supplierDTO;
            DanhGiaDTQVM.DanhGiaDTQDTOs = await _danhGiaDTQService.GetDanhGiaDTQBy_SupplierId(supplierId);

            return PartialView(DanhGiaDTQVM);
        }

        public async Task<IActionResult> ThemMoiDTQ_Partial(string supplierId) // code
        {
            DanhGiaDTQVM.SupplierDTO = await _danhGiaDTQService.GetSupplierByIdAsync(supplierId);
            DanhGiaDTQVM.DanhGiaDTQDTO.SupplierId = supplierId;
            DanhGiaDTQVM.DanhGiaDTQDTO.TenNcu = DanhGiaDTQVM.SupplierDTO.Tengiaodich;
            DanhGiaDTQVM.MucDoHapDans = SD.MucDoHapDan();
            DanhGiaDTQVM.DoiTuongKhachPhuHops = SD.DoiTuongKhachPhuHop();
            DanhGiaDTQVM.NhaVeSinhs = SD.NhaVeSinh();

            return PartialView(DanhGiaDTQVM);
        }

        [HttpPost, ActionName("ThemMoiDTQ_Partial")]
        public async Task<IActionResult> ThemMoiDTQ_Partial_Post(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                DanhGiaDTQVM = new DanhGiaDTQViewModel()
                {
                    DanhGiaDTQDTO = new DanhGiaDTQDTO(),
                    StrUrl = strUrl
                };

                return Json(new
                {
                    status = false,
                    message = "Model state is not valid"
                });
            }

            DanhGiaDTQVM.DanhGiaDTQDTO.NguoiTao = user.Username;
            DanhGiaDTQVM.DanhGiaDTQDTO.NgayTao = DateTime.Now;
            DanhGiaDTQVM.DanhGiaDTQDTO.LoaiDvid = 5; // MaLoai = SSE

            // ghi log
            DanhGiaDTQVM.DanhGiaDTQDTO.LogFile = "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // user.Username

            try
            {
                await _danhGiaDTQService.CreateAsync(DanhGiaDTQVM.DanhGiaDTQDTO); // save

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
                await _danhGiaDTQService.CreateErroLogAsync(errorLog);
                return Json(new
                {
                    status = false,
                    message = "Thêm mới không thành công!"
                });
            }
        }

        public async Task<IActionResult> CapNhatDTQ_Partial(string supplierId, long id)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            DanhGiaDTQVM.DanhGiaDTQDTO = await _danhGiaDTQService.GetByIdAsync(id);
            DanhGiaDTQVM.SupplierDTO = await _danhGiaDTQService.GetSupplierByIdAsync(supplierId);

            if (DanhGiaDTQVM.DanhGiaDTQDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            DanhGiaDTQVM.MucDoHapDans = SD.MucDoHapDan();
            DanhGiaDTQVM.DoiTuongKhachPhuHops = SD.DoiTuongKhachPhuHop();
            DanhGiaDTQVM.NhaVeSinhs = SD.NhaVeSinh();
            return PartialView(DanhGiaDTQVM);
        }

        [HttpPost, ActionName("CapNhatDTQ_Partial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatDTQ_Partial_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                DanhGiaDTQVM.DanhGiaDTQDTO.NgaySua = DateTime.Now;
                DanhGiaDTQVM.DanhGiaDTQDTO.NguoiSua = user.Username;

                #region log file

                string temp = "", log = "";

                var t = _danhGiaDTQService.GetByIdAsNoTracking(DanhGiaDTQVM.DanhGiaDTQDTO.Id);

                if (t.Gpkd != DanhGiaDTQVM.DanhGiaDTQDTO.Gpkd)
                {
                    temp += String.Format("- Gpkd thay đổi: {0}->{1}", t.Gpkd, DanhGiaDTQVM.DanhGiaDTQDTO.Gpkd);
                }

                if (t.Vat != DanhGiaDTQVM.DanhGiaDTQDTO.Vat)
                {
                    temp += String.Format("- Vat thay đổi: {0}->{1}", t.Vat, DanhGiaDTQVM.DanhGiaDTQDTO.Vat);
                }

                if (t.BaiDoXe != DanhGiaDTQVM.DanhGiaDTQDTO.BaiDoXe)
                {
                    temp += String.Format("- BaiDoXe thay đổi: {0}->{1}", t.BaiDoXe, DanhGiaDTQVM.DanhGiaDTQDTO.BaiDoXe);
                }

                if (t.CoCheDoHdv != DanhGiaDTQVM.DanhGiaDTQDTO.CoCheDoHdv)
                {
                    temp += String.Format("- CoCheDoHdv thay đổi: {0}->{1}", t.CoCheDoHdv, DanhGiaDTQVM.DanhGiaDTQDTO.CoCheDoHdv);
                }

                if (t.KhaoSatThucTe != DanhGiaDTQVM.DanhGiaDTQDTO.KhaoSatThucTe)
                {
                    temp += String.Format("- KhaoSatThucTe thay đổi: {0}->{1}", t.KhaoSatThucTe, DanhGiaDTQVM.DanhGiaDTQDTO.KhaoSatThucTe);
                }

                if (t.SoChoToiDa != DanhGiaDTQVM.DanhGiaDTQDTO.SoChoToiDa)
                {
                    temp += String.Format("- SoChoToiDa thay đổi: {0}->{1}", t.SoChoToiDa, DanhGiaDTQVM.DanhGiaDTQDTO.SoChoToiDa);
                }

                if (t.MucDoHapDan != DanhGiaDTQVM.DanhGiaDTQDTO.MucDoHapDan)
                {
                    temp += String.Format("- MucDoHapDan thay đổi: {0}->{1}", t.MucDoHapDan, DanhGiaDTQVM.DanhGiaDTQDTO.MucDoHapDan);
                }

                if (t.SoLuongNhaHang != DanhGiaDTQVM.DanhGiaDTQDTO.SoLuongNhaHang)
                {
                    temp += String.Format("- SoLuongNhaHang thay đổi: {0}->{1}", t.SoLuongNhaHang, DanhGiaDTQVM.DanhGiaDTQDTO.SoLuongNhaHang);
                }

                if (t.NhaVeSinh != DanhGiaDTQVM.DanhGiaDTQDTO.NhaVeSinh)
                {
                    temp += String.Format("- NhaVeSinh thay đổi: {0}->{1}", t.NhaVeSinh, DanhGiaDTQVM.DanhGiaDTQDTO.NhaVeSinh);
                }

                if (t.DoiTuongKhachPhuHop != DanhGiaDTQVM.DanhGiaDTQDTO.DoiTuongKhachPhuHop)
                {
                    temp += String.Format("- DoiTuongKhachPhuHop thay đổi: {0}->{1}", t.DoiTuongKhachPhuHop, DanhGiaDTQVM.DanhGiaDTQDTO.DoiTuongKhachPhuHop);
                }

                if (t.ViTri != DanhGiaDTQVM.DanhGiaDTQDTO.ViTri)
                {
                    temp += String.Format("- ViTri thay đổi: {0}->{1}", t.ViTri, DanhGiaDTQVM.DanhGiaDTQDTO.ViTri);
                }

                if (t.KqDat != DanhGiaDTQVM.DanhGiaDTQDTO.KqDat)
                {
                    temp += String.Format("- KqDat thay đổi: {0}->{1}", t.KqDat, DanhGiaDTQVM.DanhGiaDTQDTO.KqDat);
                }

                if (t.KqKhaoSatThem != DanhGiaDTQVM.DanhGiaDTQDTO.KqKhaoSatThem)
                {
                    temp += String.Format("- KqKhaoSatThem thay đổi: {0}->{1}", t.KqKhaoSatThem, DanhGiaDTQVM.DanhGiaDTQDTO.KqKhaoSatThem);
                }

                if (t.TiemNang != DanhGiaDTQVM.DanhGiaDTQDTO.TiemNang)
                {
                    temp += String.Format("- TiemNang thay đổi: {0}->{1}", t.TiemNang, DanhGiaDTQVM.DanhGiaDTQDTO.TiemNang);
                }

                if (t.TaiKy != DanhGiaDTQVM.DanhGiaDTQDTO.TaiKy)
                {
                    temp += String.Format("- TaiKy thay đổi: {0}->{1}", t.TaiKy, DanhGiaDTQVM.DanhGiaDTQDTO.TaiKy);
                }

                if (t.NguoiDanhGia != DanhGiaDTQVM.DanhGiaDTQDTO.NguoiDanhGia)
                {
                    temp += String.Format("- NguoiDanhGia thay đổi: {0}->{1}", t.NguoiDanhGia, DanhGiaDTQVM.DanhGiaDTQDTO.NguoiDanhGia);
                }

                if (t.NgayDanhGia != DanhGiaDTQVM.DanhGiaDTQDTO.NgayDanhGia)
                {
                    temp += String.Format("- NgayDanhGia thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", t.NgayDanhGia, DanhGiaDTQVM.DanhGiaDTQDTO.NgayDanhGia);
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
                    DanhGiaDTQVM.DanhGiaDTQDTO.LogFile = t.LogFile;
                }

                try
                {
                    await _danhGiaDTQService.UpdateAsync(DanhGiaDTQVM.DanhGiaDTQDTO);

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
                    await _danhGiaDTQService.CreateErroLogAsync(errorLog);
                    return Json(new
                    {
                        status = false,
                        message = "Cập nhật không thành công!"
                    });
                }
            }
            // not valid

            return View(DanhGiaDTQVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            DanhGiaDTQVM.StrUrl = strUrl;

            var DanhGiaDTQDTO = _danhGiaDTQService.GetByIdAsNoTracking(id);
            if (DanhGiaDTQDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            try
            {
                await _danhGiaDTQService.Delete(DanhGiaDTQDTO);

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
                await _danhGiaDTQService.CreateErroLogAsync(errorLog);

                return Json(false);
            }
        }

        public async Task<IActionResult> ExportToWord_DTQ(string supplierId, long id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id == 0)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var supplierDTO = await _danhGiaDTQService.GetSupplierByIdAsync(supplierId);
            if (string.IsNullOrEmpty(supplierId) || supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            TapDoanDTO tapDoanDTO = await _danhGiaDTQService.GetTapDoanByIdAsync(supplierDTO.TapDoanId);
            var danhGiaDTQDTO = await _danhGiaDTQService.GetByIdAsync(id);

            if (danhGiaDTQDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var loaiDvDTO = _danhGiaDTQService.GetAllLoaiDv().Where(x => x.Id == danhGiaDTQDTO.LoaiDvid).FirstOrDefault();

            DocX doc = null;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = webRootPath + @"\WordTemplates\M01e-DGNCU-DiemTQ.docx";
            doc = DocX.Load(fileName);

            doc.AddCustomProperty(new CustomProperty("TenGiaoDich", supplierDTO.Code + " - " + supplierDTO.Tengiaodich));
            doc.AddCustomProperty(new CustomProperty("TenThuongMai", supplierDTO.Tenthuongmai));
            doc.AddCustomProperty(new CustomProperty("TapDoan", tapDoanDTO == null ? "" : tapDoanDTO.Ten));
            doc.AddCustomProperty(new CustomProperty("DiaChi", supplierDTO.Diachi));
            doc.AddCustomProperty(new CustomProperty("DienThoai/Email", supplierDTO.Dienthoai + "/" + supplierDTO.Email));
            doc.AddCustomProperty(new CustomProperty("LoaiHinhDV", loaiDvDTO.TenLoai));

            doc.AddCustomProperty(new CustomProperty("GiayPhepKinhDoanh", danhGiaDTQDTO.Gpkd == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("VAT", danhGiaDTQDTO.Vat == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("BaiDoXe", danhGiaDTQDTO.BaiDoXe == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("CoCheDoHdv", danhGiaDTQDTO.CoCheDoHdv == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThucTe", danhGiaDTQDTO.KhaoSatThucTe == true ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("SoChoToiDa", danhGiaDTQDTO.SoChoToiDa));
            doc.AddCustomProperty(new CustomProperty("MucDoHapDan", danhGiaDTQDTO.MucDoHapDan));
            doc.AddCustomProperty(new CustomProperty("SoLuongNhaHang", danhGiaDTQDTO.SoLuongNhaHang));
            doc.AddCustomProperty(new CustomProperty("NhaVeSinh", danhGiaDTQDTO.NhaVeSinh));
            doc.AddCustomProperty(new CustomProperty("DoiTuongKhachPhuHop", danhGiaDTQDTO.DoiTuongKhachPhuHop));
            doc.AddCustomProperty(new CustomProperty("ViTri", danhGiaDTQDTO.ViTri));
            doc.AddCustomProperty(new CustomProperty("DatYeuCau", danhGiaDTQDTO.KqDat == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThem", danhGiaDTQDTO.KqKhaoSatThem == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TaiKy", danhGiaDTQDTO.TaiKy == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("TiemNang", danhGiaDTQDTO.TiemNang == true ? "Có" : ""));

            doc.AddCustomProperty(new CustomProperty("Ngay", DateTime.Now.Day));
            doc.AddCustomProperty(new CustomProperty("Thang", DateTime.Now.Month));
            doc.AddCustomProperty(new CustomProperty("Nam", DateTime.Now.Year));

            doc.AddList("First Item", 0, ListItemType.Numbered);

            MemoryStream stream = new MemoryStream();

            // Saves the Word document to MemoryStream
            doc.SaveAs(stream);
            stream.Position = 0;
            // Download Word document in the browser
            return File(stream, "application/msword", "DiemTQ_" + user.Username + "_" + DateTime.Now + ".docx");
        }
    }
}