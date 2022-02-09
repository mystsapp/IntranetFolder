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
    public interface IErrorRepository : IRepository<ErrorLog>
    {
    }

    public class ErrorRepository : Repository<ErrorLog>, IErrorRepository
    {
        public ErrorRepository(qltaikhoanContext context) : base(context)
        {
        }
    }
}