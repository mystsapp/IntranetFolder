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
    public interface IDanhGiaNhaCungUngRepository : IRepository<DanhGiaNcu>
    {
    }

    public class DanhGiaNhaCungUngRepository : Repository<DanhGiaNcu>, IDanhGiaNhaCungUngRepository
    {
        public DanhGiaNhaCungUngRepository(qltaikhoanContext context) : base(context)
        {
        }
    }
}