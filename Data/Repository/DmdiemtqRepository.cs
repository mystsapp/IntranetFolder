using Data.Interfaces;
using Data.Models;
using Data.Models_QLTour;
using Data.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IDmdiemtqRepository : IRepository<Dmdiemtq>
    {
    }

    public class DmdiemtqRepository : Repository_QLTour<Dmdiemtq>, IDmdiemtqRepository
    {
        public DmdiemtqRepository(qltourContext context) : base(context)
        {
        }
    }
}