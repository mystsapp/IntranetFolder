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
    public interface IDanhGiaNhaHangService
    {
        IEnumerable<DanhGiaNhaHangDTO> GetAll();

        Task<DanhGiaNhaHangDTO> GetByIdAsync(long id);

        Task<DanhGiaNhaHangDTO> CreateAsync(DanhGiaNhaHangDTO danhGiaNhaHangDTO);

        Task<DanhGiaNhaHangDTO> UpdateAsync(DanhGiaNhaHangDTO danhGiaNhaHangDTO);

        Task Delete(DanhGiaNhaHangDTO danhGiaNhaHangDTO);

        DanhGiaNhaHangDTO GetByIdAsNoTracking(long id);

        Task<IPagedList<DanhGiaNhaHangDTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page);

        IEnumerable<LoaiDvDTO> GetAllLoaiDv();

        Task<bool> CheckNameExist(long id, string name);

        Task<SupplierDTO> GetSupplierByIdAsync(string supplierId);

        Task<ErrorLog> CreateErroLogAsync(ErrorLog errorLog);
    }

    public class DanhGiaNhaHangService : IDanhGiaNhaHangService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DanhGiaNhaHangService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DanhGiaNhaHangDTO> GetByIdAsync(long id)
        {
            return _mapper.Map<DanhGiaNhaHang, DanhGiaNhaHangDTO>(await _unitOfWork.danhGiaNhaHangRepository.GetByLongIdAsync(id));
        }

        public IEnumerable<DanhGiaNhaHangDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<DanhGiaNhaHang>, IEnumerable<DanhGiaNhaHangDTO>>(_unitOfWork.danhGiaNhaHangRepository.GetAll());
        }

        public async Task<DanhGiaNhaHangDTO> CreateAsync(DanhGiaNhaHangDTO DanhGiaNhaHangDTO)
        {
            DanhGiaNhaHang danhGiaNhaHang = _mapper.Map<DanhGiaNhaHangDTO, DanhGiaNhaHang>(DanhGiaNhaHangDTO);
            var danhGiaNhaHang1 = await _unitOfWork.danhGiaNhaHangRepository.CreateAsync(danhGiaNhaHang);
            return _mapper.Map<DanhGiaNhaHang, DanhGiaNhaHangDTO>(danhGiaNhaHang1);
        }

        public async Task<DanhGiaNhaHangDTO> UpdateAsync(DanhGiaNhaHangDTO DanhGiaNhaHangDTO)
        {
            DanhGiaNhaHang danhGiaNhaHang = _mapper.Map<DanhGiaNhaHangDTO, DanhGiaNhaHang>(DanhGiaNhaHangDTO);
            var danhGiaNhaHang1 = await _unitOfWork.danhGiaNhaHangRepository.UpdateAsync(danhGiaNhaHang);
            return _mapper.Map<DanhGiaNhaHang, DanhGiaNhaHangDTO>(danhGiaNhaHang1);
        }

        public async Task Delete(DanhGiaNhaHangDTO DanhGiaNhaHangDTO)
        {
            DanhGiaNhaHang danhGiaNhaHang = _mapper.Map<DanhGiaNhaHangDTO, DanhGiaNhaHang>(DanhGiaNhaHangDTO);
            _unitOfWork.danhGiaNhaHangRepository.Delete(danhGiaNhaHang);
            await _unitOfWork.Complete();
        }

        public DanhGiaNhaHangDTO GetByIdAsNoTracking(long id)
        {
            return _mapper.Map<DanhGiaNhaHang, DanhGiaNhaHangDTO>(_unitOfWork.danhGiaNhaHangRepository.GetByIdAsNoTracking(x => x.Id == id));
        }

        public async Task<IPagedList<DanhGiaNhaHangDTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            List<DanhGiaNhaHangDTO> list = new List<DanhGiaNhaHangDTO>();
            List<DanhGiaNhaHang> danhGiaNcus1 = new List<DanhGiaNhaHang>();

            if (!string.IsNullOrEmpty(searchString))
            {
                var danhGiaNcus = await _unitOfWork.danhGiaNhaHangRepository.FindAsync(x => x.TenNcu.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.DiaChi) && x.DiaChi.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.DienThoai) && x.DienThoai.ToLower().Contains(searchString.ToLower())));
                danhGiaNcus1 = danhGiaNcus.ToList();
            }
            else
            {
                danhGiaNcus1 = _unitOfWork.danhGiaNhaHangRepository.GetAll().ToList();

                if (danhGiaNcus1 == null)
                {
                    return null;
                }
            }

            danhGiaNcus1 = danhGiaNcus1.OrderByDescending(x => x.NgayTao).ToList();

            list = _mapper.Map<List<DanhGiaNhaHang>, List<DanhGiaNhaHangDTO>>(danhGiaNcus1);

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
            var danhGiaNhaHangs = await _unitOfWork.danhGiaNhaHangRepository
                .FindAsync(x => x.TenNcu.Trim().ToLower() == name.Trim().ToLower());

            if (danhGiaNhaHangs.Count() > 0)
            {
                string findName = danhGiaNhaHangs.FirstOrDefault().TenNcu;
                long findId = danhGiaNhaHangs.FirstOrDefault().Id;

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
    }
}