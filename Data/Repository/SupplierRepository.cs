using Data.Interfaces;
using Data.Models;
using Data.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<IEnumerable<VTinh>> GetTinhs();

        Task<IEnumerable<Thanhpho1>> GetThanhpho1s();

        Task<IEnumerable<Quocgium>> GetQuocgias();

        Task<IEnumerable<Supplier>> FindIncludeTwoAsync(Expression<Func<Supplier, object>> expressObj, Expression<Func<Supplier, object>> expressObj2, Expression<Func<Supplier, bool>> expression);
    }

    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(qltaikhoanContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Quocgium>> GetQuocgias()
        {
            return await _context.Quocgia.ToListAsync();
        }

        public async Task<IEnumerable<Thanhpho1>> GetThanhpho1s()
        {
            return await _context.Thanhpho1s.ToListAsync();
        }

        public async Task<IEnumerable<VTinh>> GetTinhs()
        {
            return await _context.VTinhs.ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> FindIncludeTwoAsync(Expression<Func<Supplier, object>> expressObj, Expression<Func<Supplier, object>> expressObj2, Expression<Func<Supplier, bool>> expression)
        {
            return await _context.Set<Supplier>().Include(expressObj).Include(expressObj2).Where(expression).ToListAsync();
        }
    }
}