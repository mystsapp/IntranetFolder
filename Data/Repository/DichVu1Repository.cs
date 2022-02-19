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
    public interface IDichVu1Repository : IRepository<DichVu1>
    {
    }

    public class DichVu1Repository : Repository<DichVu1>, IDichVu1Repository
    {
        public DichVu1Repository(qltaikhoanContext context) : base(context)
        {
        }
    }
}