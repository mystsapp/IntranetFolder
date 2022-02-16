using AutoMapper;
using Data.Models;
using Data.Repository;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace IntranetFolder.Services
{
    public interface IDanhGiaVanChuyenService
    {
        IEnumerable<DanhGiaVanChuyenDTO> GetAll();

        Task<DanhGiaVanChuyenDTO> GetByIdAsync(long id);

        Task<DanhGiaVanChuyenDTO> CreateAsync(DanhGiaVanChuyenDTO danhGiaVanChuyenDTO);

        Task<DanhGiaVanChuyenDTO> UpdateAsync(DanhGiaVanChuyenDTO danhGiaVanChuyenDTO);

        Task Delete(DanhGiaVanChuyenDTO danhGiaVanChuyenDTO);

        DanhGiaVanChuyenDTO GetByIdAsNoTracking(long id);

        Task<IPagedList<DanhGiaVanChuyenDTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page);

        //Task<SupplierDTO> GetBySupplierByIdAsync(string supplierId);

        IEnumerable<LoaiDvDTO> GetAllLoaiDv();

        Task<bool> CheckNameExist(long id, string name);

        Task<SupplierDTO> GetSupplierByIdAsync(string supplierId);

        Task<ErrorLog> CreateErroLogAsync(ErrorLog errorLog);

        Task<IEnumerable<DanhGiaVanChuyenDTO>> GetDanhGiaVanChuyenBy_SupplierId(string supplierId);
    }

    public class DanhGiaVanChuyenService : IDanhGiaVanChuyenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DanhGiaVanChuyenService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DanhGiaVanChuyenDTO> GetByIdAsync(long id)
        {
            return _mapper.Map<DanhGiaVanChuyen, DanhGiaVanChuyenDTO>(await _unitOfWork.danhGiaVanChuyenRepository.GetByLongIdAsync(id));
        }

        public IEnumerable<DanhGiaVanChuyenDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<DanhGiaVanChuyen>, IEnumerable<DanhGiaVanChuyenDTO>>(_unitOfWork.danhGiaVanChuyenRepository.GetAll());
        }

        public async Task<DanhGiaVanChuyenDTO> CreateAsync(DanhGiaVanChuyenDTO danhGiaVanChuyenDTO)
        {
            DanhGiaVanChuyen danhGiaVanChuyen = _mapper.Map<DanhGiaVanChuyenDTO, DanhGiaVanChuyen>(danhGiaVanChuyenDTO);
            var danhGiaVanChuyen1 = await _unitOfWork.danhGiaVanChuyenRepository.CreateAsync(danhGiaVanChuyen);
            return _mapper.Map<DanhGiaVanChuyen, DanhGiaVanChuyenDTO>(danhGiaVanChuyen1);
        }

        public async Task<DanhGiaVanChuyenDTO> UpdateAsync(DanhGiaVanChuyenDTO danhGiaVanChuyenDTO)
        {
            DanhGiaVanChuyen danhGiaVanChuyen = _mapper.Map<DanhGiaVanChuyenDTO, DanhGiaVanChuyen>(danhGiaVanChuyenDTO);
            var danhGiaVanChuyen1 = await _unitOfWork.danhGiaVanChuyenRepository.UpdateAsync(danhGiaVanChuyen);
            return _mapper.Map<DanhGiaVanChuyen, DanhGiaVanChuyenDTO>(danhGiaVanChuyen1);
        }

        public async Task Delete(DanhGiaVanChuyenDTO danhGiaVanChuyenDTO)
        {
            DanhGiaVanChuyen danhGiaVanChuyen = _mapper.Map<DanhGiaVanChuyenDTO, DanhGiaVanChuyen>(danhGiaVanChuyenDTO);
            _unitOfWork.danhGiaVanChuyenRepository.Delete(danhGiaVanChuyen);
            await _unitOfWork.Complete();
        }

        public DanhGiaVanChuyenDTO GetByIdAsNoTracking(long id)
        {
            return _mapper.Map<DanhGiaVanChuyen, DanhGiaVanChuyenDTO>(_unitOfWork.danhGiaVanChuyenRepository.GetByIdAsNoTracking(x => x.Id == id));
        }

        public async Task<IPagedList<DanhGiaVanChuyenDTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            List<DanhGiaVanChuyenDTO> list = new List<DanhGiaVanChuyenDTO>();
            List<DanhGiaVanChuyen> danhGiaNcus1 = new List<DanhGiaVanChuyen>();

            if (!string.IsNullOrEmpty(searchString))
            {
                var danhGiaNcus = await _unitOfWork.danhGiaVanChuyenRepository.FindAsync(x => x.TenNcu.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.DiaChi) && x.DiaChi.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.DienThoai) && x.DienThoai.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(searchString.ToLower())));
                danhGiaNcus1 = danhGiaNcus.ToList();
            }
            else
            {
                danhGiaNcus1 = _unitOfWork.danhGiaVanChuyenRepository.GetAll().ToList();

                if (danhGiaNcus1 == null)
                {
                    return null;
                }
            }

            //danhGiaNcus1 = danhGiaNcus1.OrderByDescending(x => x.Ngay).ToList();

            //list = _mapper.Map<List<DanhGiaVanChuyen>, List<DanhGiaVanChuyenDTO>>(danhGiaNcus1);

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

        public async Task<bool> CheckNameExist(long id, string name)
        {
            var danhGiaVanChuyens = await _unitOfWork.danhGiaVanChuyenRepository
                .FindAsync(x => x.TenNcu.Trim().ToLower() == name.Trim().ToLower());

            if (danhGiaVanChuyens.Count() > 0)
            {
                string findName = danhGiaVanChuyens.FirstOrDefault().TenNcu;
                long findId = danhGiaVanChuyens.FirstOrDefault().Id;

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

        public async Task<IEnumerable<DanhGiaVanChuyenDTO>> GetDanhGiaVanChuyenBy_SupplierId(string supplierId)
        {
            return _mapper.Map<IEnumerable<DanhGiaVanChuyen>, IEnumerable<DanhGiaVanChuyenDTO>>
                (await _unitOfWork.danhGiaVanChuyenRepository.FindIncludeOneAsync(x => x.Supplier, y => y.SupplierId == supplierId));
        }
    }
}