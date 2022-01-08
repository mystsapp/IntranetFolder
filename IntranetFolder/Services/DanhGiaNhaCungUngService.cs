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
    }
}