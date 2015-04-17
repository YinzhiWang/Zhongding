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
    public class SupplierInvoiceSettlementDetailRepository : BaseRepository<SupplierInvoiceSettlementDetail>, ISupplierInvoiceSettlementDetailRepository
    {
        public IList<UISupplierInvoiceSettlementDetail> GetUIList(UISearchSupplierInvoiceSettlementDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierInvoiceSettlementDetail> uiEntities = new List<UISupplierInvoiceSettlementDetail>();

            int total = 0;

            IQueryable<SupplierInvoiceSettlementDetail> query = null;

            var whereFuncs = new List<Expression<Func<SupplierInvoiceSettlementDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SupplierInvoiceSettlementID > 0)
                    whereFuncs.Add(x => x.SupplierInvoiceSettlementID == uiSearchObj.SupplierInvoiceSettlementID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join s in DB.Supplier on q.SupplierID equals s.ID
                              select new UISupplierInvoiceSettlementDetail()
                              {
                                  ID = q.ID,
                                  InvoiceDate = q.InvoiceDate,
                                  InvoiceNumber = q.InvoiceNumber,
                                  InvoiceAmount = q.InvoiceAmount,
                                  PayAmount = q.PayAmount,
                                  SupplierName = s.SupplierName

                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }


        public IList<UISupplierInvoiceSettlementDetail> GetNeedSettleUIList(UISearchSupplierInvoiceSettlementDetail uiSearchObj)
        {
            IList<UISupplierInvoiceSettlementDetail> uiEntities = new List<UISupplierInvoiceSettlementDetail>();

            //未结算的发票
            var query = DB.SupplierInvoice.Where(x => x.IsDeleted == false && x.IsSettled != true);

            var whereFuncs = new List<Expression<Func<SupplierInvoice, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.InvoiceDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.InvoiceDate < uiSearchObj.EndDate);
                }
            }

            foreach (var func in whereFuncs)
            {
                query = query.Where(func);
            }

            uiEntities = (from q in query.OrderByDescending(x=>x.InvoiceDate)
                          join s in DB.Supplier on q.SupplierID equals s.ID
                          join c in DB.Company on q.CompanyID equals c.ID
                          select new UISupplierInvoiceSettlementDetail
                          {
                              SupplierID = q.SupplierID,
                              SupplierInvoiceID = q.ID,
                              SupplierName = s.SupplierName,
                              InvoiceDate = q.InvoiceDate,
                              InvoiceNumber = q.InvoiceNumber,
                              InvoiceAmount = q.Amount,
                              PayAmount = q.Amount * c.ProviderTexRatio ?? 0
                          }).ToList();

            return uiEntities;
        }
    }
}
