using Data.Interfaces;
using Data.Models;
using Data.Models_QLTour;
using Data.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface ISupplier_QLTourRepository
    {
        List<CodeSupplier> listCapcode();

        Tinh getTinhById(string matinh);

        CodeSupplier getCodeSupplierById(decimal id);

        string NextId();

        IEnumerable<Thanhpho1> ListThanhphoByTinh(string matinh);

        int updateCapCodeSupplier(decimal id);

        int huyCapcodeSupplier(CodeSupplier code);
    }

    public class Supplier_QLTourRepository : ISupplier_QLTourRepository
    {
        private readonly qltourContext _context;
        private readonly qltaikhoanContext _qltaikhoanContext;

        public Supplier_QLTourRepository(qltourContext context, qltaikhoanContext qltaikhoanContext)
        {
            _context = context;
            _qltaikhoanContext = qltaikhoanContext;
        }

        public List<CodeSupplier> listCapcode()
        {
            try
            {
                var codeSuppliers = _context.CodeSuppliers.FromSqlRaw("select * ,'' Code from qltour.dbo.codesupplier where daduyet=0").ToList();
                return codeSuppliers;
            }
            catch
            {
                return null;
            }
        }

        public Tinh getTinhById(string matinh)
        {
            var parammeter = new SqlParameter[]
            {
                new SqlParameter("@matinh",matinh)
            };
            var tinhs = _qltaikhoanContext.Tinhs.FromSqlRaw("qltaikhoan.dbo.spGetTinhById @matinh", parammeter).AsEnumerable();
            return tinhs.FirstOrDefault();
        }

        public CodeSupplier getCodeSupplierById(decimal id)
        {
            var parammeter = new SqlParameter[]
            {
                new SqlParameter("@id",id)
            };
            var codeSuppliers = _context.CodeSuppliers.FromSqlRaw("spGetCapcodeSupplierById @id", parammeter).AsEnumerable();
            return codeSuppliers.FirstOrDefault();
        }

        public string NextId()
        {
            return new GenerateId().NextId(lastId(), "", "00001");
        }

        private string lastId()
        {
            var lastCode = _qltaikhoanContext.Suppliers.OrderByDescending(x => x.Code).Take(1).FirstOrDefault().Code;
            return lastCode;
        }

        public class GenerateId
        {
            public string NextId(string lastID, string prefixID, string length)
            {
                if (lastID == "")
                {
                    return prefixID + length;
                }
                int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
                int lengthNumerID = lastID.Length - prefixID.Length;
                string zeroNumber = "";
                for (int i = 1; i <= lengthNumerID; i++)
                {
                    if (nextID < Math.Pow(10, i))
                    {
                        for (int j = 1; j <= lengthNumerID - i; i++)
                        {
                            zeroNumber += "0";
                        }
                        return prefixID + zeroNumber + nextID.ToString();
                    }
                }
                return prefixID + nextID;
            }
        }

        public IEnumerable<Thanhpho1> ListThanhphoByTinh(string matinh)
        {
            return _qltaikhoanContext.Thanhpho1s.Where(x => x.Matinh == matinh);
        }

        public int updateCapCodeSupplier(decimal id)
        {
            var parammeter = new SqlParameter[]
             {
                    new SqlParameter("@id",id)
             };
            try
            {
                return _qltaikhoanContext.Database.ExecuteSqlRaw("spUpdateCapCodeSupplier @id", parammeter);
            }
            catch
            {
                return 0;
            }
        }

        public int huyCapcodeSupplier(CodeSupplier code)
        {
            var parammeter = new SqlParameter[]
            {
                    new SqlParameter("@id",code.Id),
                    new SqlParameter("@lydo",code.Lydo)
            };
            try
            {
                return _context.Database.ExecuteSqlRaw("spHuyCapcodeSupplier @id, @lydo", parammeter);
            }
            catch
            {
                throw;
                // return 0;
            }
        }
    }
}