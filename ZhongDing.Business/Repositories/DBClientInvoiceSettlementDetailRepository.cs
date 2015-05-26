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
    public class DBClientInvoiceSettlementDetailRepository : BaseRepository<DBClientInvoiceSettlementDetail>, IDBClientInvoiceSettlementDetailRepository
    {
        public IList<UIDBClientInvoiceSettlementDetail> GetUIList(UISearchDBClientInvoiceSettlementDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDBClientInvoiceSettlementDetail> uiEntities = new List<UIDBClientInvoiceSettlementDetail>();

            int total = 0;

            IQueryable<DBClientInvoiceSettlementDetail> query = null;

            var whereFuncs = new List<Expression<Func<DBClientInvoiceSettlementDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DBClientInvoiceSettlementID > 0)
                    whereFuncs.Add(x => x.DBClientInvoiceSettlementID == uiSearchObj.DBClientInvoiceSettlementID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join dbcis in DB.DBClientInvoiceSettlement on q.DBClientInvoiceSettlementID equals dbcis.ID
                              join c in DB.Company on dbcis.CompanyID equals c.ID
                              join dc in DB.DistributionCompany on dbcis.DistributionCompanyID equals dc.ID
                              select new UIDBClientInvoiceSettlementDetail()
                              {
                                  ID = q.ID,
                                  DBClientInvoiceID = q.DBClientInvoiceID,
                                  CompanyName = c.CompanyName,
                                  DistributionCompanyName = dc.Name,
                                  InvoiceDate = q.InvoiceDate,
                                  InvoiceNumber = q.InvoiceNumber,
                                  InvoiceAmount = q.InvoiceAmount,
                                  ReceivedAmount = DB.DBClientInvoiceSettlementDetail.Any(x => x.IsDeleted == false
                                      && x.DBClientInvoiceSettlement.IsCanceled == false && x.DBClientInvoiceID == q.DBClientInvoiceID)
                                  ? DB.DBClientInvoiceSettlementDetail.Where(x => x.IsDeleted == false
                                      && x.DBClientInvoiceSettlement.IsCanceled == false && x.DBClientInvoiceID == q.DBClientInvoiceID)
                                      .Sum(x => x.ReceiveAmount)
                                  : 0M,
                                  CurrentReceiveAmount = q.ReceiveAmount
                              }).ToList();

                foreach (var uiEntity in uiEntities)
                {
                    uiEntity.ToBeReceiveAmount = uiEntity.InvoiceAmount - uiEntity.ReceivedAmount;
                }
            }

            totalRecords = total;

            return uiEntities;
        }


        public IList<UIDBClientInvoiceSettlementDetail> GetNeedSettleUIList(UISearchDBClientInvoiceSettlementDetail uiSearchObj)
        {
            IList<UIDBClientInvoiceSettlementDetail> uiEntities = new List<UIDBClientInvoiceSettlementDetail>();

            //未结算的发票
            var query = DB.DBClientInvoice.Where(x => x.IsDeleted == false && x.IsSettled != true
                && ((x.DBClientInvoiceSettlementDetail.Any(y => y.IsDeleted == false
                    && y.DBClientInvoiceSettlement.IsCanceled == false) 
                    ? x.DBClientInvoiceSettlementDetail.Where(y => y.IsDeleted == false
                    && y.DBClientInvoiceSettlement.IsCanceled == false)
                    .Sum(y => y.ReceiveAmount) : 0) < x.Amount));

            var whereFuncs = new List<Expression<Func<DBClientInvoice, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                whereFuncs.Add(x => x.DistributionCompanyID == uiSearchObj.DistributionCompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.InvoiceNumber))
                    whereFuncs.Add(x => x.InvoiceNumber.Contains(uiSearchObj.InvoiceNumber));

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

            uiEntities = (from q in query.OrderByDescending(x => x.InvoiceDate)
                          join c in DB.Company on q.CompanyID equals c.ID
                          join dc in DB.DistributionCompany on q.DistributionCompanyID equals dc.ID
                          select new UIDBClientInvoiceSettlementDetail
                          {
                              ID = 0,
                              DBClientInvoiceID = q.ID,
                              CompanyName = c.CompanyName,
                              DistributionCompanyName = dc.Name,
                              InvoiceDate = q.InvoiceDate,
                              InvoiceNumber = q.InvoiceNumber,
                              InvoiceAmount = q.Amount,
                              ReceivedAmount = q.DBClientInvoiceSettlementDetail.Any(y => y.IsDeleted == false
                                  && y.DBClientInvoiceSettlement.IsCanceled == false && y.DBClientInvoiceID == q.ID)
                                  ? q.DBClientInvoiceSettlementDetail.Where(y => y.IsDeleted == false
                                  && y.DBClientInvoiceSettlement.IsCanceled == false && y.DBClientInvoiceID == q.ID)
                                  .Sum(y => y.ReceiveAmount)
                                  : 0,
                          }).ToList();


            foreach (var uiEntity in uiEntities)
            {
                uiEntity.ToBeReceiveAmount = uiEntity.InvoiceAmount - uiEntity.ReceivedAmount;
            }

            return uiEntities;
        }
    }
}
