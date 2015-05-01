using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.Repositories
{
    public class SupplierCautionMoneyDeductionRepository : BaseRepository<SupplierCautionMoneyDeduction>, ISupplierCautionMoneyDeductionRepository
    {
        public IList<Domain.UIObjects.UISupplierCautionMoneyDeduction> GetUIList(Domain.UISearchObjects.UISearchSupplierCautionMoneyDeduction uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierCautionMoneyDeduction> uiCompanyList = new List<UISupplierCautionMoneyDeduction>();

            int total = 0;

            IQueryable<SupplierCautionMoneyDeduction> query = null;

            List<Expression<Func<SupplierCautionMoneyDeduction, bool>>> whereFuncs = new List<Expression<Func<SupplierCautionMoneyDeduction, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SupplierCautionMoneyID > 0)
                    whereFuncs.Add(x => x.SupplierCautionMoneyID.Equals(uiSearchObj.SupplierCautionMoneyID));

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
                                 select new UISupplierCautionMoneyDeduction()
                                 {
                                     ID = q.ID,
                                     SupplierCautionMoneyID = q.SupplierCautionMoneyID,
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
