using Data.Models;
using GleamTech.FileUltimate.AspNet.UI;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace IntranetFolder.Models
{
    public class DichVu1ViewModel
    {
        public IEnumerable<DichVu1DTO> DichVu1DTOs { get; set; }
        public IEnumerable<Quocgium> Quocgias { get; set; }
        public IEnumerable<Vungmien> Vungmiens { get; set; }
        public IEnumerable<VTinh> VTinhs { get; set; }
        public IEnumerable<Thanhpho1> Thanhpho1s { get; set; }
        public IEnumerable<LoaiDvDTO> LoaiDvs { get; set; }
        public IEnumerable<UserDTO> UserDTOs { get; set; }
        public IPagedList<HinhAnhDTO> HinhAnhDTOs { get; set; }
        public string Dichvu1Id { get; set; }
        public List<string> LoaiSaos { get; set; }
        public DichVu1DTO DichVu1DTO { get; set; }

        public SupplierDTO SupplierDTO { get; set; }
        public TinhDTO TinhDTO { get; set; }
        public int Page { get; set; }
        public string StrUrl { get; set; }
        public string SearchString { get; set; }

        //[Remote("IsStringNameAvailable", "TinhTP", ErrorMessage = "Mã này đã tồn tại.")]
        //[Required(ErrorMessage = "Mã tỉnh không được để trống.")]
        //[MaxLength(3, ErrorMessage = "Mã tỉnh tối đa 3 ký tự")]
        //public string TenCreate { get; set; }
    }
}