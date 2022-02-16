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
    public interface IDanhGiaVanChuyenRepository : IRepository<DanhGiaVanChuyen>
    {
    }

    public class DanhGiaVanChuyenRepository : Repository<DanhGiaVanChuyen>, IDanhGiaVanChuyenRepository
    {
        public DanhGiaVanChuyenRepository(qltaikhoanContext context) : base(context)
        {
        }
    }
}