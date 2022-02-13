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
    public interface IDanhGiaLandTourRepository : IRepository<DanhGiaLandtour>
    {
    }

    public class DanhGiaLandTourRepository : Repository<DanhGiaLandtour>, IDanhGiaLandTourRepository
    {
        public DanhGiaLandTourRepository(qltaikhoanContext context) : base(context)
        {
        }
    }
}