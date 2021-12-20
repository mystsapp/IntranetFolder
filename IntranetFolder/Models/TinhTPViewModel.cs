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
    public class TinhTPViewModel
    {
        public IEnumerable<VTinhDTO> VTinhDTOs { get; set; }
        public TinhDTO TinhDTO { get; set; }
        public VTinhDTO VTinhDTO { get; set; }
        public IEnumerable<Vungmien> Vungmiens { get; set; }
        public IEnumerable<Thanhpho1> Thanhpho1s { get; set; }
        public int Page { get; set; }
        public string StrUrl { get; set; }
    }
}