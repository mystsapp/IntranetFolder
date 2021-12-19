using AutoMapper;
using Data.Models;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Mapper
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Supplier, SupplierDTO>().ReverseMap();
            CreateMap<Tinh, TinhDTO>().ReverseMap();
            CreateMap<VTinh, VTinhDTO>().ReverseMap();
            CreateMap<Thanhpho1, ThanhPho1DTO>().ReverseMap();
        }
    }
}