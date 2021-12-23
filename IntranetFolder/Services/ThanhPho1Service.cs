using AutoMapper;
using Data.Models;
using Data.Repository;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using Data.Utilities;

namespace IntranetFolder.Services
{
    public interface IThanhPho1Service
    {
        Task<ThanhPho1DTO> GetByIdAsync(string id);

        Task<ThanhPho1DTO> CreateAsync(ThanhPho1DTO thanhPho1DTO);

        Task<ThanhPho1DTO> UpdateAsync(ThanhPho1DTO thanhPho1DTO);

        Task Delete(ThanhPho1DTO thanhPho1DTO);

        ThanhPho1DTO GetByIdAsNoTracking(string id);

        IEnumerable<ThanhPho1DTO> GetThanhPho1s();

        Task<IEnumerable<ThanhPho1DTO>> GetThanhPho1s_By_Tinh(string maTinh);

        Task<TinhDTO> GetTinhByIdAsync(string id);

        Task<string> GetNextId(string tinhId);
    }

    public class ThanhPho1Service : IThanhPho1Service
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ThanhPho1Service(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ThanhPho1DTO> GetByIdAsync(string id)
        {
            return _mapper.Map<Thanhpho1, ThanhPho1DTO>(await _unitOfWork.thanhPho1Repository.GetByIdAsync(id));
        }

        public async Task<ThanhPho1DTO> CreateAsync(ThanhPho1DTO thanhPho1DTO)
        {
            Thanhpho1 thanhpho1 = _mapper.Map<ThanhPho1DTO, Thanhpho1>(thanhPho1DTO);
            var thanhpho11 = await _unitOfWork.thanhPho1Repository.CreateAsync(thanhpho1);
            return _mapper.Map<Thanhpho1, ThanhPho1DTO>(thanhpho11);
        }

        public async Task<ThanhPho1DTO> UpdateAsync(ThanhPho1DTO thanhPho1DTO)
        {
            Thanhpho1 thanhpho1 = _mapper.Map<ThanhPho1DTO, Thanhpho1>(thanhPho1DTO);
            var thanhpho11 = await _unitOfWork.thanhPho1Repository.UpdateAsync(thanhpho1);
            return _mapper.Map<Thanhpho1, ThanhPho1DTO>(thanhpho11);
        }

        public async Task Delete(ThanhPho1DTO thanhPho1DTO)
        {
            Thanhpho1 thanhpho1 = _mapper.Map<ThanhPho1DTO, Thanhpho1>(thanhPho1DTO);
            _unitOfWork.thanhPho1Repository.Delete(thanhpho1);
            await _unitOfWork.Complete();
        }

        public ThanhPho1DTO GetByIdAsNoTracking(string id)
        {
            return _mapper.Map<Thanhpho1, ThanhPho1DTO>(_unitOfWork.thanhPho1Repository.GetByIdAsNoTracking(x => x.Matp == id));
        }

        public IEnumerable<ThanhPho1DTO> GetThanhPho1s()
        {
            return _mapper.Map<IEnumerable<Thanhpho1>, IEnumerable<ThanhPho1DTO>>
                (_unitOfWork.thanhPho1Repository.GetAll());
        }

        public async Task<IEnumerable<ThanhPho1DTO>> GetThanhPho1s_By_Tinh(string maTinh)
        {
            return _mapper.Map<IEnumerable<Thanhpho1>, IEnumerable<ThanhPho1DTO>>
                (await _unitOfWork.thanhPho1Repository.FindAsync(x => x.Matinh == maTinh));
        }

        public async Task<TinhDTO> GetTinhByIdAsync(string id)
        {
            return _mapper.Map<Tinh, Model.TinhDTO>(await _unitOfWork.tinhRepository.GetByIdAsync(id));
        }

        public async Task<string> GetNextId(string tinhId)
        {
            var tinh = _unitOfWork.tinhRepository.GetById(tinhId);
            var thanhpho1s = await _unitOfWork.thanhPho1Repository.FindAsync(x => x.Matinh == tinhId);

            Thanhpho1 thanhpho1 = new Thanhpho1();
            if (thanhpho1s.Count() > 0)
            {
                thanhpho1 = thanhpho1s.OrderByDescending(x => x.Matp).FirstOrDefault();
            }

            if (thanhpho1 == null || string.IsNullOrEmpty(thanhpho1.Matp))
            {
                return Data.Utilities.GetNextId.NextTPId("", tinhId, "001");
            }
            else
            {
                return Data.Utilities.GetNextId.NextTPId(thanhpho1.Matp, tinhId, "001");
            }
        }
    }
}