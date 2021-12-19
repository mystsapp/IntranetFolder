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
    public interface IThanhPho1Repository : IRepository<Thanhpho1>
    {
    }

    public class ThanhPho1Repository : Repository<Thanhpho1>, IThanhPho1Repository
    {
        public ThanhPho1Repository(qltaikhoanContext context) : base(context)
        {
        }
    }
}