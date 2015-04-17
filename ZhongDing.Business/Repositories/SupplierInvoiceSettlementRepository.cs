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
    public class SupplierInvoiceSettlementRepository : BaseRepository<SupplierInvoiceSettlement>, ISupplierInvoiceSettlementRepository
    {
        public IList<UISupplierInvoiceSettlement> GetUIList(UISearchSupplierInvoiceSettlement uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierInvoiceSettlement> uiEntities = new List<UISupplierInvoiceSettlement>();

            int total = 0;

            IQueryable<SupplierInvoiceSettlement> query = null;

            var whereFuncs = new List<Expression<Func<SupplierInvoiceSettlement, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ExcludeCanceled)
                    whereFuncs.Add(x => x.IsCanceled == false);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.SettlementDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.SettlementDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join c in DB.Company on q.CompanyID equals c.ID
                              select new UISupplierInvoiceSettlement()
                              {
                                  ID = q.ID,
                                  SettlementDate = q.SettlementDate,
                                  TotalInvoiceAmount = q.TotalInvoiceAmount,
                                  TaxRatio = q.TaxRatio,
                                  TotalPayAmount = q.TotalPayAmount,
                                  CompanyName = c.CompanyName,
                                  InvoiceNumberArray = q.SupplierInvoiceSettlementDetail.Select(x => x.InvoiceNumber)

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
