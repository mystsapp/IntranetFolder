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
    public interface IDanhGiaDTQRepository : IRepository<DanhGiaDiemThamQuan>
    {
    }

    public class DanhGiaDTQRepository : Repository<DanhGiaDiemThamQuan>, IDanhGiaDTQRepository
    {
        public DanhGiaDTQRepository(qltaikhoanContext context) : base(context)
        {
        }
    }
}