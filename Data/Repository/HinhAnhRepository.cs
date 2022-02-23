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
    public interface IHinhAnhRepository : IRepository<HinhAnh>
    {
        Task DeleteRangeAsync(List<HinhAnh> hinhAnhs);
    }

    public class HinhAnhRepository : Repository<HinhAnh>, IHinhAnhRepository
    {
        public HinhAnhRepository(qltaikhoanContext context) : base(context)
        {
        }

        public async Task DeleteRangeAsync(List<HinhAnh> hinhAnhs)
        {
            _context.RemoveRange(hinhAnhs);
        }
    }
}