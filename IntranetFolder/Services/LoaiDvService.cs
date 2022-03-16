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
    public interface ILoaiDvService
    {
        IEnumerable<LoaiDvDTO> GetAll();

        Task<LoaiDvDTO> GetByIdAsync(int id);

        Task<LoaiDvDTO> CreateAsync(LoaiDvDTO loaiDvDTO);

        Task<LoaiDvDTO> UpdateAsync(LoaiDvDTO loaiDvDTO);

        Task Delete(LoaiDvDTO loaiDvDTO);

        LoaiDvDTO GetByIdAsNoTracking(int id);

        Task<IPagedList<LoaiDvDTO>> ListLoaiDv(string searchString, string searchFromDate, string searchToDate, int? page);
    }

    public class LoaiDvService : ILoaiDvService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoaiDvService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoaiDvDTO> GetByIdAsync(int id)
        {
            return _mapper.Map<LoaiDv, LoaiDvDTO>(await _unitOfWork.loaiDvRepository.GetByIdAsync(id));
        }

        public IEnumerable<LoaiDvDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<LoaiDv>, IEnumerable<LoaiDvDTO>>(_unitOfWork.loaiDvRepository.GetAll());
        }

        public async Task<LoaiDvDTO> CreateAsync(LoaiDvDTO loaiDvDTO)
        {
            LoaiDv loaiDv = _mapper.Map<LoaiDvDTO, LoaiDv>(loaiDvDTO);
            var loaiDv1 = await _unitOfWork.loaiDvRepository.CreateAsync(loaiDv);
            return _mapper.Map<LoaiDv, LoaiDvDTO>(loaiDv1);
        }

        public async Task<LoaiDvDTO> UpdateAsync(LoaiDvDTO loaiDvDTO)
        {
            LoaiDv loaiDv = _mapper.Map<LoaiDvDTO, LoaiDv>(loaiDvDTO);
            var loaiDv1 = await _unitOfWork.loaiDvRepository.UpdateAsync(loaiDv);
            return _mapper.Map<LoaiDv, LoaiDvDTO>(loaiDv1);
        }

        public async Task Delete(LoaiDvDTO loaiDvDTO)
        {
            LoaiDv loaiDv = _mapper.Map<LoaiDvDTO, LoaiDv>(loaiDvDTO);
            _unitOfWork.loaiDvRepository.Delete(loaiDv);
            await _unitOfWork.Complete();
        }

        public LoaiDvDTO GetByIdAsNoTracking(int id)
        {
            return _mapper.Map<LoaiDv, LoaiDvDTO>(_unitOfWork.loaiDvRepository.GetByIdAsNoTracking(x => x.Id == id));
        }

        public async Task<IPagedList<LoaiDvDTO>> ListLoaiDv(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            List<LoaiDvDTO> list = new List<LoaiDvDTO>();
            List<LoaiDv> LoaiDvs1 = new List<LoaiDv>();

            if (!string.IsNullOrEmpty(searchString))
            {
                var LoaiDvs = await _unitOfWork.loaiDvRepository.FindAsync(x => x.MaLoai.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.TenLoai) && x.TenLoai.ToLower().Contains(searchString.ToLower())));
                LoaiDvs1 = LoaiDvs.ToList();
            }
            else
            {
                LoaiDvs1 = _unitOfWork.loaiDvRepository.GetAll().ToList();

                if (LoaiDvs1 == null)
                {
                    return null;
                }
            }

            LoaiDvs1 = LoaiDvs1.OrderByDescending(x => x.TenLoai).OrderByDescending(x => x.NgayTao).ToList();

            list = _mapper.Map<List<LoaiDv>, List<LoaiDvDTO>>(LoaiDvs1);

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