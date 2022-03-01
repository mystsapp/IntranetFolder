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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DichVu1Partial(string supplierId, int page)
        {
            var supplierDTO = await _dichVu1Service.GetSupplierByIdAsync(supplierId);
            if (supplierDTO == null)
            {
                //ViewBag.ErrorMessage = "Supplier này không tồn tại.";
                return Content("Supplier này không tồn tại.");
            }
            DichVu1VM.Page = page;
            DichVu1VM.SupplierDTO = supplierDTO;
            DichVu1VM.DichVu1DTOs = await _dichVu1Service.GetDichVu1By_SupplierId(supplierId);

            return PartialView(DichVu1VM);
        }

        public async Task<IActionResult> EditDichVu1(string id, string supplierId, int page, string failMessage, /*string dichVu1DTO,*/ string stringImageUrls)
        {
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
            //if (dichVu1DTO != null) // dichVu1DTO: ThemMoiDichVu1HinhAnh chuyền qua
            //{
            //    DichVu1VM.DichVu1DTO = dichVu1DTO;
            //    //DichVu1VM.DichVu1DTO = JsonConvert.DeserializeObject<DichVu1DTO>(dichVu1DTO);
            //}
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
            DichVu1VM.Page = page;
            DichVu1VM.SupplierDTO = supplierDTO;
            //DichVu1VM.DichVu1DTO.SupplierId = supplierId;
            //DichVu1VM.Vungmiens = await _dichVu1Service.Vungmiens();
            //DichVu1VM.VTinhs = await _dichVu1Service.GetTinhs();
            //DichVu1VM.Thanhpho1s = await _dichVu1Service.GetThanhpho1s();
            DichVu1VM.LoaiSaos = SD.LoaiSao();
            DichVu1VM.LoaiDvs = _dichVu1Service.GetAllLoaiDv();
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

                if (t.TenDv != DichVu1VM.DichVu1DTO.TenDv)
                {
                    temp += String.Format("- TenDv thay đổi: {0}->{1}", t.TenDv, DichVu1VM.DichVu1DTO.TenDv);
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
                    temp += String.Format("- LoaiDvid thay đổi: {0}->{1}", t.Website, DichVu1VM.DichVu1DTO.LoaiDvid);
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
                    await _dichVu1Service.UpdateAsync(DichVu1VM.DichVu1DTO);

                    //// update
                    ////HotelRoomModel.Details = await QuillHtml.GetHTML();
                    //var updateRoomResult = await HotelRoomRepository.UpdateHotelRoom(HotelRoomModel.Id, HotelRoomModel);
                    //if (HotelRoomModel.ImageUrls != null && HotelRoomModel.ImageUrls.Any())
                    //{
                    //    foreach (var deletedImageName in DeletedImageNames)
                    //    {
                    //        var imageName = deletedImageName.Replace($"{NavigationManager.BaseUri}RoomImages/", "");

                    //        var result = FileUpload.DeleteFile(imageName);
                    //        await HotelImagesRepository.DeleteHotelImageByImageUrl(deletedImageName);
                    //    }
                    //    await AddHotelRoomImage(updateRoomResult);
                    //}
                    //await JsRuntime.ToastrSuccess("Hotel room updated successfully.");

                    SetAlert("Cập nhật thành công", "success");

                    return RedirectToAction(nameof(Index), "Supplier", new
                    {
                        id = DichVu1VM.DichVu1DTO.SupplierId,
                        page = DichVu1VM.Page
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
                return View(DichVu1VM);
            }
        }

        //public DichVu1DTO dichVu1DTO;

        public async Task<IActionResult> ThemMoiDichVu1(string supplierId, int page, string failMessage, String dichVu1DTO, string stringImageUrls)
        {
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
            if (!string.IsNullOrEmpty(dichVu1DTO)) // dichVu1DTO: ThemMoiDichVu1HinhAnh chuyền qua
            {
                DichVu1VM.DichVu1DTO = JsonConvert.DeserializeObject<DichVu1DTO>(dichVu1DTO);
            }
            if (!string.IsNullOrEmpty(stringImageUrls)) // dichVu1DTO: ThemMoiDichVu1HinhAnh chuyền qua
            {
                DichVu1VM.DichVu1DTO.StringImageUrls = stringImageUrls;
                DichVu1VM.DichVu1DTO.ImageUrls = JsonConvert.DeserializeObject<List<string>>(stringImageUrls);
            }
            DichVu1VM.Page = page;
            DichVu1VM.SupplierDTO = supplierDTO;
            DichVu1VM.DichVu1DTO.SupplierId = supplierId;
            //DichVu1VM.Vungmiens = await _dichVu1Service.Vungmiens();
            //DichVu1VM.VTinhs = await _dichVu1Service.GetTinhs();
            //DichVu1VM.Thanhpho1s = await _dichVu1Service.GetThanhpho1s();
            DichVu1VM.LoaiSaos = SD.LoaiSao();
            DichVu1VM.LoaiDvs = _dichVu1Service.GetAllLoaiDv();
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
                        return RedirectToAction(nameof(ThemMoiDichVu1), new
                        {
                            dichVu1DTO = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO),
                            stringImageUrls = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO.ImageUrls),
                            supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                            page = DichVu1VM.Page
                        });
                    }
                    else // edit
                    {
                        return RedirectToAction(nameof(EditDichVu1), new
                        {
                            id = DichVu1VM.DichVu1DTO.MaDv,
                            //dichVu1DTO = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO),
                            //stringImageUrls = JsonConvert.SerializeObject(images),
                            stringImageUrls = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO.ImageUrls),
                            supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                            page = DichVu1VM.Page
                        });
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
            SetAlert("Thêm mới thành công.", "success");

            return RedirectToAction(nameof(Index), "Supplier", new
            {
                id = DichVu1VM.DichVu1DTO.SupplierId,
                page = DichVu1VM.Page
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(string imageUrl, string dichVu1Id)
        {
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            List<string> imageUrls = JsonConvert.DeserializeObject<List<string>>(DichVu1VM.DichVu1DTO.StringImageUrls);
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
                    return RedirectToAction(nameof(ThemMoiDichVu1), new
                    {
                        dichVu1DTO = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO),
                        stringImageUrls = JsonConvert.SerializeObject(imageUrls),
                        supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                        page = DichVu1VM.Page
                    });
                }
                else // Edit
                {
                    return RedirectToAction(nameof(EditDichVu1), new
                    {
                        id = DichVu1VM.DichVu1DTO.MaDv,
                        dichVu1DTO = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO),
                        stringImageUrls = JsonConvert.SerializeObject(imageUrls),
                        supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                        page = DichVu1VM.Page
                    });
                }
            }
            catch (Exception ex)
            {
                //await JsRuntime.ToastrError(ex.Message);
            }
            return View();
        }
    }
}