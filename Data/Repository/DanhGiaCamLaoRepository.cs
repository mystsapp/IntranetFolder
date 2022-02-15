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
    public interface IDanhGiaCamLaoRepository : IRepository<DanhGiaCamLao>
    {
    }

    public class DanhGiaCamLaoRepository : Repository<DanhGiaCamLao>, IDanhGiaCamLaoRepository
    {
        public DanhGiaCamLaoRepository(qltaikhoanContext context) : base(context)
        {
        }
    }
}