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
    public interface IDanhGiaNhaCungUngService
    {
        IEnumerable<DanhGiaNcuDTO> GetAll();

        Task<DanhGiaNcuDTO> GetByIdAsync(long id);

        Task<DanhGiaNcuDTO> CreateAsync(DanhGiaNcuDTO danhGiaNcuDTO);

        Task<DanhGiaNcuDTO> UpdateAsync(DanhGiaNcuDTO danhGiaNcuDTO);

        Task Delete(DanhGiaNcuDTO danhGiaNcuDTO);

        DanhGiaNcuDTO GetByIdAsNoTracking(long id);

        Task<IPagedList<DanhGiaNcuDTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page);

        IEnumerable<LoaiDvDTO> GetAllLoaiDv();
    }

    public class DanhGiaNhaCungUngService : IDanhGiaNhaCungUngService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DanhGiaNhaCungUngService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DanhGiaNcuDTO> GetByIdAsync(long id)
        {
            return _mapper.Map<DanhGiaNcu, DanhGiaNcuDTO>(await _unitOfWork.danhGiaNhaCungUngRepository.GetByLongIdAsync(id));
        }

        public IEnumerable<DanhGiaNcuDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<DanhGiaNcu>, IEnumerable<DanhGiaNcuDTO>>(_unitOfWork.danhGiaNhaCungUngRepository.GetAll());
        }

        public async Task<DanhGiaNcuDTO> CreateAsync(DanhGiaNcuDTO danhGiaNcuDTO)
        {
            danhGiaNcuDTO.TenDv = _unitOfWork.loaiDvRepository.GetById(danhGiaNcuDTO.LoaiDvid).TenLoai;
            DanhGiaNcu danhGiaNcu = _mapper.Map<DanhGiaNcuDTO, DanhGiaNcu>(danhGiaNcuDTO);
            var danhGiaNcu1 = await _unitOfWork.danhGiaNhaCungUngRepository.CreateAsync(danhGiaNcu);
            return _mapper.Map<DanhGiaNcu, DanhGiaNcuDTO>(danhGiaNcu1);
        }

        public async Task<DanhGiaNcuDTO> UpdateAsync(DanhGiaNcuDTO danhGiaNcuDTO)
        {
            DanhGiaNcu danhGiaNcu = _mapper.Map<DanhGiaNcuDTO, DanhGiaNcu>(danhGiaNcuDTO);
            var danhGiaNcu1 = await _unitOfWork.danhGiaNhaCungUngRepository.UpdateAsync(danhGiaNcu);
            return _mapper.Map<DanhGiaNcu, DanhGiaNcuDTO>(danhGiaNcu1);
        }

        public async Task Delete(DanhGiaNcuDTO danhGiaNcuDTO)
        {
            DanhGiaNcu DanhGiaNcu = _mapper.Map<DanhGiaNcuDTO, DanhGiaNcu>(danhGiaNcuDTO);
            _unitOfWork.danhGiaNhaCungUngRepository.Delete(DanhGiaNcu);
            await _unitOfWork.Complete();
        }

        public DanhGiaNcuDTO GetByIdAsNoTracking(long id)
        {
            return _mapper.Map<DanhGiaNcu, DanhGiaNcuDTO>(_unitOfWork.danhGiaNhaCungUngRepository.GetByIdAsNoTracking(x => x.Id == id));
        }

        public async Task<IPagedList<DanhGiaNcuDTO>> ListDanhGiaNCU(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            List<DanhGiaNcuDTO> list = new List<DanhGiaNcuDTO>();
            List<DanhGiaNcu> danhGiaNcus1 = new List<DanhGiaNcu>();

            if (!string.IsNullOrEmpty(searchString))
            {
                var danhGiaNcus = await _unitOfWork.danhGiaNhaCungUngRepository.FindAsync(x => x.TenNcu.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.KnngheNghiep) && x.KnngheNghiep.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.KntaiThiTruongVn) && x.KntaiThiTruongVn.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.NlkhaiThacDvtaiDiaPhuong) && x.NlkhaiThacDvtaiDiaPhuong.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.CldvvaHdvtiengViet) && x.CldvvaHdvtiengViet.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.SanPham) && x.SanPham.ToLower().Contains(searchString.ToLower())));
                danhGiaNcus1 = danhGiaNcus.ToList();
            }
            else
            {
                danhGiaNcus1 = _unitOfWork.danhGiaNhaCungUngRepository.GetAll().ToList();

                if (danhGiaNcus1 == null)
                {
                    return null;
                }
            }

            danhGiaNcus1 = danhGiaNcus1.OrderByDescending(x => x.NgayTao).ToList();

            list = _mapper.Map<List<DanhGiaNcu>, List<DanhGiaNcuDTO>>(danhGiaNcus1);

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
    }
}