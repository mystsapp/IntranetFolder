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
    public interface ITapDoanService
    {
        IEnumerable<TapDoanDTO> GetAll();

        Task<TapDoanDTO> GetByIdAsync(int id);

        Task<TapDoanDTO> CreateAsync(TapDoanDTO TapDoanDTO);

        Task<TapDoanDTO> UpdateAsync(TapDoanDTO TapDoanDTO);

        Task Delete(TapDoanDTO TapDoanDTO);

        TapDoanDTO GetByIdAsNoTracking(int id);

        Task<IPagedList<TapDoanDTO>> ListTapDoan(string searchString, string searchFromDate, string searchToDate, int? page);
    }

    public class TapDoanService : ITapDoanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TapDoanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TapDoanDTO> GetByIdAsync(int id)
        {
            return _mapper.Map<TapDoan, TapDoanDTO>(await _unitOfWork.tapDoanRepository.GetByIdAsync(id));
        }

        public IEnumerable<TapDoanDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<TapDoan>, IEnumerable<TapDoanDTO>>(_unitOfWork.tapDoanRepository.GetAll());
        }

        public async Task<TapDoanDTO> CreateAsync(TapDoanDTO tapDoanDTO)
        {
            TapDoan tapDoan = _mapper.Map<TapDoanDTO, TapDoan>(tapDoanDTO);
            var tapDoan1 = await _unitOfWork.tapDoanRepository.CreateAsync(tapDoan);
            return _mapper.Map<TapDoan, TapDoanDTO>(tapDoan1);
        }

        public async Task<TapDoanDTO> UpdateAsync(TapDoanDTO tapDoanDTO)
        {
            TapDoan tapDoan = _mapper.Map<TapDoanDTO, TapDoan>(tapDoanDTO);
            var tapDoan1 = await _unitOfWork.tapDoanRepository.UpdateAsync(tapDoan);
            return _mapper.Map<TapDoan, TapDoanDTO>(tapDoan1);
        }

        public async Task Delete(TapDoanDTO tapDoanDTO)
        {
            TapDoan tapDoan = _mapper.Map<TapDoanDTO, TapDoan>(tapDoanDTO);
            _unitOfWork.tapDoanRepository.Delete(tapDoan);
            await _unitOfWork.Complete();
        }

        public TapDoanDTO GetByIdAsNoTracking(int id)
        {
            return _mapper.Map<TapDoan, TapDoanDTO>(_unitOfWork.tapDoanRepository.GetByIdAsNoTracking(x => x.Id == id));
        }

        public async Task<IPagedList<TapDoanDTO>> ListTapDoan(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            List<TapDoanDTO> list = new List<TapDoanDTO>();
            List<TapDoan> TapDoans1 = new List<TapDoan>();

            if (!string.IsNullOrEmpty(searchString))
            {
                var TapDoans = await _unitOfWork.tapDoanRepository.FindAsync(x => x.Ten.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.Chuoi) && x.Chuoi.ToLower().Contains(searchString.ToLower())));
                TapDoans1 = TapDoans.ToList();
            }
            else
            {
                TapDoans1 = _unitOfWork.tapDoanRepository.GetAll().ToList();

                if (TapDoans1 == null)
                {
                    return null;
                }
            }

            TapDoans1 = TapDoans1.OrderByDescending(x => x.NgayTao).ToList();

            list = _mapper.Map<List<TapDoan>, List<TapDoanDTO>>(TapDoans1);

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
    }
}