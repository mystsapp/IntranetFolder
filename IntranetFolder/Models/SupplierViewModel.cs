using Data.Models;
using GleamTech.FileUltimate.AspNet.UI;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace IntranetFolder.Models
{
    public class SupplierViewModel
    {
        public IPagedList<SupplierDTO> SupplierDTOs { get; set; }
        public IEnumerable<DanhGiaNhaHangDTO> DanhGiaNhaHangDTOs { get; set; }
        public SupplierDTO SupplierDTO { get; set; }
        public IEnumerable<VTinh> VTinhs { get; set; }
        public IEnumerable<Thanhpho1> Thanhpho1s { get; set; }
        public IEnumerable<Quocgium> Quocgias { get; set; }
        public int Page { get; set; }
        public string StrUrl { get; set; }
    }
}