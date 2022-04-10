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
    public interface IDanhGiaLandTourService
    {
        IEnumerable<DanhGiaLandTourDTO> GetAll();

        Task<DanhGiaLandTourDTO> GetByIdAsync(long id);

        Task<DanhGiaLandTourDTO> CreateAsync(DanhGiaLandTourDTO danhGiaLandTourDTO);

        Task<DanhGiaLandTourDTO> UpdateAsync(DanhGiaLandTourDTO danhGiaLandTourDTO);

        Task Delete(DanhGiaLandTourDTO danhGiaLandTourDTO);

        DanhGiaLandTourDTO GetByIdAsNoTracking(long id);

        Task<IPagedList<DanhGiaLandTourDTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page);

        //Task<SupplierDTO> GetBySupplierByIdAsync(string supplierId);

        IEnumerable<LoaiDvDTO> GetAllLoaiDv();

        Task<bool> CheckNameExist(long id, string name);

        Task<SupplierDTO> GetSupplierByIdAsync(string supplierId);

        Task<ErrorLog> CreateErroLogAsync(ErrorLog errorLog);

        Task<IEnumerable<DanhGiaLandTourDTO>> GetDanhGiaLandTourBy_SupplierId(string supplierId);

        Task<TapDoanDTO> GetTapDoanByIdAsync(int tapDoanId);
    }

    public class DanhGiaLandTourService : IDanhGiaLandTourService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DanhGiaLandTourService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DanhGiaLandTourDTO> GetByIdAsync(long id)
        {
            return _mapper.Map<DanhGiaLandtour, DanhGiaLandTourDTO>(await _unitOfWork.danhGiaLandTourRepository.GetByLongIdAsync(id));
        }

        public IEnumerable<DanhGiaLandTourDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<DanhGiaLandtour>, IEnumerable<DanhGiaLandTourDTO>>(_unitOfWork.danhGiaLandTourRepository.GetAll());
        }

        public async Task<DanhGiaLandTourDTO> CreateAsync(DanhGiaLandTourDTO danhGiaLandTourDTO)
        {
            DanhGiaLandtour danhGiaLandTour = _mapper.Map<DanhGiaLandTourDTO, DanhGiaLandtour>(danhGiaLandTourDTO);
            var danhGiaLandTour1 = await _unitOfWork.danhGiaLandTourRepository.CreateAsync(danhGiaLandTour);
            return _mapper.Map<DanhGiaLandtour, DanhGiaLandTourDTO>(danhGiaLandTour1);
        }

        public async Task<DanhGiaLandTourDTO> UpdateAsync(DanhGiaLandTourDTO danhGiaLandTourDTO)
        {
            DanhGiaLandtour danhGiaLandTour = _mapper.Map<DanhGiaLandTourDTO, DanhGiaLandtour>(danhGiaLandTourDTO);
            var danhGiaLandTour1 = await _unitOfWork.danhGiaLandTourRepository.UpdateAsync(danhGiaLandTour);
            return _mapper.Map<DanhGiaLandtour, DanhGiaLandTourDTO>(danhGiaLandTour1);
        }

        public async Task Delete(DanhGiaLandTourDTO danhGiaLandTourDTO)
        {
            DanhGiaLandtour danhGiaLandTour = _mapper.Map<DanhGiaLandTourDTO, DanhGiaLandtour>(danhGiaLandTourDTO);
            _unitOfWork.danhGiaLandTourRepository.Delete(danhGiaLandTour);
            await _unitOfWork.Complete();
        }

        public DanhGiaLandTourDTO GetByIdAsNoTracking(long id)
        {
            return _mapper.Map<DanhGiaLandtour, DanhGiaLandTourDTO>(_unitOfWork.danhGiaLandTourRepository.GetByIdAsNoTracking(x => x.Id == id));
        }

        public async Task<IPagedList<DanhGiaLandTourDTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            List<DanhGiaLandTourDTO> list = new List<DanhGiaLandTourDTO>();
            List<DanhGiaLandtour> danhGiaNcus1 = new List<DanhGiaLandtour>();

            if (!string.IsNullOrEmpty(searchString))
            {
                var danhGiaNcus = await _unitOfWork.danhGiaLandTourRepository.FindAsync(x => x.TenNcu.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.DiaChi) && x.DiaChi.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.DienThoai) && x.DienThoai.ToLower().Contains(searchString.ToLower())));
                danhGiaNcus1 = danhGiaNcus.ToList();
            }
            else
            {
                danhGiaNcus1 = _unitOfWork.danhGiaLandTourRepository.GetAll().ToList();

                if (danhGiaNcus1 == null)
                {
                    return null;
                }
            }

            danhGiaNcus1 = danhGiaNcus1.OrderByDescending(x => x.NgayTao).ToList();

            list = _mapper.Map<List<DanhGiaLandtour>, List<DanhGiaLandTourDTO>>(danhGiaNcus1);

            // search date
            DateTime fromDate, toDate;
            if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
            {
                try
                {
                    fromDate = DateTime.Parse(searchFromDate); // NgayCT
                    toDate = DateTime.Parse(searchToDate); // NgayCT

                    if (fromDate > toDate)
                    {
                        return null; //
                    }

                    list = list.Where(x => x.NgayTao >= fromDate &&
                                       x.NgayTao < toDate.AddDays(1)).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchFromDate)) // NgayCT
                {
                    try
                    {
                        fromDate = DateTime.Parse(searchFromDate);
                        list = list.Where(x => x.NgayTao >= fromDate).ToList();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                if (!string.IsNullOrEmpty(searchToDate)) // NgayCT
                {
                    try
                    {
                        toDate = DateTime.Parse(searchToDate);
                        list = list.Where(x => x.NgayTao < toDate.AddDays(1)).ToList();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            // search date

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
            var DanhGiaLandTours = await _unitOfWork.danhGiaLandTourRepository
                .FindAsync(x => x.TenNcu.Trim().ToLower() == name.Trim().ToLower());

            if (DanhGiaLandTours.Count() > 0)
            {
                string findName = DanhGiaLandTours.FirstOrDefault().TenNcu;
                long findId = DanhGiaLandTours.FirstOrDefault().Id;

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

        public async Task<IEnumerable<DanhGiaLandTourDTO>> GetDanhGiaLandTourBy_SupplierId(string supplierId)
        {
            return _mapper.Map<IEnumerable<DanhGiaLandtour>, IEnumerable<DanhGiaLandTourDTO>>
                (await _unitOfWork.danhGiaLandTourRepository.FindIncludeOneAsync(x => x.Supplier, y => y.SupplierId == supplierId));
        }

        public async Task<TapDoanDTO> GetTapDoanByIdAsync(int tapDoanId)
        {
            return _mapper.Map<TapDoan, TapDoanDTO>(_unitOfWork.tapDoanRepository.GetById(tapDoanId));
        }
    }
}