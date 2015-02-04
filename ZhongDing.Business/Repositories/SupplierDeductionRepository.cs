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
    public class SupplierDeductionRepository : BaseRepository<SupplierDeduction>, ISupplierDeductionRepository
    {
        public IList<UISupplierDeduction> GetUIList(UISearchSupplierDeduction uiSearchObj = null)
        {
            IList<UISupplierDeduction> uiCompanyList = new List<UISupplierDeduction>();

            IQueryable<SupplierDeduction> query = null;

            List<Expression<Func<SupplierDeduction, bool>>> whereFuncs = new List<Expression<Func<SupplierDeduction, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SupplierRefundAppID > 0)
                    whereFuncs.Add(x => x.SupplierRefundAppID.Equals(uiSearchObj.SupplierRefundAppID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.DeductedDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.DeductedDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiCompanyList = (from q in query
                                 join s in DB.Supplier on q.SupplierID equals s.ID
                                 join cu in this.DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                 from tcu in tempCU.DefaultIfEmpty()
                                 select new UISupplierDeduction()
                                 {
                                     ID = q.ID,
                                     SupplierRefundAppID = q.SupplierRefundAppID,
                                     SupplierID = q.SupplierID,
                                     SupplierName = s.SupplierName,
                                     DeductedDate = q.DeductedDate,
                                     Amount = q.Amount,
                                     Comment = q.Comment,
                                     CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                 }).ToList();

            }

            return uiCompanyList;
        }

        public IList<UISupplierDeduction> GetUIList(UISearchSupplierDeduction uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierDeduction> uiCompanyList = new List<UISupplierDeduction>();

            int total = 0;

            IQueryable<SupplierDeduction> query = null;

            List<Expression<Func<SupplierDeduction, bool>>> whereFuncs = new List<Expression<Func<SupplierDeduction, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SupplierRefundAppID > 0)
                    whereFuncs.Add(x => x.SupplierRefundAppID.Equals(uiSearchObj.SupplierRefundAppID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.DeductedDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.DeductedDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiCompanyList = (from q in query
                                 join s in DB.Supplier on q.SupplierID equals s.ID
                                 join cu in this.DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                 from tcu in tempCU.DefaultIfEmpty()
                                 select new UISupplierDeduction()
                                 {
                                     ID = q.ID,
                                     SupplierRefundAppID = q.SupplierRefundAppID,
                                     SupplierID = q.SupplierID,
                                     SupplierName = s.SupplierName,
                                     DeductedDate = q.DeductedDate,
                                     Amount = q.Amount,
                                     Comment = q.Comment,
                                     CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                 }).ToList();

            }

            totalRecords = total;

            return uiCompanyList;
        }
    }
}
