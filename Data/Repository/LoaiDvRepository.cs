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
    public interface ILoaiDvRepository : IRepository<LoaiDv>
    {
    }

    public class LoaiDvRepository : Repository<LoaiDv>, ILoaiDvRepository
    {
        public LoaiDvRepository(qltaikhoanContext context) : base(context)
        {
        }
    }
}