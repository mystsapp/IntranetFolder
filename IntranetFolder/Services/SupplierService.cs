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

        Task<Supplier> GetByIdAsync(string id);
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

        public async Task<Supplier> GetByIdAsync(string id)
        {
            return await _unitOfWork.supplierRepository.GetByIdAsync(id);
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
                var suppliers = await _unitOfWork.supplierRepository.FindAsync(x => x.Code.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.Tengiaodich) && x.Tengiaodich.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.Tinhtp) && x.Tinhtp.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.Tenthuongmai) && x.Tenthuongmai.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.Masothue) && x.Masothue.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.Tapdoan) && x.Tapdoan.ToLower().Contains(searchString.ToLower())));
                suppliers1 = suppliers.ToList();
            }
            else
            {
                suppliers1 = _unitOfWork.supplierRepository.GetAll().ToList();

                if (suppliers1 == null)
                {
                    return null;
                }
            }

            suppliers1 = suppliers1.OrderByDescending(x => x.Ngaytao).ToList();

            list = _mapper.Map<List<Supplier>, List<SupplierDTO>>(suppliers1);

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

        public async Task CreateAsync(SupplierDTO supplierDTO)
        {
            Supplier supplier = _mapper.Map<SupplierDTO, Supplier>(supplierDTO);
            var = _unitOfWork.supplierRepository.Create(supplier);
            await _unitOfWork.Complete();
        }
    }
}