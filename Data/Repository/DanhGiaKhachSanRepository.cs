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
    public interface IDanhGiaKhachSanRepository : IRepository<DanhGiaKhachSan>
    {
    }

    public class DanhGiaKhachSanRepository : Repository<DanhGiaKhachSan>, IDanhGiaKhachSanRepository
    {
        public DanhGiaKhachSanRepository(qltaikhoanContext context) : base(context)
        {
        }
    }
}