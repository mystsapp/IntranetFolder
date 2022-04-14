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
    public interface IDanhGiaCamLaoService
    {
        IEnumerable<DanhGiaCamLaoDTO> GetAll();

        Task<DanhGiaCamLaoDTO> GetByIdAsync(long id);

        Task<DanhGiaCamLaoDTO> CreateAsync(DanhGiaCamLaoDTO danhGiaCamLaoDTO);

        Task<DanhGiaCamLaoDTO> UpdateAsync(DanhGiaCamLaoDTO danhGiaCamLaoDTO);

        Task Delete(DanhGiaCamLaoDTO danhGiaCamLaoDTO);

        DanhGiaCamLaoDTO GetByIdAsNoTracking(long id);

        Task<IPagedList<DanhGiaCamLaoDTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page);

        //Task<SupplierDTO> GetBySupplierByIdAsync(string supplierId);

        IEnumerable<LoaiDvDTO> GetAllLoaiDv();

        Task<bool> CheckNameExist(long id, string name);

        Task<SupplierDTO> GetSupplierByIdAsync(string supplierId);

        Task<ErrorLog> CreateErroLogAsync(ErrorLog errorLog);

        Task<IEnumerable<DanhGiaCamLaoDTO>> GetDanhGiaCamLaoBy_SupplierId(string supplierId);

        Task<TapDoanDTO> GetTapDoanByIdAsync(int tapDoanId);
    }

    public class DanhGiaCamLaoService : IDanhGiaCamLaoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DanhGiaCamLaoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DanhGiaCamLaoDTO> GetByIdAsync(long id)
        {
            return _mapper.Map<DanhGiaCamLao, DanhGiaCamLaoDTO>(await _unitOfWork.danhGiaCamLaoRepository.GetByLongIdAsync(id));
        }

        public IEnumerable<DanhGiaCamLaoDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<DanhGiaCamLao>, IEnumerable<DanhGiaCamLaoDTO>>(_unitOfWork.danhGiaCamLaoRepository.GetAll());
        }

        public async Task<DanhGiaCamLaoDTO> CreateAsync(DanhGiaCamLaoDTO danhGiaCamLaoDTO)
        {
            DanhGiaCamLao danhGiaCamLao = _mapper.Map<DanhGiaCamLaoDTO, DanhGiaCamLao>(danhGiaCamLaoDTO);
            var danhGiaCamLao1 = await _unitOfWork.danhGiaCamLaoRepository.CreateAsync(danhGiaCamLao);
            return _mapper.Map<DanhGiaCamLao, DanhGiaCamLaoDTO>(danhGiaCamLao1);
        }

        public async Task<DanhGiaCamLaoDTO> UpdateAsync(DanhGiaCamLaoDTO danhGiaCamLaoDTO)
        {
            DanhGiaCamLao danhGiaCamLao = _mapper.Map<DanhGiaCamLaoDTO, DanhGiaCamLao>(danhGiaCamLaoDTO);
            var danhGiaCamLao1 = await _unitOfWork.danhGiaCamLaoRepository.UpdateAsync(danhGiaCamLao);
            return _mapper.Map<DanhGiaCamLao, DanhGiaCamLaoDTO>(danhGiaCamLao1);
        }

        public async Task Delete(DanhGiaCamLaoDTO danhGiaCamLaoDTO)
        {
            DanhGiaCamLao danhGiaCamLao = _mapper.Map<DanhGiaCamLaoDTO, DanhGiaCamLao>(danhGiaCamLaoDTO);
            _unitOfWork.danhGiaCamLaoRepository.Delete(danhGiaCamLao);
            await _unitOfWork.Complete();
        }

        public DanhGiaCamLaoDTO GetByIdAsNoTracking(long id)
        {
            return _mapper.Map<DanhGiaCamLao, DanhGiaCamLaoDTO>(_unitOfWork.danhGiaCamLaoRepository.GetByIdAsNoTracking(x => x.Id == id));
        }

        public async Task<IPagedList<DanhGiaCamLaoDTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            List<DanhGiaCamLaoDTO> list = new List<DanhGiaCamLaoDTO>();
            List<DanhGiaCamLao> danhGiaNcus1 = new List<DanhGiaCamLao>();

            if (!string.IsNullOrEmpty(searchString))
            {
                var danhGiaNcus = await _unitOfWork.danhGiaCamLaoRepository.FindAsync(x => x.TenNcu.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.SanPham) && x.SanPham.ToLower().Contains(searchString.ToLower())));
                danhGiaNcus1 = danhGiaNcus.ToList();
            }
            else
            {
                danhGiaNcus1 = _unitOfWork.danhGiaCamLaoRepository.GetAll().ToList();

                if (danhGiaNcus1 == null)
                {
                    return null;
                }
            }

            danhGiaNcus1 = danhGiaNcus1.OrderByDescending(x => x.NgayTao).ToList();

            list = _mapper.Map<List<DanhGiaCamLao>, List<DanhGiaCamLaoDTO>>(danhGiaNcus1);

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
            var DanhGiaCamLaos = await _unitOfWork.danhGiaCamLaoRepository
                .FindAsync(x => x.TenNcu.Trim().ToLower() == name.Trim().ToLower());

            if (DanhGiaCamLaos.Count() > 0)
            {
                string findName = DanhGiaCamLaos.FirstOrDefault().TenNcu;
                long findId = DanhGiaCamLaos.FirstOrDefault().Id;

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

        public async Task<IEnumerable<DanhGiaCamLaoDTO>> GetDanhGiaCamLaoBy_SupplierId(string supplierId)
        {
            return _mapper.Map<IEnumerable<DanhGiaCamLao>, IEnumerable<DanhGiaCamLaoDTO>>
                (await _unitOfWork.danhGiaCamLaoRepository.FindIncludeOneAsync(x => x.Supplier, y => y.SupplierId == supplierId));
        }

        public async Task<TapDoanDTO> GetTapDoanByIdAsync(int tapDoanId)
        {
            return _mapper.Map<TapDoan, TapDoanDTO>(_unitOfWork.tapDoanRepository.GetById(tapDoanId));
        }
    }
}