using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public interface IFolderUserReprository : IRepository<FolderUser>
    {

    }
    public class FolderUserReprository : Repository<FolderUser>, IFolderUserReprository
    {
        public FolderUserReprository(qltaikhoanContext context) : base(context)
        {
        }
    }
}
