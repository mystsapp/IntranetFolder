using Common;
using Data.Models;
using Data.Utilities;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> ThemMoiDichVu1(string supplierId, int page, string failMessage, string dichVu1DTO, string stringImageUrls)
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
        public async Task<IActionResult> ThemMoiDichVu1HinhAnh()
        {
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
                            string UploadImagePath = await _dichVu1Service.UploadFile(file);
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

                    return RedirectToAction(nameof(ThemMoiDichVu1), new
                    {
                        dichVu1DTO = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO),
                        stringImageUrls = JsonConvert.SerializeObject(DichVu1VM.DichVu1DTO.ImageUrls),
                        supplierId = DichVu1VM.DichVu1DTO.SupplierId,
                        page = DichVu1VM.Page
                    });
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
            SetAlert("", "Hotel room created successfully.");

            return RedirectToAction(nameof(Index), "Supplier", new
            {
                id = DichVu1VM.DichVu1DTO.SupplierId,
                page = DichVu1VM.Page
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(string imageUrl)
        {
            List<string> imageUrls = JsonConvert.DeserializeObject<List<string>>(DichVu1VM.DichVu1DTO.StringImageUrls);
            try
            {
                var imageIndex = imageUrls.FindIndex(x => x == imageUrl);
                var imageName = imageUrl.Replace($"{NavigationManager.BaseUri}RoomImages/", "");
                if (HotelRoomModel.Id == 0 && Title == "Create")
                {
                    var result = FileUpload.DeleteFile(imageName);
                }
                else
                {
                    // update
                    DeletedImageNames ??= new List<string>();
                    DeletedImageNames.Add(imageUrl);
                }
                HotelRoomModel.ImageUrls.RemoveAt(imageIndex);
            }
            catch (Exception ex)
            {
                await JsRuntime.ToastrError(ex.Message);
            }
            return View();
        }
    }
}