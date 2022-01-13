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
    public interface IDanhGiaNhaHangRepository : IRepository<DanhGiaNhaHang>
    {
    }

    public class DanhGiaNhaHangRepository : Repository<DanhGiaNhaHang>, IDanhGiaNhaHangRepository
    {
        public DanhGiaNhaHangRepository(qltaikhoanContext context) : base(context)
        {
        }
    }
}