using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class SupplierContractRepository : BaseRepository<SupplierContract>, ISupplierContractRepository
    {

        public IList<UISupplierContract> GetUIList(UISearchSupplierContract uiSearchObj = null)
        {
            IList<UISupplierContract> uiContracts = new List<UISupplierContract>();

            IQueryable<SupplierContract> query = null;

            List<Expression<Func<SupplierContract, bool>>> whereFuncs = new List<Expression<Func<SupplierContract, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.SupplierID > 0)
                    whereFuncs.Add(x => x.SupplierID == uiSearchObj.SupplierID);

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);

                if (!string.IsNullOrEmpty(uiSearchObj.ContractCode))
                    whereFuncs.Add(x => x.ContractCode.Contains(uiSearchObj.ContractCode));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiContracts = (from q in query
                               join s in DB.Supplier on q.SupplierID equals s.ID
                               join p in DB.Product on q.ProductID equals p.ID into tempP
                               from tp in tempP.DefaultIfEmpty()
                               join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID into tempPS
                               from tps in tempPS.DefaultIfEmpty()
                               select new UISupplierContract()
                               {
                                   ID = q.ID,
                                   ContractCode = q.ContractCode,
                                   SupplierName = s.SupplierName,
                                   UnitPrice = q.UnitPrice,
                                   ExpirationDate = q.ExpirationDate,
                                   ProductName = tp == null ? string.Empty : tp.ProductName,
                                   ProductSpecification = tps == null ? string.Empty : tps.Specification
                               }).ToList();
            }

            return uiContracts;
        }

        public IList<UISupplierContract> GetUIList(UISearchSupplierContract uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierContract> uiContracts = new List<UISupplierContract>();

            int total;

            IQueryable<SupplierContract> query = null;

            List<Expression<Func<SupplierContract, bool>>> whereFuncs = new List<Expression<Func<SupplierContract, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.SupplierID > 0)
                    whereFuncs.Add(x => x.SupplierID == uiSearchObj.SupplierID);

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);

                if (!string.IsNullOrEmpty(uiSearchObj.ContractCode))
                    whereFuncs.Add(x => x.ContractCode.Contains(uiSearchObj.ContractCode));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiContracts = (from q in query
                               join s in DB.Supplier on q.SupplierID equals s.ID
                               join p in DB.Product on q.ProductID equals p.ID into tempP
                               from tp in tempP.DefaultIfEmpty()
                               join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID into tempPS
                               from tps in tempPS.DefaultIfEmpty()
                               select new UISupplierContract()
                               {
                                   ID = q.ID,
                                   ContractCode = q.ContractCode,
                                   SupplierName = s.SupplierName,
                                   UnitPrice = q.UnitPrice,
                                   ExpirationDate = q.ExpirationDate,
                                   ProductName = tp == null ? string.Empty : tp.ProductName,
                                   ProductSpecification = tps == null ? string.Empty : tps.Specification
                               }).ToList();
            }

            totalRecords = total;

            return uiContracts;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.SupplierContract.Count() > 0)
                return this.DB.SupplierContract.Max(x => x.ID);
            else
                return null;
        }
    }
}
