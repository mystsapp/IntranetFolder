using Data.Models;
using GleamTech.FileUltimate.AspNet.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetFolder.Models
{
    public class HomeViewModel
    {
        public IEnumerable<FolderUser> FolderUsers { get; set; }
        public FolderUser FolderUser { get; set; }
        public FileManager FileManager { get; set; }
        public string StrUrl { get; set; }
    }
}
