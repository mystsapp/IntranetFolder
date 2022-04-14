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
                        message = "Thêm mới không thành công!"
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
            var danhGiaDTQDTO = await _danhGiaCamLaoService.GetByIdAsync(id);

            if (danhGiaDTQDTO == null)
            {
                ViewBag.ErrorMessage = "Item này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            var loaiDvDTO = _danhGiaCamLaoService.GetAllLoaiDv().Where(x => x.Id == danhGiaDTQDTO.LoaiDvid).FirstOrDefault();

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

            doc.AddCustomProperty(new CustomProperty("ThoiGianHoatDong", danhGiaDTQDTO.ThoiGianHoatDong));
            doc.AddCustomProperty(new CustomProperty("CacDoiTacVN", danhGiaDTQDTO.CacDoiTacVn));
            doc.AddCustomProperty(new CustomProperty("Tuyen", danhGiaDTQDTO.Tuyen));
            doc.AddCustomProperty(new CustomProperty("CLDVVaHDV", danhGiaDTQDTO.CldvvaHdv));
            doc.AddCustomProperty(new CustomProperty("SanPham", danhGiaDTQDTO.SanPham));
            doc.AddCustomProperty(new CustomProperty("GiaCa", danhGiaDTQDTO.GiaCa));
            doc.AddCustomProperty(new CustomProperty("CoHTXuLySuCo", danhGiaDTQDTO.CoHtxuLySuCo ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThucTe", danhGiaDTQDTO.KhaoSatThucTe ? "Có" : "Không"));
            doc.AddCustomProperty(new CustomProperty("DatYeuCau", danhGiaDTQDTO.Kqdat == true ? "Có" : ""));
            doc.AddCustomProperty(new CustomProperty("KhaoSatThem", danhGiaDTQDTO.KqkhaoSatThem == true ? "Có" : ""));
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
            return File(stream, "application/msword", "LandTourNN_" + user.Username + "_" + DateTime.Now + ".docx");
        }
    }
}