using Data.Interfaces;
using Data.Models;
using Data.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IDanhGiaGolfRepository : IRepository<DanhGiaGolf>
    {
    }

    public class DanhGiaGolfRepository : Repository<DanhGiaGolf>, IDanhGiaGolfRepository
    {
        public DanhGiaGolfRepository(qltaikhoanContext context) : base(context)
        {
        }
    }
}