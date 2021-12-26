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
    public class DiemTQViewModel
    {
        public IEnumerable<DiemTQDTO> DiemTQDTOs { get; set; }
        public IEnumerable<ThanhPho1DTO> ThanhPho1DTOs { get; set; }
        public IEnumerable<SupplierDTO> SupplierDTOs { get; set; }
        public DiemTQDTO DiemTQDTO { get; set; }
        public TinhDTO TinhDTO { get; set; }
        public int Page { get; set; }
        public string StrUrl { get; set; }

        //[Remote("IsStringNameAvailable", "TinhTP", ErrorMessage = "Mã này đã tồn tại.")]
        //[Required(ErrorMessage = "Mã tỉnh không được để trống.")]
        //[MaxLength(3, ErrorMessage = "Mã tỉnh tối đa 3 ký tự")]
        //public string TenCreate { get; set; }
    }
}