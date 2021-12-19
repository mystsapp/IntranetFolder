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
    public interface ITinhTPService
    {
        Task<List<VTinhDTO>> GetVTinhDTOs();

        Task<Model.TinhDTO> GetByIdAsync(string id);

        Task<Model.TinhDTO> CreateAsync(Model.TinhDTO tinhDTO);

        Task<Model.TinhDTO> UpdateAsync(Model.TinhDTO tinhDTO);

        Task Delete(Model.TinhDTO tinhDTO);

        IEnumerable<TinhDTO> GetTinhs();

        Task<IEnumerable<Thanhpho1>> GetThanhpho1s();

        Model.TinhDTO GetByIdAsNoTracking(string id);
    }

    public class TinhTPService : ITinhTPService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TinhTPService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Model.TinhDTO> GetByIdAsync(string id)
        {
            return _mapper.Map<Tinh, Model.TinhDTO>(await _unitOfWork.tinhRepository.GetByIdAsync(id));
        }

        public async Task<List<VTinhDTO>> GetVTinhDTOs()
        {
            return _mapper.Map<List<VTinh>, List<VTinhDTO>>(await _unitOfWork.tinhRepository.GetVTinhDTOs());
        }

        public async Task<TinhDTO> CreateAsync(TinhDTO tinhDTO)
        {
            Tinh tinh = _mapper.Map<TinhDTO, Tinh>(tinhDTO);
            var tinh1 = await _unitOfWork.tinhRepository.CreateAsync(tinh);
            return _mapper.Map<Tinh, TinhDTO>(tinh1);
        }

        public async Task<TinhDTO> UpdateAsync(TinhDTO tinhDTO)
        {
            Tinh tinh = _mapper.Map<TinhDTO, Tinh>(tinhDTO);
            var tinh1 = await _unitOfWork.tinhRepository.UpdateAsync(tinh);
            return _mapper.Map<Tinh, TinhDTO>(tinh1);
        }

        public async Task Delete(TinhDTO tinhDTO)
        {
            Tinh tinh = _mapper.Map<TinhDTO, Tinh>(tinhDTO);
            _unitOfWork.tinhRepository.Delete(tinh);
            await _unitOfWork.Complete();
        }

        public IEnumerable<Tinh> GetTinhs()
        {
            return _unitOfWork.tinhRepository.GetAll();
        }

        public async Task<IEnumerable<Thanhpho1>> GetThanhpho1s()
        {
            return await _unitOfWork.tinhRepository.GetThanhpho1s();
        }

        public TinhDTO GetByIdAsNoTracking(string id)
        {
            return _mapper.Map<Tinh, TinhDTO>(_unitOfWork.tinhRepository.GetByIdAsNoTracking(x => x.Matinh == id));
        }
    }
}