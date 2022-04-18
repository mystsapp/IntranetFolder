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
    public interface IDichVu1Repository : IRepository<DichVu1>
    {
        Task<IEnumerable<DichVu1>> FinTwoSideAsync(Expression<Func<DichVu1, object>> supplier, Expression<Func<DichVu1, object>> hinhAnh, Expression<Func<DichVu1, bool>> expression);

        Task<IEnumerable<DichVu1>> FinIncludeTwoAsync(Expression<Func<DichVu1, object>> supplier, Expression<Func<DichVu1, object>> loaiDv, Expression<Func<DichVu1, bool>> expression);

        Task UpdateRange(List<DichVu1> dichVu1s);
    }

    public class DichVu1Repository : Repository<DichVu1>, IDichVu1Repository
    {
        public DichVu1Repository(qltaikhoanContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DichVu1>> FinIncludeTwoAsync(Expression<Func<DichVu1, object>> supplier, Expression<Func<DichVu1, object>> loaiDv, Expression<Func<DichVu1, bool>> expression)
        {
            return await _context.DichVus1.Include(x => x.Supplier).Include(x => x.LoaiDv).Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<DichVu1>> FinTwoSideAsync(Expression<Func<DichVu1, object>> supplier, Expression<Func<DichVu1, object>> hinhAnh, Expression<Func<DichVu1, bool>> expression)
        {
            return await _context.DichVus1.Include(x => x.Supplier).Include(x => x.HinhAnhs).Where(expression).ToListAsync();
        }

        public async Task UpdateRange(List<DichVu1> dichVu1s)
        {
            _context.DichVus1.UpdateRange(dichVu1s);
            await _context.SaveChangesAsync();
        }
    }
}