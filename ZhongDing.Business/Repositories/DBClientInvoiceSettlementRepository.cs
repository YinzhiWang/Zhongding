using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class DBClientInvoiceSettlementRepository : BaseRepository<DBClientInvoiceSettlement>, IDBClientInvoiceSettlementRepository
    {
        public IList<UIDBClientInvoiceSettlement> GetUIList(UISearchDBClientInvoiceSettlement uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDBClientInvoiceSettlement> uiEntities = new List<UIDBClientInvoiceSettlement>();

            int total = 0;

            IQueryable<DBClientInvoiceSettlement> query = null;

            var whereFuncs = new List<Expression<Func<DBClientInvoiceSettlement, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.ExcludeCanceled)
                    whereFuncs.Add(x => x.IsCanceled == false);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.ReceiveDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.ReceiveDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join c in DB.Company on q.CompanyID equals c.ID
                              join dc in DB.DistributionCompany on q.DistributionCompanyID equals dc.ID
                              select new UIDBClientInvoiceSettlement()
                              {
                                  ID = q.ID,
                                  ReceiveDate = q.ReceiveDate,
                                  CompanyName = c.CompanyName,
                                  DistributionCompanyName = dc.Name,
                                  InvoiceNumberArray = q.DBClientInvoiceSettlementDetail
                                  .Where(x => x.IsDeleted == false).Select(x => x.InvoiceNumber),
                                  TotalReceiveAmount = q.TotalReceiveAmount,
                                  ConfirmDate = q.ConfirmDate
                              }).ToList();

                foreach (var uiEntity in uiEntities)
                {
                    uiEntity.InvoiceNumbers = string.Join(", ", uiEntity.InvoiceNumberArray);
                }
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
