using Data.Models;
using GleamTech.FileUltimate.AspNet.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace IntranetFolder.Models
{
    public class SupplierViewModel
    {
        public IPagedList<Supplier> Suppliers { get; set; }
        public Supplier Supplier { get; set; }
        public int Page { get; set; }
        public string StrUrl { get; set; }
    }
}