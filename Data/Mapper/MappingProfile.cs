using AutoMapper;
using Data.Models;
using Data.Models_QLTour;
using Data.Repository;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using VTinh = Data.Models.VTinh;

namespace Data.Mapper
{
    internal class MappingProfile : Profile
    {
        //private readonly IUnitOfWork _unitOfWork;

        public MappingProfile(/*IUnitOfWork unitOfWork*/)
        {
            //_unitOfWork = unitOfWork;

            CreateMap<Models.Supplier, SupplierDTO>().ReverseMap();
            CreateMap<Tinh, TinhDTO>().ReverseMap();
            CreateMap<VTinh, VTinhDTO>().ReverseMap();
            CreateMap<Thanhpho1, ThanhPho1DTO>().ReverseMap();
            CreateMap<Dmdiemtq, DiemTQDTO>().ReverseMap();
            CreateMap<DanhGiaNcu, DanhGiaNcuDTO>().ReverseMap();
        }
    }
}