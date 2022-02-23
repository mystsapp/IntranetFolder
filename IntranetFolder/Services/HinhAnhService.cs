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
    public interface IHinhAnhService
    {
        public Task<int> CreateDichVu1Image(HinhAnhDTO imageDTO);

        public Task<int> DeleteDichVu1ImageByImageId(int imageId);

        public Task<int> DeleteDichVu1ImageByDichVu1Id(string dichVu1Id);

        public Task<int> DeleteDichVu1ImageByImageUrl(string imageUrl);

        public Task<IEnumerable<HinhAnhDTO>> GetDichVu1HinhAnhDTOs(string dichVuId);
    }

    public class HinhAnhService : IHinhAnhService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HinhAnhService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateDichVu1Image(HinhAnhDTO imageDTO)
        {
            var image = _mapper.Map<HinhAnhDTO, HinhAnh>(imageDTO);
            _unitOfWork.hinhAnhRepository.Create(image);
            return await _unitOfWork.Complete();
        }

        public async Task<int> DeleteDichVu1ImageByImageUrl(string imageUrl)
        {
            var allImage = await _unitOfWork.hinhAnhRepository.FindAsync
                (x => x.Url.ToLower() == imageUrl.ToLower());

            _unitOfWork.hinhAnhRepository.Delete(allImage.FirstOrDefault());
            return await _unitOfWork.Complete();
        }

        public async Task<int> DeleteDichVu1ImageByImageId(int imageId)
        {
            var image = await _unitOfWork.hinhAnhRepository.GetByIdAsync(imageId);
            _unitOfWork.hinhAnhRepository.Delete(image);
            return await _unitOfWork.Complete();
        }

        public async Task<int> DeleteDichVu1ImageByDichVu1Id(string dichVu1Id)
        {
            var imageList = await _unitOfWork.hinhAnhRepository.FindAsync(x => x.DichVuId == dichVu1Id);
            await _unitOfWork.hinhAnhRepository.DeleteRangeAsync(imageList.ToList());
            return await _unitOfWork.Complete();
        }

        public async Task<IEnumerable<HinhAnhDTO>> GetDichVu1HinhAnhDTOs(string dichVuId)
        {
            return _mapper.Map<IEnumerable<HinhAnh>, IEnumerable<HinhAnhDTO>>(
            await _unitOfWork.hinhAnhRepository.FindAsync(x => x.DichVuId == dichVuId));
        }
    }
}