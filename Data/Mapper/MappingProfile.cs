using AutoMapper;
using Data.Models;
using Data.Models_QLTour;
using Data.Repository;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using Supplier = Data.Models.Supplier;
using VTinh = Data.Models.VTinh;

namespace Data.Mapper
{
    internal class MappingProfile : Profile
    {
        //private readonly IUnitOfWork _unitOfWork;

        public MappingProfile(/*IUnitOfWork unitOfWork*/)
        {
            //_unitOfWork = unitOfWork;

            CreateMap<Models.Supplier, SupplierDTO>()
                .ForMember(dest => dest.DanhGiaCamLaoDTOs, opt => opt.MapFrom(src => src.DanhGiaCamLaos))
                .ForMember(dest => dest.DanhGiaDTQDTOs, opt => opt.MapFrom(src => src.DanhGiaDiemThamQuans))
                .ForMember(dest => dest.DanhGiaLandTourDTOs, opt => opt.MapFrom(src => src.DanhGiaLandtours))
                .ForMember(dest => dest.DanhGiaNhaHangDTOs, opt => opt.MapFrom(src => src.DanhGiaNhaHangs))
                .ForMember(dest => dest.DanhGiaNhaHangDTOs, opt => opt.MapFrom(src => src.DanhGiaNhaHangs)).ReverseMap();
            CreateMap<Tinh, TinhDTO>().ReverseMap();
            CreateMap<VTinh, VTinhDTO>().ReverseMap();
            CreateMap<Thanhpho1, ThanhPho1DTO>().ReverseMap();
            CreateMap<Dmdiemtq, DiemTQDTO>().ReverseMap();
            CreateMap<DanhGiaNcu, DanhGiaNcuDTO>().ReverseMap();
            CreateMap<DanhGiaNhaHang, DanhGiaNhaHangDTO>()
                .ForMember(dest => dest.SupplierDTO, opt => opt.MapFrom(src => src.Supplier)).ReverseMap();
            CreateMap<DanhGiaKhachSan, DanhGiaKhachSanDTO>()
                .ForMember(dest => dest.SupplierDTO, opt => opt.MapFrom(src => src.Supplier)).ReverseMap();
            CreateMap<DanhGiaLandtour, DanhGiaLandTourDTO>()
                .ForMember(dest => dest.SupplierDTO, opt => opt.MapFrom(src => src.Supplier)).ReverseMap();
            CreateMap<DanhGiaDiemThamQuan, DanhGiaDTQDTO>()
                .ForMember(dest => dest.SupplierDTO, opt => opt.MapFrom(src => src.Supplier)).ReverseMap();
            CreateMap<DanhGiaCamLao, DanhGiaCamLaoDTO>()
                .ForMember(dest => dest.SupplierDTO, opt => opt.MapFrom(src => src.Supplier)).ReverseMap();
        }
    }
}