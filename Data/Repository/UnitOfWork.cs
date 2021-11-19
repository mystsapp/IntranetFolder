using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository userRepository { get; }
        IFolderUserReprository folderUserReprository { get; }
        ISupplierRepository supplierRepository { get; }

        Task<int> Complete();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly qltaikhoanContext _context;

        public UnitOfWork(qltaikhoanContext context)
        {
            _context = context;
            userRepository = new UserRepository(_context);
            folderUserReprository = new FolderUserReprository(_context);
            supplierRepository = new SupplierRepository(_context);
        }

        public IUserRepository userRepository { get; }

        public IFolderUserReprository folderUserReprository { get; }

        public ISupplierRepository supplierRepository { get; }

        public async Task<int> Complete()
        {
            await _context.SaveChangesAsync();
            return 1;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.Collect();
        }
    }
}