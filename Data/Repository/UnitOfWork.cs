using Data.Models;
using Data.Models_QLTour;
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
        ITinhRepository tinhRepository { get; }
        IThanhPho1Repository thanhPho1Repository { get; }
        IDanhGiaNhaCungUngRepository danhGiaNhaCungUngRepository { get; }
        ILoaiDvRepository loaiDvRepository { get; }
        IDanhGiaNhaHangRepository danhGiaNhaHangRepository { get; }
        IDanhGiaKhachSanRepository danhGiaKhachSanRepository { get; }
        IDanhGiaLandTourRepository danhGiaLandTourRepository { get; }
        IDanhGiaDTQRepository danhGiaDTQRepository { get; }
        IDanhGiaCamLaoRepository danhGiaCamLaoRepository { get; }
        IDanhGiaVanChuyenRepository danhGiaVanChuyenRepository { get; }
        IDichVu1Repository dichVu1Repository { get; }
        IErrorRepository errorRepository { get; }

        // qltour
        IDmdiemtqRepository dmdiemtqRepository { get; }

        Task<int> Complete();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly qltaikhoanContext _context;
        private readonly qltourContext _qltourContext;

        public UnitOfWork(qltaikhoanContext context, qltourContext qltourContext)
        {
            _context = context;
            _qltourContext = qltourContext;

            userRepository = new UserRepository(_context);
            folderUserReprository = new FolderUserReprository(_context);
            supplierRepository = new SupplierRepository(_context);
            tinhRepository = new TinhRepository(_context);
            thanhPho1Repository = new ThanhPho1Repository(_context);
            danhGiaNhaCungUngRepository = new DanhGiaNhaCungUngRepository(_context);
            loaiDvRepository = new LoaiDvRepository(_context);
            danhGiaNhaHangRepository = new DanhGiaNhaHangRepository(_context);
            danhGiaKhachSanRepository = new DanhGiaKhachSanRepository(_context);
            danhGiaLandTourRepository = new DanhGiaLandTourRepository(_context);
            danhGiaDTQRepository = new DanhGiaDTQRepository(_context);
            danhGiaCamLaoRepository = new DanhGiaCamLaoRepository(_context);
            danhGiaVanChuyenRepository = new DanhGiaVanChuyenRepository(_context);
            dichVu1Repository = new DichVu1Repository(_context);
            errorRepository = new ErrorRepository(_context);

            // qltour
            dmdiemtqRepository = new DmdiemtqRepository(_qltourContext);
        }

        public IUserRepository userRepository { get; }

        public IFolderUserReprository folderUserReprository { get; }

        public ISupplierRepository supplierRepository { get; }

        public ITinhRepository tinhRepository { get; }

        public IThanhPho1Repository thanhPho1Repository { get; }

        public IDmdiemtqRepository dmdiemtqRepository { get; }

        public IDanhGiaNhaCungUngRepository danhGiaNhaCungUngRepository { get; }

        public ILoaiDvRepository loaiDvRepository { get; }

        public IDanhGiaNhaHangRepository danhGiaNhaHangRepository { get; }

        public IErrorRepository errorRepository { get; }

        public IDanhGiaKhachSanRepository danhGiaKhachSanRepository { get; }

        public IDanhGiaLandTourRepository danhGiaLandTourRepository { get; }

        public IDanhGiaDTQRepository danhGiaDTQRepository { get; }

        public IDanhGiaCamLaoRepository danhGiaCamLaoRepository { get; }

        public IDanhGiaVanChuyenRepository danhGiaVanChuyenRepository { get; }

        public IDichVu1Repository dichVu1Repository { get; }

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