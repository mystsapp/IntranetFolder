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
using Data.Models_QLTour;
using Supplier = Data.Models.Supplier;

namespace IntranetFolder.Services
{
    public interface IDiemTQService
    {
        Task<DiemTQDTO> GetByIdAsync(string id);

        Task<DiemTQDTO> CreateAsync(DiemTQDTO diemTQDTO);

        Task<DiemTQDTO> UpdateAsync(DiemTQDTO diemTQDTO);

        void Delete(DiemTQDTO diemTQDTO);

        DiemTQDTO GetByIdAsNoTracking(string id);

        IEnumerable<DiemTQDTO> GetDiemTQs();

        Task<IEnumerable<DiemTQDTO>> GetDiemTQs_By_Tinh(string maTinh);

        Task<TinhDTO> GetTinhByIdAsync(string id);

        Task<string> GetNextId(string tinhId);

        Task<IEnumerable<ThanhPho1DTO>> GetThanhPho1DTOs_By_Tinh(string tinhid);

        Task<IEnumerable<SupplierDTO>> GetSuppliers();
    }

    public class DiemTQService : IDiemTQService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DiemTQService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DiemTQDTO> GetByIdAsync(string id)
        {
            return _mapper.Map<Dmdiemtq, DiemTQDTO>(await _unitOfWork.dmdiemtqRepository.GetByIdAsync(id));
        }

        public async Task<DiemTQDTO> CreateAsync(DiemTQDTO DiemTQDTO)
        {
            Dmdiemtq Dmdiemtq = _mapper.Map<DiemTQDTO, Dmdiemtq>(DiemTQDTO);
            var Dmdiemtq1 = await _unitOfWork.dmdiemtqRepository.CreateAsync(Dmdiemtq);
            return _mapper.Map<Dmdiemtq, DiemTQDTO>(Dmdiemtq1);
        }

        public async Task<DiemTQDTO> UpdateAsync(DiemTQDTO DiemTQDTO)
        {
            Dmdiemtq Dmdiemtq = _mapper.Map<DiemTQDTO, Dmdiemtq>(DiemTQDTO);
            var Dmdiemtq1 = await _unitOfWork.dmdiemtqRepository.UpdateAsync(Dmdiemtq);
            return _mapper.Map<Dmdiemtq, DiemTQDTO>(Dmdiemtq1);
        }

        public void Delete(DiemTQDTO DiemTQDTO)
        {
            Dmdiemtq Dmdiemtq = _mapper.Map<DiemTQDTO, Dmdiemtq>(DiemTQDTO);
            _unitOfWork.dmdiemtqRepository.Delete(Dmdiemtq);
        }

        public DiemTQDTO GetByIdAsNoTracking(string id)
        {
            return _mapper.Map<Dmdiemtq, DiemTQDTO>(_unitOfWork.dmdiemtqRepository.GetByIdAsNoTracking(x => x.Code == id));
        }

        public IEnumerable<DiemTQDTO> GetDiemTQs()
        {
            return _mapper.Map<IEnumerable<Dmdiemtq>, IEnumerable<DiemTQDTO>>
                (_unitOfWork.dmdiemtqRepository.GetAll());
        }

        public async Task<IEnumerable<DiemTQDTO>> GetDiemTQs_By_Tinh(string maTinh)
        {
            return _mapper.Map<IEnumerable<Dmdiemtq>, IEnumerable<DiemTQDTO>>
                (await _unitOfWork.dmdiemtqRepository.FindAsync(x => x.Tinhtp == maTinh));
        }

        public async Task<TinhDTO> GetTinhByIdAsync(string id)
        {
            return _mapper.Map<Tinh, Model.TinhDTO>(await _unitOfWork.tinhRepository.GetByIdAsync(id));
        }

        public async Task<string> GetNextId(string tinhId)
        {
            var tinh = _unitOfWork.tinhRepository.GetById(tinhId);
            var Dmdiemtqs = await _unitOfWork.dmdiemtqRepository.FindAsync(x => x.Tinhtp == tinhId);

            Dmdiemtq Dmdiemtq = new Dmdiemtq();
            if (Dmdiemtqs.Count() > 0)
            {
                Dmdiemtq = Dmdiemtqs.OrderByDescending(x => x.Code).FirstOrDefault();
            }

            if (Dmdiemtq == null || string.IsNullOrEmpty(Dmdiemtq.Code))
            {
                return Data.Utilities.GetNextId.NextTPId("", tinhId, "001");
            }
            else
            {
                return Data.Utilities.GetNextId.NextTPId(Dmdiemtq.Code, tinhId, "001");
            }
        }

        public async Task<IEnumerable<ThanhPho1DTO>> GetThanhPho1DTOs_By_Tinh(string tinhid)
        {
            return _mapper.Map<IEnumerable<Thanhpho1>, IEnumerable<ThanhPho1DTO>>
                (await _unitOfWork.thanhPho1Repository.FindAsync(x => x.Matinh == tinhid));
        }

        public async Task<IEnumerable<SupplierDTO>> GetSuppliers()
        {
            var suppliers = await _unitOfWork.supplierRepository.FindAsync(x => x.Trangthai);

            return _mapper.Map<List<Supplier>, List<SupplierDTO>>(suppliers.ToList());
        }
    }
}