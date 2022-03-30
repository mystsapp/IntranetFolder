using AutoMapper;
using Data.Models;
using Data.Repository;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace IntranetFolder.Services
{
    public interface ISupplierService
    {
        Task<IPagedList<SupplierDTO>> ListSupplier(string searchString, string searchFromDate, string searchToDate, int? page);

        Task<SupplierDTO> GetByIdAsync(string id);

        Task<SupplierDTO> CreateAsync(SupplierDTO supplierDTO);

        Task<SupplierDTO> UpdateAsync(SupplierDTO supplierDTO);

        Task Delete(SupplierDTO supplierDTO);

        string GetNextId(string param, string macn);

        Task<IEnumerable<VTinh>> GetTinhs();

        Task<IEnumerable<Thanhpho1>> GetThanhpho1s();

        Task<IEnumerable<Quocgium>> GetQuocgias();

        SupplierDTO GetByIdAsNoTracking(string id);

        Task<IEnumerable<DanhGiaNhaHangDTO>> GetDanhGiaNhaHangBy_SupplierId(string id);

        Task<IEnumerable<SupplierDTO>> FindAsync(string searchString);

        List<Data.Models_QLTour.CodeSupplier> listCapcode();

        Tinh getTinhById(string tinhtp);

        Data.Models_QLTour.CodeSupplier getCodeSupplierById(decimal id);

        string NextId();

        List<Thanhpho1> ListThanhphoByTinh(string matinh);

        IEnumerable<Supplier> GetAll_Supplier();

        Task<Supplier> Create(Supplier s);

        void updateCapCodeSupplier(decimal id);

        int huyCapcodeSupplier(Data.Models_QLTour.CodeSupplier model);

        IEnumerable<TapDoanDTO> GetAll_TapDoan();
    }

    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SupplierDTO> GetByIdAsync(string id)
        {
            return _mapper.Map<Supplier, SupplierDTO>(await _unitOfWork.supplierRepository.GetByIdAsync(id));
        }

        public async Task<IPagedList<SupplierDTO>> ListSupplier(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            List<SupplierDTO> list = new List<SupplierDTO>();
            List<Supplier> suppliers1 = new List<Supplier>();

            if (!string.IsNullOrEmpty(searchString))
            {
                var suppliers = await _unitOfWork.supplierRepository.FindIncludeOneAsync(y => y.TapDoan, x => x.Code.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.Tengiaodich) && x.Tengiaodich.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.Tinhtp) && x.Tinhtp.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.Tenthuongmai) && x.Tenthuongmai.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.Masothue) && x.Masothue.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.Diachi) && x.Diachi.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.TapDoan.Ten) && x.TapDoan.Ten.ToLower().Contains(searchString.ToLower())));
                suppliers1 = suppliers.ToList();
            }
            else
            {
                var suppliers = await _unitOfWork.supplierRepository.GetAllIncludeOneAsync(x => x.TapDoan);
                suppliers1 = suppliers.ToList();

                if (suppliers1 == null)
                {
                    return null;
                }
            }

            suppliers1 = suppliers1.OrderByDescending(x => x.Ngaytao).ToList();
            suppliers1 = suppliers1.OrderByDescending(x => x.KhuyenNghi).ToList();

            list = _mapper.Map<List<Supplier>, List<SupplierDTO>>(suppliers1);
            var vTinhs = await GetTinhs();
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Tinhtp) && item.Tinhtp != "-- Select --")
                {
                    var vTinh = vTinhs.Where(x => x.Matinh == item.Tinhtp).FirstOrDefault();
                    item.TinhtpName = vTinh == null ? "" : vTinh.Tentinh;
                }
            }

            // search date
            DateTime fromDate, toDate;
            if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
            {
                try
                {
                    fromDate = DateTime.Parse(searchFromDate); // NgayCT
                    toDate = DateTime.Parse(searchToDate); // NgayCT

                    if (fromDate > toDate)
                    {
                        return null; //
                    }

                    list = list.Where(x => x.Ngaytao >= fromDate &&
                                       x.Ngaytao < toDate.AddDays(1)).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchFromDate)) // NgayCT
                {
                    try
                    {
                        fromDate = DateTime.Parse(searchFromDate);
                        list = list.Where(x => x.Ngaytao >= fromDate).ToList();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                if (!string.IsNullOrEmpty(searchToDate)) // NgayCT
                {
                    try
                    {
                        toDate = DateTime.Parse(searchToDate);
                        list = list.Where(x => x.Ngaytao < toDate.AddDays(1)).ToList();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            // search date

            //// List<string> listRoleChiNhanh --> chi lay nhung tour thuộc phanKhuCN cua minh
            //if (listRoleChiNhanh.Count > 0)
            //{
            //    list = list.Where(item1 => listRoleChiNhanh.Any(item2 => item1.MaCNTao == item2)).ToList();
            //}
            //// List<string> listRoleChiNhanh --> chi lay nhung tour thuộc phanKhuCN cua minh

            // page the list
            const int pageSize = 10;
            decimal aa = (decimal)list.Count() / (decimal)pageSize;
            var bb = Math.Ceiling(aa);
            if (page > bb)
            {
                page--;
            }
            page = (page == 0) ? 1 : page;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);
            //if (page > listPaged.PageCount)
            //    page--;
            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        public async Task<SupplierDTO> CreateAsync(SupplierDTO supplierDTO)
        {
            Supplier supplier = _mapper.Map<SupplierDTO, Supplier>(supplierDTO);
            var supplier1 = await _unitOfWork.supplierRepository.CreateAsync(supplier);
            return _mapper.Map<Supplier, SupplierDTO>(supplier1);
        }

        public async Task<SupplierDTO> UpdateAsync(SupplierDTO supplierDTO)
        {
            Supplier supplier = _mapper.Map<SupplierDTO, Supplier>(supplierDTO);
            var supplier1 = await _unitOfWork.supplierRepository.UpdateAsync(supplier);
            return _mapper.Map<Supplier, SupplierDTO>(supplier1);
        }

        public async Task Delete(SupplierDTO supplierDTO)
        {
            Supplier supplier = _mapper.Map<SupplierDTO, Supplier>(supplierDTO);
            _unitOfWork.supplierRepository.Delete(supplier);
            await _unitOfWork.Complete();
        }

        public string GetNextId(string param, string macn)
        {
            var supplier = _unitOfWork.supplierRepository.GetAll().OrderByDescending(x => x.Code).FirstOrDefault();
            if (supplier == null || string.IsNullOrEmpty(supplier.Code))
            {
                return Data.Utilities.GetNextId.NextID("", ""); // 0001
            }
            else
            {
                return Data.Utilities.GetNextId.NextID(supplier.Code, "");
            }
        }

        public async Task<IEnumerable<VTinh>> GetTinhs()
        {
            return await _unitOfWork.supplierRepository.GetTinhs();
        }

        public async Task<IEnumerable<Thanhpho1>> GetThanhpho1s()
        {
            return await _unitOfWork.supplierRepository.GetThanhpho1s();
        }

        public async Task<IEnumerable<Quocgium>> GetQuocgias()
        {
            return await _unitOfWork.supplierRepository.GetQuocgias();
        }

        public SupplierDTO GetByIdAsNoTracking(string id)
        {
            return _mapper.Map<Supplier, SupplierDTO>(_unitOfWork.supplierRepository.GetByIdAsNoTracking(x => x.Code == id));
        }

        public async Task<IEnumerable<DanhGiaNhaHangDTO>> GetDanhGiaNhaHangBy_SupplierId(string id)
        {
            var danhGiaNhaHangs = await _unitOfWork.danhGiaNhaHangRepository.FindIncludeOneAsync(x => x.Supplier, y => y.SupplierId == id);
            return _mapper.Map<IEnumerable<DanhGiaNhaHang>, IEnumerable<DanhGiaNhaHangDTO>>
                (danhGiaNhaHangs);
        }

        public async Task<IEnumerable<SupplierDTO>> FindAsync(string searchString)
        {
            var suppliers = new List<Supplier>();
            if (!string.IsNullOrEmpty(searchString))
            {
                var suppliers1 = await _unitOfWork.supplierRepository.FindIncludeOneAsync(y => y.TapDoan, x => x.Code.ToLower().Contains(searchString.Trim().ToLower()) ||
                                            (!string.IsNullOrEmpty(x.Tengiaodich) && x.Tengiaodich.ToLower().Contains(searchString.ToLower())) ||
                                            (!string.IsNullOrEmpty(x.Tinhtp) && x.Tinhtp.ToLower().Contains(searchString.ToLower())) ||
                                            (!string.IsNullOrEmpty(x.Tenthuongmai) && x.Tenthuongmai.ToLower().Contains(searchString.ToLower())) ||
                                            (!string.IsNullOrEmpty(x.Masothue) && x.Masothue.ToLower().Contains(searchString.ToLower())) ||
                                            (!string.IsNullOrEmpty(x.Tapdoan) && x.Tapdoan.ToLower().Contains(searchString.ToLower())));
                suppliers = suppliers1.ToList();
            }
            else
            {
                var suppliers1 = await _unitOfWork.supplierRepository.GetAllIncludeOneAsync(x => x.TapDoan);
                suppliers = suppliers1.ToList();
            }

            return _mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierDTO>>(suppliers);
        }

        public List<Data.Models_QLTour.CodeSupplier> listCapcode()
        {
            return _unitOfWork.supplier_QLTourRepository.listCapcode();
        }

        public Tinh getTinhById(string tinhtp)
        {
            return _unitOfWork.supplier_QLTourRepository.getTinhById(tinhtp);
        }

        public Data.Models_QLTour.CodeSupplier getCodeSupplierById(decimal id)
        {
            return _unitOfWork.supplier_QLTourRepository.getCodeSupplierById(id);
        }

        public string NextId()
        {
            return _unitOfWork.supplier_QLTourRepository.NextId();
        }

        public List<Thanhpho1> ListThanhphoByTinh(string matinh)
        {
            return _unitOfWork.supplier_QLTourRepository.ListThanhphoByTinh(matinh).ToList();
        }

        public IEnumerable<Supplier> GetAll_Supplier()
        {
            return _unitOfWork.supplierRepository.GetAll();
        }

        public async Task<Supplier> Create(Supplier s)
        {
            var supplier = await _unitOfWork.supplierRepository.CreateAsync(s);
            return supplier;
        }

        public void updateCapCodeSupplier(decimal id)
        {
            _unitOfWork.supplier_QLTourRepository.updateCapCodeSupplier(id);
        }

        public int huyCapcodeSupplier(Data.Models_QLTour.CodeSupplier model)
        {
            return _unitOfWork.supplier_QLTourRepository.huyCapcodeSupplier(model);
        }

        public IEnumerable<TapDoanDTO> GetAll_TapDoan()
        {
            return _mapper.Map<IEnumerable<TapDoan>, IEnumerable<TapDoanDTO>>
                (_unitOfWork.tapDoanRepository.GetAll());
        }
    }
}