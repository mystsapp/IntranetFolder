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
    public interface ITinhRepository : IRepository<Tinh>
    {
        Task<List<VTinh>> GetVTinhDTOs();

        Task<IEnumerable<Thanhpho1>> GetThanhpho1s();
    }

    public class TinhRepository : Repository<Tinh>, ITinhRepository
    {
        public TinhRepository(qltaikhoanContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Thanhpho1>> GetThanhpho1s()
        {
            return await _context.Thanhpho1s.ToListAsync();
        }

        public async Task<List<VTinh>> GetVTinhDTOs()
        {
            return await _context.VTinhs.ToListAsync();
        }
    }
}