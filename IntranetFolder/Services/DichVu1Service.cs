using AutoMapper;
using Data.Models;
using Data.Repository;
using Data.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace IntranetFolder.Services
{
    public interface IDichVu1Service
    {
        IEnumerable<DichVu1DTO> GetAll();

        Task<DichVu1DTO> GetByIdAsync(long id);

        Task<DichVu1DTO> CreateAsync(DichVu1DTO dichVu1DTO);

        Task<DichVu1DTO> UpdateAsync(DichVu1DTO ichVu1DTO);

        Task Delete(DichVu1DTO dichVu1DTO);

        DichVu1DTO GetByIdAsNoTracking(string id);

        Task<IPagedList<DichVu1DTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page);

        //Task<SupplierDTO> GetBySupplierByIdAsync(string supplierId);

        IEnumerable<LoaiDvDTO> GetAllLoaiDv();

        Task<bool> CheckNameExist(string id, string name);

        Task<SupplierDTO> GetSupplierByIdAsync(string supplierId);

        Task<ErrorLog> CreateErroLogAsync(ErrorLog errorLog);

        Task<IEnumerable<DichVu1DTO>> GetDichVu1By_SupplierId(string supplierId);

        Task<IEnumerable<Vungmien>> Vungmiens();

        Task<IEnumerable<VTinh>> GetTinhs();

        Task<IEnumerable<Thanhpho1>> GetThanhpho1s();

        Task<string> UploadFile(IFormFile file);

        bool DeleteFile(string fileName);

        public Task<int> CreateDichVu1Image(HinhAnhDTO imageDTO);

        string GetMaDv(string param);
    }

    public class DichVu1Service : IDichVu1Service
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DichVu1Service(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DichVu1DTO> GetByIdAsync(long id)
        {
            return _mapper.Map<DichVu1, DichVu1DTO>(await _unitOfWork.dichVu1Repository.GetByLongIdAsync(id));
        }

        public IEnumerable<DichVu1DTO> GetAll()
        {
            return _mapper.Map<IEnumerable<DichVu1>, IEnumerable<DichVu1DTO>>(_unitOfWork.dichVu1Repository.GetAll());
        }

        public async Task<DichVu1DTO> CreateAsync(DichVu1DTO dichVu1DTO)
        {
            DichVu1 dichVu1 = _mapper.Map<DichVu1DTO, DichVu1>(dichVu1DTO);
            var dichVu11 = await _unitOfWork.dichVu1Repository.CreateAsync(dichVu1);
            return _mapper.Map<DichVu1, DichVu1DTO>(dichVu11);
        }

        public async Task<DichVu1DTO> UpdateAsync(DichVu1DTO dichVu1DTO)
        {
            DichVu1 dichVu1 = _mapper.Map<DichVu1DTO, DichVu1>(dichVu1DTO);
            var dichVu11 = await _unitOfWork.dichVu1Repository.UpdateAsync(dichVu1);
            return _mapper.Map<DichVu1, DichVu1DTO>(dichVu11);
        }

        public async Task Delete(DichVu1DTO dichVu1DTO)
        {
            DichVu1 dichVu1 = _mapper.Map<DichVu1DTO, DichVu1>(dichVu1DTO);
            _unitOfWork.dichVu1Repository.Delete(dichVu1);
            await _unitOfWork.Complete();
        }

        public DichVu1DTO GetByIdAsNoTracking(string id)
        {
            return _mapper.Map<DichVu1, DichVu1DTO>(_unitOfWork.dichVu1Repository.GetByIdAsNoTracking(x => x.MaDv == id));
        }

        public async Task<IPagedList<DichVu1DTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            List<DichVu1DTO> list = new List<DichVu1DTO>();
            List<DichVu1> danhGiaNcus1 = new List<DichVu1>();

            if (!string.IsNullOrEmpty(searchString))
            {
                var danhGiaNcus = await _unitOfWork.dichVu1Repository.FindAsync(x => x.TenDv.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.DiaChi) && x.DiaChi.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.DienThoai) && x.DienThoai.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(searchString.ToLower())));
                danhGiaNcus1 = danhGiaNcus.ToList();
            }
            else
            {
                danhGiaNcus1 = _unitOfWork.dichVu1Repository.GetAll().ToList();

                if (danhGiaNcus1 == null)
                {
                    return null;
                }
            }

            //danhGiaNcus1 = danhGiaNcus1.OrderByDescending(x => x.Ngay).ToList();

            //list = _mapper.Map<List<DichVu1>, List<DichVu1DTO>>(danhGiaNcus1);

            //// search date
            //DateTime fromDate, toDate;
            //if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
            //{
            //    try
            //    {
            //        fromDate = DateTime.Parse(searchFromDate); // NgayCT
            //        toDate = DateTime.Parse(searchToDate); // NgayCT

            //        if (fromDate > toDate)
            //        {
            //            return null; //
            //        }

            //        list = list.Where(x => x.NgayTao >= fromDate &&
            //                           x.NgayTao < toDate.AddDays(1)).ToList();
            //    }
            //    catch (Exception)
            //    {
            //        return null;
            //    }
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(searchFromDate)) // NgayCT
            //    {
            //        try
            //        {
            //            fromDate = DateTime.Parse(searchFromDate);
            //            list = list.Where(x => x.NgayTao >= fromDate).ToList();
            //        }
            //        catch (Exception)
            //        {
            //            return null;
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(searchToDate)) // NgayCT
            //    {
            //        try
            //        {
            //            toDate = DateTime.Parse(searchToDate);
            //            list = list.Where(x => x.NgayTao < toDate.AddDays(1)).ToList();
            //        }
            //        catch (Exception)
            //        {
            //            return null;
            //        }
            //    }
            //}
            //// search date

            //// List<string> listRoleChiNhanh --> chi lay nhung tour thuộc phanKhuCN cua minh
            //if (listRoleChiNhanh.Count > 0)
            //{
            //    list = list.Where(item1 => listRoleChiNhanh.Any(item2 => item1.MaCNTao == item2)).ToList();
            //}
            //// List<string> listRoleChiNhanh --> chi lay nhung tour thuộc phanKhuCN cua minh

            // page the list
            const int pageSize = 10;
            decimal aa = (decimal)list.Count() / (decimal)pageSize;
            var bb = Math.Ceiling(aa);
            if (page > bb)
            {
                page--;
            }
            page = (page == 0) ? 1 : page;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);
            //if (page > listPaged.PageCount)
            //    page--;
            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        public IEnumerable<LoaiDvDTO> GetAllLoaiDv()
        {
            return _mapper.Map<IEnumerable<LoaiDv>, IEnumerable<LoaiDvDTO>>(_unitOfWork.loaiDvRepository.GetAll());
        }

        public async Task<bool> CheckNameExist(string id, string name)
        {
            var DichVu1s = await _unitOfWork.dichVu1Repository
                .FindAsync(x => x.TenDv.Trim().ToLower() == name.Trim().ToLower());

            if (DichVu1s.Count() > 0)
            {
                string findName = DichVu1s.FirstOrDefault().TenDv;
                string findId = DichVu1s.FirstOrDefault().MaDv;

                if (findId != id)
                    return false;
                else return true;
            }

            return true;
        }

        public async Task<SupplierDTO> GetSupplierByIdAsync(string supplierId)
        {
            return _mapper.Map<Supplier, SupplierDTO>(await _unitOfWork.supplierRepository.GetByIdAsync(supplierId));
        }

        public async Task<ErrorLog> CreateErroLogAsync(ErrorLog errorLog)
        {
            return await _unitOfWork.errorRepository.CreateAsync(errorLog);
        }

        //public async Task<SupplierDTO> GetBySupplierByIdAsync(string supplierId)
        //{
        //    return _mapper.Map<Supplier, SupplierDTO>(await _unitOfWork.supplierRepository.GetByIdAsync(supplierId));
        //}

        public async Task<IEnumerable<DichVu1DTO>> GetDichVu1By_SupplierId(string supplierId)
        {
            return _mapper.Map<IEnumerable<DichVu1>, IEnumerable<DichVu1DTO>>
                (await _unitOfWork.dichVu1Repository.FindIncludeOneAsync(x => x.Supplier, y => y.SupplierId == supplierId));
        }

        public async Task<IEnumerable<Vungmien>> Vungmiens()
        {
            return await _unitOfWork.vungMienRepository.Vungmiens();
        }

        public async Task<IEnumerable<VTinh>> GetTinhs()
        {
            return await _unitOfWork.supplierRepository.GetTinhs();
        }

        public async Task<IEnumerable<Thanhpho1>> GetThanhpho1s()
        {
            return await _unitOfWork.supplierRepository.GetThanhpho1s();
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                //FileInfo fileInfo = new FileInfo(file.Name); // file detail
                var extension = Path.GetExtension(file.FileName);
                var fileName = Guid.NewGuid().ToString() + extension;
                var folderDirectory = $"{_webHostEnvironment.WebRootPath}\\HopDongImages";
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "HopDongImages", fileName); // RoomImages

                var memoryStream = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(memoryStream);

                if (!Directory.Exists(folderDirectory))
                {
                    Directory.CreateDirectory(folderDirectory);
                }

                await using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fs);
                }
                var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/";
                var fullPath = $"{url}HopDongImages/{fileName}";
                return fullPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateDichVu1Image(HinhAnhDTO imageDTO)
        {
            var image = _mapper.Map<HinhAnhDTO, HinhAnh>(imageDTO);
            _unitOfWork.hinhAnhRepository.Create(image);
            return await _unitOfWork.Complete();
        }

        public string GetMaDv(string param) // param: loaiDv
        {
            //DateTime dateTime;
            //dateTime = DateTime.Now;
            //dateTime = TourVM.Tour.NgayKyHopDong.Value;

            var currentYear = DateTime.Now.Year; // name hien tai
            var subfix = param + currentYear.ToString(); // QT2021? ?QC2021? ?NT2021? ?NC2021?
            var dichVu1s = _unitOfWork.dichVu1Repository
                                   .Find(x => x.MaDv.Trim().Contains(subfix)).ToList();// chi lay nhung MaDv cung param + nam
            var dichVu1 = new DichVu1();
            if (dichVu1s.Count() > 0)
            {
                dichVu1 = dichVu1s.OrderByDescending(x => x.MaDv).FirstOrDefault();
            }

            if (dichVu1 == null || string.IsNullOrEmpty(dichVu1.MaDv))
            {
                return GetNextId.NextID("", "") + subfix; // 0001 + subfix : 0001SSE2022
            }
            else
            {
                var oldYear = dichVu1.MaDv.Substring(7, 4);

                // cung nam
                if (oldYear == currentYear.ToString())
                {
                    var oldSoCT = dichVu1.MaDv.Substring(0, 4);
                    return GetNextId.NextID(oldSoCT, "") + subfix;
                }
                else
                {
                    // sang nam khac' chay lai tu dau
                    return GetNextId.NextID("", "") + subfix; // 0001 + subfix
                }
            }
        }

        public bool DeleteFile(string fileName)
        {
            try
            {
                var path = $"{_webHostEnvironment.WebRootPath}\\HopDongImages\\{fileName}";
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}