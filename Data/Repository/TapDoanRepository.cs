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
    public interface ITapDoanRepository : IRepository<TapDoan>
    {
    }

    public class TapDoanRepository : Repository<TapDoan>, ITapDoanRepository
    {
        public TapDoanRepository(qltaikhoanContext context) : base(context)
        {
        }
    }
}