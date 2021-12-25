using AutoMapper;
using Data.Models;
using Data.Models_QLTour;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using VTinh = Data.Models.VTinh;

namespace Data.Mapper
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Supplier, SupplierDTO>().ReverseMap();
            CreateMap<Tinh, TinhDTO>().ReverseMap();
            CreateMap<VTinh, VTinhDTO>().ReverseMap();
            CreateMap<Thanhpho1, ThanhPho1DTO>().ReverseMap();
            CreateMap<Dmdiemtq, DiemTQDTO>().ReverseMap();
        }
    }
}