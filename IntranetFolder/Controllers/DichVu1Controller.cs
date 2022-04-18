using Common;
using Data.Models;
using Data.Utilities;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace IntranetFolder.Controllers
{
    public class DichVu1Controller : BaseController
    {
        private readonly IDichVu1Service _dichVu1Service;

        [BindProperty]
        public DichVu1ViewModel DichVu1VM { get; set; }

        public DichVu1Controller(IDichVu1Service dichVu1Service)
        {
            DichVu1VM = new DichVu1ViewModel()
            {
                DichVu1DTO = new Model.DichVu1DTO()
            };
            _dichVu1Service = dichVu1Service;
        }

        public async Task<IActionResult> Index()
        {
            //IEnumerable<DichVu1DTO> dichVu1DTO = _dichVu1Service.GetAll_AsNoTracked();

            //List<DichVu1DTO> dichVu1DTOs = new List<DichVu1DTO>();
            //foreach (var item in dichVu1DTO)
            //{
            //    if (!string.IsNullOrEmpty(item.ThoiGianHd))
            //    {
            //        var thoiHanHD = item.ThoiGianHd.Split(" - ");
            //        var batDau = thoiHanHD[0];
            //        var ketThuc = "";
            //        if (thoiHanHD.Count() > 1)
            //        {
            //            ketThuc = thoiHanHD[1];
            //            item.KetThucHd = DateTime.Parse(ketThuc);
            //        }

            //        item.BatDauHd = DateTime.Parse(batDau);

            //        await _dichVu1Service.UpdateAsync(item);
            //    }
            //}
            //if (dichVu1DTOs.Count > 0)
            //{
            //    await _dichVu1Service.UpdateRangeAsync(dichVu1DTOs);
            //}
            return View();
        }

        public async Task<IActionResult> DichVu1Partial(string supplierId, int page, string searchString)
        {
            //await _dichVu1Service.DeleteHinhanh(41);
            var supplierDTO = await _dichVu1Service.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                //ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return Content("Supplier này không tồn tại.");
            }
            DichVu1VM.Page = page;
            DichVu1VM.SearchString = searchString;
            DichVu1VM.SupplierDTO = supplierDTO;
            DichVu1VM.DichVu1DTOs = await _dichVu1Service.GetDichVu1By_SupplierId(supplierId);

            return PartialView(DichVu1VM);
        }

        public async Task<IActionResult> EditDichVu1(string id, string supplierId, int page, string failMessage, DichVu1DTO dichVu1DTO, string searchString)
        {
            string stringImageUrls = "";
            if (TempData["stringImageUrls"] != null)
            {
                stringImageUrls = (string)TempData["stringImageUrls"];
            }

            //int page = 0;
            DichVu1VM.Page = page;
            DichVu1VM.SearchString = searchString;
            if (TempData["page"] != null)
            {
                DichVu1VM.Page = (int)TempData["page"];
            }

            if (TempData["id"] != null)
            {
                id = (string)TempData["id"];
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var supplierDTO = await _dichVu1Service.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                //ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return Content("Supplier này không tồn tại.");
            }
            if (string.IsNullOrEmpty(id))
            {
                return Content("DV này không tồn tại.");
            }
            DichVu1VM.DichVu1DTO = await _dichVu1Service.GetByIdAsync(id);
            DichVu1VM.DichVu1DTO.ImageUrls = DichVu1VM.DichVu1DTO.HinhAnhDTOs.Select(x => x.Url).ToList();
            var imageUrlsZin = DichVu1VM.DichVu1DTO.HinhAnhDTOs.Select(x => x.Url).ToList();
            if (!string.IsNullOrEmpty(failMessage))
            {
                ModelState.AddModelError("", failMessage);
            }
            ////if (!string.IsNullOrEmpty(dichVu1DTO)) // dichVu1DTO: ThemMoiDichVu1HinhAnh chuyền qua
            if (!string.IsNullOrEmpty(dichVu1DTO.MaDv)) // dichVu1DTO: ThemMoiDichVu1HinhAnh chuyền qua
            {
                DichVu1VM.DichVu1DTO = dichVu1DTO;
                //DichVu1VM.DichVu1DTO = JsonConvert.DeserializeObject<DichVu1DTO>(dichVu1DTO);
            }
            if (!string.IsNullOrEmpty(stringImageUrls)) // dichVu1DTO: ThemMoiDichVu1HinhAnh chuyền qua
            {
                DichVu1VM.DichVu1DTO.StringImageUrls = stringImageUrls;
                //if (DichVu1VM.DichVu1DTO.ImageUrls.Count > 0) // Edit
                //{
                //    //DichVu1VM.DichVu1DTO.ImageUrls = imageUrlsZin;

                //    DichVu1VM.DichVu1DTO.ImageUrls.AddRange(JsonConvert.DeserializeObject<List<string>>(stringImageUrls));
                //}
                //else
                //{
                //    DichVu1VM.DichVu1DTO.ImageUrls = JsonConvert.DeserializeObject<List<string>>(stringImageUrls);
                //}
                DichVu1VM.DichVu1DTO.ImageUrls = JsonConvert.DeserializeObject<List<string>>(stringImageUrls);
            }
            //else
            //{
            //    DichVu1VM.DichVu1DTO.ImageUrls = DichVu1VM.DichVu1DTO.HinhAnhDTOs.Select(x => x.Url).ToList();
            //}

            DichVu1VM.SupplierDTO = supplierDTO;
            //DichVu1VM.DichVu1DTO.SupplierId = supplierId;
            //DichVu1VM.Vungmiens = await _dichVu1Service.Vungmiens();
            //DichVu1VM.VTinhs = await _dichVu1Service.GetTinhs();
            //DichVu1VM.Thanhpho1s = await _dichVu1Service.GetThanhpho1s();
            DichVu1VM.LoaiSaos = SD.LoaiSao();
            DichVu1VM.LoaiDvs = _dichVu1Service.GetAllLoaiDv();
            DichVu1VM.UserDTOs = await _dichVu1Service.GetAllUsers_Intranet();

            return View(DichVu1VM);
        }

        [HttpPost, ActionName("EditDichVu1")]
        public async Task<IActionResult> EditDichVu1_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            // save DTO
            //HotelRoomModel.Details = await QuillHtml.GetHTML();
            DichVu1VM.DichVu1DTO.NguoiSua = user.Username;
            DichVu1VM.DichVu1DTO.NgaySua = DateTime.Now;

            //// NextID
            //var loaiDvDTO = _dichVu1Service.GetAllLoaiDv().Where(x => x.Id == DichVu1VM.DichVu1DTO.LoaiDvid).FirstOrDefault();
            //DichVu1VM.DichVu1DTO.MaDv = _dichVu1Service.GetMaDv(loaiDvDTO.MaLoai);

            if (ModelState.IsValid)
            {
                // kiem tra thay doi : trong getbyid() va ngoai view

                #region log file

                string temp = "", log = "";

                var t = _dichVu1Service.GetByIdAsNoTracking(DichVu1VM.DichVu1DTO.MaDv);

                if (t.TenHd != DichVu1VM.DichVu1DTO.TenHd)
                {
                    temp += String.Format("- TenDv thay đổi: {0}->{1}", t.TenHd, DichVu1VM.DichVu1DTO.TenHd);
                }

                if (t.LoaiSao != DichVu1VM.DichVu1DTO.LoaiSao)
                {
                    temp += String.Format("- LoaiSao thay đổi: {0}->{1}", t.LoaiSao, DichVu1VM.DichVu1DTO.LoaiSao);
                }

                if (t.ThongTinLienHe != DichVu1VM.DichVu1DTO.ThongTinLienHe)
                {
                    temp += String.Format("- ThongTinLienHe thay đổi: {0}->{1}", t.ThongTinLienHe, DichVu1VM.DichVu1DTO.ThongTinLienHe);
                }

                if (t.DiaChi != DichVu1VM.DichVu1DTO.DiaChi)
                {
                    temp += String.Format("- DiaChi thay đổi: {0}->{1}", t.DiaChi, DichVu1VM.DichVu1DTO.DiaChi);
                }

                if (t.Email != DichVu1VM.DichVu1DTO.Email)
                {
                    temp += String.Format("- Email thay đổi: {0}->{1}", t.Email, DichVu1VM.DichVu1DTO.Email);
                }

                if (t.DienThoai != DichVu1VM.DichVu1DTO.DienThoai)
                {
                    temp += String.Format("- DienThoai thay đổi: {0}->{1}", t.DienThoai, DichVu1VM.DichVu1DTO.DienThoai);
                }

                if (t.NoiDung != DichVu1VM.DichVu1DTO.NoiDung)
                {
                    temp += String.Format("- NoiDung thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", t.NoiDung, DichVu1VM.DichVu1DTO.NoiDung);
                }

                if (t.NguoiLienHe != DichVu1VM.DichVu1DTO.NguoiLienHe)
                {
                    temp += String.Format("- NguoiLienHe thay đổi: {0}->{1}", t.NguoiLienHe, DichVu1VM.DichVu1DTO.NguoiLienHe);
                }

                if (t.Website != DichVu1VM.DichVu1DTO.Website)
                {
                    temp += String.Format("- Website thay đổi: {0}->{1}", t.Website, DichVu1VM.DichVu1DTO.Website);
                }

                if (t.DaKy != DichVu1VM.DichVu1DTO.DaKy)
                {
                    temp += String.Format("- DaKy thay đổi: {0}->{1}", t.DaKy, DichVu1VM.DichVu1DTO.DaKy);
                }

                if (t.HoatDong != DichVu1VM.DichVu1DTO.HoatDong)
                {
                    temp += String.Format("- HoatDong thay đổi: {0}->{1}", t.HoatDong, DichVu1VM.DichVu1DTO.HoatDong);
                }

                if (t.Tuyen != DichVu1VM.DichVu1DTO.Tuyen)
                {
                    temp += String.Format("- Tuyen thay đổi: {0}->{1}", t.Tuyen, DichVu1VM.DichVu1DTO.Tuyen);
                }

                if (t.LoaiTau != DichVu1VM.DichVu1DTO.LoaiTau)
                {
                    temp += String.Format("- LoaiTau thay đổi: {0}->{1}", t.LoaiTau, DichVu1VM.DichVu1DTO.LoaiTau);
                }

                if (t.LoaiXe != DichVu1VM.DichVu1DTO.LoaiXe)
                {
                    temp += String.Format("- LoaiXe thay đổi: {0}->{1}", t.LoaiXe, DichVu1VM.DichVu1DTO.LoaiXe);
                }

                if (t.DauXe != DichVu1VM.DichVu1DTO.DauXe)
                {
                    temp += String.Format("- DauXe thay đổi: {0}->{1}", t.DauXe, DichVu1VM.DichVu1DTO.DauXe);
                }

                if (t.GhiChu != DichVu1VM.DichVu1DTO.GhiChu)
                {
                    temp += String.Format("- GhiChu thay đổi: {0}->{1}", t.GhiChu, DichVu1VM.DichVu1DTO.GhiChu);
                }

                if (t.GiaHd != DichVu1VM.DichVu1DTO.GiaHd)
                {
                    temp += String.Format("- GiaHd thay đổi: {0}->{1}", t.GiaHd, DichVu1VM.DichVu1DTO.GiaHd);
                }

                if (t.LoaiHd != DichVu1VM.DichVu1DTO.LoaiHd)
                {
                    temp += String.Format("- LoaiHd thay đổi: {0}->{1}", t.LoaiHd, DichVu1VM.DichVu1DTO.LoaiHd);
                }

                if (t.LoaiDvid != DichVu1VM.DichVu1DTO.LoaiDvid)
                {
                    temp += String.Format("- LoaiDvid thay đổi: {0}->{1}", t.LoaiDvid, DichVu1VM.DichVu1DTO.LoaiDvid);
                }

                if (t.NguoiTrinhKy != DichVu1VM.DichVu1DTO.NguoiTrinhKy)
                {
                    temp += String.Format("- NguoiTrinhKy thay đổi: {0}->{1}", t.NguoiTrinhKy, DichVu1VM.DichVu1DTO.NguoiTrinhKy);
                }

                if (t.NgayTrinhKy != DichVu1VM.DichVu1DTO.NgayTrinhKy)
                {
                    temp += String.Format("- NguoiTrinhKy thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", t.NguoiTrinhKy, DichVu1VM.DichVu1DTO.NguoiTrinhKy);
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
                    DichVu1VM.DichVu1DTO.LogFile = t.LogFile;
                }

                try
                {
                    // update
                    var updateDichVu1Result = await _dichVu1Service.UpdateAsync(DichVu1VM.DichVu1DTO);

                    //HotelRoomModel.Details = await QuillHtml.GetHTML();
                    if (!string.IsNullOrEmpty(DichVu1VM.DichVu1DTO.StringImageUrls)) // ko có them anh
                    {
                        DichVu1VM.DichVu1DTO.ImageUrls = JsonConvert.DeserializeObject<List<string>>(DichVu1VM.DichVu1DTO.StringImageUrls);
                        if (DichVu1VM.DichVu1DTO.ImageUrls != null && DichVu1VM.DichVu1DTO.ImageUrls.Any())
                        {
                            // xoa anh dang ton tai
                            await _dichVu1Service.DeleImagesByDichVu1Id(DichVu1VM.DichVu1DTO.MaDv);
                            // add lai list anh moi
                            HinhAnhDTO hinhAnhDTO = new HinhAnhDTO();
                            foreach (var item in DichVu1VM.DichVu1DTO.ImageUrls)
                            {
                                hinhAnhDTO = new HinhAnhDTO()
                                {
                                    DichVuId = DichVu1VM.DichVu1DTO.MaDv,
                                    Url = item
                                };
                                await _dichVu1Service.CreateDichVu1Image(hinhAnhDTO);
                            }
                        }
                    }

                    SetAlert("Cập nhật thành công", "success");

                    return RedirectToAction(nameof(Index), "Supplier", new
                    {
                        id = DichVu1VM.DichVu1DTO.SupplierId,
                        page = DichVu1VM.Page,
                        searchString = DichVu1VM.SearchString
                    });
                }
                catch (Exception ex)
                {
                    SetAlert(ex.Message, "error");

                    return View(DichVu1VM);
                }
            }
            else // not valid
            {
                DichVu1VM.SupplierDTO = await _dichVu1Service.GetSupplierByIdAsync(DichVu1VM.DichVu1DTO.SupplierId);
                DichVu1VM.LoaiDvs = _dichVu1Service.GetAllLoaiDv();
                DichVu1VM.UserDTOs = await _dichVu1Service.GetAllUsers_Intranet();
                return View(DichVu1VM);
            }
        }

        //public DichVu1DTO dichVu1DTO;

        public async Task<IActionResult> ThemMoiDichVu1(string supplierId, string failMessage, DichVu1DTO dichVu1DTO, int page, string searchString)
        {
            DichVu1VM.Page = page;
            DichVu1VM.SearchString = searchString;
            string stringImageUrls = "";
            if (TempData["stringImageUrls"] != null)
            {
                stringImageUrls = (string)TempData["stringImageUrls"];
            }

            //int page = 0;
            if (TempData["page"] != null)
            {
                DichVu1VM.Page = (int)TempData["page"];
            }
            if (!string.IsNullOrEmpty(dichVu1DTO.SupplierId))
            {
                supplierId = dichVu1DTO.SupplierId;
            }
            //dichVu1DTO = TempData["dichVu1DTO"];
            //dichVu1DTO = (DichVu1DTO)dichVu1DTO;
            if (!ModelState.IsValid)
            {
                return View();
            }

            var supplierDTO = await _dichVu1Service.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                //ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return Content("Supplier này không tồn tại.");
            }
            if (!string.IsNullOrEmpty(failMessage))
            {
                ModelState.AddModelError("", failMessage);
            }
            //if (!string.IsNullOrEmpty(dichVu1DTO)) // dichVu1DTO: ThemMoiDichVu1HinhAnh chuyền qua
            if (dichVu1DTO != null) // dichVu1DTO: ThemMoiDichVu1HinhAnh chuyền qua
            {
                DichVu1VM.DichVu1DTO = dichVu1DTO;
                //DichVu1VM.DichVu1DTO = JsonConvert.DeserializeObject<DichVu1DTO>(dichVu1DTO);
            }
            if (!string.IsNullOrEmpty(stringImageUrls)) // dichVu1DTO: ThemMoiDichVu1HinhAnh chuyền qua
            {
                DichVu1VM.DichVu1DTO.StringImageUrls = stringImageUrls;
                DichVu1VM.DichVu1DTO.ImageUrls = JsonConvert.DeserializeObject<List<string>>(stringImageUrls);
            }

            DichVu1VM.SupplierDTO = supplierDTO;
            DichVu1VM.DichVu1DTO.SupplierId = supplierId;
            //DichVu1VM.Vungmiens = await _dichVu1Service.Vungmiens();
            //DichVu1VM.VTinhs = await _dichVu1Service.GetTinhs();
            //DichVu1VM.Thanhpho1s = await _dichVu1Service.GetThanhpho1s();
            DichVu1VM.LoaiSaos = SD.LoaiSao();
            DichVu1VM.LoaiDvs = _dichVu1Service.GetAllLoaiDv();
            DichVu1VM.UserDTOs = await _dichVu1Service.GetAllUsers_Intranet();
            return View(DichVu1VM);
        }

        [HttpPost, ActionName("ThemMoiDichVu1HinhAnh")]
        public async Task<IActionResult> ThemMoiDichVu1HinhAnh(string dichVu1Id)
        {
            // Edit
            if (!string.IsNullOrEmpty(DichVu1VM.DichVu1DTO.MaDv))
            {
                var dichVu1DTO = await _dichVu1Service.GetByIdAsync(DichVu1VM.DichVu1DTO.MaDv);
                DichVu1VM.DichVu1DTO.ImageUrls = dichVu1DTO.HinhAnhDTOs.Select(x => x.Url).ToList();
            }
            if (!string.IsNullOrEmpty(DichVu1VM.DichVu1DTO.StringImageUrls))
            {
                //if (DichVu1VM.DichVu1DTO.ImageUrls.Count > 0)
                //{
                //    DichVu1VM.DichVu1DTO.ImageUrls.AddRange(JsonConvert.DeserializeObject<List<string>>(DichVu1VM.DichVu1DTO.StringImageUrls));
                //}
                //else
                //{
                //    DichVu1VM.DichVu1DTO.ImageUrls = JsonConvert.DeserializeObject<List<string>>(DichVu1VM.DichVu1DTO.StringImageUrls);
                //}
                DichVu1VM.DichVu1DTO.ImageUrls = JsonConvert.DeserializeObject<List<string>>(DichVu1VM.DichVu1DTO.StringImageUrls);
            }
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            // save image
            var files = HttpContext.Request.Form.Files;

            try
            {
                var images = new List<string>();
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        //System.IO.FileInfo fileInfo = new System.IO.FileInfo(file.Name);
                        if (extension.ToLower() == ".jpg" ||
                            extension.ToLower() == ".png" ||
                            extension.ToLower() == ".jpeg")
                        {
                            string UploadImagePath = await _dichVu1Service.UploadFile(file, DichVu1VM.DichVu1DTO.SupplierId);
                            images.Add(UploadImagePath);
                        }
                        else
                        {
                            return RedirectToAction(nameof(ThemMoiDichVu1), new
                            {
                                supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                                page = DichVu1VM.Page,
                                failMessage = "Please select .jpg/ .jpeg/ .png file only"
                            });
                        }
                    }
                    if (DichVu1VM.DichVu1DTO.ImageUrls != null && DichVu1VM.DichVu1DTO.ImageUrls.Any())
                    {
                        DichVu1VM.DichVu1DTO.ImageUrls.AddRange(images); // add image range
                    }
                    else
                    {
                        DichVu1VM.DichVu1DTO.ImageUrls = new List<string>();
                        DichVu1VM.DichVu1DTO.ImageUrls.AddRange(images); // add image range
                    }
                    if (string.IsNullOrEmpty(dichVu1Id)) // them moi
                    {
                        //DichVu1ViewModel dichVu1ViewModel = new DichVu1ViewModel();
                        //dichVu1ViewModel.DichVu1DTO = DichVu1VM.DichVu1DTO;
                        var dichVu1DTO = DichVu1VM.DichVu1DTO;

                        //dichVu1DTO.stringImageUrls = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO.ImageUrls);
                        //dichVu1DTO.supplierId = DichVu1VM.DichVu1DTO.SupplierId;
                        //dichVu1DTO.page = DichVu1VM.Page;

                        //return RedirectToAction(nameof(ThemMoiDichVu1), new
                        //{
                        //    dichVu1DTO = DichVu1VM.DichVu1DTO,
                        //    //dichVu1DTO = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO),
                        //    stringImageUrls = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO.ImageUrls),
                        //    supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                        //    page = DichVu1VM.Page
                        //});

                        TempData["stringImageUrls"] = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO.ImageUrls);
                        TempData["page"] = DichVu1VM.Page;
                        return RedirectToAction("ThemMoiDichVu1", dichVu1DTO);
                    }
                    else // edit
                    {
                        var dichVu1DTO = DichVu1VM.DichVu1DTO;
                        TempData["stringImageUrls"] = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO.ImageUrls);
                        TempData["page"] = DichVu1VM.Page;
                        TempData["id"] = DichVu1VM.DichVu1DTO.MaDv;
                        return RedirectToAction("EditDichVu1", dichVu1DTO);
                        //return RedirectToAction(nameof(EditDichVu1), new
                        //{
                        //    id = DichVu1VM.DichVu1DTO.MaDv,
                        //    //dichVu1DTO = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO),
                        //    //stringImageUrls = JsonConvert.SerializeObject(images),
                        //    stringImageUrls = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO.ImageUrls),
                        //    supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                        //    page = DichVu1VM.Page
                        //});
                    }
                }
                else
                {
                    return RedirectToAction(nameof(ThemMoiDichVu1), new
                    {
                        supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                        page = DichVu1VM.Page,
                        failMessage = "Image uploading failed"
                    });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(ThemMoiDichVu1), new
                {
                    supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                    page = DichVu1VM.Page,
                    failMessage = ex.Message
                });
            }
        }

        [HttpPost, ActionName("ThemMoiDichVu1")]
        public async Task<IActionResult> ThemMoiDichVu1_Post()
        {
            // from login session
            var user = HttpContext.Session.GetSingle<User>("loginUser");

            // save DTO
            //HotelRoomModel.Details = await QuillHtml.GetHTML();
            DichVu1VM.DichVu1DTO.NguoiTao = user.Username;
            DichVu1VM.DichVu1DTO.NgayTao = DateTime.Now;

            // NextID
            var loaiDvDTO = _dichVu1Service.GetAllLoaiDv().Where(x => x.Id == DichVu1VM.DichVu1DTO.LoaiDvid).FirstOrDefault();
            DichVu1VM.DichVu1DTO.MaDv = _dichVu1Service.GetMaDv(loaiDvDTO.MaLoai);

            var dichVu1DTO = await _dichVu1Service.CreateAsync(DichVu1VM.DichVu1DTO);
            // add image
            if (!string.IsNullOrEmpty(DichVu1VM.DichVu1DTO.StringImageUrls))
            {
                HinhAnhDTO hinhAnhDTO = new HinhAnhDTO();
                DichVu1VM.DichVu1DTO.ImageUrls = JsonConvert.DeserializeObject<List<string>>(DichVu1VM.DichVu1DTO.StringImageUrls);
                foreach (var image in DichVu1VM.DichVu1DTO.ImageUrls)
                {
                    // check image exist not duplicate for update HotelRoomModel
                    // HotelRoomModel.HotelRoomImages == null for create new
                    if (DichVu1VM.DichVu1DTO.HinhAnhDTOs == null || DichVu1VM.DichVu1DTO.HinhAnhDTOs.Where(x => x.Url == image).Count() == 0)
                    {
                        hinhAnhDTO = new HinhAnhDTO()
                        {
                            DichVuId = dichVu1DTO.MaDv,
                            Url = image
                        };
                        await _dichVu1Service.CreateDichVu1Image(hinhAnhDTO);
                    }
                }
            }

            SetAlert("Thêm mới thành công.", "success");

            return RedirectToAction(nameof(Index), "Supplier", new
            {
                id = DichVu1VM.DichVu1DTO.SupplierId,
                page = DichVu1VM.Page,
                searchString = DichVu1VM.SearchString
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(string imageUrl, string dichVu1Id)
        {
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            List<string> imageUrls = new List<string>();
            // DichVu1VM.DichVu1DTO.StringImageUrls == null
            // khi xoa hinhanh co san trong edit (khong phai them roi delete di)
            if (string.IsNullOrEmpty(DichVu1VM.DichVu1DTO.StringImageUrls))
            {
                var dichVu1DTO = await _dichVu1Service.GetByIdAsync_AsNoTracking(dichVu1Id);
                imageUrls = dichVu1DTO.HinhAnhDTOs.Select(x => x.Url).ToList();
            }
            else
            {
                imageUrls = JsonConvert.DeserializeObject<List<string>>(DichVu1VM.DichVu1DTO.StringImageUrls);
            }

            try
            {
                var imageIndex = imageUrls.FindIndex(x => x == imageUrl);
                var imageName = imageUrl.Replace($"{baseUrl}/HopDongImages/", "");
                var result = _dichVu1Service.DeleteFile(imageName);
                //if (HotelRoomModel.Id == 0 && Title == "Create")
                //{
                //    var result = FileUpload.DeleteFile(imageName);
                //}
                //else
                //{
                //    // update
                //    DeletedImageNames ??= new List<string>();
                //    DeletedImageNames.Add(imageUrl);
                //}
                //HotelRoomModel.ImageUrls.RemoveAt(imageIndex);
                imageUrls.RemoveAt(imageIndex);

                if (string.IsNullOrEmpty(dichVu1Id)) // them moi
                {
                    var dichVu1DTO = DichVu1VM.DichVu1DTO;
                    TempData["stringImageUrls"] = JsonConvert.SerializeObject(imageUrls);
                    TempData["page"] = DichVu1VM.Page;
                    return RedirectToAction("ThemMoiDichVu1", dichVu1DTO);

                    //return RedirectToAction(nameof(ThemMoiDichVu1), new
                    //{
                    //    dichVu1DTO = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO),
                    //    stringImageUrls = JsonConvert.SerializeObject(imageUrls),
                    //    supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                    //    page = DichVu1VM.Page
                    //});
                }
                else // Edit
                {
                    IEnumerable<HinhAnhDTO> hinhAnhDTOs = await _dichVu1Service.GetHinhanhByDichVu1Id(dichVu1Id);
                    var hinhAnhs = hinhAnhDTOs.Where(x => x.Url == imageUrl);
                    if (hinhAnhs.Count() > 0)
                    {
                        await _dichVu1Service.DeleteHinhanh(hinhAnhs.FirstOrDefault().Id);
                    }

                    var dichVu1DTO = DichVu1VM.DichVu1DTO;
                    TempData["stringImageUrls"] = JsonConvert.SerializeObject(imageUrls);
                    TempData["page"] = DichVu1VM.Page;
                    TempData["id"] = DichVu1VM.DichVu1DTO.MaDv;
                    return RedirectToAction("EditDichVu1", dichVu1DTO);

                    //return RedirectToAction(nameof(EditDichVu1), new
                    //{
                    //    id = DichVu1VM.DichVu1DTO.MaDv,
                    //    dichVu1DTO = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO),
                    //    stringImageUrls = JsonConvert.SerializeObject(imageUrls),
                    //    supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                    //    page = DichVu1VM.Page
                    //});
                }
            }
            catch (Exception ex)
            {
                //await JsRuntime.ToastrError(ex.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Delete(string id)
        {
            try
            {
                var dichVu1DTO = await _dichVu1Service.GetByIdAsync_AsNoTracking(id);

                // xoa file hinh
                foreach (var image in dichVu1DTO.HinhAnhDTOs)
                {
                    await _dichVu1Service.DeleteImageFile(image.Url);
                }

                // xoa hinhanh
                foreach (var image in dichVu1DTO.HinhAnhDTOs)
                {
                    await _dichVu1Service.DeleteHinhanh(image.Id);
                }

                var dichVu1DTO1 = _dichVu1Service.GetByIdAsNoTracking(id);
                await _dichVu1Service.Delete(dichVu1DTO1);

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
            //return Json(false);
        }

        public async Task<IActionResult> GetAnhHDByDVId(string dichvu1Id, int page = 1)
        {
            DichVu1VM.Dichvu1Id = dichvu1Id;
            DichVu1VM.DichVu1DTO = await _dichVu1Service.GetByIdAsync(dichvu1Id);
            DichVu1VM.HinhAnhDTOs = await _dichVu1Service.GetAnhHDByDVId_PagedList(dichvu1Id, page);
            return PartialView(DichVu1VM);
        }
    }
}