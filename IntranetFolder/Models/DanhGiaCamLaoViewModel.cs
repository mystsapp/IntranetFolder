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
    public class DanhGiaCamLaoViewModel
    {
        public IEnumerable<DanhGiaCamLaoDTO> DanhGiaCamLaoDTOs { get; set; }
        public DanhGiaCamLaoDTO DanhGiaCamLaoDTO { get; set; }
        public SupplierDTO SupplierDTO { get; set; }

        public int Page { get; set; }
        public string StrUrl { get; set; }

        //[Remote("IsStringNameAvailable", "DanhGiaNhaHang", ErrorMessage = "Tên này đã tồn tại.")]
        //[Required(ErrorMessage = "Tên không được để trống.")]
        //public string TenCreate { get; set; }
    }
}