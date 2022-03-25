using Common;
using Data.Models;
using Data.Repository;
using Data.Utilities;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetFolder.Controllers
{
    public class SupplierController : BaseController
    {
        private readonly ISupplierService _supplierService;

        [BindProperty]
        public SupplierViewModel SupplierVM { get; set; }

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
            SupplierVM = new SupplierViewModel()
            {
                SupplierDTO = new SupplierDTO()
            };
        }

        public async Task<IActionResult> Index(string searchString, string searchFromDate, string searchToDate, string boolSgtcode, string id, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.id = "";
            }

            SupplierVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            SupplierVM.Page = page;
            SupplierVM.SearchString = searchString;

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;
            ViewBag.boolSgtcode = boolSgtcode;

            if (!string.IsNullOrEmpty(id)) // for redirect with id
            {
                SupplierVM.SupplierDTO = await _supplierService.GetByIdAsync(id);
                ViewBag.id = SupplierVM.SupplierDTO.Code;
            }
            else
            {
                SupplierVM.SupplierDTO = new SupplierDTO();
            }
            SupplierVM.SupplierDTOs = await _supplierService.ListSupplier(searchString, searchFromDate, searchToDate, page);
            return View(SupplierVM);
        }

        public async Task<IActionResult> Create(string strUrl)
        {
            SupplierVM.StrUrl = strUrl;
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            SupplierVM.VTinhs = await _supplierService.GetTinhs();
            SupplierVM.Quocgias = await _supplierService.GetQuocgias();
            SupplierVM.LoaiSaos = SD.LoaiSao();
            SupplierVM.TapDoanDTOs = _supplierService.GetAll_TapDoan();
            //var quocgia = SupplierVM.Quocgias.Where(x => x.Id == 230).FirstOrDefault(); // VIETNAM

            // next Id
            SupplierVM.SupplierDTO.Code = _supplierService.GetNextId("", user.Macn);
            // next Id

            return View(SupplierVM);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (!ModelState.IsValid)
            {
                SupplierVM = new SupplierViewModel()
                {
                    SupplierDTO = new SupplierDTO(),
                    StrUrl = strUrl
                };

                return View(SupplierVM);
            }

            SupplierVM.SupplierDTO.Ngaytao = DateTime.Now;
            SupplierVM.SupplierDTO.Nguoitao = user.Username;
            SupplierVM.SupplierDTO.Tour = "IB";

            // next Id
            SupplierVM.SupplierDTO.Code = _supplierService.GetNextId("", user.Macn);
            // next Id

            try
            {
                await _supplierService.CreateAsync(SupplierVM.SupplierDTO); // save

                SetAlert("Thêm mới thành công.", "success");

                return Redirect(strUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                return View(SupplierVM);
            }
        }

        public async Task<IActionResult> Edit(string id, string strUrl)
        {
            // from session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            SupplierVM.StrUrl = strUrl;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            SupplierVM.SupplierDTO = await _supplierService.GetByIdAsync(id);

            if (SupplierVM.SupplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            SupplierVM.VTinhs = await _supplierService.GetTinhs();
            var thanhpho1s = await _supplierService.GetThanhpho1s();
            SupplierVM.Thanhpho1s = thanhpho1s.Where(x => x.Matinh == SupplierVM.SupplierDTO.Tinhtp);
            SupplierVM.Quocgias = await _supplierService.GetQuocgias();
            SupplierVM.LoaiSaos = SD.LoaiSao();
            SupplierVM.TapDoanDTOs = _supplierService.GetAll_TapDoan();

            return View(SupplierVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id, string strUrl)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (id != SupplierVM.SupplierDTO.Code)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (ModelState.IsValid)
            {
                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                string temp = "", log = "";

                var t = _supplierService.GetByIdAsNoTracking(id);

                if (t.Diachi != SupplierVM.SupplierDTO.Diachi)
                {
                    temp += String.Format("- Diachi thay đổi: {0}->{1}", t.Diachi, SupplierVM.SupplierDTO.Diachi);
                }

                if (t.Dienthoai != SupplierVM.SupplierDTO.Dienthoai)
                {
                    temp += String.Format("- Dienthoai thay đổi: {0}->{1}", t.Dienthoai, SupplierVM.SupplierDTO.Dienthoai);
                }

                if (t.Email != SupplierVM.SupplierDTO.Email)
                {
                    temp += String.Format("- Email thay đổi: {0}->{1}", t.Email, SupplierVM.SupplierDTO.Email);
                }

                if (t.Fax != SupplierVM.SupplierDTO.Fax)
                {
                    temp += String.Format("- Fax thay đổi: {0}->{1}", t.Fax, SupplierVM.SupplierDTO.Fax);
                }

                if (t.Masothue != SupplierVM.SupplierDTO.Masothue)
                {
                    temp += String.Format("- Masothue thay đổi: {0}->{1}", t.Masothue, SupplierVM.SupplierDTO.Masothue);
                }

                if (t.Nganhnghe != SupplierVM.SupplierDTO.Nganhnghe)
                {
                    temp += String.Format("- Nganhnghe thay đổi: {0}->{1}", t.Nganhnghe, SupplierVM.SupplierDTO.Nganhnghe);
                }

                if (t.Ngayhethan != SupplierVM.SupplierDTO.Ngayhethan)
                {
                    temp += String.Format("- Ngayhethan thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", t.Ngayhethan, SupplierVM.SupplierDTO.Ngayhethan);
                }

                if (t.Nguoilienhe != SupplierVM.SupplierDTO.Nguoilienhe)
                {
                    temp += String.Format("- Nguoilienhe thay đổi: {0}->{1}", t.Nguoilienhe, SupplierVM.SupplierDTO.Nguoilienhe);
                }

                if (t.Quocgia != SupplierVM.SupplierDTO.Quocgia)
                {
                    temp += String.Format("- Quocgia thay đổi: {0}->{1}", t.Quocgia, SupplierVM.SupplierDTO.Quocgia);
                }

                if (t.TapDoanId != SupplierVM.SupplierDTO.TapDoanId)
                {
                    temp += String.Format("- Tapdoan thay đổi: {0}->{1}", t.TapDoanId, SupplierVM.SupplierDTO.TapDoanId);
                }

                if (t.Tengiaodich != SupplierVM.SupplierDTO.Tengiaodich)
                {
                    temp += String.Format("- Tengiaodich thay đổi: {0}->{1}", t.Tengiaodich, SupplierVM.SupplierDTO.Tengiaodich);
                }

                if (t.Tennganhang != SupplierVM.SupplierDTO.Tennganhang)
                {
                    temp += String.Format("- Tennganhang thay đổi: {0}->{1}", t.Tennganhang, SupplierVM.SupplierDTO.Tennganhang);
                }

                if (t.Tenthuongmai != SupplierVM.SupplierDTO.Tenthuongmai)
                {
                    temp += String.Format("- Tenthuongmai thay đổi: {0}->{1}", t.Tenthuongmai, SupplierVM.SupplierDTO.Tenthuongmai);
                }

                if (t.Thanhpho != SupplierVM.SupplierDTO.Thanhpho)
                {
                    temp += String.Format("- Thanhpho thay đổi: {0}->{1}", t.Thanhpho, SupplierVM.SupplierDTO.Thanhpho);
                }

                if (t.Tinhtp != SupplierVM.SupplierDTO.Tinhtp)
                {
                    temp += String.Format("- Tinhtp thay đổi: {0}->{1}", t.Tinhtp, SupplierVM.SupplierDTO.Tinhtp);
                }

                if (t.Tknganhang != SupplierVM.SupplierDTO.Tknganhang)
                {
                    temp += String.Format("- Tknganhang thay đổi: {0}->{1}", t.Tknganhang, SupplierVM.SupplierDTO.Tknganhang);
                }

                if (t.Tour != SupplierVM.SupplierDTO.Tour)
                {
                    temp += String.Format("- Tour thay đổi: {0}->{1}", t.Tour, SupplierVM.SupplierDTO.Tour);
                }

                if (t.Trangthai != SupplierVM.SupplierDTO.Trangthai)
                {
                    temp += String.Format("- Trangthai thay đổi: {0}->{1}", t.Tinhtp, SupplierVM.SupplierDTO.Trangthai);
                }

                if (t.Website != SupplierVM.SupplierDTO.Website)
                {
                    temp += String.Format("- Website thay đổi: {0}->{1}", t.Website, SupplierVM.SupplierDTO.Website);
                }

                #endregion log file

                // kiem tra thay doi
                if (temp.Length > 0)
                {
                    log = System.Environment.NewLine;
                    log += "=============";
                    log += System.Environment.NewLine;
                    log += temp + " -User cập nhật tour: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString(); // username
                    t.Logfile = t.Logfile + log;
                    SupplierVM.SupplierDTO.Logfile = t.Logfile;
                }

                try
                {
                    await _supplierService.UpdateAsync(SupplierVM.SupplierDTO);
                    SetAlert("Cập nhật thành công", "success");

                    return Redirect(strUrl);
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(SupplierVM);
                }
            }
            // not valid

            return View(SupplierVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, string strUrl/*, string tabActive*/)
        {
            SupplierVM.StrUrl = strUrl;// + "&tabActive=" + tabActive; // for redirect tab

            var supplierDTO = _supplierService.GetByIdAsNoTracking(id);
            if (supplierDTO == null)
                return NotFound();
            try
            {
                await _supplierService.Delete(supplierDTO);

                SetAlert("Xóa thành công.", "success");
                return Redirect(SupplierVM.StrUrl);
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");
                ModelState.AddModelError("", ex.Message);
                return Redirect(SupplierVM.StrUrl);
            }
        }

        public async Task<IActionResult> DichVuPartial(string id, string strUrl)
        {
            SupplierVM.StrUrl = strUrl;// + "&tabActive=" + tabActive; // for redirect tab

            var supplierDTO = await _supplierService.GetByIdAsync(id);
            if (supplierDTO == null)
            {
                ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            SupplierVM.SupplierDTO = supplierDTO;
            SupplierVM.DanhGiaNhaHangDTOs = await _supplierService.GetDanhGiaNhaHangBy_SupplierId(id);

            return PartialView(SupplierVM);
        }

        // ExportExcel
        [HttpPost]
        public async Task<IActionResult> ExportExcel(string searchString/*, string loaiKh*/)
        {
            IEnumerable<SupplierDTO> supplierDTOs = await _supplierService.FindAsync(searchString);

            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Supplier");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;// CODE
            xlSheet.Column(2).Width = 30;// TÊN GIAO DỊCH
            xlSheet.Column(3).Width = 40;// ĐỊA CHỈ
            xlSheet.Column(4).Width = 30;// TỈNH / THÀNH PHỐ
            xlSheet.Column(5).Width = 30;// TÊN THƯƠNG MẠI
            xlSheet.Column(6).Width = 20;// MÃ SỐ THUẾ
            xlSheet.Column(7).Width = 30;// GROUP / TẬP DOÀN
            xlSheet.Column(8).Width = 20;// TRẠNG THÁI
            xlSheet.Column(9).Width = 30;// NGƯỜI TRÌNH KÝ HD

            xlSheet.Cells[2, 1].Value = "BẢNG KÊ DANH MỤC SUPPLIER";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 9].Merge = true;

            //xlSheet.Cells[4, 4].Value = "Loại Kh: ";
            //xlSheet.Cells[4, 4].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            //var loai = CommonList.LoaiKh().Where(x => x.StrId == loaiKh).FirstOrDefault();
            //xlSheet.Cells[4, 5].Value = loai.Name;
            //xlSheet.Cells[4, 5].Style.Fill.PatternType = ExcelFillStyle.DarkHorizontal;
            //xlSheet.Cells[4, 5].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            //xlSheet.Cells[4, 5].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold | FontStyle.Italic));

            ExcelTool.setCenterAligment(1, 1, 2, 1, xlSheet);

            // Tạo header
            xlSheet.Cells[4, 1].Value = "CODE";
            xlSheet.Cells[4, 2].Value = "TÊN GIAO DỊCH";
            xlSheet.Cells[4, 3].Value = "ĐỊA CHỈ";
            xlSheet.Cells[4, 4].Value = "TỈNH / THÀNH PHỐ";
            xlSheet.Cells[4, 5].Value = "TÊN THƯƠNG MẠI";
            xlSheet.Cells[4, 6].Value = "MÃ SỐ THUẾ";
            xlSheet.Cells[4, 7].Value = "GROUP / TẬP DOÀN";
            xlSheet.Cells[4, 8].Value = "TRẠNG THÁI";
            xlSheet.Cells[4, 9].Value = "NGƯỜI TRÌNH KÝ HD";

            ExcelTool.setBorder(4, 1, 4, 9, xlSheet);
            ExcelTool.setCenterAligment(4, 1, 6, 9, xlSheet);
            xlSheet.Cells[4, 1, 4, 9].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            xlSheet.Cells[4, 1, 4, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center; // canh giữa cột
            xlSheet.Column(2).Style.WrapText = true; // WrapText for column
            xlSheet.Column(3).Style.WrapText = true;
            xlSheet.Column(5).Style.WrapText = true;

            // do du lieu tu table
            int dong = 5;

            //int iRowIndex = 6;

            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");// ColorTranslator.FromHtml("#D3D3D3");
            Color colorTotalRow = ColorTranslator.FromHtml("#66ccff");
            Color colorThanhLy = ColorTranslator.FromHtml("#7FFF00");
            Color colorChuaThanhLy = ColorTranslator.FromHtml("#FFDEAD");

            int idem = 1;

            if (supplierDTOs.Count() > 0)
            {
                foreach (var item in supplierDTOs)
                {
                    //xlSheet.Cells[dong, 1].Value = idem;
                    //ExcelTool.TrSetCellBorder(xlSheet, dong, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Justify, Color.Silver, "Times New Roman", 12, FontStyle.Regular);
                    ////xlSheet.Cells[dong, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[dong, 1].Value = item.Code;
                    ExcelTool.TrSetCellBorder(xlSheet, dong, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Justify, Color.Silver, "Times New Roman", 12, FontStyle.Regular);
                    //xlSheet.Cells[dong, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[dong, 2].Value = item.Tengiaodich;// == true ? "Có" : "Không";
                    ExcelTool.TrSetCellBorder(xlSheet, dong, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Justify, Color.Silver, "Times New Roman", 12, FontStyle.Regular);
                    //xlSheet.Cells[dong, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[dong, 3].Value = item.Diachi;
                    ExcelTool.TrSetCellBorder(xlSheet, dong, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 12, FontStyle.Regular);
                    //xlSheet.Cells[dong, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[dong, 4].Value = item.Tinhtp;
                    ExcelTool.TrSetCellBorder(xlSheet, dong, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 12, FontStyle.Regular);
                    //xlSheet.Cells[dong, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[dong, 5].Value = item.Tenthuongmai;
                    ExcelTool.TrSetCellBorder(xlSheet, dong, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 12, FontStyle.Regular);
                    // xlSheet.Cells[dong, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[dong, 6].Value = item.Masothue;
                    ExcelTool.TrSetCellBorder(xlSheet, dong, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 12, FontStyle.Regular);
                    // xlSheet.Cells[dong, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[dong, 7].Value = string.IsNullOrEmpty(item.TapDoanDTO.Ten) ? "" : item.TapDoanDTO.Ten;
                    ExcelTool.TrSetCellBorder(xlSheet, dong, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 12, FontStyle.Regular);
                    // xlSheet.Cells[dong, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[dong, 8].Value = item.Trangthai == true ? "Kích hoạt" : "Khoá";
                    ExcelTool.TrSetCellBorder(xlSheet, dong, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 12, FontStyle.Regular);
                    // xlSheet.Cells[dong, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[dong, 9].Value = item.NguoiTrinhKyHd;
                    ExcelTool.TrSetCellBorder(xlSheet, dong, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 12, FontStyle.Regular);
                    // xlSheet.Cells[dong, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    //setBorder(5, 1, dong, 10, xlSheet);
                    //ExcelTool.NumberFormat(dong, 6, dong + 1, 6, xlSheet);

                    dong++;
                    idem++;
                }

                //xlSheet.Cells[dong, 5].Value = "Tổng cộng:";
                //xlSheet.Cells[dong, 5].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold | FontStyle.Italic));
                //xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (dong - 1) + ")";

                ////NumberFormat(dong, 3, dong, 4, xlSheet);
                ////setFontBold(dong, 1, dong, 10, 12, xlSheet);
                //ExcelTool.setBorder(dong, 1, dong, 8, xlSheet);
                //ExcelTool.DateFormat(6, 1, dong, 1, xlSheet);

                //xlSheet.Cells[dong + 2, 1].Value = "Người lập bảng kê";
                //xlSheet.Cells[dong + 2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
                //xlSheet.Cells[dong + 2, 4].Value = "Kế toán trưởng";
                //xlSheet.Cells[dong + 2, 4].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));

                //setCenterAligment(dong + 2, 1, dong + 2, 4, xlSheet);
            }
            else
            {
                SetAlert("Không có khách hàng nào.", "warning");
                return RedirectToAction(nameof(Index), new { searchString = searchString });
            }

            //dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";

            // Sum tổng tiền
            // xlSheet.Cells[dong, 5].Value = "TC:";
            //DateTimeFormat(6, 4, 6 + d.Count(), 4, xlSheet);
            // DateTimeFormat(6, 4, 9, 4, xlSheet);
            // setCenterAligment(6, 4, 9, 4, xlSheet);
            // xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (6 + d.Count() - 1) + ")";

            //setBorder(5, 1, 5 + d.Count() + 2, 10, xlSheet);

            //setFontBold(5, 1, 5, 8, 11, xlSheet);
            //setFontSize(6, 1, 6 + d.Count() + 2, 8, 11, xlSheet);
            // canh giua cot stt
            //setCenterAligment(6, 1, 6 + dong + 2, 1, xlSheet);
            // canh giua code chinhanh
            //setCenterAligment(6, 3, 6 + dong + 2, 3, xlSheet);
            // NumberFormat(6, 6, 6 + d.Count(), 6, xlSheet);
            // định dạng số cot, đơn giá, thành tiền tong cong
            // NumberFormat(6, 8, dong, 9, xlSheet);

            ExcelTool.setBorder(dong - 1, 1, dong - 1, 9, xlSheet);
            // setFontBold(dong, 5, dong, 6, 12, xlSheet);

            //xlSheet.View.FreezePanes(6, 20);

            //end du lieu

            byte[] fileContents;
            try
            {
                fileContents = ExcelApp.GetAsByteArray();
                SetAlert("Good job!", "success");
                return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "BangTheoDMSupplier_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetThanhPhoByTinh_Hong(string tinhTPId)
        {
            var thanhpho1s = await _supplierService.GetThanhpho1s();
            return Json(new
            {
                status = true,
                data = thanhpho1s.Where(x => x.Matinh == tinhTPId),
            });
        }

        public ActionResult getThongbao()
        {
            var codeSuppliers = _supplierService.listCapcode();

            return Json(codeSuppliers.Count);
        }

        [HttpGet]
        public ActionResult listCapcode(string search)
        {
            search = string.IsNullOrEmpty(search) ? "" : search.ToUpper();
            ViewBag.search = search;
            var codeSuppliers = _supplierService.listCapcode();
            HttpContext.Session.SetString("urlCodeSupplier", UriHelper.GetDisplayUrl(Request));
            if (!string.IsNullOrEmpty(search))
            {
                codeSuppliers = codeSuppliers.Where(x => x.Tengiaodich.Replace(" ", "").Contains(search.Replace(" ", "")) || x.Tenthuongmai.Replace(" ", "").Contains(search.Replace(" ", "")) || x.Masothue.Contains(search)).ToList();
            }

            foreach (var i in codeSuppliers)
            {
                if (!string.IsNullOrEmpty(i.Tinhtp))
                {
                    i.Tinhtp = _supplierService.getTinhById(i.Tinhtp).Tentinh;
                }
                else
                {
                    i.Tinhtp = "";
                }
            }
            return View(codeSuppliers);
        }

        public async Task<ActionResult> EditCapcodeSupplierById(decimal id)
        {
            var result = _supplierService.getCodeSupplierById(id);
            result.Code = _supplierService.NextId();
            await listQuocgia(result.Quocgia ?? "");
            await listTinh(result.Tinhtp ?? "");
            listThanhphoByTinh(result.Tinhtp ?? "", result.Thanhpho ?? "");
            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult> EditCapcodeSupplierById(Data.Models_QLTour.CodeSupplier entity)
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            if (ModelState.IsValid)
            {
                var s = new Supplier();
                s.Code = _supplierService.NextId();
                s.Tengiaodich = entity.Tengiaodich;
                s.Tenthuongmai = entity.Tenthuongmai;
                s.Tapdoan = entity.Tapdoan ?? "";
                s.Tinhtp = entity.Tinhtp;
                s.Thanhpho = entity.Thanhpho;
                s.Quocgia = entity.Quocgia;
                s.Diachi = entity.Diachi;
                s.Dienthoai = entity.Dienthoai;
                s.Fax = entity.Fax;
                s.Masothue = entity.Masothue;
                s.Nganhnghe = entity.Nganhnghe ?? "";
                s.Nguoilienhe = entity.Nguoilienhe;
                s.Website = entity.Website;
                s.Email = entity.Email;
                s.Trangthai = true;
                s.Ngaytao = DateTime.Now;
                s.Chinhanh = entity.Chinhanh;
                s.Logfile = "User cấp code: " + user.Username + ", cấp theo yêu cầu của " + entity.Nguoiyeucau + " ngày " + entity.Ngayyeucau.Value.ToString("dd/MM/yyyy");
                var result = await _supplierService.Create(s);
                if (result != null)
                {
                    SetAlert("Cấp code thành công", "success");
                    _supplierService.updateCapCodeSupplier(entity.Id);
                }
                else
                {
                    SetAlert("Cấp code không thành công", "error");
                }
                return Redirect(HttpContext.Session.GetString("urlCodeSupplier"));
            }
            return View();
        }

        public async Task listQuocgia(string selected)
        {
            var quocgiaIE = await _supplierService.GetQuocgias();
            List<Quocgium> quocgia = quocgiaIE.ToList();
            ViewBag.quocgia = new SelectList(quocgia, "TenNuoc", "TenNuoc", selected);
        }

        public async Task listThanhpho(string selected)
        {
            var thanhpho1sIE = await _supplierService.GetThanhpho1s();
            List<Thanhpho1> thanhpho = thanhpho1sIE.ToList();
            ViewBag.thanhpho = new SelectList(thanhpho, "Tentp", "Tentp", selected);
        }

        public void Trangthai(string select)
        {
            List<SelectListItem> trangthai = new List<SelectListItem>();
            trangthai.Add(new SelectListItem { Text = "Kích hoạt", Value = "true" });
            trangthai.Add(new SelectListItem { Text = "Khóa", Value = "false" });

            ViewBag.trangthai = new SelectList(trangthai, "Value", "Text", select);
        }

        public async Task listTinh(string selected)
        {
            var vTinhs = await _supplierService.GetTinhs();
            List<VTinh> tinh = vTinhs.ToList();
            var empty = new VTinh
            {
                Matinh = "",
                Tentinh = "-- Chọn tỉnh thành --"
            };
            tinh.Insert(0, empty);
            ViewBag.tinh = new SelectList(tinh, "Matinh", "Tentinh", selected);
        }

        public void listThanhphoByTinh(string matinh, string selected)
        {
            List<Thanhpho1> thanhpho = _supplierService.ListThanhphoByTinh(matinh);
            var empty = new Thanhpho1
            {
                Matp = "",
                Tentp = "-- Chọn thông tin --"
            };
            thanhpho.Insert(0, empty);
            ViewBag.thanhpho = new SelectList(thanhpho, "Matp", "Tentp", selected);
        }

        public JsonResult getGroup()
        {
            var suppliers = _supplierService.GetAll_Supplier();//.Where(x => x.Tapdoan.Length > 0 || x.Tapdoan != null).Select(x => x.Tapdoan).Distinct();
            var suppliers1 = suppliers.Where(x => !string.IsNullOrEmpty(x.Tapdoan));//
            var data = suppliers1.Select(x => x.Tapdoan).Distinct();
            return Json(data);
        }

        public ActionResult listTenthuongmai(string search)//, string search1)
        {
            search = search ?? "";
            // search1 = search1 ?? "";
            ViewBag.search = search;
            List<Supplier> result = new List<Supplier>();
            if (!string.IsNullOrEmpty(search))// && !string.IsNullOrEmpty(search1))
            {
                var suppliers = _supplierService.GetAll_Supplier();//.Where(x => x.Tenthuongmai.Trim().Contains(search.Trim().ToUpper()) || x.Tengiaodich.Trim().Contains(search1.Trim().ToUpper()));//.Distinct().ToList();
                suppliers = suppliers.Where(x => (!string.IsNullOrEmpty(x.Tenthuongmai) && x.Tenthuongmai.Trim().Contains(search.Trim().ToUpper()) ||
                                                 (!string.IsNullOrEmpty(x.Tengiaodich) && x.Tengiaodich.Trim().Contains(search.Trim().ToUpper()))));
                result = suppliers.ToList();
            }
            //else
            //{
            //    result = _supplierService.GetAll_Supplier().Where(x => x.Tenthuongmai.Trim().Contains(search.Trim().ToUpper()) || x.Tengiaodich.Trim().Contains(search.Trim().ToUpper())).ToList().Distinct().ToList();
            //}

            if (result == null)
            {
                return PartialView();
            }
            else
            {
                foreach (var i in result)
                {
                    if (!string.IsNullOrEmpty(i.Tinhtp))
                    {
                        i.Tinhtp = _supplierService.getTinhById(i.Tinhtp).Tentinh;
                    }
                    else
                    {
                        i.Tinhtp = "";
                    }
                }
                return PartialView(result);
            }
        }

        [HttpGet]
        public ActionResult DeleteCapcode(decimal id)
        {
            var s = _supplierService.getCodeSupplierById(id);
            return PartialView(s);
        }

        [HttpPost]
        public ActionResult DeleteCapcode(Data.Models_QLTour.CodeSupplier model)
        {
            var result = _supplierService.huyCapcodeSupplier(model);
            return Redirect(HttpContext.Session.GetString("urlCodeSupplier"));
        }

        [HttpGet]
        public ActionResult GetThanhphoByTinh(string matinh)
        {
            List<Thanhpho1> thanhphos = new List<Thanhpho1>();
            if (!string.IsNullOrWhiteSpace(matinh))
            {
                thanhphos = _supplierService.ListThanhphoByTinh(matinh).ToList();
                if (thanhphos.Count() == 0)
                {
                    var empty = new Thanhpho1
                    {
                        Matp = "",
                        Tentp = "Không có thông tin"
                    };
                    thanhphos.Insert(0, empty);
                }
                else
                {
                    var empty = new Thanhpho1
                    {
                        Matp = "",
                        Tentp = "-- Chọn thông tin --"
                    };
                    thanhphos.Insert(0, empty);
                }
            }

            return Json(thanhphos);
        }
    }
}