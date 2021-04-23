using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class Application
    {
        public Application()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
        }

        public string Mact { get; set; }
        public string Chuongtrinh { get; set; }
        public string Link { get; set; }
        public string Mota { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
