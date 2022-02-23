using Common;
using IntranetFolder.Models;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public async Task<IActionResult> ThemMoiDichVu1(string supplierId, int page)
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

        [HttpPost, ActionName("ThemMoiDichVu1")]
        public async Task<IActionResult> ThemMoiDichVu1_Post(string supplierId, int page)
        {
            var files = HttpContext.Request.Form.Files;

            try
            {
                var images = new List<string>();
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(file.Name);
                        if (fileInfo.Extension.ToLower() == ".jpg" ||
                            fileInfo.Extension.ToLower() == ".png" ||
                            fileInfo.Extension.ToLower() == ".jpeg")
                        {
                            string UploadImagePath = await _dichVu1Service.UploadFile(file);
                            images.Add(UploadImagePath);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Please select .jpg/ .jpeg/ .png file only");
                            DichVu1VM.SupplierDTO = await _dichVu1Service.GetSupplierByIdAsync(supplierId);
                            DichVu1VM.DichVu1DTO.SupplierId = supplierId;
                            DichVu1VM.LoaiSaos = SD.LoaiSao();
                            DichVu1VM.LoaiDvs = _dichVu1Service.GetAllLoaiDv();
                            return View(DichVu1VM);
                        }
                    }
                    if (DichVu1VM.DichVu1DTO.ImageUrls != null && DichVu1VM.DichVu1DTO.ImageUrls.Any())
                    {
                        DichVu1VM.DichVu1DTO.ImageUrls.AddRange(images);
                    }
                    else
                    {
                        DichVu1VM.DichVu1DTO.ImageUrls = new List<string>();
                        DichVu1VM.DichVu1DTO.ImageUrls.AddRange(images);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Image uploading failed");
                    DichVu1VM.SupplierDTO = await _dichVu1Service.GetSupplierByIdAsync(supplierId);
                    DichVu1VM.DichVu1DTO.SupplierId = supplierId;
                    DichVu1VM.LoaiSaos = SD.LoaiSao();
                    DichVu1VM.LoaiDvs = _dichVu1Service.GetAllLoaiDv();
                    return View(DichVu1VM);
                }
            }
            catch (Exception ex)
            {
                SetAlert("error", ex.Message);
            }

            // create
            //HotelRoomModel.Details = await QuillHtml.GetHTML();
            var dichVu1DTO = await _dichVu1Service.CreateAsync(DichVu1VM.DichVu1DTO);
            // add image
            foreach (var image in HotelRoomModel.ImageUrls)
            {
                // check image exist not duplicate for update HotelRoomModel
                // HotelRoomModel.HotelRoomImages == null for create new
                if (HotelRoomModel.HotelRoomImages == null || HotelRoomModel.HotelRoomImages.Where(x => x.RoomImageUrl == image).Count() == 0)
                {
                    RoomImage = new HotelRoomImageDTO()
                    {
                        RoomId = roomDetails.Id,
                        RoomImageUrl = image
                    };
                    await HotelImagesRepository.CreateHotelRoomImage(RoomImage);
                }
            }
            SetAlert("", "Hotel room created successfully.");

            return View();
        }
    }
}