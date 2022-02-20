using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IVungMienRepository
    {
        Task<IEnumerable<Vungmien>> Vungmiens();
    }

    public class VungMienRepository : IVungMienRepository
    {
        private readonly qltaikhoanContext _context;

        public VungMienRepository(qltaikhoanContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vungmien>> Vungmiens()
        {
            return await _context.Vungmiens.ToListAsync();
        }
    }
}